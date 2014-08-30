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

        public Texture2D titleScreen;
        public Texture2D startButton;
        public Texture2D creditsButton, settingsButton;
        public Texture2D exitButton;
           
        public Vector2 startButtonPosition;
        public Vector2 creditsButtonPosition, settingsButtonPosition;
        public Vector2 exitButtonPosition;
        
        public enum GameStates { StartMenu, Playing };
        public GameStates gameState = GameStates.StartMenu;
      
        #endregion

        public void Initialize(GraphicsDevice device)
        {
            //set the Positions of the Buttons
            startButtonPosition = new Vector2(600, 120);
            creditsButtonPosition = new Vector2(600, 200);
            settingsButtonPosition = new Vector2(600, 280);
            exitButtonPosition = new Vector2(600, 350);

        }

        public void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            titleScreen = Content.Load<Texture2D>("TitleScreenFinal");
            startButton = Content.Load<Texture2D>("NewGame");
            creditsButton = Content.Load<Texture2D>("Credits");
            settingsButton = Content.Load<Texture2D>("Settings");
            exitButton = Content.Load<Texture2D>("Quit");
            
        }

        public void DrawStartMenu(GameWindow Window, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(titleScreen, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.Draw(startButton, startButtonPosition, Color.White);
            spriteBatch.Draw(creditsButton, creditsButtonPosition, Color.White);
            spriteBatch.Draw(settingsButton, settingsButtonPosition, Color.White);
            spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);   
        }
    }
}
