﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace OBB_CD_Comparison
{
    public class WorldEntity : Movable
    {
        #region Properties
        protected Sprite sprite = null;
        public bool IsVisible { get { return sprite.isVisible; } set { sprite.isVisible = value; } }
        public CollidableRectangle collisionDetector;
        public override Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                sprite.Position = value;
                collisionDetector.Position = value;
            }
        }
        public override float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                sprite.Rotation = value;
                collisionDetector.Rotation = value;
            }
        }
        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                sprite.Origin = value;
            }
        }
        protected Vector2 origin;
        public float Width { get { return sprite.Width; } }
        public float Height { get { return sprite.Height; } }
        public float Radius { get { return collisionDetector.Radius; } }
        public bool IsCollidable { get; set; }
        public static float REPULSIONDISTANCE = 100;
        #endregion
        public WorldEntity(Texture2D texture, Vector2 position, float rotation = 0, float mass = 1, float thrust = 1, float friction = 0.1f, bool isVisible = true, bool isCollidable = true) : base(position, rotation, mass, thrust, friction)
        {
            this.sprite = new Sprite(texture);
            collisionDetector = new CollidableRectangle(position, rotation, sprite.Width, sprite.Height);
            Position = position;
            Rotation = rotation;
            IsVisible = isVisible;
            IsCollidable = isCollidable;
            Origin = new Vector2(Width / 2, Height / 2);
        }
        #region Methods
        public void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb);
        }

        public override void Update(GameTime gameTime) //OBS Ska vara en funktion i thruster
        {
            base.Update(gameTime);
        }

        public bool Contains(Vector2 point)
        {
            return IsCollidable && collisionDetector.Contains(point);
        }

        public void Collide(WorldEntity e)
        {
            //collision direct
            if (CollidesWith(e))
            {
                //collission repulsion
                Vector2 vectorFromOther = e.Position - position;
                float distance = vectorFromOther.Length();
                vectorFromOther.Normalize();
                Vector2 collissionRepulsion = 0.5f * Vector2.Normalize(-vectorFromOther) * (Vector2.Dot(velocity, vectorFromOther) + Vector2.Dot(e.Velocity, -vectorFromOther)); //make velocity depend on position
                TotalExteriorForce += collissionRepulsion;

                //overlap repulsion
                float distance2 = (position - e.Position).Length();
                float radius = Radius * (e.Mass + Mass)/2;
                if (distance2 < radius / 2)
                    distance2 = radius / 2;
                Vector2 overlapRepulsion = 1f * Vector2.Normalize(position - e.Position) / (float)Math.Pow(distance2 / radius, 1 / 1);
                TotalExteriorForce += overlapRepulsion;
            }
        }

        public void GenerateAxes()
        {
            collisionDetector.GenerateAxes();
        }

        public bool CollidesWith(WorldEntity e)
        {
            return IsCollidable && e.IsCollidable && collisionDetector.CollidesWith(e.collisionDetector);
        }
        public virtual void ApplyRepulsion(WorldEntity otherEntity)
        {
            TotalExteriorForce += Mass * CalculateGravitationalRepulsion(this, otherEntity);
        }
        public static Vector2 CalculateGravitationalRepulsion(WorldEntity entityAffected, WorldEntity entityAffecting)
        {
            if (entityAffected.Radius + entityAffecting.Radius + REPULSIONDISTANCE > Vector2.Distance(entityAffected.Position, entityAffecting.Position))
            {
                Vector2 vectorToE = entityAffecting.Position - entityAffected.Position;
                float distance = vectorToE.Length();
                float res = 0;
                return Vector2.Normalize(vectorToE) * res;
            }
            return Vector2.Zero;
        }
        #endregion
    }
}
