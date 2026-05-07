using System;
using System.Collections.Generic;
using System.IO; // Needed for File.Move and Directory.Move
using TempElementsLib.Interfaces;
using TempElementsLib.Models;

namespace TempElementsLib
{
    public class TempElementsList : ITempElements
    {
        private bool disposed;
        private readonly List<ITempElement> elements = new List<ITempElement>();
        public IReadOnlyCollection<ITempElement> Elements => elements;
        public bool IsEmpty => elements.Count == 0;

        public T AddElement<T>() where T : ITempElement, new()
        {
            T element = new T();
            elements.Add(element);
            return element;
        }

        public void DeleteElement<T>(T element) where T : ITempElement, new()
        {
            if (element != null)
            {
                element.Dispose();
                elements.Remove(element);
            }
        }

        public void MoveElementTo<T>(T element, string newPath) where T : ITempElement, new()
        {
            if (element is TempFile fileElement)
            {
                File.Move(fileElement.FilePath, newPath);
            }
            else if (element is TempDir dirElement)
            {
                Directory.Move(dirElement.DirPath, newPath);
            }
        }

        public void RemoveDestroyed()
        {
            elements.RemoveAll(e => e.IsDestroyed);
        }

        #region Dispose section ==============================================
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    foreach (var element in elements)
                    {
                        element?.Dispose();
                    }
                    elements.Clear();
                }

                disposed = true;
            }
        }

        ~TempElementsList()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}