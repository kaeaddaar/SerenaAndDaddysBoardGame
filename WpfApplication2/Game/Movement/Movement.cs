using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;     // For Point3D use
using System.Windows.Media;             // For using Brush

namespace appGameBoardTest.Game.Movement
{

    class clsMovement
    {
        public static bool PushEntity(ref Components.Movement Movement, ref Components.Movable Movable, ref Game.GameBoard GB, ref Components.Movement P_Movement,
            ref Components.Movement origMovement)
        {
            if (!Movable.isMovable) { return false; }   // Only perform if entity is movable

            P_Movement.Vector.X = origMovement.Vector.X;
            P_Movement.Vector.Y = origMovement.Vector.Y;
            P_Movement.Vector.Z = origMovement.Vector.Z;
 
            GeometryModel3D tmpGBGeoMove = GB.GeoMove;

            tmpGBGeoMove = clsModels.GetGeo_98(new Point3D(P_Movement.Location.X + P_Movement.Vector.X, P_Movement.Location.Y + P_Movement.Vector.Y,
                P_Movement.Location.Z + P_Movement.Vector.Z), Brushes.AntiqueWhite);
            GB.myModelMovement.Children.Add(tmpGBGeoMove);
            GB.GB_Info.GeoMove_Location = tmpGBGeoMove.Bounds.ToString();
            GB.updateGBInfo();

            // Normal code to perform move below
            Tile tmpTile = null;
            if (!OnATile(ref tmpTile, ref P_Movement, ref GB))
            {   
                // Clean up the movement check square
                GB.myModelMovement.Children.Remove(tmpGBGeoMove);
                GB.GB_Info.GeoMove_Location = "Null";
                GB.updateGBInfo();

                return false;
            }     // If inGameMode then don't move to new location if it doesn't have a tile under it

            Components.Movement tmpMoveBounds;
            Components.Movement GeoMove_Wrapper = new Components.Movement(0);
            GeoMove_Wrapper.Geometry = tmpGBGeoMove;
            tmpMoveBounds = BoundingBoxCollision(GeoMove_Wrapper, GB);

            if (tmpMoveBounds != null)    // if there is there a bounding box collision then don't move
            {
                // Clean up the movement check square
                GB.myModelMovement.Children.Remove(tmpGBGeoMove);
                GB.GB_Info.GeoMove_Location = "Null";
                GB.updateGBInfo();

                return false;
            }

            // Clean up the movement check square
            GB.myModelMovement.Children.Remove(tmpGBGeoMove);
            GB.GB_Info.GeoMove_Location = "Null";
            GB.updateGBInfo();

            P_Movement.doMove();
            //GB.updateGBInfo();

            return true;
        } //PushEntity


        private static bool OnATile(ref Tile tmpTile, ref Components.Movement Movement, ref GameBoard GB)
        {
            if (GB.inGameMode)     // If inGameMode then don't move to new location if it doesn't have a tile under it
            {
                tmpTile = GB.getTileByPoint(new Point3D(Movement.Location.X + Movement.Vector.X, Movement.Location.Y +
                    Movement.Vector.Y, Movement.Location.Z + Movement.Vector.Z - .1));
                if (tmpTile == null) { return false; }    // If tile is null (not found) then exit 
            }
            return true;
        } //OnATile


        public static bool MoveEntity(ref Components.Movement Movement, ref Components.Movable Movable, Game.GameBoard GB)
        {
            // Try to draw a model then sleep for .5 sec
            GB.GeoMove = clsModels.GetGeo_98(new Point3D(Movement.Location.X + Movement.Vector.X, Movement.Location.Y + Movement.Vector.Y, 
                Movement.Location.Z + Movement.Vector.Z), Brushes.Black);
            GB.myModelMovement.Children.Add(GB.GeoMove);
            GB.GB_Info.GeoMove_Location = GB.GeoMove.Bounds.ToString();
            GB.updateGBInfo();

            // Normal code to perform move below
            Tile tmpTile = null;
            if (!OnATile(ref tmpTile, ref Movement, ref GB)) // If inGameMode then don't move to new location if it doesn't have a tile under it
            {                     // Clean up the movement check square
                GB.myModelMovement.Children.Remove(GB.GeoMove);
                GB.GB_Info.GeoMove_Location = "Null";
                GB.updateGBInfo();

                return false;
            }     

            Components.Movement tmpMoveBounds;
            Components.Movement GeoMove_Wrapper = new Components.Movement(0);
            GeoMove_Wrapper.Geometry = GB.GeoMove;
            tmpMoveBounds = BoundingBoxCollision(GeoMove_Wrapper, GB);
            
            if (tmpMoveBounds != null)    // if there is a bounding box collision then try to push an entity
            {   
                Components.Movement tmpM = GB.dictMovement[tmpMoveBounds.EntityID];
                Components.Movable tmpMable = GB.dictMovable[tmpMoveBounds.EntityID];
                if (!PushEntity(ref tmpM, ref tmpMable,ref GB, ref tmpMoveBounds, ref Movement))  
                {
                    // Clean up the movement check square
                    GB.myModelMovement.Children.Remove(GB.GeoMove);
                    GB.GB_Info.GeoMove_Location = "Null";
                    GB.updateGBInfo();
                    
                    return false;
                }
            }

            // Clean up the movement check square
            GB.myModelMovement.Children.Remove(GB.GeoMove);
            GB.GB_Info.GeoMove_Location = "Null";
            GB.updateGBInfo();

            Movement.doMove();
            //GB.updateGBInfo();

            return true;    // This indicates move was successful
        } //MoveEntity


        public static GeometryModel3D RectCollision(Components.Movement Movement, Game.GameBoard GB)
        {
            foreach (GeometryModel3D tmpModel3D in GB.myModel3DDisplay.Children)
            {
                if (Movement.Geometry.Bounds.IntersectsWith(tmpModel3D.Bounds))
                { return tmpModel3D; }
            }
            return null;
        }


        // This collission detection routine returns true if collision detected. Does so only on top left of XY right now. Will need to be upgraded
        public static Components.Movement BoundingBoxCollision(Components.Movement Movement, Game.GameBoard GB)
        {
            foreach (Components.Movement M in GB.dictMovement.Values)
            {
                if (M.EntityID != Movement.EntityID)
                {
                    if (M.Geometry.Bounds.IntersectsWith(Movement.Geometry.Bounds))
                    {return M;}
                }
            }
            return null;
        } //BoundingBoxCollisionTest

    }
}
