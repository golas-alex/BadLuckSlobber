using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;




namespace BadLuckSlobber
{
    public class GameMenu
    {
        #region Variable Declaration

        public Texture2D titleScreen, settingsScreen, creditsScreen, pauseScreen;
        public Texture2D startButton, creditsButton, settingsButton, exitButton;
        public Texture2D backButton, OffButton, OnButton;
           
        public Vector2 startButtonPosition, creditsButtonPosition, settingsButtonPosition, exitButtonPosition;
        public Vector2 backButtonPosition, OffButtonPosition, OnButtonPosition;
        
        public enum GameStates { StartMenu, Playing , Settings, Credits, PauseMenu};
        public GameStates gameState = GameStates.StartMenu;
      
        #endregion

        public void Initialize(GraphicsDevice device)
        {
            //set the Positions of the Buttons
            startButtonPosition = new Vector2(600, 120);
            creditsButtonPosition = new Vector2(600, 200);
            settingsButtonPosition = new Vector2(600, 280);
            exitButtonPosition = new Vector2(600, 350);
            backButtonPosition = new Vector2(30, 50);
            OffButtonPosition = new Vector2(680, 220);
            OnButtonPosition = new Vector2(680, 220);

        }

        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            titleScreen = Content.Load<Texture2D>("TitleScreenFinal");
            settingsScreen = Content.Load<Texture2D>("SettingsMenu");
            creditsScreen = Content.Load<Texture2D>("CreditsMenu");
            pauseScreen = Content.Load<Texture2D>("Screens/PauseMenu");
            startButton = Content.Load<Texture2D>("NewGame");
            creditsButton = Content.Load<Texture2D>("Credits");
            settingsButton = Content.Load<Texture2D>("Settings");
            exitButton = Content.Load<Texture2D>("Quit");
            backButton = Content.Load<Texture2D>("Back");
            OffButton = Content.Load<Texture2D>("Off");
            OnButton = Content.Load<Texture2D>("On");
            
        }

        public void DrawStartMenu(GameWindow Window, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(startButton, startButtonPosition, Color.White);
            spriteBatch.Draw(creditsButton, creditsButtonPosition, Color.White);
            spriteBatch.Draw(settingsButton, settingsButtonPosition, Color.White);
            spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);   
        }

        public void DrawSettingMenu(GameWindow Window, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(settingsScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(backButton, backButtonPosition, Color.White);

        }

        public void DrawCreditsMenu(GameWindow Window, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(creditsScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(backButton, backButtonPosition, Color.White);
        }

        public void DrawPauseMenu(GameWindow Window, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pauseScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
        }

    }
}
