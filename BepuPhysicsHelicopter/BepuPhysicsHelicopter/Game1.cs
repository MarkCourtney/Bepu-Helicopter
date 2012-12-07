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
        DepthStencilState state;

        Texture2D crosshairs;
        SpriteBatch spriteBatch;
        List<GameEntity> children;
        GraphicsDeviceManager graphics;
        KeyboardState keyState;
        MouseState mouseState;
        SoundEffect helicopterNoise, rideOfTheValkyries;


        Random random;
        Vector3 heliPos = new Vector3(-50, 15, -10);
        Vector3 groundPos = new Vector3(0, 0, 0);
        Vector3 seaSawPos = new Vector3(190, 70, 0);

        // New BepuEntity objects
        HelicopterBase hBase;

        public HelicopterBase HBase
        {
            get { return hBase; }
            set { hBase = value; }
        }
        HelicopterRotor hRotor;
        HelicopterSkid hSkid;
        HelicopterTail hTail;
        HelicopterTailRotor hTailRotor;
        HelicopterClawHolder hClawHolder;
        HelicopterClawHinge hClawHinge;
        HelicopterClawItemHolder hClawItemHolder;
        SeaSaw seaSaw;

        float timeDelta;
        Camera camera;
        Space space;
        Cylinder cameraCylindar;
        BepuEntity ground, box, slope, ball;

        float r, g, b;

        Joints joints;

        public Joints Joints
        {
            get { return joints; }
            set { joints = value; }
        }
        
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

            state = new DepthStencilState();
            state.DepthBufferEnable = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera();
            camera.Position = new Vector3(0, 30, 100);

            children = new List<GameEntity>();

            children.Add(camera);

            random = new Random();

            hBase = new HelicopterBase();   // Instance of HelicopterBase
            hRotor = new HelicopterRotor();
            hSkid = new HelicopterSkid();
            hTail = new HelicopterTail();
            hTailRotor = new HelicopterTailRotor();
            hClawHolder = new HelicopterClawHolder();
            hClawHinge = new HelicopterClawHinge();
            hClawItemHolder = new HelicopterClawItemHolder();
            seaSaw = new SeaSaw();
            helicopterNoise = Content.Load<SoundEffect>("Helicopter Sound Effect");
            rideOfTheValkyries = Content.Load<SoundEffect>("ROTV");
            
            base.Initialize();
        }


        BepuEntity createBox(Vector3 position, float width, float height, float length, float r, float g, float b)
        {
            box = new BepuEntity();
            box.modelName = "cube";                                              // Use the cube model
            box.LoadContent();
            box.body = new Box(position, width, height, length, 200);                 // Place it in the world and give it's dimensions
            box.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            box.diffuse = new Vector3(r, g, b);                       // Set the colour to a shade of yellow
            space.Add(box.body);                                                 // Add to the world
            children.Add(box);                                                   // Add to the list of entities
            return box;
        }


        BepuEntity createSlope(Vector3 position, float width, float height, float length)
        {
            slope = new BepuEntity();
            slope.modelName = "cube";                                              // Use the cube model
            slope.LoadContent();
            slope.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            slope.body.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 0.7f), 10000);
            slope.body.BecomeKinematic();                                          // Make the entity state
            slope.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            slope.diffuse = new Vector3(0.3f, 0.3f, 0.45f);                       // Set the colour to a shade of yellow
            space.Add(slope.body);                                                 // Add to the world
            children.Add(slope);                                                   // Add to the list of entities
            return slope;
        }

        BepuEntity createGround(Vector3 position, float width, float height, float length)
        {
            ground = new BepuEntity();
            ground.modelName = "cube";                                              // Use the cube model
            ground.LoadContent();
            ground.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            ground.body.BecomeKinematic();                                          // Make the entity state
            ground.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            ground.diffuse = new Vector3(0.15f, 0.15f, 0.3f);                       // Set the colour to a shade of yellow
            space.Add(ground.body);                                                 // Add to the world
            children.Add(ground);                                                   // Add to the list of entities
            return ground;
        }

        BepuEntity createGround2(Vector3 position, float width, float height, float length)
        {
            ground = new BepuEntity();
            ground.modelName = "cube";                                              // Use the cube model
            ground.LoadContent();
            ground.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            ground.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            ground.diffuse = new Vector3(0.15f, 0.15f, 0.3f);                       // Set the colour to a shade of yellow
            space.Add(ground.body);                                                 // Add to the world
            children.Add(ground);                                                   // Add to the list of entities
            return ground;
        }


        BepuEntity createBall(Vector3 position, float radius)
        {
            ball = new BepuEntity();
            ball.modelName = "sphere";                              // Use the cube model
            ball.LoadContent();
            ball.body = new Sphere(position, radius, 100);          // Place it in the world and give it's dimensions
            //ball.body.AngularVelocity += new Vector3(0, 0, 1);
            ball.localTransform = Matrix.CreateScale(radius);       // Scale the model
            ball.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());           // Set the colour to a shade of yellow
            space.Add(ball.body);                                   // Add to the world
            children.Add(ball);                                     // Add to the list of entities
            return ball;
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
            //space.Add(cameraCylindar);


            seaSaw.createSeaSawBoard(seaSawPos + 7 * Vector3.Up, 40, 1, 25);
            seaSaw.createSeaSawHolder(seaSawPos, 1, 3, 10);
            seaSaw.createSeaSawContainer(seaSawPos + Vector3.Up * 4 + Vector3.Right * 21, 1, 15, 10);
            seaSaw.createSeaSawContainer(seaSawPos + Vector3.Backward * 15, 42, 8, 1);
            seaSaw.createSeaSawContainer(seaSawPos + Vector3.Forward * 15, 42, 8, 1);
            createBall(seaSawPos + Vector3.Right * 5 + Vector3.Up * 15, 10);


            createGround(new Vector3(0, 0, 0), 300, 1, 300);            // Create the ground
            createGround2(new Vector3(100, 100, 0), 20, 1, 20);
            createSlope(new Vector3(200, 50, 0), 150, 1, 300);

            for (int i = 0; i < 30; i++)
            {
                r = (float) random.NextDouble();
                g = (float) random.NextDouble();
                b = (float) random.NextDouble();

                //createBox(new Vector3(random.Next(-150, 150), 4, random.Next(-150, 150)), 5, 5, 5, r, g, b);
            }

            int k = 0;
            for (int j = 1; j < 8; j++)
            {
                k += 5;
                for (int i = 0; i < 1; i++)
                {
                    r = (float)random.NextDouble();
                    g = (float)random.NextDouble();
                    b = (float)random.NextDouble();

                    createBox(new Vector3(85, k, 5 * i), 5, 5, 5, r, g, b);
                    createBox(new Vector3(70, k, 5 * i), 5, 5, 5, r, g, b);
                    createBox(new Vector3(55, k, 5 * i), 5, 5, 5, r, g, b);
                    createBox(new Vector3(40, k, 5 * i), 5, 5, 5, r, g, b);
                }
            }

            //createBall(new Vector3(200,125,20), 10);


            hBase.createHelicopter(heliPos, 4, 4, 4);                   // Create a new helicopter base at the heliPos, with width, length and height 4
            hRotor.createRotor(heliPos, new Vector3(0, 4, 0), 2, .1f, 15);

            hTail.createTail(heliPos, new Vector3(0, 0, 8), 1, 10, 1);

            hSkid.createSkid1(heliPos, new Vector3(3, -5, 0), 1, 1, 10);
            hSkid.createSkid2(heliPos, new Vector3(-3, -5, 0), 1, 1, 10);

            hTailRotor.createTailRotor(heliPos, new Vector3(2, 0, 12), .1f, 1f, 5f);

            hClawHolder.createClawHolder(heliPos, new Vector3(0, -7, 0), 5, 2);

            hClawHinge.createClawHinge1(heliPos, new Vector3(5, -10, 0), 5, .1f, 2);
            hClawHinge.createClawHinge2(heliPos, new Vector3(-5, -10, 0), 5, .1f, 2);
            hClawHinge.createClawHinge3(heliPos, new Vector3(0, -10, 5), 2, .1f, 5);
            hClawHinge.createClawHinge4(heliPos, new Vector3(0, -10, -5), 2, .1f, 5);


            hClawItemHolder.createClawItemHolder1(heliPos, new Vector3(7.5f, -12.6f, 0), .1f, 5, 2);
            hClawItemHolder.createClawItemHolder2(heliPos, new Vector3(-7.5f, -12.6f, 0), .1f, 5, 2);
            hClawItemHolder.createClawItemHolder3(heliPos, new Vector3(0, -12.6f, 7.5f), 2, 5, .1f);
            hClawItemHolder.createClawItemHolder4(heliPos, new Vector3(0, -12.6f, -7.5f), 2, 5, .1f);

            joints = new Joints(helicopterNoise, rideOfTheValkyries, ground, seaSaw, hBase, hRotor, hSkid, hTail, hTailRotor, hClawHolder, hClawHinge, hClawItemHolder);                                 // Add the helicopter base to the joints as part of the constructor
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

            //if (keyState.IsKeyDown(Keys.C) && sLauncher.sphereLauncher.body.LinearVelocity.Y < 10)
            //    sLauncher.sphereLauncher.body.LinearVelocity += Vector3.Up * 4;
            //else if (sLauncher.sphereLauncher.body.LinearVelocity.Y >= 10)
            //{
            //    sLauncher.sphereLauncher.body.LinearVelocity = Vector3.Zero;
            //}

            cameraCylindar.Position = camera.Position;  // Place the cylinder on the camera;

            hBase.Update(gameTime);
            joints.Update(gameTime);

            space.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (GameEntity child in children)
            {
                GraphicsDevice.DepthStencilState = state;
                child.Draw(gameTime);
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
