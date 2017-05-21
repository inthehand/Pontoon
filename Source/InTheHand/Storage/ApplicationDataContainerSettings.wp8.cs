//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.wp8.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Foundation.Collections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace InTheHand.Storage
{
    partial class ApplicationDataContainerSettings :
        IPropertySet, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IObservableMap<string, object>
    {
        internal void Initialize()
        {
            _applicationSettings = global::System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += Current_Deactivated;
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Closing += Current_Closing;

        }
        
        private IsolatedStorageSettings _applicationSettings;

        void rootFrame_Navigating(object sender, global::System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            _applicationSettings.Save();
        }

        void Current_Deactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            _applicationSettings.Save();
        }

        void Current_Closing(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            _applicationSettings.Save();
        }

        private void DoAdd(string key, object value)
        {
            if (value is DateTimeOffset)
            {
                DateTimeOffset offset = (DateTimeOffset)value;
                value = offset.UtcDateTime;
            }

            _applicationSettings.Add(key, value);
        }

        private bool DoContainsKey(string key)
        {
            return _applicationSettings.Contains(key);
        }

        private bool DoContains(string key, object value)
        {
            if(DoContainsKey(key))
            {
                if(GetItem(key) == value)
                {
                    return true;
                }
            }

            return false;
        }

        private ICollection<string> GetKeys()
        {
                ICollection<string> genericKeys = new Collection<string>();

                foreach (string key in _applicationSettings.Keys)
                {
                    genericKeys.Add(key);
                }

                return genericKeys;
        }
        
        private bool DoRemove(string key)
        {
            return _applicationSettings.Remove(key);
        }
        
        private bool DoTryGetValue(string key, out object value)
        {
            return _applicationSettings.TryGetValue<object>(key, out value);
        }

        private ICollection<object> GetValues()
        {
            Collection<object> genericValues = new Collection<object>();

            foreach (object value in _applicationSettings.Values)
            {
                genericValues.Add(value);
            }

            return genericValues;
        }

        private object GetItem(string key)
        {

            object value = _applicationSettings[key];
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                value = new DateTimeOffset(dateTime);
            }

            return value;
        }

        private void SetItem(string key, object value)
        {
            // temporary workaround while investigating datetimeoffset behaviour in isostore
            if (value is DateTimeOffset)
            {
                DateTimeOffset offset = (DateTimeOffset)value;
                value = offset.UtcDateTime;
            }

            if (_applicationSettings.Contains(key))
            {
                if (_applicationSettings[key] != value)
                {
                    _applicationSettings[key] = value;
                }
            }
            else
            {
                // if not present add a new value (matches RT behaviour)
                DoAdd(key, value);
            }
        }

        private void DoClear()
        {
            _applicationSettings.Clear();
        }

        private int GetCount()
        {
            return _applicationSettings.Count;
        }

        private IEnumerator<KeyValuePair<string, object>> DoGetEnumerator()
        {
            return new ApplicationDataContainerEnumerator();
        }
    }
    
    internal sealed class ApplicationDataContainerEnumerator : IEnumerator<KeyValuePair<string, object>>
    {

        private IsolatedStorageSettings _settings;
        private IEnumerator _keyEnumerator;

        internal ApplicationDataContainerEnumerator()
        {
            _settings = IsolatedStorageSettings.ApplicationSettings;
            _keyEnumerator = _settings.Keys.GetEnumerator();
        }

        public KeyValuePair<string, object> Current
        {
            get {
                object val = _settings[_keyEnumerator.Current.ToString()];
                return new KeyValuePair<string, object>(_keyEnumerator.Current.ToString(), val);
            }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return _keyEnumerator.MoveNext();
        }

        public void Reset()
        {
            _keyEnumerator.Reset();
        }

        public void Dispose()
        {
            _keyEnumerator.Reset();
        }
    }
    
}