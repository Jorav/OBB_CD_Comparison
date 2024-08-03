using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OBB_CD_Comparison.src.BVH;
using System;
using System.Collections.Generic;
using System.Text;

namespace OBB_CD_Comparison.src.BVH
{
    public interface INode
    {
        public Vector2 Position { get; set; }
        public Vector2 MassCenter { get; }
        public float Radius { get; }
        public float Mass { get; }
        public CollidableCircle BoundingCircle {get;}
        public BoundingCircleNode Parent {get;set;}
        /*
        public void Update(GameTime gameTime);
        public void Accelerate(Vector2 direction, float force);
        public void AccelerateTo(Vector2 position, float force);
        public bool CollidesWith(IEntity e){
            return Math.Sqrt(Math.Pow((double)(Position.X) - (double)(e.Position.X), 2) + Math.Pow((double)(Position.Y) - (double)(e.Position.Y), 2)) <= (Radius + e.Radius);
        }
        public void Draw(SpriteBatch spriteBatch);
        public void Collide(IEntity e);
        public void GenerateAxes();
        public IEntity BranchAndBound(WorldEntity eNew, IEntity bestEntity, float bestCost, float inheritedCost, PriorityQueue<IEntity, float> queue);*/
    }
}
