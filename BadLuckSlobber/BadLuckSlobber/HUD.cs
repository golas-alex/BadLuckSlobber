using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;



namespace BadLuckSlobber
{
    public class HUD
    {
        public int playerScore, time, screenWidth, screenHeight;
        public SpriteFont playerScoreFont, playerGoalFont, playerTimeFont;
        public Vector2 playerScorePos, playerTimePos;
        public int i = 0;
        public float j = 100;
       
        //Constructor
        public HUD()
        {
            playerScore = 0;
            screenHeight = 0;
            screenWidth = 100;
            playerScoreFont = null;
            playerTimeFont = null;
            playerScorePos = new Vector2(10, 10);
            playerTimePos = new Vector2(150, 10);
        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("Arial");
            playerTimeFont = Content.Load<SpriteFont>("Arial");
            playerGoalFont = Content.Load<SpriteFont>("Arial");
        }

        //Update
        public void Update(GameTime gameTime)
        {

        }

        //Draw
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
                spriteBatch = new SpriteBatch(device);
                spriteBatch.Begin();
                spriteBatch.DrawString(playerScoreFont, "Score = " + playerScore, playerScorePos, Color.Yellow);
                spriteBatch.DrawString(playerTimeFont, "Time = " + time, playerTimePos, Color.Yellow);
                spriteBatch.End();
        }

    }
}
