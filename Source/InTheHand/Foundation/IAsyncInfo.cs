// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncInfo.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.IAsyncInfo))]
#else

using System;

namespace Windows.Foundation
{
    /// <summary>
    /// Supports asynchronous actions and operations.
    /// </summary>
    public interface IAsyncInfo
    {
        // <summary>
        // Cancels the asynchronous operation.
        // </summary>
        //void Cancel();

        /// <summary>
        /// Closes the asynchronous operation.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets a string that describes an error condition of the asynchronous operation.
        /// </summary>
        Exception ErrorCode
        { get; }

        /// <summary>
        /// Gets the handle of the asynchronous operation.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets a value that indicates the status of the asynchronous operation.
        /// </summary>
        AsyncStatus Status { get; }

    }
}
#endif