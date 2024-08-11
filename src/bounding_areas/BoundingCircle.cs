

using System;
using Microsoft.Xna.Framework;

namespace OBB_CD_Comparison.src.bounding_areas
{
    public class BoundingCircle
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Area {get{return Radius*Radius;}}

        public BoundingCircle(Vector2 position, float radius) 
        {
            Position = position;
            Radius = radius;
        }

        public BoundingCircle CombinedBoundingCircle(BoundingCircle cOther)
        {
            BoundingCircle largestCircle;
            BoundingCircle smallestCircle;
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
                return BoundingAreaFactory.CreateCircle(largestCircle.Position, largestCircle.Radius);
            else if(distance>=largestCircle.Radius) //if smallest circle center outside large circle
            {
                Vector2 position = (smallestCircle.Position+largestCircle.Position+smallestToLargest*largestCircle.Radius-smallestToLargest*smallestCircle.Radius)/2;
                float radius = (largestCircle.Position-position).Length() + largestCircle.Radius;
                return BoundingAreaFactory.CreateCircle(position, radius);
            }
            else //if smallest circle center inside large circle but not fully
            {
                Vector2 position = (smallestCircle.Position+largestCircle.Position+smallestToLargest*largestCircle.Radius-smallestToLargest*smallestCircle.Radius)/2;
                float radius = (largestCircle.Position-position).Length()+largestCircle.Radius;
                return BoundingAreaFactory.CreateCircle(position, radius);
            }
        }

        public bool CollidesWith(BoundingCircle c){
            return Math.Sqrt(Math.Pow((double)(Position.X) - (double)(c.Position.X), 2) + Math.Pow((double)(Position.Y) - (double)(c.Position.Y), 2)) <= (Radius + c.Radius);
        }
    }
}
