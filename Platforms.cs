using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

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

        public int RectY
        {
            get { return _location.Y; }
        }

        public bool Intersects(Rectangle platform)
        {
           return _location.Intersects(platform);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, _colour);
        }
    }
}
