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
        float timeDelta, rotation;
        Vector3 force;

        public BepuEntity createHelicopter(Vector3 position, float width, float height, float length)
        {
            helicopter = new BepuEntity();
            helicopter.modelName = "cube";
            helicopter.LoadContent();
            helicopter.body = new Box(position, width, height, length, 1);
            helicopter.localTransform = Matrix.CreateScale(width, height, length);
            helicopter.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            helicopter.Look = new Vector3(1, 0, 0);
            helicopter.body.BecomeKinematic();
            Game1.Instance.Space.Add(helicopter.body);
            Game1.Instance.Children.Add(helicopter);
            return helicopter;
        }

        public void applyForce(Vector3 appliedForce)        // Method for applying force to the helicopter base
        {
            force += appliedForce;
        }

        public override void Update(GameTime gameTime)
        {
            timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            keyState = Keyboard.GetState();

            helicopter.Look.X = (float)Math.Sin(rotation);
            helicopter.Look.Y = 0;
            helicopter.Look.Z = (float)Math.Cos(rotation);

            helicopter.body.Orientation = Quaternion.CreateFromYawPitchRoll(rotation, 0, 0);    // Change the helicopters base orientation X axis based on the rotation

            // For all keyStates the engine must be on
            if (keyState.IsKeyDown(Keys.K) && Game1.Instance.Joints.engineOn || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.6f)      
            {
                applyForce(helicopter.Look * 10);       // Apply a diretional force in the look direction
            }
            else if (keyState.IsKeyDown(Keys.I) && Game1.Instance.Joints.engineOn || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.6f)
            {
                applyForce(helicopter.Look * -10);      // Apply a diretional force in oppisite the look direction
            }
            else
            {
                velocity -= velocity * timeDelta;       // Slow down the velocity
            }
           

            if ((keyState.IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(Vector3.Up * 10);            // Increase the vertical velocity
            }
            else if ((keyState.IsKeyDown(Keys.C) || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.5f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(Vector3.Down * 10);          // Decrease the vertical velocity
            }
            else
            {
                velocity -= velocity * timeDelta;       // Slow down the velocity
            }

            if ((keyState.IsKeyDown(Keys.L) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0.6f) && Game1.Instance.Joints.engineOn)
            {
                rotation -= timeDelta;
            }
            else if ((keyState.IsKeyDown(Keys.J) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < -0.6f) && Game1.Instance.Joints.engineOn)
            {
                rotation += timeDelta;                         // Rotate the helicopter
            }

            if (!Game1.Instance.Joints.engineOn)
            {
                if (helicopter.body.Position.Y > 20)
                {
                    applyForce(Vector3.Down * 20);             // Keep the helicopter above a certain level
                }
                else
                {
                    applyForce(Vector3.Down * 4);              // When the engine is off decrease the vertical height of the helicopter
                }
            }

            if (helicopter.body.Position.Y < 12)
            {
                applyForce(Vector3.Up * 4);                     // Keep the helicopter above a certain level
            }

            velocity = velocity + force * timeDelta;
            helicopter.body.Position = helicopter.body.Position + velocity * timeDelta;       // Apply the forces to the helicopter body position

            force = Vector3.Zero;   // Return the force to 0 each update loop
        }
    }
}
