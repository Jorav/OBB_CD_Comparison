using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OBB_CD_Comparison.src;

namespace OBB_CD_Comparison.src.BVH
{
    public class BoundingCircleNode : IEntity
    {
        public BoundingCircleNode Parent { get; set; }
        public IEntity[] children = new IEntity[2];
        public float Radius { get { return radius; } protected set { radius = value; } }
        protected float radius;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 posChange = value - Position;
                Vector2[] vectors;
                foreach (IEntity c in children)
                    c.Position += posChange;
                position = value;
            }
        }
        protected Vector2 position;
        public float Mass { get; set; }
        public int Count { get{return count;} set{int difference = value-count; count = value; if(Parent != null) Parent.Count+=difference;} }
        private int count;
        public Vector2 MassCenter { get; private set; }
        public CollidableCircle BoundingCircle { get; private set; }
        public BoundingCircleNode()
        {
        }
        public BoundingCircleNode(BoundingCircleNode parent, BoundingCircleNode child1, BoundingCircleNode child2)
        {
            this.Parent = parent;
            children[0] = child1;
            children[1] = child2;
        }
        #region node-functionality
        public void Add(IEntity e)
        {
            if (children.Length < 2)
            {
                if (children[0] == null)
                    children[0] = e;
                else if (children[1] == null)
                    children[1] = e;
                e.Parent = this;
                Mass += e.Mass;
                if(e is BoundingCircleNode node)
                    Count+=node.Count;
                else
                    Count++;
            }
        }

        public void RefitBoundingCircle()
        {
            if (children.Length == 1)
                if (children[0] != null)
                    BoundingCircle = children[0].BoundingCircle; //BoundingCircle = new CollidableCircle(children[0].BoundingCircle.Position, children[0].BoundingCircle.Radius);
                else
                    BoundingCircle = children[1].BoundingCircle;

            else if (children.Length == 2)
            {
                CollidableCircle newBoundingCircle = children[0].BoundingCircle.CombinedBoundingCircle(children[1].BoundingCircle);
                if (BoundingCircle == null)
                    BoundingCircle = new();
                BoundingCircle.Position = newBoundingCircle.Position;
                BoundingCircle.Radius = newBoundingCircle.Radius;
            }
            Position = BoundingCircle.Position;
            Radius = BoundingCircle.Radius;

            UpdateMassCenter();
        }
        public void UpdateMassCenter()
        {
            MassCenter = Vector2.Zero;
            Mass = 0;
            foreach (IEntity child in children)
                if (child != null)
                {
                    Mass += child.Mass;
                    MassCenter += child.MassCenter * child.Mass;
                }
            MassCenter /= Mass;
        }

        public void Reset()
        {
            BoundingCircle = null;
            children[0] = null;
            children[1] = null;
            Parent = null;
            Count = 0;
            Radius = 0;
            position = Vector2.Zero;
            MassCenter = Vector2.Zero;
        }
        #endregion
        #region update-logic
        public void Draw(SpriteBatch sb)
        {
            foreach (IEntity c in children)
                c.Draw(sb);
        }
        public void Update(GameTime gameTime)
        {
            foreach (IEntity child in children)
                child.Update(gameTime);
        }

        internal void InternalCollission()
        {
            if (children.Length == 2)
                children[0].Collide(children[1]);
            if (children[0] != null && children[0] is BoundingCircleNode node1)
                node1.InternalCollission();
            if (children[1] != null && children[0] is BoundingCircleNode node2)
                node2.InternalCollission();
        }

        public void Collide(IEntity e)
        {
            if (BoundingCircle.CollidesWith(e.BoundingCircle))
            {
                if (e is BoundingCircleNode node)
                {
                    foreach (IEntity childE in node.children)
                        Collide(childE);
                }
                else
                    foreach (IEntity child in children)
                        child.Collide(e);
            }
        }
        internal void ApplyInternalGravity()
        {
            Vector2 distanceFromController;
            foreach (IEntity entity in children)
            {
                distanceFromController = MassCenter - entity.MassCenter; // OBSOBSOBS make this depend on the distance of the worldentity, not the controller 
                if (distanceFromController.Length() > 1)//entity.Radius)
                    entity.AccelerateTo(MassCenter, Game1.GRAVITY * (Mass - entity.Mass) / (float)Math.Pow((distanceFromController.Length()), 1)); //2d gravity r is raised to 1
                if (entity is BoundingCircleNode node)
                    node.ApplyInternalGravity();
            }
        }

        public void AccelerateTo(Vector2 position, float force)
        {
            foreach (IEntity e in children)
                e.AccelerateTo(position, force);
        }
        #endregion
    }
}