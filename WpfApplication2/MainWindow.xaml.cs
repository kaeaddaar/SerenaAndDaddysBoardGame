using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Media;

namespace appGameBoardTest
{
    public partial class MainWindow : Window
    {
        public Game.GameBoard GB;
        List hitResultsList = new List();

        MediaPlayer wplayer = new MediaPlayer();

        public MainWindow()
        {
            GB = new Game.GameBoard();
            this.Content = GB.myViewport3D;
            this.Top = 30;
            this.Left = 360;
        }

        public void PlayBackgroundMusic()
        {
            wplayer.Open(new Uri(@"..\..\Game\GameSounds\SuperMario3DWorld_Theme.wav", UriKind.Relative));
            wplayer.Volume = .500;   //.05 is 50% I think
            wplayer.Play();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            GB.GBWindow_KeyDown(sender, e);

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GB.HitTest(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlayBackgroundMusic();
        }
    }
}
