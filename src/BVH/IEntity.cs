using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OBB_CD_Comparison.src.BVH;
using System;
using System.Collections.Generic;
using System.Text;

namespace OBB_CD_Comparison.src.BVH
{
    public interface IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 MassCenter { get; }
        public float Radius { get; }
        public float Mass { get; }
        public CollidableCircle BoundingCircle { get; }
        public BoundingCircleNode Parent { get; set; }
        public void Draw(SpriteBatch spriteBatch);
        public void Update(GameTime gameTime);
        void AccelerateTo(Vector2 position, float force);
        /*
        public void Accelerate(Vector2 direction, float force);
        public void GenerateAxes();
        */
    }
}
