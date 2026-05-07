using System;
using System.IO;
using TempElementsLib.Interfaces;

namespace TempElementsLib.Models
{
    public class TempFile : ITempFile
    {
        private bool disposed;
        public FileStream fileStream { get; }
        public FileInfo fileInfo { get; }

        public string FilePath => fileInfo.FullName;
        public bool IsDestroyed { get; private set; }

        // Constructor 1: Default (uses Path.GetTempFileName)
        public TempFile() : this(Path.GetTempFileName()) { }

        // Constructor 2: Specific path
        public TempFile(string path)
        {
            fileInfo = new FileInfo(path);
            // Open or create the file with Read/Write access
            fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            IsDestroyed = false;
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
                if (disposing)
                {
                    fileStream?.Dispose();
                }

                try
                {
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                }
                catch { /* Ignore or log errors during cleanup */ }

                IsDestroyed = true;
                disposed = true;
            }
        }

        ~TempFile() => Dispose(false);
    }
}