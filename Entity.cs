using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OBB_CD_Comparison
{
    public interface Entity
    {
        public Vector2 Position { get; set; }
        public float Radius { get; }
        public float Mass { get; }
        public void Update(GameTime gameTime);
        public void Accelerate(Vector2 direction, float force);
        public void Draw(SpriteBatch spriteBatch);
        public void Collide(Entity e);
        public void GenerateAxes();
    }
}
