using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace OBB_CD_Comparison
{
    public abstract class Entity : Movable
    {
        public bool IsCollidable { get; set; }
        public virtual float Radius { get; }
        //public IController Manager { get; set; }
        public static float REPULSIONDISTANCE = 100;
        public Entity(Vector2 position, float rotation = 0, float mass = 1, float thrust = 1, float friction = 0.1f, float elasticity = 1) : base(position, rotation, mass, thrust, friction) { }
        public abstract bool Contains(Vector2 point);
        public abstract void Draw(SpriteBatch sb);
        public virtual void ApplyRepulsion(Entity otherEntity)
        {
            TotalExteriorForce += Mass * CalculateGravitationalRepulsion(this, otherEntity);
        }
        public static Vector2 CalculateGravitationalRepulsion(Entity entityAffected, Entity entityAffecting)
        {
            if (entityAffected.Radius + entityAffecting.Radius + REPULSIONDISTANCE > Vector2.Distance(entityAffected.Position, entityAffecting.Position))
            {
                Vector2 vectorToE = entityAffecting.Position - entityAffected.Position;
                float distance = vectorToE.Length();
                float res = 0;
                if (distance != 0)
                    res = -Physics.CalculateGravityRepulsion(entityAffected.Radius, entityAffecting.Radius, distance);
                return Vector2.Normalize(vectorToE) * res;
            }
            return Vector2.Zero;
        }
    }
}
