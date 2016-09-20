// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageDialogAsyncOperation.cs" company="In The Hand Ltd">
//   Copyright (c) 2012-16 In The Hand Ltd, All rights reserved.
// </copyright>
// <summary>
//   Provides information about a message dialog's asynchronous operation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Windows.Foundation;
using Windows.UI.Popups;

namespace Windows.UI.Popups
{
    /// <summary>
    /// Provides information about a message dialog's asynchronous operation.
    /// </summary>
    // [CLSCompliant(false)]
    internal sealed class MessageDialogAsyncOperation : IAsyncOperation<IUICommand>, IAsyncInfo
    {
        private static uint s_id = 0;

        private MessageDialog owner;
 
        private IUICommand command;

        private uint id;
        
        internal MessageDialogAsyncOperation(MessageDialog owner)
        {
            this.id = s_id++;
            this.owner = owner;
        }

        #region IAsyncOperation<IUICommand> Members
        /// <summary>
        /// Gets or sets the handler for the event that is raised when a message box's asynchronous operation is completed.
        /// </summary>
        public AsyncOperationCompletedHandler<IUICommand> Completed
        {
            get;
            set;
        }

        private void OnCompleted()
        {
            if (Completed != null)
            {
                Completed(this, AsyncStatus.Completed);
            }
        }

        /// <summary>
        /// Gets the command that the user selected from a message dialog during an asynchronous operation.
        /// </summary>
        /// <returns>The command that the user selected.</returns>
        public IUICommand GetResults()
        {
            if (this.Status != AsyncStatus.Completed)
            {
                throw new InvalidOperationException();
            }

            return this.command;
        }

        internal void SetResults(IUICommand command)
        {
            this.status = AsyncStatus.Completed;
            this.command = command;
            this.OnCompleted();
        }
        #endregion

        #region IAsyncInfo Members
        /// <summary>
        /// Cancels the asynchronous operation.
        /// </summary>
        public void Cancel()
        {
            status = AsyncStatus.Canceled;
        }

        /// <summary>
        /// Closes the asynchronous operation.
        /// </summary>
        public void Close()
        {          
        }

        /// <summary>
        /// Gets the error code that is associated with an unsuccessful operation, if any.
        /// </summary>
        public Exception ErrorCode
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the identifier of the operation.
        /// </summary>
        public uint Id
        {
            get { return id; }
        }

        /// <summary>
        /// Starts the asynchronous operation.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }

        private AsyncStatus status = AsyncStatus.Started;

        /// <summary>
        /// Gets the status of the asynchronous operation.
        /// </summary>
        public AsyncStatus Status
        {
            get
            {
                return status;
            }
        }
        #endregion
    }
}
