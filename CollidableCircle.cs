using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OBB_CD_Comparison
{
    public class CollidableCircle
    {
        public Vector2 Position { get; set; }
        private float radius;
        public float Radius { get { return radius; } set { radius = value; } }
        public float Rotation { get; set; }

        public CollidableCircle(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool CollidesWith(CollidableCircle c)
        {
            return Math.Sqrt(Math.Pow((double)(Position.X) - (double)(c.Position.X), 2) + Math.Pow((double)(Position.Y) - (double)(c.Position.Y), 2)) <= (Radius + c.Radius);
        }

        public bool Contains(Vector2 position)
        {
            return (position - Position).Length() <= Radius;
        }
    }
}
