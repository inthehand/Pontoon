//-----------------------------------------------------------------------
// <copyright file="StorageFolder.cs" company="In The Hand Ltd">
//     Copyright © 2016 In The Hand Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using InTheHand.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InTheHand.Storage
{
    /// <summary>
    /// Manages folders and their contents and provides information about them.
    /// </summary>
    public sealed class StorageFolder
    {
        /// <summary>
        /// Gets a StorageFile object to represent the file at the specified path.
        /// </summary>
        /// <param name="path">The path of the file to get a StorageFile to represent.
        /// If your path uses slashes, make sure you use backslashes(\).
        /// Forward slashes(/) are not accepted by this method.</param>
        /// <returns>When this method completes, it returns the file as a StorageFile.</returns>
        public static Task<StorageFolder> GetFolderFromPathAsync(string path)
        {
            return Task.Run<StorageFolder>(() =>
            {
                return new StorageFolder(path);
            });
        }

        private string _path;

        internal StorageFolder(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Creates a new file with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new file to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFile that represents the new file.</returns>
        public Task<StorageFile> CreateFileAsync(string desiredName)
        {
            return Task.Run<StorageFile>(() =>
            {
                string filepath = global::System.IO.Path.Combine(Path, desiredName);

                if(global::System.IO.File.Exists(filepath))
                {
                    throw new IOException();
                }

                File.Create(filepath).Close();

                return new Storage.StorageFile(filepath);
            });
        }

        /// <summary>
        /// Creates a new subfolder with the specified name in the current folder.
        /// </summary>
        /// <param name="desiredName">The name of the new subfolder to create in the current folder.</param>
        /// <returns>When this method completes, it returns a StorageFolder that represents the new subfolder.</returns>
        public Task<StorageFile> CreateFolderAsync(string desiredName)
        {
            return Task.Run<StorageFile>(() =>
            {
                string newpath = global::System.IO.Path.Combine(Path, desiredName);

                if (global::System.IO.Directory.Exists(newpath))
                {
                    throw new IOException();
                }

                Directory.CreateDirectory(newpath);

                return new Storage.StorageFile(newpath);
            });
        }

        public Task DeleteAsync()
        {
#if __ANDROID__ || __IOS__
            return Task.Run(() =>
            {
                if(!Directory.Exists(Path))
                {
                    throw new FileNotFoundException();
                }

                global::System.IO.Directory.Delete(Path);
            });
#endif
        }

        public Task<StorageFile> GetFileAsync(string filename)
        {
            return Task.Run<StorageFile>(() =>
            {
                string filepath = global::System.IO.Path.Combine(Path, filename);

                if(!global::System.IO.File.Exists(filepath))
                {
                    throw new FileNotFoundException();
                }

                return new StorageFile(filepath);
            });
        }

        public Task<IReadOnlyList<StorageFile>> GetFilesAsync()
        {
            return Task.Run<IReadOnlyList<StorageFile>>(() =>
            {
                List<StorageFile> files = new List<StorageFile>();
                foreach(string filename in global::System.IO.Directory.GetFiles(_path))
                {
                    files.Add(new StorageFile(global::System.IO.Path.Combine(_path, filename)));
                }

                return files;
            });
        }

        /*public Task<StorageFolder> GetParentAsync()
        {
            return Task.Run<StorageFolder>(() =>
            {
                return new StorageFolder();
            });
        }*/

        public DateTimeOffset DateCreated
        {
            get
            {
                var utc = global::System.IO.Directory.GetCreationTimeUtc(_path);
                var local = global::System.IO.Directory.GetCreationTime(_path);
                var offset = local - utc;
                return new DateTimeOffset(local, offset);
            }
        }

        public string Name
        {
            get
            {
                return global::System.IO.Path.GetDirectoryName(_path);
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
        }
    }
}