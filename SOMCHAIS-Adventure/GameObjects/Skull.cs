using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOMCHAIS_Adventure
{
    internal class Skull : Enemy
    {
        public enum Direction
        {
            Left,
            Right
        }
        public Direction MovingDirection;
        public float Speed;
        public float MovedDistance;
        public Skull(Texture2D texture) : base(texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            Life = 3;
            MovedDistance = 0;
            Speed = 50;
            MovingDirection = Direction.Left;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            if (Life == 0)
            {
                IsActive = false;
            }

            if (MovingDirection == Direction.Left)
            {
                Velocity.X = -Speed;
            }
            else
            {
                Velocity.X = Speed;
            }

            float movingThisLoop = Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            MovedDistance += Math.Abs(movingThisLoop);
            float newX = Position.X + movingThisLoop;

            float newY = Position.Y;

            if (Position.X >= Singleton.SCREENWIDTH - this.Rectangle.Width)
            {
                MovingDirection = Direction.Left;
                MovedDistance = 0;
            }
            if (Position.X <= 0)
            {
                MovingDirection = Direction.Right;
                MovedDistance = 0;
            }

            Position = new Vector2(newX, newY);

            base.Update(gameTime, gameObjects);
        }
    }
}
