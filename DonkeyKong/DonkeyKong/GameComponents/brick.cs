using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    class Brick : GenericSprite
    {
        /// <summary>
        /// A brick is a standart Generic sprite that can be cloned
        /// </summary>
        /// <param name="game"></param>
        /// <param name="graphicsDevice"></param>
        public Brick(Game game) : base (game)
        {
        }

        /// <summary>
        /// Returns a shallow copy of the brick
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }



    }
}
