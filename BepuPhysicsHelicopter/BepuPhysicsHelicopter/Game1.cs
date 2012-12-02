using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;

namespace BepuPhysicsHelicopter
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static Game1 instance = null;

        Texture2D crosshairs;
        SpriteBatch spriteBatch;
        List<GameEntity> children;
        GraphicsDeviceManager graphics;
        KeyboardState keyState;
        MouseState mouseState;


        Random random;
        Vector3 heliPos = new Vector3(0, 10, 10);
        Vector3 groundPos = new Vector3(0, 0, 0);



        float timeDelta;
        Camera camera;
        Space space;
        Cylinder cameraCylindar; 
        BepuEntity ground;
        
        public static Game1 Instance
        {
            get { return Game1.instance; }
            set { Game1.instance = value; }
        }

        public Space Space
        {
            get { return space; }
            set { space = value; }
        }

        public List<GameEntity> Children
        {
            get { return children; }
            set { children = value; }
        }


        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera();
            camera.Position = new Vector3(0, 30, 100);

            children = new List<GameEntity>();

            children.Add(camera);

            random = new Random();

            base.Initialize();
        }


        BepuEntity createGround(Vector3 position, float width, float height, float length)
        {
            ground = new BepuEntity();
            ground.modelName = "cube";                                              // Use the cube model
            ground.LoadContent();
            ground.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            ground.body.BecomeKinematic();                                          // Make the entity state
            ground.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            ground.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());   // Set the colour
            space.Add(ground.body);     // Add to the world
            children.Add(ground);       // Add to the list of entities
            return ground;
        }

        void resetWorld()     // Reset the world
        {
            for (int i = 0; i < children.Count(); i++)
            {
                if (children[i] is BepuEntity)
                {
                    children.Remove(children[i]);       // Remove all the entities in the list
                    i--;
                }
            }
            space = null;
            space = new Space();                                        // Create a new instance of space
            space.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);     // Apply real gravity


            cameraCylindar = new Cylinder(Camera.Position, 5, 2);       // Make the camera a cylinder   
            space.Add(cameraCylindar);

            createGround(new Vector3(0, 0, 0), 300, 1, 300);            // Create the ground
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resetWorld();                                       // Reset the world to it's original scene

            crosshairs = Content.Load<Texture2D>("sprites_crosshairs");

            foreach (GameEntity child in children)
            {
                child.LoadContent();
            }
        }


        protected override void Update(GameTime gameTime)
        {
            timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else if (keyState.IsKeyDown(Keys.Tab))
            {
                resetWorld();
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);
            }

            cameraCylindar.Position = camera.Position;  // Place the cylinder on the camera;
            space.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (GameEntity child in children)
            {
                child.Draw(gameTime);                   // Draw all the game entities
            }
            Vector2 center = Vector2.Zero, origin = Vector2.Zero;
            center.X = graphics.PreferredBackBufferWidth / 2;
            center.Y = graphics.PreferredBackBufferHeight / 2;
            Rectangle spriteRect = new Rectangle(76, 28, 15, 15);
            origin.X = spriteRect.Width / 2;
            origin.Y = spriteRect.Height / 2;
            spriteBatch.Draw(crosshairs, center, spriteRect, Color.Orange, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();
        }

        public Camera Camera
        {
            get
            {
                return camera;
            }
            set
            {
                camera = value;
            }
        }

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                return graphics;
            }
        }
    }
}
