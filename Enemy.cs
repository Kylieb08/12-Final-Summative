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
        private Vector2 _enemyLocation, _enemyDirection;
        private Rectangle _collisionRect, _drawRect;
        private Texture2D _texture;
        private int _rows, _columns, _frames, _frame, _directionRow, 
            _runLeftRow, _runRightRow, _width, _height;
        private float _spriteSpeed, _time, _frameSpeed;

        public Enemy(Texture2D texture, Rectangle collisionRect)
        {
            //Processing the sprite sheet
            _collisionRect = collisionRect;
            _texture = texture;
            _rows = 8;
            _columns = 6;
            _runRightRow = 4;
            _runLeftRow = 5;
            _directionRow = _runLeftRow;

            //Time
            _time = 0.0f;
            _frameSpeed = 0.08f;
            _frames = 6;
            _frame = 0;

            //Enemy
            _enemyLocation = new Vector2(710, 160);
            _drawRect = new Rectangle(704, 160, 103, 86);
            _spriteSpeed = 1.5f;
        }

        public void Update(Rectangle window, GameTime gameTime)
        {
            //_collisionRect.X -= (int)_speed.X;
            //if (_collisionRect.Left < 550 || _collisionRect.Right > 780)
            //    _speed.X *= -1;
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_time > _frameSpeed && _enemyDirection != Vector2.Zero)
            {
                _time = 0f;
                _frame = (_frame + 1) % _frames;
            }

            SetEnemyDirection();
            _enemyLocation += _enemyDirection * _spriteSpeed;
        }

        private void SetEnemyDirection()
        {
            _enemyDirection = Vector2.Zero;
            _enemyDirection.X += 1;
            if (_enemyDirection.X < 550 || _enemyDirection.X > 780)
                _enemyDirection *= -1;


           if (_enemyDirection != Vector2.Zero)
           {
                _enemyDirection.Normalize();
                if (_enemyDirection.X < 0)
                    _directionRow = _runLeftRow;

                else if (_enemyDirection.X > 0)
                    _directionRow = _runRightRow;
           }

            else
            {
                _frame = 0;
                _directionRow = _runLeftRow;
            }
        }

        public bool Intersects(Rectangle enemy)
        {
            return _collisionRect.Intersects(enemy);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _collisionRect, null, Color.Red, 0f, 
                Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
