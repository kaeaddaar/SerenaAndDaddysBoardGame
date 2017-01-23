using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

namespace appGameBoardTest
{
    class clsModels
    {
        // Gets the geometry of a square made up of two triangles
        public static GeometryModel3D GetGeoBox(Point3D loc, Brush B)
        {

            double Xp = loc.X;
            double Yp = loc.Y;
            double Zp = loc.Z;

            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(0, 0, 0));
            myPositionCollection.Add(new Point3D(1, 0, 0));
            myPositionCollection.Add(new Point3D(1, 1.0, 0));
            myPositionCollection.Add(new Point3D(1, 1.0, 0));
            myPositionCollection.Add(new Point3D(0, 1.0, 0));
            myPositionCollection.Add(new Point3D(0, 0, 0));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            DiffuseMaterial myMaterial = new DiffuseMaterial(B);
            myGeometryModel.Material = myMaterial;
            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            TranslateTransform3D tMove = new TranslateTransform3D();
            tMove.OffsetX = loc.X;
            tMove.OffsetY = loc.Y;
            tMove.OffsetZ = loc.Z;

            myGeometryModel.Transform = tMove;

            return myGeometryModel;
        } //GetGeoBox


        // Gets the geometry of a square made up of two triangles
        public static GeometryModel3D GetGeo(Point3D loc, Brush B)
        {

            double Xp = loc.X;
            double Yp = loc.Y;
            double Zp = loc.Z;

            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(0, 0, 0));
            myPositionCollection.Add(new Point3D(1, 0, 0));
            myPositionCollection.Add(new Point3D(1, 1.0, 0));
            myPositionCollection.Add(new Point3D(1, 1.0, 0));
            myPositionCollection.Add(new Point3D(0, 1.0, 0));
            myPositionCollection.Add(new Point3D(0, 0, 0));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            DiffuseMaterial myMaterial = new DiffuseMaterial(B);
            myGeometryModel.Material = myMaterial;
            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            TranslateTransform3D tMove = new TranslateTransform3D();
            tMove.OffsetX = loc.X;
            tMove.OffsetY = loc.Y;
            tMove.OffsetZ = loc.Z;

            myGeometryModel.Transform = tMove;

            return myGeometryModel;
        } //GetGeo


        // Gets the geometry of a square made up of two triangles
        public static GeometryModel3D GetGeo_98(Point3D loc, Brush B)
        {

            double Xp = loc.X;
            double Yp = loc.Y;
            double Zp = loc.Z;

            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            Vector3DCollection myNormalCollection = new Vector3DCollection();
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myNormalCollection.Add(new Vector3D(0, 0, 1));
            myMeshGeometry3D.Normals = myNormalCollection;

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();

            myPositionCollection.Add(new Point3D(.01, .01, 0));
            myPositionCollection.Add(new Point3D(.99, .01, 0));
            myPositionCollection.Add(new Point3D(.99, .99, 0));
            myPositionCollection.Add(new Point3D(.99, .99, 0));
            myPositionCollection.Add(new Point3D(.01, .99, 0));
            myPositionCollection.Add(new Point3D(.01, .01, 0));
            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 1));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(0, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 0));
            myTextureCoordinatesCollection.Add(new System.Windows.Point(1, 1));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            DiffuseMaterial myMaterial = new DiffuseMaterial(B);
            myGeometryModel.Material = myMaterial;
            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            TranslateTransform3D tMove = new TranslateTransform3D();
            tMove.OffsetX = loc.X;
            tMove.OffsetY = loc.Y;
            tMove.OffsetZ = loc.Z;

            myGeometryModel.Transform = tMove;

            return myGeometryModel;
        } //GetGeo_99


 
    }
}
