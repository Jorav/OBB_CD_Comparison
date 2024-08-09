using Microsoft.VisualBasic;
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
        private HashSet<WorldEntity> worldEntities = new();
        public List<(WorldEntity, WorldEntity)> CollissionPairs = new();

        public BoundingCircleTree()
        {
        }

        public void Add(WorldEntity we)
        {
            worldEntities.Add(we);
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
            if (oldParent != null)
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
                inheritedCost += combinedArea - currentNode.BoundingCircle.Area;
                float cLow = weNew.BoundingCircle.Area + inheritedCost;
                if (cLow < bestCost)
                {
                    if (currentNode.children[0] != null)
                        queue.Enqueue(currentNode.children[0], inheritedCost);
                    if (currentNode.children[1] != null)
                        queue.Enqueue(currentNode.children[1], inheritedCost);
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
            root.ApplyInternalGravityNLOGN();
            //ApplyInternalGravityN();
            //ApplyInternalGravityN2();
            root.GetInternalCollissions(CollissionPairs);
            ResolveInternalCollissions();
        }

        private void ApplyInternalGravityN()
        {
            Vector2 distanceFromController;
            foreach (WorldEntity entity in worldEntities)
            {
                distanceFromController = root.MassCenter - entity.Position;
                if (distanceFromController.Length() > entity.Radius)
                    entity.Accelerate(Vector2.Normalize(root.MassCenter - entity.Position), Game1.GRAVITY * (root.Mass - entity.Mass) * entity.Mass / (float)Math.Pow((distanceFromController.Length()), 1)); //2d gravity r is raised to 1
                //entity.Accelerate(Vector2.Normalize(Position - entity.Position), (float)Math.Pow(((distanceFromController.Length() - entity.Radius) / AverageDistance()) / 2 * entity.Mass, 2));
            }
        }
        private void ApplyInternalGravityN2()
        {
            foreach (WorldEntity we1 in worldEntities)
                foreach (WorldEntity we2 in worldEntities)
                    if (we1 != we2)
                        we1.AccelerateTo(we2.Position, Game1.GRAVITY * we1.Mass * we2.Mass / (float)Math.Pow(((we1.Position - we2.Position).Length()), 1));
        }

        private void ResolveInternalCollissions()
        {
            HashSet<WorldEntity> entities = new();
            foreach ((WorldEntity, WorldEntity) pair in CollissionPairs)
            {
                entities.Add(pair.Item1);
                entities.Add(pair.Item2);
            }
            foreach (WorldEntity we in entities)
                we.GenerateAxes();
            foreach ((WorldEntity, WorldEntity) pair in CollissionPairs)
            {
                if (pair.Item1.CollidesWith(pair.Item2))
                {
                    pair.Item1.Collide(pair.Item2);
                    pair.Item2.Collide(pair.Item1);
                }
            }
            CollissionPairs.Clear();
        }

        private void RebuildTree()
        {
            Stack<BoundingCircleNode> nodesToRebuild = new();

            nodesToRebuild.Push(root);

            while (nodesToRebuild.Count > 0)
            {
                BoundingCircleNode currentNode = nodesToRebuild.Pop();
                foreach (BoundingCircleNode child in currentNode.children)
                {
                    if (child != null)
                    {
                        if (child.WorldEntity == null)
                            nodesToRebuild.Push(child);
                    }
                }
                freeNodes.Push(currentNode);
            }
            root = null;
            foreach (WorldEntity we in worldEntities)
                Add(we);
        }
    }

}