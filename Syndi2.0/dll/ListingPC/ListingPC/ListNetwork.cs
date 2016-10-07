using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;

namespace ListingPC
{

    public class Testing
    {
        private ManualResetEvent _doneEvent;
        private int k;
        private string _command;
        private List<string> FullList;
        private static string NameOfPC;
        private static string TypeOfPC;
        
        //Creating a struct to return all the details of the PC.
        public struct DataOfPC
        {
            public string Name;
            public string Type;
        }

        public string Name
        {
            get
            {
                return NameOfPC;
            }
        }

        public string Type
        {
            get
            {
                return TypeOfPC;
            }
        }
        
        public Testing(List<string> _FullList, string command, int a, ManualResetEvent doneEvent)
        {
            FullList = _FullList;
            k = a;
            _doneEvent = doneEvent;
            _command = command;
            NameOfPC = FullList.ToArray()[a];
        }

        public void ThreadPoolCallback(Object o)
        {
            tester(FullList, k, _command);
            _doneEvent.Set();
        }

        public class Globals
        {
            public List<string> PublicPCList;
            public List<string> PasswordProtectedPCList;
            public List<string> NetworkPathErrorPCList;
            public List<DataOfPC> Result;
            public int o;
            public int p;
            public Globals()
            {
                PublicPCList = new List<string>();
                PasswordProtectedPCList = new List<string>();
                NetworkPathErrorPCList = new List<string>();
                Result = new List<DataOfPC>();
                o = 0;
                p = 0;
            }
        }

        public static Globals global = new Globals();

        public static void tester(List<string> FullList, int k, string command)
        {
            try
            {
                var FullListArray = FullList.ToArray();
                string connectionOutput = ViewCommandLineResult(command);
                var NetworkPathErrorRegex = new Regex(@"System error 53 has occurred.");
                var PasswordProtectionRegex = new Regex(@"System error 5 has occurred.");

                bool NetworkError = NetworkPathErrorRegex.IsMatch(connectionOutput);
                bool PasswordProtection = PasswordProtectionRegex.IsMatch(connectionOutput);

                List<bool> output = new List<bool>();
                output.Add(NetworkError);
                output.Add(PasswordProtection);

                var s = output.ToArray();
                if (s[0])
                {
                    global.NetworkPathErrorPCList.Add(FullListArray[k]);
                    TypeOfPC = "Network error";

                }
                else if (s[1])
                {
                    global.PasswordProtectedPCList.Add(FullListArray[k]);
                    global.p += 1;
                    TypeOfPC = "Password";
                    DataOfPC d;
                    d.Name = FullListArray[k];
                    d.Type = "Password";
                    global.Result.Add(d);
                }
                else
                {
                    global.PublicPCList.Add(FullListArray[k]);
                    global.o += 1;
                    TypeOfPC = "Public";
                    DataOfPC d;
                    d.Name = FullListArray[k];
                    d.Type = "Public";
                    global.Result.Add(d);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ignore  " + e);
            }
        }

        public static string ViewCommandLineResult(string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            process.StartInfo = startInfo;
            process.Start();

            /*
             * Reading the standard output from the command prompt. 
             */
            StreamReader sr = process.StandardOutput;
            string outputDump = sr.ReadToEnd();


            /*
             * Reading the standard output from the command prompt. 
             */
            string error = process.StandardError.ReadLine();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                // String is null or empty.
                return error;
            }

            return outputDump;

        }

        public static string timeout(string command)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            string outputDump = ViewCommandLineResult(command);

            Task<string> task = new Task<string>(() => ViewCommandLineResult(command), token);
            task.Start();

            if (task.Wait(TimeSpan.FromSeconds(10)))
            {
                return task.Result;
            }

            else
            {
                return "Error";
            }
        }

    }


    public class ListNetwork
    {

            public static List<Testing.DataOfPC> ScanNetwork(){   
        
            string command = "/C net view /all";
            string outputDump = Testing.ViewCommandLineResult(command);

            var items = Regex.Split(outputDump, @"\\");

            List<string> FullList = new List<string>();
            int i = 2;

            while (i < items.Length)
            {
                /*
                 Seperating the list of PC's from the output string to get only the PC names.
                 */
                var s = items[i];
                /*
                 Sometimes we get the Remark of some PC's , so inorder to avoid them from getting 
                 * into the main list regex is used as space and its seperated.
                 */
                var Name = Regex.Split(s, @" ");
                FullList.Add(Name[0]);
                i = i + 2;
            }
     
            int n = FullList.ToArray().Length;
            List<int> l = new List<int>();
            List<Testing> ans = new List<Testing>();
            int flag;
            for (flag = 0; flag < n; flag += 64)
            {
                l.Add(flag);
            }
            l.Add(n);
            var arr = l.ToArray();
            flag = 0;

            
            while (flag < arr.Length - 1) {    
                ManualResetEvent[] doneEvents = new ManualResetEvent[arr[flag+1]-arr[flag]];
                Testing[] testArray = new Testing[arr[flag + 1] - arr[flag]];

                int counter = 0;
                string NameOfPC;
                var FullListArray = FullList.ToArray();
                
                for ( i = arr[flag]; i < arr[flag+1]; i++)
                {
                    NameOfPC = "\\";
                    NameOfPC += FullListArray[i];
                    command = "/C net view \\";
                    command += NameOfPC;

                    doneEvents[counter] = new ManualResetEvent(false);
                    Testing t = new Testing(FullList, command, i, doneEvents[counter]);
                    testArray[counter] = t;
                    
                    counter += 1;
                    ThreadPool.QueueUserWorkItem(t.ThreadPoolCallback, i);
                }
                
                WaitHandle.WaitAll(doneEvents);
                
                flag += 1;
            }
            return Testing.global.Result;
       }

    }
}
