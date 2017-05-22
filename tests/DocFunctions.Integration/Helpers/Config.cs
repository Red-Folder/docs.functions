﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFunctions.Integration.Helpers
{
    public class Config
    {
        private string _blogname;

        public Config(string blogname)
        {
            _blogname = blogname;
        }

        public string RepoUrl
        {
            get
            {
                return $"https://rfc-doc-functions-staging.azurewebsites.net/api/Blog/{BlogName}?code={AzureFunctionKey}";
            }
        }

        public string ImageUrl
        {
            get
            {
                return $"http://rfc-website-staging.azurewebsites.net/media/blog/{BlogName}/image.png";
            }
        }

        public string BlogName
        {
            get
            {
                return _blogname;
            }
        }

        public string AzureFunctionKey
        {
            get
            {
                return GetAppSetting("AzureFunctionKey");
            }
        }

        public string GitHubUsername
        {
            get
            {
                return GetAppSetting("github-username");
            }
        }

        public string GitHubKey
        {
            get
            {
                return GetAppSetting("github-key");
            }
        }

        public string GitHubRepo
        {
            get
            {
                return GetAppSetting("github-repo");
            }
        }
        private string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key];
            }
            return System.Environment.GetEnvironmentVariable(key);
        }

        private string GetConenctionString(string key)
        {
            if (ConfigurationManager.ConnectionStrings[key] != null)
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            return System.Environment.GetEnvironmentVariable(key);
        }

    }
}
