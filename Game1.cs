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
        Texture2D playerSpriteSheet, rectangleTexture, loseTexture, winTexture, 
            bgTexture, exitTexture, coinTexture, enemySpriteSheet;
        KeyboardState keyboardState;
        MouseState mouseState;
        Rectangle window, playerCollisionRect, playerDrawRect, platformRect, 
            enemyRect, exitRect, infoRect, legibilityRect;
        List<Rectangle> coins;

        int rows, columns, frame, frames, directionRow, runLeftRow, runRightRow, 
            attackLeftRow, attackRightRow, idleRightRow, idleLeftRow, width, height, coinsCollected;

        float speed, time, frameSpeed, gravity, jumpSpeed;
        Vector2 playerLocation, playerDirection, fallSpeed;
        Color platformColor;
        List<Platforms> platforms;
        bool onGround = false, enemyDead = false;

        Platforms platform;
        Enemy enemy;
        SpriteFont coinFont, titleFont;

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

            screen = Screen.Title;

            infoRect = new Rectangle(5, 445, 200, 50);
            legibilityRect = new Rectangle(10, 10, 450, 258);
            exitRect = new Rectangle(710, 160, 60, 60);
            platformRect = new Rectangle(0, 400, 800, 15);
            platformColor = Color.Black;
            platforms = new List<Platforms>();

            //Processing spritesheet
            rows = 8;
            columns = 6;
            attackLeftRow = 0;
            attackRightRow = 1;
            idleRightRow = 2;
            //idleLeftRow = 3;
            runRightRow = 4;
            runLeftRow = 5;
            //jumpRightRow = 6;
            //jumpLeftRow = 7;
            directionRow = idleRightRow;

            //Time
            time = 0.0f;
            frameSpeed = 0.08f;
            frames = 6;
            frame = 0;

            //Player
            playerLocation = new Vector2(20, 340);
            playerCollisionRect = new Rectangle(26, 340, 60, 60);
            playerDrawRect = new Rectangle(20, 340, 103, 86);
            speed = 1.5f;
            gravity = 0.3f;
            jumpSpeed = 8f;
            fallSpeed = Vector2.Zero;
            coinsCollected = 0;

            //Enemy
            enemyRect = new Rectangle(720, 160, 60, 60);

            //Coins
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(20, 435, 25, 25));
            coins.Add(new Rectangle(265, 350, 25, 25));
            coins.Add(new Rectangle(240, 120, 25, 25));

            UpdateRects();

            base.Initialize();

            //Width and height
            width = playerSpriteSheet.Width / columns;
            height = playerSpriteSheet.Height / rows;

            GeneratePlatforms();

            platform = new Platforms(rectangleTexture, platformRect, platformColor);
            enemy = new Enemy(enemySpriteSheet, enemyRect);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            rectangleTexture = Content.Load<Texture2D>("Images/rectangle");
            playerSpriteSheet = Content.Load<Texture2D>("Images/sprite_sheet");
            coinTexture = Content.Load<Texture2D>("Images/coin");
            enemySpriteSheet = Content.Load<Texture2D>("Images/sprite_sheet");

            loseTexture = Content.Load<Texture2D>("Images/burning_forest");
            winTexture = Content.Load<Texture2D>("Images/win_forest");
            bgTexture = Content.Load<Texture2D>("Images/forest");
            exitTexture = Content.Load<Texture2D>("Images/door");

            coinFont = Content.Load<SpriteFont>("Fonts/coinFont");
            titleFont = Content.Load<SpriteFont>("Fonts/titleFont");
        }

        public void GeneratePlatforms()
        {
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(0, 400, window.Width, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(164, 345, 50, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(29, 290, 50, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(111, 203, 50, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(230, 170, 50, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(395, 273, 50, 15)), platformColor));
            platforms.Add(new Platforms(rectangleTexture, (new Rectangle(530, 220, 250, 15)), platformColor));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            this.Window.Title = "x = " + mouseState.X + ", y = " + mouseState.Y;

            if (screen == Screen.Title)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                    screen = Screen.Game;

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (infoRect.Contains(mouseState.Position))
                        screen = Screen.Info;
                }
            }

            else if (screen == Screen.Info)
            {
                if (keyboardState.IsKeyDown((Keys)Keys.R))
                    screen = Screen.Title;
            }

            else if (screen == Screen.Game)
            {
                enemy.Update(window, gameTime);

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

                //Window
                if (!window.Contains(playerCollisionRect))
                {
                    playerLocation -= playerDirection * speed;
                    UpdateRects();
                }

                //Side of platforms
                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i].Intersects(playerCollisionRect))
                    {
                        playerLocation -= playerDirection * speed;
                        UpdateRects();
                    }
                }

                //Gravity
                if (!onGround)
                {
                    fallSpeed.Y += gravity;
                    if (fallSpeed.Y < 0f && keyboardState.IsKeyUp(Keys.Space))
                        fallSpeed.Y /= 1.5f;
                }

                else if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    fallSpeed.Y -= jumpSpeed;
                    onGround = false;
                }

                else
                   fallSpeed.Y += gravity;

                playerLocation.Y += fallSpeed.Y;
                UpdateRects();

                //Top and bottom of platforms
                foreach (Platforms platform in platforms)
                {
                    if (platform.Intersects(playerCollisionRect))
                    {
                        if (fallSpeed.Y >= 0f)
                        {
                            onGround = true;
                            fallSpeed.Y = 0f;
                            playerLocation.Y = platform.RectY - playerCollisionRect.Height;
                        }

                        else
                        {
                            playerLocation.Y -= fallSpeed.Y;
                            fallSpeed.Y = 0;
                        }

                        UpdateRects();
                    }

                    //Enemy
                    if (enemy.Intersects(playerCollisionRect) && keyboardState.IsKeyDown(Keys.LeftControl))
                    {
                        enemyDead = true;
                    }

                    else if (enemy.Intersects(playerCollisionRect) && !keyboardState.IsKeyDown(Keys.LeftControl) && !enemyDead)
                    {
                        screen = Screen.Lose;
                    }

                    //Exit
                    if (playerCollisionRect.Contains(exitRect))
                    {
                        screen = Screen.Win;
                    }

                    //Coins
                    for (int i = 0; i < coins.Count; i++)
                    {
                        if (playerCollisionRect.Intersects(coins[i]))
                        {
                            coins.RemoveAt(i);
                            i--;
                            coinsCollected += 1;
                        }
                    }
                }
            }

            else if (screen == Screen.Win)
            {
                frame = 0;
                directionRow = idleRightRow;
                playerDrawRect = new Rectangle(330, 340, 70, 60);
            }

            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(bgTexture, window, Color.White);
                _spriteBatch.DrawString(titleFont, "SAVE THE FOREST", new Vector2(40, 20), Color.White);
                _spriteBatch.DrawString(coinFont, "Get To The End To Stop The Forest From Burning Down", new Vector2(70, 100), Color.White);
                _spriteBatch.DrawString(coinFont, "Press Enter to Play", new Vector2(280, 137), Color.White);
                _spriteBatch.Draw(rectangleTexture, infoRect, Color.SpringGreen * 0.6f);
                _spriteBatch.DrawString(coinFont, "HOW TO PLAY", new Vector2(20, 460), Color.White);
            }

            else if (screen == Screen.Info)
            {
                _spriteBatch.Draw(bgTexture, window, Color.White);
                _spriteBatch.Draw(rectangleTexture, legibilityRect, Color.Black * 0.5f);
                _spriteBatch.DrawString(coinFont, "HOW TO PLAY", new Vector2(20, 20), Color.White);
                _spriteBatch.DrawString(coinFont, "Use a and d or the left and", new Vector2(20, 57), Color.White);
                _spriteBatch.DrawString(coinFont, "right arrow keys to move", new Vector2(20, 94), Color.White);
                _spriteBatch.DrawString(coinFont, "Press space to jump", new Vector2(20, 131), Color.White);
                _spriteBatch.DrawString(coinFont, "Use the left control button to attack", new Vector2(20, 168), Color.White);
                _spriteBatch.DrawString(coinFont, "Running into the enemy will kill you", new Vector2(20, 205), Color.White);
                _spriteBatch.DrawString(coinFont, "Press R to return to title", new Vector2(20, 242), Color.White);
            }

            else if (screen == Screen.Game)
            {
                _spriteBatch.Draw(bgTexture, window, Color.White);
                _spriteBatch.Draw(exitTexture, exitRect, Color.White);

                _spriteBatch.Draw(playerSpriteSheet, playerDrawRect, new Rectangle(frame * width, directionRow * height, width, height), Color.White);
                //_spriteBatch.Draw(rectangleTexture, playerCollisionRect, Color.Black * 0.3f); //Draws hitbox

                foreach (Platforms platform in platforms)
                    platform.Draw(_spriteBatch);

                if (!enemyDead)
                    enemy.Draw(_spriteBatch);

                foreach (Rectangle coin in coins)
                    _spriteBatch.Draw(coinTexture, coin, Color.White);

                _spriteBatch.DrawString(coinFont, $"= {coinsCollected}", new Vector2(55, 435), Color.White);
            }

            else if (screen == Screen.Lose)
            {
                _spriteBatch.Draw(loseTexture, window, Color.White);
            }

            else if (screen == Screen.Win)
            {
                _spriteBatch.Draw(winTexture, window, Color.White);
                _spriteBatch.Draw(playerSpriteSheet, playerDrawRect, new Rectangle(frame * width, directionRow * height, width, height), Color.White);
            }

                _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateRects()
        {
            playerCollisionRect.Location = playerLocation.ToPoint();
            playerDrawRect.Location = new Point(playerCollisionRect.X - 5, playerCollisionRect.Y - 17);

            if (playerDirection.X > 0 && keyboardState.IsKeyDown(Keys.LeftControl))
                playerDrawRect.Location = new Point(playerCollisionRect.X - 5, playerCollisionRect.Y - 5);

            if (playerDirection.X < 0 && keyboardState.IsKeyDown(Keys.LeftControl))
                playerDrawRect.Location = new Point(playerCollisionRect.X - 5, playerCollisionRect.Y - 5);

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
                if (playerDirection.X > 0 && keyboardState.IsKeyDown(Keys.LeftControl))
                    directionRow = attackRightRow;

                else if (playerDirection.X < 0 && keyboardState.IsKeyDown(Keys.LeftControl))
                    directionRow = attackLeftRow;

                //else if (playerDirection.X < 0 && onGround && keyboardState.IsKeyDown(Keys.Space))
                //    directionRow = jumpLeftRow;

                //else if (playerDirection.X > 0 && onGround && keyboardState.IsKeyDown(Keys.Space))
                //    directionRow = jumpRightRow;

                else if (playerDirection.X < 0 && onGround)
                    directionRow = runLeftRow;

                else if (playerDirection.X > 0 && onGround)
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
