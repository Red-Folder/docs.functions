using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Clients
{
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
            string fullname = String.Format("ftp://{0}/{1}", _host, filename);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullname);
            request.Credentials = new NetworkCredential(_username, _password);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.EnableSsl = true;
            request.UseBinary = true;

            var payload = Encoding.UTF8.GetBytes(contents);
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
