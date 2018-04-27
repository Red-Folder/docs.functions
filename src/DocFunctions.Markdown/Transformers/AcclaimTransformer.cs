using docsFunctions.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace DocFunctions.Markdown.Transformers
{
    public class AcclaimTransformer: BaseTransformer
    {
        private const string APPBUILDER = @"
          <div data-iframe-width='150' data-iframe-height='270' data-share-badge-id='f57a78d2-31ae-42d5-a39d-48eaa1bd06cd'></div>
          <script type='text/javascript'>
            (function() {
              var s = document.createElement('script');
              s.type = 'text/javascript';
              s.async = true;
              s.src = '//cdn.youracclaim.com/assets/utilities/embed.js';
              var o = document.getElementsByTagName('script')[0];
              o.parentNode.insertBefore(s, o);
            })();
          </script>
        ";
        private const string WEBAPPLICATIONS = @"
          <div data-iframe-width='150' data-iframe-height='270' data-share-badge-id='9ffb89e1-1f12-4382-9033-028aaebe793b'></div>
          <script type='text/javascript'>
            (function() {
              var s = document.createElement('script');
              s.type = 'text/javascript';
              s.async = true;
              s.src = '//cdn.youracclaim.com/assets/utilities/embed.js';
              var o = document.getElementsByTagName('script')[0];
              o.parentNode.insertBefore(s, o);
            })();
          </script>
        ";
        
        public AcclaimTransformer() : base()
        {

        }

        public AcclaimTransformer(ITransformer innerTransformer) : base(innerTransformer)
        {

        }

        protected override string PostTransform(Blog meta, string markdown)
        {
            return markdown.Replace("{ACCLAIM-APPBUILDER}", APPBUILDER).Replace("{ACCLAIM-WEBAPPLICATIONS}", WEBAPPLICATIONS);
        }
    }
}
