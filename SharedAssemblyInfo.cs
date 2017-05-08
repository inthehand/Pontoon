// ******************************************************************
// Copyright (c) In The Hand Ltd. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if WINDOWS_PHONE
[assembly: AssemblyConfiguration("Windows Phone 8.0")]
#elif WINDOWS_PHONE_81
[assembly: AssemblyConfiguration("Windows Phone Silverlight 8.1")]
#elif WINDOWS_PHONE_APP
[assembly: AssemblyConfiguration("Windows Phone 8.1")]
#elif __IOS__
[assembly: AssemblyConfiguration("iOS")]
#elif __TVOS__
[assembly: AssemblyConfiguration("tvOS")]
#elif __ANDROID__
[assembly: AssemblyConfiguration("Android")]
#elif __MAC__
[assembly: AssemblyConfiguration("macOS")]
#elif WINDOWS_APP
[assembly: AssemblyConfiguration("Windows 8.1")]
#elif WINDOWS_UWP
[assembly: AssemblyConfiguration("Windows Universal")]
#elif WIN32
[assembly: AssemblyConfiguration("Windows")]
#elif TIZEN
[assembly: AssemblyConfiguration("Tizen")]
#else
[assembly: AssemblyConfiguration("Portable")]
#endif
[assembly: AssemblyProduct("Pontoon")]
[assembly: AssemblyCompany("In The Hand Ltd")]
[assembly: AssemblyCopyright("Copyright © In The Hand Ltd 2013-17. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("10.0.0.0")]
[assembly: AssemblyFileVersion("10.2017.05.08")]
[assembly: ComVisible(false)]
[assembly: NeutralResourcesLanguageAttribute("en-US")]