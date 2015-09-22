using System;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;

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

    public static MvcHtmlString Image(this HtmlHelper helper, string src, object htmlAttributes)
    {
      return BuildImageTag(src, htmlAttributes);
    }

    public static MvcHtmlString ImageFor<TModel, TProperty>(
    this HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression)
    {
      var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
      return BuildImageTag(imgUrl.ToString(), null);
    }
    public static MvcHtmlString ImageFor<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression,
        object htmlAttributes)
    {
      var imgUrl = expression.Compile()(htmlHelper.ViewData.Model);
      if(imgUrl==null)
      {
        return BuildImageTag("", htmlAttributes);
      }
      else
      { 
        return BuildImageTag(imgUrl.ToString(), htmlAttributes);
      }
    }

    private static MvcHtmlString BuildImageTag(string imgUrl, object htmlAttributes)
    {
      TagBuilder tag = new TagBuilder("img");

      tag.Attributes.Add("src", imgUrl);
      if (htmlAttributes != null)
        tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

      return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
    }
  }
}