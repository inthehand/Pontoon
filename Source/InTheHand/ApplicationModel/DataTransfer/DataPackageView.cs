//-----------------------------------------------------------------------
// <copyright file="DataPackageView.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// A read-only version of a <see cref="DataPackage"/>.
    /// Apps that receive shared content get this object when acquiring content.
    /// </summary>
    public sealed class DataPackageView
    {
        private DataPackage _package;

        internal DataPackageView(DataPackage package)
        {
            _package = package;
            Properties = new DataPackagePropertySetView(package.Properties);
        }

        /// <summary>
        /// Returns the formats the <see cref="DataPackageView"/> contains.
        /// </summary>
        public IReadOnlyList<string> AvailableFormats
        {
            get
            {
                return new ReadOnlyCollection<string>(_package._data.Keys.ToArray<string>());
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
        public Task<object> GetDataAsync(string formatId)
        {
            return Task.Run<object>(() => 
            {
                if (_package._data.ContainsKey(formatId))
                {
                    DataProviderHandler handler = _package._data[formatId] as DataProviderHandler;

                    if (handler != null)
                    {
                        DataProviderRequest request = new DataProviderRequest(_package, formatId);
                        handler.Invoke(request);
                    }
                    else
                    {
                        return _package._data[formatId];
                    }
                }

                return null;
            });
        }

        /// <summary>
        /// Gets the text in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public Task<string> GetTextAsync()
        {
            return Task.Run<string>(() =>
            {
                if (_package._data.ContainsKey(StandardDataFormats.Text))
                {
                    DataProviderHandler handler = _package._data[StandardDataFormats.Text] as DataProviderHandler;

                    if (handler != null)
                    {
                        DataProviderRequest request = new DataProviderRequest(_package, StandardDataFormats.Text);
                        handler.Invoke(request);
                    }

                    return _package._data[StandardDataFormats.Text].ToString();
                }

                return null;
            });
        }

        private Uri GetUri(string format)
        {
            if (_package._data.ContainsKey(format))
            {
                DataProviderHandler handler = _package._data[format] as DataProviderHandler;

                if (handler != null)
                {
                    DataProviderRequest request = new DataProviderRequest(_package, format);
                    handler.Invoke(request);
                }

                return (Uri)_package._data[format];
            }

            return null;
        }

        /// <summary>
        /// Gets the application link in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public Task<Uri> GetApplicationLinkAsync()
        {
            return Task.Run<Uri>(() =>
            {
                return GetUri(StandardDataFormats.ApplicationLink);
            });
        }

        /// <summary>
        /// Gets the web link in the <see cref="DataPackageView"/> object.
        /// </summary>
        /// <returns></returns>
        public Task<Uri> GetWebLinkAsync()
        {
            return Task.Run<Uri>(() =>
            {
                return GetUri(StandardDataFormats.WebLink);
            });
        }
    }
}