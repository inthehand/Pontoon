//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.Android.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Android.App;
using Android.Content;
using Android.Preferences;

using InTheHand.Foundation.Collections;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InTheHand.Storage
{
    public sealed partial class ApplicationDataContainerSettings :
        Java.Lang.Object, ISharedPreferencesOnSharedPreferenceChangeListener, 
        IPropertySet, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IObservableMap<string, object>
    {
        private ISharedPreferences _preferences;

        private void Initialize()
        {
            _preferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
        }

        private void AddMapChanged()
        {
            _preferences.RegisterOnSharedPreferenceChangeListener(this);
        }

        private void RemoveMapChanged()
        {
            _preferences.UnregisterOnSharedPreferenceChangeListener(this);
        }

        void ISharedPreferencesOnSharedPreferenceChangeListener.OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            _mapChanged?.Invoke(this, new ApplicationDataMapChangedEventArgs(key, CollectionChange.Reset));
        }

        private void DoAdd(string key, object value)
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            List<string> pkg = new List<string>();
            pkg.Add(value.GetType().Name);
            pkg.Add(value.ToString());
            editor.PutStringSet(key, pkg);
            editor.Commit();
        }
        
        private bool DoContainsKey(string key)
        {
            object o = null;
            bool success = DoTryGetValue(key, out o);
            return success;
        }

        private ICollection<string> GetKeys()
        {
            ICollection<string> genericKeys = new Collection<string>();

            foreach (KeyValuePair<string, object> entry in _preferences.All)
            {
                genericKeys.Add(entry.Key);
            }

            return genericKeys;
        }

        private bool DoRemove(string key)
        {            
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.Remove(key);
            return editor.Commit();  
        }
        
        private bool DoTryGetValue(string key, out object value)
        {
            ICollection<string> vals = _preferences.GetStringSet(key, new List<string> { "null", "" });
            string type = string.Empty;
            string val = string.Empty;
            foreach (string v in vals)
            {
                if (string.IsNullOrEmpty(type))
                {
                    type = v;
                }
                else
                {
                    val = v;
                    break;
                }
            }

            //todo deserialise type
            switch (type)
            {
                case "null":
                    value = null;
                    return false;

                case "System.Boolean":
                    value = bool.Parse(val);
                    return true;

                case "System.Int32":
                    value = int.Parse(val);
                    return true;

                case "System.Int64":
                    value = long.Parse(val);
                    return true;

                case "System.Single":
                    value = float.Parse(val);
                    return true;

                case "System.DateTimeOffset":
                    value = DateTimeOffset.Parse(val);
                    return true;

                default:
                    value = val;
                    return true;
            }
        }

        private ICollection<object> GetValues()
        {
            Collection<object> genericValues = new Collection<object>();

            foreach (KeyValuePair<string, object> kvp in _preferences.All)
            {
                ICollection<string> rawVal = kvp.Value as ICollection<string>;
                if (rawVal != null)
                {
                    string type = string.Empty;
                    string val = string.Empty;
                    foreach (string v in rawVal)
                    {
                        if (string.IsNullOrEmpty(type))
                        {
                            type = v;
                        }
                        else
                        {
                            val = v;
                            break;
                        }
                    }

                    //todo deserialise type
                    switch (type)
                    {
                        case "System.Boolean":
                            genericValues.Add(bool.Parse(val));
                            break;
                        case "System.Int32":
                            genericValues.Add(int.Parse(val));
                            break;
                        case "System.Int64":
                            genericValues.Add(long.Parse(val));
                            break;
                        case "System.Single":
                            genericValues.Add(float.Parse(val));
                            break;
                        case "System.DateTimeOffset":
                            genericValues.Add(DateTimeOffset.Parse(val));
                            break;
                        default:
                            genericValues.Add(val);
                            break;
                    }
                }
            }

            return genericValues;

        }

        private object GetItem(string key)
        {
            ICollection<string> vals = _preferences.GetStringSet(key, new List<string> { "null", "" });
            string type = string.Empty;
            string val = string.Empty;
            foreach (string v in vals)
            {
                if (string.IsNullOrEmpty(type))
                {
                    type = v;
                }
                else
                {
                    val = v;
                    break;
                }
            }

            //todo deserialise type
            switch (type)
            {
                case "null":
                    return null;

                default:
                    return val;
            }
        }

        private void SetItem(string key, object value)
        {
            DoAdd(key, value);
        }
        
        private void DoClear()
        {
            ISharedPreferencesEditor editor = _preferences.Edit();
            editor.Clear();
            editor.Commit();
        }
        
        private bool DoContains(string key, object value)
        {
            object o = null;

            if (DoTryGetValue(key, out o))
            {
                return value == o;
            }

            return false;
        }

        private int GetCount()
        {
            return _preferences.All.Count;
        }
    }
}