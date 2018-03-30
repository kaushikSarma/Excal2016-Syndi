using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace NetworkScanner
{

    public class Scan
    {
        //Creating a struct to return all the details of the PC.
        public struct StructDataOfPC
        {
            public string NameOfPC;
            public string TypeOfPC;
            public long SizeOfSharedFolders;
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello");
        }

        //Fetches all the details of any PC.
        public static List<StructDataOfPC> DetailsOfPC()
        {
            List<StructDataOfPC> FinalOutput = new List<StructDataOfPC>();

            List<List<string>> ListOfPC;
            ListOfPC = RetrievePCNames();
            
            var PublicList = ListOfPC[0].ToArray();
            var PasswordProtectedList = ListOfPC[1].ToArray();
            
            for (int i = 0; i < PublicList.Length; i++) {
                StructDataOfPC s;
                s.NameOfPC = PublicList[i];
                s.TypeOfPC = "Public";
                List<string> NameOfFolders = IdentifyFolderNames(PublicList[i]);
                long sizeOfBytes = SizeOfSharedFiles(PublicList[i], NameOfFolders);
                s.SizeOfSharedFolders = sizeOfBytes;
                FinalOutput.Add(s);
            }

            for (int i = 0; i < PasswordProtectedList.Length; i++)
            {
                StructDataOfPC s;
                s.NameOfPC = PasswordProtectedList[i];
                s.TypeOfPC = "Password";
                List<string> NameOfFolders = IdentifyFolderNames(PasswordProtectedList[i]);
                long sizeOfBytes = 0;
                s.SizeOfSharedFolders = sizeOfBytes;
                FinalOutput.Add(s);
            }

            return FinalOutput;
        }

        // Gets the result from the command line.
        public static string ViewCommandLineResult(string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true; // Helps in supressing the window.
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

        public static string ViewDirectoryListing(string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true; // Helps in supressing the window.
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
                return "||||| " + error;
            }
            return outputDump;

        }

        /*
         Copying Files from one PC to other.
         */

        public static bool CopyFiles(string @oldPath, string @newPath)
        {
            try
            {
                string command;
                command = "/C robocopy " + oldPath + " " + newPath + " /e";
                string CopyingResult = ViewCommandLineResult(command);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to copy files");
                return false;   
            }
            
        }

        /*
         Connecting to the publicly open ACTIVE PC's available
        */

        public static string ShowFiles(string pc)
        {
            string command, outputDump;
            command = "/C net view \\\\";
            command += pc;
            outputDump = ViewDirectoryListing(command);
            return outputDump;
            
        }

        /*
        Connecting to the protected ACTIVE PC's available
       */
        public static bool ShowFiles(string pc, string username, string password)
        {

            string command, outputDump;
            command = "/C net use \\\\" + pc + " /user:" + username + " " + password + " /persistent:no";
            outputDump = ViewDirectoryListing(command);
            Console.WriteLine(outputDump);
            bool AuthenticationError = outputDump.StartsWith("||||| ");
            if (!AuthenticationError)
            {
                command = "/C net view \\\\" + pc;
                outputDump = ViewDirectoryListing(command);
                Console.WriteLine(outputDump);
                return true;
            }
            else
            {
                outputDump = outputDump.Remove(0, 6);
                Console.WriteLine(outputDump);
                return false;
            }

        }


        public static string timeout(string command)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task<string> task = new Task<string>(() => ViewCommandLineResult(command), token);
            task.Start();

            if (task.Wait(TimeSpan.FromSeconds(2)))
            {
                return task.Result;
            }

            else
            {
                return "System error 53 has occurred.";
            }
        }

        public static List<List<string>> RetrievePCNames()
        {

            string command = "/C arp -a";
            string outputDump = ViewCommandLineResult(command);
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


            var FullListArray = FullList.ToArray();
            List<string> PublicPCList = new List<string>();
            List<string> PasswordProtectedPCList = new List<string>();
            List<string> NetworkPathErrorPCList = new List<string>();
            List<List<string>> DistinguishedList = new List<List<string>>();

            string NameOfPC;

            for (int k = 0; k < FullListArray.Length; k++)
            {
                NameOfPC = "\\";
                NameOfPC += FullListArray[k];
                command = "/C net view \\";
                command += NameOfPC;
                
                string connectionOutput = timeout(command);
                var NetworkPathErrorRegex = new Regex(@"System error 53 has occurred.");
                var PasswordProtectionRegex = new Regex(@"System error 5 has occurred.");

                bool NetworkError = NetworkPathErrorRegex.IsMatch(connectionOutput);
                bool PasswordProtection = PasswordProtectionRegex.IsMatch(connectionOutput);

                if (NetworkError)
                {
                    NetworkPathErrorPCList.Add(FullListArray[k]);
                }
                else if (PasswordProtection)
                {
                    PasswordProtectedPCList.Add(FullListArray[k]);
                }
                else
                {
                    PublicPCList.Add(FullListArray[k]);
                }

            }

            

            DistinguishedList.Add(PublicPCList);
            DistinguishedList.Add(PasswordProtectedPCList);
            DistinguishedList.Add(NetworkPathErrorPCList);

            return DistinguishedList;
        }

        public static List<string> IdentifyFolderNames(string pc)
        {
            string command, outputDump;
            List<string> FoldersList = new List<string>();

            command = "/C net view \\\\";
            command += pc;
            outputDump = ViewCommandLineResult(command);
            Console.WriteLine(outputDump);
            foreach (Match match in Regex.Matches(outputDump, @"(.{1}.*?)Disk(.*)([^ {2,10}]*)\n"))
                FoldersList.Add(match.Groups[1].Value);

            return FoldersList;

        }

        //Get the folder size in bytes
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            try
            {
                FileInfo[] fis = d.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Not accessible");
            }
            try
            {
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Not accessible");
            }
            return size;
        }

        public static long SizeOfSharedFiles(string pc, List<string> folderNames)
        {
            long sum = 0;
            var folderNamesArray = folderNames.ToArray();
            string command = "\\\\" + pc;
            for (int i = 0; i < folderNamesArray.Length; i++)
            {
                sum += DirSize(new DirectoryInfo(command + "\\" + folderNamesArray[i]));
            }

            return sum;
        }

        //Recursively lists the inner directories inside a directory.
        public static List<string> GetInnerDirectories(string path)
        {
            List<string> InnerDirectoryNames = new List<string>();

            DirectoryInfo di = new DirectoryInfo(@path);
            DirectoryInfo[] subDirs = di.GetDirectories();
            if (subDirs.Length > 0)
            {
                foreach (DirectoryInfo subDir in subDirs)
                {
                    InnerDirectoryNames.Add(subDir.Name);
                }
            }

            return InnerDirectoryNames;

        }

        //Recursively lists the inner files inside a directory.
        public static List<Tuple<string, long>> GetInnerFiles(string path)
        {
            List<Tuple<string, long>> InnerFileDetails = new List<Tuple<string, long>>();

            DirectoryInfo di = new DirectoryInfo(@path);
            if (di != null)
            {
                FileInfo[] subFiles = di.GetFiles();
                if (subFiles.Length > 0)
                {
                    foreach (FileInfo subFile in subFiles)
                    {
                        InnerFileDetails.Add(new Tuple<string, long>(subFile.Name, subFile.Length));
                    }
                }
            }

            return InnerFileDetails;
        }

    }
}