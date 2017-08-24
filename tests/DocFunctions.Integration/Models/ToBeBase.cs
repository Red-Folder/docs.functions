namespace DocFunctions.Integration.Models
{
    public class ToBeBase
    {
        internal string _repoFilename;

        public ToBeBase(string repoFilename)
        {
            _repoFilename = repoFilename;
        }

        public string RepoFilename
        {
            get
            {
                return _repoFilename;
            }
        }
    }
}
