// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedEventHandler.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE
[assembly:global::System.Runtime.CompilerServices.TypeForwardedTo(typeof(Windows.Foundation.TypedEventHandler<,>))]
#else

namespace Windows.Foundation
{
    /// <summary>
    /// Represents a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender">The event source.</typeparam>
    /// <typeparam name="TResult">The event data. If there is no event data, this parameter will be null.</typeparam>
    /// <param name="sender">The event source.</param>
    /// <param name="args">The event data. If there is no event data, this parameter will be null.</param>
    public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);
}
#endif