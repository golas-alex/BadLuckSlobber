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
using std;

namespace BadLuckSlobber
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        enum CollisionType { None, Wall, Target }

        GraphicsDeviceManager graphics;
        GraphicsDevice device;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        KeyboardState lastKeyboardState;
        KeyboardState currentKeyboardState;

        MouseState lastMouseState;
        MouseState currentMouseState;

        GameMenu gameMenu = new GameMenu();
        Player player;
        PlayerCamera camera;
        HUD hud = new HUD();
        Level level = new Level();

        Model playerModel;

        //BoundingBox[] wallBoundingBoxes;
        //List<BoundingSphere> targetList = new List<BoundingSphere>();

        bool cameraSpringEnabled = true;

        #endregion

        #region Initialization

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.Portrait;


            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Create the chase camera
            camera = new PlayerCamera();

            // Set the camera offsets
            camera.DesiredPositionOffset = new Vector3(0.0f, 2000.0f, 6000.0f);
            camera.LookAtOffset = new Vector3(0.0f, 150.0f, 0.0f);

            // Set camera perspective
            camera.NearPlaneDistance = 10.0f;
            camera.FarPlaneDistance = 100000.0f;
        }


        /// <summary>
        /// Initalize the game
        /// </summary>
        protected override void Initialize()
        {

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "BadLuckSlobber";

            gameMenu.Initialize(GraphicsDevice);
            base.Initialize();

            player = new Player(device);

            // Set the camera aspect ratio
            // This must be done after the class to base.Initalize() which will
            // initialize the graphics device.
            camera.AspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
                graphics.GraphicsDevice.Viewport.Height;

            // Perform an inital reset on the camera so that it starts at the resting
            // position. If we don't do this, the camera will start at the origin and
            // race across the world to get behind the chased object.
            // This is performed here because the aspect ratio is needed by Reset.
            UpdateCameraChaseTarget();
            camera.Reset();
        }


        /// <summary>
        /// Load graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            playerModel = Content.Load<Model>("Slobber");

            gameMenu.LoadContent(Content, graphics);
            hud.LoadContent(Content);
            level.LoadContent(Content, graphics);

            device = graphics.GraphicsDevice;

            //SetUpBoundingBoxes();
        }

        #endregion

        #region Methods

        #region Unimplemented
        //private void SetUpBoundingBoxes()
        //{
        //    int areaWidth = level.floorPlan.GetLength(0);
        //    int areaLength = level.floorPlan.GetLength(1);

        //    List<BoundingBox> bbList = new List<BoundingBox>();

        //    for (int x = 0; x < areaWidth; x++)
        //    {
        //        for (int z = 0; z < areaLength; z++)
        //        {
        //            int areaType = level.floorPlan[x, z];
        //            if (areaType != 0)
        //            {
        //                Vector3[] wallPoints = new Vector3[2];
        //                wallPoints[0] = new Vector3(x, 0, -z);
        //                wallPoints[1] = new Vector3(x + 1, 1/*wallHeigth*/, -z - 1);
        //                BoundingBox wallBox = BoundingBox.CreateFromPoints(wallPoints);
        //                bbList.Add(wallBox);
        //            }
        //        }
        //    }
        //    wallBoundingBoxes = bbList.ToArray();
        //}

        //private CollisionType CheckCollision(BoundingSphere sphere)
        //{
        //    for (int i = 0; i < wallBoundingBoxes.Length; i++)
        //    {
        //        if (wallBoundingBoxes[i].Contains(sphere) != ContainmentType.Disjoint)
        //            return CollisionType.Wall;
        //    }

        //    //for (int i = 0; i < targetList.Count; i++)
        //    //{
        //    //    if (targetList[i].Contains(sphere) != ContainmentType.Disjoint)
        //    //    {
        //    //        targetList.RemoveAt(i);
        //    //        i--;
        //    //        //AddTargets();

        //    //        return CollisionType.Target;
        //    //    }
        //    //}

        //    return CollisionType.None;
        //}
        #endregion

        protected override void Update(GameTime gameTime)
        {
            // Process keyboard input
            ProcessKeyboard(gameTime);

            MouseGetState(gameTime);
            
            // Update the player
            player.Update(gameTime);

            // Update the camera to chase the new target
            UpdateCameraChaseTarget();
            

            // The chase camera's update behavior is the springs, but we can
            // use the Reset method to have a locked, spring-less camera
            if (cameraSpringEnabled)
                camera.Update(gameTime);
            else
                camera.Reset();

            //BoundingSphere playerSphere = new BoundingSphere(player.Position, 0.025f);
            //if (CheckCollision(playerSphere) == CollisionType.Wall)
            //{
            //    player.Reset();
            //    camera.Reset();
            //    //score = 0;
            //}

            //if (CheckCollision(playerSphere) == CollisionType.Target)
            //    score++;

            base.Update(gameTime);
        }

        void MouseGetState(GameTime gameTime)
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
                        
            //if (mouseState.LeftButton == ButtonState.Pressed)
            if (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(currentMouseState.X, currentMouseState.Y);
                //Console.WriteLine(Mouse.GetState());
            }

            //previousMouseState = mouseState;
        }
        

        void MouseClicked(int x,int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 20, 20);

            if (gameMenu.gameState == GameMenu.GameStates.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)gameMenu.startButtonPosition.X,
                            (int)gameMenu.startButtonPosition.Y, 130, 70) ;
                Rectangle creditsButtonRect = new Rectangle((int)gameMenu.creditsButtonPosition.X,
                            (int)gameMenu.creditsButtonPosition.Y, 130, 70);
                Rectangle settingsButtonRect = new Rectangle((int)gameMenu.settingsButtonPosition.X,
                            (int)gameMenu.settingsButtonPosition.Y, 130, 70);
                Rectangle exitButtonRect = new Rectangle((int)gameMenu.exitButtonPosition.X,
                            (int)gameMenu.exitButtonPosition.Y, 130, 70);

                //bool wahrheit = mouseClickRect.Intersects(startButtonRect);
                //Console.WriteLine(wahrheit);

                //player clicked start button
                if (mouseClickRect.Intersects(startButtonRect)) 
                {
                    gameMenu.gameState = GameMenu.GameStates.Playing;
                }

                //player clicked credits button
                if (mouseClickRect.Intersects(creditsButtonRect))
                {
                    gameMenu.gameState = GameMenu.GameStates.Credits;
                }

                //player clicked settings button
                if (mouseClickRect.Intersects(settingsButtonRect))
                {
                    gameMenu.gameState = GameMenu.GameStates.Settings;
                }

                //player clicked exit button
                if (mouseClickRect.Intersects(exitButtonRect))
                {
                    this.Exit();
                }
            }
            if ((gameMenu.gameState == GameMenu.GameStates.Settings)||(gameMenu.gameState == GameMenu.GameStates.Credits))
            {
                Rectangle backButtonRect = new Rectangle((int)gameMenu.backButtonPosition.X, (int)gameMenu.backButtonPosition.Y, 100, 70);
                Rectangle FullScreenRect = new Rectangle((int)gameMenu.OffButtonPosition.X, (int)gameMenu.OffButtonPosition.Y, 100, 70);

                if (mouseClickRect.Intersects(backButtonRect))
                {
                    gameMenu.gameState = GameMenu.GameStates.StartMenu;
                }
                if (gameMenu.gameState == GameMenu.GameStates.Settings)
                {
                    if (mouseClickRect.Intersects(FullScreenRect))
                    {
                        if (graphics.IsFullScreen == false)
                        {
                            graphics.IsFullScreen = true;
                        }
                        else graphics.IsFullScreen = false;
                    }
                    graphics.ApplyChanges();
                }
            }

        }

        private void UpdateCameraChaseTarget()
        {
            camera.ChasePosition = player.Position + new Vector3(0, 3000, 0);
            camera.ChaseDirection = player.Direction;
            camera.Up = player.Up;
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // Exit when the Escape key or Back button is pressed
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Pressing the A button or key toggles the spring behavior on and off
            if (lastKeyboardState.IsKeyUp(Keys.Space) && (currentKeyboardState.IsKeyDown(Keys.Space)))
            {
                cameraSpringEnabled = !cameraSpringEnabled;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            DrawMenu();

            base.Draw(gameTime);
        }

        private void DrawMenu()
        {
            spriteBatch = new SpriteBatch(device);
            spriteBatch.Begin();

            //draws the StartMenu
            if (gameMenu.gameState == GameMenu.GameStates.StartMenu)
            {
                gameMenu.DrawStartMenu(Window, spriteBatch);
            }

            if (gameMenu.gameState == GameMenu.GameStates.Playing)
            {
                DrawModel(playerModel, player.PlayerWorld);
                level.DrawLevel(camera);
                //hud.Draw(spriteBatch, device);
                hud.TutorialHud(spriteBatch, spriteFont);
            }

            if (gameMenu.gameState == GameMenu.GameStates.Settings)
            {
                gameMenu.DrawSettingMenu(Window, spriteBatch);
                if (graphics.IsFullScreen == false)
                {
                    spriteBatch.Draw(gameMenu.OffButton, gameMenu.OffButtonPosition, Color.White);
                }
                else if (graphics.IsFullScreen)
                {
                    spriteBatch.Draw(gameMenu.OnButton, gameMenu.OnButtonPosition, Color.White);
                }
            }

            if (gameMenu.gameState == GameMenu.GameStates.Credits)
            {
                gameMenu.DrawCreditsMenu(Window, spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;

                    // Use the matrices provided by the chase camera
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}