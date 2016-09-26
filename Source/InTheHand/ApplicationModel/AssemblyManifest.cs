using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InTheHand.ApplicationModel
{
    internal sealed class AssemblyManifest
    {
        private Assembly _launchingAssembly;
        public AssemblyManifest()
        {
            _launchingAssembly = Assembly.GetEntryAssembly();
        }
        
        public string Description
        {
            get
            {
                var attr = _launchingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                if (attr != null)
                {
                    return attr.Description;
                }

                return string.Empty;
            }
        }

        public string DisplayName
        {
            get
            {
                var attr = _launchingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
                if (attr != null)
                {
                    return attr.Title;
                }

                return string.Empty;
            }
        }

        public string PublisherDisplayName
        {
            get
            {
                var attr = _launchingAssembly.GetCustomAttribute<AssemblyCompanyAttribute>();
                if (attr != null)
                {
                    return attr.Company;
                }

                return string.Empty;
            }
        }

        public Version AssemblyVersion
        {
            get
            {
                var attr = _launchingAssembly.GetCustomAttribute<AssemblyVersionAttribute>();
                if (attr != null)
                {
                    return new Version(attr.Version);
                }

                return new Version();
            }
        }

        public Guid Guid
        {
            get
            {
                var attr = _launchingAssembly.GetCustomAttribute<GuidAttribute>();
                if (attr != null)
                {
                    return new Guid(attr.Value);
                }

                return Guid.Empty;
            }
        }
    }
}
