//-----------------------------------------------------------------------
// <copyright file="WindowsXamlResourceLoader.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.UI.Xaml.Resources
{
    using global::System;
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Xaml.Resources;
#endif

    [CLSCompliant(false)]
    public sealed class WindowsXamlResourceLoader : CustomXamlResourceLoader
    {
        private ResourceLoader _loader;

        public WindowsXamlResourceLoader()
        {
            _loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
        }

        public WindowsXamlResourceLoader(string baseName)
        {
            _loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse(baseName);
        }

        /// <summary>
        /// Specifies the logic of resource lookup for this CustomXamlResourceLoader. Given a resource ID returns the requested string resource.
        /// </summary>
        /// <param name="resourceId">The string-form key of the resource to get.</param>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        protected override object GetResource(string resourceId, string objectType, string propertyName, string propertyType)
        {
            var s = _loader.GetString(resourceId);

            return string.IsNullOrEmpty(s) ? "{" + resourceId + "}" : s;
        }
    }
}
