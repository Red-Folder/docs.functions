using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Wappers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Lib.Actions
{
    public class DeleteImageAction : IAction
    {
        private Removed _data;
        private IFtpsClient _ftpsClient;

        public DeleteImageAction(Removed data,
                                IFtpsClient ftpsClient)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");


            _data = data;
            _ftpsClient = ftpsClient;
        }

        public void Execute()
        {
            var filename = $"/site/mediaroot/blog{_data.FullFilename}";
            Log.Information("Using Ftps to delete: {filename}", filename);
            _ftpsClient.Delete(filename);
        }
    }
}
