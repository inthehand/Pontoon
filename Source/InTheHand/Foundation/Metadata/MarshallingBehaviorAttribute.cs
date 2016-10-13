// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarshalingBehaviorAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.MarshalingBehaviorAttribute))]
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.MarshalingType))]
#else

using System;

namespace Windows.Foundation.Metadata
{
    [AttributeUsage(global::System.AttributeTargets.Class)]
    [ContractVersion(typeof(FoundationContract), 65536U)]
    public sealed class MarshalingBehaviorAttribute : Attribute
    {
        private MarshalingType _behavior;

        public MarshalingBehaviorAttribute(MarshalingType behavior)
        {
            _behavior = behavior;
        }
    }

    [ContractVersion(typeof(FoundationContract), 65536U)]
    public enum MarshalingType
    {
        InvalidMarshaling,
        None,
        Agile,
        Standard,
    }
}
#endif