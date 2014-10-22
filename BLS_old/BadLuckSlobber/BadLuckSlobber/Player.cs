#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
#endregion

namespace BadLuckSlobber
{
    class Player
    {
        
        #region Fields
        
         public Vector3 Position;

        //bool jump = false;
        //int jumpvalue = 200;
        //int gravity = 5;
        //int beforeJump = 0;
        
        // Set the avatar position and rotation variables.
        //Vector3 avatarPosition = new Vector3(0, 0, -50);
        public float avatarRot;


        // Set rates in world units per 1/60th second (the default fixed-step interval).
        float rotationSpeed = 0.03f;
        float moveSpeed = 0.03f;


        #endregion
        
        #region Initialization

        public Player(GraphicsDevice device)
        {
            Position = new Vector3(4.0f, 0.0f, -1.5f);
        }

        //public void Reset()
        //{
        //    Position = new Vector3(0, MinimumAltitude, 0);
        //}

        #endregion

        #region Methods

        //public void Jump(int gravity, int jumpvalue)
        //{
        //    Position.Y += jumpvalue;
        //    jumpvalue -= gravity;
        //}

        public Vector3 Update(GameTime gameTime, KeyboardState keyState)
        {
            KeyboardState keyboardState = keyState;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                Position.X = Position.X - moveSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                Position.X = Position.X + moveSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                Position.Z = Position.Z + moveSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.E))
            {
                Position.Z = Position.Z - moveSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                //Rotation
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                //Rotation
            }

            //if (keyboardState.IsKeyDown(Keys.Up) && jump == false)
            //{
            //    jump = true;
            //    int vor_sprung = Convert.ToInt16(Position.Y);
            //}

            //if (jump == true)
            //{
            //    Position.Y += jumpvalue;
            //    jumpvalue -= gravity;

            //    if (Position.Y == beforeJump)
            //    {
            //        jump = false;
            //        jumpvalue = 200;
            //    }
            //}

            return Position;
        }
 
            /// <summary>
            /// Updates the position and direction of the avatar.
            /// </summary>
            public Vector3 UpdateAvatarPosition(GameTime gametime, KeyboardState keyboardState)
            {
               //KeyboardState keyboardState = Keyboard.GetState();
               //GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                
               if (keyboardState.IsKeyDown(Keys.Left) )
               {
                   // Rotate left.
                   avatarRot += rotationSpeed;
               }
               if (keyboardState.IsKeyDown(Keys.Right))
               {
                   // Rotate right.
                   avatarRot -= rotationSpeed;
               }
               
               if (keyboardState.IsKeyDown(Keys.Up))
               {
                   Matrix forwardMovement = Matrix.CreateRotationY(avatarRot);
                   Vector3 v = new Vector3(0, 0, moveSpeed);
                   v = Vector3.Transform(v, forwardMovement);
                   Position.Z += v.Z;
                   Position.X += v.X;
               }
               if (keyboardState.IsKeyDown(Keys.Down))
               {
                   Matrix forwardMovement = Matrix.CreateRotationY(avatarRot);
                   Vector3 v = new Vector3(0, 0, -moveSpeed);
                   v = Vector3.Transform(v, forwardMovement);
                   Position.Z += v.Z;
                   Position.X += v.X;
               }
               return Position;
            }

            /// <summary>
            /// Updates the position and direction of the camera relative to the avatar.
            /// </summary>

            //void UpdateCamera(Matrix world, Matrix view, Matrix proj)
            //{
            //   // Calculate the camera's current position.
            //   //Vector3 thirdPersonReference = new Vector3(0, 100, -100);

            //   Matrix rotationMatrix = Matrix.CreateRotationY(avatarRot);

            //   // Create a vector pointing the direction the camera is facing.
            //   //Vector3 transformedReference =
            //   //   Vector3.Transform(thirdPersonReference, rotationMatrix);

            //   // Calculate the position the camera is looking from.
            //   //Vector3 cameraPosition = transformedReference + Position;

            //   // Calculate the position the camera is looking at.
            //   //Vector3 cameraLookat = cameraPosition + transformedReference;

            //   // Set up the view matrix and projection matrix.
            //   //view = Matrix.CreateLookAt(cameraPosition, avatarPosition, new Vector3(0.0f, 1.0f, 0.0f));

            //   //Viewport viewport = graphics.GraphicsDevice.Viewport;
            //   //float aspectRatio = (float)viewport.Width / (float)viewport.Height;

            //   //projection = Matrix.CreatePerspectiveFieldOfView(viewAngle, aspectRatio, nearClip, farClip);
            //}


        #endregion
        
    }
}
