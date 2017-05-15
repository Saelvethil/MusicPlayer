using System;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using MusicPlayerCore.Core;
using MusicPlayerCore.Player;

namespace MusicPlayerView
{
    public partial class MainWindow : Window
    {
        ApplicationController controller = new ApplicationController();
        LibraryView libraryView;
        int selectedIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            
            UpdatePlaylist();
            SetInfo(controller.PlayEngine.CurrentSong);
            SliderVolume.Value = controller.PlayEngine.Volume;
            controller.PlayEngine.SongFinished += (sender, args) =>
                this.Dispatcher.Invoke((Action)(() => { SetInfo(controller.PlayEngine.CurrentSong); }));
            Thread t = new Thread(() =>
            {
                Thread.Sleep(500);
                while (true)
                {
                    Thread.Sleep(50);
                    try
                    {
                        this.Dispatcher.Invoke((Action)(() => { SetPosition(controller.PlayEngine.CurrentSong); }));
                    }
                    catch { }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        public void UpdatePlaylist()
        {
            if (controller.PlaylistManager.Playlist.ListSize > 0)
            {
                EnableButtons(true);
            }
            else
            {
                EnableButtons(false);
            }
            DataContext = controller.PlaylistManager.Playlist;
            ListBoxSongs.Items.Refresh();
        }

        private void EnableButtons(bool value)
        {
            ButtonNext.IsEnabled = value;
            ButtonPlay.IsEnabled = value;
            ButtonPrevious.IsEnabled = value;
            ButtonStop.IsEnabled = value;
        }
        public void SetInfo(ISong song)
        {
            if (song != null)
            {
                LabelSongName.Content = song.SongName;
                LabelAlbumName.Content = song.AlbumName;
                LabelAlbumYear.Content = song.Year;
                LabelArtistName.Content = song.ArtistName;
                LabelDuration.Content = ParseTime(song.Duration);
                LabelPosition.Content = ParseTime(song.Position);
            }
        }

        public void SetPosition(ISong song)
        {
            if (song != null)
            {
                double pos = song.Player.Position;
                LabelPosition.Content = ParseTime(pos);
                ProgressBarPosition.Value = pos * 100 / song.Duration;
            }
        }

        private String ParseTime(double time)
        {
            double d = (double)time;
            int seconds = (int)d % 60;
            String str = "";
            str = ((int)d / 60 + ":" + ((seconds / 10 > 0) ? seconds.ToString() : ("0" + seconds.ToString())));
            return str;
        }
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            if (controller.PlayEngine.IsPlaying())
            {
                controller.PlayEngine.Pause();
                ButtonPlay.Content = "Play";
            }
            else
            {
                controller.PlayEngine.Play();
                ButtonPlay.Content = "Pause";
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            controller.PlayEngine.Stop();
            ButtonPlay.Content = "Play";
        }


        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            controller.PlayEngine.PreviousSong();
            SetInfo(controller.PlayEngine.CurrentSong);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            controller.PlayEngine.NextSong();
            SetInfo(controller.PlayEngine.CurrentSong);
        }

        private void ToggleButtonRandom_Click(object sender, RoutedEventArgs e)
        {
            bool check = ToggleButtonRandom.IsChecked.HasValue ? (bool)ToggleButtonRandom.IsChecked : false;
            controller.PlayEngine.SetRandomEnumerator(check);
        }



        private void ListBoxSongs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (ListBoxSongs.SelectedIndex != selectedIndex)
            {
                selectedIndex = ListBoxSongs.SelectedIndex;
                controller.PlayEngine.SetSong((ISong)ListBoxSongs.SelectedItem);

                SetInfo(controller.PlayEngine.CurrentSong);
                ButtonPlay.Content = "Pause";
            }


        }



        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(controller.PlayEngine.CurrentSong != null)
                 controller.PlayEngine.Volume = (int)e.NewValue;
        }

        private void Library_Click(object sender, RoutedEventArgs e)
        {
            libraryView = new LibraryView(controller, this);

            libraryView.Left = this.Left + 300;
            libraryView.Top = this.Top;

            libraryView.Show();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxSongs.SelectedIndex != -1)
            {
                controller.PlaylistManager.RemoveSongFromPlaylist(ListBoxSongs.SelectedIndex);
                UpdatePlaylist();
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                MenuItem_Click(sender, e);
            }
        }

        private void LoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "playlist";
            dlg.Filter = " Playlist|*.xml;*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                controller.PlayEngine.Stop();
                controller.PlaylistManager.LoadPlaylistFromFile(filename);
                controller.PlayEngine.SetPlaylist(controller.PlaylistManager.Playlist);
                UpdatePlaylist();
            }
        }

        private void SavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "playlist";
            dlg.Filter = " .xml|*.xml;|.json|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                controller.PlaylistManager.SavePlaylist(filename);
            }
        }

        private void NewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            SongList newList = new SongList();
            controller.PlaylistManager.Playlist = newList;
            controller.PlayEngine.SetPlaylist(controller.PlaylistManager.Playlist);
            UpdatePlaylist();
            Library_Click(sender, e);
        }


    }
}
