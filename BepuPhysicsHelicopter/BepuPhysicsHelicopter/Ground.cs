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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Ground
    {
        public BepuEntity slope, ground;

        public BepuEntity createSlope(Vector3 position, float width, float height, float length)
        {
            slope = new BepuEntity();
            slope.modelName = "cube";                                              // Use the cube model
            slope.LoadContent();
            slope.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            slope.body.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 0.7f), 1);
            slope.body.BecomeKinematic();                                          // Make the entity state
            slope.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            slope.diffuse = new Vector3(0.3f, 0.3f, 0.45f);                        // Set the colour to a shade of yellow
            Game1.Instance.Space.Add(slope.body);                                                 // Add to the world
            Game1.Instance.Children.Add(slope);                                                   // Add to the list of entities
            return slope;
        }


        public BepuEntity createGround(Vector3 position, float width, float height, float length)
        {
            ground = new BepuEntity();
            ground.modelName = "cube";                                              // Use the cube model
            ground.LoadContent();
            ground.body = new Box(position, width, height, length);                 // Place it in the world and give it's dimensions
            ground.body.BecomeKinematic();                                          // Make the entity state
            ground.localTransform = Matrix.CreateScale(width, height, length);      // Scale the model
            ground.diffuse = new Vector3(0.15f, 0.15f, 0.3f);                       // Set the colour to a shade of yellow
            Game1.Instance.Space.Add(ground.body);                                                 // Add to the world
            Game1.Instance.Children.Add(ground);                                                   // Add to the list of entities
            return ground;
        }
    }
}
