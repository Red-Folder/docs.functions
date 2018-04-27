using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocFunctions.Markdown.Transformers
{
    public class ROIArticleTransformer: BaseTransformer
    {
        private const string ROIBLOCK = @"
          <div class=""roi-block reverse"">
            This article is part of my <strong>Better Return On Investment from Software Development</strong> series.<br/>
            These articles are aimed at senior management funding Software Developer or IT Professionals attempting to show the ROI benefits of what can sometimes seem counterintuitive.<br/>
            This ever growing library is provided free of charge and can be found <a href=""/Projects/ROI"">here</a>.
          </div>
        ";

        public ROIArticleTransformer() : base()
        {

        }

        public ROIArticleTransformer(ITransformer innerTransformer) : base(innerTransformer)
        {

        }
        protected override string PostTransform(Blog meta, string markdown)
        {

            if (IsROIArticle(meta))
            {
                return ROIBLOCK + markdown;
            }
            else
            {
                return markdown;
            }
        }

        private bool IsROIArticle(Blog meta)
        {
            try
            {
                return (meta.KeyWords != null && meta.KeyWords.Contains("ROI"));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
