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
        public int playerScore, time;
        public SpriteFont scoreFont;
        public Vector2 playerScorePos, playerTimePos;

        //fps
        int _total_frames = 0;
        float _elapsed_time = 0.0f;
        int _fps = 0;
        Texture2D HudLevel0;
        public Rectangle okButton;
       
        //Constructor
        public HUD()
        {
            playerScorePos = new Vector2(10, 10);
            playerTimePos = new Vector2(150, 10);
            okButton = new Rectangle(370, 460, 100, 35);
        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            scoreFont = Content.Load<SpriteFont>("Arial");
            HudLevel0 = Content.Load<Texture2D>("Screens/HudLevel0");
        }

        //Update
        public void Update(GameTime gameTime)
        {
            // Update
            _elapsed_time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
 
            // 1 Second has passed
            if (_elapsed_time >= 1000.0f)
            {
                _fps = _total_frames;
                _total_frames = 0;
                _elapsed_time = 0;
            }
            
        }

        public void TutorialHud(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //string move = " Move:\n     ___\n     |W|\n|A| |S| |D|\n";
            //string jump = " Jump:\n ________\n |  Space  |\n";
            _total_frames++;
            
            //spriteBatch.Begin();
            //string text = "Bewegen: W,A,S,D\n" + "Springen: Up\n" + "Reset: R\n" + "toggle Camera: Space";
            //spriteBatch.DrawString(spriteFont, move, new Vector2(5, 5), Color.GhostWhite);
            //spriteBatch.DrawString(spriteFont, jump, new Vector2(5, 120), Color.GhostWhite);
            spriteBatch.DrawString(spriteFont, string.Format("fps={0}", _fps), new Vector2(10.0f, 200.0f), Color.White);
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


        public void DrawHudLevel0(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Draw(HudLevel0, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

        }

    }
}
