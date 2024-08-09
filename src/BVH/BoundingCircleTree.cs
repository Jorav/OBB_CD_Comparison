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
        public Vector2 Position { get { return root.Position; } }
        public float Radius { get { return root.Radius; } }
        BoundingCircleNode root;
        private Stack<BoundingCircleNode> freeNodes = new();
        public static List<(WorldEntity, WorldEntity)> CollissionPairs = new();

        public BoundingCircleTree()
        {
        }

        public void Add(WorldEntity we)
        {
            BoundingCircleNode leaf = AllocateLeafNode(we);
            if (root == null)
            {
                root = leaf;
                return;
            }
            // Stage 1: find the best sibling for the new leaf
            BoundingCircleNode bestSibling = FindBestSibling(we);

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
            }
            else
            {
                root = newParent;
            }
            newParent.Add(bestSibling);
            newParent.Add(leaf);
            // Stage 3: walk back up the tree refitting AABBs
            BoundingCircleNode parent = newParent;
            do
            {
                parent.RefitBoundingCircle();
                parent = parent.Parent;
            } while (parent != null);
        }

        private BoundingCircleNode AllocateLeafNode(WorldEntity we)
        {
            BoundingCircleNode leafNode = AllocateNode();
            leafNode.WorldEntity = we;
            return leafNode;
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

        public BoundingCircleNode FindBestSibling(WorldEntity weNew)
        {
            BoundingCircleNode bestSibling = root;
            float bestCost = root.BoundingCircle.CombinedBoundingCircle(weNew.BoundingCircle).Area;
            PriorityQueue<BoundingCircleNode, float> queue = new();
            queue.Enqueue(root, 0);            

            while (queue.Count > 0)
            {
                queue.TryDequeue(
                        out BoundingCircleNode currentNode,
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
            root.ApplyInternalGravity();
            root.GetInternalCollissions(CollissionPairs);
            ResolveInternalCollissions();
        }

        private void ResolveInternalCollissions()
        {
            HashSet<WorldEntity> entities = new();
            foreach((WorldEntity, WorldEntity) pair in CollissionPairs){
                entities.Add(pair.Item1);
                entities.Add(pair.Item2);
            }
            foreach(WorldEntity we in entities)
                we.GenerateAxes();
            foreach((WorldEntity, WorldEntity) pair in CollissionPairs){
                pair.Item1.Collide(pair.Item2);
                pair.Item2.Collide(pair.Item1);
            }
            CollissionPairs.Clear();
        }

        private void RebuildTree()
        {
            Stack<WorldEntity> entities = new();
            Stack<BoundingCircleNode> nodesToRebuild = new();

            nodesToRebuild.Push(root);

            while (nodesToRebuild.Count > 0)
            {
                BoundingCircleNode currentNode = nodesToRebuild.Pop();
                foreach (BoundingCircleNode child in currentNode.children)
                {
                    if (child != null)
                    {
                        if (child.WorldEntity != null)
                            entities.Push(child.WorldEntity);
                        else
                            nodesToRebuild.Push(child);
                    }
                }
                freeNodes.Push(currentNode);
            }
            root = null;
            foreach (WorldEntity we in entities)
                Add(we);
        }
    }

}