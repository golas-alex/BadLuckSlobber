using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using std;

namespace BadLuckSlobber
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Fields

        enum CollisionType { None, Wall, Boundary, Slime }

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
        Entity entity = new Entity();

        int screenWidth, screenHeight;
        int score = 0;

        Vector3 playerPosition;
        Quaternion playerRot;
        public bool hasJumped;
        public bool buttonClicked = false;

        Model playerModel;
        SoundEffect menuSong;
        public SoundEffectInstance menuSongInstance;
        public bool pause;

        BoundingBox[] wallBoundingBoxes;
        BoundingBox[] slimeBoxes;
        BoundingBox completeLevelBox;
        
        //List<BoundingSphere> targetList = new List<BoundingSphere>();

        #endregion

        #region Initialization

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            player = new Player(device);
            playerPosition = player.playerPosition;
            playerRot = player.playerRotation;
            

            // Create the chase camera
            camera = new PlayerCamera();

        }


        /// <summary>
        /// Initalize the game
        /// </summary>
        protected override void Initialize()
        {
            screenWidth = graphics.PreferredBackBufferWidth = 800;
            screenHeight = graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "BadLuckSlobber";    

            camera.Initialize(playerPosition);
            gameMenu.Initialize(GraphicsDevice);


            base.Initialize();
        }


        /// <summary>
        /// Load graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            playerModel = Content.Load<Model>("SlobberNeu");
           
            menuSong = Content.Load<SoundEffect>("fungi");
            menuSongInstance = menuSong.CreateInstance();
            menuSongInstance.IsLooped = true;
            menuSongInstance.Play();

            List<Model> entityModels = new List<Model>();
            entityModels.Add(Content.Load<Model>("Models/Schleim"));
            int[] entityQuantities = new int[1];
            entityQuantities[0] = 3;
            List<Vector3> entityPositions = new List<Vector3>();
            entityPositions.Add(new Vector3(1.5f, 0.02f, -2f));
            entityPositions.Add(new Vector3(3f, 0.02f, -6f));
            entityPositions.Add(new Vector3(5f, 0.02f, -1.5f));


            gameMenu.LoadContent(Content, graphics);
            hud.LoadContent(Content);
            level.LoadContent(Content, graphics);
            entity.LoadEntity(entityPositions, entityModels, entityQuantities);
            slimeBoxes = entity.slimeBoxes;

            level.DrawLevel(camera.worldMatrix, camera.view, camera.projection);

            device = graphics.GraphicsDevice;

            SetUpBoundingBoxes();
        }

        #endregion

        #region Methods

        #region Unimplemented

        private void SetUpBoundingBoxes()
        {
            int levelWidth = level.floorPlan.GetLength(0);
            int levelLength = level.floorPlan.GetLength(1);

            List<BoundingBox> bbList = new List<BoundingBox>();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int z = 0; z < levelLength; z++)
                {
                    int levelType = level.floorPlan[x, z];
                    if (levelType != 0)
                    {
                        int wallHeight = level.wallHeight;
                        Vector3[] wallPoints = new Vector3[2];
                        wallPoints[0] = new Vector3(x, 0, -z);
                        wallPoints[1] = new Vector3(x + 1, wallHeight, -z - 1);
                        BoundingBox wallBox = BoundingBox.CreateFromPoints(wallPoints);
                        bbList.Add(wallBox);
                    }
                }
            }
            wallBoundingBoxes = bbList.ToArray();

            Vector3[] boundaryPoints = new Vector3[2];
            boundaryPoints[0] = new Vector3(0f, 0, 0f);
            boundaryPoints[1] = new Vector3(levelWidth, level.wallHeight, levelLength);
            completeLevelBox = BoundingBox.CreateFromPoints(boundaryPoints);
        }

        private CollisionType CheckCollision(BoundingBox player)
        {
            for (int i = 0; i < wallBoundingBoxes.Length; i++)
            {
                if (wallBoundingBoxes[i].Contains(player) != ContainmentType.Disjoint)
                    return CollisionType.Wall;
            }

            if (completeLevelBox.Contains(player) != ContainmentType.Contains)
                return CollisionType.Boundary;

            for (int i = 0; i < slimeBoxes.Length; i++)
            {
                if (slimeBoxes[i].Contains(player) != ContainmentType.Disjoint)
                {
                    entity.entityModels.RemoveAt(i);
                    i--;
                    return CollisionType.Slime;
                }
            }
            //for (int i = 0; i < targetList.Count; i++)
            //{
            //    if (targetList[i].Contains(sphere) != ContainmentType.Disjoint)
            //    {
            //        targetList.RemoveAt(i);
            //        i--;
            //        //AddTargets();

            //        return CollisionType.Target;
            //    }
            //}

            return CollisionType.None;
        }
        #endregion


        protected override void Update(GameTime gameTime)
        {
            //KeyboardState keyboard = new KeyboardState();
            float turningSpeed = 0.03f;


            // Exit when the Escape key or Back button is pressed
           /* if (keyboard.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            */
            MouseGetState(gameTime, graphics);

            hud.Update(gameTime);
            // movement of Slobber


            //player.UpdatePosition(gameTime, currentKeyboardState);
            //playerPosition = player.Update(gameTime, currentKeyboardState);
            //player.UpdateAvatarPosition(gameTime, currentKeyboardState);



            //BoundingSphere playerSphere = new BoundingSphere(playerPosition, 0.04f);
            Vector3[] playerPoints = new Vector3[2];
            playerPoints[0] = new Vector3(playerPosition.X - 0.2f, 0.01f, playerPosition.Z - 0.08f);
            playerPoints[1] = new Vector3(playerPosition.X + 0.2f, playerPosition.Y + 0.01f, playerPosition.Z + 0.08f);
            BoundingBox playerBox = BoundingBox.CreateFromPoints(playerPoints);

            if (CheckCollision(playerBox) == CollisionType.Wall)
            {
                
                //playerPosition = player.playerPosition;
                //playerPosition = new Vector3(playerPosition.X+0.01f, playerPosition.Y, playerPosition.Z-0.01f);
                //playerPosition = player.playerPosition;
                turningSpeed = -0.25f;

                //Console.WriteLine(playerPosition);
                //playerRot = Quaternion.Identity;
                //player.Reset();
                //camera.Reset();
                //score = 0;
            }
            if (CheckCollision(playerBox) != CollisionType.Wall)
            {
                turningSpeed = 0.02f;
            }

            if (CheckCollision(playerBox) == CollisionType.Slime)
                score++;

            ProcessKeyboard(gameTime, turningSpeed);

            //if (playerPosition.Z > -1.0f)
            //    playerPosition.Z = -1.2f;
            //if (playerPosition.X > 8.8f)
            //    playerPosition.X = 8.6f;
            //if (playerPosition.Z < -7.8f)
            //    playerPosition.Z = -7.6f;
            
            //playerPosition = new Vector3(playerPosition.X + 1.0f, playerPosition.Y, playerPosition.Z +1.0f);
            //if (CheckCollision(playerSphere) == CollisionType.Target)
            //    score++;
            camera.Update(graphics, playerPosition, playerRot);

            base.Update(gameTime);
        }

        public void MouseGetState(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            //if (mouseState.LeftButton == ButtonState.Pressed)
            if (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(currentMouseState.X, currentMouseState.Y, graphics);
                //Console.WriteLine(Mouse.GetState());
            }

            //previousMouseState = mouseState;
        }

        public void MouseClicked(int x, int y, GraphicsDeviceManager graphics)
        {
            Rectangle mouseClickRect = new Rectangle(x-10, y-10, 20, 20);

            Rectangle startButtonRect = new Rectangle((int)gameMenu.startButtonPosition.X,
                        (int)gameMenu.startButtonPosition.Y, 130, 70);
            Rectangle creditsButtonRect = new Rectangle((int)gameMenu.creditsButtonPosition.X,
                        (int)gameMenu.creditsButtonPosition.Y, 130, 70);
            Rectangle settingsButtonRect = new Rectangle((int)gameMenu.settingsButtonPosition.X,
                        (int)gameMenu.settingsButtonPosition.Y, 130, 70);
            Rectangle exitButtonRect = new Rectangle((int)gameMenu.exitButtonPosition.X,
                        (int)gameMenu.exitButtonPosition.Y, 130, 70);

            if(gameMenu.gameState == GameMenu.GameStates.StartMenu)
            {
                //menuSong.Play();
                //menuSong.Play(0.1f, 0.0f, 0.0f);
                //menuSongInstance.Play();


                //player clicked start button
                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameMenu.gameState = GameMenu.GameStates.Playing;
                    menuSongInstance.Stop();
                    //firstTime = true;

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
                    //gameMenu.gameState = GameMenu.GameStates.StartMenu;
                }
                //Console.WriteLine(gameMenu.gameState);
                //bool wahrheit = mouseClickRect.Intersects(startButtonRect);
                //Console.WriteLine(wahrheit);
            }
            
            if (gameMenu.gameState == GameMenu.GameStates.Playing)
            {
                Rectangle PauseResumeButton = new Rectangle(300, 200, 200, 50);
                Rectangle PauseMainMenuButton = new Rectangle(300, 280, 200, 50);
                Rectangle PauseSettingsButton = new Rectangle(300, 360, 200, 50);
                Rectangle PauseQuitButton = new Rectangle(300, 440, 200, 50);


                if (currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                    pause = true;
                    gameMenu.gameState = GameMenu.GameStates.PauseMenu;
                }
                if ((mouseClickRect.Intersects(PauseResumeButton)|| currentKeyboardState.IsKeyDown(Keys.Escape))&&pause)
                {
                    pause = false;
                }

                if (mouseClickRect.Intersects(PauseMainMenuButton) && pause)
                {
                    gameMenu.gameState = GameMenu.GameStates.StartMenu;
                }

                if (mouseClickRect.Intersects(PauseSettingsButton) && pause)
                {
                    gameMenu.gameState = GameMenu.GameStates.Settings;
                }

                if (mouseClickRect.Intersects(PauseQuitButton) && pause)
                {
                    this.Exit();
                }

                if (mouseClickRect.Intersects(hud.okButton))
                {
                    buttonClicked = true;
                }
            }
            
            if ((gameMenu.gameState == GameMenu.GameStates.Settings) || (gameMenu.gameState == GameMenu.GameStates.Credits))
            {
                Rectangle backButtonRect = new Rectangle((int)gameMenu.backButtonPosition.X, (int)gameMenu.backButtonPosition.Y, 100, 70);
                Rectangle FullScreenRect = new Rectangle((int)gameMenu.OffButtonPosition.X, (int)gameMenu.OffButtonPosition.Y, 100, 70);

                if (mouseClickRect.Intersects(backButtonRect)&& pause == false)
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


                        //graphics.ToggleFullScreen;

                    }
                    if (mouseClickRect.Intersects(backButtonRect) && pause)
                        gameMenu.gameState = GameMenu.GameStates.Playing;

                    graphics.ApplyChanges();
                }
            }

        }

        private void ProcessKeyboard(GameTime gameTime, float turningSpeed)
        {
            lastKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            float leftRightRot = 0;
            float jumpValue = 0.5f;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float gravity = 0.02f;


            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, 1), playerRot);


            if (currentKeyboardState.IsKeyDown(Keys.D))
                leftRightRot -= turningSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.A))
                leftRightRot += turningSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.W))
                playerPosition += addVector * turningSpeed;
            if (currentKeyboardState.IsKeyDown(Keys.S))
                playerPosition -= addVector * turningSpeed;
            //if (currentKeyboardState.IsKeyDown(Keys.Space))
            //playerPosition += new Vector3(0, jumpValue , 0) * elapsed;
            //jump(gameTime);
            //hasJumped = true;
            //jump();
            if (currentKeyboardState.IsKeyDown(Keys.Space) && hasJumped == false)
            {
                Vector3 v = new Vector3(0, jumpValue, 0);
                playerPosition.Y += v.Y;
                hasJumped = true;
            }
            if (hasJumped == true && playerPosition.Y > player.playerPosition.Y)
            {
                Vector3 v = new Vector3(0, -gravity, 0);
                playerPosition.Y += v.Y;
            }
            if (hasJumped == true && playerPosition.Y < player.playerPosition.Y)
            {
                hasJumped = false;
            }

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), leftRightRot);
            playerRot *= additionalRot;

        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.Black);

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
                level.DrawLevel(camera.worldMatrix, camera.view, camera.projection);
                entity.DrawEntity(camera.worldMatrix, camera.view, camera.projection);
                //DrawModel(playerModel, Matrix.Identity);
                //level.DrawLevel(camera.worldMatrix, camera.view, camera.projection);
                DrawModel(playerModel, camera.worldMatrix, camera.view, camera.projection);
                string showScore = "Score" + score;
                spriteBatch.DrawString(spriteFont, showScore, new Vector2(350, 5), Color.GhostWhite);
                hud.TutorialHud(spriteBatch, spriteFont);


                if (pause)
                {
                    gameMenu.DrawPauseMenu(Window, spriteBatch);
                }
                
                if (!buttonClicked)
                {
                    hud.DrawHudLevel0(spriteBatch, Window);
                    //firstTime = false;
                    //gameMenu.gameState = GameMenu.GameStates.StartMenu;
                }
               
               if (currentKeyboardState.IsKeyDown(Keys.Escape))
                {
                  pause = true;
                  if (pause)
                  {
                      gameMenu.DrawPauseMenu(Window, spriteBatch);
                      //gameMenu.gameState = GameMenu.GameStates.PauseMenu;
                      Console.WriteLine(gameMenu.gameState);
                  }
                    //gameMenu.gameState = GameMenu.GameStates.PauseMenu;
                    //firstTime = false;
                    //gameMenu.gameState = GameMenu.GameStates.StartMenu;
                }
               
            }
            /*
            if (gameMenu.gameState == GameMenu.GameStates.PauseMenu)
            {
                gameMenu.DrawPauseMenu(Window, spriteBatch);
            }
            */
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

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix proj)
        {

            world = Matrix.CreateScale(0.005f) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) * Matrix.CreateFromQuaternion(playerRot) * Matrix.CreateTranslation(playerPosition);
            

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();

                    effect.Projection = proj;
                    effect.View = view;
                    effect.World = world;     
                }
                mesh.Draw();
            }
        }
        #endregion
    }
}