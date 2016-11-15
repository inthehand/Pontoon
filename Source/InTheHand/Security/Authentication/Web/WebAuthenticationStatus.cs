//-----------------------------------------------------------------------
// <copyright file="WebAuthenticationStatus.cs" company="In The Hand Ltd">
//     Copyright © 2014-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.Security.Authentication.Web
{
    /// <summary>
    /// Contains the status of the asynchronous operation.
    /// </summary>
    public enum WebAuthenticationStatus
    {
        /// <summary>
        /// The operation succeeded, and the response data is available.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The operation was canceled by the user.
        /// </summary>
        UserCancel = 1,

        /// <summary>
        /// The operation failed because a specific HTTP error was returned, for example 404.
        /// </summary>
        ErrorHttp = 2,
    }
}