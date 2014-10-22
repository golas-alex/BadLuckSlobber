using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BadLuckSlobber
{
    class Level
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        BasicEffect effect;

        Texture2D wallTexture;
        Texture2D floorTexture;
 
        VertexBuffer floorVertexBuffer;
        VertexBuffer wallVertexBuffer;

        public int[,] floorPlan;
        public int wallHeight = 1;

        public void LoadContent(ContentManager Content, GraphicsDeviceManager graph)
        {
            graphics = graph;
            device = graphics.GraphicsDevice;
            effect = new BasicEffect(device);

            wallTexture = Content.Load<Texture2D>("Textures/WalltextureDark");
            floorTexture = Content.Load<Texture2D>("Textures/FloorTextureDark");

            LoadFloorPlan();
            SetUpVertices();
        }

        private void LoadFloorPlan()
        {
            floorPlan = new int[,]
             {
                 {1,1,1,1,1,1,1,1,1,1},
                 {1,0,0,0,1,0,0,0,0,1},
                 {1,0,0,0,1,0,0,0,0,1},
                 {1,0,1,1,0,1,0,0,0,1},
                 {1,0,1,0,0,0,1,0,1,1},
                 {1,0,1,0,0,0,1,0,1,1},
                 {1,0,0,0,0,0,1,0,1,1},
                 {1,0,1,0,1,1,1,0,1,1},
                 {1,0,1,0,0,0,0,0,1,1},
                 {1,1,1,1,1,1,1,1,1,1},
               
             };
        }

        private void SetUpVertices()
        {
            int levelWidth = floorPlan.GetLength(0);
            int levelLength = floorPlan.GetLength(1);
            device = graphics.GraphicsDevice;

            List<VertexPositionNormalTexture> verticesListFloor = new List<VertexPositionNormalTexture>();
            List<VertexPositionNormalTexture> verticesListWall = new List<VertexPositionNormalTexture>();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int z = 0; z < levelLength; z++)
                {
                    int currentTile = floorPlan[x, z];
                    //floor
                    if (floorPlan[x,z] == 0)
                    {
                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 1, 0), new Vector2(currentTile, 0)));
                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z -1), new Vector3(0, 1, 0), new Vector2(currentTile, 1)));
                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x+1, 0, -z), new Vector3(0, 1, 0), new Vector2(currentTile + 1, 0)));

                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z-1), new Vector3(0, 1, 0), new Vector2(currentTile, 1)));
                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x+1, 0, -z-1), new Vector3(0, 1, 0), new Vector2(currentTile + 1, 1)));
                    verticesListFloor.Add(new VertexPositionNormalTexture(new Vector3(x+1, 0, -z), new Vector3(0, 1, 0), new Vector2(currentTile + 1, 0)));
                    }

                    //wall
                    if (floorPlan[x, z] == 1)
                    {   //right Wall
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, 1), new Vector2(currentTile + 1, 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z - 1), new Vector3(0, 0, -1), new Vector2((currentTile), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentTile), 1)));

                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z - 1), new Vector3(0, 0, -1), new Vector2((currentTile), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentTile + 1), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, wallHeight, -z - 1), new Vector3(0, 0, -1), new Vector2((currentTile + 1), 0)));


                        //front wall
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentTile * 2), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, wallHeight, -z - 1), new Vector3(1, 0, 0), new Vector2((currentTile * 2 - 1), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(1, 0, 0), new Vector2((currentTile * 2 - 1), 1)));

                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, wallHeight, -z - 1), new Vector3(1, 0, 0), new Vector2((currentTile * 2 - 1), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentTile * 2), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, wallHeight, -z), new Vector3(1, 0, 0), new Vector2((currentTile * 2), 0)));



                        //left wall
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2 - 1), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2 - 1), 0)));

                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2 - 1), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, wallHeight, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentTile * 2), 1)));


                        //back wall
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentTile * 2), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(-1, 0, 0), new Vector2((currentTile * 2 - 1), 1)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z - 1), new Vector3(-1, 0, 0), new Vector2((currentTile * 2 - 1), 0)));

                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z - 1), new Vector3(-1, 0, 0), new Vector2((currentTile * 2 - 1), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, wallHeight, -z), new Vector3(-1, 0, 0), new Vector2((currentTile * 2), 0)));
                        verticesListWall.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentTile * 2), 1)));
                    }

                }
            }

            floorVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesListFloor.Count, BufferUsage.WriteOnly);
            floorVertexBuffer.SetData<VertexPositionNormalTexture>(verticesListFloor.ToArray());
          

            wallVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesListWall.Count, BufferUsage.WriteOnly);
            wallVertexBuffer.SetData<VertexPositionNormalTexture>(verticesListWall.ToArray());
        }

            public void DrawLevel(Matrix world, Matrix view, Matrix proj)
            {
                effect = new BasicEffect(device);
                effect.World = world;
                effect.View = view;
                effect.Projection = proj;
                effect.TextureEnabled = true;
                
                effect.Texture = floorTexture;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.SetVertexBuffer(floorVertexBuffer);
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, floorVertexBuffer.VertexCount / 3);
                }

                effect.Texture = wallTexture;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.SetVertexBuffer(wallVertexBuffer);
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, wallVertexBuffer.VertexCount / 3);

                }
        }
    }
}
