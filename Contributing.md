# Contributing to Pontoon

Pontoon is designed to expose the Universal Windows Platform API to a range of platforms supporting .NET development.
Any additions to the library must match the UWP equivalent API or in exceptional circumstances use extension methods which work with the Pontoon and UWP codebase (For example Toast notifications).

## A good pull request
Every contribution has to come with:

* Before starting coding, **you should open an issue** and start discussing with the community to see if your idea/feature is interesting enough
* Detailed XML documentation which will be built into the [API documentation library](https://inthehand.github.io).
* A sample for the Sample apps (If applicable)
* Unit tests (If applicable)
* You tested your code with each of the supported platform versions.

PR has to be validated by the project co-ordinator before being merged.

## General rules

* DO NOT require that users perform any extensive initialization before they can start programming basic scenarios.
* DO provide good defaults for all values associated with parameters, options, etc.
* DO ensure that APIs are intuitive and can be successfully used in basic scenarios without referring to the reference documentation.
* DO communicate incorrect usage of APIs as soon as possible. 
* DO consider default behaviour across platforms and document limitations/differences.
* DO use extension methods over static methods where possible.
* DO NOT return true or false to give sucess status. Throw exceptions if there was a failure.
* DO use verbs like GET.
* DO NOT use verbs that are not already used like fetch.

## Naming conventions
* We are following the coding guidelines of [.NET Core Foundational libraries](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md). 

## Documentation
* DO NOT expect that your API is so well designed that it needs no documentation. No API is that intuitive.
* DO provide great documentation with all APIs.
* DO cross-reference existing UWP documentation where it is useful. 
* DO use readable and self-documenting identifier names. 
* DO use consistent naming and terminology.
* DO provide strongly typed APIs.
* DO use verbose identifier names.

## Files and folders
* DO associate no more than one class per file.
* DO use folders to group files based on UWP namespaces.