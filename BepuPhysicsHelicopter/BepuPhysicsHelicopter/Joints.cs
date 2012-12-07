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
        HelicopterBase hBase;
        HelicopterRotor hRotor;
        HelicopterSkid hSkid;
        HelicopterTail hTail;
        HelicopterTailRotor hTailRotor;
        HelicopterClawHolder hClawHolder;
        HelicopterClawHinge hClawHinge;
        HelicopterClawItemHolder hClawItemHolder;
        SeaSaw seaSaw;
        BepuEntity ground;

        SoundEffect helicopterNoise, rideOfTheValkyries;
        SoundEffectInstance hnInstance, rotvInstance;

        KeyboardState keyState, oldState;
        //GamePad gamePadState, oldPadState;

        RevoluteJoint hBaseRotorJoint, hTailRotorJoint, hClawHingeJoint1, hClawHingeJoint2, hClawHingeJoint3, hClawHingeJoint4;

        SwivelHingeJoint hClawHolderJoint;

        WeldJoint hTailJoint, hSkidJoint1, hSkidJoint2, hClawItemHolderJoint1, hClawItemHolderJoint2, hClawItemHolderJoint3, hClawItemHolderJoint4;

        RevoluteJoint seaSawJoint;


        public bool engineOn;

        public Joints(SoundEffect hn, SoundEffect rotv, BepuEntity g, SeaSaw ss, HelicopterBase hb, HelicopterRotor hr, HelicopterSkid hs, HelicopterTail ht, HelicopterTailRotor htr, HelicopterClawHolder hch, HelicopterClawHinge hchinge, HelicopterClawItemHolder hcih)
        {
            engineOn = false;

            hBase = hb;
            hRotor = hr;
            hSkid = hs;
            hTail = ht;
            hTailRotor = htr;
            hClawHolder = hch;
            hClawHinge = hchinge;
            hClawItemHolder = hcih;
            seaSaw = ss;
            ground = g;


            helicopterNoise = hn;
            hnInstance = helicopterNoise.CreateInstance();
            hnInstance.Volume = 0.6f;
            hnInstance.IsLooped = true;


            rideOfTheValkyries = rotv;
            rotvInstance = rideOfTheValkyries.CreateInstance();
            rotvInstance.Volume = 0.6f;
            rotvInstance.IsLooped = true;

            hBaseRotorJoint = new RevoluteJoint(hBase.helicopter.body, hRotor.rotor.body, hBase.helicopter.body.Position, Vector3.Up);
            hBaseRotorJoint.Motor.Settings.MaximumForce = 200;
            hBaseRotorJoint.Motor.IsActive = false;

            hTailJoint = new WeldJoint(hTail.tail.body, hBase.helicopter.body);
            hSkidJoint1 = new WeldJoint(hSkid.skid1.body, hBase.helicopter.body);
            hSkidJoint2 = new WeldJoint(hSkid.skid2.body, hBase.helicopter.body);

            hTailRotorJoint = new RevoluteJoint(hTailRotor.tailRotor.body, hTail.tail.body, hTailRotor.tailRotor.body.Position, Vector3.Right);
            hTailRotorJoint.Motor.Settings.MaximumForce = 180;
            hTailRotorJoint.Motor.IsActive = false;

            hClawHolderJoint = new SwivelHingeJoint(hBase.helicopter.body, hClawHolder.clawHolder.body, hBase.helicopter.body.Position , Vector3.Right);
           
            hClawHolderJoint.HingeLimit.IsActive = true;
            hClawHolderJoint.HingeLimit.MinimumAngle = -MathHelper.Pi/45;
            hClawHolderJoint.HingeLimit.MaximumAngle = MathHelper.Pi/45;

            hClawHingeJoint1 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge1.body, hClawHolder.clawHolder.body.Position, Vector3.Forward);
            hClawHingeJoint1.Motor.IsActive = true;
            hClawHingeJoint1.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint1.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            hClawHingeJoint1.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint1.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint1.Limit.IsActive = true;
            hClawHingeJoint1.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint1.Limit.MaximumAngle = MathHelper.PiOver4;

            hClawHingeJoint2 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge2.body, hClawHolder.clawHolder.body.Position, Vector3.Backward);
            hClawHingeJoint2.Motor.IsActive = true;
            hClawHingeJoint2.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint2.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            //Weaken the claw to prevent it from crushing the boxes.
            hClawHingeJoint2.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint2.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint2.Limit.IsActive = true;
            hClawHingeJoint2.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint2.Limit.MaximumAngle = MathHelper.PiOver4;

            hClawHingeJoint3 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge3.body, hClawHolder.clawHolder.body.Position, Vector3.Right);
            hClawHingeJoint3.Motor.IsActive = true;
            hClawHingeJoint3.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint3.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            //Weaken the claw to prevent it from crushing the boxes.
            hClawHingeJoint3.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint3.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint3.Limit.IsActive = true;
            hClawHingeJoint3.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint3.Limit.MaximumAngle = MathHelper.PiOver4;


            hClawHingeJoint4 = new RevoluteJoint(hClawHolder.clawHolder.body, hClawHinge.clawHinge4.body, hClawHolder.clawHolder.body.Position, Vector3.Left);
            hClawHingeJoint4.Motor.IsActive = true;
            hClawHingeJoint4.Motor.Settings.Mode = MotorMode.Servomechanism;
            hClawHingeJoint4.Motor.Settings.Servo.Goal = -MathHelper.PiOver2;
            //Weaken the claw to prevent it from crushing the boxes.
            hClawHingeJoint4.Motor.Settings.Servo.SpringSettings.DampingConstant /= 20;
            hClawHingeJoint4.Motor.Settings.Servo.SpringSettings.StiffnessConstant /= 20;

            hClawHingeJoint4.Limit.IsActive = true;
            hClawHingeJoint4.Limit.MinimumAngle = -MathHelper.TwoPi;
            hClawHingeJoint4.Limit.MaximumAngle = MathHelper.PiOver4;

            hClawItemHolderJoint1 = new WeldJoint(hClawHinge.clawHinge1.body, hClawItemHolder.clawItemHolder1.body);
            hClawItemHolderJoint2 = new WeldJoint(hClawHinge.clawHinge2.body, hClawItemHolder.clawItemHolder2.body);
            hClawItemHolderJoint3 = new WeldJoint(hClawHinge.clawHinge3.body, hClawItemHolder.clawItemHolder3.body);
            hClawItemHolderJoint4 = new WeldJoint(hClawHinge.clawHinge4.body, hClawItemHolder.clawItemHolder4.body);

            seaSawJoint = new RevoluteJoint(seaSaw.seaSawHolder.body, seaSaw.seaSawBoard.body, seaSaw.seaSawHolder.body.Position, Vector3.Forward);


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
            Game1.Instance.Space.Add(seaSawJoint);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyState = Keyboard.GetState();
            //gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.U) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.5f)
            {
                hClawHingeJoint1.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint1.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint1.Limit.MinimumAngle);
                hClawHingeJoint2.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint2.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint2.Limit.MinimumAngle);
                hClawHingeJoint3.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint3.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint3.Limit.MinimumAngle);
                hClawHingeJoint4.Motor.Settings.Servo.Goal = MathHelper.Max(hClawHingeJoint4.Motor.Settings.Servo.Goal + 3f * timeDelta, hClawHingeJoint4.Limit.MinimumAngle);
            }
            else if (keyState.IsKeyDown(Keys.Y) || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.5f)
            {
                hClawHingeJoint1.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint1.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint1.Limit.MinimumAngle);
                hClawHingeJoint2.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint2.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint2.Limit.MinimumAngle);
                hClawHingeJoint3.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint3.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint3.Limit.MinimumAngle);
                hClawHingeJoint4.Motor.Settings.Servo.Goal = MathHelper.Min(hClawHingeJoint4.Motor.Settings.Servo.Goal + 1.5f * timeDelta, hClawHingeJoint4.Limit.MinimumAngle);
            }

            if (keyState.IsKeyDown(Keys.P) && !oldState.Equals(keyState) || GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)   // Ensure you can't press P twice in a row
            {
                engineOn = true;            // Turn the engine on  

                hnInstance.Play();
                rotvInstance.Play();

                hBaseRotorJoint.Motor.IsActive = true;  // Start the motor
                hTailRotorJoint.Motor.IsActive = true;

                oldState = keyState;
            }

            if (keyState.IsKeyDown(Keys.O) && !oldState.Equals(keyState) || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)   // Ensure you can't press O twice in a row
            {
                engineOn = false;           // Turn the engine off

                hnInstance.Pause();
                rotvInstance.Pause();

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

