using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OBB_CD_Comparison.src.BVH
{
    public class BoundingCircleTree
    {
        BoundingCircleNode root = new();
        private Stack<BoundingCircleNode> freeNodes = new();

        BoundingCircleTree()
        {

        }

        public void Add(WorldEntity we)
        {
            if (root.children.Length == 0)
            {
                root.Add(we);
                return;
            }
            // Stage 1: find the best sibling for the new leaf
            INode bestSibling = FindBestSibling(we);

            // Stage 2: create a new parent
            BoundingCircleNode oldParent = bestSibling.Parent;
            BoundingCircleNode newParent = AllocateNode();
            newParent.Parent = oldParent;
            //newParent.BoundingCircle = Union(box, tree.nodes[sibling].box); done in stage 3
            if (bestSibling.Parent != null)
            {
                if (oldParent.children[0] == bestSibling)
                    oldParent.children[0] = newParent;
                else
                    oldParent.children[1] = newParent;

                newParent.Add(bestSibling);
                newParent.Add(we);
                //bestSibling.Parent = newParent;
                //we.Parent = newParent;
            }
            else
            {
                newParent.Add(bestSibling);
                newParent.Add(we);
                //bestSibling.Parent = newParent;
                //we.Parent = newParent;
                root = newParent;
            }

            // Stage 3: walk back up the tree refitting AABBs
            BoundingCircleNode parent = newParent;
            do
            {
                parent.RefitBoundingCircle();
                parent = parent.Parent;
            }while(parent != null);
        }

        private BoundingCircleNode AllocateNode()
        {
            if (freeNodes.Count == 0)
                return new BoundingCircleNode();
            else
            {
                BoundingCircleNode freeNode = freeNodes.Pop();
                freeNode.Reset();
                return freeNode;
            }
        }

        public INode FindBestSibling(WorldEntity weNew)
        {
            INode bestSibling = root;
            float bestCost = root.BoundingCircle.CombinedBoundingCircle(weNew.BoundingCircle).Area;
            PriorityQueue<INode, float> queue = new();

            while (queue.Count > 0)
            {
                queue.TryDequeue(
                        out INode currentNode,
                        out float inheritedCost
                );

                if (inheritedCost >= bestCost)
                    return bestSibling;
                float combinedArea = currentNode.BoundingCircle.CombinedBoundingCircle(weNew.BoundingCircle).Area;
                float currentCost = combinedArea + inheritedCost;
                if (currentCost < bestCost)
                {
                    bestSibling = currentNode;
                    bestCost = currentCost;
                }
                if (currentNode is BoundingCircleNode BCNode)
                {
                    inheritedCost += combinedArea - BCNode.BoundingCircle.Area;
                    float cLow = weNew.BoundingCircle.Area + inheritedCost;
                    if (cLow < bestCost)
                    {
                        if (BCNode.children[0] != null)
                            queue.Enqueue(BCNode.children[0], inheritedCost);
                        if (BCNode.children[1] != null)
                            queue.Enqueue(BCNode.children[1], inheritedCost);
                    }
                }

            }
            return bestSibling;
        }

        /*
        BoundingCircleNode root = new BoundingCircleNode();

        BoundingCircleTree(){
        }

        public void InsertLeaf(WorldEntity e)
        {
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

*/
    }

}