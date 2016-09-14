//-----------------------------------------------------------------------
// <copyright file="DataPackageView.cs" company="In The Hand Ltd">
//     Copyright © 2013-16 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_PHONE_81
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.ApplicationModel.DataTransfer.DataPackageView))]
#else

using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::System.Collections.ObjectModel;
using Windows.Foundation;
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.Foundation;
#endif

namespace Windows.ApplicationModel.DataTransfer
{
    /// <summary>
    /// A read-only version of a <see cref="DataPackage"/>.
    /// Apps that receive shared content get this object when acquiring content.
    /// </summary>
    public sealed class DataPackageView
    {
        private DataPackage package;

        internal DataPackageView(DataPackage package)
        {
            this.package = package;
            this.Properties = new DataPackagePropertySetView(package.Properties);
        }

        /// <summary>
        /// Returns the formats the <see cref="DataPackageView"/> contains.
        /// </summary>
        public IReadOnlyList<string> AvailableFormats
        {
            get
            {
                return new ReadOnlyCollection<string>(package.data.Keys.ToArray<string>());
            }
        }

        /// <summary>
        /// Gets a <see cref="DataPackagePropertySetView"/> object, which contains a read-only set of properties for the data in the <see cref="DataPackageView"/> object.
        /// </summary>
        public DataPackagePropertySetView Properties
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks to see if the <see cref="DataPackageView"/> contains a specific data format.
        /// </summary>
        /// <param name="formatId">The name of the format.</param>
        /// <returns>True if the <see cref="DataPackageView"/> contains the format; false otherwise.</returns>
        public bool Contains(string formatId)
        {
            return AvailableFormats.Contains<string>(formatId);
        }

        /// <summary>
        /// Gets the data contained in the <see cref="DataPackageView"/>.
        /// </summary>
        /// <param name="formatId">The format of the data.</param>
        /// <returns>The data.</returns>
        public IAsyncOperation<object> GetDataAsync(string formatId)
        {
            return Task.Run<object>(() => 
            {
                if (package.data.ContainsKey(formatId))
                {
                    DataProviderHandler handler = package.data[formatId] as DataProviderHandler;

                    if (handler != null)
                    {
                        DataProviderRequest request = new DataProviderRequest(package, formatId);
                        handler.Invoke(request);
                    }
                    else
                    {
                        return package.data[formatId];
                    }
                }

                return null;
            }).AsAsyncOperation<object>();
        }

        /// <summary>
        /// Gets the text in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<string> GetTextAsync()
        {
            return Task.Run<string>(() =>
            {
                if (package.data.ContainsKey(StandardDataFormats.Text))
                {
                    DataProviderHandler handler = package.data[StandardDataFormats.Text] as DataProviderHandler;

                    if (handler != null)
                    {
                        DataProviderRequest request = new DataProviderRequest(package, StandardDataFormats.Text);
                        handler.Invoke(request);
                    }

                    return package.data[StandardDataFormats.Text].ToString();
                }

                return null;
            }).AsAsyncOperation<string>();
        }

        private Uri GetUriClr(string format)
        {
            if (package.data.ContainsKey(format))
            {
                DataProviderHandler handler = package.data[format] as DataProviderHandler;

                if (handler != null)
                {
                    DataProviderRequest request = new DataProviderRequest(package, format);
                    handler.Invoke(request);
                }

                return (Uri)package.data[format];
            }

            return null;
        }

        /// <summary>
        /// Gets the application link in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<Uri> GetApplicationLinkAsync()
        {
            return Task.Run<Uri>(() =>
            {
                return GetUriClr(StandardDataFormats.ApplicationLink);
            }).AsAsyncOperation<Uri>();
        }

        /// <summary>
        /// Gets the web link in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public IAsyncOperation<Uri> GetWebLinkAsync()
        {
            return Task.Run<Uri>(() =>
            {
                return GetUriClr(StandardDataFormats.WebLink);
            }).AsAsyncOperation<Uri>();
        }
    }
}
#endif