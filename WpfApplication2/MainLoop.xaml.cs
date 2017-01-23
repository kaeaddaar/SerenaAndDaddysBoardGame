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
using System.Windows.Forms;

namespace appGameBoardTest
{
    /// <summary>
    /// Interaction logic for MainLoop.xaml
    /// </summary>
    public partial class MainLoop : Window
    {
        public appGameBoardTest.Game.GameBoard GB;
        public MainLoop()
        {
            // The first thing to note, as that after I implimented this code, it prevents the app from doing anything. 
            // Events for keypress don't fire etc., but it is a start to the reseach on implementing a game loop.
            GB = new Game.GameBoard();
            InitializeComponent();
        }
  
        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            DateTime tick = new DateTime();
            DateTime t = new DateTime();
            tick = DateTime.Now;
            tick = tick.AddMilliseconds(500);
            t = DateTime.Now;

            do
            {
                System.Windows.Forms.Application.DoEvents();
                t = DateTime.Now;
                if (t >= tick)
                {
                    if (GB.dictCustomPlayerGroups["SpaceRocks"] != null)
                    {
                        foreach (Entities.Player p in GB.dictCustomPlayerGroups["SpaceRocks"])
                        {
                            Components.Vector3D_Compass Compass = new Components.Vector3D_Compass();
                            p.Movement.Vector = Compass.North;
                            Game.Movement.clsMovement.MoveEntity(ref p.Movement, ref p.Movable, GB);
                            //SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                            //simpleSound.Play();

                        }

                    }
                    tick = tick.AddMilliseconds(500);
                }
            } while (true);
        }
    }
}
