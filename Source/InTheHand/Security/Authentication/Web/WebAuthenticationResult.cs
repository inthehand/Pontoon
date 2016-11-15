//-----------------------------------------------------------------------
// <copyright file="WebAuthenticationResult.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Security.Authentication.Web
{
    /// <summary>
    /// Indicates the result of the authentication operation.
    /// </summary>
    public sealed class WebAuthenticationResult
    {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
        private Windows.Security.Authentication.Web.WebAuthenticationResult _result;

        public static implicit operator Windows.Security.Authentication.Web.WebAuthenticationResult(WebAuthenticationResult result)
        {
            return result._result;
        }

        public static implicit operator WebAuthenticationResult(Windows.Security.Authentication.Web.WebAuthenticationResult result)
        {
            return new WebAuthenticationResult(result);
        }

        private WebAuthenticationResult(Windows.Security.Authentication.Web.WebAuthenticationResult result)
        {
            _result = result;
        }
#else
        private string _responseData;
        private uint _responseErrorDetail;
        private WebAuthenticationStatus _responseStatus;

        internal WebAuthenticationResult(string responseData, uint errorDetail, WebAuthenticationStatus status)
        {
            _responseData = responseData;
            _responseErrorDetail = errorDetail;
            _responseStatus = status;
        }
#endif
        /// <summary>
        /// Contains the protocol data when the operation successfully completes.
        /// </summary>
        public string ResponseData
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _result.ResponseData;
#else
                return _responseData;
#endif
            }
        }

        /// <summary>
        /// Returns the HTTP error code when ResponseStatus is equal to WebAuthenticationStatus.ErrorHttp.
        /// This is only available if there is an error.
        /// </summary>
        /// <value>The specific HTTP error, for example 400.</value>
        public uint ResponseErrorDetail
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return _result.ResponseErrorDetail;
#else
                return _responseErrorDetail;
#endif
            }
        }

        /// <summary>
        /// Contains the status of the asynchronous operation when it completes.
        /// </summary>
        public WebAuthenticationStatus ResponseStatus
        {
            get
            {
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP
                return (WebAuthenticationStatus)((int)_result.ResponseStatus);
#else
                return _responseStatus;
#endif
            }
        }
    }
}
