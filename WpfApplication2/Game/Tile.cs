using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Media3D;
using System.Windows.Media;

//namespace appGameBoardTest.Game
namespace appGameBoardTest
{
    public class Tile
    {
        public Point3D p3D;
        public Brush B;
        public GeometryModel3D Geo;

        public Tile(GeometryModel3D GeoIn)
        {
            Geo = GeoIn;
        }

    }
}
