//-----------------------------------------------------------------------
// <copyright file="AppDataPaths.cs" company="In The Hand Ltd">
//     Copyright © 2018 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using InTheHand.ApplicationModel;

namespace InTheHand.Storage
{
    /// <summary>
    /// AppDataPaths returns paths to commonly used application folders based on the KNOWNFOLDERID naming pattern.
    /// </summary>
    public sealed class AppDataPaths
    {
        private static AppDataPaths s_current = new AppDataPaths();

        /// <summary>
        /// Gets the paths to a user's various app data folders. Use this method in single user apps.
        /// </summary>
        public static AppDataPaths GetDefault()
        {
            return s_current;
        }

        private AppDataPaths()
        {
        }

        public string Cookies
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Cookies);
            }
        }

        public string Desktop
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.DesktopDirectory);
            }
        }

        public string Documents
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.MyDocuments);
            }
        }

        public string Favorites
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.Favorites);
            }
        }

        public string History
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.History);
            }
        }

        public string InternetCache
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.InternetCache);
            }
        }

        public string LocalAppData
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.LocalApplicationData);
            }
        }

        public string ProgramData
        {
            get
            {
                return global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.ApplicationData);
            }
        }

        private static readonly Guid RoamingAppDataGuid = new Guid("{3EB685DB-65F9-4CF6-A03A-E3EF65729F3D}");

        public string RoamingAppData
        {
            get
            {
                return KnownFolders.GetKnownFolderPathFromGuid(RoamingAppDataGuid);
            }
        }


    }
}