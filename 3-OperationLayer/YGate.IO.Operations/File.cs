using System.IO;
using System.Text;
namespace YGate.IO.Operations
{
    public static class File
    {
        private static List<string> GetAllFiles(string Path)
        {
            var returendlist = Directory.GetFiles(Path).ToList();
            return returendlist;
        }

        public static List<string> GetAllCssFile(string rootPath)
        {
            return GetAllFiles($"{rootPath}{Path.DirectorySeparatorChar}css");
        }

        public static List<string> GetAllJsFile(string rootPath)
        {
            return GetAllFiles($"{rootPath}{Path.DirectorySeparatorChar}js");
        }

        public static List<string> GetAllBlog(string rootpath)
        {
            return GetAllFiles($"{rootpath}{Path.DirectorySeparatorChar}doc");
        }

        public static bool WriteBlog(string rootPath, string fileName, string Data)
        {
            StreamWriter streamWriter = new($"{rootPath}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}doc{Path.DirectorySeparatorChar}{fileName}.html");
            streamWriter.Write(Data);
            return true;
        }

        public static string ReadToFile(string rootPath, string Filename)
        {
            StreamReader reader = new($"{rootPath}{Path.DirectorySeparatorChar}{Filename}");
            string FileReadToString = reader.ReadToEnd();
            return FileReadToString;

        }

        public static async void SaveLog(string v)
        {
            string linesdata = "\n" + v;
            using (FileStream fs = new FileStream("logs.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                await fs.WriteAsync(ASCIIEncoding.UTF8.GetBytes(linesdata));
                fs.Close();
            }
        }
    }
}
