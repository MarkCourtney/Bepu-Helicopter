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
        SpriteBatch spriteBatch;
        List<GameEntity> children;
        GraphicsDeviceManager graphics;
        KeyboardState keyState;
        SoundEffect helicopterNoise, rideOfTheValkyries;
        Random random;

        // New Vector position
        Vector3 heliPos = new Vector3(-50, 15, -10);
        Vector3 groundPos = new Vector3(0, 0, 0);
        Vector3 seeSawPos = new Vector3(250, 140, 0);

        // New BepuEntity objects
        Ground groundEntity;
        OtherEntities otherEntities;
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
        seeSaw seeSaw;

        Space space;
        Camera camera;

        Joints joints;

        // floats for red, green, blue vales for diffuse and timeDelta
        float r, g, b, timeDelta;



        public static Game1 Instance
        {
            get { return Game1.instance; }
            set { Game1.instance = value; }
        }

        public Joints Joints
        {
            get { return joints; }
            set { joints = value; }
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

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
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

            children = new List<GameEntity>();

            children.Add(camera);       // Add the camera to the list of children

            random = new Random();

            groundEntity = new Ground();
            otherEntities = new OtherEntities();
            hBase = new HelicopterBase();
            hRotor = new HelicopterRotor();
            hSkid = new HelicopterSkid();
            hTail = new HelicopterTail();
            hTailRotor = new HelicopterTailRotor();
            hClawHolder = new HelicopterClawHolder();
            hClawHinge = new HelicopterClawHinge();
            hClawItemHolder = new HelicopterClawItemHolder();
            seeSaw = new seeSaw();
            helicopterNoise = Content.Load<SoundEffect>("Helicopter Sound Effect");
            rideOfTheValkyries = Content.Load<SoundEffect>("ROTV");
            
            base.Initialize();
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


            // Create the entities that make up the seesaw
            seeSaw.createSeeSawBoard(seeSawPos + 7 * Vector3.Up, 40, 1, 25);
            seeSaw.createSeeSawHolder(seeSawPos, 1, 3, 10);
            seeSaw.createSeeSawContainer(seeSawPos + Vector3.Up * 4 + Vector3.Right * 21, 1, 15, 10);
            seeSaw.createSeeSawContainer(seeSawPos + Vector3.Backward * 15, 42, 8, 1);
            seeSaw.createSeeSawContainer(seeSawPos + Vector3.Forward * 15, 42, 8, 1);
            otherEntities.createBall(seeSawPos + Vector3.Right * 5 + Vector3.Up * 15, 10);

            // Create all the ground entities
            groundEntity.createGround(new Vector3(0, 0, 0), 300, 1, 300);
            groundEntity.createSlope(new Vector3(200, 50, 0), 150, 1, 300);

            // Create all the boxes located in the world with different masses
            for (int i = 0; i < 30; i++)
            {
                r = (float) random.NextDouble();
                g = (float) random.NextDouble();
                b = (float) random.NextDouble();

                otherEntities.createBox(new Vector3(random.Next(-150, -50), 4, random.Next(-150, 150)), 5, 5, 5, r, g, b, 100);
            }

            int k = 0;
            for (int j = 1; j < 8; j++)
            {
                k += 5;
                for (int i = 0; i < 8; i++)
                {
                    r = (float)random.NextDouble();
                    g = (float)random.NextDouble();
                    b = (float)random.NextDouble();

                    otherEntities.createBox(new Vector3(45, k, -30 + 10 * i), 5, 5, 10, r, g, b, 1);
                    otherEntities.createBox(new Vector3(30, k, -30 + 10 * i), 5, 5, 10, r, g, b, 1);
                    otherEntities.createBox(new Vector3(15, k, -30 + 10 * i), 5, 5, 10, r, g, b, 1);
                    otherEntities.createBox(new Vector3(0, k, -30 + 10 * i), 5, 5, 10, r, g, b, 1);
                }
            }


            // Create the entities that make up the helicopter and claw, defining the position, length, width and height of all entities
            hBase.createHelicopter(heliPos, 4, 4, 4);

            hRotor.createRotor(heliPos, new Vector3(0, 5, 0), 2, .1f, 15);

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

            // Calls the joints class and creates all the joints
            joints = new Joints(helicopterNoise, rideOfTheValkyries, seeSaw, hBase, hRotor, hSkid, hTail, hTailRotor, hClawHolder, hClawHinge, hClawItemHolder);                                 // Add the helicopter base to the joints as part of the constructor
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resetWorld();                                       // Reset the world to it's original scene

            foreach (GameEntity child in children)
            {
                child.LoadContent();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyState = Keyboard.GetState();

            camera.Position = hBase.helicopter.body.Position;

            // Set the view of the camera position and target
            // Set the position of the camera to be the center minus the look vector * 60
            // This rotates the camera around the helicopter base
            camera.setView(-hBase.helicopter.Look * 60 + new Vector3(4, -30, 0), hBase.helicopter.body.Position);

            
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

            otherEntities.Update(gameTime);
            hBase.Update(gameTime);         // Update the helicopter base
            joints.Update(gameTime);        // Update the joints
            otherEntities.Update(gameTime);

            space.Update();                 // Update the space

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

            spriteBatch.End();
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
