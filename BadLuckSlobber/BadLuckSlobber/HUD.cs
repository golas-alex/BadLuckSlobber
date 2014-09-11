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
        public SpriteFont scoreFont;
        public Vector2 playerScorePos, playerTimePos;
       
        //Constructor
        public HUD()
        {
            playerScorePos = new Vector2(10, 10);
            playerTimePos = new Vector2(150, 10);
        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            scoreFont = Content.Load<SpriteFont>("Arial");
        }

        //Update
        public void Update(GameTime gameTime)
        {

        }

        public void TutorialHud(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            string move = " Move:\n     ___\n     |W|\n|A| |S| |D|\n";
            string jump = " Jump:\n ________\n |  Space  |\n";
            //spriteBatch.Begin();
            //string text = "Bewegen: W,A,S,D\n" + "Springen: Up\n" + "Reset: R\n" + "toggle Camera: Space";
            spriteBatch.DrawString(spriteFont, move, new Vector2(5, 5), Color.GhostWhite);
            spriteBatch.DrawString(spriteFont, jump, new Vector2(5, 120), Color.GhostWhite);
            //spriteBatch.End();
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            spriteBatch = new SpriteBatch(device);
            spriteBatch.Begin();
            spriteBatch.DrawString(scoreFont, "Score = " + playerScore, playerScorePos, Color.Yellow);
            spriteBatch.DrawString(scoreFont, "Time = " + time, playerTimePos, Color.Yellow);
            spriteBatch.End();
        }


    }
}
