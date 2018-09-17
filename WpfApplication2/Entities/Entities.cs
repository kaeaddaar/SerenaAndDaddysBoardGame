using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;     // For using Point3D
using System.Windows.Media;             // For using Brush

namespace appGameBoardTest.Entities
{
    public interface IPlayer : IBaseEntity
    {
        Components.Character_Containers Character_Containers { get; set; }
        Components.Character_Attributes Character_Attributes { get; set; }
        void doMove();
    }

    public class Player : BaseEntity, IPlayer
    {
        //public UInt32 EntityID;
        //public String EntityName;
        //public appGameBoardTest.Game.GameBoard GB;

        //public Components.Movement Movement;
        //public Components.Movable Movable = new Components.Movable();

        public Components.Character_Containers Character_Containers { get; set; }
        public Components.Character_Attributes Character_Attributes { get; set; }

        public void doMove()
        {
            TranslateTransform3D tMove = new TranslateTransform3D();

            // Calculate where we are moving the Player to
            tMove.OffsetX = Movement.Location.X + Movement.Vector.X;
            tMove.OffsetY = Movement.Location.Y + Movement.Vector.Y;
            tMove.OffsetZ = Movement.Location.Z + Movement.Vector.Z;

            // Apply the move to the Geometry
            Movement.Geometry.Transform = tMove;

            // Update the location to indicate that the movement has taken place
            Movement.Location.X = Movement.Location.X + Movement.Vector.X;
            Movement.Location.Y = Movement.Location.Y + Movement.Vector.Y;
            Movement.Location.Z = Movement.Location.Z + Movement.Vector.Z;

            // Indicate that the object has stopped, since the move has been completed.
            Movement.Vector.X = 0;
            Movement.Vector.Y = 0;
            Movement.Vector.Z = 0;
        }


        public Player(ref UInt32 nextEntityId, Game.GameBoard GB) : base (ref nextEntityId, GB)
        {
            //this.EntityID = nextEntityId;
            //nextEntityId++;
            //// Movement component
            //this.Movement.EntityID = this.EntityID;
            //this.Movement = new Components.Movement(nextEntityId);
            //GB.dictMovement.Add(this.EntityID, Movement);
            //// Movable component
            //Movable.EntityID = this.EntityID;
            //GB.dictMovable.Add(this.EntityID, Movable);
            Character_Containers = new Components.Character_Containers();
            Character_Attributes = new Components.Character_Attributes();

            //Character_Containers component
            Character_Containers.EntityID = this.EntityID;
            GB.dictCharacter_Containers.Add(this.EntityID, Character_Containers);
            //Character_Attributes component
            Character_Attributes.EntityID = this.EntityID;
            GB.dictCharacter_Attributes.Add(this.EntityID, Character_Attributes);
        }


        public Player(Point3D Location, Brush B, Vector3D Vector, ref UInt32 nextEntityId, Game.GameBoard GB, String tmpName) :
            base(Location, B, Vector, ref nextEntityId, GB, tmpName)
        {
            //this.EntityID = nextEntityId;
            //nextEntityId++;
            //// Movement component
            //this.Movement = new Components.Movement(this.EntityID);
            ////this.Movement.EntityID = this.EntityID;
            //GB.dictMovement.Add(this.EntityID, Movement);
            //// Movable component
            //Movable.EntityID = this.EntityID;
            //GB.dictMovable.Add(this.EntityID, Movable);

            Character_Containers = new Components.Character_Containers();
            Character_Attributes = new Components.Character_Attributes();

            //Character_Containers component
            Character_Containers.EntityID = this.EntityID;
            GB.dictCharacter_Containers.Add(this.EntityID, Character_Containers);
            //Character_Attributes component
            Character_Attributes.EntityID = this.EntityID;
            GB.dictCharacter_Attributes.Add(this.EntityID, Character_Attributes);

            //EntityName = tmpName;  // Get nextEntityId and set name

            //// Initialize Movement based on settings passed in
            //Movement.Location = Location;
            //Movement.Geometry = clsModels.GetGeo_98(new Point3D(Movement.Location.X, Movement.Location.Y,
            //    Movement.Location.Z), B);
            //Movement.Vector = Vector;

            //GB.dictPlayer.Add(EntityID, this);
        }
    }

    public interface IBaseEntity
    {
        UInt32 EntityID { get; set; }
        String EntityName { get; set; }
        Components.Movement Movement { get; set; }
        Components.Movable Movable { get; }
    }

    public class BaseEntity : IBaseEntity
    {
        public UInt32 EntityID { get; set; }
        public String EntityName { get; set; }
        public appGameBoardTest.Game.GameBoard GB { get; set; }

        public Components.Movement Movement { get; set; }
        public Components.Movable Movable => new Components.Movable();

        public BaseEntity(ref UInt32 nextEntityId, Game.GameBoard GB)
        {
            this.EntityID = nextEntityId;
            nextEntityId++;
            // Movement component
            Movement.EntityID = this.EntityID;
            this.Movement = new Components.Movement(this.EntityID);
            GB.dictMovement.Add(this.EntityID, Movement);
            // Movable component
            Movable.EntityID = this.EntityID;
            GB.dictMovable.Add(this.EntityID, Movable);
        }

        public BaseEntity(Point3D Location, Brush B, Vector3D Vector, ref UInt32 nextEntityId, Game.GameBoard GB, String tmpName)
        {
            this.EntityID = nextEntityId;
            nextEntityId++;
            // Movement component
            this.Movement = new Components.Movement(this.EntityID);
            //            Movement.EntityID = this.EntityID;
            GB.dictMovement.Add(this.EntityID, Movement);
            // Movable component
            Movable.EntityID = this.EntityID;
            GB.dictMovable.Add(this.EntityID, Movable);

            EntityName = tmpName;  // Get nextEntityId and set name

            // Initialize Movement based on settings passed in
            Movement.Location = Location;
            Movement.Geometry = clsModels.GetGeo_98(new Point3D(Movement.Location.X, Movement.Location.Y,
                Movement.Location.Z), B);
            Movement.Vector = Vector;

            GB.dictBox.Add(EntityID, this);
        }
    }

}
