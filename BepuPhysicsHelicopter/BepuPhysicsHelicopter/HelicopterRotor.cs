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
    public class HelicopterRotor : GameEntity
    {
        public BepuEntity rotor;

        public BepuEntity createRotor(Vector3 pos, Vector3 offSet, float width, float height, float length)
        {
            rotor = new BepuEntity();
            rotor.modelName = "cube";
            rotor.LoadContent();
            rotor.body = new Box(pos + offSet, width, height, length, 1);
            rotor.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(rotor.body);
            Game1.Instance.Children.Add(rotor);
            return rotor;
        }
    }
}