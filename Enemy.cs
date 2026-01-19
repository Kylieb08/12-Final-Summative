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
    internal class Enemy
    {
        private Vector2 _speed;
        private Rectangle _location;
        private Texture2D _texture;

        public Enemy(Texture2D texture, Rectangle location)
        {
            _speed = Vector2.Zero;
            _location = location;
            _texture = texture;
        }

        public void Update(Rectangle window)
        {
            _speed.X -= 1;

            _location.Offset(_speed);            
        }

        public bool Intersects(Rectangle enemy)
        {
            return _location.Intersects(enemy);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _location, null, Color.Red, 0f, 
                Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
