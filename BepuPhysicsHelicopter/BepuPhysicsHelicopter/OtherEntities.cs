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
    public class OtherEntities
    {
        public BepuEntity box, ball;

        Random random = new Random();

        public BepuEntity createBox(Vector3 position, float width, float height, float length, float r, float g, float b, int mass)
        {
            box = new BepuEntity();
            box.modelName = "cube";                                              // Use the cube model
            box.LoadContent();
            box.body = new Box(position, width, height, length, mass);           // Place it in the world and give it's dimensions
            box.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            box.diffuse = new Vector3(r, g, b);                                  // Set the colour to a shade of yellow
            Game1.Instance.Space.Add(box.body);                                  // Add to the world
            Game1.Instance.Children.Add(box);                                    // Add to the list of entities
            return box;
        }


        public BepuEntity createBall(Vector3 position, float radius)
        {
            ball = new BepuEntity();
            ball.modelName = "sphere";                              // Use the cube model
            ball.LoadContent();
            ball.body = new Sphere(position, radius, 100);          // Place it in the world and give it's dimensions
            ball.localTransform = Matrix.CreateScale(radius);       // Scale the model
            ball.diffuse = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());           // Set the colour to a shade of yellow
            Game1.Instance.Space.Add(ball.body);                    // Add to the world
            Game1.Instance.Children.Add(ball);                      // Add to the list of entities
            return ball;
        }

        public void Update(GameTime gameTime)
        {
            if (ball.body.Position.X < 150)                         // Ball picks up speed when it get to 100 on the X   
            {                                                       // Used to hit the boxes with more power
                ball.body.AngularVelocity = new Vector3(0, 0, 2.35f);
            }
        }
    }
}
