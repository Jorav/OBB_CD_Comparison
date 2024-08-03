﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OBB_CD_Comparison.src.BVH
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
                Vector2[] vectors;
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
        public Vector2 MassCenter{get;private set;}
        public ControllerBVH ParentController { get; set; }

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
            ParentController = parent;
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
                MassCenter = GetMassCenter();
                radius = GetRadius();
                e.ParentController = this;
            }
        }
        public bool RemoveEntity(IEntity c)
        {
            if (entities != null && entities.Contains(c))
            {
                entities.Remove(c);
                position = GetMassCenter();
                MassCenter = GetMassCenter();
                radius = GetRadius();
                c.ParentController = null;
                return true;
            }
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (IEntity c in Entities)
                c.Update(gameTime);
            if(ParentController == null)
            {
                UpdateTree();
                ApplyInternalGravity();
                InternalCollission();
            }
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
            if(bestSibling.ParentController != null){ //not root node: replace bestSibling with a node containing bestSibling and e
                ControllerBVH newParent = new();
                bestSibling.ParentController.AddEntity(newParent);
                bestSibling.ParentController.RemoveEntity(bestSibling);
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
        }

        private void RefitParentBoundingBoxes()
        {
            Radius = GetRadius();
            if(ParentController != null)
                ParentController.RefitParentBoundingBoxes();
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
                if(c1 is ControllerBVH bvh)
                    bvh.InternalCollission();
            }
        }
        public void Collide(IEntity e)
        {
            if (((IEntity)this).CollidesWith(e))
            {
                if (e is ControllerBVH c)
                {
                    foreach (IEntity ce in c.Entities)
                        Collide(ce);
                }
                else
                    foreach (IEntity e1 in Entities)
                        e1.Collide(e);
            }
            
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
        protected void ApplyInternalGravity()
        {
            Vector2 distanceFromController;
            foreach (IEntity entity in Entities)
            {
                distanceFromController = Position - entity.Position; // OBSOBSOBS make this depend on the distance of the worldentity, not the controller 
                if (distanceFromController.Length() > 1)//entity.Radius)
                    entity.AccelerateTo(Position, Game1.GRAVITY * (Mass - entity.Mass) / (float)Math.Pow((distanceFromController.Length()), 1)); //2d gravity r is raised to 1
                if(entity is ControllerBVH bvh)
                    bvh.ApplyInternalGravity();
            }
        }
        protected void ApplyInternalGravityN()
        {
            Vector2 distanceFromController;
            foreach (IEntity entity in Entities)
            {
                distanceFromController = Position - entity.Position;
                if (distanceFromController.Length() > 1)//entity.Radius)
                    entity.AccelerateTo(Position, Game1.GRAVITY * (Mass - 1) / (float)Math.Pow((distanceFromController.Length()), 1));
            }
        }

        protected void ApplyInternalGravityN2()
        {
            List<WorldEntity> worldEntities = new();
            UnwrapTree(worldEntities);
            foreach(WorldEntity we1 in worldEntities)
                foreach(WorldEntity we2 in worldEntities)
                    if(we1 != we2)
                        we1.AccelerateTo(we2.Position, Game1.GRAVITY * we1.Mass * we2.Mass / (float)Math.Pow(((we1.Position - we2.Position).Length()), 1));
        }

        public Vector2 GetPosition(){
            Vector2 sum = Vector2.Zero;
            int count = 0;
            foreach (IEntity c in Entities)
            {
                sum += c.Position;
                count++;
            }
            if(count > 0)
                return sum/count;
            return sum;
        }
        
        protected Vector2 GetMassCenter()
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

        public void AccelerateTo(Vector2 position, float force)
        {
            foreach (IEntity e in entities)
                e.AccelerateTo(position, force);
        }

        public void GenerateAxes()
        {
            foreach (IEntity e in entities)
                e.GenerateAxes();
        }
    }
}
