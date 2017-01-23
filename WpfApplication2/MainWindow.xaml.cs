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
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // These public variables are avaiable to all of the functions and procedures in this window
        public Game.GameBoard GB;
        List hitResultsList = new List();
        public Game.GameLoop GBLoop;

        MediaPlayer wplayer = new MediaPlayer();

        public MainWindow()
        {

            GB = new Game.GameBoard();
            this.Content = GB.myViewport3D;
            //Application.Current.Dispatcher.Invoke();
            this.Top = 30;
            this.Left = 360;

        }

        public void PlayBackgroundMusic()
        {

            wplayer.Open(new Uri(@"..\..\Game\GameSounds\SuperMario3DWorld_Theme.wav", UriKind.Relative));
            wplayer.Volume = .500;   //.05 is 50% I think
            wplayer.Play();
            
            //wplayer.Open(new Uri(@"..\..\Game\GameSounds\18-overworld-bgm.mp3", UriKind.Relative));
            //wplayer.Volume = .075;   //.05 is 50% I think
            //wplayer.Play();

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
            GBLoop = new Game.GameLoop(ref GB);
            GBLoop.MainLoop();
            //do
            //{
            //    if (GBLoop.TimeToFireEvent)
            //    {
            //        //SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");

            //        //simpleSound.Play();

            //        if (GB.dictCustomPlayerGroups["SpaceRocks"] != null)
            //        {
            //            foreach (Entities.Player p in GB.dictCustomPlayerGroups["SpaceRocks"])
            //            {
            //                Components.Vector3D_Compass Compass = new Components.Vector3D_Compass();
            //                p.Movement.Vector = Compass.North;
            //                Game.Movement.clsMovement.MoveEntity(ref p.Movement, ref p.Movable, GB);
            //                //SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
            //                //simpleSound.Play();

            //            }

            //        }
            //        GBLoop.TimeToFireEvent = false;
            //    }

            //} while (true);

        }
    }
}
