// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.StaticAttribute))]
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.PlatformAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [AttributeUsage(AttributeTargets.Class)]
    [ContractVersion(typeof(FoundationContract), 65536U)]
    [AllowMultiple]
    public sealed class StaticAttribute : Attribute
    {
        private Type _type;
        private uint _version;
        private Platform _platform;
        private Type _contractName;

        public StaticAttribute(Type type, uint version)
        {
            _type = type;
            _version = version;
        }
        public StaticAttribute(Type type, uint version, Platform platform)
        {
            _type = type;
            _version = version;
            _platform = platform;
        }
        public StaticAttribute(Type type, uint version, Type contractName)
        {
            _type = type;
            _version = version;
            _contractName = contractName;
        }
    }

    [ContractVersion(typeof(FoundationContract), 65536U)]
    public enum Platform
    {
        Windows,
        WindowsPhone,
    }
}
#endif