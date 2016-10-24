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
using System.Collections.ObjectModel;

#if __IOS__
using Foundation;
#endif

namespace Windows.Globalization
{
    /// <summary>
    /// Specifies the language-related preferences that the app can use and maintain.
    /// </summary>
    public static class ApplicationLanguages
    {
        /// <summary>
        /// Gets the ranked list of current runtime language values preferred by the user.
        /// </summary>
        /// <value>A computed list of languages that merges the app's declared supported languages (<see cref="ManifestLanguages"/>) with the user's ranked list of preferred languages.</value>
        public static IReadOnlyList<string> Languages
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return Windows.Globalization.ApplicationLanguages.Languages;
#else
                List<string> langs = new List<string>();
#if __ANDROID__
                // for now just return the single default system locale
                langs.Add(ToNetLanguage(Java.Util.Locale.Default.ToString().Replace("_", "-")));
#elif __IOS__
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

#endif
                return langs.AsReadOnly();
#endif
            }
        }

        private static IReadOnlyList<string> s_manifestLanguages;

        /// <summary>
        /// Gets the app's declared list of supported languages.
        /// </summary>
        /// <value>The list of supported languages declared in the app's manifest.</value>
        public static IReadOnlyList<string> ManifestLanguages
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return Windows.Globalization.ApplicationLanguages.ManifestLanguages;
#else
                if (s_manifestLanguages == null)
                {
                    List<string> langs = new List<string>();
#if __ANDROID__

#elif __IOS__
                
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

#endif
                    s_manifestLanguages = langs.AsReadOnly();
                }

                return s_manifestLanguages;

#endif
            }
        }

        private static string ToNetLanguage(string language)
        {
            var netLanguage = language;
            //certain languages need to be converted to CultureInfo equivalent
            switch (language)
            {
                case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
                    break;
                case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET
                    netLanguage = "id-ID"; // correct code for .NET
                    break;
                case "gsw":
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