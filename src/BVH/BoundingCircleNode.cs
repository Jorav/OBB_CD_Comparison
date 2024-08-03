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
    public class BoundingCircleNode : INode
    {
        public BoundingCircleNode Parent {get; set;}
        public INode[] children = new INode[2];
        public float Radius { get { return radius; } protected set { radius = value; } }
        protected float radius;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 posChange = value - Position;
                Vector2[] vectors;
                foreach (INode c in children)
                    c.Position += posChange;
                position = value;
            }
        }
        protected Vector2 position;
        public float Mass {get;set;}
        public Vector2 MassCenter{get; private set;}
        public CollidableCircle BoundingCircle{get;private set;}
        public BoundingCircleNode()
        {
        }
        public BoundingCircleNode(BoundingCircleNode parent, BoundingCircleNode child1, BoundingCircleNode child2)
        {
            this.Parent = parent;
            children[0] = child1;
            children[1] = child2;
        }
        public void Add(INode e){
            if(children[0] == null)
                children[0] = e;
            else if(children[1] == null)
                children[1] = e;
            e.Parent = this;
            Mass+=e.Mass;
        }
        protected float CalculateRadius() //TODO: Update this to make it more efficient, e.g. by having sorted list, TODO: only allow IsCollidable to affect this?
        {
            if (children.Length == 1)
            {
                if (children[0] != null)
                   return children[0].Radius;
                else
                    return children[1].Radius;
            }
            else if (children.Length > 1)
            {
                return (Vector2.Distance(children[0].Position, children[1].Position) + children[0].Radius + children[1].Radius)/2;
            }
            return 0;
        }
        protected Vector2 CalculatePosition(){
            if(children.Count() == 0)
                return Vector2.Zero;
            if(children.Count() == 1)
                if(children[0] != null)
                    return children[0].Position;
                else
                    return children[1].Position;
            return (children[0].Position+children[1].Position)/2;
        }

        public void RefitBoundingCircle()
        {
            if(children.Length == 1)
                if(children[0] != null)
                    BoundingCircle = children[0].BoundingCircle; //BoundingCircle = new CollidableCircle(children[0].BoundingCircle.Position, children[0].BoundingCircle.Radius);
                else
                    BoundingCircle = children[1].BoundingCircle;

            else if (children.Length == 2){
                CollidableCircle newBoundingCircle = children[0].BoundingCircle.CombinedBoundingCircle(children[1].BoundingCircle);
                if(BoundingCircle == null)
                    BoundingCircle = new();
                BoundingCircle.Position = newBoundingCircle.Position;
                BoundingCircle.Radius = newBoundingCircle.Radius;
            }
            Position = BoundingCircle.Position;
            Radius = BoundingCircle.Radius;
            
            CalculateMassCenter();
        }
        public void CalculateMassCenter(){
            MassCenter = Vector2.Zero;
            Mass = 0;
            foreach(INode child in children)
                if(child != null)
                {
                    Mass+=child.Mass;
                    MassCenter+=child.MassCenter*child.Mass;
                }
            MassCenter /= Mass;
        }

        public void Reset()
        {
            BoundingCircle = null;
            children[0] = null;
            children[1] = null;
            Parent = null;
            Radius = 0;
            position = Vector2.Zero;
            MassCenter = Vector2.Zero;
        }
    }
}