//-----------------------------------------------------------------------
// <copyright file="UserPermission.cs" company="In The Hand Ltd">
//     Copyright © 2013-14 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WINDOWS_PHONE && !WINDOWS_PHONE_81
using InTheHand.Storage;
#else
using Windows.Storage;
#endif

namespace InTheHand.UI.ApplicationSettings
{
    public sealed class UserPermission : INotifyPropertyChanged
    {
        private static UserPermission instance;

        public static UserPermission Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new UserPermission();
                }

                return instance;
            }
        }

        private UserPermission()
        {
            // load saved value
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("InTheHand.UI.ApplicationSettings.UserPermission.ToastNotifications"))
            {
                allowToastNotifications = (bool)ApplicationData.Current.LocalSettings.Values["InTheHand.UI.ApplicationSettings.UserPermission.ToastNotifications"];
            }
        }

        private bool allowToastNotifications = true;
        /// <summary>
        /// Gets a value that indicates if the user has enabled toast notifications.
        /// </summary>
        /// <value>true if user has enabled toast notifications; else false.
        /// Default value is true.</value>
        public bool AllowToastNotifications
        {
            get
            {
                return allowToastNotifications;
            }
            internal set
            {
                if(allowToastNotifications != value)
                {
                    allowToastNotifications = value;

                    // save value
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey("InTheHand.UI.ApplicationSettings.UserPermission.ToastNotifications"))
                    {
                        ApplicationData.Current.LocalSettings.Values["InTheHand.UI.ApplicationSettings.UserPermission.ToastNotifications"] = allowToastNotifications;
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values.Add("InTheHand.UI.ApplicationSettings.UserPermission.ToastNotifications", allowToastNotifications);
                    }

                    OnPropertyChanged("AllowToastNotifications");
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
