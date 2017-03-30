//-----------------------------------------------------------------------
// <copyright file="ApplicationDataContainerSettings.iOS.cs" company="In The Hand Ltd">
//     Copyright (c) 2013-17 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Foundation;

using InTheHand.Foundation.Collections;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace InTheHand.Storage
{
    public sealed partial class ApplicationDataContainerSettings :
        IPropertySet, IDictionary<string, object>, IEnumerable<KeyValuePair<string, object>>, IObservableMap<string, object>
    {

        private NSUserDefaults _defaults;
        private NSUbiquitousKeyValueStore _store;

        private NSObject _observer;

        private void Initialize(string name)
        {
            switch (_locality)
            {
                case ApplicationDataLocality.Roaming:
                    _store = NSUbiquitousKeyValueStore.DefaultStore;
                    break;

                case ApplicationDataLocality.SharedLocal:
                    _defaults = new NSUserDefaults(name, NSUserDefaultsType.SuiteName);
                    if(_defaults == null)
                    {
                        throw new ArgumentException("name");
                    }
                    break;

                default:
                    _defaults = NSUserDefaults.StandardUserDefaults;
                    break;
            }
        }

        private void AddMapChanged()
        {
            if (!IsRoaming)
            {
                _observer = NSNotificationCenter.DefaultCenter.AddObserver(new NSString("NSUserDefaultsDidChangeNotification"), (n) =>
                {
                    // indicate a reset change (because we can't determine the specific key)
                    _mapChanged?.Invoke(this, new ApplicationDataMapChangedEventArgs(null, CollectionChange.Reset));
                });
            }
        }

        private void AddImpl(string key, object value)
        {
            this[key] = value;
        }
        
        private bool ContainsKeyImpl(string key)
        {
            if (IsRoaming)
            {
                return _store.ValueForKey(new NSString(key)) != null;
            }
            else
            {
                // TODO: see if there is a more efficient way of checking the key exists
                return _defaults.ValueForKey(new NSString(key)) != null;
            }
        }
        
        private bool RemoveImpl(string key)
        {
            if (IsRoaming)
            {
                _store.Remove(key);
            }
            else
            {
                _defaults.RemoveObject(key);
            }

            return true;
        }
        
        private bool TryGetValueImpl(string key, out object value)
        {
            NSObject obj = null;
            if (IsRoaming)
            {
                obj = _store.ValueForKey(new NSString(key));
            }
            else
            {
                obj = _defaults.ValueForKey(new NSString(key));
            }

            value = IOSTypeConverter.ConvertToObject(obj);
            return obj != null;
        }
        
        private object GetItem(string key)
        {
           
                NSObject obj = null;
                if (IsRoaming)
                {
                    obj = _store.ValueForKey(new NSString(key));
                }
                else
                {
                    obj = _defaults.ValueForKey(new NSString(key));
                }
                return IOSTypeConverter.ConvertToObject(obj);

            }

        private void SetItem(string key, object value)
        {
            if (value == null)
            {
                if (IsRoaming)
                {
                    _store.Remove(key);
                }
                else
                {
                    _defaults.RemoveObject(key);
                }
            }
            else
            {
                TypeCode code = Type.GetTypeCode(value.GetType());
                switch (code)
                {
                    case TypeCode.String:
                        if (IsRoaming)
                        {
                            _store.SetString(key, value.ToString());
                        }
                        else
                        {
                            _defaults.SetString(value.ToString(), key);
                        }
                        break;
                    case TypeCode.Int32:
                        if (IsRoaming)
                        {
                            _store.SetLong(key, (long)value);
                        }
                        else
                        {
                            _defaults.SetInt((int)value, key);
                        }
                        break;
                    case TypeCode.Double:
                        if (IsRoaming)
                        {
                            _store.SetDouble(key, (double)value);
                        }
                        else
                        {
                            _defaults.SetDouble((double)value, key);
                        }
                        break;
                    case TypeCode.Single:
                        if (IsRoaming)
                        {
                            _store.SetDouble(key, (double)value);
                        }
                        else
                        {
                            _defaults.SetFloat((float)value, key);
                        }
                        break;
                    case TypeCode.Boolean:
                        if (IsRoaming)
                        {
                            _store.SetBool(key, (bool)value);
                        }
                        else
                        {
                            _defaults.SetBool((bool)value, key);
                        }
                        break;

                    default:
                        if (IsRoaming)
                        {
                            _store.SetValueForKey(IOSTypeConverter.ConvertToNSObject(value), new NSString(key));
                        }
                        else
                        {
                            _defaults.SetValueForKey(IOSTypeConverter.ConvertToNSObject(value), new NSString(key));
                        }
                        break;
                }
            }
        }
        
        private void ClearImpl()
        {
#if __IOS__ || __TVOS__
            if (IsRoaming)
            {
                _store.Init();
            }
            else
            {
                _defaults.Init();
            }
#endif
        }
       
        private bool ContainsImpl(string key, object value)
        {
            if(ContainsKeyImpl(key))
            {
                if(GetItem(key) == value)
                {
                    return true;
                }
            }

            return false;
        }

    }
    
    internal static class IOSTypeConverter
    {
        public static object ConvertToObject(NSObject obj)
        {
            object val = null;

            if (obj != null)
            {
                if(obj is NSString)
                {
                    return obj.ToString();
                }

                if(obj is NSDate)
                {
                    return DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }

                if(obj is NSUuid)
                {
                    return new Guid(((NSUuid)obj).GetBytes());
                }

                if(obj is NSDecimalNumber)
                {
                    return decimal.Parse(obj.ToString(), CultureInfo.InvariantCulture);
                }

                if(obj is NSNumber)
                {
                    var x = (NSNumber)obj;
                    switch(x.ObjCType)
                    {
                        case "c":
                            return x.BoolValue;
                        case "l":
                        case "i":
                            return x.Int32Value;
                        case "s":
                            return x.Int16Value;
                        case "q":
                            return x.Int64Value;
                        case "Q":
                            return x.UInt64Value;
                        case "C":
                            return x.ByteValue;
                        case "L":
                        case "I":
                            return x.UInt32Value;
                        case "S":
                            return x.UInt16Value;
                        case "f":
                            return x.FloatValue;
                        case "d":
                            return x.DoubleValue;
                        case "B":
                            return x.BoolValue;
                        default:
                            return x.ToString();
                    }
                }

                if (obj.GetType() == typeof(NSString))
                {
                    val = ((NSString)obj).ToString();
                }
                else if (obj.GetType() == typeof(NSDate))
                {
                    val = DateTimeOffsetHelper.FromNSDate((NSDate)obj);
                }
            }

            return val;
        }

        public static NSObject ConvertToNSObject(object obj)
        {
            if(obj != null)
            {
                if(obj is Boolean)
                {
                    return NSNumber.FromBoolean((bool)obj);
                }
                else if (obj is Byte)
                {
                    return NSNumber.FromByte((byte)obj);
                }
                else if (obj is SByte)
                {
                    return NSNumber.FromSByte((sbyte)obj);
                }
                else if (obj is Int16)
                {
                    return NSNumber.FromInt16((short)obj);
                }
                else if (obj is Int32)
                {
                    return NSNumber.FromInt32((int)obj);
                }
                else if (obj is Int64)
                {
                    return NSNumber.FromInt64((long)obj);
                }
                else if (obj is UInt16)
                {
                    return NSNumber.FromUInt16((ushort)obj);
                }
                else if (obj is UInt32)
                {
                    return NSNumber.FromUInt32((uint)obj);
                }
                else if (obj is UInt64)
                {
                    return NSNumber.FromUInt64((ulong)obj);
                }
                else if (obj is Single)
                {
                    return NSNumber.FromFloat((float)obj);
                }
                else if (obj is Double)
                {
                    return NSNumber.FromDouble((double)obj);
                }
                else if (obj is string)
                {
                    return new NSString(obj.ToString());
                }
                else if(obj is Guid)
                {
                    return new NSUuid(((Guid)obj).ToByteArray());
                }
                else if(obj is DateTimeOffset)
                {
                    return ((DateTimeOffset)obj).ToNSDate();
                }
            }

            return null;
        }
    }
}