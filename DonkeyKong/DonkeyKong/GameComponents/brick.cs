using DonkeyKong.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.GameComponents
{
    class Brick : GenericSprite
    {
        public Brick(Game game) : base (game)
        {
                
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
