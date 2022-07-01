using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;

namespace JoJoStands.UI
{
    public class MouseTextPanel : UIPanel
    {
        public UIText uiText;
        public Vector2 ownerPos;

        public bool visible = false;
        public bool wrapText = false;
        public int textCharacterLimit = 0;

        private readonly string[] StringSplitters = new string[1] { " " };

        public MouseTextPanel(int width, int height, string defaultText = "", bool wrapAllText = false, int textWrapCharacterLimit = 0)
        {
            Width.Pixels = width;
            Height.Pixels = height;
            wrapText = wrapAllText;
            textCharacterLimit = textWrapCharacterLimit;

            uiText = new UIText(defaultText);
            Append(uiText);
        }

        public void ShowText(string newText)
        {
            visible = true;
            if (wrapText)
                newText = WrapText(newText, textCharacterLimit, StringSplitters);

            uiText.SetText(newText);
            Width.Pixels = FontAssets.MouseText.Value.MeasureString(newText).X * Main.UIScale + 8f;
            Height.Pixels = FontAssets.MouseText.Value.MeasureString(newText).Y * Main.UIScale + 8f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!visible)
                return;

            Left.Pixels = Main.MouseScreen.X + 10f - ownerPos.X;
            Top.Pixels = Main.MouseScreen.Y + 10f - ownerPos.Y;
            if (Left.Pixels < 2)
                Left.Pixels = 2;
            if (Left.Pixels + Width.Pixels >= Main.screenWidth)
                Left.Pixels = Main.screenWidth - Width.Pixels - 2;
            if (Top.Pixels < 2)
                Top.Pixels = 2;
            if (Top.Pixels + Height.Pixels >= Main.screenHeight)
                Top.Pixels = Main.screenHeight - Height.Pixels - 2;

            uiText.Left.Pixels = 4f;
            uiText.Top.Pixels = 2f;
            Width.Pixels = FontAssets.MouseText.Value.MeasureString(uiText.Text).X * Main.UIScale + 8f;
            Height.Pixels = FontAssets.MouseText.Value.MeasureString(uiText.Text).Y * Main.UIScale + 8f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!visible)
                return;

            base.Draw(spriteBatch);
        }

        private string WrapText(string textToBreak, int sentenceCharacterLimit, string[] stringSplitParts)
        {
            List<string> createdSentences = new List<string>();
            string[] wordsArray = textToBreak.Split(stringSplitParts, StringSplitOptions.None);

            string sentenceResult = "";
            for (int word = 0; word < wordsArray.Length; word++)
            {
                if (wordsArray[word].Contains("\n"))
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = wordsArray[word] + " ";
                    continue;
                }

                if (sentenceResult.Length + wordsArray[word].Length > sentenceCharacterLimit)
                {
                    createdSentences.Add(sentenceResult);
                    sentenceResult = "\n" + wordsArray[word] + " ";
                }
                else
                {
                    sentenceResult += wordsArray[word] + " ";
                }
            }
            if (sentenceResult != "")       //Cause sometimes it doesn't fill the needed conditions to be considered something to add
            {
                createdSentences.Add(sentenceResult);
            }

            string finalResult = "";
            foreach (string sentencePiece in createdSentences)
            {
                finalResult += sentencePiece;
            }
            return finalResult;
        }
    }
}
