using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//sprite sheet is 850 x 1148 pixels

namespace _12_Final_Summative
{
    public enum Screen
    {
        Title,
        Info,
        Game,
        Win,
        Lose
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Screen screen;
        Texture2D playerSpriteSheet, rectangleTexture;
        KeyboardState keyboardState;
        MouseState mouseState;
        Rectangle window, playerCollisionRect, playerDrawRect;
        int rows, columns, frame, frames, directionRow, runLeftRow, runRightRow, 
            jumpRightRow,jumpLeftRow, attackLeftRow, attackRightRow, idleRightRow, idleLeftRow, width, height;
        float speed, time, frameSpeed, gravity, jumpSpeed;
        Vector2 playerLocation, playerDirection, fallSpeed;
        List<Rectangle> platforms;
        bool onGround = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            window = new Rectangle(0, 0, 800, 500);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            screen = Screen.Game;

            platforms = new List<Rectangle>();
            platforms.Add(new Rectangle(0, 400, window.Width, 15));

            //Processing spritesheet
            rows = 8;
            columns = 6;
            attackLeftRow = 0;
            attackRightRow = 1;
            idleRightRow = 2;
            idleLeftRow = 3;
            runRightRow = 4;
            runLeftRow = 5;
            jumpRightRow = 6;
            jumpLeftRow = 7;
            directionRow = idleRightRow;

            //Time
            time = 0.0f;
            frameSpeed = 0.08f;
            frames = 6;
            frame = 0;

            //Player
            playerLocation = new Vector2(20, 200);
            playerCollisionRect = new Rectangle(26, 218, 70, 60);
            playerDrawRect = new Rectangle(20, 200, 103, 86);
            speed = 1.5f;
            gravity = 0.3f;
            jumpSpeed = 8f;
            fallSpeed = Vector2.Zero;

            UpdateRects();

            base.Initialize();

            //Width and height
            width = playerSpriteSheet.Width / columns;
            height = playerSpriteSheet.Height / rows;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            rectangleTexture = Content.Load<Texture2D>("Images/rectangle");
            playerSpriteSheet = Content.Load<Texture2D>("Images/sprite_sheet");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            this.Window.Title = "x = " + mouseState.X + ", y = " + mouseState.Y;

            if (screen == Screen.Game)
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (time > frameSpeed && playerDirection != Vector2.Zero)
                {
                    time = 0f;
                    frame = (frame + 1) % frames;
                }

                SetPlayerDirection();
                playerLocation += playerDirection * speed;
                UpdateRects();

                //Collision detection
                if (!window.Contains(playerCollisionRect))
                {
                    playerLocation -= playerDirection * speed;
                    UpdateRects();
                }

                if (!onGround)
                {
                    fallSpeed.Y += gravity;
                    if (fallSpeed.Y < 0f && keyboardState.IsKeyUp(Keys.Space))
                        fallSpeed.Y /= 1.5f;
                }

                else if (keyboardState.IsKeyDown(Keys.Space)&& onGround)
                {
                    fallSpeed.Y -= jumpSpeed;
                    onGround = false;
                }

                playerLocation.Y += fallSpeed.Y;

                foreach (Rectangle platform in platforms)
                {
                    if (playerCollisionRect.Intersects(platform))
                    {
                        if (fallSpeed.Y > 0f)
                        {
                            onGround = true;
                            fallSpeed.Y = 0f;
                            playerLocation.Y = platform.Y - playerCollisionRect.Height;
                        }

                        else
                        {
                            fallSpeed.Y = 0;
                            playerLocation.Y = platform.Bottom;
                        }
                    }
                }

                UpdateRects();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (screen == Screen.Game)
            {
                _spriteBatch.Draw(playerSpriteSheet, playerDrawRect, new Rectangle(frame * width, directionRow * height, width, height), Color.White);
                _spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f); //Draws hitbox

                foreach (Rectangle platform in platforms)
                    _spriteBatch.Draw(rectangleTexture, platform, Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateRects()
        {
            playerCollisionRect.Location = playerLocation.ToPoint();
            playerDrawRect.Location = new Point(playerCollisionRect.X - 5, playerCollisionRect.Y - 17);
        }

        private void SetPlayerDirection()
        {
            playerDirection = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                playerDirection.X -= 1;

            if (keyboardState.IsKeyDown(Keys.D) ||  keyboardState.IsKeyDown(Keys.Right))
                playerDirection.X += 1;

            if (playerDirection != Vector2.Zero)
            {
                playerDirection.Normalize();
                if (playerDirection.X < 0)
                    directionRow = runLeftRow;

                else if (playerDirection.X > 0)
                    directionRow = runRightRow;
            }

            else
            {
                frame = 0;
                directionRow = idleRightRow;
            }
                
        }
    }
}
