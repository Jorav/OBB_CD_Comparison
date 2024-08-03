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
                root.Add((IEntity)we);
                return;
            }
            // Stage 1: find the best sibling for the new leaf
            IEntity bestSibling = FindBestSibling(we);

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
                newParent.Add((IEntity)we);
                //bestSibling.Parent = newParent;
                //we.Parent = newParent;
            }
            else
            {
                newParent.Add(bestSibling);
                newParent.Add((IEntity)we);
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

        public IEntity FindBestSibling(WorldEntity weNew)
        {
            IEntity bestSibling = root;
            float bestCost = root.BoundingCircle.CombinedBoundingCircle(weNew.BoundingCircle).Area;
            PriorityQueue<IEntity, float> queue = new();

            while (queue.Count > 0)
            {
                queue.TryDequeue(
                        out IEntity currentNode,
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

        public void Draw(SpriteBatch sb)
        {
            root.Draw(sb);
        }
        public virtual void Update(GameTime gameTime)
        {
            root.Update(gameTime);
            RebuildTree();
            ApplyInternalGravity();
            InternalCollission();
        }

        private void InternalCollission()
        {
            root.InternalCollission();
        }

        private void ApplyInternalGravity()
        {
            root.ApplyInternalGravity();
        }

        private void RebuildTree()
        {
            throw new NotImplementedException();
        }
    }

}