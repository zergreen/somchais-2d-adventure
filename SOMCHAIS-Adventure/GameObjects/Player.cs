using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOMCHAIS_Adventure
{
    class Player : GameObject
    {
        public Keys Left, Right, Fire, Up;
        public Bullet bullet;
        private float jumpVelocity = -10000f;
        private float gravity = 10000f;
        private bool isJumping = false;

        public Player() : base()
        {

        }

        public Player(Texture2D texture) : base(texture)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Position = new Vector2(400, Singleton.SCREENHEIGHT - 100);
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Singleton.Instance.CurrentKey.IsKeyDown(Left))
            {
                Viewport = new Rectangle(0, 348, 32, 36);
                Velocity.X = -500;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Right))
            {
                Viewport = new Rectangle(0, 348, 32, 36);
                Velocity.X = 500;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Up) && !isJumping)
            {
                Viewport = new Rectangle(0, 348, 32, 36);
                Velocity.Y = jumpVelocity;
                isJumping = true;
            }
            if (Singleton.Instance.CurrentKey.IsKeyDown(Fire) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                Viewport = new Rectangle(64, 348, 36, 36);
                var newBullet = bullet.Clone() as Bullet;
                newBullet.Position = new Vector2(Rectangle.Width / 2 + Position.X - newBullet.Rectangle.Width / 2, Position.Y);
                newBullet.Reset();
                gameObjects.Add(newBullet);
            }

            float newX = Position.X + Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            newX = MathHelper.Clamp(newX, 0, (Singleton.SCREENWIDTH) - Rectangle.Width);

            Velocity.Y += gravity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            float newY = Position.Y + Velocity.Y * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            Position = new Vector2(newX, newY);

            //if (Position != Vector2.Zero)
            //{
            //    Singleton.Instance.player.Position = Position;

            //    //Singleton.Instance.heroX = Position.X;
            //    //Singleton.Instance.heroY = Position.Y;

            //}

            //Singleton.Instance.heroX = Position.X;
            //Singleton.Instance.heroY = Position.Y;

            Singleton.Instance.player.Position = Position;



            Velocity = Vector2.Zero;

            // Check for ground collision
            if (Position.Y >= (Singleton.SCREENHEIGHT - 100))
            {
                Position.Y = (Singleton.SCREENHEIGHT - 100);
                Velocity.Y = 0;
                isJumping = false;
            }


            base.Update(gameTime, gameObjects);
        }
    }
}
