using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SOMCHAIS_Adventure
{
    public class MainSence : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<GameObject> _gameObjects;
        private int _numObject;

        private Matrix _translation;

        private Camera _camera;

        private Vector2 _position;

        private Texture2D _pixel;

        SpriteFont _font;

        private bool isPaused = false;

        public MainSence()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "SOMCHAI'S ADVENTURE";
            _graphics.PreferredBackBufferWidth = Singleton.SCREENWIDTH;
            _graphics.PreferredBackBufferHeight = Singleton.SCREENHEIGHT;
            _graphics.ApplyChanges();

            _gameObjects = new List<GameObject>();

            _position = new Vector2(100, 500);

            _camera = new();

            //Singleton.Instance.player = new Player();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("GameFont");

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White }); // Set the 
            Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Singleton.Instance.CurrentKey = Keyboard.GetState();
            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                if (_gameObjects[i].IsActive) _gameObjects[i].Update(gameTime, _gameObjects);
            }
            for (int i = 0; i < _numObject; i++)
            {
                if (!_gameObjects[i].IsActive)
                {
                    _gameObjects.RemoveAt(i);
                    i--;
                    _numObject--;
                }
            }

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.R) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                ResetEnemies();

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Z) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                _camera.Zoom -= 0.1f;

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.X) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                _camera.Zoom += 0.1f;

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.P) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                TogglePause();

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            //CalculateTranslation();

            //if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.R) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)

           

            _camera.Update(Singleton.Instance.player.Position, 400, 400); 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Texture2D tile = this.Content.Load<Texture2D>("tile");


            //_spriteBatch.Begin();
            //_spriteBatch.Begin(transformMatrix: _translation);

            //_spriteBatch.Draw(tile, new Vector2(0, 0), Color.White);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.Transform);

            Rectangle platformRectangle = new Rectangle(0, 100, 100, 20);
            _spriteBatch.Draw(_pixel, platformRectangle, Color.Yellow);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 200, 100, 20), Color.Yellow);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 300, 100, 20), Color.Yellow);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 400, 100, 20), Color.Yellow);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 500, 100, 20), Color.Yellow);
            _spriteBatch.Draw(_pixel, new Rectangle(0, 600, 100, 20), Color.Yellow);

            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            // HUD

            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, new Rectangle(400, 0, 20, 20), Color.Red);
            _spriteBatch.Draw(_pixel, new Rectangle(100, 0, 20, 20), Color.Green);
            _spriteBatch.Draw(_pixel, new Rectangle(700, 0, 20, 20), Color.Pink);

            _spriteBatch.Draw(_pixel, new Rectangle(100, 580, 20, 20), Color.Orange);

            _spriteBatch.End();

            //pause screen

            // If the game is paused, draw the pause screen
            if (isPaused)
            {
                _spriteBatch.Begin(blendState: BlendState.AlphaBlend);

                // Draw semi-transparent overlay
                var pauseOverlay = new Texture2D(GraphicsDevice, 1, 1);
                pauseOverlay.SetData(new[] { new Color(0, 0, 0, 0.5f) }); // Semi-transparent black
                _spriteBatch.Draw(pauseOverlay, new Rectangle(0, 0, 800, 600), Color.White);

                // Draw pause menu options
                // Here you would draw text like "Resume", "Options", "Quit" etc.
                // Example:
                string pauseText = "Game Paused\nPress [Key] to resume";
                Vector2 pauseTextSize = _font.MeasureString(pauseText);
                Vector2 pauseTextPosition = new Vector2(400 - pauseTextSize.X / 2, 300 - pauseTextSize.Y / 2);
                _spriteBatch.DrawString(_font, pauseText, pauseTextPosition, Color.White);

                _spriteBatch.End();
            }

            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {

            //Singleton.Instance.player.Position.X = 0;

            //Reset Value to Initialize Value
            Singleton.Instance.CurrentGameState = Singleton.GameState.StartNewLife;

            //Load GameSprite
            Texture2D gameSprite = this.Content.Load<Texture2D>("sprite");

            _gameObjects.Clear();

            Platform platform = new Platform(gameSprite)
            {
                Name = "Platform",
                Viewport = new Rectangle(0, 416, 96, 32)
            };

            for (int i = 0; i < 9; i++)
            {
                var clonePlat = platform.Clone() as Platform;
                clonePlat.Position = new Vector2(0 + i*96, Singleton.SCREENHEIGHT - 100 + clonePlat.Rectangle.Height);
                _gameObjects.Add(clonePlat);

                //if(i==7)
                //    clonePlat.Position = new Vector2(0 + i * 96, Singleton.SCREENHEIGHT - 200 + clonePlat.Rectangle.Height);
                //_gameObjects.Add(clonePlat);

            }




            _gameObjects.Add(new Player(gameSprite)
            {
                Name = "Player",
                Viewport = new Rectangle(0, 348, 32, 36),
                Position = new Vector2(400, Singleton.SCREENHEIGHT - 100),
                Left = Keys.Left,
                Right = Keys.Right,
                Fire = Keys.Space,
                Up = Keys.Up,
                bullet = new Bullet(gameSprite)
                {
                    Name = "PlayerBullet",
                    Viewport = new Rectangle(224, 352, 32, 32),
                    Velocity = new Vector2(200f, 0)
                }
            });

            //Singleton.Instance.Player.Position = 
            Singleton.Instance.heroX = 400;
            Singleton.Instance.heroY = Singleton.SCREENHEIGHT - 100;

            Singleton.Instance.player.Position = new Vector2(400, Singleton.SCREENHEIGHT - 100);




            //Singleton.Instance.hero.Position = new Vector2(100, 600 - 100);

            ResetEnemies();

            foreach (GameObject s in _gameObjects)
            {
                s.Reset();
            }
        }

        protected void ResetEnemies()
        {
            //Load GameSprite
            Texture2D gameSprite = this.Content.Load<Texture2D>("Sprite");

            Skull skull = new Skull(gameSprite)
            {
                Name = "Enemy",
                Viewport = new Rectangle(0, 0, 32, 32),
                Position = new Vector2(Singleton.SCREENWIDTH - 100, Singleton.SCREENHEIGHT - 100),
            };

            skull.Reset();

            _gameObjects.Add(skull);
        }

        private void CalculateTranslation()
        {
            var dx = (800 / 2) - Singleton.Instance.heroX;
            //dx = MathHelper.Clamp(dx, -_map.MapSize.X + 800 + (_map.TileSize.X / 2), _map.TileSize.X / 2);
            var dy = (600 / 2) - Singleton.Instance.heroY;
            //dy = MathHelper.Clamp(dy, -_map.MapSize.Y + 600 + (_map.TileSize.Y / 2), _map.TileSize.Y / 2);
            _translation = Matrix.CreateTranslation(dx, dy, 0f);
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
        }
    }
}
