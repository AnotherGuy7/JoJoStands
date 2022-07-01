using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class GlobalMouseTextPanel : UIState
    {
        public static GlobalMouseTextPanel globalMouseTextPanel;

        public UIText uiText;
        public UIPanel uiPanel;
        public UIPanel mainPanel;

        public bool visible = false;
        private int visibilityTimer = 0;
        private Vector2 truePanelSize;      //Vanilla limits the size of children UI based on the size of the master UI obj. To get around this there's a huge panel in the background and a "true" panel which shows. This is the size of that panel.

        private readonly string[] StringSplitters = new string[1] { " " };

        public GlobalMouseTextPanel(int width, int height, string defaultText = "")
        {
            truePanelSize = new Vector2(width, height);

            uiPanel = new UIPanel();
            uiPanel.Width.Pixels = width;
            uiPanel.Height.Pixels = height;
            Append(uiPanel);

            uiText = new UIText(defaultText);
            Append(uiText);

            globalMouseTextPanel = this;
        }

        public override void OnInitialize()
        {
            mainPanel = new UIPanel();
            mainPanel.HAlign = 0.5f;
            mainPanel.VAlign = 0.5f;
            mainPanel.Width.Pixels = 900;
            mainPanel.Height.Pixels = 900;
            mainPanel.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            mainPanel.BorderColor = new Color(0, 0, 0, 0);

            Append(mainPanel);
        }

        public void ShowText(string newText)
        {
            visible = true;
            visibilityTimer = 2;
            uiText.SetText(newText);
            uiPanel.Width.Pixels = FontAssets.MouseText.Value.MeasureString(newText).X + 8f;
            uiPanel.Height.Pixels = FontAssets.MouseText.Value.MeasureString(newText).Y + 8f;
            truePanelSize = new Vector2(uiPanel.Width.Pixels, uiPanel.Height.Pixels);
        }

        public void ShowText(string newText, int textCharacterLimit)
        {
            visible = true;
            visibilityTimer = 2;
            newText = WrapText(newText, textCharacterLimit, StringSplitters);

            uiText.SetText(newText);
            uiPanel.Width.Pixels = FontAssets.MouseText.Value.MeasureString(newText).X + 8f;
            uiPanel.Height.Pixels = FontAssets.MouseText.Value.MeasureString(newText).Y + 8f;
            truePanelSize = new Vector2(uiPanel.Width.Pixels, uiPanel.Height.Pixels);
        }

        public override void Update(GameTime gameTime)
        {
            if (!visible)
                return;

            if (visibilityTimer > 0)
            {
                visibilityTimer--;
                if (visibilityTimer <= 0)
                    visible = false;
            }

            Left.Pixels = Main.MouseScreen.X + 10f;
            Top.Pixels = Main.MouseScreen.Y + 10f;
            if (Left.Pixels < 2)
                Left.Pixels = 2;
            if (Left.Pixels + (truePanelSize.X) >= Main.screenWidth)
                Left.Pixels = Main.screenWidth - truePanelSize.X - 2;
            if (Top.Pixels < 2)
                Top.Pixels = 2;
            if (Top.Pixels + truePanelSize.Y >= Main.screenHeight)
                Top.Pixels = Main.screenHeight - truePanelSize.Y - 2;

            uiText.Left.Pixels = 4f;
            uiText.Top.Pixels = 8f;
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
