using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace OBB_CD_Comparison
{
    public class WorldEntity : Entity
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
        public override float Radius { get { return collisionDetector.Radius; } }
        public bool IsFiller { get; set; }
        #endregion
        public WorldEntity(Texture2D texture, Vector2 position, float rotation = 0, float mass = 1, float thrust = 1, float friction = 0.1f, bool isVisible = true, bool isCollidable = true) : base(position, rotation, mass, thrust, friction)
        {
            this.sprite = new Sprite(texture);
            collisionDetector = new CollidableRectangle(position, rotation, sprite.Width, sprite.Height);
            Position = position;
            IsVisible = isVisible;
            IsCollidable = isCollidable;
            Origin = new Vector2(Width / 2, Height / 2);
        }
        #region Methods
        public override void Draw(SpriteBatch sb)
        {
            sprite.Draw(sb);
        }

        public override void Update(GameTime gameTime) //OBS Ska vara en funktion i thruster
        {
            base.Update(gameTime);
        }

        public override bool Contains(Vector2 point)
        {
            return IsCollidable && collisionDetector.Contains(point);
        }

        public void Collide(WorldEntity e)
        {
            //collision direct
            if (CollidesWith(e))
            {
                TotalExteriorForce += Physics.CalculateCollissionRepulsion(Position, e.Position, Velocity * Mass, e.Velocity * e.Mass);
                TotalExteriorForce += Physics.CalculateOverlapRepulsion(Position, e.Position, Radius) * (e.Mass + Mass) / 2;
            }
            else
            {
                Vector2 distanceBeforeMoving = Position - Velocity - (e.Position - e.Velocity);
                Vector2 distance = Position - e.Position;
            }
        }

        public virtual void HandleCollision(WorldEntity eOther, bool passesThroughFromBack = false, bool passesThroughFromFront = false)
        {
            TotalExteriorForce += Physics.CalculateCollissionRepulsion(Position, eOther.Position, Velocity * Mass, eOther.Velocity * eOther.Mass);
            TotalExteriorForce += Physics.CalculateOverlapRepulsion(Position, eOther.Position, Radius) * (eOther.Mass + Mass) / 2;     
        }

        public bool CollidesWith(WorldEntity e)
        {
            return IsCollidable && e.IsCollidable && collisionDetector.CollidesWith(e.collisionDetector);
        }
        #endregion
    }
}
