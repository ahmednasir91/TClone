using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TwitterClone.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString UserStyle(this HtmlHelper html)
        {
            var styleTag = new TagBuilder("style");
            styleTag.SetInnerText(@"body {
          background-position: left 40px;
          background-attachment: fixed;
          background-repeat: repeat;
            background-repeat: no-repeat;
          background-color: #C0DEED;
      }");
            return new MvcHtmlString(styleTag.ToString());
        }
    }
}