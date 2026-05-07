using System;
using System.IO;
using TempElementsLib.Interfaces;

namespace TempElementsLib.Models
{
    public class TempDir : ITempDir
    {
        private bool disposed;
        public DirectoryInfo dirInfo { get; }
        public string DirPath => dirInfo.FullName;
        public bool IsDestroyed { get; private set; }

        public bool IsEmpty => Directory.Exists(DirPath) &&
                               Directory.GetFileSystemEntries(DirPath).Length == 0;

        public TempDir() : this(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())) { }

        public TempDir(string path)
        {
            try
            {
                dirInfo = Directory.CreateDirectory(path);
                IsDestroyed = false;
            }
            catch (IOException ex)
            {
                throw new Exception($"Could not create directory at {path}", ex);
            }
        }

        public void Empty()
        {
            if (IsDestroyed) throw new ObjectDisposedException(nameof(TempDir));

            foreach (var file in dirInfo.GetFiles()) file.Delete();
            foreach (var dir in dirInfo.GetDirectories()) dir.Delete(true);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                try
                {
                    if (Directory.Exists(DirPath))
                    {
                        Directory.Delete(DirPath, true);
                    }
                }
                catch { /* Standard cleanup silence */ }

                IsDestroyed = true;
                disposed = true;
            }
        }

        ~TempDir() => Dispose(false);
    }
}