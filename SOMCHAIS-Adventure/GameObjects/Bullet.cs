using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOMCHAIS_Adventure
{
    class Bullet : GameObject
    {
        public float DistanceMoved;

        public Bullet(Texture2D texture) : base(texture)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Viewport, Color.White);
            base.Draw(spriteBatch);
        }

        public override void Reset()
        {
            DistanceMoved = 0;
            base.Reset();
        }

        public override void Update(GameTime gameTime, List<GameObject> gameObjects)
        {
            DistanceMoved += Math.Abs(Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond);
            Position = Position + Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

            foreach (GameObject s in gameObjects)
            {
                if (Name.Equals("PlayerBullet"))
                {
                    if (IsTouching(s) && (s.Name.Equals("Enemy")))
                    {
                        (s as Enemy).Life -= 1;
                        IsActive = false;
                    }
                }
            }


            if (DistanceMoved >= Singleton.SCREENWIDTH || DistanceMoved <= 0) IsActive = false;

            base.Update(gameTime, gameObjects);
        }
    }
}
