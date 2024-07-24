using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
namespace OBB_CD_Comparison
{
    public class ControllerBVH : IEntity
    {
        protected List<IEntity> entities;
        public List<IEntity> Entities { get { return entities; } set { SetEntities(value); } }
        //rotected float collissionOffset = 100; //TODO make this depend on velocity + other things?
        public float Radius { get { return radius; } protected set { radius = value; } }
        protected float radius;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                Vector2 posChange = value - Position;
                foreach (IEntity c in Entities)
                    c.Position += posChange;
                position = value;
            }
        }

        public float Mass
        {
            get
            {
                float sum = 0;
                foreach (IEntity c in Entities)
                    sum += c.Mass;
                return sum;
            }
        }

        public ControllerBVH Parent { get; set; }

        protected Vector2 position;

        public ControllerBVH(List<IEntity> controllables)
        {
            SetEntities(controllables);
        }

        public ControllerBVH([OptionalAttribute] Vector2 position, [OptionalAttribute] ControllerBVH parent)
        {
            if (position == null)
                position = Vector2.Zero;
            SetEntities(new List<IEntity>());
            Parent = parent;
        }
        public virtual void SetEntities(List<IEntity> newControllables)
        {
            if (newControllables != null)
            {
                List<IEntity> oldControllables = Entities;
                entities = new List<IEntity>();
                foreach (IEntity c in newControllables)
                    AddEntity(c);
                if (Entities.Count == 0)
                {
                    Entities = oldControllables;
                }
            }
        }
        public void AddEntity(IEntity e)
        {
            if (entities == null)
            {
                entities = new List<IEntity>();
            }

            if (e != null)
            {
                entities.Add(e);
                position = GetMassCenter();
                radius = GetRadius();
                e.Parent = this;
            }
        }
        public bool RemoveEntity(IEntity c)
        {
            if (entities != null && entities.Contains(c))
            {
                entities.Remove(c);
                position = GetMassCenter();
                radius = GetRadius();
                c.Parent = null;
                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateEntities(gameTime);
            if(Parent == null)
                UpdateTree();
            position = GetMassCenter();
            Radius = GetRadius();
            ApplyInternalGravity();
            InternalCollission();
        }

        private void UpdateTree()
        {
            List<WorldEntity> EToBeReinserted = new();
            UnwrapTree(EToBeReinserted);
            Entities.Clear();
            foreach(WorldEntity we in EToBeReinserted)
                InsertLeaf(we);
        }

        private List<WorldEntity> UnwrapTree(List<WorldEntity> list){
            foreach(IEntity e in Entities){
                if(e is ControllerBVH bvh)
                    bvh.UnwrapTree(list);
                else if(e is WorldEntity we)
                    list.Add(we);
            }
            return list;
        }

        public void InsertLeaf(WorldEntity e) //que
        {
            //ControllerBVH leafController = new();
            //leafController.AddEntity(e);

            if(Entities.Count == 0){
                AddEntity(e);
                return;
            }
            
            // Stage 1: find the best sibling for the new leaf
            IEntity bestSibling = BranchAndBound(e, this, AreaIncrease(e)+Radius*Radius, 0, new PriorityQueue<IEntity, float>());

            // Stage 2: create a new parent
            if(bestSibling.Parent != null){ //not root node: replace bestSibling with a node containing bestSibling and e
                ControllerBVH newParent = new();
                bestSibling.Parent.AddEntity(newParent);
                bestSibling.Parent.RemoveEntity(bestSibling);
                newParent.AddEntity(bestSibling);
                newParent.AddEntity(e);
                newParent.RefitParentBoundingBoxes();// update upwards
            }
            else{ //root node: copy the root node, and make it a branch of this, together with e
                if(Entities.Count == 2)
                {
                    ControllerBVH rootCopy = new();
                    List<IEntity> oldEntities = new();
                    foreach(IEntity entity in entities)
                        oldEntities.Add(entity);
                    foreach(IEntity entity in oldEntities){
                        entities.Remove(entity);
                        rootCopy.AddEntity(entity);
                    }
                    AddEntity(rootCopy);
                }
                AddEntity(e);
            }
            // Stage 3: walk back up the tree refitting boundingcircles

            // Stage 1: find the best sibling for the new leaf
            /**
                int bestSibling = 0;
                for (int i = 0; i < m_nodeCount; ++i)
                {
                bestSibling = PickBest(bestSibling, i);
                }
            */
            // Stage 2: create a new parent
            /**
                int oldParent = tree.nodes[sibling].parentIndex;
                int newParent = AllocateInternalNode(tree);
                tree.nodes[newParent].parentIndex = oldParent;
                tree.nodes[newParent].box = Union(box, tree.nodes[sibling].box);
                if (oldParent != nullIndex)
                {
                    // The sibling was not the root
                    if (tree.nodes[oldParent].child1 == sibling)
                    {
                        tree.nodes[oldParent].child1 = newParent;
                    }
                    else
                    {
                        tree.nodes[oldParent].child2 = newParent;
                    }
                    tree.nodes[newParent].child1 = sibling;
                    tree.nodes[newParent].child2 = leafIndex;
                    tree.nodes[sibling].parentIndex = newParent;
                    tree.nodes[leafIndex].parentIndex = newParent;
                }
                else
                {
                // The sibling was the root
                tree.nodes[newParent].child1 = sibling;
                tree.nodes[newParent].child2 = leafIndex;
                tree.nodes[sibling].parentIndex = newParent;
                tree.nodes[leafIndex].parentIndex = newParent;
                tree.rootIndex = newParent;
                }
            */

            // Stage 3: walk back up the tree refitting boundingcircles
            /**
                int index = tree.nodes[leafIndex].parentIndex;
                while (index != nullIndex)
                {
                int child1 = tree.nodes[index].child1;
                int child2 = tree.nodes[index].child2;
                tree.nodes[index].box = Union(tree.nodes[child1].box, tree.nodes[child2].box);
                index = tree.nodes[index].parentIndex;
                }
            */
            
            //return newArea - currentArea;
        }

        private void RefitParentBoundingBoxes()
        {
            Radius = GetRadius();
            if(Parent != null)
                Parent.RefitParentBoundingBoxes();
        }

        public IEntity BranchAndBound(WorldEntity eNew, IEntity bestEntity, float bestCost, float inheritedCost, PriorityQueue<IEntity, float> queue){
            if(inheritedCost >= bestCost)
                return bestEntity; //return the best node

            float areaIncrease = AreaIncrease(eNew);
            float totalCost = areaIncrease + Radius*Radius + inheritedCost;
            if(totalCost<bestCost){
                bestEntity = this;
                bestCost = totalCost;
            }
                
            inheritedCost+=areaIncrease;
            foreach(IEntity eBranch in entities)
                if (eNew.Radius*eNew.Radius + inheritedCost < bestCost)
                    queue.Enqueue(eBranch, inheritedCost);
                
            if(queue.Count == 0)
                return bestEntity; //return the best node
            else{
                queue.TryDequeue(
                    out IEntity nextEntity,
                    out float nextInheritedCost
            );
                return nextEntity.BranchAndBound(eNew, bestEntity, bestCost, nextInheritedCost, queue);
            }
            
        }

        protected float AreaIncrease(IEntity e) {
            float currentArea = Radius * Radius;
            AddEntity(e);
            float newArea = Radius * Radius;
            RemoveEntity(e);
            return newArea-currentArea;
        }

        protected void InternalCollission()
        {
            foreach (IEntity c1 in Entities)
            {
                foreach (IEntity c2 in Entities)
                {
                    if (c1 != c2)
                        c1.Collide(c2);
                }
            }
        }
        public void Collide(IEntity e)
        {
            if (((IEntity)this).CollidesWith(e))
            {
                if (e is ControllerBVH c)
                    foreach (IEntity ce in c.Entities)
                        Collide(ce);
                else
                    foreach (IEntity e1 in Entities)
                        e1.Collide(e);
            }
            
        }

        private void UpdateEntities(GameTime gameTime)
        {
            foreach (IEntity c in Entities)
                c.Update(gameTime);
        }

        //TODO: make this work 
        protected float GetRadius() //TODO: Update this to make it more efficient, e.g. by having sorted list, TODO: only allow IsCollidable to affect this?
        {
            if (Entities.Count == 1)
            {
                if (Entities[0] != null)
                   return Entities[0].Radius;
            }
            else if (Entities.Count > 1)
            {
                float largestDistance = 0;
                foreach (IEntity c in Entities)
                {
                    float distance = Vector2.Distance(c.Position, Position) + c.Radius;
                    if (distance > largestDistance)
                        largestDistance = distance;
                }
                return largestDistance;
            }
            return 0;
        }
        protected float AverageDistance()
        {
            float nr = 1;
            float distance = 0;
            float mass = 0;
            foreach (IEntity c in Entities)
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
            foreach (IEntity entity in Entities)
            {
                distanceFromController = Position - entity.Position;
                if (distanceFromController.Length() > entity.Radius)
                    entity.Accelerate(Vector2.Normalize(Position - entity.Position), 10 * (Mass - entity.Mass) * entity.Mass / (float)Math.Pow((distanceFromController.Length()), 1)); //2d gravity r is raised to 1
                //entity.Accelerate(Vector2.Normalize(Position - entity.Position), (float)Math.Pow(((distanceFromController.Length() - entity.Radius) / AverageDistance()) / 2 * entity.Mass, 2));
                if(entity is ControllerBVH bvh)
                    bvh.ApplyInternalGravity();
            }
        }

        protected Vector2 GetMassCenter() //TODO: only allow IsCollidable to affect this?
        {
            Vector2 sum = Vector2.Zero;
            float weight = 0;
            foreach (IEntity c in Entities)
            {
                weight += c.Mass;
                sum += c.Position * c.Mass;
            }
            if (weight > 0)
            {
                return sum / (weight);
            }
            return sum;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (IEntity c in Entities)
                c.Draw(sb);
        }

        public void Accelerate(Vector2 direction, float force)
        {
            foreach (IEntity e in entities)
                e.Accelerate(direction, force);
        }

        public void GenerateAxes()
        {
            foreach (IEntity e in entities)
                e.GenerateAxes();
        }
    }
}
