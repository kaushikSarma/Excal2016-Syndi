using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SegLibrary
{
    public class Seperate
    {
        public static List<string> GetVideos(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(mp4|avi|asf|mov|qt|flv|swf|avchd|kmp|mkv)", RegexOptions.IgnoreCase);
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
        public static List<string> GetImages(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(ani|bmp|cal|fax|gif|img|jbg|jpg|jpe|jpeg|mac|pbm|pcd|pcx|pct|pgm|png|ppm|psd|ras|tga|tiff|wmf|jpg)", RegexOptions.IgnoreCase);
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
        public static List<string> GetDocs(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(docx|xls|txt|pdf|doc|ppt|one|pub|xlsx|pptx|doc|vsdx|accdb|dot|maw|c|cpp|as|h|asm)", RegexOptions.IgnoreCase);
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
        public static List<string> GetAudios(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(pcm|wav|aiff|mp3|aac|ogg|wma|flac|alac|wma|m4a)", RegexOptions.IgnoreCase);
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
        public static void CurrSearch(string sDir, Regex regex, List<string> result)
        {

            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*"))
                {
                    if (regex.IsMatch(f))
                        result.Add(f);
                }
                foreach (string f in Directory.GetDirectories(sDir, "*"))
                {
                    if (regex.IsMatch(f))
                        result.Add(f);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        public static void DirSearch(string sDir, Regex regex, List<string> result)
        {

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d, "*"))
                    {
                        if (regex.IsMatch(f))
                            result.Add(f);
                    }
                    DirSearch(d, regex, result);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

    }
}
