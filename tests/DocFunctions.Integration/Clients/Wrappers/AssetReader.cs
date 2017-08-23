using DocFunctions.Integration.Models;
using DocFunctions.Lib.Wappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Clients.Wrappers
{
    public class AssetReader
    {
        private Config _config;
        public AssetReader(Config config)
        {
            _config = config;
        }

        public string GetTextFile(string path)
        {
            var rawText = File.ReadAllText(path);

            return rawText.Replace("[BLOGNAME]", _config.BlogName);
        }

        public byte[] GetImageFile(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
