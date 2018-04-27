using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace DocFunctions.Markdown.Transformers
{
    public class ImageTransformer : BaseTransformer
    {
        public ImageTransformer() : base()
        {

        }

        public ImageTransformer(ITransformer innerTransformer) : base(innerTransformer)
        {

        }

        protected override string PostTransform(Blog meta, string markdown)
        {
            var className = "img-responsive";

            return Regex.Replace(markdown, @"<img (.*)\>", m => {
                var imgInner = m.Value;

                if (imgInner.Contains("class"))
                {
                    return Regex.Replace(imgInner, @"class=""(.*)""", m2 => {
                        var classInner = m2.Value;

                        if (classInner.Contains(className))
                        {
                            return classInner;
                        }
                        else
                        {
                            return classInner.Insert(classInner.IndexOf('"') + 1, String.Format("{0} ", className));
                        }

                    });
                }
                else
                {
                    return imgInner.Insert("<img ".Length, String.Format(@"class=""{0}"" ", className));
                }
            });
        }
    }
}
