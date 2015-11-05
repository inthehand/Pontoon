//-----------------------------------------------------------------------
// <copyright file="DataPackagePropertySetView.cs" company="In The Hand Ltd">
//     Copyright © 2013-15 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Gets the set of properties of a <see cref="DataPackageView"/> object.
    /// </summary>
    public sealed class DataPackagePropertySetView
    {
        private DataPackagePropertySet propertySet;

        internal DataPackagePropertySetView(DataPackagePropertySet propertySet)
        {
            this.propertySet = propertySet;
        }

        /*
        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the app's location in the Windows Phone Store.
        /// </summary>
        public Uri ApplicationListingUri
        {
            get
            {
                return new Uri("http://www.windowsphone.com/s?appid=" + InTheHand.ApplicationModel.Package.Current.Id.ProductId);
            }
        }

        /// <summary>
        /// Gets the name of the app that created the <see cref="DataPackage"/> object. 
        /// </summary>
        public string ApplicationName
        {
            get
            {
                return InTheHand.ApplicationModel.Package.Current.DisplayName;
            }
        }*/

        /// <summary>
        /// Gets the text that displays as a title for the contents of the <see cref="DataPackageView"/> object.
        /// </summary>
        public string Title
        {
            get
            {
                return propertySet.Title;
            }
        }

        /// <summary>
        /// Gets the text that describes the contents of the <see cref="DataPackage"/>.
        /// </summary>
        public string Description
        {
            get
            {
                return propertySet.Description;
            }
        }
    }
}
