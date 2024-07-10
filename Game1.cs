using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace OBB_CD_Comparison
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Controller controller;
        private PerformanceMeasurer performanceMeasurer;
        public static int ScreenWidth;
        public static int ScreenHeight;

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
            controller = new Controller();
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(100, 100), 100f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(200, 200)));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(700, 700),50f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-500, 700),200f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-570, 755), 1200f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-580, 523), 30f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(200, 100), 100f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(300, 200)));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(400, 700), 50f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-600, 700), 200f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-770, 755), 1200f));
            controller.AddEntity(new WorldEntity(textureParticle, new Vector2(-880, 523), 30f));


        }

        protected override void Update(GameTime gameTime)
        {
            // Add your update logic here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            controller.Update(gameTime);
            performanceMeasurer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Add your drawing code here
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(transformMatrix: controller.Camera.Transform);
           // _spriteBatch.Begin();
                controller.Draw(_spriteBatch);
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
