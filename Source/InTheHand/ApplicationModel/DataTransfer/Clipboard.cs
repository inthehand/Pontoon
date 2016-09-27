//-----------------------------------------------------------------------
// <copyright file="Clipboard.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.DataTransfer.Clipboard))]
#else
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.Foundation;
#if __ANDROID__
using Android.Content;
#elif __IOS__
using Foundation;
using UIKit;
#elif WINDOWS_PHONE_APP
using System.Reflection;
#endif

namespace Windows.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Gets and sets information from the clipboard object.
    /// </summary>
    public static partial class Clipboard
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
#elif WIN32
            EmptyClipboard();
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
            if(UIPasteboard.General.Count > 0)
            {
                DataPackage package = new DataPackage();
                var str = UIPasteboard.General.String;
                if(!string.IsNullOrEmpty(str))
                {
                    package.SetText(str);
                }
                var url = UIPasteboard.General.Url;
                if(url != null)
                {
                    package.SetWebLink(new Uri(url.ToString()));
                }

                return package.GetView();
            }
#elif WINDOWS_PHONE_APP
            if(_on10)
            {
                var dpv = _type10.GetRuntimeMethod("GetContent", new Type[0]).Invoke(null, new object[0]) as Windows.ApplicationModel.DataTransfer.DataPackageView;
                if(dpv != null)
                {
                    DataPackage package = new DataPackage();
                    
                    if (!string.IsNullOrEmpty(dpv.Properties.Title))
                    {
                        package.Properties.Title = dpv.Properties.Title;
                    }
                    if (!string.IsNullOrEmpty(dpv.Properties.Description))
                    {
                        package.Properties.Description = dpv.Properties.Description;
                    }
                    
                    foreach (string format in dpv.AvailableFormats)
                    {
                        if (format == StandardDataFormats.ApplicationLink || format == StandardDataFormats.WebLink || format == StandardDataFormats.Text)
                        {
                            Task<object> t = Task.Run<object>(async () =>
                            {
                                return await dpv.GetDataAsync(format);
                            });
                            t.Wait();

                            package.SetData(format, t.Result);
                        }

                    }

                    return package.GetView();
                }
            }
#elif WIN32
            string txt = GetText();
            if(!string.IsNullOrEmpty(txt))
            {
                DataPackage package = new DataPackage();
                package.SetText(txt);
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
#if WINDOWS_PHONE_APP
            if (_on10)
            {
                _type10.GetRuntimeMethod("SetContent", new Type[] { typeof(Windows.ApplicationModel.DataTransfer.DataPackage) }).Invoke(null, new object[] { content });
                ContentChanged?.Invoke(null, null);
                return;
            }
#endif
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
            else
            {
                Clear();
                return;
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
#elif WIN32
            if (string.IsNullOrEmpty(text) && uri != null)
            {
                SetText(uri.ToString());
            }
            else
            {
                SetText(text);
            }
#else
            throw new PlatformNotSupportedException();
#endif
            ContentChanged?.Invoke(null, null);
        }

        /// <summary>
        /// Occurs when the data stored in the Clipboard changes.
        /// </summary>
        public static event EventHandler<object> ContentChanged;
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
#endif