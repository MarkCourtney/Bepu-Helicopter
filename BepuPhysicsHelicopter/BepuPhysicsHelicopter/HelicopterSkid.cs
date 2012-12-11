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
    public class HelicopterSkid
    {
        public BepuEntity skid1, skid2;

        public BepuEntity createSkid1(Vector3 pos, Vector3 offSet, float width, float height, float length)
        {
            skid1 = new BepuEntity();
            skid1.modelName = "cube";
            skid1.LoadContent();
            skid1.body = new Box(pos + offSet, width, height, length, 1);
            skid1.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(skid1.body);
            Game1.Instance.Children.Add(skid1);
            return skid1;
        }

        public BepuEntity createSkid2(Vector3 pos, Vector3 offSet, float width, float height, float length)
        {
            skid2 = new BepuEntity();
            skid2.modelName = "cube";
            skid2.LoadContent();
            skid2.body = new Box(pos + offSet, width, height, length, 1);
            skid2.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(skid2.body);
            Game1.Instance.Children.Add(skid2);
            return skid1;
        }
    }
}
