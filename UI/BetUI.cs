using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class BetUI : UIState      //where you left off, finish adding D'Arby cheating in games and make it noticable, make Game 3
    {
        public DragableUIPanel Bet;
        private UIPanel playArea;
        private UIText pCoinText;
        private UIText gCoinText;
        public static bool Visible;

        private const int Game_RoShamBo = 1;
        private const int Game_RollingGame = 2;
        private const int Game_MiniPoker = 3;

        private Texture2D[] roShamBoHands;
        string[] roShamBoHandPaths = new string[6]
        {
            "JoJoStands/Extras/Rock_Right",
            "JoJoStands/Extras/Paper_Right",
            "JoJoStands/Extras/Scissors_Right",
            "JoJoStands/Extras/Rock_Left",
            "JoJoStands/Extras/Paper_Left",
            "JoJoStands/Extras/Scissors_Left"
        };

        //1st minigame stuff
        private UIImageButton rockButton;
        private UIImageButton paperButton;
        private UIImageButton scissorsButton;
        private UIImage playerResult;
        private UIImage npcResult;
        private int playerChoice = 0;
        private int enemyChoice = 0;

        //2nd minigame stuff
        private Asset<Texture2D>[] dieTextureAssets;
        private UIImageButton rollButton;
        private UIText rollText;
        private UIImage[] dieRollResults;
        private bool rollingDie = false;
        private int rollTurn = 0;        //1 is player, 2 is NPC
        private int rollCounter = 0;
        private int[] rollValues;

        private Asset<Texture2D> playerCardTexture1;
        private Asset<Texture2D> playerCardTexture2;
        private Asset<Texture2D> npcCardTexture1;
        private Asset<Texture2D> npcCardTexture2;
        private UIImageButton drawPile;
        private UIImageButton PlayerCard1Button;
        private UIImageButton PlayerCard2Button;
        private UIImage NPCCard1Image;
        private UIImage NPCCard2Image;
        private UIImageButton ShowCardsButton;
        private int playerCard1;
        private int playerCard2;
        private int NPCCard1;
        private int NPCCard2;
        private bool waitingForCardSwap = false;
        private bool swappedOutCard = false;
        private bool showingCards = false;

        private bool won = false;
        private int chosenGame = 0;
        private int resultCounter = 0;
        private bool buttonsInitialized = false;
        private bool resultsInitialized = false;
        private int cheatChance = 0; //every win, cheat chance goes up by a random number of 2-7
        private bool accuseOfCheating = false;
        private bool cheating = false;

        private int pCoins = 0;
        private int gCoins = 0;

        public override void Update(GameTime gameTime)
        {
            if (chosenGame != 0)
                GameActive();

            pCoinText.SetText("Betting: " + pCoins + " Platinum Coins");
            gCoinText.SetText("Betting: " + gCoins + " Gold Coins");
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            Asset<Texture2D> UpArrow = ModContent.Request<Texture2D>("JoJoStands/Extras/UpArrow", AssetRequestMode.ImmediateLoad);
            Asset<Texture2D> DownArrow = ModContent.Request<Texture2D>("JoJoStands/Extras/DownArrow", AssetRequestMode.ImmediateLoad);
            Asset<Texture2D> playTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay", AssetRequestMode.ImmediateLoad);

            roShamBoHands = new Texture2D[6];
            for (int i = 0; i < 6; i++)
            {
                roShamBoHands[i] = (Texture2D)ModContent.Request<Texture2D>(roShamBoHandPaths[i], AssetRequestMode.ImmediateLoad);
            }

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
            betButton.OnLeftClick += new MouseEvent(BetButtonClicked);
            Bet.Append(betButton);

            Asset<Texture2D> closeTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete", AssetRequestMode.ImmediateLoad);
            UIImageButton closeButton = new UIImageButton(closeTexture);
            closeButton.Top.Set(0f, 0f);
            closeButton.Left.Set(0f, 0f);
            closeButton.Width.Set(22f, 0f);
            closeButton.Height.Set(22f, 0f);
            closeButton.OnLeftClick += new MouseEvent(CloseButtonClicked);
            Bet.Append(closeButton);

            UIImageButton pCoinButton = new UIImageButton(UpArrow);
            pCoinButton.Top.Set(104f, 0f);
            pCoinButton.Left.Set(5f, 0f);
            pCoinButton.Width.Set(22f, 0f);
            pCoinButton.Height.Set(14f, 0f);
            pCoinButton.OnLeftClick += new MouseEvent(PlatinumCoinButtonClicked);
            Bet.Append(pCoinButton);

            UIImageButton nPCoinButton = new UIImageButton(DownArrow);
            nPCoinButton.Top.Set(116f, 0f);
            nPCoinButton.Left.Set(5f, 0f);
            nPCoinButton.Width.Set(22f, 0f);
            nPCoinButton.Height.Set(14f, 0f);
            nPCoinButton.OnLeftClick += new MouseEvent(NPlatinumCoinButtonClicked);
            Bet.Append(nPCoinButton);

            UIImageButton gCoinButton = new UIImageButton(UpArrow);
            gCoinButton.Top.Set(54f, 0f);
            gCoinButton.Left.Set(5f, 0f);
            gCoinButton.Width.Set(22f, 0f);
            gCoinButton.Height.Set(14f, 0f);
            gCoinButton.OnLeftClick += new MouseEvent(GoldCoinButtonClicked);
            Bet.Append(gCoinButton);

            UIImageButton nGCoinButton = new UIImageButton(DownArrow);
            nGCoinButton.Top.Set(66f, 0f);
            nGCoinButton.Left.Set(5f, 0f);
            nGCoinButton.Width.Set(22f, 0f);
            nGCoinButton.Height.Set(14f, 0f);
            nGCoinButton.OnLeftClick += new MouseEvent(NGoldCoinButtonClicked);
            Bet.Append(nGCoinButton);

            UIImageButton accuseButton = new UIImageButton(playTexture);
            accuseButton.Top.Set(150f, 0f);
            accuseButton.Left.Set(210f, 0f);
            accuseButton.Width.Set(playTexture.Value.Width, 0f);
            accuseButton.Height.Set(playTexture.Value.Height, 0f);
            accuseButton.OnLeftClick += new MouseEvent(AccuseButtonClicked);
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
                pCoins += 1;
        }

        private void NPlatinumCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (pCoins > 0 && chosenGame == 0)
                pCoins -= 1;
        }

        private void GoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (player.CountItem(ItemID.GoldCoin) > gCoins && chosenGame == 0)
                gCoins += 1;
        }

        private void NGoldCoinButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (gCoins > 0 && chosenGame == 0)
                gCoins -= 1;
        }

        private void AccuseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (chosenGame != 0)
            {
                accuseOfCheating = true;
                won = cheating;
                if (won)
                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerCheatWin").Value);
                else
                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerCheatLose").Value);
                GameEnd();

            }
            if (chosenGame == 0)
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerNoGameCheat").Value);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuClose);
            if (chosenGame == 0)
                Visible = false;
        }

        private void BetButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            if (pCoins != 0 || gCoins != 0 && chosenGame == 0)
                chosenGame = Main.rand.Next(1, 4);

            else
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerNoBetWarning").Value);

            if (chosenGame != 0)
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerActiveBetWarning").Value);
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
            rollingDie = true;
            rollButton.Remove();
        }

        private void DrawPileClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GambleCardGameHint1").Value);
            if (!swappedOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                waitingForCardSwap = true;
            }
        }

        private void SwapOutCard1(UIMouseEvent evt, UIElement listeningElement)
        {
            if (waitingForCardSwap && !swappedOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                playerCard1 = Main.rand.Next(1, 11);
                playerCardTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard1 + "s" + Main.rand.Next(1, 5));
                PlayerCard1Button.SetImage(playerCardTexture1);
                waitingForCardSwap = false;
                swappedOutCard = true;
            }
        }

        private void SwapOutCard2(UIMouseEvent evt, UIElement listeningElement)
        {
            if (waitingForCardSwap && !swappedOutCard)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                playerCard2 = Main.rand.Next(1, 11);
                playerCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + playerCard2 + "s" + Main.rand.Next(1, 5));
                PlayerCard2Button.SetImage(playerCardTexture2);
                waitingForCardSwap = false;
                swappedOutCard = true;
            }
        }

        private void ShowCardsClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            showingCards = true;
        }


        private void GameActive()
        {
            if (chosenGame == Game_RoShamBo)        //Ro-Sham-Bo game
            {
                if (!buttonsInitialized)
                {
                    ReInitialize(1);
                    buttonsInitialized = true;
                }

                if (playerChoice != 0)      //1 is rock, 2 is paper, 3 is scissor
                {
                    enemyChoice = Main.rand.Next(1, 4);
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
                    if ((playerChoice == 1 && enemyChoice == 2) || (playerChoice == 2 && enemyChoice == 3) || (playerChoice == 3 && enemyChoice == 1))
                    {
                        resultCounter++;
                        won = false;
                    }
                    if ((playerChoice == 2 && enemyChoice == 1) || (playerChoice == 3 && enemyChoice == 2) || (playerChoice == 1 && enemyChoice == 3))
                    {
                        resultCounter++;
                        won = true;
                    }
                    if (playerChoice == enemyChoice)
                    {
                        resultCounter++;
                        if (resultCounter >= 160)
                        {
                            resultCounter = 0;
                            playerChoice = 0;
                            playerResult.Remove();
                            npcResult.Remove();
                            resultsInitialized = false;
                        }
                    }
                }
            }
            else if (chosenGame == Game_RollingGame)        //the rolling game
            {
                for (int i = 0; i < 3; i++)
                {
                    dieTextureAssets[i] = ModContent.Request<Texture2D>("JoJoStands/Extras/Die_" + rollValues[i]);
                }


                if (!buttonsInitialized)
                {
                    ReInitialize(2);
                    buttonsInitialized = true;
                    rollTurn = 1;
                }
                if (rollingDie)
                {
                    rollCounter++;
                    if (resultsInitialized)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            dieRollResults[i].SetImage(dieTextureAssets[i]);
                            dieRollResults[i].Left.Pixels = 20f + (45 * i);
                            dieRollResults[i].Top.Pixels = 104f;
                            rollValues[i] = Main.rand.Next(1, 6 + 1);
                        }
                    }
                    else
                    {
                        ReInitialize(5);
                        resultsInitialized = true;
                    }

                    if (rollTurn == 1)      //if it's your turn
                        rollText.SetText("You are Rolling...");
                    else if (rollTurn == 2)      //if it's D'Arbys turn
                        rollText.SetText("D'Arby is Rolling...");

                    if (rollCounter >= 90)
                    {
                        if (rollTurn == 2 && Main.rand.Next(0, 100 + 1) <= cheatChance)
                        {
                            cheating = true;
                            for (int i = 0; i < 3; i++)
                            {
                                rollValues[i] = 6;
                                dieTextureAssets[i] = ModContent.Request<Texture2D>("JoJoStands/Extras/WeightedDie");
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            dieRollResults[i].Left.Pixels = 20f + (45 * i);
                            dieRollResults[i].Top.Pixels = 104f;
                        }
                        rollingDie = false;
                        rollText.SetText("");
                    }
                }
                if (rollCounter == 90)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        dieRollResults[i].SetImage(dieTextureAssets[i]);
                    }
                    resultCounter++;
                    if (rollTurn == 1 && resultCounter >= 160)
                    {
                        if (rollValues[0] == rollValues[1] && rollValues[1] == rollValues[2] && (rollValues[2] == 4 || rollValues[2] == 5 || rollValues[2] == 6))
                            won = true;
                        else if ((rollValues[0] + rollValues[1] + rollValues[2]) == 6)
                            won = true;
                        else if (rollValues[0] > 3 && rollValues[1] > 3 && rollValues[2] > 3)
                            won = true;
                        else if (rollValues[0] == rollValues[1] && rollValues[1] == rollValues[2] && (rollValues[2] == 1 || rollValues[2] == 2 || rollValues[2] == 3))
                            won = false;
                        else if (rollValues[0] <= 3 && rollValues[1] <= 3 && rollValues[2] <= 3)
                            won = false;
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                dieRollResults[i].Remove();
                            }
                            resultCounter = 0;
                            rollTurn = 2;
                            rollValues[0] = 0;
                            rollValues[1] = 0;
                            rollValues[2] = 0;
                            rollCounter = 0;
                            resultCounter = 0;
                            rollingDie = true;
                            resultsInitialized = false;
                        }
                    }
                    if (rollTurn == 2 && resultCounter >= 160)
                    {
                        if (rollValues[0] == rollValues[1] && rollValues[1] == rollValues[2] && (rollValues[2] == 4 || rollValues[2] == 5 || rollValues[2] == 6))
                            won = false;
                        else if ((rollValues[0] + rollValues[1] + rollValues[2]) == 6)
                            won = false;
                        else if (rollValues[0] > 3 && rollValues[1] > 3 && rollValues[2] > 3)
                            won = false;
                        else if (rollValues[0] == rollValues[1] && rollValues[1] == rollValues[2] && (rollValues[2] == 1 || rollValues[2] == 2 || rollValues[2] == 3))
                            won = true;
                        else if (rollValues[0] <= 3 && rollValues[1] <= 3 && rollValues[2] <= 3)
                            won = true;
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                dieRollResults[i].Remove();
                            }
                            rollValues[0] = 0;
                            rollValues[1] = 0;
                            rollValues[2] = 0;
                            buttonsInitialized = false;
                            rollCounter = 0;
                            resultsInitialized = false;
                            resultCounter = 0;
                        }
                    }
                }
            }
            else if (chosenGame == Game_MiniPoker)        //the mini-poker game
            {
                if (!buttonsInitialized)
                {
                    playerCard1 = Main.rand.Next(1, 11);
                    playerCard2 = Main.rand.Next(1, 11);
                    NPCCard1 = Main.rand.Next(1, 11);
                    NPCCard2 = Main.rand.Next(1, 11);
                    if (Main.rand.Next(0, 100 + 1) <= cheatChance)
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
                        if (cheating)
                            npcCardTexture1 = npcCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/cheatCard_" + Main.rand.Next(1, 5));
                        else
                        {
                            npcCardTexture1 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + NPCCard1 + "s" + Main.rand.Next(1, 5));
                            npcCardTexture2 = ModContent.Request<Texture2D>("JoJoStands/Extras/card_" + NPCCard2 + "s" + Main.rand.Next(1, 5));
                        }
                        NPCCard1Image.SetImage(npcCardTexture1);
                        NPCCard2Image.SetImage(npcCardTexture2);
                    }
                    if ((playerCard1 + playerCard2) > (NPCCard1 + NPCCard2))
                        won = true;
                    else if ((playerCard1 + playerCard2) < (NPCCard1 + NPCCard2))
                        won = false;
                    else
                    {
                        resultCounter++;
                        if (resultCounter >= 160)
                            resultCounter = 0;
                    }
                }
            }
            if (resultCounter >= 180)
            {
                if (!accuseOfCheating && cheating)
                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerCheatHint").Value);

                GameEnd();
            }
        }

        private void GameEnd()
        {
            Player player = Main.player[Main.myPlayer];
            playerChoice = 0;
            resultCounter = 0;
            enemyChoice = 0;

            if (won)
            {
                cheatChance += Main.rand.Next(2, 8);
                int multiplier = 1;
                if (accuseOfCheating && cheating)
                    multiplier = 2;

                if (pCoins != 0)
                    player.QuickSpawnItem(null, ItemID.PlatinumCoin, pCoins * multiplier);

                if (gCoins != 0)
                    player.QuickSpawnItem(null, ItemID.GoldCoin, gCoins * multiplier);

                if (!cheating)
                    Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerBetLose").Value);
                pCoins = 0;
                gCoins = 0;

            }
            else
            {
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.GamblerBetWin").Value);
                for (int c = 0; c < Main.InventorySlotsTotal; c++)
                {
                    if (player.inventory[c].type == ItemID.PlatinumCoin)
                        player.inventory[c].stack -= pCoins;
                    if (player.inventory[c].type == ItemID.GoldCoin)
                        player.inventory[c].stack -= gCoins;
                }
                pCoins = 0;
                gCoins = 0;
            }
            if (cheating)
            {
                cheatChance = 0;
                cheating = false;
            }
            accuseOfCheating = false;

            if (chosenGame == Game_RoShamBo)
            {
                if (resultsInitialized)
                {
                    playerResult.Remove();
                    npcResult.Remove();
                    resultsInitialized = false;
                }
            }
            else if (chosenGame == Game_RollingGame)
            {
                if (buttonsInitialized)
                {
                    rollButton.Remove();
                    rollText.Remove();
                    buttonsInitialized = false;
                }
                if (resultsInitialized)
                {
                    resultsInitialized = false;
                    for (int i = 0; i < 3; i++)
                    {
                        dieRollResults[i].Remove();
                    }
                }
                rollValues[0] = 0;
                rollValues[1] = 0;
                rollValues[2] = 0;
                rollTurn = 0;
                rollingDie = false;
                rollCounter = 0;
            }
            else if (chosenGame == Game_MiniPoker)
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
                waitingForCardSwap = false;
                showingCards = false;
                swappedOutCard = false;
            }
            chosenGame = 0;
        }

        private void ReInitialize(int set)
        {
            if (set == Game_RoShamBo)       //Rock Paper Scissors buttons
            {
                Asset<Texture2D> rockTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Rock", AssetRequestMode.ImmediateLoad);
                rockButton = new UIImageButton(rockTexture);
                rockButton.HAlign = 0.2f;
                rockButton.VAlign = 0.3f;
                rockButton.Width.Set(30f, 0f);
                rockButton.Height.Set(30f, 0f);
                rockButton.OnLeftClick += new MouseEvent(RockButtonClicked);
                playArea.Append(rockButton);

                Asset<Texture2D> paperTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Paper", AssetRequestMode.ImmediateLoad);
                paperButton = new UIImageButton(paperTexture);
                paperButton.HAlign = 0.2f;
                paperButton.VAlign = 0.6f;
                paperButton.Width.Set(30f, 0f);
                paperButton.Height.Set(30f, 0f);
                paperButton.OnLeftClick += new MouseEvent(PaperButtonClicked);
                playArea.Append(paperButton);

                Asset<Texture2D> scissorTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Scissors", AssetRequestMode.ImmediateLoad);
                scissorsButton = new UIImageButton(scissorTexture);
                scissorsButton.HAlign = 0.2f;
                scissorsButton.VAlign = 0.9f;
                scissorsButton.Width.Set(30f, 0f);
                scissorsButton.Height.Set(30f, 0f);
                scissorsButton.OnLeftClick += new MouseEvent(ScissorButtonClicked);
                playArea.Append(scissorsButton);
            }
            else if (set == Game_RollingGame)      //Roll button
            {
                Asset<Texture2D> playTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay", AssetRequestMode.ImmediateLoad);
                rollButton = new UIImageButton(playTexture);
                rollButton.HAlign = 0.5f;
                rollButton.VAlign = 0.8f;
                rollButton.Width.Set(60f, 0f);
                rollButton.Height.Set(30f, 0f);
                rollButton.OnLeftClick += new MouseEvent(RollButtonClicked);
                playArea.Append(rollButton);

                rollText = new UIText("Roll");
                rollText.HAlign = 0.58f;
                rollText.VAlign = 0.76f;
                playArea.Append(rollText);
            }
            else if (set == Game_MiniPoker)
            {
                drawPile = new UIImageButton(ModContent.Request<Texture2D>("JoJoStands/Extras/card_back", AssetRequestMode.ImmediateLoad));
                drawPile.HAlign = 0.9f;
                drawPile.VAlign = 0.4f;
                drawPile.Height.Set(58f, 0f);
                drawPile.Width.Set(42f, 0f);
                drawPile.SetVisibility(1f, 1f);
                drawPile.OnLeftClick += new MouseEvent(DrawPileClicked);
                playArea.Append(drawPile);

                PlayerCard1Button = new UIImageButton(playerCardTexture1);
                PlayerCard1Button.HAlign = 0.15f;
                PlayerCard1Button.VAlign = 0.7f;
                PlayerCard1Button.Height.Set(playerCardTexture1.Value.Height, 0f);
                PlayerCard1Button.Width.Set(playerCardTexture1.Value.Width, 0f);
                PlayerCard1Button.SetVisibility(1f, 1f);
                PlayerCard1Button.OnLeftClick += new MouseEvent(SwapOutCard1);
                playArea.Append(PlayerCard1Button);

                PlayerCard2Button = new UIImageButton(playerCardTexture2);
                PlayerCard2Button.HAlign = 0.55f;
                PlayerCard2Button.VAlign = 0.7f;
                PlayerCard2Button.Height.Set(playerCardTexture2.Value.Height, 0f);
                PlayerCard2Button.Width.Set(playerCardTexture2.Value.Width, 0f);
                PlayerCard2Button.SetVisibility(1f, 1f);
                PlayerCard2Button.OnLeftClick += new MouseEvent(SwapOutCard2);
                playArea.Append(PlayerCard2Button);

                NPCCard1Image = new UIImage(npcCardTexture1);
                NPCCard1Image.HAlign = 0.15f;
                NPCCard1Image.VAlign = 0.3f;
                NPCCard1Image.Height.Set(npcCardTexture1.Value.Height, 0f);
                NPCCard1Image.Width.Set(npcCardTexture1.Value.Width, 0f);
                playArea.Append(NPCCard1Image);

                NPCCard2Image = new UIImage(npcCardTexture2);
                NPCCard2Image.HAlign = 0.55f;
                NPCCard2Image.VAlign = 0.3f;
                NPCCard2Image.Height.Set(npcCardTexture2.Value.Height, 0f);
                NPCCard2Image.Width.Set(npcCardTexture2.Value.Width, 0f);
                playArea.Append(NPCCard2Image);

                ShowCardsButton = new UIImageButton(ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonPlay", AssetRequestMode.ImmediateLoad));
                ShowCardsButton.HAlign = 0.9f;
                ShowCardsButton.VAlign = 0.65f;
                ShowCardsButton.Height.Set(30f, 0f);
                ShowCardsButton.Width.Set(30f, 0f);
                ShowCardsButton.OnLeftClick += new MouseEvent(ShowCardsClicked);
                playArea.Append(ShowCardsButton);
            }
            else if (set == 4)      //Result UIImages of Rock, Paper, Scissors
            {
                Texture2D playerResultTexture = roShamBoHands[playerChoice - 1];       //1 variable cause if not, you'd have to type out much more if's for what each vairable could be
                Texture2D npcResultTexture = roShamBoHands[enemyChoice - 1 + 3];

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
                for (int i = 0; i < 3; i++)
                {
                    dieRollResults[i] = new UIImage(dieTextureAssets[i]);
                    dieRollResults[i].Left.Set(20f + (45 * i), 0f);
                    dieRollResults[i].Top.Set(104f, 0f);
                    dieRollResults[i].Height.Set(dieTextureAssets[i].Value.Height, 0f);
                    dieRollResults[i].Width.Set(dieTextureAssets[i].Value.Width, 0f);
                    playArea.Append(dieRollResults[i]);
                }
            }
        }
    }
}