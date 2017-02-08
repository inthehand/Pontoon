//-----------------------------------------------------------------------
// <copyright file="DataPackage.cs" company="In The Hand Ltd">
//     Copyright © 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace InTheHand.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Contains the data that a user wants to exchange with another app.
    /// </summary>
    public sealed class DataPackage
    {
        internal Dictionary<string, object> _data;

        /// <summary>
        /// Constructor that creates a new <see cref="DataPackage"/>.
        /// </summary>
        public DataPackage()
        {
            _data = new Dictionary<string, object>();
            Properties = new DataPackagePropertySet();
        }

        /// <summary>
        /// Allows you to get and set properties like the title of the content being shared.
        /// </summary>
        public DataPackagePropertySet Properties { get; private set; }

        /// <summary>
        /// Returns a <see cref="DataPackageView"/> object.
        /// This object is a read-only copy of the <see cref="DataPackage"/> object.
        /// </summary>
        /// <returns>The object that is a read-only copy of the <see cref="DataPackage"/> object.</returns>
        public DataPackageView GetView()
        {
            return new DataPackageView((DataPackage)MemberwiseClone());
        }

        /// <summary>
        /// Sets the data contained in the DataPackage.
        /// </summary>
        /// <param name="formatId">Specifies the format of the data.
        /// We recommend that you set this value by using the StandardDataFormats class.</param>
        /// <param name="value">Specifies the content that the DataPackage contains.</param>
        public void SetData(string formatId, object value)
        {
            if (_data.ContainsKey(formatId))
            {
                _data[formatId] = value;
            }
            else
            {
                _data.Add(formatId, value);
            }
        }

        /// <summary>
        /// Sets a delegate to handle requests from the target app.
        /// </summary>
        /// <param name="formatId">Specifies the format of the data.
        /// We recommend that you set this value by using the StandardDataFormats class.</param>
        /// <param name="delayRenderer">A delegate that is responsible for processing requests from a target app.</param>
        public void SetDataProvider(string formatId, DataProviderHandler delayRenderer)
        {
            if (_data.ContainsKey(formatId))
            {
                _data[formatId] = delayRenderer;
            }
            else
            {
                _data.Add(formatId, delayRenderer);
            }
        }

        /// <summary>
        /// Sets the text that a <see cref="DataPackage"/> contains.
        /// </summary>
        /// <param name="value">The text.</param>
        public void SetText(string value)
        {
            if (_data.ContainsKey(StandardDataFormats.Text))
            {
                _data[StandardDataFormats.Text] = value;
            }
            else
            {
                _data.Add(StandardDataFormats.Text, value);
            }
        }
        
        /// <summary>
        /// Sets the web link that a <see cref="DataPackage"/> contains.
        /// </summary>
        /// <param name="value">A URI with an http or https scheme that corresponds to the content being displayed to the user.</param>
        /// <remarks>Whenever possible, you should set this property.
        /// A source app provides a value for this property, and a target app reads the value.
        /// Use this property to indicate the source of the shared content.</remarks>
        public void SetWebLink(Uri value)
        {
            if (_data.ContainsKey(StandardDataFormats.WebLink))
            {
                _data[StandardDataFormats.WebLink] = value;
            }
            else
            {
                _data.Add(StandardDataFormats.WebLink, value);
            }
        }

        /// <summary>
        /// Sets the application link that a <see cref="DataPackage"/> contains.
        /// </summary>
        /// <param name="value">A URI with a scheme that isn't http or https that's handled by the source app. </param>
        /// <remarks>Whenever possible, you should set this property.
        /// This URI represents a deep link that takes the user back to the currently displayed content.
        /// A source app provides a value for this property, and a target app reads the value.
        /// Use this property to indicate the source of the shared content. 
        /// <para>The scheme of this URI must not be http or https.
        /// The app sharing this URI must be capable of being the default handler, although it may not be set as the default handler.</para></remarks>
        public void SetApplicationLink(Uri value)
        {
            if (_data.ContainsKey(StandardDataFormats.ApplicationLink))
            {
                _data[StandardDataFormats.ApplicationLink] = value;
            }
            else
            {
                _data.Add(StandardDataFormats.ApplicationLink, value);
            }
        }
    }
}