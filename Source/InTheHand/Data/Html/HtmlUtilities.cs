// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlUtilities.cs" company="In The Hand Ltd">
//   Copyright (c) 2016-17 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if __ANDROID__
using Android.Text;
#elif __UNIFIED__
using Foundation;
#elif WINDOWS_PHONE || WIN32
using System.Text.RegularExpressions;
#endif

namespace InTheHand.Data.Html
{
    /// <summary>
    /// Provides utility methods for use with HTML-formatted data.
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Android</term><description>Android 4.4 and later</description></item>
    /// <item><term>iOS</term><description>iOS 9.0 and later</description></item>
    /// <item><term>macOS</term><description>OS X 10.7 and later</description></item>
    /// <item><term>tvOS</term><description>tvOS 9.0 and later</description></item>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows Phone 8.1 or later</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item>
    /// <item><term>Windows (Desktop Apps)</term><description>Windows 7 or later</description></item>
    /// </list>
    /// </remarks>
    public static class HtmlUtilities
    {
        /// <summary>
        /// Converts HTML-formatted data to a string that contains the text content extracted from the HTML.
        /// </summary>
        /// <param name="html">A String containing HTML-formatted data.</param>
        /// <returns>A String of text content.</returns>
        public static string ConvertToText(string html)
        {
            // the platform implementations don't strip out unordered lists so we'll replace with bullet characters
            int i = 0;
            int i2;
            while (i > -1)
            {
                i = html.IndexOf("<li");
                if (i > -1)
                {
                    i2 = html.IndexOf(">", i);

                    if (i2 > -1)
                    {
                        html = html.Replace(html.Substring(i, (i2 - i) + 1), "<p>• ");
                    }
                }
            }

            i = 0;
            while (i > -1)
            {
                i = html.IndexOf("<ul");
                if (i > -1)
                {
                    i2 = html.IndexOf(">", i);

                    if (i2 > -1)
                    {
                        html = html.Replace(html.Substring(i, (i2 - i) + 1), "");
                    }
                }
            }

            i = 0;
            while (i > -1)
            {
                i = html.IndexOf("<img");
                if (i > -1)
                {
                    i2 = html.IndexOf(">", i);

                    if (i2 > -1)
                    {
                        html = html.Replace(html.Substring(i, (i2 - i) + 1), "");
                    }
                }
            }
            
            html = html.Replace("</ul>", "");
            html = html.Replace("</li>", "<p/>");

#if __ANDROID__
            ISpanned sp = Android.Text.Html.FromHtml(html, FromHtmlOptions.ModeLegacy);
            return sp.ToString().Trim();

#elif __UNIFIED__
            byte[] data = global::System.Text.Encoding.UTF8.GetBytes(html);
            NSData d = NSData.FromArray(data);
            NSAttributedStringDocumentAttributes importParams = new NSAttributedStringDocumentAttributes();
            importParams.DocumentType = NSDocumentType.HTML;
            NSError error = new NSError();
            error = null;
            NSDictionary dict = new NSDictionary();
#if __MAC__
            var attrString = new NSAttributedString(d, importParams, out dict, out error);
#else
            var attrString = new NSAttributedString(d, importParams, out dict, ref error);
#endif
            return attrString.Value;

#elif WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
            return Windows.Data.Html.HtmlUtilities.ConvertToText(html);
#elif WINDOWS_PHONE || WIN32
            return global::System.Net.WebUtility.HtmlDecode(Regex.Replace(html, "<(.|\n)*?>", ""));
#else
            throw new global::System.PlatformNotSupportedException();
#endif
        }
    }
}