using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace NetworkScanner
{
    public class Scan
    {
        public static string ViewCommandLineResult(string args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = args;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
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

        public static bool CopyFiles(string pc, string path, string newPath)
        {
            try
            {
                string command, oldPath;
                oldPath = pc + "\\" + path;

                command = "copy " + oldPath + " " + newPath + " /y";
                string CopyingResult = ViewCommandLineResult(command);
                return true;
            }
            catch (Exception)
            {
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
        public static string ShowFiles(string pc, string username, string password)
        {
            
            string command, outputDump;
            command = "/C net use \\\\" + pc + " /user:" + username + " " + password + " /persistent:no";
            outputDump = ViewDirectoryListing(command);
            bool AuthenticationError = outputDump.StartsWith("||||| ");
            if (!AuthenticationError)
            {
                command = "/C net view \\\\" + pc;
                outputDump = ViewDirectoryListing(command);
                return outputDump;
            }
            else
            {
                outputDump = outputDump.Remove(0, 6);
                return outputDump;
            }
            
        }


        public static List<List<string>> RetrievePCNames()
        {

            string command = "/C net view /all";
            string outputDump = ViewCommandLineResult(command);
            var items = Regex.Split(outputDump, @"\\");

            List<string> FullList = new List<string>();
            int i = 2;

            while (i < items.Length)
            {
                var s = items[i];
                var Name = Regex.Split(s, @" ");
                FullList.Add(Name[0]);
                i = i + 2;
            }


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
                string connectionOutput = ViewCommandLineResult(command);

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

    }
}