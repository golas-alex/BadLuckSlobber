#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace BadLuckSlobber
{
    class PlayerCamera
    {
        public Matrix worldMatrix;
        public Matrix view;
        public Matrix projection;
        Vector3 cameraPosition;
        Vector3 thirdPersonReference;

        #region Methods

        public void Initialize(Vector3 position) 
        {
            cameraPosition = new Vector3(position.X, position.Y, position.Z);
            thirdPersonReference= new Vector3(0f, 0.5f, -1.0f);
        }

        public void Update(GraphicsDeviceManager graphics, Vector3 position, Quaternion rotation)
        {  
            //Vector3 cameraLookAt = new Vector3(0.0f, 1.0f, 0.0f);
            float fovAngle = MathHelper.ToRadians(45.0f);
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            //float aspectRatio = graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height;
            float near = 0.1f; // the near clipping plane distance
            float far = 1000f; // the far clipping plane distance

            //Matrix rotationMatrix = Matrix.CreateRotationY(rotation);
            Matrix rotationMatrix = Matrix.CreateFromQuaternion(rotation);
            Vector3 transformedReference = Vector3.Transform(thirdPersonReference, rotationMatrix);
            cameraPosition = transformedReference + position;

            worldMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            view = Matrix.CreateLookAt(cameraPosition, position, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);
        }

        #endregion

    }
}
