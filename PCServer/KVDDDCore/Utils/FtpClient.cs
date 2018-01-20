using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace KVDDDCore.Utils
{
    public class FtpClient
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        //private StreamReader reader = null;

        //private int bufferSize = 2048;
        private List<string> deadDir = new List<string>();

        public FtpClient(string hostIP, string userName, string password)
        {
            host = hostIP;
            user = userName;
            pass = password;
        }
        public Stream Download(string remoteFile)
        {
            try
            {
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpStream = ftpResponse.GetResponseStream();
                return ftpStream;
                //reader = new StreamReader(ftpStream);
                //string json = reader.ReadToEnd();
                //return json;
            }
            catch
            {
                return null;
            }
        }

        public string DownloadToStr(string remoteFile)
        {
            try
            {
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = ftpResponse.GetResponseStream();
                var reader = new StreamReader(ftpStream);
                string json = reader.ReadToEnd();
                return json;
            }
            catch
            {
                return "";
            }
      
        }
        public List<string> DownloadToListStr(string remoteFile)
        {
            try
            {
                List<string> list = new List<string>();
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = ftpResponse.GetResponseStream();
                var reader = new StreamReader(ftpStream);
                while (!reader.EndOfStream)
                {
                    list.Add(reader.ReadLine());
                }
                reader.Close();
                return list;
            }
            catch
            {
                return new List<string>();
            }
       
        }

        /// <summary>
        /// 读取人脸识别数据
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public List<string> DownloadDirToListStr(string remoteFile)
        {
            try
            {
                List<string> dirlist = new List<string>();
                List<string> jsonList = new List<string>();
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = ftpResponse.GetResponseStream();
                var reader = new StreamReader(ftpStream);
                while (!reader.EndOfStream)
                {
                    dirlist.Add(reader.ReadLine());
                }
                reader.Close();

                for (int i = 0; i < dirlist.Count; i++)
                {
                    if (deadDir.Contains(dirlist[i]))
                        continue;
                    deadDir.Add(dirlist[i]);
                    string dataStr= DownloadToStr(dirlist[i] + "/data.json");
                    if (dataStr != null)
                        jsonList.Add(dataStr);
                }
                return jsonList;
            }
            catch
            {
                return new List<string>();
            }
        }



        //    FileStream localFileStream = new FileStream(localFile, FileMode.Create);
        //    byte[] byteBuffer = new byte[bufferSize];
        //    int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
        //    try
        //    {
        //        while (bytesRead > 0)
        //        {
        //            localFileStream.Write(byteBuffer, 0, bytesRead);
        //            bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //    localFileStream.Close();
        //    ftpStream.Close();
        //    ftpResponse.Close();
        //    ftpRequest = null;


        //public void Upload(string remoteFile, string localFile)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
        //        ftpStream = ftpRequest.GetRequestStream();
        //        FileStream localFileStream = new FileStream(localFile, FileMode.Create);
        //        byte[] byteBuffer = new byte[bufferSize];
        //        int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
        //        try
        //        {
        //            while (bytesSent != 0)
        //            {
        //                ftpStream.Write(byteBuffer, 0, bytesSent);
        //                bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
        //            }
        //        }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //        localFileStream.Close();
        //        ftpStream.Close();
        //        ftpRequest = null;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return;
        //}
        //public void Delete(string deleteFile)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return;
        //}
        //public void Rename(string currentFileNameAndPath, string newFileName)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.Rename;
        //        ftpRequest.RenameTo = newFileName;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return;
        //}
        //public void createDirectory(string newDirectory)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return;
        //}
        //public string getFileCreatedDateTime(string fileName)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpStream = ftpResponse.GetResponseStream();
        //        StreamReader ftpReader = new StreamReader(ftpStream);
        //        string fileInfo = null;
        //        try { fileInfo = ftpReader.ReadToEnd(); }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //        ftpReader.Close();
        //        ftpStream.Close();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //        return fileInfo;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return "";
        //}
        //public string GetFileSize(string fileName)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpStream = ftpResponse.GetResponseStream();
        //        StreamReader ftpReader = new StreamReader(ftpStream);
        //        string fileInfo = null;
        //        try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //        ftpReader.Close();
        //        ftpStream.Close();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //        return fileInfo;
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return "";
        //}
        //public string[] DirectoryListSimple(string directory)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpStream = ftpResponse.GetResponseStream();
        //        StreamReader ftpReader = new StreamReader(ftpStream);
        //        string directoryRaw = null;
        //        try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //        ftpReader.Close();
        //        ftpStream.Close();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //        try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return new string[] { "" };
        //}
        //public string[] DirectoryListDetailed(string directory)
        //{
        //    try
        //    {
        //        ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
        //        ftpRequest.Credentials = new NetworkCredential(user, pass);
        //        ftpRequest.UseBinary = true;
        //        ftpRequest.UsePassive = true;
        //        ftpRequest.KeepAlive = true;
        //        ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        //        ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        //        ftpStream = ftpResponse.GetResponseStream();
        //        StreamReader ftpReader = new StreamReader(ftpStream);
        //        string directoryRaw = null;
        //        try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //        ftpReader.Close();
        //        ftpStream.Close();
        //        ftpResponse.Close();
        //        ftpRequest = null;
        //        try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
        //        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    }
        //    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        //    return new string[] { "" };
        //}
    }
}
