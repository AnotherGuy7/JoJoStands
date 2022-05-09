using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class BetUI : UIState      //where you left off, finish adding D'Arby cheating in games and make it noticable, make Game 3
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
        public int rollTurn = 0;        //1 is player, 2 is NPC
        public int rollCounter = 0;
        public int roll1;
        public int roll2;
        public int roll3;

        public Texture2D playerCardTexture1;
        public Texture2D playerCardTexture2;
        public Texture2D npcCardTexture1;
        public Texture2D npcCardTexture2;
        public UIImageButton drawPile;
        public UIImageButton PlayerCard1Button;
        public UIImageButton PlayerCard2Button;
        public UIImage NPCCard1Image;
        public UIImage NPCCard2Image;
        public UIImageButton ShowCardsButton;
        public int playerCard1;
        public int playerCard2;
        public int NPCCard1;
        public int NPCCard2;
        public bool waitingForCardSwap = false;
        public bool swappeOutCard = false;
        public bool showingCards = false;

        public bool won = false;
        public int chosenGame = 0;
        public static bool Visible;
        public int resultCounter = 0;
        public bool buttonsInitialized = false;
        public bool resultsInitialized = false;
        public int cheatChance = 0; //every win, cheat chance goes up by a random number of 2-7
        public bool accuseOfCheating = false;
        public static bool cheating = false;

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
            Texture2D UpArrow = ModContent.Request<Texture2D>("JoJoStands/Extras/UpArrow");
            Texture2D DownArrow = ModContent.Request<Texture2D>("JoJoStands/Extras/DownArrow");
            Texture2D playTexture = ModContent.Request<Texture2D>("Terraria/UI/ButtonPlay");

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

            UIImageButton betButton = new UIImageButton(playTexture);
            betButton.Top.Set(260f, 0f);
            betButton.Left.Set(30f, 0f);
            betButton.Width.Set(22f, 0f);
            betButton.Height.Set(22f, 0f);
            betButton.OnClick += new MouseEvent(BetButtonClicked);
            Bet.Append(betButton);

            Texture2D closeTexture = ModContent.Request<Texture2D>("Terraria/UI/ButtonDelete");
            UIImageButton closeButton = new UIImageButton(closeTexture);
            closeButton.Top.Set(0f, 0f);
            closeButton.Left.Set(0f, 0f);
            closeButton.Width.Set(22f, 0f);
            closeButton.Height.Set(22f, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            Bet.Append(closeButton);

            UIImageButton pCoinButton = new UIImageButton(UpArrow);
            pCoinButton.Top.Set(104f, 0f);
            pCoinButton.Left.Set(5f, 0f);
            pCoinButton.Width.Set(22f, 0f);
            pCoinButton.Height.Set(14f, 0f);
            pCoinButton.OnClick += new MouseEvent(PlatinumCoinButtonClicked);
            Bet.Append(pCoinButton);

            UIImageButton nPCoinButton = new UIImageButton(DownArrow);
            nPCoinButton.Top.Set(116f, 0f);
            nPCoinButton.Left.Set(5f, 0f);
            nPCoinButton.Width.Set(22f, 0f);
            nPCoinButton.Height.Set(14f, 0f);
            nPCoinButton.OnClick += new MouseEvent(NPlatinumCoinButtonClicked);
            Bet.Append(nPCoinButton);

            UIImageButton gCoinButton = new UIImageButton(UpArrow);
            gCoinButton.Top.Set(54f, 0f);
            gCoinButton.Left.Set(5f, 0f);
            gCoinButton.Width.Set(22f, 0f);
            gCoinButton.Height.Set(14f, 0f);
            gCoinButton.OnClick += new MouseEvent(GoldCoinButtonClicked);
            Bet.Append(gCoinButton);

            UIImageButton nGCoinButton = new UIImageButton(DownArrow);
            nGCoinButton.Top.Set(66f, 0f);
            nGCoinButton.Left.Set(5f, 0f);
            nGCoinButton.Width.Set(22f, 0f);
            nGCoinButton.Height.Set(14f, 0f);
            nGCoinButton.OnClick += new MouseEvent(NGoldCoinButtonClicked);
            Bet.Append(nGCoinButton);

            UIImageButton accuseButton = new UIImageButton(playTexture);
            accuseButton.Top.Set(150f, 0f);
            accuseButton.Left.Set(210f, 0f);
            accuseButton.Width.Set(playTexture.Width, 0f);
            accuseButton.Height.Set(playTexture.Height, 0f);
            accuseButton.OnClick += new MouseEvent(AccuseButtonClicked);
            Bet.Append(accuseButton);


            UIText GambleText = new UIText("Gamble");
            GambleText.Top.Set(265f, 0f);
            GambleText.Left.Set(60f, 0f);
            Bet.Append(GambleText);

            pCoinText = new UIText("0");
            pCoinText.Top.Set(110f, 0f);
            pCoinText.Left.Set(30f, 0f);
            Bet.Append(pCoinText);

            gCoinText = new UIText("0");
            gCoinText.Top.Set(60f, 0f);
            gCoinText.Left.Set(30f, 0f);
            Bet.Append(gCoinText);

            UIText AccuseText = new UIText("Accuse of Cheating");
            AccuseText.Top.Set(150f, 0f);
            AccuseText.Left.Set(50f, 0f);
            Bet.Append(AccuseText);

            Append(Bet);
            base.OnInitialize();
        }

        private void PlatinumCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (player.CountItem(ItemID.PlatinumCoin) > pCoins && chosenGame == 0)
            {
                pCoins += 1;
            }
        }

        private void NPlatinumCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (pCoins > 0 && chosenGame == 0)
            {
                pCoins -= 1;
            }
        }

        private void GoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (player.CountItem(ItemID.GoldCoin) > gCoins && chosenGame == 0)
            {
                gCoins += 1;
            }
        }

        private void NGoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (gCoins > 0 && chosenGame == 0)
            {
                gCoins -= 1;
            }
        }

        private void AccuseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (chosenGame != 0)
            {
                accuseOfCheating = true;
                if (cheating)
                {
                    won = true;
                    Main.NewText("*D'Arby is sweating a lot and is looking like very, very pale...\nPlease don't tell Jotaro! I'll give you 2x the money!");

                }
                if (!cheating)
                {
                    won = false;
                    Main.NewText("I'm no cheater! You're accusing the me of the past! I'm a whole new gambler now!");
                }
                GameEnd();

            }
            if (chosenGame == 0)
            {
                Main.NewText("What am I cheating at? We're not even betting!");
            }
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            if (chosenGame == 0)
            {
                Visible = false;
            }
        }

        private void BetButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (pCoins != 0 || gCoins != 0 && chosenGame == 0)
            {
                chosenGame = Main.rand.Next(1, 4);
            }
            else
            {
                Main.NewText("It wouldn't be gambling without some loss and gain.");
            }
            if (chosenGame != 0)
            {
                Main.NewText("We are already betting... Can't you see that?");
            }
        }

        private void RockButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            playerChoice = 1;
        }

        private void PaperButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            playerChoice = 2;
        }

        private void ScissorButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            playerChoice = 3;
        }

        private void RollButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            Rolling = true;
            rollButton.Remove();
        }

        private void DrawPileClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.NewText("Click the card you'd like to switch out");
            if (!swappeOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                waitingForCardSwap = true;
            }
        }

        private void SwapOutCard1(UIMouseEvent evt, UIElement listeningElement)
        {
            if (waitingForCardSwap && !swappeOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                playerCard1 = Main.rand.Next(1, 11);
                playerCardTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard1 + "s" + Main.rand.Next(1, 5));
                PlayerCard1Button.SetImage(playerCardTexture1);
                waitingForCardSwap = false;
                swappeOutCard = true;
            }
        }

        private void SwapOutCard2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (waitingForCardSwap && !swappeOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                playerCard2 = Main.rand.Next(1, 11);
                playerCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard2 + "s" + Main.rand.Next(1, 5));
                PlayerCard2Button.SetImage(playerCardTexture2);
                waitingForCardSwap = false;
                swappeOutCard = true;
            }
        }

        private void ShowCardsClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            showingCards = true;
        }


        private void GameActive()
        {
            if (chosenGame == 1)        //Ro-Sham-Bo game
            {
                if (!buttonsInitialized)
                {
                    ReInitialize(1);
                    buttonsInitialized = true;
                }
                if (randomSign == 0)
                {
                    randomSign = Main.rand.Next(1, 4);
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
            if (chosenGame == 2)        //the rolling game
            {
                dieTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/Die_" + roll1);
                dieTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/Die_" + roll2);
                dieTexture3 = ModContent.Request<Texture2D>("JoJoStands/Extras/Die_" + roll3);

                if (!buttonsInitialized)
                {
                    ReInitialize(2);
                    buttonsInitialized = true;
                    rollTurn = 1;
                }
                if (Rolling)
                {
                    rollCounter++;
                    if (!resultsInitialized)
                    {
                        ReInitialize(5);
                        resultsInitialized = true;
                    }
                    if (resultsInitialized)
                    {
                        rollResult1.SetImage(dieTexture1);
                        rollResult2.SetImage(dieTexture2);
                        rollResult3.SetImage(dieTexture3);

                        rollResult1.Left.Pixels += Main.rand.Next(-1, 2);
                        rollResult2.Left.Pixels += Main.rand.Next(-1, 2);
                        rollResult3.Left.Pixels += Main.rand.Next(-1, 2);
                        rollResult1.Top.Pixels += Main.rand.Next(-1, 2);
                        rollResult2.Top.Pixels += Main.rand.Next(-1, 2);
                        rollResult3.Top.Pixels += Main.rand.Next(-1, 2);

                        roll1 = Main.rand.Next(1, 7);
                        roll2 = Main.rand.Next(1, 7);
                        roll3 = Main.rand.Next(1, 7);

                        rollResult1.Left.Pixels = 20f;
                        rollResult2.Left.Pixels = 70f;
                        rollResult3.Left.Pixels = 110f;
                        rollResult1.Top.Pixels = rollResult2.Top.Pixels = rollResult3.Top.Pixels = 104f;
                    }
                    if (rollTurn == 1)      //if it's your turn
                    {
                        rollText.SetText("You are Rolling...");
                    }
                    if (rollTurn == 2)      //if it's D'Arbys turn
                    {
                        rollText.SetText("D'Arby is Rolling...");
                    }
                    if (rollCounter >= 90)
                    {
                        if (rollTurn == 2 && Main.rand.Next(0, 100) <= cheatChance)
                        {
                            cheating = true;
                            roll1 = 6;
                            roll2 = 6;
                            roll3 = 6;
                            dieTexture1 = dieTexture2 = dieTexture3 = ModContent.Request<Texture2D>("JoJoStands/Extras/WeightedDie");
                        }
                        rollResult1.Left.Pixels = 20f;
                        rollResult2.Left.Pixels = 70f;
                        rollResult3.Left.Pixels = 110f;
                        rollResult1.Top.Pixels = rollResult2.Top.Pixels = rollResult3.Top.Pixels = 104f;
                        Rolling = false;
                        rollText.SetText("");
                    }
                }
                if (rollCounter == 90)
                {
                    rollResult1.SetImage(dieTexture1);      //so that it doesn't stay stuck at the previous numbers
                    rollResult2.SetImage(dieTexture2);
                    rollResult3.SetImage(dieTexture3);

                    resultCounter++;
                    if (rollTurn == 1 && resultCounter >= 160)
                    {
                        if (roll1 == roll2 && roll2 == roll3 && (roll3 == 4 || roll3 == 5 || roll3 == 6))
                        {
                            won = true;
                        }
                        else if ((roll1 + roll2 + roll3) == 6)
                        {
                            won = true;
                        }
                        else if (roll1 > 3 && roll2 > 3 && roll3 > 3)
                        {
                            won = true;
                        }
                        else if (roll1 == roll2 && roll2 == roll3 && (roll3 == 1 || roll3 == 2 || roll3 == 3))
                        {
                            won = false;
                        }
                        else if (roll1 <= 3 && roll2 <= 3 && roll3 <= 3)
                        {
                            won = false;
                        }
                        else
                        {
                            rollResult1.Remove();
                            rollResult2.Remove();
                            rollResult3.Remove();
                            resultCounter = 0;
                            rollTurn = 2;
                            roll1 = 0;
                            roll2 = 0;
                            roll3 = 0;
                            rollCounter = 0;
                            resultCounter = 0;
                            Rolling = true;
                            resultsInitialized = false;
                        }
                    }
                    if (rollTurn == 2 && resultCounter >= 160)
                    {
                        if (roll1 == roll2 && roll2 == roll3 && (roll3 == 4 || roll3 == 5 || roll3 == 6))
                        {
                            won = false;
                        }
                        else if ((roll1 + roll2 + roll3) == 6)
                        {
                            won = false;
                        }
                        else if (roll1 > 3 && roll2 > 3 && roll3 > 3)
                        {
                            won = false;
                        }
                        else if (roll1 == roll2 && roll2 == roll3 && (roll3 == 1 || roll3 == 2 || roll3 == 3))
                        {
                            won = true;
                        }
                        else if (roll1 <= 3 && roll2 <= 3 && roll3 <= 3)
                        {
                            won = true;
                        }
                        else
                        {
                            rollResult1.Remove();
                            rollResult2.Remove();
                            rollResult3.Remove();
                            roll1 = 0;
                            roll2 = 0;
                            roll3 = 0;
                            buttonsInitialized = false;
                            rollCounter = 0;
                            resultsInitialized = false;
                            resultCounter = 0;
                        }
                    }
                }
            }
            if (chosenGame == 3)        //the mini-poker game
            {
                if (!buttonsInitialized)
                {
                    playerCard1 = Main.rand.Next(1, 11);
                    playerCard2 = Main.rand.Next(1, 11);
                    NPCCard1 = Main.rand.Next(1, 11);
                    NPCCard2 = Main.rand.Next(1, 11);
                    if (Main.rand.Next(0, 100) <= cheatChance)
                    {
                        cheating = true;
                        NPCCard1 = NPCCard2 = 10;
                    }
                    playerCardTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard1 + "s" + Main.rand.Next(1, 5));
                    playerCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard2 + "s" + Main.rand.Next(1, 5));
                    npcCardTexture1 = npcCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_back");
                    ReInitialize(3);
                    buttonsInitialized = true;
                }

                if (showingCards)
                {
                    resultCounter++;
                    if (npcCardTexture1 == ModContent.Request<Texture2D>("JoJoStands/Extras/card_back"))
                    {
                        if (!cheating)
                        {
                            npcCardTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + NPCCard1 + "s" + Main.rand.Next(1, 5));
                            npcCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + NPCCard2 + "s" + Main.rand.Next(1, 5));
                        }
                        if (cheating)
                        {
                            npcCardTexture1 = npcCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/cheatCard_" + Main.rand.Next(1, 5));
                        }
                        NPCCard1Image.SetImage(npcCardTexture1);
                        NPCCard2Image.SetImage(npcCardTexture2);
                    }
                    if ((playerCard1 + playerCard2) > (NPCCard1 + NPCCard2))
                    {
                        won = true;
                    }
                    if ((playerCard1 + playerCard2) < (NPCCard1 + NPCCard2))
                    {
                        won = false;
                    }
                    if ((playerCard1 + playerCard2) == (NPCCard1 + NPCCard2))
                    {
                        resultCounter++;
                        if (resultCounter >= 160)
                        {
                            resultCounter = 0;
                        }
                    }
                }
            }
            if (resultCounter >= 180)
            {
                if (!accuseOfCheating && cheating)
                {
                    Main.NewText("*D'Arby smiles...");
                }
                GameEnd();
            }
        }

        private void GameEnd()
        {
            Player player = Main.player[Main.myPlayer];
            playerChoice = 0;
            resultCounter = 0;
            randomSign = 0;

            if (won)
            {
                cheatChance += Main.rand.Next(2, 8);
                if (pCoins != 0)
                {
                    //Item.NewItem(player.position, ItemID.PlatinumCoin, pCoins);
                    player.QuickSpawnItem(ItemID.PlatinumCoin, pCoins);
                }
                if (gCoins != 0)
                {
                    //Item.NewItem(player.position, ItemID.GoldCoin, gCoins);
                    player.QuickSpawnItem(ItemID.GoldCoin, gCoins);

                }
                if (accuseOfCheating && cheating)
                {
                    if (pCoins != 0)
                    {
                        //Item.NewItem(player.position, ItemID.PlatinumCoin, pCoins * 2);
                        player.QuickSpawnItem(ItemID.PlatinumCoin, pCoins * 2);
                    }
                    if (gCoins != 0)
                    {
                        //Item.NewItem(player.position, ItemID.GoldCoin, gCoins * 2);
                        player.QuickSpawnItem(ItemID.GoldCoin, gCoins * 2);
                    }
                }
                if (!cheating)
                {
                    Main.NewText("Dang it... No! How is this possible!? Fine, just take the coins, I have much more anyway.");
                }
                pCoins = 0;
                gCoins = 0;

            }
            else
            {
                for (int c = 0; c < Main.InventorySlotsTotal; c++)
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
            if (cheating)
            {
                cheatChance = 0;
                cheating = false;
            }
            if (accuseOfCheating)
            {
                accuseOfCheating = false;
            }

            if (chosenGame == 1)
            {
                if (resultsInitialized)
                {
                    playerResult.Remove();
                    npcResult.Remove();
                    resultsInitialized = false;
                }
                chosenGame = 0;     //here in each one so that chosenGame doesn't turn 0 first
            }
            if (chosenGame == 2)
            {
                if (buttonsInitialized)
                {
                    rollButton.Remove();
                    rollText.Remove();
                    buttonsInitialized = false;
                }
                if (resultsInitialized)
                {
                    rollResult1.Remove();
                    rollResult2.Remove();
                    rollResult3.Remove();
                    resultsInitialized = false;
                }
                roll1 = 0;
                roll2 = 0;
                roll3 = 0;
                rollTurn = 0;
                Rolling = false;
                chosenGame = 0;
                rollCounter = 0;
            }
            if (chosenGame == 3)
            {
                if (buttonsInitialized)
                {
                    PlayerCard1Button.Remove();
                    PlayerCard2Button.Remove();
                    NPCCard1Image.Remove();
                    NPCCard2Image.Remove();
                    ShowCardsButton.Remove();
                    drawPile.Remove();
                    buttonsInitialized = false;
                }
                playerCard1 = 0;
                playerCard2 = 0;
                NPCCard1 = 0;
                NPCCard2 = 0;
                chosenGame = 0;
                waitingForCardSwap = false;
                showingCards = false;
                swappeOutCard = false;
            }
        }

        public void ReInitialize(int set)
        {
            if (set == 1)       //Rock Paper Scissors buttons
            {
                Texture2D rockTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Rock");
                rockButton = new UIImageButton(rockTexture);
                rockButton.HAlign = 0.2f;
                rockButton.VAlign = 0.3f;
                rockButton.Width.Set(30f, 0f);
                rockButton.Height.Set(30f, 0f);
                rockButton.OnClick += new MouseEvent(RockButtonClicked);
                playArea.Append(rockButton);

                Texture2D paperTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Paper");
                paperButton = new UIImageButton(paperTexture);
                paperButton.HAlign = 0.2f;
                paperButton.VAlign = 0.6f;
                paperButton.Width.Set(30f, 0f);
                paperButton.Height.Set(30f, 0f);
                paperButton.OnClick += new MouseEvent(PaperButtonClicked);
                playArea.Append(paperButton);

                Texture2D scissorTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Scissors");
                scissorsButton = new UIImageButton(scissorTexture);
                scissorsButton.HAlign = 0.2f;
                scissorsButton.VAlign = 0.9f;
                scissorsButton.Width.Set(30f, 0f);
                scissorsButton.Height.Set(30f, 0f);
                scissorsButton.OnClick += new MouseEvent(ScissorButtonClicked);
                playArea.Append(scissorsButton);
            }
            else if (set == 2)      //Roll button
            {
                Texture2D playTexture = ModContent.Request<Texture2D>("Terraria/UI/ButtonPlay");
                rollButton = new UIImageButton(playTexture);
                rollButton.HAlign = 0.5f;
                rollButton.VAlign = 0.8f;
                rollButton.Width.Set(60f, 0f);
                rollButton.Height.Set(30f, 0f);
                rollButton.OnClick += new MouseEvent(RollButtonClicked);
                playArea.Append(rollButton);

                rollText = new UIText("Roll");
                rollText.HAlign = 0.58f;
                rollText.VAlign = 0.76f;
                playArea.Append(rollText);
            }
            else if (set == 3)
            {
                drawPile = new UIImageButton(ModContent.Request<Texture2D>("JoJoStands/Extras/card_back"));
                drawPile.HAlign = 0.9f;
                drawPile.VAlign = 0.4f;
                drawPile.Height.Set(58f, 0f);
                drawPile.Width.Set(42f, 0f);
                drawPile.SetVisibility(1f, 1f);
                drawPile.OnClick += new MouseEvent(DrawPileClicked);
                playArea.Append(drawPile);

                PlayerCard1Button = new UIImageButton(playerCardTexture1);
                PlayerCard1Button.HAlign = 0.15f;
                PlayerCard1Button.VAlign = 0.7f;
                PlayerCard1Button.Height.Set(playerCardTexture1.Height, 0f);
                PlayerCard1Button.Width.Set(playerCardTexture1.Width, 0f);
                PlayerCard1Button.SetVisibility(1f, 1f);
                PlayerCard1Button.OnClick += new MouseEvent(SwapOutCard1);
                playArea.Append(PlayerCard1Button);

                PlayerCard2Button = new UIImageButton(playerCardTexture2);
                PlayerCard2Button.HAlign = 0.55f;
                PlayerCard2Button.VAlign = 0.7f;
                PlayerCard2Button.Height.Set(playerCardTexture2.Height, 0f);
                PlayerCard2Button.Width.Set(playerCardTexture2.Width, 0f);
                PlayerCard2Button.SetVisibility(1f, 1f);
                PlayerCard2Button.OnClick += new MouseEvent(SwapOutCard2);
                playArea.Append(PlayerCard2Button);

                NPCCard1Image = new UIImage(npcCardTexture1);
                NPCCard1Image.HAlign = 0.15f;
                NPCCard1Image.VAlign = 0.3f;
                NPCCard1Image.Height.Set(npcCardTexture1.Height, 0f);
                NPCCard1Image.Width.Set(npcCardTexture1.Width, 0f);
                playArea.Append(NPCCard1Image);

                NPCCard2Image = new UIImage(npcCardTexture2);
                NPCCard2Image.HAlign = 0.55f;
                NPCCard2Image.VAlign = 0.3f;
                NPCCard2Image.Height.Set(npcCardTexture2.Height, 0f);
                NPCCard2Image.Width.Set(npcCardTexture2.Width, 0f);
                playArea.Append(NPCCard2Image);

                ShowCardsButton = new UIImageButton(ModContent.Request<Texture2D>("Terraria/UI/ButtonPlay"));
                ShowCardsButton.HAlign = 0.9f;
                ShowCardsButton.VAlign = 0.65f;
                ShowCardsButton.Height.Set(30f, 0f);
                ShowCardsButton.Width.Set(30f, 0f);
                ShowCardsButton.OnClick += new MouseEvent(ShowCardsClicked);
                playArea.Append(ShowCardsButton);
            }
            else if (set == 4)      //Result UIImages of Rock, Paper, Scissors
            {
                Texture2D playerResultTexture = null;       //1 variable cause if not, you'd have to type out much more if's for what each vairable could be
                Texture2D npcResultTexture = null;
                if (playerChoice == 1)
                {
                    playerResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Rock_Right");
                }
                if (playerChoice == 2)
                {
                    playerResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Paper_Right");
                }
                if (playerChoice == 3)
                {
                    playerResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Scissors_Right");
                }

                if (randomSign == 1)
                {
                    npcResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Rock_Left");
                }
                if (randomSign == 2)
                {
                    npcResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Paper_Left");
                }
                if (randomSign == 3)
                {
                    npcResultTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Scissors_Left");
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
            else if (set == 5)      //Rolling results
            {
                rollResult1 = new UIImage(dieTexture1);
                rollResult1.Top.Set(104f, 0f);
                rollResult1.Left.Set(20f, 0f);
                rollResult1.Height.Set(dieTexture1.Height, 0f);
                rollResult1.Width.Set(dieTexture1.Width, 0f);
                playArea.Append(rollResult1);

                rollResult2 = new UIImage(dieTexture2);
                rollResult2.Top.Set(104f, 0f);
                rollResult2.Left.Set(70f, 0f);
                rollResult2.Height.Set(dieTexture2.Height, 0f);
                rollResult2.Width.Set(dieTexture2.Width, 0f);
                playArea.Append(rollResult2);

                rollResult3 = new UIImage(dieTexture3);
                rollResult3.Top.Set(104f, 0f);
                rollResult3.Left.Set(110f, 0f);
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