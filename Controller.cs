using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OBB_CD_Comparison
{
    public class Controller
    {
        protected List<WorldEntity> entities;
        public List<WorldEntity> Entities { get { return entities; } set { SetEntities(value); } }
        protected float collissionOffset = 100; //TODO make this depend on velocity + other things?
        public float Radius { get { return radius; } protected set { radius = value; } }
        protected float radius;
        public virtual Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 posChange = value - Position;
                foreach (WorldEntity c in Entities)
                    c.Position += posChange;
                position = value;
            }
        }

        public float Mass
        {
            get
            {
                float sum = 0;
                foreach (WorldEntity c in Entities)
                    sum += c.Mass;
                return sum;
            }
        }

        protected Vector2 position;

        public Controller(List<WorldEntity> controllables)
        {
            SetEntities(controllables);
        }

        public Controller([OptionalAttribute] Vector2 position)
        {
            if (position == null)
                position = Vector2.Zero;
            SetEntities(new List<WorldEntity>());
        }
        public virtual void SetEntities(List<WorldEntity> newControllables)
        {
            if (newControllables != null)
            {
                List<WorldEntity> oldControllables = Entities;
                entities = new List<WorldEntity>();
                foreach (WorldEntity c in newControllables)
                    AddEntity(c);
                if (Entities.Count == 0)
                {
                    Entities = oldControllables;
                }
            }
        }
        public virtual void AddEntity(WorldEntity c)
        {
            if (entities == null)
            {
                entities = new List<WorldEntity>();
                c.Position = Position;
            }

            if (c != null)
            {
                entities.Add(c);
                UpdatePosition();
                UpdateRadius();
            }
        }

        public void RotateTo(Vector2 position)
        {
            foreach (WorldEntity c in Entities)
                c.RotateTo(position);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateEntities(gameTime);
            UpdatePosition();
            UpdateRadius();
            ApplyInternalGravity();
            InternalCollission();
        }

        protected void InternalCollission()
        {
            foreach (WorldEntity c1 in Entities)
            {
                c1.GenerateAxes();
                foreach (WorldEntity c2 in Entities)
                {
                    c2.GenerateAxes();
                    if (c1 != c2)
                        c1.Collide(c2);
                }
            }
        }

        private void UpdateEntities(GameTime gameTime)
        {
            foreach (WorldEntity c in Entities)
                c.Update(gameTime);
        }

        //TODO: make this work 
        protected void UpdateRadius() //TODO: Update this to make it more efficient, e.g. by having sorted list, TODO: only allow IsCollidable to affect this?
        {
            if (Entities.Count == 1)
            {
                if (Entities[0] != null)
                    Radius = Entities[0].Radius;
            }
            else if (Entities.Count > 1)
            {
                float largestDistance = 0;
                foreach (WorldEntity c in Entities)
                {
                    float distance = Vector2.Distance(c.Position, Position) + c.Radius;
                    if (distance > largestDistance)
                        largestDistance = distance;
                }
                Radius = largestDistance;
            }
        }
        protected float AverageDistance()
        {
            float nr = 1;
            float distance = 0;
            float mass = 0;
            foreach (WorldEntity c in Entities)
            {
                distance += (Vector2.Distance(c.Position, Position) + c.Radius) * c.Mass;
                //nr += 1;
                mass += c.Mass;
            }
            if (mass != 0)
                return distance / nr / mass;
            return 1;
        }
        protected void ApplyInternalGravity()
        {
            Vector2 distanceFromController;
            foreach (WorldEntity entity in Entities)
            {
                distanceFromController = Position - entity.Position;
                if (distanceFromController.Length() > entity.Radius)
                    entity.Accelerate(Vector2.Normalize(Position - entity.Position), 10*(Mass-entity.Mass)*entity.Mass/(float)Math.Pow((distanceFromController.Length()), 1)); //2d gravity r is raised to 1
                //entity.Accelerate(Vector2.Normalize(Position - entity.Position), (float)Math.Pow(((distanceFromController.Length() - entity.Radius) / AverageDistance()) / 2 * entity.Mass, 2));
            }
        }

        protected void UpdatePosition() //TODO: only allow IsCollidable to affect this?
        {
            Vector2 sum = Vector2.Zero;
            float weight = 0;
            foreach (WorldEntity c in Entities)
            {
                weight += c.Mass;
                sum += c.Position * c.Mass;
            }
            if (weight > 0)
            {
                position = sum / (weight);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (WorldEntity c in Entities)
                c.Draw(sb);
        }
    }
}
