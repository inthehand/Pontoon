//-----------------------------------------------------------------------
// <copyright file="WebAuthenticationBroker.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

using System.Threading;
using System.Threading.Tasks;
#if WINDOWS_PHONE_APP
using Windows.Foundation;
using System.Reflection;
#elif WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
#endif

namespace InTheHand.Security.Authentication.Web
{
    /// <summary>
    /// Starts the authentication operation.
    /// You can call the methods of this class multiple times in a single application or across multiple applications at the same time. 
    /// </summary>
    /// <remarks>
    /// <para/><list type="table">
    /// <listheader><term>Platform</term><description>Version supported</description></listheader>
    /// <item><term>Windows UWP</term><description>Windows 10</description></item>
    /// <item><term>Windows Store</term><description>Windows 8.1 or later</description></item>
    /// <item><term>Windows Phone Store</term><description>Windows 10 Mobile</description></item>
    /// <item><term>Windows Phone Silverlight</term><description>Windows Phone 8.0 or later</description></item></list>
    /// </remarks>
    public static class WebAuthenticationBroker
    {
#if WINDOWS_PHONE_APP
        private static Type type10 = Type.GetType("Windows.Security.Authentication.Web.WebAuthenticationBroker, Windows, ContentType=WindowsRuntime");
#elif WINDOWS_PHONE
        internal static WebAuthenticationResult Result;
        internal static AutoResetEvent Handle;

        private async static Task WaitResponse()
        {
            Handle.WaitOne();
        }
#endif

        /// <summary>
        /// Starts the asynchronous authentication operation with two inputs.
        /// You can call this method multiple times in a single application or across multiple applications at the same time. 
        /// </summary>
        /// <param name="options">The options for the authentication operation.</param>
        /// <param name="requestUri">The starting URI of the web service. This URI must be a secure address of https://.</param>
        /// <returns>The way to query the status and get the results of the authentication operation.
        /// If you are getting an invalid parameter error, the most common cause is that you are not using HTTPS for the requestUri parameter.</returns>
        /// <remarks>There is no explicit callbackUri parameter in this method.
        /// The application's default URI is used internally as the terminator.
        /// For more information, see <see cref="GetCurrentApplicationCallbackUri"/>.
        /// This method must be called on the UI thread.
        /// When this method is used, session state or persisted cookies are retained across multiple calls from the same or different apps.</remarks>
        public static async Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, Uri requestUri)
        {
#if WINDOWS_UWP || WINDOWS_APP
            return await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync((Windows.Security.Authentication.Web.WebAuthenticationOptions)((int)options), requestUri);
#elif WINDOWS_PHONE_APP
            var method = type10.GetRuntimeMethod("AuthenticateAsync", new Type[] { typeof(Windows.Security.Authentication.Web.WebAuthenticationOptions), typeof(Uri) });
            if(method != null)
            {
                IAsyncOperation<Windows.Security.Authentication.Web.WebAuthenticationResult> op = (IAsyncOperation<Windows.Security.Authentication.Web.WebAuthenticationResult>)method.Invoke(null, new object[] { (Windows.Security.Authentication.Web.WebAuthenticationOptions)((int)options), requestUri });
                return await op;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
#elif WINDOWS_PHONE
            return await AuthenticateAsync(options, requestUri, GetCurrentApplicationCallbackUri());
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Starts the asynchronous authentication operation with three inputs.
        /// You can call this method multiple times in a single application or across multiple applications at the same time. 
        /// </summary>
        /// <param name="options">The options for the authentication operation.</param>
        /// <param name="requestUri">The starting URI of the web service. This URI must be a secure address of https://.</param>
        /// <param name="callbackUri">The callback URI that indicates the completion of the web authentication.
        /// The broker matches this URI against every URI that it is about to navigate to.
        /// The broker never navigates to this URI, instead the broker returns the control back to the application when the user clicks a link or a web server redirection is made.</param>
        /// <returns>The way to query the status and get the results of the authentication operation.
        /// If you are getting an invalid parameter error, the most common cause is that you are not using HTTPS for the requestUri parameter.</returns>
        /// <remarks>When this method is used, no session state or persisted cookies are retained across multiple calls from the same or different apps.
        /// This method must be called on the UI thread.</remarks>
        public static async Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, Uri requestUri, Uri callbackUri)
        {
#if WINDOWS_UWP || WINDOWS_APP
            return await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync((Windows.Security.Authentication.Web.WebAuthenticationOptions)((int)options), requestUri, callbackUri);
#elif WINDOWS_PHONE_APP
            var method = type10.GetRuntimeMethod("AuthenticateAsync", new Type[] { typeof(Windows.Security.Authentication.Web.WebAuthenticationOptions), typeof(Uri), typeof(Uri) });
            if (method != null)
            {
                IAsyncOperation<Windows.Security.Authentication.Web.WebAuthenticationResult> op = (IAsyncOperation<Windows.Security.Authentication.Web.WebAuthenticationResult>)method.Invoke(null, new object[] { (Windows.Security.Authentication.Web.WebAuthenticationOptions)((int)options), requestUri, callbackUri });
                var result = await op;
                return result;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
#elif WINDOWS_PHONE
            Result = new WebAuthenticationResult(null, 0, WebAuthenticationStatus.UserCancel);

            Frame f = Application.Current.RootVisual as Frame;
            if (f != null)
            {
                if (!f.Dispatcher.CheckAccess())
                {
                    throw new InvalidOperationException("Must be called on UI thread.");
                }

                Handle = new AutoResetEvent(false);

                Uri navUri = new Uri(string.Format("/InTheHand;component/Security/Authentication/Web/WebAuthenticationPage.xaml?uri={0}&returnUri={1}", Uri.EscapeDataString(requestUri.ToString()), Uri.EscapeDataString(callbackUri.ToString())), UriKind.Relative);

                f.Navigate(navUri);
                
                await Task.Run(new Func<Task>(WaitResponse));
            }

            return Result;
#else
            throw new PlatformNotSupportedException();
#endif
        }

        /// <summary>
        /// Gets the current application callback URI.
        /// </summary>
        /// <returns>The URI of the current application.</returns>
        /// <remarks>The current application callback URI is used as an implicit value of the callbackUri parameter of the AuthenticateAsync method.
        /// However, applications need the URI value to add it to the request URI as required by the online provider.</remarks>
        public static Uri GetCurrentApplicationCallbackUri()
        {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
            return Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
#else
            return new Uri("ms-app://" + InTheHand.ApplicationModel.Package.Current.Id.ProductId);
#endif
        }
    }
}