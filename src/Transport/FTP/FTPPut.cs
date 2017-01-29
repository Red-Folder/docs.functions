using System;
using System.IO;
using System.Net;
using System.Text;

namespace RedFolder.Transport.FTP
{
    public class FTPPut: IPut
    {
        private string _host;
        private string _path;
        private string _filename;
        private string _username;
        private string _password;

        public FTPPut(string host, string path, string filename, string username, string password)
        {
            _host = host;
            _path = path;
            _filename = filename;
            _username = username;
            _password = password;
        }

        public void Put(byte[] payload)
        {
            string fullname = String.Format("{0}/{1}/{2}", _host, _path, _filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullname);
            request.Credentials = new NetworkCredential(_username, _password);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.EnableSsl = true;
            request.UseBinary = true;

            request.ContentLength = payload.Length;
            using (Stream s = request.GetRequestStream())
            {
                s.Write(payload, 0, payload.Length);
            }

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }
    }
}
