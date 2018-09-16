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

namespace appGameBoardTest.Game
{
    public class GameBoard_V1a      // Refactoring Gameboard and applying SOLID principles of OOP   
    {
        // Responsible for gameboard, responsible to game board design team only
        Model3DGroup myModel3DBoard;       // Tiles for the board go here
        GameBoard_V1a()
        {
            myModel3DBoard = new Model3DGroup();
        }
    }

    public class GameVisuals
    {
        public Viewport3D myViewport3D;      // viewport gets tied to MainWindow.Content from MainWindow so needs to be public
        public PerspectiveCamera myPCamera;
        public Model3DGroup myModel3DDisplay = new Model3DGroup();     // All other group will go into this group before getting displayed
        GameVisuals()
        {
            // Responsible for visual elements like camera so things can link to or use the visual elements
            myViewport3D = new Viewport3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object, 
            // the camera must be positioned and pointed such that the object is within view  
            // of the camera.
            PerspectiveCamera myPCamera = new PerspectiveCamera();
            myModel3DDisplay = new Model3DGroup();
        }
    }

    public class GameLoop
    {
        System.Timers.Timer myTimer;
        private GameBoard GB;
        public bool TimeToFireEvent;

        public GameLoop()
        {
            myTimer = new System.Timers.Timer();
            TimeToFireEvent = false;
        }

        public GameLoop(ref GameBoard GBRef)
        {
            myTimer = new System.Timers.Timer();
            TimeToFireEvent = false;
            GB = GBRef;
        }

        public void MainLoop()
        {
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler( execLoop );
            myTimer.Interval = 500;
            myTimer.Start();
        }

        public void execLoop( object source, System.Timers.ElapsedEventArgs e)
        {
            TimeToFireEvent = true;
        }
    }


    public class GBPieces
    {

    }

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
        Window2 winInfo = new Window2();
        public Game.UserInerface.GBInfo GB_Info;
        public GeometryModel3D GeoMove;

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

            GB_Info = new UserInerface.GBInfo(winInfo);
            setup_info_window();
            updateGBInfo();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.5);
            timer.Tick += timer_Tick;
            timer.Start();
            //if (canSee(this)){MessageBox.Show("YellowMan2 can see RedMan.");}
        } //GameBoard

        void timer_Tick(object sender, EventArgs e)
        {
            Entities.Player Piece = YellowMan2;
            Piece.Movement.Vector.X = 0; Piece.Movement.Vector.Y = 1; Piece.Movement.Vector.Z = 0;  
            Game.Movement.clsMovement.MoveEntity(ref Piece.Movement, ref Piece.Movable, this);
        }

        public bool canSee(Game.GameBoard GB)
        {
            Vector3D VSource = new Vector3D(YellowMan2.Movement.Geometry.Bounds.Location.X, YellowMan2.Movement.Geometry.Bounds.Location.Y, YellowMan2.Movement.Geometry.Bounds.Location.Z);
            Vector3D VTarget = new Vector3D(RedMan.Movement.Geometry.Bounds.Location.X, RedMan.Movement.Geometry.Bounds.Location.Y, RedMan.Movement.Geometry.Bounds.Location.Z);
            Vector3D zeroBased = new Vector3D();
            zeroBased = VTarget - VSource;        // Save a step by subtracting the Target from the Souce to get the vector to the target instead of zero based target

            foreach (Entities.Player tmpPlayer in dictPlayer.Values)
            { 
                if (tmpPlayer.EntityID != YellowMan2.EntityID && tmpPlayer.EntityID != RedMan.EntityID)  //Prevent checking against yerself
                {
                    //Vector3D tmpV = new Vector3D(YellowMan2.Movement.Geometry.Bounds.Location.X, tmpPlayer.Movement.Geometry.Bounds.Location.Y, 
                    //    tmpPlayer.Movement.Geometry.Bounds.Location.Z);
                    Vector3D tmpV = VTarget - VSource;
                    tmpV.Normalize();
                    RayHitTestParameters tmpRay = new RayHitTestParameters(YellowMan2.Movement.Geometry.Bounds.Location,tmpV);
                    List<Point3D> tmpIntersection = new List<Point3D>();
                    
                    if (MathHelper.clsMathAssist.HitTestPointsTowards(tmpRay, tmpPlayer.Movement.Geometry.Bounds))

                    if (MathHelper.clsMathAssist.HitTestManual(tmpRay, tmpPlayer.Movement.Geometry.Bounds, ref tmpIntersection,GB))
                    {
                        return false;
                    }
                }
            }
            return true;   //If you didn't find something in the way then you can see the object
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


        public void HitTest_Fuzzle()
        {
            Point mouseposition = new Point(YellowMan1.Movement.Location.X, YellowMan1.Movement.Location.Y);
            Point3D testpoint3D = YellowMan1.Movement.Location;
            Vector3D testdirection = new Vector3D(RedMan.Movement.Location.X - YellowMan1.Movement.Location.X,
                RedMan.Movement.Location.Y - YellowMan1.Movement.Location.Y, RedMan.Movement.Location.Z - YellowMan1.Movement.Location.Z);

            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);

            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            
            //Test all directions
            for (int i = -1; i <= 1; i++ )
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        RayHitTestParameters tmpRayParams = new RayHitTestParameters(testpoint3D, new Vector3D(i,j,k));
                        //test for a result in the Viewport3D
                        VisualTreeHelper.HitTest(myViewport3D, null, HTResult, pointparams);
                    }
                }
            }

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


                    //// START ----- take item we click on and convert the image to SpaceRockTri ----- START
                    //ImageBrush B = new ImageBrush();
                    //B.ImageSource = new BitmapImage(new Uri(@"..\..\Game\Images\Items\SpaceRockTri.jpg", UriKind.Relative));

                    //MeshGeometry3D myMeshGeometry3D = (MeshGeometry3D)hitgeo.Geometry;

                    //PointCollection myTextureCoordinatesCollection = new PointCollection();
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
                    //myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
                    //myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

                    //DiffuseMaterial myMaterial = new DiffuseMaterial(B);
                    //hitgeo.Material = myMaterial;
                    //// END ----- take item we click on and convert the image to SpaceRockTri ----- END


                    //UpdateResultInfo(rayMeshResult);
                    //UpdateMaterial(hitgeo, (side1GeometryModel3D.Material as MaterialGroup));
                }
            }

            return HitTestResultBehavior.Continue;
        } //HTResult


        public void updateGBInfo()
        {
            GB_Info.RedMan_Location = "" + RedMan.Movement.Location;
            GB_Info.RedMan_Geo_Location = "" + RedMan.Movement.Geometry.Bounds;
            GB_Info.numTiles = "" + lstTiles.Count;


            GB_Info.AppendToEnd = "Append String (Not used yet)";
            GB_Info.AppendToEnd = "RedMan(Blue) Bounds: " + RedMan.Movement.Geometry.Bounds + System.Environment.NewLine;
            GB_Info.AppendToEnd = GB_Info.AppendToEnd +"RedMan Location: " + RedMan.Movement.Location;

            GB_Info.AppendToEnd = GB_Info.AppendToEnd + "YellowMan2 Bounds: " + YellowMan2.Movement.Geometry.Bounds + System.Environment.NewLine;
            GB_Info.AppendToEnd = GB_Info.AppendToEnd + "YellowMan2 Location: " + YellowMan2.Movement.Location + System.Environment.NewLine;

            //Display the vector information for YellowMan2 to Redman
            Vector3D VSource = new Vector3D(YellowMan2.Movement.Geometry.Bounds.Location.X, YellowMan2.Movement.Geometry.Bounds.Location.Y, YellowMan2.Movement.Geometry.Bounds.Location.Z);
            Vector3D VTarget = new Vector3D(RedMan.Movement.Geometry.Bounds.Location.X, RedMan.Movement.Geometry.Bounds.Location.Y, RedMan.Movement.Geometry.Bounds.Location.Z);
            Vector3D zeroBased = new Vector3D();
            zeroBased = VTarget-VSource;        // Save a step by subtraccting the Target from the Souce to get the vector to the target instead of zero based target
            Vector3D zeroManual = new Vector3D(VSource.X - VTarget.X, VSource.Y - VTarget.Y, VSource.Z - VTarget.Z);
            zeroManual.Negate();

            GB_Info.AppendToEnd = GB_Info.AppendToEnd + "Vector Yellowman2 - Vector RedMan: " + zeroBased + System.Environment.NewLine;
            GB_Info.AppendToEnd = GB_Info.AppendToEnd + "Vector Yellowman2 - Vector RedMan(Manual): " + zeroManual + System.Environment.NewLine;

            GB_Info.updateInfoWindow();
        } //updateGBInfo


        private void setup_info_window()
        {
            winInfo.Top = 30;
            winInfo.Left = 30;
            winInfo.Height = 930;
            winInfo.Width = 330;
            winInfo.Show();
            winInfo.txt1.Height = 900;
            winInfo.txt1.Width = 300;

        } //setup_info_window

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
                this.updateGBInfo();
                if (canSee(this)) { /*MessageBox.Show("YellowMan2 can see RedMan.");*/ }
                else { MessageBox.Show("YellowMan2 can't see Redman."); }
            }
            // YellowMan1 movement keys
            if (e.Key == Key.S)
            {
                YellowMan1.Movement.Vector.X = 0; YellowMan1.Movement.Vector.Y = -1; YellowMan1.Movement.Vector.Z = 0;  // Sounth
                Game.Movement.clsMovement.MoveEntity(ref YellowMan1.Movement, ref YellowMan1.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                
                simpleSound.Play();
            }
            else if (e.Key == Key.A)
            {
                YellowMan1.Movement.Vector.X = -1; YellowMan1.Movement.Vector.Y = 0; YellowMan1.Movement.Vector.Z = 0;  // West
                Game.Movement.clsMovement.MoveEntity(ref YellowMan1.Movement, ref YellowMan1.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.D)
            {
                YellowMan1.Movement.Vector.X = 1; YellowMan1.Movement.Vector.Y = 0; YellowMan1.Movement.Vector.Z = 0;  // East
                Game.Movement.clsMovement.MoveEntity(ref YellowMan1.Movement, ref YellowMan1.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.W)
            {
                YellowMan1.Movement.Vector.X = 0; YellowMan1.Movement.Vector.Y = 1; YellowMan1.Movement.Vector.Z = 0;  // North
                Game.Movement.clsMovement.MoveEntity(ref YellowMan1.Movement, ref YellowMan1.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\smb_jump-small.wav");
                simpleSound.Play();
            }
            // Redman movement keys
            if (e.Key == Key.Down)
            {
                RedMan.Movement.Vector.X = 0; RedMan.Movement.Vector.Y = -1; RedMan.Movement.Vector.Z = 0;  // Sounth
                Game.Movement.clsMovement.MoveEntity(ref RedMan.Movement, ref RedMan.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Left)
            {
                RedMan.Movement.Vector.X = -1; RedMan.Movement.Vector.Y = 0; RedMan.Movement.Vector.Z = 0;  // West
                Game.Movement.clsMovement.MoveEntity(ref RedMan.Movement, ref RedMan.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Right)
            {
                RedMan.Movement.Vector.X = 1; RedMan.Movement.Vector.Y = 0; RedMan.Movement.Vector.Z = 0;  // East
                Game.Movement.clsMovement.MoveEntity(ref RedMan.Movement, ref RedMan.Movable, this);
                SoundPlayer simpleSound = new SoundPlayer(Environment.CurrentDirectory + @"\..\..\Game\GameSounds\TappingAlong.wav");
                simpleSound.Play();
            }
            else if (e.Key == Key.Up)
            {
                RedMan.Movement.Vector.X = 0; RedMan.Movement.Vector.Y = 1; RedMan.Movement.Vector.Z = 0;  // North
                Game.Movement.clsMovement.MoveEntity(ref RedMan.Movement, ref RedMan.Movable, this);
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
            updateGBInfo();     // This will refresh the update window after the keypress is complete
        } //GBWindow_KeyDown

    } //GameBoard
}
