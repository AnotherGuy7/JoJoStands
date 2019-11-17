using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace JoJoStands.UI
{
    internal class BetUI : UIState
    {
        public DragableUIPanel Bet;
        public UIPanel playArea;
        public UIText pCoinText;
        public UIText gCoinText;

        //1st minigame stuff
        public UIImageButton rockButton;
        public UIImageButton paperButton;
        public UIImageButton scissorsButton;
        public UIImage playerResult;
        public UIImage npcResult;
        public int playerChoice = 0;
        public int randomSign = 0;

        //2nd minigame stuff
        public Texture2D dieTexture1;
        public Texture2D dieTexture2;
        public Texture2D dieTexture3;
        public UIImageButton rollButton;
        public UIText rollText;
        public UIImage rollResult1;
        public UIImage rollResult2;
        public UIImage rollResult3;
        public bool Rolling = false;
        public int rollTurn = 0;        //0 is player, 1 is NPC
        public int rollCounter = 0;

        public bool gameActive = false;
        public bool won = false;
        public int chosenGame = 0;
        public static bool Visible;
        public int resultCounter = 0;
        public bool buttonsInitialized = false;
        public bool resultsInitialized = false;

        public int pCoins = 0;
        public int gCoins = 0;

        public override void Update(GameTime gameTime)
        {
            if (chosenGame != 0)
            {
                GameActive();
            }
            pCoinText.SetText("Betting: " + pCoins + " Platinum Coins");
            gCoinText.SetText("Betting: " + gCoins + " Gold Coins");
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            Bet = new DragableUIPanel();
            Bet.HAlign = 0.5f;
            Bet.VAlign = 0.5f;
            Bet.Width.Set(470f, 0f);
            Bet.Height.Set(300f, 0f);

            playArea = new UIPanel();
            //playArea.HAlign = 0.8f;
            //playArea.VAlign = 0.5f;
            playArea.Top.Set(10f, 0f);
            playArea.Left.Set(245f, 0f);
            playArea.Width.Set(200f, 0f);
            playArea.Height.Set(260f, 0f);
            Bet.Append(playArea);

            Texture2D playTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            UIImageButton betButton = new UIImageButton(playTexture);
            betButton.Top.Set(260f, 0f);
            betButton.Left.Set(30f, 0f);
            betButton.Width.Set(30f, 0f);
            betButton.Height.Set(30f, 0f);
            betButton.OnClick += new MouseEvent(BetButtonClicked);
            Bet.Append(betButton);

            Texture2D closeTexture = ModContent.GetTexture("Terraria/UI/ButtonDelete");
            UIImageButton closeButton = new UIImageButton(closeTexture);
            closeButton.Top.Set(0f, 0f);
            closeButton.Left.Set(0f, 0f);
            closeButton.Width.Set(20f, 0f);
            closeButton.Height.Set(20f, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            Bet.Append(closeButton);

            Texture2D pCoinTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            UIImageButton pCoinButton = new UIImageButton(pCoinTexture);
            pCoinButton.Top.Set(100f, 0f);
            pCoinButton.Left.Set(5f, 0f);
            pCoinButton.Width.Set(20f, 0f);
            pCoinButton.Height.Set(20f, 0f);
            pCoinButton.OnClick += new MouseEvent(PlatinumCoinButtonClicked);
            Bet.Append(pCoinButton);

            UIImageButton nPCoinButton = new UIImageButton(pCoinTexture);
            nPCoinButton.Top.Set(120f, 0f);
            nPCoinButton.Left.Set(5f, 0f);
            nPCoinButton.Width.Set(20f, 0f);
            nPCoinButton.Height.Set(20f, 0f);
            nPCoinButton.OnClick += new MouseEvent(NPlatinumCoinButtonClicked);
            Bet.Append(nPCoinButton);

            Texture2D gCoinTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            UIImageButton gCoinButton = new UIImageButton(pCoinTexture);
            gCoinButton.Top.Set(50f, 0f);
            gCoinButton.Left.Set(5f, 0f);
            gCoinButton.Width.Set(20f, 0f);
            gCoinButton.Height.Set(20f, 0f);
            gCoinButton.OnClick += new MouseEvent(GoldCoinButtonClicked);
            Bet.Append(gCoinButton);

            UIImageButton nGCoinButton = new UIImageButton(gCoinTexture);
            nGCoinButton.Top.Set(70f, 0f);
            nGCoinButton.Left.Set(5f, 0f);
            nGCoinButton.Width.Set(20f, 0f);
            nGCoinButton.Height.Set(20f, 0f);
            nGCoinButton.OnClick += new MouseEvent(NGoldCoinButtonClicked);
            Bet.Append(nGCoinButton);

            UIText GambleText = new UIText("Gamble");
            GambleText.Top.Set(280f, 0f);
            GambleText.Left.Set(30f, 0f);
            Bet.Append(GambleText);

            pCoinText = new UIText("0");
            pCoinText.Top.Set(110f, 0f);
            pCoinText.Left.Set(30f, 0f);
            Bet.Append(pCoinText);

            gCoinText = new UIText("0");
            gCoinText.Top.Set(60f, 0f);
            gCoinText.Left.Set(30f, 0f);
            Bet.Append(gCoinText);

            Append(Bet);
            base.OnInitialize();
        }

        private void PlatinumCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            Main.PlaySound(SoundID.MenuTick);
            if (player.CountItem(ItemID.PlatinumCoin) > pCoins)
            {
                pCoins += 1;
            }
        }

        private void NPlatinumCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            if (pCoins > 0)
            {
                pCoins -= 1;
            }
        }

        private void GoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            Main.PlaySound(SoundID.MenuTick);
            if (player.CountItem(ItemID.GoldCoin) > gCoins)
            {
                gCoins += 1;
            }
        }

        private void NGoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            if (gCoins > 0)
            {
                gCoins -= 1;
            }
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuClose);
            Visible = false;
        }

        private void BetButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            Main.PlaySound(SoundID.MenuTick);
            if (pCoins != 0 || gCoins != 0)
            {
                chosenGame = /*Main.rand.Next(1, 4)*/ 2;
            }
            else
            {
                Main.NewText("It wouldn't be gambling without some loss and gain.");
            }
        }

        private void RockButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            playerChoice = 1;
        }

        private void PaperButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            playerChoice = 2;
        }

        private void ScissorButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            playerChoice = 3;
        }

        private void RollButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            Rolling = true;
            rollButton.Remove();
        }

        public int roll1;
        public int roll2;
        public int roll3;
        public bool rollsInitialized = false;

        public virtual void GameActive()
        {
            if (chosenGame == 1)
            {
                if (!buttonsInitialized)
                {
                    ReInitialize(1);
                    buttonsInitialized = true;
                }
                if (randomSign == 0)
                {
                    randomSign = Main.rand.Next(1, 3);
                }
                if (playerChoice != 0)      //1 is rock, 2 is paper, 3 is scissor
                {
                    if (!resultsInitialized)
                    {
                        ReInitialize(4);
                        resultsInitialized = true;
                    }
                    if (buttonsInitialized)
                    {
                        rockButton.Remove();
                        paperButton.Remove();
                        scissorsButton.Remove();
                        buttonsInitialized = false;
                    }
                    if ((playerChoice == 1 && randomSign == 2) || (playerChoice == 2 && randomSign == 3) || (playerChoice == 3 && randomSign == 1))
                    {
                        resultCounter++;
                        won = false;
                    }
                    if ((playerChoice == 2 && randomSign == 1) || (playerChoice == 3 && randomSign == 2) || (playerChoice == 1 && randomSign == 3))
                    {
                        resultCounter++;
                        won = true;
                    }
                    if (playerChoice == randomSign)
                    {
                        resultCounter++;
                        if (resultCounter >= 160)
                        {
                            resultCounter = 0;
                            randomSign = 0;
                            playerChoice = 0;
                            playerResult.Remove();
                            npcResult.Remove();
                            resultsInitialized = false;
                        }
                    }
                }
            }
            if (chosenGame == 2)
            {
                if (!buttonsInitialized)
                {
                    ReInitialize(2);
                    buttonsInitialized = true;
                }
                if (Rolling)
                {
                    rollCounter++;
                    dieTexture1 = ModContent.GetTexture("JoJoStands/Extras/Die_" + roll1);
                    dieTexture2 = ModContent.GetTexture("JoJoStands/Extras/Die_" + roll2);
                    dieTexture3 = ModContent.GetTexture("JoJoStands/Extras/Die_" + roll3);
                    rollResult1.Left.Pixels = rollResult1.Left.Pixels + Main.rand.Next(-1, 2);
                    rollResult2.Left.Pixels = rollResult2.Left.Pixels + Main.rand.Next(-1, 2);
                    rollResult3.Left.Pixels = rollResult3.Left.Pixels + Main.rand.Next(-1, 2);
                    rollResult1.Top.Pixels = rollResult1.Top.Pixels + Main.rand.Next(-1, 2);
                    rollResult2.Top.Pixels = rollResult2.Top.Pixels + Main.rand.Next(-1, 2);
                    rollResult3.Top.Pixels = rollResult3.Top.Pixels + Main.rand.Next(-1, 2);
                    roll1 = Main.rand.Next(1, 7);
                    roll2 = Main.rand.Next(1, 7);
                    roll3 = Main.rand.Next(1, 7);
                    if (!rollsInitialized)
                    {
                        ReInitialize(5);
                        rollsInitialized = true;
                    }
                    if (rollTurn == 0)
                    {
                        rollText.SetText("You are Rolling...");
                    }
                    if (rollTurn == 1)
                    {
                        rollText.SetText("D'Arby is Rolling...");
                    }
                    if (rollCounter >= 90)
                    {
                        rollResult1.Left.Pixels = 60f;
                        rollResult2.Left.Pixels = 110f;
                        rollResult3.Left.Pixels = 160f;
                        rollResult1.Top.Pixels = rollResult2.Top.Pixels = rollResult3.Top.Pixels = 104f;
                        Rolling = false;
                        rollText.Remove();
                    }
                }
                if (rollCounter > 89)
                {
                    resultCounter++;
                    if (rollTurn == 0)
                    {
                        if (roll1 == roll2 && roll2 == roll3 && (roll3 == 4 || roll3 == 5 || roll3 == 6))
                        {
                            won = true;
                            GameEnd();
                        }
                        else if ((roll1 + roll2 + roll3) == 6)
                        {
                            won = true;
                            GameEnd();
                        }
                        else if (roll1 > 3 && roll2 > 3 && roll3 > 3)
                        {
                            won = true;
                            GameEnd();
                        }
                        else if (roll1 == roll2 && roll2 == roll3 && (roll3 == 1 || roll3 == 2 || roll3 == 3))
                        {
                            won = false;
                            GameEnd();
                        }
                        else if (roll1 <= 3 && roll2 <= 3 && roll3 <= 3)
                        {
                            won = false;
                            GameEnd();
                        }
                        else
                        {
                            rollTurn = 1;
                            roll1 = 0;
                            roll2 = 0;
                            roll3 = 0;
                            rollCounter = 0;
                            Rolling = true;
                        }
                    }
                    if (rollTurn == 1)
                    {
                        if (roll1 == roll2 && roll2 == roll3 && (roll3 == 4 || roll3 == 5 || roll3 == 6))
                        {
                            won = false;
                            GameEnd();
                        }
                        else if ((roll1 + roll2 + roll3) == 6)
                        {
                            won = false;
                            GameEnd();
                        }
                        else if (roll1 > 3 && roll2 > 3 && roll3 > 3)
                        {
                            won = false;
                            GameEnd();
                        }
                        else if (roll1 == roll2 && roll2 == roll3 && (roll3 == 1 || roll3 == 2 || roll3 == 3))
                        {
                            won = true;
                            GameEnd();
                        }
                        else if (roll1 <= 3 && roll2 <= 3 && roll3 <= 3)
                        {
                            won = true;
                            GameEnd();
                        }
                        else
                        {
                            rollTurn = 0;
                            roll1 = 0;
                            roll2 = 0;
                            roll3 = 0;
                            buttonsInitialized = false;
                            rollCounter = 0;
                        }
                    }
                }
                if (resultCounter >= 160)
                {
                    rollResult1.Remove();
                    rollResult2.Remove();
                    rollResult3.Remove();
                    rollsInitialized = false;
                    resultCounter = 0;
                }
            }
            if (chosenGame == 3)
            {

            }
            if (resultCounter >= 180)
            {
                GameEnd();
            }
        }

        public virtual void GameEnd()
        {
            Player player = Main.player[Main.myPlayer];
            chosenGame = 0;
            playerChoice = 0;
            resultCounter = 0;
            randomSign = 0;
            buttonsInitialized = false;
            resultsInitialized = false;
            roll1 = 0;
            roll2 = 0;
            roll3 = 0;
            rollTurn = 0;
            Rolling = false;

            if (won)
            {
                if (pCoins != 0)
                {
                    Item.NewItem(player.position, ItemID.PlatinumCoin, pCoins * Main.rand.Next(1, 3));
                }
                if (gCoins != 0)
                {
                    Item.NewItem(player.position, ItemID.GoldCoin, gCoins * Main.rand.Next(1, 3));
                }
                Main.NewText("Dang it... No! How is this possible!? Fine, just take the coins, I have much more anyway.");
                pCoins = 0;
                gCoins = 0;

            }
            else
            {
                Visible = false;
                for (int c = 0; c < Main.maxInventory; c++)
                {
                    if (player.inventory[c].type == ItemID.PlatinumCoin)
                    {
                        player.inventory[c].stack -= pCoins;
                    }
                    if (player.inventory[c].type == ItemID.GoldCoin)
                    {
                        player.inventory[c].stack -= gCoins;
                    }
                }
                Main.NewText("Thanks for the coins!");
                pCoins = 0;
                gCoins = 0;
            }

            if (chosenGame == 1)
            {
                playerResult.Remove();
                npcResult.Remove();
            }
            if (chosenGame == 2)
            {
                rollResult1.Remove();
                rollResult2.Remove();
                rollResult3.Remove();
                rollButton.Remove();
                rollText.Remove();
                roll1 = 0;
                roll2 = 0;
                roll3 = 0;
                rollTurn = 0;
                Rolling = false;
            }
            if (chosenGame == 3)
            {

            }
        }

        public void ReInitialize(int set)
        {
            if (set == 1)       //Rock Paper Scissors buttons
            {
                Texture2D rockTexture = ModContent.GetTexture("JoJoStands/Extras/Rock");
                rockButton = new UIImageButton(rockTexture);
                rockButton.HAlign = 0.2f;
                rockButton.VAlign = 0.3f;
                rockButton.Width.Set(30f, 0f);
                rockButton.Height.Set(30f, 0f);
                rockButton.OnClick += new MouseEvent(RockButtonClicked);
                playArea.Append(rockButton);

                Texture2D paperTexture = ModContent.GetTexture("JoJoStands/Extras/Paper");
                paperButton = new UIImageButton(paperTexture);
                paperButton.HAlign = 0.2f;
                paperButton.VAlign = 0.6f;
                paperButton.Width.Set(30f, 0f);
                paperButton.Height.Set(30f, 0f);
                paperButton.OnClick += new MouseEvent(PaperButtonClicked);
                playArea.Append(paperButton);

                Texture2D scissorTexture = ModContent.GetTexture("JoJoStands/Extras/Scissors");
                scissorsButton = new UIImageButton(scissorTexture);
                scissorsButton.HAlign = 0.2f;
                scissorsButton.VAlign = 0.9f;
                scissorsButton.Width.Set(30f, 0f);
                scissorsButton.Height.Set(30f, 0f);
                scissorsButton.OnClick += new MouseEvent(ScissorButtonClicked);
                playArea.Append(scissorsButton);
            }
            else if (set == 2)
            {
                Texture2D playTexture = ModContent.GetTexture("Terraria/UI/ButtonPlay");
                rollButton = new UIImageButton(playTexture);
                rollButton.HAlign = 0.5f;
                rollButton.VAlign = 0.8f;
                rollButton.Width.Set(60f, 0f);
                rollButton.Height.Set(30f, 0f);
                rollButton.OnClick += new MouseEvent(RollButtonClicked);
                playArea.Append(rollButton);

                rollText = new UIText("Roll");
                rollText.HAlign = 0.5f;
                rollText.VAlign = 0.8f;
                playArea.Append(rollText);
            }
            else if (set == 3)
            {

            }
            else if (set == 4)      //Result UIImages of Rock, Paper, Scissors
            {
                Texture2D playerResultTexture = null;       //1 variable cause if not, you'd have to type out much more if's for what each vairable could be
                Texture2D npcResultTexture = null;
                if (playerChoice == 1)
                {
                    playerResultTexture = ModContent.GetTexture("JoJoStands/Extras/Rock_Right");
                }
                if (playerChoice == 2)
                {
                    playerResultTexture = ModContent.GetTexture("JoJoStands/Extras/Paper_Right");
                }
                if (playerChoice == 3)
                {
                    playerResultTexture = ModContent.GetTexture("JoJoStands/Extras/Scissors_Right");
                }

                if (randomSign == 1)
                {
                    npcResultTexture = ModContent.GetTexture("JoJoStands/Extras/Rock_Left");
                }
                if (randomSign == 2)
                {
                    npcResultTexture = ModContent.GetTexture("JoJoStands/Extras/Paper_Left");
                }
                if (randomSign == 3)
                {
                    npcResultTexture = ModContent.GetTexture("JoJoStands/Extras/Scissors_Left");
                }

                if (playerResultTexture != null)
                {
                    playerResult = new UIImage(playerResultTexture);
                    playerResult.HAlign = 0.3f;
                    playerResult.VAlign = 0.5f;
                    playerResult.Width.Set(playerResultTexture.Width, 0f);
                    playerResult.Height.Set(playerResultTexture.Height, 0f);
                    playArea.Append(playerResult);
                }
                if (npcResultTexture != null)
                {
                    npcResult = new UIImage(npcResultTexture);
                    npcResult.HAlign = 0.7f;
                    npcResult.VAlign = 0.5f;
                    npcResult.Width.Set(npcResultTexture.Width, 0f);
                    npcResult.Height.Set(npcResultTexture.Height, 0f);
                    playArea.Append(npcResult);
                }
            }
            else if (set == 5)
            {
                rollResult1 = new UIImage(dieTexture1);
                rollResult1.Top.Set(104f, 0f);
                rollResult1.Left.Set(60f, 0f);
                rollResult1.Height.Set(dieTexture1.Height, 0f);
                rollResult1.Width.Set(dieTexture1.Width, 0f);
                playArea.Append(rollResult1);

                rollResult2 = new UIImage(dieTexture2);
                rollResult2.Top.Set(104f, 0f);
                rollResult2.Left.Set(110f, 0f);
                rollResult2.Height.Set(dieTexture2.Height, 0f);
                rollResult2.Width.Set(dieTexture2.Width, 0f);
                playArea.Append(rollResult2);

                rollResult3 = new UIImage(dieTexture3);
                rollResult3.Top.Set(104f, 0f);
                rollResult3.Left.Set(160f, 0f);
                rollResult3.Height.Set(dieTexture3.Height, 0f);
                rollResult3.Width.Set(dieTexture3.Width, 0f);
                playArea.Append(rollResult3);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            base.DrawSelf(spriteBatch);
        }
    }
}