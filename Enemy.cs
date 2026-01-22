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
        //private int _rows, _columns, _frames, _frame, _directionRow, _runLeftRow, _runRightRow, _width, _height;
        //private float _spriteSpeed, _time, _frameSpeed;

        public Enemy(Texture2D texture, Rectangle location)
        {
            _speed = Vector2.One;
            _location = location;
            _texture = texture;
        }

        public void Update(Rectangle window)
        {
            _location.X -= (int)_speed.X;
            if (_location.Left < 550 || _location.Right > 780)
                _speed.X *= -1;
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
