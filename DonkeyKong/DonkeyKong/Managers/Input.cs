/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DonkeyKong.Managers
{
    /// <summary>
    /// This class contains all the potentially necessary inputs for the game.
    /// Not all objects who use this class will have all Inputs set, some might not need all of them;
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials
    /// </summary>
    class Input
    {
        public Keys Down { get; set; }

        public Keys Left { get; set; }

        public Keys Right { get; set; }

        public Keys Up { get; set; }

        public List<Keys> Action { get; set; }
    }
}
