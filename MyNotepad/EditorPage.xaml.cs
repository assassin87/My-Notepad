using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MyNotepad
{
    public partial class EditorPage : PhoneApplicationPage
    {
        public EditorPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine(App.CurrentFileName);

            if (App.CurrentFileName != null)
            {
                Header.Text = "edit";
                var storage = IsolatedStorageFile.GetUserStoreForApplication();
                using (var stream = storage.OpenFile(App.CurrentFileName, FileMode.Open))
                {
                    StreamReader reader = new StreamReader(stream);
                    var note = reader.ReadToEnd();
                    Editor.Text = note;
                }
                
                
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            // So far, we assume that all files are new ones!
            // 1. Name the file from the first line of text
            var firstLinefeed = Editor.Text.IndexOfAny(new char[] { '\r', '\n' });
            string filename;

            if (firstLinefeed > -1)
            {
                filename = Editor.Text.Substring(0, firstLinefeed);
            }
            else
            {
                filename = Editor.Text;
            }

            // Remove non-alphanumeric chars from filename
            filename = new Regex("[^0-9a-zA-Z]").Replace(filename, string.Empty);
            filename = "/" + filename + ".txt";

            Debug.WriteLine(filename);

            // 2. Save to isolated storage
            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            using (var stream = storage.CreateFile(filename))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(Editor.Text);
                    writer.Flush();
                }
            }
        }
    }
}