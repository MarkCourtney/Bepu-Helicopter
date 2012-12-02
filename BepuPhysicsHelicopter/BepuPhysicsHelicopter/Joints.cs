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

        KeyboardState keyState, oldState;

        public bool engineOn;

        public Joints(HelicopterBase hb)
        {
            engineOn = false;

            hBase = hb;
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.P) && !oldState.Equals(keyState))   // Ensure you can't press P twice in a row
            {
                engineOn = true;            // Turn the engine on  

                oldState = keyState;
            }

            if (keyState.IsKeyDown(Keys.O) && !oldState.Equals(keyState))   // Ensure you can't press O twice in a row
            {
                engineOn = false;           // Turn the engine off

                oldState = keyState;
            }
        }

    }
}

