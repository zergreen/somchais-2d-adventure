﻿using Microsoft.Xna.Framework;
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

        SpriteFont _font;

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
                _camera.Zoom -= 0.1f;
            //ResetEnemies();

            if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.S) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
                _camera.Zoom += 0.1f;

            Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

            //CalculateTranslation();

            //if (Singleton.Instance.CurrentKey.IsKeyDown(Keys.R) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)


            _camera.Update(new Vector2(Singleton.Instance.heroX,Singleton.Instance.heroY), 400, 400); 

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

            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                _gameObjects[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
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
    }
}