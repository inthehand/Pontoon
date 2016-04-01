//-----------------------------------------------------------------------
// <copyright file="Clipboard.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
#if __ANDROID__
using Android.Content;
#elif __IOS__
using Foundation;
using UIKit;
#elif WINDOWS_PHONE_APP || WINDOWS_APP || WINDOWS_UWP
using Windows.ApplicationModel.DataTransfer;
using System.Reflection;
#endif

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Gets and sets information from the clipboard object.
    /// </summary>
    public static class Clipboard
    {
#if __ANDROID__
        static ClipboardManager _clipboardManager = Android.App.Application.Context.GetSystemService(Context.ClipboardService) as ClipboardManager;
#elif WINDOWS_PHONE_APP
        private static bool _on10;
        private static Type _type10;
        static Clipboard()
        {
            //check for 10
            _type10 = Type.GetType("Windows.ApplicationModel.DataTransfer.Clipboard, Windows, ContentType=WindowsRuntime");
            if (_type10 != null)
            {
                _on10 = true;
            }
        }
#endif
        /// <summary>
        /// Removes all data from the Clipboard.
        /// </summary>
        public static void Clear()
        {
#if __ANDROID__
            _clipboardManager.PrimaryClip = null;
#elif __IOS__
            UIPasteboard.General.SetData(null, "kUTTypePlainText");
#elif WINDOWS_UWP || WINDOWS_APP
            Windows.ApplicationModel.DataTransfer.Clipboard.Clear();
#elif WINDOWS_PHONE_APP
            if (_on10)
            {
                _type10.GetRuntimeMethod("Clear", new Type[0]).Invoke(null,new object[0]);
            }
            else
            {
                global::System.Windows.Clipboard.SetText("");
            }
#elif WINDOWS_PHONE
            global::System.Windows.Clipboard.SetText("");
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the current content that is stored in the clipboard object.
        /// </summary>
        /// <returns>Contains the content of the Clipboard.</returns>
        public static DataPackageView GetContent()
        {
#if __ANDROID__
            if(_clipboardManager.HasPrimaryClip)
            {
                DataPackage package = new DataPackage();

                if (_clipboardManager.PrimaryClipDescription != null)
                {
                    package.Properties.Description = _clipboardManager.PrimaryClipDescription.Label;
                }

                ClipData data = _clipboardManager.PrimaryClip;
                for (int i = 0; i < data.ItemCount; i++)
                {
                    ClipData.Item item = data.GetItemAt(i);
                    if (!string.IsNullOrEmpty(item.Text))
                    {
                        package.SetText(item.Text);
                    }
                    else if(item.Uri != null)
                    {
                        package.SetWebLink(new Uri(item.Uri.ToString()));
                    }
                }

                return package.GetView();
            }
#elif __IOS__
            
#elif WINDOWS_APP || WINDOWS_UWP
            Windows.ApplicationModel.DataTransfer.DataPackageView view = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (view != null)
            {
                DataPackage package = new DataPackage();
                if (!string.IsNullOrEmpty(view.Properties.Title))
                {
                    package.Properties.Title = view.Properties.Title;
                }
                if (!string.IsNullOrEmpty(view.Properties.Description))
                {
                    package.Properties.Description = view.Properties.Description;
                }

                foreach (string format in view.AvailableFormats)
                {
                    if (format == StandardDataFormats.ApplicationLink || format == StandardDataFormats.WebLink || format == StandardDataFormats.Text)
                    {
                        Task<object> t = Task.Run<object>(async () => { return await view.GetDataAsync(format); });
                        t.Wait();

                        package.SetData(format, t.Result);
                    }

                }

                return package.GetView();
            }
#else
            throw new PlatformNotSupportedException();
#endif
            return null;
        }

        /// <summary>
        /// Sets the current content that is stored in the clipboard object.
        /// </summary>
        /// <param name="content">Contains the content of the clipboard.
        /// If NULL, the clipboard is emptied.</param>

        public async static void SetContent(DataPackage content)
        {
#if WINDOWS_APP || WINDOWS_UWP
            Windows.ApplicationModel.DataTransfer.DataPackage pkg = null;

            if (content != null)
            {
                pkg.RequestedOperation = DataPackageOperation.Copy;
                if (!string.IsNullOrEmpty(content.Properties.Title))
                {
                    pkg.Properties.Title = content.Properties.Title;
                }
                if (!string.IsNullOrEmpty(content.Properties.Description))
                {
                    pkg.Properties.Description = content.Properties.Description;
                }

                InTheHand.ApplicationModel.DataTransfer.DataPackageView view = content.GetView();
                foreach (string format in view.AvailableFormats)
                {
                    pkg.SetData(format, await view.GetDataAsync(format));
                }
            }

            Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(pkg);
#else
            string text = "";
            Uri uri = null;

            if (content != null)
            {
                DataPackageView view = content.GetView();
                if (view.Contains(StandardDataFormats.Text))
                {

                    text = await view.GetTextAsync();
                }
                if (view.Contains(StandardDataFormats.WebLink))
                {
                    uri = await view.GetWebLinkAsync();
                }
                else if (view.Contains(StandardDataFormats.ApplicationLink))
                {
                    uri = await view.GetApplicationLinkAsync();
                }
            }
#if __ANDROID__
            if (string.IsNullOrEmpty(text) && uri != null)
            {
                _clipboardManager.PrimaryClip = ClipData.NewUri(null, content.Properties.Title, Android.Net.Uri.Parse(uri.ToString()));
            }
            else
            {
                _clipboardManager.PrimaryClip = ClipData.NewPlainText(content.Properties.Title, text);
            }

#elif __IOS__
            UIPasteboard.General.String = text;
            if(uri != null)
            {
                UIPasteboard.General.Url = NSUrl.FromString(uri.ToString());
            }
#elif WINDOWS_PHONE || WINDOWS_PHONE_APP
            if (string.IsNullOrEmpty(text) && uri != null)
            {
                global::System.Windows.Clipboard.SetText(uri.ToString());
            }
            else
            {
                global::System.Windows.Clipboard.SetText(text);
            }
#else
            throw new PlatformNotSupportedException();
#endif
#endif
        }

        /// <summary>
        /// Occurs when the data stored in the Clipboard changes.
        /// </summary>
        public static event EventHandler<object> ContentChanged
        {
            add { }
            remove { }
        }
    }
}

#if WINDOWS_PHONE_APP
namespace System.Windows
{
    internal static class Clipboard
    {
        /// <summary>
        /// For Windows XAML Phone Applications use a separate app to push items to the Clipboard
        /// </summary>
        /// <param name="text"></param>
        public static void SetText(string text)
        {
            global::Windows.System.LauncherOptions options = new global::Windows.System.LauncherOptions();
            options.PreferredApplicationDisplayName = "Clipboarder";
            options.PreferredApplicationPackageFamilyName = "InTheHandLtd.Clipboarder";
            options.DisplayApplicationPicker = false;
            global::Windows.System.Launcher.LaunchUriAsync(new Uri(string.Format("clipboard:Set?Text={0}", Uri.EscapeDataString(text))), options);
        }
    }
}
#endif