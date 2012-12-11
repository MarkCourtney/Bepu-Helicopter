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
    public class HelicopterTailRotor
    {
        public BepuEntity tailRotor;

        public BepuEntity createTailRotor(Vector3 pos, Vector3 offSet, float width, float height, float length)
        {
            tailRotor = new BepuEntity();
            tailRotor.modelName = "cube";
            tailRotor.LoadContent();
            tailRotor.body = new Box(pos + offSet, width, height, length, 1);
            tailRotor.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(tailRotor.body);
            Game1.Instance.Children.Add(tailRotor);
            return tailRotor;
        }
    }
}
