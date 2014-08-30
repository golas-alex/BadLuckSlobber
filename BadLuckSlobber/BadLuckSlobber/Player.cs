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

        private const float MinimumAltitude = 350.0f;

        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Up;
        private Vector3 right;
        public Vector3 Right
        {
            get { return right; }
        }

        private const float RotationRate = 1.5f;
        private const float Mass = 1.0f;
        private const float ThrustForce = 20000.0f;
        private const float DragFactor = 0.97f;
        public Vector3 Velocity;

        public Matrix PlayerWorld;

        bool jump = false;
        int jumpvalue = 200;
        int gravity = 5;
        int beforeJump = 0;

        #endregion

        #region Initialization

        public Player(GraphicsDevice device)
        {
            Reset();
        }

        public void Reset()
        {
            Position = new Vector3(0, MinimumAltitude, 0);
            Direction = Vector3.Forward;
            Up = Vector3.Up;
            right = Vector3.Right;
            Velocity = Vector3.Zero;
        }

        #endregion

        #region Methods

        public void Jump(int gravity, int jumpvalue)
        {
            Position.Y += jumpvalue;
            jumpvalue -= gravity;

        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Determine rotation amount from input
            Vector2 rotationAmount = gamePadState.ThumbSticks.Left;
            if (keyboardState.IsKeyDown(Keys.A))
                rotationAmount.X = 1.0f;
            if (keyboardState.IsKeyDown(Keys.D))
                rotationAmount.X = -1.0f;

            if (keyboardState.IsKeyDown(Keys.Up) && jump == false)
            {
                jump = true;
                int vor_sprung = Convert.ToInt16(Position.Y);
            }

            if (jump == true)
            {
                Position.Y += jumpvalue;
                jumpvalue -= gravity;

                if (Position.Y == beforeJump)
                {
                    jump = false;
                    jumpvalue = 200;
                }
            }
            
            // Scale rotation amount to radians per second
            rotationAmount = rotationAmount * RotationRate * elapsed;

            // Create rotation matrix from rotation amount
            Matrix rotationMatrix =
            Matrix.CreateFromAxisAngle(Right, rotationAmount.Y) *
            Matrix.CreateRotationY(rotationAmount.X);

            // Rotate orientation vectors
            Direction = Vector3.TransformNormal(Direction, rotationMatrix);
            Up = Vector3.TransformNormal(Up, rotationMatrix);

            Direction.Normalize();
            Up.Normalize();

            // Re-calculate Right
            right = Vector3.Cross(Direction, Up);

            Up = Vector3.Cross(Right, Direction);

            // Determine thrust amount from input
            float thrustAmount = 0;
            if (keyboardState.IsKeyDown(Keys.W))
                thrustAmount = -1.0f;
            if (keyboardState.IsKeyDown(Keys.S))
                thrustAmount = +1.0f;

            // Calculate force from thrust amount
            Vector3 force = Direction * thrustAmount * ThrustForce;

            // Apply acceleration
            Vector3 acceleration = force / Mass;
            Velocity += acceleration * elapsed;

            // Apply psuedo drag
            Velocity *= DragFactor;

            // Apply velocity
            Position += Velocity * elapsed;

            Position.Y = Math.Max(Position.Y, MinimumAltitude);

            PlayerWorld = Matrix.Identity;
            PlayerWorld.Forward = Direction;
            PlayerWorld.Up = Up;
            PlayerWorld.Right = right;
            PlayerWorld.Translation = Position;
        }

        #endregion
    }
}
