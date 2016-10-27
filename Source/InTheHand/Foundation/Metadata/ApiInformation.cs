// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiInformation.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
//#if WINDOWS_UWP
//using System;
//using System.Runtime.CompilerServices;
//[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.ApiInformation))]
//#else

using System;
using System.Reflection;

namespace InTheHand.Foundation.Metadata
{
    /// <summary>
    /// Enables you to detect whether a specified member, type, or API contract is present so that you can safely make API calls across a variety of devices.
    /// </summary>
    //[ContractVersion(typeof (FoundationContract), 65536U)]
    public static class ApiInformation
    {
        private const string assemblyQualification = ", InTheHand";
#if WINDOWS_PHONE_APP || WINDOWS_APP
        private static Type _type10;

        static ApiInformation()
        {
            //check for 10
            _type10 = Type.GetType("Windows.Foundation.Metadata.ApiInformation, Windows, ContentType=WindowsRuntime", false);
        }
#endif
        /// <summary>
        /// Returns true or false to indicate whether the API contract with the specified name and major version number is present.
        /// </summary>
        /// <param name="contractName">The name of the API contract.</param>
        /// <param name="majorVersion">The major version number of the API contract.</param>
        /// <returns>True if the specified API contract is present; otherwise, false.</returns>
        public static bool IsApiContractPresent(string contractName, ushort majorVersion)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_type10 != null)
            {
                return (bool)_type10.GetRuntimeMethod("IsApiContractPresent", new Type[] { typeof(string), typeof(ushort) }).Invoke(null, new object[] { contractName, majorVersion });
            }
#endif
            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether the API contract with the specified name and major and minor version number is present.
        /// </summary>
        /// <param name="contractName">The name of the API contract.</param>
        /// <param name="majorVersion">The major version number of the API contract.</param>
        /// <param name="minorVersion">The minor version number of the API contract.</param>
        /// <returns>True if the specified API contract is present; otherwise, false.</returns>
        public static bool IsApiContractPresent(string contractName, ushort majorVersion, ushort minorVersion)
        {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent(contractName, majorVersion, minorVersion);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
            if (_type10 != null)
            {
                return (bool)_type10.GetRuntimeMethod("IsApiContractPresent", new Type[] { typeof(string), typeof(ushort), typeof(ushort) }).Invoke(null, new object[] { contractName, majorVersion, minorVersion });
            }
#endif
            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified named constant is present for a specified enumeration.
        /// </summary>
        /// <param name="enumTypeName">The namespace-qualified name of the type.</param>
        /// <param name="valueName">The name of the constant.</param>
        /// <returns>True if the specified constant is present; otherwise, false.</returns>
        public static bool IsEnumNamedValuePresent(string enumTypeName, string valueName)
        {
            if (enumTypeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
                return Windows.Foundation.Metadata.ApiInformation.IsEnumNamedValuePresent(enumTypeName, valueName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsEnumNamedValuePresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { enumTypeName, valueName });
                }
                else
                {
                    var wenumType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", enumTypeName));
                    if (wenumType != null)
                    {
                        try
                        {
                            object val = global::System.Enum.Parse(wenumType, valueName, false);
                            if (val != null)
                            {
                                return true;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
#endif
            }

            var enumType = Type.GetType(enumTypeName + assemblyQualification, false);
            if (enumType != null)
            {
                try
                {
                    object val = global::System.Enum.Parse(enumType, valueName, false);
                    return val != null;
                }
                catch
                {
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified event is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>True if the specified event is present for the type; otherwise, false.</returns>
        public static bool IsEventPresent(string typeName, string eventName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
                return Windows.Foundation.Metadata.ApiInformation.IsEventPresent(typeName, eventName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsEventPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, eventName });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        try
                        {
                            var eventInfo = wType.GetRuntimeEvent(eventName);
                            return eventInfo != null;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if(t != null)
            {
                return t.GetRuntimeEvent(eventName) != null;
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified method is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>True if the specified method is present for the type; otherwise, false.</returns>
        public static bool IsMethodPresent(string typeName, string methodName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
                return Windows.Foundation.Metadata.ApiInformation.IsMethodPresent(typeName, methodName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsMethodPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, methodName });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        foreach (var method in wType.GetRuntimeMethods())
                        {
                            if (method.Name == methodName)
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if (t != null)
            {
                foreach(MethodInfo mi in t.GetRuntimeMethods())
                {
                    if (mi.Name == methodName)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified method overload with the specified number of input parameters is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="inputParameterCount">The number of input parameters for the overload.</param>
        /// <returns>True if the specified method is present for the type; otherwise, false.</returns>
        public static bool IsMethodPresent(string typeName, string methodName, uint inputParameterCount)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsMethodPresent(typeName, methodName, inputParameterCount);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsMethodPresent", new Type[] { typeof(string), typeof(string), typeof(uint) }).Invoke(null, new object[] { typeName, methodName, inputParameterCount });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        foreach (var method in wType.GetRuntimeMethods())
                        {
                            if (method.Name == methodName && method.GetParameters().Length == inputParameterCount)
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if (t != null)
            {
                foreach (MethodInfo mi in t.GetRuntimeMethods())
                {
                    if (mi.Name == methodName && mi.GetParameters().Length == inputParameterCount)
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified property (writeable or read-only) is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the specified property is present for the type; otherwise, false.</returns>
        public static bool IsPropertyPresent(string typeName, string propertyName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsPropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        try
                        {
                            var prop = wType.GetRuntimeProperty(propertyName);
                            return prop != null;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if (t != null)
            {
                return t.GetRuntimeProperty(propertyName) != null;
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified read-only property is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the specified property is present for the type; otherwise, false.</returns>
        public static bool IsReadOnlyPropertyPresent(string typeName, string propertyName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsReadOnlyPropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsReadOnlyPropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        try
                        {
                            var prop = wType.GetRuntimeProperty(propertyName);
                            return prop != null && prop.CanWrite == false;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if (t != null)
            {
                PropertyInfo pi = t.GetRuntimeProperty(propertyName);
                if (pi != null && pi.SetMethod == null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified writeable property is present for a specified type.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>True if the specified property is present for the type; otherwise, false.</returns>
        public static bool IsWriteablePropertyPresent(string typeName, string propertyName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsWriteablePropertyPresent(typeName, propertyName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsWriteablePropertyPresent", new Type[] { typeof(string), typeof(string) }).Invoke(null, new object[] { typeName, propertyName });
                }
                else
                {
                    var wType = Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName));
                    if (wType != null)
                    {
                        try
                        {
                            var prop = wType.GetRuntimeProperty(propertyName);
                            return prop != null && prop.CanWrite == true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            if (t != null)
            {
                PropertyInfo pi = t.GetRuntimeProperty(propertyName);
                if (pi != null && pi.SetMethod != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true or false to indicate whether a specified type is present.
        /// </summary>
        /// <param name="typeName">The namespace-qualified name of the type.</param>
        /// <returns>True if the specified type is present; otherwise, false.</returns>
        public static bool IsTypePresent(string typeName)
        {
            if (typeName.StartsWith("Windows."))
            {
#if WINDOWS_UWP
            return Windows.Foundation.Metadata.ApiInformation.IsTypePresent(typeName);
#elif WINDOWS_APP || WINDOWS_PHONE_APP
                if (_type10 != null)
                {
                    return (bool)_type10.GetRuntimeMethod("IsTypePresent", new Type[] { typeof(string) }).Invoke(null, new object[] { typeName });
                }
                else
                {
                    return Type.GetType(string.Format("{0}, Windows, ContentType=WindowsRuntime", typeName)) != null;
                }
#endif
            }

            Type t = Type.GetType(typeName + assemblyQualification, false);
            return t != null;
        }
    }
}
//#endif