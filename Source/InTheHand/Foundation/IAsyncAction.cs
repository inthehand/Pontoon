// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAsyncAction.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Windows.Foundation
{
    /// <summary>
    /// Represents an asynchronous action.
    /// This is the return type for many Windows Runtime asynchronous methods that don't have a result object, and don't report ongoing progress.
    /// </summary>
    public interface IAsyncAction
    {
        /// <summary>
        /// Returns the results of the action.
        /// </summary>
        void GetResults();

        /// <summary>
        /// Gets or sets the method that handles the action completed notification.
        /// </summary>
        AsyncActionCompletedHandler Completed { get; set; }
    }

    /// <summary>
    /// Represents a method that handles the completed event of an asynchronous action.
    /// </summary>
    /// <param name="asyncInfo">The asynchronous action.</param>
    /// <param name="asyncStatus">One of the enumeration values.</param>
    public delegate void AsyncActionCompletedHandler(IAsyncAction asyncInfo, AsyncStatus asyncStatus);

    /// <summary>
    /// Specifies the status of an asynchronous operation.
    /// </summary>
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