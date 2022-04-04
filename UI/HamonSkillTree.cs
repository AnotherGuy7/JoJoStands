using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class HamonSkillTree : UIState
    {
        public UIPanel HamonSkillTreePanel;
        public static bool Visible;
        //public UIText hamonSkillTooltip;
        private MouseTextPanel hamonSkillTooltips;
        private UIText hamonSkillPointsText;
        private UIImageButton hamonSkillTreeUpArrow;
        private UIImageButton hamonSkillTreeDownArrow;
        private UIImageButton hamonSkillTreeXButton;
        private Texture2D[] skillTreeTextures = new Texture2D[3];
        private const int MaxUIPages = 3;
        private bool[] pageLockBosses = new bool[5] { NPC.downedBoss1, NPC.downedBoss3, Main.hardMode, NPC.downedMechBossAny, NPC.downedPlantBoss };
        private string[] pageLockMessages = new string[5] { "The next page is unlocked Post-Eye of Cthulu.", "The next page is unlocked Post-Skeletron.", "The next page is unlocked Post-Wall of Flesh.", "The next page is unlocked Post-Any Mech boss.", "The next page is unlocked Post-Plantera." };


        private int currentShownPage = 1;
        private const float UIScale = 1f;
        private AdjustableButton[] hamonSkillIcons = new AdjustableButton[10];
        private Vector2[] hamonSkillIconPositions = new Vector2[10];
        private Texture2D[] hamonSkillIconImages = new Texture2D[10];
        private string[] hamonSkillIconTooltips = new string[10];
        private int[] affectedSkillSlotIndexes = new int[HamonPlayer.HamonSkillsLimit];      //This is an array of indexes for the slot that goes true
        private int[] affectedSkillSlotHamonRequired = new int[HamonPlayer.HamonSkillsLimit];        //An array of values that change the amount of Hamon the skill needs
        private int[] affectedSkillSlotHamonSkillLevel = new int[HamonPlayer.HamonSkillsLimit];       //An array of values that changes the skill level (for upgrading skills and stuff)
        private bool[] slotsToLock = new bool[10];
        private bool[] unobtainableSlots = new bool[10];
        private Texture2D unknownSkillTexture;

        public static void OpenHamonSkillTree()
        {
            Visible = true;
            JoJoStands.Instance.HamonSkillTreeUI.InitializeButtons(1);
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            hamonSkillTooltips.visible = false;
            for (int i = 0; i < hamonSkillIcons.Length; i++)
            {
                AdjustableButton icon = hamonSkillIcons[i];

                if (icon.invisible)
                {
                    icon.drawAlpha = 0f;
                    continue;
                }

                if (icon.lockedInFocus)
                {
                    icon.drawAlpha = 1f;
                    icon.drawColor = Color.Yellow;
                }
                if (unobtainableSlots[i])
                    icon.drawColor = Color.Red;
                if (icon.IsMouseHovering)
                {
                    hamonSkillTooltips.ShowText(hamonSkillIconTooltips[i]);
                    hamonSkillTooltips.ownerPos = new Vector2(Main.screenWidth * HamonSkillTreePanel.HAlign, Main.screenHeight * HamonSkillTreePanel.VAlign) - new Vector2(HamonSkillTreePanel.Width.Pixels / 2f, HamonSkillTreePanel.Height.Pixels / 2f);      //On screen changes, it had to update
                }
            }
            hamonSkillPointsText.SetText("SP: " + hamonPlayer.skillPointsAvailable);

            /*KeyboardState keyboardState = Keyboard.GetState();     //Code for finding button positions (Just control it to where you want it and remember the coordiantes)

             if (keyboardState.IsKeyDown(Keys.Up))
             {
                 hamonSkillIcons[1].Top.Pixels -= 1;
             }
             if (keyboardState.IsKeyDown(Keys.Down))
             {
                 hamonSkillIcons[1].Top.Pixels += 1;
             }
             if (keyboardState.IsKeyDown(Keys.Left))
             {
                 hamonSkillIcons[1].Left.Pixels -= 1;
             }
             if (keyboardState.IsKeyDown(Keys.Right))
             {
                 hamonSkillIcons[1].Left.Pixels += 1;
             }
             Main.NewText("X: " + hamonSkillIcons[1].Left.Pixels + "; Y:" + hamonSkillIcons[1].Top.Pixels);
             Main.NewText("X: " + hamonSkillIcons[1].Left.Pixels + "; Y:" + hamonSkillIcons[1].Top.Pixels);*/

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            HamonSkillTreePanel = new UIPanel();
            HamonSkillTreePanel.HAlign = 0.5f;
            HamonSkillTreePanel.VAlign = 0.5f;
            SetElementSize(HamonSkillTreePanel, new Vector2(428f, 352f));
            HamonSkillTreePanel.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            HamonSkillTreePanel.BorderColor = new Color(0, 0, 0, 0);

            /*hamonSkillTooltip = new UIText("Click on any button to view its info.");
            SetElementSize(hamonSkillTooltip, new Vector2(102f, 38f));
            SetElementPosition(hamonSkillTooltip, new Vector2(186f, 178f));
            HamonSkillTreePanel.Append(hamonSkillTooltip);*/

            hamonSkillPointsText = new UIText("Skill Points Available: ");
            SetElementSize(hamonSkillPointsText, new Vector2(102f, 38f));
            SetElementPosition(hamonSkillPointsText, new Vector2(0f, 295f));
            HamonSkillTreePanel.Append(hamonSkillPointsText);

            hamonSkillTreeUpArrow = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/HamonTreeUpArrow"));
            SetElementSize(hamonSkillTreeUpArrow, new Vector2(32f, 32f));
            SetElementPosition(hamonSkillTreeUpArrow, new Vector2(10f, 10f));
            hamonSkillTreeUpArrow.OnClick += OnClickHamonSkillTreeUpArrow;
            HamonSkillTreePanel.Append(hamonSkillTreeUpArrow);

            hamonSkillTreeDownArrow = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/HamonTreeDownArrow"));
            SetElementSize(hamonSkillTreeDownArrow, new Vector2(32f, 32f));
            SetElementPosition(hamonSkillTreeDownArrow, new Vector2(10f, 46f));
            hamonSkillTreeDownArrow.OnClick += OnClickHamonSkillTreeDownArrow;
            HamonSkillTreePanel.Append(hamonSkillTreeDownArrow);

            hamonSkillTreeXButton = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/HamonTreeXButton"));
            SetElementSize(hamonSkillTreeXButton, new Vector2(32f, 32f));
            SetElementPosition(hamonSkillTreeXButton, new Vector2(360f, 6f));
            hamonSkillTreeXButton.OnClick += OnClickHamonSkillTreeXButton;
            HamonSkillTreePanel.Append(hamonSkillTreeXButton);

            skillTreeTextures[0] = ModContent.GetTexture("JoJoStands/UI/HamonSkillTree_Page1");
            skillTreeTextures[1] = ModContent.GetTexture("JoJoStands/UI/HamonSkillTree_Page2");
            skillTreeTextures[2] = ModContent.GetTexture("JoJoStands/UI/HamonSkillTree_Page3");
            unknownSkillTexture = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_Unknown");

            for (int b = 0; b < hamonSkillIcons.Length; b++)
            {
                hamonSkillIcons[b] = new AdjustableButton(ModContent.GetTexture("JoJoStands/Extras/HamonIcon_Empty"), Vector2.Zero, new Vector2(30f, 30f), Color.White, true);
                SetElementSize(hamonSkillIcons[b], new Vector2(30f, 30f));
                hamonSkillIcons[b].OnClick += OnClickAnyIcon;        //This ties *all* of the buttons to the same method
                hamonSkillIcons[b].OnDoubleClick += OnDoubleClickAnyIcon;
                hamonSkillIcons[b].owner = HamonSkillTreePanel;
                HamonSkillTreePanel.Append(hamonSkillIcons[b]);
            }
            InitializeButtons(1);

            hamonSkillTooltips = new MouseTextPanel(256, 1, "Test Desc");       //Has to be at the end so it doesn't interfere with other button collisions
            hamonSkillTooltips.ownerPos = new Vector2(Main.screenWidth * HamonSkillTreePanel.HAlign, Main.screenHeight * HamonSkillTreePanel.VAlign) - new Vector2(HamonSkillTreePanel.Width.Pixels / 2f, HamonSkillTreePanel.Height.Pixels / 2f);
            HamonSkillTreePanel.Append(hamonSkillTooltips);

            Append(HamonSkillTreePanel);
        }

        private void OnClickHamonSkillTreeUpArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            pageLockBosses = new bool[5] { NPC.downedBoss1, NPC.downedBoss3, Main.hardMode, NPC.downedMechBossAny, NPC.downedPlantBoss };
            bool pageUnlocked = pageLockBosses[currentShownPage - 1];       //This works since pages are from 1 to max and the array is in 0 to max - 1, meaning currentShownPage gets the next page info
            if (pageUnlocked)
            {
                currentShownPage++;
                if (currentShownPage > MaxUIPages)
                {
                    currentShownPage = MaxUIPages;
                }
            }
            else
            {
                Main.NewText(pageLockMessages[currentShownPage - 1]);
            }
            InitializeButtons(currentShownPage);
        }

        private void OnClickHamonSkillTreeDownArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            currentShownPage--;
            Main.PlaySound(SoundID.MenuTick);
            if (currentShownPage <= 0)
            {
                currentShownPage = 1;
            }
            InitializeButtons(currentShownPage);
        }

        private void OnClickHamonSkillTreeXButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Visible = false;
            currentShownPage = 1;
            InitializeButtons(1);
        }

        private void OnClickAnyIcon(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;

            for (int b = 0; b < hamonSkillIcons.Length; b++)
            {
                if (listeningElement == hamonSkillIcons[b])
                {
                    buttonIndex = b;
                    buttonClickedOn = hamonSkillIcons[b];
                }
                if (hamonSkillIcons[b].focusedOn)
                {
                    hamonSkillIcons[b].focusedOn = false;
                }
            }

            if (buttonClickedOn == null)
            {
                Main.NewText("Something went wrong with the Hamon Skill Tree.", Color.Red);
                Visible = false;
                return;
            }
            if (buttonClickedOn.invisible)
            {
                return;
            }

            Main.PlaySound(SoundID.MenuTick);
            buttonClickedOn.focusedOn = true;
            //hamonSkillTooltip.SetText(hamonSkillIconTooltips[buttonIndex]);
        }

        private void OnDoubleClickAnyIcon(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            for (int b = 0; b < hamonSkillIcons.Length; b++)
            {
                if (listeningElement == hamonSkillIcons[b])
                {
                    buttonIndex = b;
                    buttonClickedOn = hamonSkillIcons[b];
                }
                if (hamonSkillIcons[b].focusedOn)
                {
                    hamonSkillIcons[b].focusedOn = false;
                }
            }

            if (buttonClickedOn == null)
            {
                Main.NewText("Something went wrong with the Hamon Skill Tree.", Color.Red);
                Visible = false;
                return;
            }
            if (buttonClickedOn.invisible)
            {
                return;
            }

            if (affectedSkillSlotIndexes[buttonIndex] == -1)
            {
                return;
            }

            Main.PlaySound(SoundID.MenuTick);
            if (hamonPlayer.skillPointsAvailable > 0)
            {
                if (!hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[buttonIndex]] || hamonPlayer.hamonSkillLevels[affectedSkillSlotIndexes[buttonIndex]] < affectedSkillSlotHamonSkillLevel[buttonIndex])      //To prevent ability degrading if it can happen
                {
                    buttonClickedOn.focusedOn = true;
                    buttonClickedOn.lockedInFocus = true;
                    hamonPlayer.learnedAnyAbility = true;
                    hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[buttonIndex]] = true;
                    hamonPlayer.hamonSkillLevels[affectedSkillSlotIndexes[buttonIndex]] = affectedSkillSlotHamonSkillLevel[buttonIndex];
                    if (affectedSkillSlotHamonRequired[buttonIndex] != 0)
                    {
                        hamonPlayer.hamonAmountRequirements[affectedSkillSlotIndexes[buttonIndex]] = affectedSkillSlotHamonRequired[buttonIndex];
                    }
                    hamonPlayer.skillPointsAvailable -= 1;
                    //hamonSkillTooltip.SetText(hamonSkillIconTooltips[buttonIndex]);
                    Main.NewText("Skill Obtained!", Color.Yellow);
                }
            }
            else if (hamonPlayer.skillPointsAvailable <= 0)
            {
                Main.NewText("You don't have any skill points! (Get more by using Sun Droplets)", Color.Yellow);
            }
            else if (hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[buttonIndex]])
            {
                Main.NewText("You've already obtained this skill before!", Color.Yellow);
            }
            InitializeButtons(currentShownPage);
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            spriteBatch.Draw(skillTreeTextures[currentShownPage - 1], HamonSkillTreePanel.GetClippingRectangle(spriteBatch), Color.White);
        }

        private void InitializeButtons(int page)
        {
            for (int b = 0; b < hamonSkillIcons.Length; b++)
            {
                hamonSkillIcons[b].SetImage(ModContent.GetTexture("JoJoStands/Extras/HamonIcon_Empty"));
                hamonSkillIcons[b].invisible = false;
                hamonSkillIcons[b].lockedInFocus = false;
                slotsToLock[b] = false;
                unobtainableSlots[b] = false;
                SetElementPosition(hamonSkillIcons[b], Vector2.Zero);
            }
            for (int i = 0; i < hamonSkillIcons.Length; i++)
            {
                AdjustableButton icon = hamonSkillIcons[i];
                if (icon.lockedInFocus)
                {
                    icon.drawAlpha = 1f;
                    icon.drawColor = Color.Yellow;
                }
                else
                {
                    icon.drawAlpha = 0.8f;
                    icon.drawColor = Color.White;
                }
            }

            switch (page)
            {
                case 1:
                    SetElementPosition(hamonSkillIcons[0], new Vector2(184f, 238f));
                    hamonSkillIconTooltips[0] = "Learn to control your Hamon.";
                    hamonSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_0");
                    hamonSkillIcons[0].SetImage(hamonSkillIconImages[0]);
                    hamonSkillIcons[0].lockedInFocus = true;        //This is because Hamon is always already learned

                    SetElementPosition(hamonSkillIcons[1], new Vector2(153f, 202f));
                    hamonSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_8");
                    hamonSkillIcons[1].SetImage(hamonSkillIconImages[1]);
                    hamonSkillIconTooltips[1] = "Your Hamon disperses itself into the living things around you.\nWhile Hamon is past 15, walking on grass grows flowers. Press & hold UP to use a leaf glider while falling.";
                    affectedSkillSlotIndexes[1] = HamonPlayer.HamonHerbalGrowth;
                    affectedSkillSlotHamonRequired[1] = 15;
                    affectedSkillSlotHamonSkillLevel[1] = 1;
                    CheckForIconAbilityUnlocked(1);

                    SetElementPosition(hamonSkillIcons[2], new Vector2(215f, 202f));
                    hamonSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_4");
                    hamonSkillIcons[2].SetImage(hamonSkillIconImages[2]);
                    hamonSkillIconTooltips[2] = "Your Hamon automatically focuses itself enough to help you keep your balance.\nWhen your Hamon is past 40, you will be able to water walk.";
                    affectedSkillSlotIndexes[2] = HamonPlayer.WaterWalkingSKill;
                    affectedSkillSlotHamonRequired[2] = 40;
                    affectedSkillSlotHamonSkillLevel[2] = 1;
                    CheckForIconAbilityUnlocked(2);

                    SetElementPosition(hamonSkillIcons[3], new Vector2(184f, 202f));
                    hamonSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_1");
                    hamonSkillIcons[3].SetImage(hamonSkillIconImages[3]);
                    hamonSkillIconTooltips[3] = "The Hamon you breathe in focuses itself to heal your body.\nSlightly increased Life Regen when Hamon Breathing.";
                    affectedSkillSlotIndexes[3] = HamonPlayer.BreathingRegenSkill;
                    affectedSkillSlotHamonSkillLevel[3] = 1;
                    CheckForIconAbilityUnlocked(3);

                    SetElementPosition(hamonSkillIcons[4], new Vector2(160f, 99f));
                    hamonSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_2");
                    hamonSkillIcons[4].SetImage(hamonSkillIconImages[4]);
                    hamonSkillIconTooltips[4] = "Your Hamon can now be injected into weapons.\nWhen your Hamon is past 60, double-tap Special while holding a melee weapon to imbue it with Hamon.";
                    affectedSkillSlotIndexes[4] = HamonPlayer.WeaponsHamonImbueSkill;
                    affectedSkillSlotHamonRequired[4] = 60;
                    affectedSkillSlotHamonSkillLevel[4] = 1;
                    CheckForIconAbilityUnlocked(4);
                    CheckForIconLock(4, 3);

                    SetElementPosition(hamonSkillIcons[5], new Vector2(208f, 99f));
                    hamonSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_1");
                    hamonSkillIcons[5].SetImage(hamonSkillIconImages[5]);
                    hamonSkillIconTooltips[5] = "You discovered you can use your Hamon to heal yourself.\nHolding Right-click while holding the Hamon item for 4 seconds will heal you.";
                    affectedSkillSlotIndexes[5] = HamonPlayer.HamonItemHealing;
                    affectedSkillSlotHamonRequired[5] = 10;
                    affectedSkillSlotHamonSkillLevel[5] = 1;
                    CheckForIconAbilityUnlocked(5);
                    CheckForIconLock(5, 3);

                    CheckForAlternateAbilityUnlokced(4, 5);
                    CheckForAlternateAbilityUnlokced(5, 4);

                    SetUnusedIconsInvisible(5);
                    break;
                case 2:
                    //Slot 0: 64, 142 (Bottom-left)
                    //Slot 1: 64, 98 (On top of bottom-left)
                    //Slot 2: 64, 64 (Left intersection)
                    //Slot 3: 85, 33 (Left almost done)
                    //Slot 4: 120, 33 (End intersection)
                    //Slot 5: 154, 33 (Right almost done)
                    //Slot 6: 175, 64 (One on top of bottom-right)
                    //Slot 7: 175, 106 (Bottom-right)

                    SetElementPosition(hamonSkillIcons[0], new Vector2(160f, 242f));
                    hamonSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_5");
                    hamonSkillIcons[0].SetImage(hamonSkillIconImages[0]);
                    hamonSkillIconTooltips[0] = "Your Hamon discharges into shockwaves upon heavy impact with the ground.\nWhile Hamon is past 30, when you land on the ground while holding DOWN, a hamon shockwave blasts through the ground.";
                    affectedSkillSlotIndexes[0] = HamonPlayer.HamonShockwave;
                    affectedSkillSlotHamonRequired[0] = 30;
                    affectedSkillSlotHamonSkillLevel[0] = 1;
                    hamonSkillIcons[0].lockedInFocus = false;
                    CheckForIconAbilityUnlocked(0);
                    CheckForSpecificAbiltyUnlocked(0, HamonPlayer.WeaponsHamonImbueSkill);

                    SetElementPosition(hamonSkillIcons[1], new Vector2(160f, 157f));
                    hamonSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_2");
                    hamonSkillIcons[1].SetImage(hamonSkillIconImages[1]);
                    hamonSkillIconTooltips[1] = "Further mastery of injecting Hamon into your weapons leads to better abilities.\nWhen your Hamon is past 40, double-tap Special while holding a melee weapon to imbue it with Hamon.";
                    affectedSkillSlotIndexes[1] = HamonPlayer.WeaponsHamonImbueSkill;
                    affectedSkillSlotHamonRequired[1] = 40;
                    affectedSkillSlotHamonSkillLevel[1] = 2;
                    CheckForIconAbilityUnlocked(1, 2);
                    CheckForIconLock(1, 0);

                    SetElementPosition(hamonSkillIcons[2], new Vector2(183f, 120f));
                    hamonSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_7");
                    hamonSkillIcons[2].SetImage(hamonSkillIconImages[2]);
                    hamonSkillIconTooltips[2] = "A bit of your life for some Hamon may not be as bad as it seems.\nWhen your Hamon is under 30 and your health is above 25%, double-tap Special while holding nothing to consume 5% of health for 30 Hamon.";
                    affectedSkillSlotIndexes[2] = HamonPlayer.HamonOvercharge;
                    affectedSkillSlotHamonRequired[2] = 30;
                    affectedSkillSlotHamonSkillLevel[2] = 1;
                    CheckForIconAbilityUnlocked(2);

                    SetElementPosition(hamonSkillIcons[3], new Vector2(183f, 64f));
                    hamonSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_0");
                    hamonSkillIcons[3].SetImage(hamonSkillIconImages[3]);
                    hamonSkillIconTooltips[3] = "Enough usage of your Hamon has led you to increased efficiency with it.\nPassive Hamon Regen speed slightly increased.";
                    affectedSkillSlotIndexes[3] = HamonPlayer.PassiveHamonRegenBoost;
                    affectedSkillSlotHamonSkillLevel[3] = 1;
                    CheckForIconAbilityUnlocked(3);

                    SetElementPosition(hamonSkillIcons[4], new Vector2(209f, 157f));
                    hamonSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_1");
                    hamonSkillIcons[4].SetImage(hamonSkillIconImages[4]);
                    hamonSkillIconTooltips[4] = "Further use of Hamon Healing has lead to greater efficiency with it.\nHolding Right-click while holding the Hamon item for 2 seconds will heal you.";
                    affectedSkillSlotIndexes[4] = HamonPlayer.HamonItemHealing;
                    affectedSkillSlotHamonRequired[4] = 6;
                    affectedSkillSlotHamonSkillLevel[4] = 2;
                    CheckForIconAbilityUnlocked(4, 2);

                    SetElementPosition(hamonSkillIcons[5], new Vector2(209f, 242f));
                    hamonSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_6");
                    hamonSkillIcons[5].SetImage(hamonSkillIconImages[5]);
                    hamonSkillIconTooltips[5] = "Your Hamon is capable of protecting you even from the most dangerous attacks.\nWhile Hamon is greater than 45, double-tap DOWN to enable your defensive aura. Incoming attack damage is reduced by 5% (Consumes 3 Hamon on hit)";
                    affectedSkillSlotIndexes[5] = HamonPlayer.DefensiveHamonAura;
                    affectedSkillSlotHamonRequired[5] = 45;
                    affectedSkillSlotHamonSkillLevel[5] = 1;
                    CheckForIconAbilityUnlocked(5);
                    CheckForSpecificAbiltyUnlocked(5, HamonPlayer.HamonItemHealing);

                    CheckForIconLock(4, 5);     //This is because 4 isn't initialized yet if it's called where it normally is
                    CheckForIconLock(3, 2);
                    if (!CheckForIconLock(2, 1, true))
                    {
                        CheckForIconLock(2, 4);
                    }

                    SetUnusedIconsInvisible(5);
                    break;
                case 3:
                    SetElementPosition(hamonSkillIcons[0], new Vector2(186f, 279f));
                    hamonSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_11");
                    hamonSkillIcons[0].SetImage(hamonSkillIconImages[0]);
                    hamonSkillIconTooltips[0] = "Sending thin waves of Hamon into the ground allows you to detect differences in content.\nWhile your Hamon is past 8, hold DOWN for two seconds to see nearby ores within 16 tiles.";
                    affectedSkillSlotIndexes[0] = HamonPlayer.OreDetection;
                    affectedSkillSlotHamonRequired[0] = 15;
                    affectedSkillSlotHamonSkillLevel[0] = 1;
                    hamonSkillIcons[0].lockedInFocus = false;
                    CheckForIconAbilityUnlocked(0);
                    CheckForSpecificAbiltyUnlocked(0, HamonPlayer.PassiveHamonRegenBoost);

                    SetElementPosition(hamonSkillIcons[1], new Vector2(186f, 225f));
                    hamonSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_9");
                    hamonSkillIcons[1].SetImage(hamonSkillIconImages[1]);
                    hamonSkillIconTooltips[1] = "You learned that Hamon Breathing and focusing on your body can nullify and remove poisons.\nHamon Breathing decreases the duration time of Poison and other debuffs.";
                    affectedSkillSlotIndexes[1] = HamonPlayer.PoisonCancellation;
                    affectedSkillSlotHamonRequired[1] = 0;
                    affectedSkillSlotHamonSkillLevel[1] = 1;
                    CheckForIconAbilityUnlocked(1);
                    CheckForIconLock(1, 0);

                    SetElementPosition(hamonSkillIcons[2], new Vector2(186f, 178f));
                    hamonSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_1");
                    hamonSkillIcons[2].SetImage(hamonSkillIconImages[2]);
                    hamonSkillIconTooltips[2] = "The Hamon you create while breathing in focuses itself to heal your body.\nGreatly increased Life Regen when Hamon Breathing.";
                    affectedSkillSlotIndexes[2] = HamonPlayer.BreathingRegenSkill;
                    affectedSkillSlotHamonSkillLevel[2] = 2;
                    CheckForIconAbilityUnlocked(2, 2);
                    CheckForIconLock(2, 1);

                    SetElementPosition(hamonSkillIcons[3], new Vector2(162f, 126f));
                    hamonSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_13");
                    hamonSkillIcons[3].SetImage(hamonSkillIconImages[3]);
                    hamonSkillIconTooltips[3] = "Dispersing small amounts of Hamon into the air reveals what's around you.\nPress and hold Second Special while standing still to release a small wave of Hamon and tag enemies. Tagged enemies are highlighted in yellow and receive 15% more damage from Hamon Sources.\nConsumes 30 Hamon.";
                    affectedSkillSlotIndexes[3] = HamonPlayer.SunTag;
                    affectedSkillSlotHamonRequired[3] = 30;
                    affectedSkillSlotHamonSkillLevel[3] = 1;
                    CheckForIconAbilityUnlocked(3);
                    CheckForIconLock(3, 2);

                    SetElementPosition(hamonSkillIcons[4], new Vector2(203f, 126f));
                    hamonSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_12");
                    hamonSkillIcons[4].SetImage(hamonSkillIconImages[4]);
                    hamonSkillIconTooltips[4] = "Focusing on entities around you allows you to create connections with them which slow them down.\nPress and hold Second Special while standing still to release a shackle in the direction of the cursor. Hit enemies with the shackle to tie the shackle to them. Shackled enemies do 15% less damage to you.\nConsumes 45 Hamon.";
                    affectedSkillSlotIndexes[4] = HamonPlayer.SunShackles;
                    affectedSkillSlotHamonRequired[4] = 45;
                    affectedSkillSlotHamonSkillLevel[4] = 1;
                    CheckForIconAbilityUnlocked(4);
                    CheckForIconLock(4, 2);

                    SetElementPosition(hamonSkillIcons[5], new Vector2(186f, 69f));
                    hamonSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_14");
                    hamonSkillIcons[5].SetImage(hamonSkillIconImages[5]);
                    hamonSkillIconTooltips[5] = "Channelling your Hamon into yourself allows you to increase muscle efficiency.\nWhile you have more than 30 Hamon, hold LEFT and RIGHT for two seconds to get Hamon Charged II.\nConsumes 30 Hamon.";
                    affectedSkillSlotIndexes[5] = HamonPlayer.MuscleOverdrive;
                    affectedSkillSlotHamonRequired[5] = 30;
                    affectedSkillSlotHamonSkillLevel[5] = 1;
                    CheckForIconAbilityUnlocked(5);
                    if (!CheckForIconLock(5, 3, true))
                    {
                        CheckForIconLock(5, 4);
                    }

                    //Loc 3 = 186, 279
                    //Loc 4 = 162, 126
                    //Loc 5 = 203, 126
                    //Loc 6 = 142, 101
                    //Loc 7 = 186, 69

                    CheckForAlternateAbilityUnlokced(3, 4);
                    CheckForAlternateAbilityUnlokced(4, 3);
                    SetUnusedIconsInvisible(5);
                    break;
            }

            for (int b = 0; b < hamonSkillIcons.Length; b++)       //Done this way because if affectedSkillSlotIndexes changes to -1 while another icon needs to check it, it makes a broken icon chain
            {
                if (slotsToLock[b])
                {
                    if (unobtainableSlots[b])
                    {
                        hamonSkillIconTooltips[b] = "This skill cannot be unlocked because the alternate skill has already been unlocked.";
                        affectedSkillSlotIndexes[b] = -1;
                        continue;
                    }

                    hamonSkillIcons[b].SetImage(unknownSkillTexture);
                    hamonSkillIconTooltips[b] = "Unlock the skill before this skill to be able to see this skill.";
                    affectedSkillSlotIndexes[b] = -1;
                }
                hamonSkillIcons[b].UpdateButtonPosition();
            }

            //Text wrapping
            if (!Main.gameMenu)
            {
                string[] stringSplitters = new string[1] { " " };
                for (int tooltipIndex = 0; tooltipIndex < hamonSkillIconTooltips.Length; tooltipIndex++)
                {
                    if (hamonSkillIconTooltips[tooltipIndex] != null && hamonSkillIconTooltips[tooltipIndex].Length != 0)
                    {
                        hamonSkillIconTooltips[tooltipIndex] = WrapText(hamonSkillIconTooltips[tooltipIndex], 26, stringSplitters);
                    }
                }
            }
        }

        private void SetElementPosition(UIElement element, Vector2 position)
        {
            element.Left.Pixels = position.X * UIScale;
            element.Top.Pixels = position.Y * UIScale;
            element.HAlign = 0f;
            element.VAlign = 0f;
            element.Left.Percent = 0f;
            element.Top.Percent = 0f;
        }

        private void SetElementSize(UIElement element, Vector2 size)
        {
            element.Width.Pixels = size.X * UIScale;
            element.Height.Pixels = size.Y * UIScale;
        }

        /// <summary>
        /// Checks if the ability in the icon is unlocked.
        /// </summary>
        /// <returns></returns>
        private bool CheckForIconAbilityUnlocked(int iconIndex, int minimumLevel = 1)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return false;

            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            bool learnedAbility = hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[iconIndex]];
            bool minimumLevelMet = hamonPlayer.hamonSkillLevels[affectedSkillSlotIndexes[iconIndex]] >= minimumLevel;
            if (learnedAbility && minimumLevelMet)
            {
                hamonSkillIcons[iconIndex].lockedInFocus = true;
            }

            return learnedAbility && minimumLevelMet;
        }

        /// <summary>
        /// Checks if the specified icon should be marked as "Unknown"
        /// </summary>
        /// <returns></returns>
        private bool CheckForIconLock(int iconIndex, int dependentIconIndex, bool chained = false, bool debug = false)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return false;

            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            bool learnedAbility = false;
            bool minimumLevelMet = false;
            if (debug)
                Main.NewText(hamonPlayer.learnedHamonSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]) + "; " + affectedSkillSlotIndexes[dependentIconIndex]);

            if (hamonPlayer.learnedHamonSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]))
            {
                if (debug)
                    Main.NewText(iconIndex + "; Run");
                learnedAbility = hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[dependentIconIndex]];
                minimumLevelMet = hamonPlayer.hamonSkillLevels[affectedSkillSlotIndexes[dependentIconIndex]] >= affectedSkillSlotHamonSkillLevel[dependentIconIndex];
                if (!learnedAbility || !minimumLevelMet)
                {
                    if (debug)
                        Main.NewText(iconIndex + "; " + learnedAbility + "; " + minimumLevelMet);

                    if (chained)
                        return false;

                    slotsToLock[iconIndex] = true;
                }
            }
            
            if (debug)
                Main.NewText(iconIndex + "; " + (learnedAbility && minimumLevelMet));
            return learnedAbility && minimumLevelMet;
        }

        /// <summary>
        /// A method for finding if any abilty has been unlocked. Useful for finding if the perk in the previous page was unlocked.
        /// </summary>
        /// <param name="iconIndex">Index of he icon that may be locked.</param>
        /// <param name="abilityIndex">Index of the ability being checked for.</param>
        /// <param name="minimumLevel">Minimum level of the skill needed in order to not lock the icon.</param>
        /// <returns></returns>
        private bool CheckForSpecificAbiltyUnlocked(int iconIndex, int abilityIndex, int minimumLevel = 1)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return false;

            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            bool learnedAbility = hamonPlayer.learnedHamonSkills[abilityIndex];
            bool minimumLevelMet = hamonPlayer.hamonSkillLevels[abilityIndex] >= minimumLevel;
            if (!learnedAbility || !minimumLevelMet)
            {
                slotsToLock[iconIndex] = true;
            }

            return learnedAbility && minimumLevelMet;
        }

        /// <summary>
        /// Checks if an alternate version of that ability is unlocked. Locks this one if the other is unlocked. Useful for alternate abilities like the Sun Tag vs the Sun Shackles.
        /// </summary>
        /// <returns>Whether or not the icon was locked.</returns>
        private bool CheckForAlternateAbilityUnlokced(int iconIndex, int dependentIconIndex)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return false;

            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            bool learnedAbility = false;
            bool minimumLevelMet = false;

            if (hamonPlayer.learnedHamonSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]))
            {
                learnedAbility = hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[dependentIconIndex]];
                minimumLevelMet = hamonPlayer.hamonSkillLevels[affectedSkillSlotIndexes[dependentIconIndex]] >= affectedSkillSlotHamonSkillLevel[dependentIconIndex];
                if (learnedAbility || minimumLevelMet)
                {
                    slotsToLock[iconIndex] = true;
                    unobtainableSlots[iconIndex] = true;
                }
            }

            return learnedAbility || minimumLevelMet;
        }

        private void SetUnusedIconsInvisible(int highestUsedIndex)
        {
            for (int i = highestUsedIndex + 1; i < hamonSkillIcons.Length; i++)
            {
                hamonSkillIcons[i].invisible = true;
            }
        }

        private string WrapText(string textToBreak, int sentenceCharacterLimit, string[] stringSplitParts)
        {
            List<string> createdSentences = new List<string>();
            string[] wordsArray = textToBreak.Split(stringSplitParts, System.StringSplitOptions.None);

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