

using System;
using Microsoft.Xna.Framework;

namespace OBB_CD_Comparison.src.BVH
{
    public class CollidableCircle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Area {get{return Radius*Radius;}}

        public CollidableCircle() { }
        public CollidableCircle(Vector2 position, float radius) 
        {
            Position = position;
            Radius = radius;
        }

        public CollidableCircle CombinedBoundingCircle(CollidableCircle cOther)
        {
            CollidableCircle largestCircle;
            CollidableCircle smallestCircle;
            if(Radius > cOther.Radius)
            {
                largestCircle = this;
                smallestCircle = cOther;
            }
            else
            {
                largestCircle = cOther;
                smallestCircle = this;
            }
            Vector2 smallestToLargest = largestCircle.Position-smallestCircle.Position;
            float distance = smallestToLargest.Length();
            smallestToLargest.Normalize();

            if(largestCircle.Radius > distance+smallestCircle.Radius) //if smallest circle completely inside large circle
                return new CollidableCircle(largestCircle.Position, largestCircle.Radius);
            else if(distance>=largestCircle.Radius) //if smallest circle center outside large circle
            {
                Vector2 position = (smallestCircle.Position+largestCircle.Position+smallestToLargest*largestCircle.Radius-smallestToLargest*smallestCircle.Radius)/2;
                float radius = (largestCircle.Position-position).Length() + largestCircle.Radius;
                return new CollidableCircle(position, radius);
            }
            else //if smallest circle center inside large circle but not fully
            {
                Vector2 position = (smallestCircle.Position+largestCircle.Position+smallestToLargest*largestCircle.Radius-smallestToLargest*smallestCircle.Radius)/2;
                float radius = (largestCircle.Position-position).Length()+largestCircle.Radius;
                return new CollidableCircle(position, radius);
            }
        }

        public bool CollidesWith(CollidableCircle c){
            return Math.Sqrt(Math.Pow((double)(Position.X) - (double)(c.Position.X), 2) + Math.Pow((double)(Position.Y) - (double)(c.Position.Y), 2)) <= (Radius + c.Radius);
        }
    }
}
