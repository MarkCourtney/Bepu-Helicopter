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
using BEPUphysics.Constraints.SolverGroups;


namespace BepuPhysicsHelicopter
{
    public class Joints : GameEntity
    {
        HelicopterBase hBase;
        HelicopterRotor hRotor;
        HelicopterSkid hSkid;
        HelicopterTail hTail;
        HelicopterTailRotor hTailRotor;

        KeyboardState keyState, oldState;

        RevoluteJoint hBaseRotorJoint, hTailRotorJoint;

        WeldJoint hTailJoint, hSkidJoint1, hSkidJoint2;

        public bool engineOn;

        public Joints(HelicopterBase hb, HelicopterRotor hr, HelicopterSkid hs, HelicopterTail ht, HelicopterTailRotor htr)
        {
            engineOn = false;

            hBase = hb;
            hRotor = hr;
            hSkid = hs;
            hTail = ht;
            hTailRotor = htr;

            hBaseRotorJoint = new RevoluteJoint(hBase.helicopter.body, hRotor.rotor.body, hBase.helicopter.body.Position, Vector3.Up);
            hBaseRotorJoint.Motor.Settings.MaximumForce = 200;
            hBaseRotorJoint.Motor.IsActive = false;

            hTailJoint = new WeldJoint(hTail.tail.body, hBase.helicopter.body);
            hSkidJoint1 = new WeldJoint(hSkid.skid1.body, hBase.helicopter.body);
            hSkidJoint2 = new WeldJoint(hSkid.skid2.body, hBase.helicopter.body);

            hTailRotorJoint = new RevoluteJoint(hTailRotor.tailRotor.body, hTail.tail.body, hTailRotor.tailRotor.body.Position, Vector3.Right);
            hTailRotorJoint.Motor.Settings.MaximumForce = 180;
            hTailRotorJoint.Motor.IsActive = false;

            Game1.Instance.Space.Add(hBaseRotorJoint);
            Game1.Instance.Space.Add(hTailJoint);
            Game1.Instance.Space.Add(hSkidJoint1);
            Game1.Instance.Space.Add(hSkidJoint2);
            Game1.Instance.Space.Add(hTailRotorJoint);
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.P) && !oldState.Equals(keyState))   // Ensure you can't press P twice in a row
            {
                engineOn = true;            // Turn the engine on  

                hBaseRotorJoint.Motor.IsActive = true;  // Start the motor
                hTailRotorJoint.Motor.IsActive = true;

                oldState = keyState;
            }

            if (keyState.IsKeyDown(Keys.O) && !oldState.Equals(keyState))   // Ensure you can't press O twice in a row
            {
                engineOn = false;           // Turn the engine off

                hBaseRotorJoint.Motor.IsActive = false; // Stop the motor
                hTailRotorJoint.Motor.IsActive = false;

                oldState = keyState;
            }

            if (engineOn)
            {
                hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity += 0.05f;     // Increase the rotation of the rotor around the center of the helicopter base
                hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity += 0.045f;     // Increase the rotation of the rotor around the center of the helicopter base
            }
            else if (!engineOn)
            {
                hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity -= 0.05f;     // Decrease the rotation 
                hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity -= 0.045f;

                if (hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity < 0)
                {
                    hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity = 0;
                    hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity = 0;
                }
            }
        }

    }
}

