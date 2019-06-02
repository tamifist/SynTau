using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoGene.Mobile.Helpers
{
    public class DebugTextWriter : TextWriter
    {
        private StringBuilder buffer;
        private string tag;

        public DebugTextWriter(string tag) : this()
        {
            this.tag = tag;
        }

        public DebugTextWriter()
        {
            buffer = new StringBuilder();
        }

        public override void Write(char value)
        {
            switch (value)
            {
                case '\n':
                    return;
                case '\r':

                    if (tag != null)
                        Debug.WriteLine("[{0}] {1}", tag, buffer.ToString());
                    else
                        Debug.WriteLine(buffer.ToString());

                    buffer.Clear();
                    return;
                default:
                    buffer.Append(value);
                    break;
            }
        }

        public override void Write(string value)
        {
            if (tag != null)
                Debug.WriteLine("[{0}] {1}", value);
            else
                Debug.WriteLine(value);
        }

        #region implemented abstract members of TextWriter
        public override Encoding Encoding
        {
            get { throw new NotImplementedException(); }
        }
        #endregion
    }
}