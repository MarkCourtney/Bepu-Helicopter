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


namespace BepuPhysicsHelicopter
{
    public class HelicopterClawItemHolder : GameEntity
    {
        public BepuEntity clawItemHolder1, clawItemHolder2, clawItemHolder3, clawItemHolder4;

        public BepuEntity createClawItemHolder1(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawItemHolder1 = new BepuEntity();
            clawItemHolder1.modelName = "cube";
            clawItemHolder1.LoadContent();
            clawItemHolder1.body = new Box(position + offSet, width, height, length, 10);
            clawItemHolder1.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawItemHolder1.body);
            Game1.Instance.Children.Add(clawItemHolder1);
            return clawItemHolder1;
        }

        public BepuEntity createClawItemHolder2(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawItemHolder2 = new BepuEntity();
            clawItemHolder2.modelName = "cube";
            clawItemHolder2.LoadContent();
            clawItemHolder2.body = new Box(position + offSet, width, height, length, 10);
            clawItemHolder2.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawItemHolder2.body);
            Game1.Instance.Children.Add(clawItemHolder2);
            return clawItemHolder1;
        }

        public BepuEntity createClawItemHolder3(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawItemHolder3 = new BepuEntity();
            clawItemHolder3.modelName = "cube";
            clawItemHolder3.LoadContent();
            clawItemHolder3.body = new Box(position + offSet, width, height, length, 10);
            clawItemHolder3.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawItemHolder3.body);
            Game1.Instance.Children.Add(clawItemHolder3);
            return clawItemHolder3;
        }

        public BepuEntity createClawItemHolder4(Vector3 position, Vector3 offSet, float width, float height, float length)
        {
            clawItemHolder4 = new BepuEntity();
            clawItemHolder4.modelName = "cube";
            clawItemHolder4.LoadContent();
            clawItemHolder4.body = new Box(position + offSet, width, height, length, 10);
            clawItemHolder4.localTransform = Matrix.CreateScale(width, height, length);
            Game1.Instance.Space.Add(clawItemHolder4.body);
            Game1.Instance.Children.Add(clawItemHolder4);
            return clawItemHolder4;
        }
    }
}
