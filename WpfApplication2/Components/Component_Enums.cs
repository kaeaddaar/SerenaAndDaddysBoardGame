using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appGameBoardTest.Components
{
    class Component_Enums
    {
    }


    public enum ItemType
    {
        // The flag = 0001.
        Armor = 0x01,
        // The flag = 0010.
        Weapon = 0x02,
        // The flag = 0100.
        Backpack = 0x04,
        // The flag = is 1000.
        SmallObject = 0x08,
        // The flag = is 10000.
        MediumObject = 0x16,
        // The flag = is 10000.
        LargeObject = 0x32,
    }

    
    public enum Character_Sex
    {
        Make = 0x01,
        Female = 0x02,
    }


    public enum Character_Class
    {
        None = 0x01,
        Player = 0x02,
        Fuzzle = 0x04,
    }

}
