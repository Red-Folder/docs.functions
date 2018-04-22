using DocFunctions.Lib.Models.Audit;
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
        private IBlobClient _ftpsClient;
        private IWebCache _cache;

        public DeleteImageAction(Removed data,
                                IBlobClient ftpsClient,
                                IWebCache cache)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (ftpsClient == null) throw new ArgumentNullException("ftpsClient");
            if (cache == null) throw new ArgumentNullException("cache");

            _data = data;
            _ftpsClient = ftpsClient;
        }

        public void Execute()
        {
            AuditTree.Instance.StartOperation($"Executing Delete Image Action for {_data.Filename}");
            try
            {
                var filename = $"/site/mediaroot/blog/{_data.FullFilename}";
                Log.Information("Using Ftps to delete: {filename}", filename);
                AuditTree.Instance.Add("Deleting Image from the server");
                _ftpsClient.Delete(filename);

                AuditTree.Instance.Add($"Removing cache for TODO - need image url");
                _cache.RemoveCachedInstances("TODO - need image url");
            }
            catch (Exception ex)
            {
                AuditTree.Instance.AddFailure($"Failed due to exception: {ex.Message}");
            }
            AuditTree.Instance.EndOperation();
        }
    }
}
