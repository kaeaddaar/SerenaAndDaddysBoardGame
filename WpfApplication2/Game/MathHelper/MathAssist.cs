using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;     // Point3D, Vector3D
using System.Windows;                   // Point, Vector


namespace appGameBoardTest.Game.MathHelper
{
    class clsMathAssist
    {
        // Any static functions can go here
        public static void test()
        {
            return;
        }

        
        // Calculate the dot product of a ray against a target, or on a zero based vector. I wrote this from scratch :)
        // Where a Ray is composed of a Source Point and a Source Vector
        // Positive results mean this Ray points towards the target point
        // 0 result means this Ray is perpendicular to the point
        // Negative results mean this Ray points away form the target point
        // The following sets of functions for calculating dot product and norms are for 2 dimentional and 3 dimentional vectors (May add more later)

        public static double dot2(Vector U1, Vector U2)
        {
            return U1.X * U2.X + U1.Y * U2.Y;
        } //dot2


        public static double dot2(Point RaySourcePt, Vector RaySourceVector, Point TargetPoint)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0)
            Vector TargetVector = new Vector(TargetPoint.X - RaySourcePt.X, TargetPoint.Y - RaySourcePt.Y);
            return dot2(RaySourceVector, TargetVector);
        } //dot2


        public static double dot3(Vector3D U1, Vector3D U2)
        {
            return U1.X * U2.X + U1.Y * U2.Y + U1.Z * U2.Z;
        } //dot3


        // Would I be able to write the same for a three dimensional vector?
        public static double dot3(Point3D RaySourcePt, Vector3D RaySourceVector, Point3D TargetPoint)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVector = new Vector3D(TargetPoint.X - RaySourcePt.X, TargetPoint.Y - RaySourcePt.Y, TargetPoint.Z - RaySourcePt.Z);
            return dot3(RaySourceVector, TargetVector);
        } //dot3


        public static double dot3(RayHitTestParameters Ray, Point3D TargetPoint)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVector = new Vector3D(TargetPoint.X - Ray.Origin.X, TargetPoint.Y - Ray.Origin.Y, TargetPoint.Z - Ray.Origin.Z);
            return dot3(Ray.Direction, TargetVector);
        } //dot3


        //The norm of a vector is the length of a vector (like Pythagorean theorem)
        //In linear algebra the norm of U is ||U|| = SquareRoot(U dot U) where dot is the dot product operator
        public static double Norm2(Vector U)    // meant to use a zero based vector
        {
            return Math.Sqrt(dot2(U, U));       // ||U|| = SquareRoot(U dot U)  // return the distance
        } //Norm2


        //The norm of a vector is the length of a vector (like Pythagorean theorem)
        //In linear algebra the norm of U is ||U|| = SquareRoot(U dot U) where dot is the dot product operator
        public static double Norm2(Point RaySourcePt, Point TargetPoint)    // meant to use two points
        {
            // This will be become a zero based vector (check terminology) by translating both points so the source = (0,0)
            Vector TargetVector = new Vector(TargetPoint.X - RaySourcePt.X, TargetPoint.Y - RaySourcePt.Y);
            return Math.Sqrt(dot2(TargetVector,TargetVector));
        } //Norm2

        
        public static double Norm3(RayHitTestParameters Ray, Point3D TargetPoint)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVector = new Vector3D(TargetPoint.X - Ray.Origin.X, TargetPoint.Y - Ray.Origin.Y, TargetPoint.Z - Ray.Origin.Z);
            return Math.Sqrt(dot3(TargetVector, TargetVector));
        } //Norm3


        public static double Norm3(Point3D RaySourcePt, Point3D TargetPoint)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVector = new Vector3D(TargetPoint.X - RaySourcePt.X, TargetPoint.Y - RaySourcePt.Y, TargetPoint.Z - RaySourcePt.Z);
            return Math.Sqrt(dot3(TargetVector, TargetVector));
        } //Norm3


        //If I want to calculate if my ray intersects a bounding box, what do I do?
        // * Anything with X, Y, or Z co-ordinates further away from the source than the distance to target can be cut
        // * Anything that is not facing one of the verticies of the bounding box can be cut (dot3(Ray, TargetPoint)) where we test against each of the vertices
        //   of the bounding box. ??
        // * For each plane (ex: Z) calculate the magnitude of Z
        //   - Magnitude of Z = absoluteVal(TargetVector.Z)  // Where TargetVector is a zero based vector of a Ray pointing at the target
        //   - RayPoint + TargetVector = IntersectionOfPlane
        //   - IntersectionOfPlane.X and IntersectionOfPlane.Y are within the bounded plane then the ray intersects that bounded plane


        // Returns true if ray pointing towards any point in the bounding box
        public static bool HitTestPointsTowards(RayHitTestParameters Ray, Rect3D BoundingBox)
        {
            for (double iX = 0; iX <= BoundingBox.SizeX;iX = iX + BoundingBox.SizeX )
            {
                for (double iY = 0; iY <= BoundingBox.SizeY; iY = iY + BoundingBox.SizeY)
                {
                   // for (double iZ = 0; iZ <= BoundingBox.SizeZ; iZ = iZ + BoundingBox.SizeZ)
                    //{
                     //   if (dot3(Ray, new Point3D(BoundingBox.X, BoundingBox.Y, BoundingBox.Z)) > 0) { return true; }
                    //}
                }
            }

            return false;  // The ray didn't point towards any of the points in the bounding box
        } //HitTestPointsTowards


        //Returns true if bounding box is out of range of sight distance
        public static bool HitTestOutOfRangeXYZ(RayHitTestParameters Ray, Rect3D BoundingBox, double sightDistance)
        {
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVectorBottomLeftLower = new Vector3D(BoundingBox.X - Ray.Origin.X, BoundingBox.Y - Ray.Origin.Y, BoundingBox.Z - Ray.Origin.Z);
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVectorTopRightUpper = new Vector3D(BoundingBox.X + BoundingBox.SizeX - Ray.Origin.X, BoundingBox.Y + BoundingBox.SizeY - Ray.Origin.Y, 
                BoundingBox.Z + BoundingBox.SizeZ - Ray.Origin.Z);

            if (TargetVectorBottomLeftLower.X > sightDistance && TargetVectorTopRightUpper.X > sightDistance) { return true; }
            if (TargetVectorBottomLeftLower.Y > sightDistance && TargetVectorTopRightUpper.Y > sightDistance) { return true; }
            if (TargetVectorBottomLeftLower.Z > sightDistance && TargetVectorTopRightUpper.Z > sightDistance) { return true; }
            if (Norm3(Ray, new Point3D((TargetVectorBottomLeftLower.X + TargetVectorTopRightUpper.X)/2,(TargetVectorBottomLeftLower.Y + TargetVectorTopRightUpper.Y)/2,
                (TargetVectorBottomLeftLower.Z + TargetVectorTopRightUpper.Z)/2) ) > sightDistance + 0.56)  // the .56 is a cheat so I don't have to get closest point
            { return true; }

            return false;   // if we get to here then at least part of the bounding box is in range
        } //HitTestOutOfRangeXYZ


        //HitTestmanual will perform the hit test, but we will use another function to loop through the GeometryModel3D objects and do cutting tests from above
        public static bool HitTestManual(RayHitTestParameters Ray, Rect3D BB_IsItInTheWay, ref List<Point3D> lstIntersections, Game.GameBoard GB)       // Stop at first occurance of hit
        {   
            //start by making sure the vector on the ray is normallized
            Ray.Direction.Normalize();

           // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D CheckBBVectorBottomLeftLower = new Vector3D(BB_IsItInTheWay.X - Ray.Origin.X, BB_IsItInTheWay.Y - Ray.Origin.Y, BB_IsItInTheWay.Z - Ray.Origin.Z);

            CheckBBVectorBottomLeftLower.Normalize();
            for (int X = 0; X <= 1; X++)    //Check all planes for all points (There is definately duplication, I should be able to optimize of common planes
            {
                for (int Y = 0; Y <= 1; Y++)
                {
                    for (int Z = 0; Z <= 1; Z++)
                    {
                        if (dot3(Ray, new Point3D(BB_IsItInTheWay.X + BB_IsItInTheWay.SizeX * X, BB_IsItInTheWay.Y + BB_IsItInTheWay.SizeY * Y, 
                            BB_IsItInTheWay.Z + BB_IsItInTheWay.SizeZ * Z)) > 0)    //Figure out if the ray is pointing towards the point (add same pt logic later)
                        { // get point at which this vector intersects each of the axis aligned planes attached to this point 
                            // start by taking Ray.Origin.Z (The normalized vector from) and making it 1
                            // 1 - Z = Z1 (the number to get to 1)
                            double normPts = Norm3(Ray.Origin, new Point3D(BB_IsItInTheWay.X + BB_IsItInTheWay.SizeX * X, BB_IsItInTheWay.Y + BB_IsItInTheWay.SizeY * Y,
                                BB_IsItInTheWay.Z + BB_IsItInTheWay.SizeZ * Z));    //Just in case I need to use it
                            
                            //Use the absolute value of from the vector to allow us to extend the points on the vector by distance along axis
                            double X1 = Math.Abs(Ray.Direction.X);
                            double Y1 = Math.Abs(Ray.Direction.Y);
                            double Z1 = Math.Abs(Ray.Direction.Z);
                            // The distance between the ray's source point and the Bounding Box Point along axis
                            double diffX = Math.Abs(Ray.Origin.X - BB_IsItInTheWay.X);
                            double diffY = Math.Abs(Ray.Origin.Y - BB_IsItInTheWay.Y);
                            double diffZ = Math.Abs(Ray.Origin.Z - BB_IsItInTheWay.Z);

                            Vector3D IntersectionVectorYZ = new Vector3D();
                            IntersectionVectorYZ = Ray.Direction * X1 * diffX;
                            Vector3D IntersectionVectorXZ = new Vector3D();
                            IntersectionVectorXZ = Ray.Direction * Y1 * diffY;
                            Vector3D IntersectionVectorXY = new Vector3D();
                            IntersectionVectorXY = Ray.Direction * Z1 * diffZ;
                            
                            // Which planes intersect within a bounded plane YZ
                            if (IntersectionVectorYZ.Y >= BB_IsItInTheWay.Y + BB_IsItInTheWay.SizeY &&
                                IntersectionVectorYZ.Z <= BB_IsItInTheWay.Z + BB_IsItInTheWay.SizeZ)
                            { lstIntersections.Add(new Point3D(IntersectionVectorYZ.X, IntersectionVectorYZ.Y, IntersectionVectorYZ.Z)); return true; }

                            // Which planes intersect within a bounded plane XZ
                            if (IntersectionVectorXZ.Y >= BB_IsItInTheWay.X + BB_IsItInTheWay.SizeX &&
                                IntersectionVectorXZ.Z <= BB_IsItInTheWay.Z + BB_IsItInTheWay.SizeZ)
                            { lstIntersections.Add(new Point3D(IntersectionVectorXZ.X, IntersectionVectorXZ.Y, IntersectionVectorXZ.Z)); return true; }
                        
                            // Which planes intersect within a bounded plane XY
                            if (IntersectionVectorXY.Y >= BB_IsItInTheWay.X + BB_IsItInTheWay.SizeX &&
                                IntersectionVectorXY.Y <= BB_IsItInTheWay.Y + BB_IsItInTheWay.SizeY)
                            { lstIntersections.Add(new Point3D(IntersectionVectorXY.X, IntersectionVectorXY.Y, IntersectionVectorXY.Z)); return true; }

                        }  // if ray was pointing towards the point
                    } //End for Z
                } //End for Y
            }  //End for X
            
// The code in the commented out section below is replaced by the code above.
/*            
            // Check for intersection against the 3 bounded planes that share this point (XY, XZ, YZ)
            // Ex: XY is for checking against the Z plane
//            Point3D IntersectionOfPlaneBottomLeftLower = new Point3D(Ray.Origin.X + TargetVectorBottomLeftLower.X, Ray.Origin.Y + TargetVectorBottomLeftLower.Y,
//                Ray.Origin.Z + TargetVectorBottomLeftLower.Z);
            Point3D IntersectionOfPlaneBottomLeftLower = new Point3D(Ray.Origin.X + CheckBBVectorBottomLeftLower.X, Ray.Origin.Y + CheckBBVectorBottomLeftLower.Y,
                Ray.Origin.Z + CheckBBVectorBottomLeftLower.Z);

            // If the X value and Y values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane Z for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneBottomLeftLower.X, IntersectionOfPlaneBottomLeftLower.Y,
                BB_IsItInTheWay.X, BB_IsItInTheWay.Y, BB_IsItInTheWay.SizeX, BB_IsItInTheWay.SizeY))
            { lstIntersections.Add(IntersectionOfPlaneBottomLeftLower); return true; }

            // If the X value and Z values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane Y for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneBottomLeftLower.X, IntersectionOfPlaneBottomLeftLower.Z,
                BB_IsItInTheWay.X, BB_IsItInTheWay.Z, BB_IsItInTheWay.SizeX, BB_IsItInTheWay.SizeZ))
            { lstIntersections.Add(IntersectionOfPlaneBottomLeftLower); return true; }

            // If the Y value and Z values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane X for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneBottomLeftLower.Y, IntersectionOfPlaneBottomLeftLower.Z,
                BB_IsItInTheWay.Y, BB_IsItInTheWay.Z, BB_IsItInTheWay.SizeY, BB_IsItInTheWay.SizeZ))
            { lstIntersections.Add(IntersectionOfPlaneBottomLeftLower); return true; }


            // ----- Second set of faces to check -----
            // TargetVector will be become a zero based vector (check terminology) by translating both points so the source = (0,0,0)
            Vector3D TargetVectorTopRightUpper = new Vector3D(BB_IsItInTheWay.X + BB_IsItInTheWay.SizeX - Ray.Origin.X, BB_IsItInTheWay.Y + BB_IsItInTheWay.SizeY - Ray.Origin.Y, 
                BB_IsItInTheWay.Z + BB_IsItInTheWay.SizeZ - Ray.Origin.Z);

            // Check for intersection against the 3 bounded planes that share this point (XY, XZ, YZ)
            // Ex: XY is for checking against the Z plane
            Point3D IntersectionOfPlaneTopRightUpper = new Point3D(Ray.Origin.X + TargetVectorTopRightUpper.X, Ray.Origin.Y + TargetVectorTopRightUpper.Y,
                Ray.Origin.Z + TargetVectorTopRightUpper.Z);

            // If the X value and Y values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane Z for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneTopRightUpper.X, IntersectionOfPlaneTopRightUpper.Y,
                BB_IsItInTheWay.X, BB_IsItInTheWay.Y, BB_IsItInTheWay.SizeX, BB_IsItInTheWay.SizeY))
            { lstIntersections.Add(IntersectionOfPlaneBottomLeftLower); return true; }

            // If the X value and Z values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane Y for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneTopRightUpper.X, IntersectionOfPlaneTopRightUpper.Z,
                BB_IsItInTheWay.X, BB_IsItInTheWay.Z, BB_IsItInTheWay.SizeX, BB_IsItInTheWay.SizeZ))
            { lstIntersections.Add(IntersectionOfPlaneTopRightUpper); return true; }

            // If the Y value and Z values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane X for TargetVectorBottomLeftLower then we connect
            if (pointWithinBoundedPlane(IntersectionOfPlaneTopRightUpper.Y, IntersectionOfPlaneTopRightUpper.Z,
                BB_IsItInTheWay.Y, BB_IsItInTheWay.Z, BB_IsItInTheWay.SizeY, BB_IsItInTheWay.SizeZ))
            { lstIntersections.Add(IntersectionOfPlaneTopRightUpper); return true; }
*/
            return false;       // If nothing else was found we return false
            
        } //HitTestManual


        private static bool pointsTowardsBoundingBox(RayHitTestParameters Ray, Rect3D BB)
        {
            for (int X = 0; X <= 1; X++)
            {
                for (int Y = 0; Y <= 1; Y++)
                {
                    for (int Z = 0; Z <= 1; Z++)
                    {
                        if (pointsTowardsPlane(Ray.Origin.X, BB.X + BB.SizeX)) { return true; }
                        if (pointsTowardsPlane(Ray.Origin.Y, BB.Y + BB.SizeY)) { return true; }
                        if (pointsTowardsPlane(Ray.Origin.Z, BB.Z + BB.SizeZ)) { return true; }
                    }
                }
            }

            return false;
        }


        private static bool pointsTowardsBoundingBoxDot3(RayHitTestParameters Ray, Rect3D BB)
        {
            for (int X = 0; X <= 1; X++)
            {
                for (int Y = 0; Y <= 1; Y++)
                {
                    for (int Z = 0; Z <= 1; Z++)
                    {
                        if (dot3(Ray, new Point3D(BB.X + BB.SizeX * X, BB.Y + BB.SizeY * Y, BB.Z + BB.SizeZ * Z)) > 0) { return true; }
                    }
                }
            }

            return false;
        }


        private static bool pointsTowardsPlane(double RayAxis, double TargetAxis)
        {
            // R.Direction.X / Math.Abs(Ray.Direction.X) calculates whether you are moving in a positive(+1), parallel(0), or negative(-1) direction along that axis
            if (RayAxis == 0)
            { return false; }
            else
            {
                if (RayAxis / Math.Abs(RayAxis) == TargetAxis / Math.Abs(TargetAxis))
                { return true; }
                else { return false; }
            }
        } //pointsTowardsPlane

        
        private static bool pointWithinBoundedPlane(double vectorToOrigTargetAxis1, double vectorToOrigTargetAxis2, 
            double BoundingBoxAxis1, double BoundingBoxAxis2, double BoundingBoxSizeAxis1, double BoundingBoxSizeAxis2)
        {
            // If the X (Axis1) value and Y (Axis2) values from the IntersectionOfPlaneBottomLeftLower are with the bounded plane Z (Axis3)
            //   for TargetVectorBottomLeftLower then we have a hit
            if (vectorToOrigTargetAxis1 >= BoundingBoxAxis1 && vectorToOrigTargetAxis1 <= BoundingBoxAxis1 + BoundingBoxSizeAxis1)
            {
                if (vectorToOrigTargetAxis2 >= BoundingBoxAxis2 && vectorToOrigTargetAxis2 <= BoundingBoxAxis2 + BoundingBoxSizeAxis2)
                {
                    return true;
                }
            }
            return false;
        } //pointWithinBoundedPlane

    } //clsMathAssist


}





