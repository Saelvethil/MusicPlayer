using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using MusicPlayerCore.Core;
using MusicPlayerCore.SongLibrary;
using MusicPlayerCore.Player;

namespace MusicPlayerView
{
    /// <summary>
    /// Interaction logic for LibraryView.xaml
    /// </summary>
    public partial class LibraryView : Window
    {
        public SongLibrary SongLibrary;
        ApplicationController controller;
        MainWindow mainWindow;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        int lastSelection = -1;
        TreeViewItem root;

        public LibraryView(ApplicationController controller, MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = SongLibrary.DefaultPlaylist;
            SongLibrary = controller.SongLibrary;
            this.controller = controller;
            this.mainWindow = mainWindow;
            FillTreeView();
            EnableButtons(false);


        }
        private void FillTreeView()
        {
            if (root != null) LibraryTreeView.Items.Remove(root);

            root = new TreeViewItem();

            root.Header = "All Songs";


            ILibraryComponent libraryComponent = SongLibrary.root;
            root.Tag = libraryComponent;

            foreach (var child in libraryComponent.GetChildren())
            {
                var artist = new TreeViewItem() { Header = ((LibraryArtist)child).ArtistName };
                root.Items.Add(artist);
                artist.Tag = child;
                foreach (var artistChild in child.GetChildren())
                {
                    var album = new TreeViewItem() { Header = ((LibraryAlbum)artistChild).AlbumName };
                    artist.Items.Add(album);
                    album.Tag = artistChild;
                    foreach (var albumChild in artistChild.GetChildren())
                    {
                        album.Items.Add(new TreeViewItem() { Header = ((LibrarySong)albumChild).SongName, Tag = albumChild });
                    }
                }
            }
            LibraryTreeView.Items.Add(root);
        }


        private void LibraryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (root != null)
            {
                ILibraryComponent tmp = (ILibraryComponent)((TreeViewItem)LibraryTreeView.SelectedItem).Tag;
                SongList list = new SongList();
                tmp.FillSongs(list);
                DataContext = list;
                Songs.SelectAll();
                lastSelection = -1;
            }
        }


        void GridViewColumnHeaderClickedHandler(object sender,
                                                RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    Songs.UnselectAll();
                    lastSelection = -1;


                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header 
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }

        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(Songs.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }



        private void Songs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {



            if (lastSelection != -1)
            {

                lastSelection = Songs.SelectedIndex;
                controller.PlaylistManager.AddSongToPlaylist((ISong)Songs.SelectedItem);
                controller.PlayEngine.SetPlaylist(controller.PlaylistManager.Playlist);
                mainWindow.UpdatePlaylist();
                Songs.UnselectAll();
                EnableButtons(false);
                lastSelection = -1;
            }
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtons(true);
            lastSelection = Songs.SelectedIndex;
        }

        private void ButtonUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            Songs.UnselectAll();
            EnableButtons(false);
        }

        private void EnableButtons(bool value)
        {
            ButtonAddSelected.IsEnabled = value;
        }

        private void ButtonAddSelected_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList listBoxItems = Songs.SelectedItems;
            List<ISong> list = new List<ISong>();
            foreach (var item in listBoxItems)
            {
                list.Add((ISong)item);
            }
            foreach (ISong song in list)
            {
                controller.PlaylistManager.AddSongToPlaylist(song);
            }
            controller.PlayEngine.SetPlaylist(controller.PlaylistManager.Playlist);
            mainWindow.UpdatePlaylist();
            Songs.UnselectAll();
            EnableButtons(false);
        }

        private void ButtonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            Songs.SelectAll();
            EnableButtons(true);
        }

        private void ButtonAddToLibrary_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.FileName = "songName";
            dlg.Filter = " song|*.mp3;*.wav;*.ogg";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string[] filenames = dlg.FileNames;
                foreach (string name in filenames)
                {
                    controller.SongLibrary.AddSong(name);
                }
                DataContext = SongLibrary.DefaultPlaylist;
                FillTreeView();
            }
        }



    }
}
