// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractVersionAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.ContractVersionAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [ContractVersion(typeof(FoundationContract), 65536U)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
    [AllowMultiple]
    public sealed class ContractVersionAttribute : Attribute
    {
        private uint _version;
        private Type _contract;
        public ContractVersionAttribute(uint version)
        {
            _version = version;
        }
        public ContractVersionAttribute(Type contract, uint version)
        {
            _contract = contract;
            _version = version;
        }
    }
}
#endif