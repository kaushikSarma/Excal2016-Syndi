using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Net.NetworkInformation;



namespace ListingPC
{

    public class FolderSize
    {

        public static long DirSize(DirectoryInfo d)
        {

            try
            {
                long size = 0;
                // Add file sizes.
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                // Add subdirectory sizes.
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
                return size;
            }
            catch (Exception e)
            {
                Console.WriteLine("Folder not shared exception " + e);
                return 0;
            }
        }


        public static long SizeOfSharedFiles(string pc)
        {
            string command, outputDump;
            List<string> FoldersList = new List<string>();

            command = @"/C net view \\";
            command += pc;

            outputDump = Testing.timeout(command);
            if (outputDump == "Error")
            {
                return 0;
            }
            foreach (Match match in Regex.Matches(outputDump, @"(.{1}.*?)Disk(.*)([^ {2,10}]*)\n"))
                FoldersList.Add(match.Groups[1].Value);

            long sum = 0;
            var folderNamesArray = FoldersList.ToArray();
            command = "\\\\" + pc;
            for (int i = 0; i < folderNamesArray.Length; i++)
            {
                sum += DirSize(new DirectoryInfo(command + "\\" + folderNamesArray[i]));
            }

            return sum;
        }

        public struct SharedFolderSize
        {
            public string name;
            public long size;
        }
        public static long val1, val2, val3;
        public static string name1, name2, name3;

        public static List<SharedFolderSize> CaluclateSharedFolderSize(List<Testing.DataOfPC> pcList)
        {
            List<SharedFolderSize> ListSize = new List<SharedFolderSize>();
            SharedFolderSize s;
            try
            {

                for (int v = 0; v < pcList.ToArray().Length + 3;)
                {
                    name1 = pcList.ToArray()[v].Name;
                    name2 = pcList.ToArray()[v + 1].Name;
                    name3 = pcList.ToArray()[v + 2].Name;

                    Thread threadx = new Thread(() => { val1 = SizeOfSharedFiles(name1); });
                    Thread thready = new Thread(() => { val2 = SizeOfSharedFiles(name1); });
                    Thread threadz = new Thread(() => { val3 = SizeOfSharedFiles(name1); });

                    threadx.Start();
                    thready.Start();
                    threadz.Start();

                    threadx.Join();
                    thready.Join();
                    threadz.Join();

                    s.name = name1;
                    s.size = val1;
                    ListSize.Add(s);

                    s.name = name2;
                    s.size = val2;
                    ListSize.Add(s);

                    s.name = name3;
                    s.size = val3;
                    ListSize.Add(s);

                    v += 3;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("**Ignore : " + e);
            }
            return ListSize;
        }

    }

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
        public static void Main(string[] args)
        {
            ScanNetwork();
            Console.Read();
        }

        public static List<Testing.DataOfPC> ScanNetwork()
        {
            string command = "/C arp -a";
            string outputDump = Testing.ViewCommandLineResult(command);
            List<string> FullList = new List<string>();
            
            // Iterate all other PCs
            foreach (Match match in Regex.Matches(outputDump, @"([0-9\.]+){4} [^\-]"))
            {
                Console.WriteLine(match.ToString().Trim());
                //Console.WriteLine(Regex.IsMatch(match.ToString(), @"2[2-5][4-5]\.([0-9\.]+){3}"));
                //Console.WriteLine(Regex.IsMatch(match.ToString(), @"([0-9\.]+){3}\.255"));
                if (Regex.IsMatch(match.ToString().Trim(), @"2[2-5][4-5]\.([0-9\.]+){3}") || Regex.IsMatch(match.ToString().Trim(), @"([0-9\.]+){3}\.255") || Regex.IsMatch(match.ToString().Trim(), @"([0-9\.]+){3}\.0"))
                {
                    Console.WriteLine("This is a multicast address range");
                }
                else
                {
                    FullList.Add(match.ToString().Trim());
                }
            }

            // Adds owner host to the list of PCs
            FullList.Add(Dns.GetHostName().ToString());

            int n = FullList.ToArray().Length;
            List<int> l = new List<int>();
            List<Testing> ans = new List<Testing>();
            int flag;
            for (flag = 0; flag < n; flag += 4)
            {
                l.Add(flag);
            }
            l.Add(n);
            var arr = l.ToArray();
            flag = 0;


            while (flag < arr.Length - 1)
            {
                ManualResetEvent[] doneEvents = new ManualResetEvent[arr[flag + 1] - arr[flag]];
                Testing[] testArray = new Testing[arr[flag + 1] - arr[flag]];

                int counter = 0;
                string NameOfPC;
                var FullListArray = FullList.ToArray();
                //var done = new CountdownEvent(4);
                int i;
                for (i = arr[flag]; i < arr[flag + 1]; i++)
                {
                    NameOfPC = "\\";
                    NameOfPC += FullListArray[i];
                    command = "/C net view \\";
                    command += NameOfPC;

                    doneEvents[counter] = new ManualResetEvent(false);
                    //done.AddCount();
                    Testing t = new Testing(FullList, command, i, doneEvents[counter]);
                    testArray[counter] = t;

                    counter += 1;
                    ThreadPool.QueueUserWorkItem(t.ThreadPoolCallback, i);
                }

                //WaitHandle.WaitAll(doneEvents);
                //done.Signal();
                //done.Wait();
                foreach (WaitHandle handle in doneEvents)
                {
                    handle.WaitOne();
                }
                flag += 1;
            }
            return Testing.global.Result;
        }

    }
}