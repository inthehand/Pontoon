//-----------------------------------------------------------------------
// <copyright file="DataRequestedEventArgs.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Contains information about the DataRequested event.
    /// The system fires this event when the user invokes the Share UI.
    /// </summary>
    public sealed class DataRequestedEventArgs
    {
        internal DataRequestedEventArgs()
        {
            this.Request = new DataRequest();   
        }

        /// <summary>
        /// Enables you to get the DataRequest object and either give it data or a failure message.
        /// </summary>
        public DataRequest Request { get; private set; }
    }

    /// <summary>
    /// Lets your app supply the content the user wants to share or specify a message, if an error occurs.
    /// </summary>
    public sealed class DataRequest
    {
        internal DataRequest()
        {
            this.Data = new DataPackage();
        }

        /// <summary>
        /// Sets or gets a DataPackage object that contains the content a user wants to share.
        /// </summary>
        public DataPackage Data { get; set; }
    }
}
