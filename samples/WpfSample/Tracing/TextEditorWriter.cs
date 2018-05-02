using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfSample.Tracing
{
    public class TextEditorWriter : TextWriter
    {
        private TextEditor _editor;

        public TextEditorWriter(TextEditor editor)
        {
            _editor = editor;
        }

        public override Encoding Encoding => Encoding.Default;

        public override void WriteLine(string value)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                _editor.Document.Insert(_editor.Document.TextLength, value);
                _editor.Document.Insert(_editor.Document.TextLength, "\n");
            }));
        }
    }
}
