// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FoundationContract.cs" company="In The Hand Ltd">
//   Copyright (c) 2016 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#if WINDOWS_UWP
[assembly:System.Runtime.CompilerServices.TypeForwardedTo(typeof(Windows.Foundation.FoundationContract))]
#else

using Windows.Foundation.Metadata;

namespace Windows.Foundation
{
  [ContractVersion(65536U)]
  [ApiContract]
  public struct FoundationContract
  {
  }
}
#endif