﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OBB_CD_Comparison.src.BVH;
using OBB_CD_Comparison.src.old;
using System;
using System.Collections.Generic;

namespace OBB_CD_Comparison.src
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private AABBTree controllerTree;
        private Camera camera;
        private PerformanceMeasurer performanceMeasurer;
        //private MeanSquareError meanSquareError;
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static float GRAVITY = 10;
        public static int ITERATIONS_TO_FINISH = 60*10;
        public int currentIterations = 0;
        public static SpriteFont font;
        public static float timeStep = (1f/60f); 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // use this and Content to load your game content here
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D textureParticle = Content.Load<Texture2D>("RotatingHull");
            font = Content.Load<SpriteFont>("font");
            //Sprite spriteParticle = new Sprite(textureParticle);
            controllerTree = new AABBTree();
            /*
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(1134,245)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(1124,15)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(1124,120)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(0,0)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(3,300)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(500,5)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(110,0)));
            controllerTree.Add(new WorldEntity(textureParticle, new Vector2(232,300)));*/   

            string[] ConfigVar = EntityFactory.ReadConfig();
            GRAVITY= float.Parse(ConfigVar[2]);
            List<WorldEntity> returnedList = EntityFactory.EntFacImplementation(ConfigVar[0],ConfigVar[1],textureParticle);
            controllerTree.root = controllerTree.CreateTreeTopDown_Median(null, returnedList);
            /*foreach(WorldEntity w in returnedList)
            {
                controllerTree.Add(w);
            }*/
            camera = new Camera(controllerTree) { AutoAdjustZoom = true };
            performanceMeasurer = new PerformanceMeasurer();
            //meanSquareError = new MeanSquareError(returnedList.ToArray());
            //meanSquareError.LoadPreviousPositions();
        }

        protected override void Update(GameTime gameTime)
        {
            //UpdateRunning(gameTime);
            UpdateDeterministic(gameTime);
        }

        //for running
        protected void UpdateRunning(GameTime gameTime){
            // Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            controllerTree.Update(gameTime);
            camera.Update();
            performanceMeasurer.Update(gameTime);
            //meanSquareError.Update(gameTime);
            base.Update(gameTime);
        }

        //for testing
        protected void UpdateDeterministic(GameTime gameTime){
            // Add your update logic here
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            controllerTree.UpdateDeterministic(performanceMeasurer);
            camera.Update();
            //meanSquareError.UpdateDeterministic(timeStep);
            base.Update(gameTime);
            if(++currentIterations == ITERATIONS_TO_FINISH)
                Exit();
        }

        protected override void Draw(GameTime gameTime)
        {
            // Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: camera.Transform);
           // _spriteBatch.Begin();
                controllerTree.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            performanceMeasurer.Exit();
            //meanSquareError.Exit();
            base.OnExiting(sender, args);
        }
    }
}
