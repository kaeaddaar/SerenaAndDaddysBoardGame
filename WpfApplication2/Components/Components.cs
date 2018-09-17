using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows;  // for point

namespace appGameBoardTest.Components
{
    class clsComponents
    {
        // Static component methods can go here.
    }

    public static class Vector3D_Compass
    {
        public static Vector3D North { get { return new Vector3D(0, 1, 0); } }
        public static Vector3D South { get { return new Vector3D(0, -1, 0); } }
        public static Vector3D East { get { return new Vector3D(1, 0, 0); } }
        public static Vector3D West { get { return new Vector3D(-1, 0, 0); } }
    }

    public class Movement
    {
        public UInt32 EntityID;

        public Point3D Location;
        public Vector3D Vector;
        public GeometryModel3D Geometry;

        public Movement(UInt32 EntityID)
        {
            this.EntityID = EntityID;
        }


        public Movement(UInt32 EntityID, Point3D Location,  Vector3D Vector, GeometryModel3D Geometry, Game.GameBoard GB)
        {
            this.EntityID = EntityID;
            
            this.Location = Location;
            this.Vector = Vector;
            this.Geometry = Geometry;
            GB.dictMovement.Add(this.EntityID, this);
        }

        
        public void doMove()
        {
            TranslateTransform3D tMove = new TranslateTransform3D();

            // Calculate where we are moving the Player to
            tMove.OffsetX = this.Location.X + this.Vector.X;
            tMove.OffsetY = this.Location.Y + this.Vector.Y;
            tMove.OffsetZ = this.Location.Z + this.Vector.Z;

            // Apply the move to the Geometry
            this.Geometry.Transform = tMove;

            // Update the location to indicate that the movement has taken place
            this.Location.X = this.Location.X + this.Vector.X;
            this.Location.Y = this.Location.Y + this.Vector.Y;
            this.Location.Z = this.Location.Z + this.Vector.Z;

            // Indicate that the object has stopped, since the move has been completed.
            this.Vector.X = 0;
            this.Vector.Y = 0;
            this.Vector.Z = 0;
        }
    } //Movement


    public class Movable
    {
        public UInt32 EntityID;

        public Boolean isMovable = true;
    } //Movable


    public abstract class Container
    {
        public UInt32 ContainerId;
        public String ContainerName;
        public String Contains;
        public double MaxQty = 1;
    } //Container


    public class Container_Qty : Container
    {
        public double Qty;
    } //Container_Qty


    public class Container_Item : Container
    {
        public Dictionary<UInt32, Item> dictItems;

    } //Container_Item


    public class Item
    {
        public UInt32 ItemId;
        public ItemType ItemType;
        public String Name;
        public Dictionary<UInt32, Object> Components;        //Container_Qty, or Container_Items
        //Attack
        //Defense
        //Physical

    } //Item


    public class Character_Containers
    {
        public UInt32 EntityID;

        public Container_Qty Health = new Container_Qty();
        public Container_Qty Learning = new Container_Qty();
        public Container_Qty Focus = new Container_Qty();
        public Container_Qty Mana = new Container_Qty();
        public Container_Qty Movement = new Container_Qty();

        public Container_Item Armor = new Container_Item();
        public Container_Item Weapon = new Container_Item();
    } //Character_Containers

            
    public class Character_Attributes
    {
        public UInt32 EntityID;

        public string Name;
        public Character_Class Class;
        public Character_Sex Sex;
        public double Height_Meters;
        public double Strength;
        public double Constitution;
        public double Dexterity;
        public double Charisma;
        public double Intelligence;
        public double Wizdom;
    } //Character_Attributes

}
