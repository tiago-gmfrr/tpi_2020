/***
* Program : DonkeyKong
* Author : Tiago Gama
* Project : TPI 2020
* Date : 25.05.2020 - 09.06.2020
* Version : 1.0
* Description : Recreation of the original Donkey Kong game by Nintendo
***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Models
{
    /// <summary>
    /// This is a score model, it contains all the variables necessary to manage a score.
    /// Inspiration : https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial015
    /// </summary>
    public class Score
    {
        public string PlayerName { get; set; }
        public string Value { get; set; }
    }
}
