using System;
using System.Web;
using System.Web.Mvc;

namespace IcedMemories.Helpers
{
  public static class ImageHelper
  {
    public static MvcHtmlString Image(this HtmlHelper helper, string src, string altText, string width, string height)
    {
      var builder = new TagBuilder("img");
      builder.MergeAttribute("src", src);
      builder.MergeAttribute("alt", altText);
      try
      {
        if (Convert.ToInt32(width) > 0)
        {
          builder.MergeAttribute("width", width);
        }
      }
      catch (Exception exWidth)
      { }
      try
      { 
        if(Convert.ToInt32(height)>0)
        {
          builder.MergeAttribute("height", height);
        }
      }
      catch(Exception exHeight)
      {}

      return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
    }
  }
}