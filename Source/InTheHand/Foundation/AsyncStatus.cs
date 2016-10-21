// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncStatus.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Foundation.AsyncStatus))]
//#else

namespace InTheHand.Foundation
{
    /// <summary>
    /// Specifies the status of an asynchronous operation.
    /// </summary>
    //[ContractVersion(typeof(FoundationContract), 65536u)]
	public enum AsyncStatus
    {
        /// <summary>
        /// The operation has started.
        /// </summary>
        Started = 0,

        /// <summary>
        /// The operation has completed.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// The operation was canceled.
        /// </summary>
        Canceled = 2,

        /// <summary>
        /// The operation has encountered an error.
        /// </summary>
        Error = 3,
    }
}
//#endif