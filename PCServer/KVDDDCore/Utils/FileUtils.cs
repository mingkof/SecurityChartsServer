using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KVDDDCore.Utils
{
    public class FileUtils
    {
        private static List<string> readedDirName = new List<string>();
        public static string ReadFile(Stream stream)
        {
            if (stream == null)
                return "";

            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);

            try
            {
                while (!sr.EndOfStream)
                {
                    sb.Append(sr.ReadLine());
                }
                sr.Close();
            }
            catch
            {
            }

            return sb.ToString();
        }

        public static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                return "";

            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                sb.Append(sr.ReadLine());
            }
            sr.Close();

            return sb.ToString();
        }

        public static List<string> ReadDirChild(string path,string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            List<string> resList = new List<string>();
            DirectoryInfo root = new DirectoryInfo(path);

            try
            {
                foreach (var item in root.GetDirectories())
                {
                    if (readedDirName.Contains(item.Name))
                        continue;
                    string childDir = item.Name;
                    string filePath = path + @"\" + childDir + @"\" + fileName;

                    string dataStr = ReadFile(filePath);
                    if (dataStr != null)
                    {
                        resList.Add(dataStr);
                    }
                    readedDirName.Add(childDir);
                }
            }
            catch
            {

                return new List<string>();
            }
            
            return resList;
        }


        public static List<string> ReadFileToList(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                list.Add(sr.ReadLine());
            }
            sr.Close();

            return list;
        }


        public static Stream ReadFileToStream(string path)
        {
            try
            {
                // 打开文件   
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                // 读取文件的 byte[]   
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                fileStream.Close();
                // 把 byte[] 转换成 Stream   
                Stream stream = new MemoryStream(bytes);
                return stream;
            }
            catch 
            {

                return null;
            }
          
        }

        //写
        public static void WriteFile(string path, string content)
        {
            if (!File.Exists(path))
            {
                FileInfo myfile = new FileInfo(path);
                FileStream fs = myfile.Create();
                fs.Close();
            }
            char[] c = content.ToCharArray();

            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(c);
            }
        }

        public static void WriteFile(string path, List<string> list, bool overWrite, Encoding encode)
        {
            if (overWrite || !File.Exists(path))
            {
                FileInfo myfile = new FileInfo(path);
                FileStream fs = myfile.Create();
                fs.Close();
            }
            StreamWriter sw = new StreamWriter(path, true, encode);
            foreach (string item in list)
            {
                sw.WriteLine(item);
            }
            sw.Flush();
            sw.Close();
        }
        public static bool WriteFile(string path, string content, bool overWrite, Encoding encode)
        {
            if (overWrite || !File.Exists(path))
            {
                FileInfo myfile = new FileInfo(path);
                FileStream fs = myfile.Create();
                fs.Close();
            }
            StreamWriter sw = new StreamWriter(path, true, encode);
            sw.Write(content);

            sw.Flush();
            sw.Close();

            return true;
        }


        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception)
            {
                //throw new Exception("GetMD5HashFromFile() fail, error:” +ex.Message);
            }

            return "";
        }

    }
}
