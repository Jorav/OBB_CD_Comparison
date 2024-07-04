﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OBB_CD_Comparison
{
    //OBS: Old heritage, god knows how this works
    public class CollidableRectangle
    {
        private Vector2 UL { get; set; }
        private Vector2 DL { get; set; }
        private Vector2 DR { get; set; }
        private Vector2 UR { get; set; }
        public float Width { get { return (int)(Math.Round((UR - UL).Length())); } }
        public float Height { get { return (int)(Math.Round((UR - DR).Length())); } }
        public Vector2 AbsolutePosition { get { return (UL + DR) / 2; } }
        private Vector2 origin;
        public Vector2 Origin
        {
            set
            {
                origin = value;
                UpdateRotation();
            }
            get
            {
                return origin;
            }
        }
        private Vector2 position;
        public Vector2 Position
        {
            set
            {
                Vector2 change = value - position;
                UL += change;
                DL += change;
                DR += change;
                UR += change;
                position = value;
            }
            get
            {
                return position;
            }
        }
        private float rotation = 0;
        public float Rotation
        {
            set
            {
                rotation = value;
                UpdateRotation();
            }
            get { return rotation; }
        }
        public float Radius { get { return (float)Math.Sqrt(Math.Pow(Width / 2, 2) + Math.Pow(Height / 2, 2)); } }

        public CollidableRectangle(Vector2 UL, Vector2 DL, Vector2 DR, Vector2 UR)
        {
            this.UL = UL;
            this.DL = DL;
            this.DR = DR;
            this.UR = UR;
        }
        public CollidableRectangle(Vector2 position, float rotation, int width, int height)
        {
            UL = new Vector2(position.X, position.Y);
            DL = new Vector2(position.X, position.Y + height);
            DR = new Vector2(position.X + width, position.Y + height);
            UR = new Vector2(position.X + width, position.Y);
            this.position = position;
            origin = new Vector2(Width / 2, Height / 2);
            Rotation = rotation;

        }
        public bool CollidesWith(CollidableRectangle r)
        {
            bool collides = true;
            Vector2[] axes = GenerateAxes(r);
            float[] scalarA = new float[4];
            float[] scalarB = new float[4];
            foreach (Vector2 axis in axes)
            {
                scalarA[0] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(UL, axis) / axis.LengthSquared()));
                scalarA[1] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(DL, axis) / axis.LengthSquared()));
                scalarA[2] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(DR, axis) / axis.LengthSquared()));
                scalarA[3] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(UR, axis) / axis.LengthSquared()));
                scalarB[0] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.UL, axis) / axis.LengthSquared()));
                scalarB[1] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.DL, axis) / axis.LengthSquared()));
                scalarB[2] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.DR, axis) / axis.LengthSquared()));
                scalarB[3] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.UR, axis) / axis.LengthSquared()));
                if (scalarB.Max() < scalarA.Min() + 1 || scalarA.Max() < scalarB.Min() + 1)
                    collides = false;
            }
            return collides;
        }

        private void UpdateRotation()
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);
            Vector2 height = new Vector2(0, Height);
            Vector2 width = new Vector2(Width, 0);
            UL = Position + Vector2.Transform(-Origin, rotationMatrix);
            DL = Position + Vector2.Transform(-Origin + height, rotationMatrix);
            DR = Position + Vector2.Transform(-Origin + height + width, rotationMatrix);
            UR = Position + Vector2.Transform(-Origin + width, rotationMatrix);
        }
        private void UpdateScale()
        {
            Matrix rotationMatrix = Matrix.CreateRotationZ(rotation);
            Vector2 height = new Vector2(0, Height);
            Vector2 width = new Vector2(Width, 0);
            UL = Position + Vector2.Transform(-Origin, rotationMatrix);
            DL = Position + Vector2.Transform(-Origin + height, rotationMatrix);
            DR = Position + Vector2.Transform(-Origin + height + width, rotationMatrix);
            UR = Position + Vector2.Transform(-Origin + width, rotationMatrix);
        }

        public bool Contains(Vector2 position)
        {
            Vector2 AM = position - UL;
            Vector2 AD = DL - UL;
            Vector2 AB = UR - UL;
            return 0 <= Vector2.Dot(AM, AB) && Vector2.Dot(AM, AB) <= Vector2.Dot(AB, AB) && 0 <= Vector2.Dot(AM, AD) && Vector2.Dot(AM, AD) <= Vector2.Dot(AD, AD);
        }

        public bool CollidesWithRectangle(CollidableRectangle r)
        {
            bool collides = true;
            Vector2[] axes = GenerateAxes(r);
            float[] scalarA = new float[4];
            float[] scalarB = new float[4];
            foreach (Vector2 axis in axes)
            {
                scalarA[0] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(UL, axis) / axis.LengthSquared()));
                scalarA[1] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(DL, axis) / axis.LengthSquared()));
                scalarA[2] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(DR, axis) / axis.LengthSquared()));
                scalarA[3] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(UR, axis) / axis.LengthSquared()));
                scalarB[0] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.UL, axis) / axis.LengthSquared()));
                scalarB[1] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.DL, axis) / axis.LengthSquared()));
                scalarB[2] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.DR, axis) / axis.LengthSquared()));
                scalarB[3] = Vector2.Dot(axis, Vector2.Multiply(axis, Vector2.Dot(r.UR, axis) / axis.LengthSquared()));
                if (scalarB.Max() < scalarA.Min() + 1 || scalarA.Max() < scalarB.Min() + 1)
                    collides = false;
            }/*
            if(!collides && stretchedRectangle != null)
            {
                return stretchedRectangle.CollidesWithRectangle(r);
            }*/
            return collides;
        }
        private Vector2[] GenerateAxes(CollidableRectangle r)
        {
            Vector2[] axes = new Vector2[4];
            axes[0] = new Vector2(UR.X - UL.X, UR.Y - UL.Y);
            axes[1] = new Vector2(UR.X - DR.X, UR.Y - DR.Y);
            axes[2] = new Vector2(r.UL.X - r.DL.X, r.UL.Y - r.DL.Y);
            axes[3] = new Vector2(r.UL.X - r.UR.X, r.UL.Y - r.UR.Y);
            return axes;
        }
    }
}