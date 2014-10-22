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

namespace BadLuckSlobber
{
    class Entity
    {
        List<Vector3> entityPositions;
        public List<Model> entityModels;
        int[] entityQuantities;
        public BoundingBox[] slimeBoxes;

        public void LoadEntity(List<Vector3> positions, List<Model> models, int[] quantities)
        {
            entityPositions = positions;
            entityModels = models;
            entityQuantities = quantities;
            setUpBoundingBoxes();
        }

        public void DrawEntity(Matrix world, Matrix view, Matrix proj)
        {
            foreach (Model model in entityModels)
            {
                for (int number = 0; number < entityQuantities.Length; number++)
                {
                    for (int i = 0; i < entityQuantities[number]; i++)
                    {
                        world = Matrix.CreateScale(0.005f) * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                Matrix.CreateTranslation(entityPositions[i]);

                        foreach (ModelMesh mesh in model.Meshes)
                        {
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.Projection = proj;
                                effect.View = view;
                                effect.World = world;
                            }
                            mesh.Draw();
                        }
                    }
                }
            }
        }

        public void setUpBoundingBoxes()
        {
            List<BoundingBox> slimeList = new List<BoundingBox>();

            for (int i = 0; i < entityQuantities.Length; i++)
            {
                Vector3[] slimePoints = new Vector3[2];
                slimePoints[0] = new Vector3(entityPositions[i].X - 0.2f, 0.01f, entityPositions[i].Z - 0.08f);
                slimePoints[1] = new Vector3(entityPositions[i].X + 0.2f, entityPositions[i].Y + 0.01f, entityPositions[i].Z + 0.08f);
                BoundingBox slimeBox = BoundingBox.CreateFromPoints(slimePoints);
                slimeList.Add(slimeBox);
            }
            slimeBoxes = slimeList.ToArray();
        }
    }
}
