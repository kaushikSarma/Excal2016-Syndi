using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using SegLibrary;
using NetworkScanner;

namespace SearchLibrary
{
    public class Search
    {
        public static List<string> GetSearchList(List<string> pc, string query)
        {
            var regex = new Regex(@query, RegexOptions.IgnoreCase);
            List<string> result = new List<string>();
            foreach (var singlepc in pc)
            {
                var path = "\\\\" + singlepc;
                List<string> folders = Scan.IdentifyFolderNames(singlepc);
                foreach (var folder in folders)
                {
                    List<string> tmp = new List<string>();
                    string appendpath = path + @"\\" + folder.Trim();
                    // Console.WriteLine(appendpath);
                    Seperate.DirSearch(appendpath, regex, tmp);
                    result.AddRange(tmp);
                }
            }
            return result;
        }
    }
}
