using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appGameBoardTest.Game.UserInerface
{
    public class GBInfo
    {
        Window2 infoWindow;
        
        public GBInfo(Window2 infoWindow)
        {
            this.infoWindow = infoWindow;
        }


        public string RedMan_Location = "";
        public string RedMan_Geo_Location = "";
        public string numTiles = "";
        public string GeoMove_Location = "";
        public string AppendToEnd = "";

        public string updateInfoWindow()
        {
            string strTemp = "";

            strTemp = strTemp + "Redman Location: " + RedMan_Location + System.Environment.NewLine;
            strTemp = strTemp + "Redman Geo Location: " + RedMan_Geo_Location + System.Environment.NewLine;
            strTemp = strTemp + "Number of Tiles: " + numTiles + System.Environment.NewLine;
            strTemp = strTemp + "GeoMove_Location: " + GeoMove_Location + System.Environment.NewLine;

            strTemp = strTemp + "" + AppendToEnd + System.Environment.NewLine;

            infoWindow.txt1.Text = strTemp;
            return strTemp;
            //return base.ToString();
        }
    }
    class clsUserInterface
    {
    }
}
