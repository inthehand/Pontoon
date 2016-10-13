// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationLanguages.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Globalization.ApplicationLanguages))]
#else

using System.Collections.Generic;

#if __IOS__
using Foundation;
#endif

namespace Windows.Globalization
{
    /// <summary>
    /// Identifies the operating system, or platform, supported by an assembly.
    /// </summary>
    public static class ApplicationLanguages
    {
        /// <summary>
        /// Gets the ranked list of current runtime language values preferred by the user.
        /// </summary>
        public static IReadOnlyList<string> Languages
        {
            get
            {
#if __IOS__
                List<string> langs = new List<string>();
                foreach(string preferredLang in NSLocale.PreferredLanguages)
                {
                    foreach(string ml in ManifestLanguages)
                    {
                        if (ml == ToNetLanguage(preferredLang))
                        {
                            langs.Add(ToNetLanguage(preferredLang));
                        }
                        else if(ml.Substring(0,2) == ToNetLanguage(preferredLang).Substring(0,2))
                        {
                            langs.Add(ToNetLanguage(preferredLang));
                        }
                    }
                }

                return langs.AsReadOnly();
#else
                return null;
#endif
            }
        }

        private static IReadOnlyList<string> s_manifestLanguages;

        public static IReadOnlyList<string> ManifestLanguages
        {
            get
            {
#if __IOS__
                if (s_manifestLanguages == null)
                {
                    List<string> langs = new List<string>();
                    var bundleLangs = NSBundle.MainBundle.InfoDictionary["CFBundleLocalizations"];
                    if (bundleLangs != null)
                    {
                        NSArray arrayLangs = bundleLangs as NSArray;
                        for (global::System.nuint i = 0; i < arrayLangs.Count; i++)
                        {
                            string l = arrayLangs.GetItem<NSString>(i).ToString();
                            langs.Add(ToNetLanguage(l));
                        }
                    }

                    var devRegion = NSBundle.MainBundle.InfoDictionary["CFBundleDevelopmentRegion"];
                    if (devRegion == null)
                    {
                        if (!langs.Contains("en"))
                        {
                            langs.Add("en");
                        }
                    }
                    else
                    {
                        string devRegionString = ToNetLanguage((devRegion as NSString).ToString());
                        if (!langs.Contains(devRegionString))
                        {
                            langs.Add(devRegionString);
                        }
                    }

                    s_manifestLanguages = langs.AsReadOnly();
                }

                return s_manifestLanguages;
#else
                return null;
#endif
            }
        }

        private static string ToNetLanguage(string language)
        {
            var netLanguage = language;
            //certain languages need to be converted to CultureInfo equivalent
            switch (language)
            {
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
                    break;
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    netLanguage = "de-CH"; // closest supported
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }

            return netLanguage;
        }

    }
}
#endif