using System;
using System.IO;
using System.Text;
using TempElementsLib.Interfaces;

namespace TempElementsLib.Models
{
    public class TempTxtFile : TempFile
    {
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        public TempTxtFile() : base()
        {
            _writer = new StreamWriter(fileStream, Encoding.UTF8, 1024, leaveOpen: true);
            _reader = new StreamReader(fileStream, Encoding.UTF8, true, 1024, leaveOpen: true);
        }

        public void Write(string text)
        {
            _writer.Write(text);
            _writer.Flush(); 
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(text);
            _writer.Flush();
        }

        public string ReadLine()
        {
            fileStream.Position = 0; 
            return _reader.ReadLine();
        }

        public string ReadAllText()
        {
            fileStream.Position = 0;
            return _reader.ReadToEnd();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer?.Dispose();
                _reader?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}