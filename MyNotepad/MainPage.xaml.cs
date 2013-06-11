using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace MyNotepad
{
    public partial class MainPage : PhoneApplicationPage
    {
        public class NoteDisplayModel
        {
            public string Title { get; set; }
            public string Filename { get; set; }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            RefillFilenamesList();
        }

        private void RefillFilenamesList()
        {
            var storage = IsolatedStorageFile.GetUserStoreForApplication();

            var filenames = storage.GetFileNames("*.txt");

            var nicerFilenames = from filename in filenames
                                 orderby filename
                                 select new NoteDisplayModel
                                 {
                                     Title = System.IO.Path.GetFileNameWithoutExtension(filename),
                                     Filename = filename
                                 };

            AllFiles.ItemsSource = nicerFilenames;
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            string uri = "/EditorPage.xaml";
            App.CurrentFileName = null;

            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
            
        }

        private void AllFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = (NoteDisplayModel)e.AddedItems[0];
                var filename = item.Filename;

                App.CurrentFileName = filename;
                NavigationService.Navigate(new Uri("/EditorPage.xaml", UriKind.Relative));
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var filename = (string)((MenuItem)sender).CommandParameter;

            var storage = IsolatedStorageFile.GetUserStoreForApplication();
            storage.DeleteFile(filename);

            RefillFilenamesList();
        }
    }
}