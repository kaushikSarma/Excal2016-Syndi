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
<<<<<<< HEAD
        public static List<string> GetVideos(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(mp4|avi|asf|mov|qt|flv|swf|avchd|kmp|mkv)", RegexOptions.IgnoreCase);
=======
        static List<string> GetVideos(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(mp4|avi|asf|mov|qt|flv|swf|avchd|kmp)", RegexOptions.IgnoreCase);
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
<<<<<<< HEAD
        public static List<string> GetImages(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(ani|bmp|cal|fax|gif|img|jbg|jpg|jpe|jpeg|mac|pbm|pcd|pcx|pct|pgm|png|ppm|psd|ras|tga|tiff|wmf|jpg)", RegexOptions.IgnoreCase);
=======
        static List<string> GetImages(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(ani|bmp|cal|fax|gif|img|jbg|jpe|jpeg|mac|pbm|pcd|pcx|pct|pgm|png|ppm|psd|ras|tga|tiff|wmf)", RegexOptions.IgnoreCase);
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
<<<<<<< HEAD
        public static List<string> GetDocs(string path)
=======
        static List<string> GetDocs(string path)
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(docx|xls|txt|pdf|doc|ppt|one|pub|xlsx|pptx|doc|vsdx|accdb|dot|maw)", RegexOptions.IgnoreCase);
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
<<<<<<< HEAD
        public static List<string> GetAudios(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(pcm|wav|aiff|mp3|aac|ogg|wma|flac|alac|wma|m4a)", RegexOptions.IgnoreCase);
=======
        static List<string> GetAudios(string path)
        {
            List<string> result = new List<string>();
            var regex = new Regex(@".*\.(pcm|wav|aiff|mp3|aac|ogg|wma|flac|alac|wma)", RegexOptions.IgnoreCase);
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
            CurrSearch(path, regex, result);
            DirSearch(path, regex, result);
            return result;
        }
<<<<<<< HEAD
        public static void CurrSearch(string sDir, Regex regex, List<string> result)
=======
        static void CurrSearch(string sDir, Regex regex, List<string> result)
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
        {

            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*"))
                {
                    if (regex.IsMatch(f))
                        result.Add(f);
                }
<<<<<<< HEAD
                foreach (string f in Directory.GetDirectories(sDir, "*"))
                {
                    if (regex.IsMatch(f))
                        result.Add(f);
                }
=======
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
<<<<<<< HEAD
        public static void DirSearch(string sDir, Regex regex, List<string> result)
=======
        static void DirSearch(string sDir, Regex regex, List<string> result)
>>>>>>> 9efb8e42243f51850c1a22ecbcd33cd374ca75eb
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
