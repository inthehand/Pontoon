// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiContractAttribute.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
using System.Runtime.CompilerServices;
[assembly: TypeForwardedTo(typeof(Windows.Foundation.Metadata.ApiContractAttribute))]
#else

using System;

namespace Windows.Foundation.Metadata
{
  [AttributeUsage(AttributeTargets.Struct)]
  [ContractVersion(typeof (FoundationContract), 65536U)]
  public sealed class ApiContractAttribute : Attribute
  {
  }
}
#endif