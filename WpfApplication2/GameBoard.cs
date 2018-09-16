using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;     // For using Point3D
using System.Windows.Media;             // For using Brush
using System.Windows.Controls;          // For using the Viewport
using System.Windows.Input;             // For using keypress event
using System.Windows.Media.Imaging;     // For using BitmapImage

using System.Media;                     // For sound player
using System.Windows;
using System.Windows.Threading;
using appGameBoardTest.Components;
using System.Threading;
using System.Diagnostics;
using appGameBoardTest.Extensions;

namespace appGameBoardTest.Game
{

    public class GameBoard
        // Purpose: Track map elements and positions of entities
        // Purpose: Tracks the info used to draw the map and decide what Items on the map need to be drawn.
    {

        List<appGameBoardTest.Tile> lstTiles;                   // The map is built from this list of Tiles

        // Declare scene objects.
        public Viewport3D myViewport3D = new Viewport3D();      // viewport gets tied to MainWindow.Content from MainWindow so needs to be public
        Model3DGroup myModel3DBoard = new Model3DGroup();       // Tiles for the board go here
        public Model3DGroup myModel3DEntities = new Model3DGroup();    // Entities go here, Made public so the movement algorithm can create and destroy stuff
        public Model3DGroup myModel3DDisplay = new Model3DGroup();     // All other group will go into this group before getting displayed
        public Model3DGroup myModelMovement = new Model3DGroup();
        
        GeometryModel3D myGeometryModel = new GeometryModel3D();
        ModelVisual3D myModelVisual3D = new ModelVisual3D();
        // Defines the camera used to view the 3D object. In order to view the 3D object, 
        // the camera must be positioned and pointed such that the object is within view  
        // of the camera.
        PerspectiveCamera myPCamera = new PerspectiveCamera();

        // used to turn on mode to keep on tiles
        public Boolean inGameMode = true;       // For passing in info to movement functions

        // Arrays of Components
        public Dictionary<UInt32, Components.Movement> dictMovement = new Dictionary<UInt32, Components.Movement>();
        public Dictionary<UInt32, Components.Movable> dictMovable = new Dictionary<uint, Components.Movable>();
        public Dictionary<UInt32, Components.Character_Containers> dictCharacter_Containers = new Dictionary<uint, Components.Character_Containers>();
        public Dictionary<UInt32, Components.Character_Attributes> dictCharacter_Attributes = new Dictionary<uint, Components.Character_Attributes>();

        // Arrays of Entities
        public Dictionary<UInt32, Entities.Player> dictPlayer = new Dictionary<uint, Entities.Player>();
        public Dictionary<UInt32, Entities.BaseEntity> dictBox = new Dictionary<uint, Entities.BaseEntity>();
        public Dictionary<String, List<Entities.Player>> dictCustomPlayerGroups = new Dictionary<string, List<Entities.Player>>();

        Entities.Player YellowMan1; Entities.Player YellowMan2; Entities.Player YellowMan3;
        Entities.Player RedMan;
        Entities.BaseEntity Block;

        public UInt32 nextEntityId = 1;        // Public so that move routines work
        //Window2 winInfo = new Window2();
        //public Game.UserInerface.GBInfo GB_Info;
        public GeometryModel3D GeoMove;

        // used in game loop
        private Stopwatch watch;
        private DispatcherTimer timer;
        private long ticksSinceEnemyMove;

        // queue to track movement requests
        Queue<Entities.Player> MovementQueue_ByBlock;

        public GameBoard()
        {
            cameraSetup();
            lightingSetup();

            // This is where I start adding stuff like the map and players
            lstTiles = clsSquareMaps.File_Main();   // Load the tile map from a file

            loadEntities();                 // holds the calls to create the various entities of the game
            
            loadTileMapToModels();          // loops through lstTiles and adds the square geometries to myModel3DGroup
            
            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DDisplay;
            myModel3DDisplay.Children.Add(myModel3DBoard);
            myModel3DDisplay.Children.Add(myModel3DEntities);
            myModel3DDisplay.Children.Add(myModelMovement);

            // Sets up the viewport, which is tied to the content of MainWindow (The form)
            myViewport3D.Children.Add(myModelVisual3D);
            dictCustomPlayerGroups.Add("SpaceRocks", new List<Entities.Player>());

            //GB_Info = new UserInerface.GBInfo(winInfo);
            //setup_info_window();
            //updateGBInfo();

            MovementQueue_ByBlock = new Queue<Entities.Player>();
            ticksSinceEnemyMove = 0;

            watch = new Stopwatch();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += timer_Tick;
            timer.Start();

        } //GameBoard constructor

        void timer_Tick(object sender, EventArgs e)
        {

            if (ticksSinceEnemyMove > (750 * 1000)) 
            {
                Vector3D directionToMario = new Vector3D
                (
                    RedMan.Movement.Location.X - YellowMan2.Movement.Location.X,
                    RedMan.Movement.Location.Y - YellowMan2.Movement.Location.Y,
                    RedMan.Movement.Location.Z - YellowMan2.Movement.Location.Z
                );
                directionToMario.Normalize();
                Vector3D DirectionToGo = new Vector3D();
                if (directionToMario.X > 0.5)
                {
                    DirectionToGo = Vector3D_Compass.East;
                }
                if (directionToMario.X < -0.5)
                {
                    DirectionToGo = Vector3D_Compass.West;
                }
                if (directionToMario.Y > 0.5)
                {
                    DirectionToGo = Vector3D_Compass.North;
                }
                if (directionToMario.Y < -0.5)
                {
                    DirectionToGo = Vector3D_Compass.South;
                }

                YellowMan2.Movement.Vector = DirectionToGo;
                MovementQueue_ByBlock.Enqueue(YellowMan2);
                ticksSinceEnemyMove = 0; // moved
            }

            ticksSinceEnemyMove = ticksSinceEnemyMove + timer.RunGameLoop(watch, ProcessMovement);

        }

        int ProcessMovement()
        {
            foreach (var e in MovementQueue_ByBlock)
            {
                Game.Movement.clsMovement.MoveEntity(ref e.Movement, ref e.Movable, this);
            }

            return 0; // pass any int to fit Func<int> signature
        }

        public void HitTest(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            Point mouseposition = args.GetPosition(myViewport3D);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);

            //test for a result in the Viewport3D
            VisualTreeHelper.HitTest(myViewport3D, null, HTResult, pointparams);
        } //HitTest


        public HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            //MessageBox.Show(rawresult.ToString());
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;

                if (rayMeshResult != null)
                {
                    GeometryModel3D hitgeo = rayMeshResult.ModelHit as GeometryModel3D;
                    //MessageBox.Show("" + hitgeo.Bounds);
                    
                    ImageBrush B = new ImageBrush();
                    B.ImageSource = new BitmapImage(new Uri(@"..\..\Game\Images\Items\SpaceRockTri.jpg", UriKind.Relative));
                    if (hitgeo.Bounds.Z <= 0)
                    {
                        foreach (KeyValuePair<uint,appGameBoardTest.Components.Movement> kvp in this.dictMovement)
                        {
                            Components.Movement mov = (Components.Movement)kvp.Value;
                            if (mov.Location.X == hitgeo.Bounds.X && mov.Location.Y == hitgeo.Bounds.Y && mov.Location.Z == hitgeo.Bounds.Z + .1)
                            {
                                return HitTestResultBehavior.Continue;
                            }
                        }
                        
                        Entities.Player tmpPlayer = new Entities.Player(new Point3D(hitgeo.Bounds.X, hitgeo.Bounds.Y, hitgeo.Bounds.Z + .1), B, new Vector3D(0, 0, 0), ref nextEntityId, this, "YellowMan1");
                        myModel3DEntities.Children.Add(tmpPlayer.Movement.Geometry);

                        if (dictCustomPlayerGroups["SpaceRocks"] != null )
                        {
                            dictCustomPlayerGroups["SpaceRocks"].Add(tmpPlayer);
                        }

                    }

                }
            }

            return HitTestResultBehavior.Continue;
        } //HTResult


        private void loadEntities()
        {
            // Create the RedMan entity (THIS IS THE PLAYER)
            ImageBrush colors_brush = new ImageBrush();
            //            colors_brush.ImageSource = new BitmapImage(new Uri("C:\\Users\\Clifford\\Documents\\Visual Studio 2013\\Projects\\WpfApplication2\\WpfApplication2\\Game\\PinkAndBlueGirl.png", UriKind.Absolute));
            colors_brush.ImageSource = new BitmapImage(new Uri(@"..\..\Game\MarioHead.jpg", UriKind.Relative));

            //            RedMan = new Entities.Player(new Point3D(1, 1, .1), Brushes.Blue, new Vector3D(0, 0, 0), ref nextEntityId, this, "RedMan");
            RedMan = new Entities.Player(new Point3D(1, 1, .1), colors_brush, new Vector3D(0, 0, 0), ref nextEntityId, this, "RedMan");
            myModel3DEntities.Children.Add(RedMan.Movement.Geometry);
            RedMan.Character_Containers.Health.MaxQty = 1;
            RedMan.Character_Containers.Health.Qty = 1;

            // Create YellowMan<Player> entity
            ImageBrush Sophia_brush = new ImageBrush();
            Sophia_brush.ImageSource = new BitmapImage(new Uri(@"..\..\Game\BlueToadHead.jpg", UriKind.Relative));

            YellowMan1 = new Entities.Player(new Point3D(6, 3, .1), Sophia_brush, new Vector3D(0, 0, 0), ref nextEntityId, this, "YellowMan1");
            myModel3DEntities.Children.Add(YellowMan1.Movement.Geometry);

            ImageBrush Elise_brush = new ImageBrush();
            Elise_brush.ImageSource = new BitmapImage(new Uri(@"..\..\Game\FunnyRedMushroom.jpg", UriKind.Relative));

            // Create YellowMan<Player> entity
            YellowMan2 = new Entities.Player(new Point3D(3, 8, .1), Elise_brush, new Vector3D(0, 0, 0), ref nextEntityId, this, "YellowMan2");
            myModel3DEntities.Children.Add(YellowMan2.Movement.Geometry);

            //myModel3DEntities.Children.Remove(YellowMan2.Movement.Geometry);      // Testing removing some geometry. It works by index or object

            // Create YellowMan<Player> entity
            YellowMan3 = new Entities.Player(new Point3D(2, 3, .1), Brushes.Yellow, new Vector3D(0, 0, 0), ref nextEntityId, this, "YellowMan3");
            myModel3DEntities.Children.Add(YellowMan3.Movement.Geometry);

            // Create a Block.Box entity
            ImageBrush Shield_brush = new ImageBrush();
            Shield_brush.ImageSource = new BitmapImage(new Uri(@"..\..\Game\Images\Items\IronShield.png", UriKind.Relative));

            Block = new Entities.BaseEntity(new Point3D(5, 5, .1), Shield_brush, new Vector3D(0, 0, 0), ref nextEntityId, this, "Block1");
            Block.Movable.isMovable = true;
            myModel3DEntities.Children.Add(Block.Movement.Geometry);

            // Create a Block.Box entity
            ImageBrush ShieldPeach_brush = new ImageBrush();
            ShieldPeach_brush.ImageSource = new BitmapImage(new Uri(@"..\..\Game\Images\Items\ShieldPeach.jpg", UriKind.Relative));
            Block = new Entities.BaseEntity(new Point3D(7, 5, .1), ShieldPeach_brush, new Vector3D(0, 0, 0), ref nextEntityId, this, "Block2");
            myModel3DEntities.Children.Add(Block.Movement.Geometry);
            
        } //loadEntities

        
        private void cameraSetup()
        {
            // Specify where in the 3D scene the camera is.
            myPCamera.Position = new Point3D(10, -6, 30);
            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = new Vector3D(0, .5, -1);
            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = 60;

            // Asign the camera to the viewport
            myViewport3D.Camera = myPCamera;

        } //cameraSetup

        
        private void lightingSetup()
        {
            // Define the lights cast in the scene. Without light, the 3D object cannot  
            // be seen. Note: to illuminate an object from additional directions, create  
            // additional lights.
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);


            //add the directional light to the models
            myModel3DBoard.Children.Add(myDirectionalLight);

    
            myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.Honeydew;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            myModel3DBoard.Children.Add(myDirectionalLight);

        } //lightingSetup


        private void loadTileMapToModels()
        {
            // Add the geometry model to the model group.
            foreach (Tile T in lstTiles)
            {
                //myModel3DBoard.Children.Add(appGameBoardTest.clsModels.GetGeo(T.p3D, T.B));
                myModel3DBoard.Children.Add(T.Geo);
            }
        } //loadTileMapToModels

        
        // What Tile am I on?
        public Tile getTileByPoint(Point3D p3D)     // Top Left of tile matches co-ordinates
        {
            foreach (Tile tmpTile in lstTiles)
            {
                if (tmpTile.p3D.X == p3D.X)
                {
                    if (tmpTile.p3D.Y == p3D.Y)
                    {
                        if (tmpTile.p3D.Z == p3D.Z)
                        {
                            return tmpTile;
                        }
                    }
                }
            }
            return null;
        } //getTileByPoint

        
        //Keypress handling from the Window/Form
        public void GBWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
            {
                if (inGameMode) { inGameMode = false; } else { inGameMode = true; }
                //this.updateGBInfo();

            }
            // YellowMan1 movement keys
            if (e.Key == Key.S)
            {
                YellowMan1.Movement.Vector = Vector3D_Compass.South; // South
                MovementQueue_ByBlock.Enqueue(YellowMan1);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                
                simpleSound.Play();
            }
            else if (e.Key == Key.A)
            {
                YellowMan1.Movement.Vector = Vector3D_Compass.West; // West
                MovementQueue_ByBlock.Enqueue(YellowMan1);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.D)
            {
                YellowMan1.Movement.Vector = Vector3D_Compass.East; // East
                MovementQueue_ByBlock.Enqueue(YellowMan1); SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.W)
            {
                YellowMan1.Movement.Vector = Vector3D_Compass.North; // North
                MovementQueue_ByBlock.Enqueue(YellowMan1);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            // Redman movement keys
            if (e.Key == Key.Down)
            {
                RedMan.Movement.Vector = Vector3D_Compass.South; // Sounth
                MovementQueue_ByBlock.Enqueue(RedMan);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Left)
            {
                RedMan.Movement.Vector = Vector3D_Compass.West; // West
                MovementQueue_ByBlock.Enqueue(RedMan);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Right)
            {
                RedMan.Movement.Vector = Vector3D_Compass.East; // East
                MovementQueue_ByBlock.Enqueue(RedMan);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Up)
            {
                RedMan.Movement.Vector = Vector3D_Compass.North; // North
                MovementQueue_ByBlock.Enqueue(RedMan);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Space)
            {
                Point3D addP3D = new Point3D(RedMan.Movement.Location.X, RedMan.Movement.Location.Y, RedMan.Movement.Location.Z - .1);
                Tile addTile = new Tile(appGameBoardTest.clsModels.GetGeo(addP3D, Brushes.DarkGreen));
                addTile.p3D.X = RedMan.Movement.Location.X;
                addTile.p3D.Y = RedMan.Movement.Location.Y;
                addTile.p3D.Z = RedMan.Movement.Location.Z - .1;
                addTile.B = Brushes.DarkGray;

                Tile tmpTile;
                tmpTile =  getTileByPoint(addTile.p3D);
                if (tmpTile != null) { return; }
                
                this.lstTiles.Add(addTile);
                //myModel3DGroup.Children.Add(appGameBoardTest.clsModels.GetGeo(T.p3D, T.B));
                myModel3DBoard.Children.Add(addTile.Geo);
            }
            //updateGBInfo();     // This will refresh the update window after the keypress is complete
        } //GBWindow_KeyDown

    } //GameBoard
}
