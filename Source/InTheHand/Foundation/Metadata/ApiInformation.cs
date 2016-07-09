// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiInformation.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;

namespace InTheHand.Foundation.Metadata
{
    /// <summary>
    /// Enables you to detect whether a specified member, type, or API contract is present so that you can safely make API calls across a variety of devices.
    /// </summary>
    [CLSCompliant(false)]
    public static class ApiInformation
    {
#if WINDOWS_PHONE_APP || WINDOWS_APP
        private static bool _on10;
        private static Type _type10;
        static ApiInformation()
        {
            //check for 10
            _type10 = Type.GetType("Windows.Foundation.Metadata.ApiInformation, Windows, ContentType=WindowsRuntime");
            if (_type10 != null)
            {
                _on10 = true;
            }
        }
#endif
        public static bool IsApiContractPresent(string contractName, ushort majorVersion)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsApiContractPresent", new Type[] { typeof(string), typeof(ushort) }).Invoke(null, new object[] { contractName, majorVersion });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion, minorVersion);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsApiContractPresent", new Type[] { typeof(string), typeof(ushort), typeof(ushort) }).Invoke(null, new object[] { contractName, majorVersion, minorVersion });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsEnumNamedValuePresent(string enumTypeName, string valueName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsEnumNamedValuePresent(enumTypeName, valueName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsEnumNamedValuePresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { enumTypeName, valueName });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsEventPresent(string typeName, string eventName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsEventPresent(typeName, eventName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsEventPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, eventName });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsMethodPresent(string typeName, string methodName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsMethodPresent(typeName, methodName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsMethodPresent", new Type[] { typeof(string), typeof(string)}).Invoke(null, new object[] { typeName, methodName});
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsMethodPresent(string typeName, string methodName, uint inputParameterCount)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsMethodPresent(typeName, methodName, inputParameterCount);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsMethodPresent", new Type[] { typeof(string), typeof(string), typeof(uint) }).Invoke(null, new object[] { typeName, methodName, inputParameterCount });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsPropertyPresent(string typeName, string propertyName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsPropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsReadOnlyPropertyPresent(string typeName, string propertyName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsReadOnlyPropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsReadOnlyPropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsWriteablePropertyPresent(string typeName, string propertyName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsWriteablePropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsWriteablePropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
            }

            return false;
#else
            return false;
#endif
        }

        public static bool IsTypePresent(string typeName)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent(typeName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if(_on10)
            {
                return (bool)_type10.GetRuntimeMethod("IsTypePresent", new Type[] { typeof(string) }).Invoke(null, new object[] { typeName });
            }

            return false;
#else
            return false;
#endif
        }
    }
}