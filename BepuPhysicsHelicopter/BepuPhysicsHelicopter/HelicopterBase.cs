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
using BEPUphysics.Entities.Prefabs;


namespace BepuPhysicsHelicopter
{
    public class HelicopterBase : GameEntity
    {
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
            helicopter.diffuse = new Vector3(0.7f, 0.2f, 0.2f);
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

            // Ensures the helicopter doesn;t move too fast for the joints
            if (velocity.X > 18)
            {
                velocity.X = 18;
            }
            if (velocity.Y > 18)
            {
                velocity.Y = 18;
            }
            if (velocity.Z > 18)
            {
                velocity.Z = 18;
            }

            // For all keyStates the engine must be on
            if ((keyState.IsKeyDown(Keys.K) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.6f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(helicopter.Look * 20);       // Apply a diretional force in the look direction
            }
            else if ((keyState.IsKeyDown(Keys.I) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.6f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(helicopter.Look * -20);      // Apply a diretional force in oppisite the look direction
            }
            else
            {
                velocity -= velocity * timeDelta;       // Slow down the velocity
            }

            if ((keyState.IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(Vector3.Up * 20);            // Increase the vertical velocity
            }
            else if ((keyState.IsKeyDown(Keys.C) || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.5f) && Game1.Instance.Joints.engineOn)
            {
                applyForce(Vector3.Down * 20);          // Decrease the vertical velocity
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

            Console.WriteLine(velocity);

            force = Vector3.Zero;   // Return the force to 0 each update loop
        }
    }
}