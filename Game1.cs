using Microsoft.Win32.SafeHandles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//sprite sheet is 864 x 1152 pixels

namespace _12_Final_Summative
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerSpriteSheet, rectangleTexture;
        KeyboardState keyboardState;
        Rectangle window, playerCollisionRect, playerDrawRect;
        int rows, columns, frame, frames, directionRow, runLeftRow, runRightRow, 
            jumpRightRow,jumpLeftRow, attackLeftRow, attackRightRow, idleRightRow, idleLeftRow, width, height;
        float speed, time, framespeed;
        Vector2 playerLocation, playerDirection;

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

            //player
            playerLocation = new Vector2(20, 20);
            playerCollisionRect = new Rectangle(20, 20, 103, 86);
            playerDrawRect = new Rectangle(20, 20, 103, 86);
            speed = 1.5f;

            UpdateRects();

            base.Initialize();
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void UpdateRects()
        {
            playerCollisionRect.Location = playerLocation.ToPoint();
            playerDrawRect.Location = new Point(playerCollisionRect.X - 15, playerCollisionRect.Y - 15);
        }
    }
}
