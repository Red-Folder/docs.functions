using DocFunctions.Integration.Models;
using System.IO;

namespace DocFunctions.Integration.Clients.Wrappers
{
    public class AssetReader
    {
        private const string TOKEN_BLOGNAME = "[BLOGNAME]";

        private Config _config;
        public AssetReader(Config config)
        {
            _config = config;
        }

        public string GetTextFile(string path)
        {
            var rawText = File.ReadAllText(path);

            return rawText.Replace(TOKEN_BLOGNAME, _config.BlogName);
        }

        public byte[] GetImageFile(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
