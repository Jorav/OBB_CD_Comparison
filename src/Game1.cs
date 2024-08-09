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
        private BoundingCircleTree controllerTree;
        private Camera camera;
        private PerformanceMeasurer performanceMeasurer;
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static float GRAVITY = 10;

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
            performanceMeasurer = new PerformanceMeasurer();
            _graphics.ApplyChanges();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D textureParticle = Content.Load<Texture2D>("RotatingHull");
            //Sprite spriteParticle = new Sprite(textureParticle);
            controllerTree = new BoundingCircleTree();
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
            foreach(WorldEntity w in returnedList)
            {
                controllerTree.Add(w);
            }
            camera = new Camera(controllerTree) { AutoAdjustZoom = true };
        }

        protected override void Update(GameTime gameTime)
        {
            // Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            controllerTree.Update(gameTime);
            camera.Update();
            performanceMeasurer.Update(gameTime);
            base.Update(gameTime);
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
            base.OnExiting(sender, args);
        }
    }
}
