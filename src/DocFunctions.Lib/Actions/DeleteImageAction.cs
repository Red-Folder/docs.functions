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
        private IBlobClient _blobClient;
        private IWebCache _cache;
        private AuditTree _audit;

        public DeleteImageAction(Removed data,
                                IBlobClient blobClient,
                                IWebCache cache,
                                AuditTree audit)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (blobClient == null) throw new ArgumentNullException("blobClient");
            if (cache == null) throw new ArgumentNullException("cache");
            if (audit == null) throw new ArgumentNullException("audit");

            _data = data;
            _blobClient = blobClient;
            _cache = cache;
            _audit = audit;
        }

        public void Execute()
        {
            _audit.StartOperation($"Executing Delete Image Action for {_data.Path}: {_data.Filename}");
            try
            {
                var filename = $"{_data.FullFilename}".ToLower();
                Log.Information("Deleting: {filename}", filename);
                _audit.Audit("Deleting Image from the server");
                _blobClient.Delete(filename);

                _audit.Audit($"Removing cache for TODO - need image url");
                _cache.RemoveCachedInstances("TODO - need image url");
            }
            catch (Exception ex)
            {
                _audit.Error($"Failed due to exception: {ex.Message}", ex);
            }
            _audit.EndOperation();
        }
    }
}
