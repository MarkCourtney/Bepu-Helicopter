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
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Constraints.TwoEntity.Motors;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Materials;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;


namespace BepuPhysicsHelicopter
{
    public class Joints : GameEntity
    {
        // New objects
        HelicopterBase hBase;
        HelicopterRotor hRotor;
        HelicopterSkid hSkid;
        HelicopterTail hTail;
        HelicopterTailRotor hTailRotor;
        HelicopterClawHolder hClawHolder;
        HelicopterClawHinge hClawHinge;
        HelicopterClawItemHolder hClawItemHolder;
        seeSaw seeSaw;

        SoundEffect helicopterNoise, rideOfTheValkyries;
        SoundEffectInstance hnInstance, rotvInstance;

        KeyboardState keyState, oldState;

        // New Joints
        RevoluteJoint hBaseRotorJoint, hTailRotorJoint, hClawHingeJoint1, hClawHingeJoint2, hClawHingeJoint3, hClawHingeJoint4, seeSawJoint;

        SwivelHingeJoint hClawHolderJoint;

        WeldJoint hTailJoint, hSkidJoint1, hSkidJoint2, hClawItemHolderJoint1, hClawItemHolderJoint2, hClawItemHolderJoint3, hClawItemHolderJoint4;


        public bool engineOn;

        public Joints(SoundEffect hn, SoundEffect rotv, seeSaw ss, HelicopterBase hb, HelicopterRotor hr, HelicopterSkid hs, HelicopterTail ht, HelicopterTailRotor htr, HelicopterClawHolder hch, HelicopterClawHinge hchinge, HelicopterClawItemHolder hcih)
        {
            engineOn = false;       // Set the engine to off

            hBase = hb;
            hRotor = hr;
            hSkid = hs;
            hTail = ht;
            hTailRotor = htr;
            hClawHolder = hch;
            hClawHinge = hchinge;
            hClawItemHolder = hcih;
            seeSaw = ss;


            helicopterNoise = hn;
            hnInstance = helicopterNoise.CreateInstance();          // New instance of sound effect
            hnInstance.Volume = 0.6f;
            hnInstance.IsLooped = true;


            rideOfTheValkyries = rotv;
            rotvInstance = rideOfTheValkyries.CreateInstance();     // New instance of sound effect
            rotvInstance.Volume = 0.6f;
            rotvInstance.IsLooped = true;

            hBaseRotorJoint = new RevoluteJoint(hBase.helicopter.body, hRotor.rotor.body, hBase.helicopter.body.Position, Vector3.Up);  // Joint for rotor on top of helicopter
            hBaseRotorJoint.Motor.Settings.MaximumForce = 200;
            hBaseRotorJoint.Motor.IsActive = false;

            // Well joints for skids on bottom of helicopter and tail, attach each entity to the helicopter
            hTailJoint = new WeldJoint(hTail.tail.body, hBase.helicopter.body);     
            hSkidJoint1 = new WeldJoint(hSkid.skid1.body, hBase.helicopter.body);
            hSkidJoint2 = new WeldJoint(hSkid.skid2.body, hBase.helicopter.body);

            hTailRotorJoint = new RevoluteJoint(hTailRotor.tailRotor.body, hTail.tail.body, hTailRotor.tailRotor.body.Position, Vector3.Right); // Joint for rotor on tail of the helicopter
            hTailRotorJoint.Motor.Settings.MaximumForce = 180;
            hTailRotorJoint.Motor.IsActive = false;

            hClawHolderJoint = new SwivelHingeJoint(hBase.helicopter.body, hClawHolder.clawHolder.body, hBase.helicopter.body.Position, Vector3.Right);    // Joint for claw holder below helicopter
           
            hClawHolderJoint.HingeLimit.IsActive = true;
            hClawHolderJoint.HingeLimit.MinimumAngle = -MathHelper.Pi/45;
            hClawHolderJoint.HingeLimit.MaximumAngle = MathHelper.Pi/45;

            // 4 similar joints for hinge joints between the claw holder and a part of the claw
            hClawHingeJoint1 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge1.body, hClawHolder.clawHolder.body.Position, Vector3.Forward);
            hClawHingeJoint1.Motor.IsActive = true;
            hClawHingeJoint1.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint1.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            //hClawHingeJoint1.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;   // Damping to quicken the claw contracting 
            hClawHingeJoint1.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;   // Stop the claw from over moving on the axis

            hClawHingeJoint1.Limit.IsActive = true;
            hClawHingeJoint1.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint1.Limit.MaximumAngle = MathHelper.PiOver4;

            hClawHingeJoint2 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge2.body, hClawHolder.clawHolder.body.Position, Vector3.Backward);
            hClawHingeJoint2.Motor.IsActive = true;
            hClawHingeJoint2.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint2.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            hClawHingeJoint2.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint2.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint2.Limit.IsActive = true;
            hClawHingeJoint2.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint2.Limit.MaximumAngle = MathHelper.PiOver4;

            hClawHingeJoint3 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge3.body, hClawHolder.clawHolder.body.Position, Vector3.Right);
            hClawHingeJoint3.Motor.IsActive = true;
            hClawHingeJoint3.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint3.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            hClawHingeJoint3.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint3.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint3.Limit.IsActive = true;
            hClawHingeJoint3.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint3.Limit.MaximumAngle = MathHelper.PiOver4;


            hClawHingeJoint4 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge4.body, hClawHolder.clawHolder.body.Position, Vector3.Left);
            hClawHingeJoint4.Motor.IsActive = true;
            hClawHingeJoint4.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint4.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            hClawHingeJoint4.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint4.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint4.Limit.IsActive = true;
            hClawHingeJoint4.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint4.Limit.MaximumAngle = MathHelper.PiOver4;

            // More weld joints as the claw pieces are made up off two different sections
            hClawItemHolderJoint1 = new WeldJoint(hClawHinge.clawHinge1.body, hClawItemHolder.clawItemHolder1.body);
            hClawItemHolderJoint2 = new WeldJoint(hClawHinge.clawHinge2.body, hClawItemHolder.clawItemHolder2.body);
            hClawItemHolderJoint3 = new WeldJoint(hClawHinge.clawHinge3.body, hClawItemHolder.clawItemHolder3.body);
            hClawItemHolderJoint4 = new WeldJoint(hClawHinge.clawHinge4.body, hClawItemHolder.clawItemHolder4.body);

            // Joint for the seesaw with the ball located on it
            seeSawJoint = new RevoluteJoint(seeSaw.seeSawHolder.body, seeSaw.seeSawBoard.body, seeSaw.seeSawHolder.body.Position, Vector3.Forward);

            // Add all the joints to Space
            Game1.Instance.Space.Add(hBaseRotorJoint);
            Game1.Instance.Space.Add(hTailJoint);
            Game1.Instance.Space.Add(hSkidJoint1);
            Game1.Instance.Space.Add(hSkidJoint2);
            Game1.Instance.Space.Add(hTailRotorJoint);
            Game1.Instance.Space.Add(hClawHolderJoint);
            Game1.Instance.Space.Add(hClawHingeJoint1);
            Game1.Instance.Space.Add(hClawHingeJoint2);
            Game1.Instance.Space.Add(hClawHingeJoint3);
            Game1.Instance.Space.Add(hClawHingeJoint4);
            Game1.Instance.Space.Add(hClawItemHolderJoint1);
            Game1.Instance.Space.Add(hClawItemHolderJoint2);
            Game1.Instance.Space.Add(hClawItemHolderJoint3);
            Game1.Instance.Space.Add(hClawItemHolderJoint4);
            Game1.Instance.Space.Add(seeSawJoint);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyState = Keyboard.GetState();


            // Applied keyboard and XBox Controller inputs
            if (keyState.IsKeyDown(Keys.U) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                // Contract the claw to pick up objects
                hClawHingeJoint1.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint1.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint1.Limit.MinimumAngle);
                hClawHingeJoint2.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint2.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint2.Limit.MinimumAngle);
                hClawHingeJoint3.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint3.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint3.Limit.MinimumAngle);
                hClawHingeJoint4.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint4.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint4.Limit.MinimumAngle);
            }
            else if (keyState.IsKeyDown(Keys.Y) || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                // Expand the claw to let go off objects
                hClawHingeJoint1.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint1.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint1.Limit.MinimumAngle);
                hClawHingeJoint2.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint2.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint2.Limit.MinimumAngle);
                hClawHingeJoint3.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint3.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint3.Limit.MinimumAngle);
                hClawHingeJoint4.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint4.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint4.Limit.MinimumAngle);
            }

            if (keyState.IsKeyDown(Keys.P) && !oldState.Equals(keyState) || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)   // Ensure you can't press P twice in a row
            {
                engineOn = true;            // Turn the engine on  

                hnInstance.Play();          // Play the sound effects
                rotvInstance.Play();        

                hBaseRotorJoint.Motor.IsActive = true;  // Start the both rotors
                hTailRotorJoint.Motor.IsActive = true;  

                oldState = keyState;
            }

            if (keyState.IsKeyDown(Keys.O) && !oldState.Equals(keyState) || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)   // Ensure you can't press O twice in a row
            {
                engineOn = false;           // Turn the engine off

                hnInstance.Pause();         // Pause the sound effects
                rotvInstance.Pause();

                hBaseRotorJoint.Motor.IsActive = false; // Stop the rotors
                hTailRotorJoint.Motor.IsActive = false;

                oldState = keyState;
            }


            if (engineOn)
            {
                hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity += 0.05f;     // Increase the rotation of the rotor around the center of the helicopter base
                hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity += 0.045f;    // Increase the rotation of the rotor on the tail
            }
            else if (!engineOn)
            {
                hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity -= 0.05f;     // Decrease the rotation speed of both motors
                hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity -= 0.045f;

                if (hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity < 0)      // When the rotors have stopped don't spin in the oppisite direction
                {
                    hBaseRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity = 0;
                    hTailRotorJoint.Motor.Settings.VelocityMotor.GoalVelocity = 0;
                }
            }
        }

    }
}

