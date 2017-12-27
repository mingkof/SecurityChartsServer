using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KVDDDCore.Utils
{
    public class FileUtils
    {
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
