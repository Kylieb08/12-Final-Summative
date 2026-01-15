using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace _12_Final_Summative
{
    public class Platforms
    {
        private Rectangle _location;
        private Texture2D _texture;
        private Color _colour;

        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public Platforms(Texture2D texture, Rectangle location, Color colour)
        {
            _location = location;
            _texture = texture;
            _colour = colour;
        }

        public bool Intersects(Rectangle platform)
        {
           return _location.IntersectsWith(platform);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, _colour);
        }
    }
}
