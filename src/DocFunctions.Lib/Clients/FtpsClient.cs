using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
    [ExcludeFromCodeCoverage]
    public class FtpsClient : IFtpsClient
    {
        private string _host;
        private string _username;
        private string _password;

        public FtpsClient(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
        }

        public void Upload(string filename, string contents)
        {
            Upload(filename, Encoding.UTF8.GetBytes(contents));
        }

        public void Upload(string filename, byte[] contents)
        {
            EnsurePathExists(filename);

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FormatUrl(filename));
            request.Credentials = new NetworkCredential(_username, _password);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.EnableSsl = true;
            request.UseBinary = true;

            request.ContentLength = contents.Length;
            using (Stream s = request.GetRequestStream())
            {
                s.Write(contents, 0, contents.Length);
            }

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }

        private string FormatUrl(string suffix)
        {
            return String.Format("ftp://{0}/{1}", _host, suffix);
        }

        private void EnsurePathExists(string filename)
        {
            var directories = filename.Split('/');

            // Minus 1 as we don't want to look at the filename
            var path = "";
            for (int i = 0; i < (directories.Length -1); i++)
            {
                path += directories[i] + "/";
                CreateDirectoryIfNotExists(path);
            }
        }

        private void CreateDirectoryIfNotExists(string path)
        {
            if (!DirectoryExists(path))
            {
                MakeDirectory(path);
            }
        }

        private bool DirectoryExists(string directory)
        {
            bool directoryExists;

            var request = (FtpWebRequest)WebRequest.Create(FormatUrl(directory));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(_username, _password);

            try
            {
                using (request.GetResponse())
                {
                    directoryExists = true;
                }
            }
            catch (WebException)
            {
                directoryExists = false;
            }
            return directoryExists;
        }

        private void MakeDirectory(string directory)
        {
            var request = (FtpWebRequest)WebRequest.Create(FormatUrl(directory));

            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(_username, _password);

            try
            {
                using (var resp = (FtpWebResponse)request.GetResponse()) // Exception occurs here
                {
                }
            }
            catch (WebException ex)
            {
            }
        }
    }
}
