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
using BEPUphysics.MathExtensions;
using BEPUphysics.Constraints.TwoEntity.Joints;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.DataStructures;


namespace BepuPhysicsHelicopter
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HelicopterBase : GameEntity
    {
        Random random = new Random();
        public BepuEntity helicopter;
        KeyboardState keyState;
        float td, rotation;
        Vector3 force;

        public BepuEntity createHelicopter(Vector3 position, float width, float height, float length)
        {
            helicopter = new BepuEntity();
            helicopter.modelName = "cube";
            helicopter.LoadContent();
            helicopter.body = new Box(position, width, height, length, 1);
            helicopter.localTransform = Matrix.CreateScale(width, height, length);
            helicopter.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            helicopter.Look = new Vector3(0, 0, 0);
            helicopter.body.BecomeKinematic();
            Game1.Instance.Space.Add(helicopter.body);
            Game1.Instance.Children.Add(helicopter);
            return helicopter;
        }

        public void applyForce(Vector3 appliedForce)
        {
            force += appliedForce;
        }

        public override void Update(GameTime gameTime)
        {
            td = (float)gameTime.ElapsedGameTime.TotalSeconds;

            keyState = Keyboard.GetState();

            helicopter.Look.X = (float)Math.Sin(rotation);
            helicopter.Look.Y = 0;
            helicopter.Look.Z = (float)Math.Cos(rotation);

            helicopter.body.Orientation = Quaternion.CreateFromYawPitchRoll(rotation, 0, 0);    // Change the helicopters base orientation X axis based on the rotation


            // For all keyStates the engine must be on
            if (keyState.IsKeyDown(Keys.K) && Game1.Instance.Joints.engineOn)       
            {
                applyForce(helicopter.Look * 10);      // Apply a diretional force in the direction the helicopter is looking
            }
            else if (keyState.IsKeyDown(Keys.I) && Game1.Instance.Joints.engineOn)
            {
                applyForce(helicopter.Look * -10);
            }
            else
            {
                velocity -= velocity * td;          // Slow down the velocity
            }

            if (keyState.IsKeyDown(Keys.L) && Game1.Instance.Joints.engineOn)
            {
                rotation -= td;
            }
            else if (keyState.IsKeyDown(Keys.J) && Game1.Instance.Joints.engineOn)
            {
                rotation += td;                     // Rotate the helicopter
            }

            if (keyState.IsKeyDown(Keys.Space) && Game1.Instance.Joints.engineOn)
            {
                applyForce(Up * 5);                    // Increase the vertical velocity
            }
            else if (keyState.IsKeyDown(Keys.C) && Game1.Instance.Joints.engineOn)
            {
                applyForce(-Up * 5);
            }
            else if (!keyState.IsKeyDown(Keys.Space) && Game1.Instance.Joints.engineOn)
            {
                velocity -= velocity * td;
            }

            velocity = velocity + force * td;
            helicopter.body.Position = helicopter.body.Position + velocity * td;

            force = Vector3.Zero;
        }
    }
}
