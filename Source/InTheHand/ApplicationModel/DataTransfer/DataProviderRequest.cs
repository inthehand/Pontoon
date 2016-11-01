//-----------------------------------------------------------------------
// <copyright file="DataProviderRequest.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.DataTransfer.DataProviderRequest))]
//[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.DataTransfer.DataProviderHandler))]
//#else

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// An object of this type is passed to the <see cref="DataProviderHandler"/> delegate. 
    /// </summary>
    public sealed class DataProviderRequest
    {
        private DataPackage _package;

        internal DataProviderRequest(DataPackage package, string formatId)
        {
            _package = package;
            FormatId = formatId;
        }

        /// <summary>
        /// Specifies the format id.
        /// </summary>
        public string FormatId { get; private set; }

        /// <summary>
        /// Sets the content of the DataPackage to be shared with a target app.
        /// </summary>
        /// <param name="value">The object associated with a particular format in the DataPackage.</param>
        public void SetData(object value)
        {
            _package.SetData(FormatId, value);
        }
    }

    /// <summary>
    /// Provides data when the target app requests it, instead of including the data in the DataPackage ahead of time.
    /// DataProviderHandler is used when the source app wants to avoid unnecessary work that is resource intensive, such as performing format conversions.
    /// </summary>
    /// <param name="request">Contains the data that the user wants to share.</param>
    public delegate void DataProviderHandler(DataProviderRequest request);
}
//#endif