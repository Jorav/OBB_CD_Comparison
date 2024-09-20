using Microsoft.Xna.Framework;
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
        //private Controller controller;
        private Camera camera;
        private PerformanceMeasurer performanceMeasurer;
        //private MeanSquareError meanSquareError;
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static float GRAVITY = 10;
        public static SpriteFont font;
        public static int FRAMES_PER_SECOND = 60;
        public static float TIME_STEP = (1f/FRAMES_PER_SECOND);
        public Tests tests;

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
            controllerTree = new AABBTree();

            string[] ConfigVar = EntityFactory.ReadConfig();
            int seed = int.Parse(ConfigVar[0]);
            int nr = int.Parse(ConfigVar[1]);
            float gravity = float.Parse(ConfigVar[2]);
            int test_case = int.Parse(ConfigVar[3]);
            
            //List<WorldEntity> returnedList = EntityFactory.EntFacImplementation(ConfigVar[0],ConfigVar[1],textureParticle);
            //int seed = 100;
            camera = new Camera();
            Tests.nrOfEntities = new int[] { nr };
            tests = new Tests(this, camera, seed);
            
            /*int maxEntitiesNeeded = 0;
            for(int i = 0; i< tests.nrOfEntities.Length; i++)
                if(tests.nrOfEntities[i] > maxEntitiesNeeded)
                    maxEntitiesNeeded = tests.nrOfEntities[i];*/
            GRAVITY = gravity;
            Tests.CURRENT_CONTROLLER_TEST = test_case;
            tests.LoadEntities(EntityFactory.EntFacImplementation(seed.ToString(), nr.ToString(), textureParticle));
            //controllerTree.root = controllerTree.CreateTreeTopDown_Median(null, returnedList);
            //controller = new Controller(returnedList);
            /*foreach(WorldEntity w in returnedList)
            {
                controllerTree.Add(w);
            }*/
            //camera = new Camera(controllerTree) { AutoAdjustZoom = true };
            //performanceMeasurer = new PerformanceMeasurer();
            //meanSquareError = new MeanSquareError(returnedList.ToArray());
            //meanSquareError.LoadPreviousPositions();
        }

        protected override void Update(GameTime gameTime)
        {
            //UpdateRunning(gameTime);
            //UpdateDeterministic(gameTime);
            tests.Update(gameTime);
            base.Update(gameTime);
        }

        //for running
        protected void UpdateRunning(GameTime gameTime){
            // Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //controllerTree.Update(gameTime);
            //controller.Update(gameTime);
            //camera.Update();
            //performanceMeasurer.Update(gameTime);
            //meanSquareError.Update(gameTime);
            base.Update(gameTime);
        }

        //for testing
        protected void UpdateDeterministic(GameTime gameTime){
            // Add your update logic here
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            //controllerTree.UpdateDeterministic(performanceMeasurer);
            //controller.UpdateDeterministic();
            //camera.Update();
            //meanSquareError.UpdateDeterministic(timeStep);
            tests.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {/*
            // Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: camera.Transform);
           // _spriteBatch.Begin();
            tests.Draw(_spriteBatch);
            //controllerTree.Draw(_spriteBatch);
            //controller.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);*/
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            //performanceMeasurer.Exit();
            //meanSquareError.Exit();
            base.OnExiting(sender, args);
        }
    }
}
