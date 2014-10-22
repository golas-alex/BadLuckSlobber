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
        
        //need for Rotation
        public Vector3 playerPosition;
        public Quaternion playerRotation = Quaternion.Identity;
        public float turningSpeed;
        
        #endregion
        
        #region Initialization

        public Player(GraphicsDevice device)
        {
            playerPosition = new Vector3(1.5f, 0.135f, -3f);
        }


        public void UpdatePosition(GameTime gameTime, KeyboardState keyboardState)
        {
            //KeyboardState keyboardState = Keyboard.GetState();
            float leftRightRot = 0;
            turningSpeed = 0.02f;
            float jumpValue = 1.0f;
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float gravity = 0.025f;
            bool isJumping = false;

            //Vector3 jump = new Vector3(

            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, 1), playerRotation);

            if (keyboardState.IsKeyDown(Keys.D))
                leftRightRot -= turningSpeed;
            if (keyboardState.IsKeyDown(Keys.A))
                leftRightRot += turningSpeed;
            if (keyboardState.IsKeyDown(Keys.W))
                playerPosition += addVector * turningSpeed;
            if (keyboardState.IsKeyDown(Keys.S))
                playerPosition -= addVector * turningSpeed;

            if (keyboardState.IsKeyDown(Keys.Space) && isJumping == false)
            {
                Vector3 v = new Vector3(0, jumpValue, 0);
                playerPosition.Y += v.Y;
                isJumping = true;
            }
            if (isJumping == true && playerPosition.Y > 0.3f)
            {
                Vector3 v = new Vector3(0, -gravity, 0);
                playerPosition.Y += v.Y;
            }
            if (isJumping == true && playerPosition.Y <= 0.3f)
            {
                isJumping = false;
            }

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), leftRightRot);
            playerRotation *= additionalRot;


        }
        #endregion


        #region Methods

/*
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
        */
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
