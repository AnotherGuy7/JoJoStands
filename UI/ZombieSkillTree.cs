using JoJoStands.Items.Vampire;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class ZombieSkillTree : UIState
    {
        public static bool Visible;


        public UIPanel ZombieSkillTreePanel;
        private MouseTextPanel zombieSkillTooltips;
        private UIText zombieSkillPointsText;
        private UIImageButton zombieSkillTreeLeftArrow;
        private UIImageButton zombieSkillTreeRightArrow;
        private UIImageButton zombieSkillTreeXButton;
        private const int MaxUIPages = 3;
        private Texture2D[] skillTreeTextures = new Texture2D[MaxUIPages];
        private bool[] pageLockBosses = new bool[MaxUIPages - 1] { NPC.downedBoss2, NPC.downedBoss3 };
        private string[] pageLockMessages = new string[MaxUIPages - 1] { "The next page is unlocked Post-Evil Boss.", "The next page is unlocked Post-Skeletron." };


        private int currentShownPage = 1;
        private const float UIScale = 1f;
        private const int MaxButtonsPerPage = 10;
        private AdjustableButton[] zombieSkillIcons = new AdjustableButton[MaxButtonsPerPage];
        private Texture2D[] zombieSkillIconImages = new Texture2D[MaxButtonsPerPage];
        private string[] zombieSkillIconTooltips = new string[MaxButtonsPerPage];
        private int[] affectedSkillSlotIndexes = new int[VampirePlayer.ExpectedAmountOfZombieSkills];      //This is an array of indexes for the slot that goes true
        private int[] affectedSkillSlotZombieSkillLevel = new int[VampirePlayer.ExpectedAmountOfZombieSkills];       //An array of values that changes the skill level (for upgrading skills and stuff)
        private bool[] slotsToLock = new bool[MaxButtonsPerPage];
        private bool[] unobtainableSlots = new bool[MaxButtonsPerPage];
        private Texture2D unknownSkillTexture;

        public static void OpenZombieSkillTree()
        {
            Visible = true;
            JoJoStands.Instance.ZombieSkillTreeUI.InitializeButtons(1);
        }

        public override void Update(GameTime gameTime)
        {
            Player player = Main.player[Main.myPlayer];
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            zombieSkillTooltips.visible = false;
            for (int i = 0; i < MaxButtonsPerPage; i++)
            {
                AdjustableButton icon = zombieSkillIcons[i];

                if (icon.invisible)
                {
                    icon.drawAlpha = 0f;
                    continue;
                }

                if (icon.lockedInFocus)
                {
                    icon.drawAlpha = 1f;
                    icon.drawColor = Color.Red;
                }
                if (unobtainableSlots[i])
                    icon.drawColor = Color.Red;

                if (icon.IsMouseHovering)
                {
                    zombieSkillTooltips.ShowText(zombieSkillIconTooltips[i]);
                    zombieSkillTooltips.ownerPos = new Vector2(Main.screenWidth * ZombieSkillTreePanel.HAlign, Main.screenHeight * ZombieSkillTreePanel.VAlign) - new Vector2(ZombieSkillTreePanel.Width.Pixels / 2f, ZombieSkillTreePanel.Height.Pixels / 2f);      //On screen changes, it had to update
                }
            }
            zombieSkillPointsText.SetText("SP: " + zombiePlayer.vampireSkillPointsAvailable);

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            ZombieSkillTreePanel = new UIPanel();
            ZombieSkillTreePanel.HAlign = 0.5f;
            ZombieSkillTreePanel.VAlign = 0.5f;
            SetElementSize(ZombieSkillTreePanel, new Vector2(428f, 352f));
            ZombieSkillTreePanel.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            ZombieSkillTreePanel.BorderColor = new Color(0, 0, 0, 0);

            /*zombieSkillTooltip = new UIText("Click on any button to view its info.");
            SetElementSize(zombieSkillTooltip, new Vector2(102f, 38f));
            SetElementPosition(zombieSkillTooltip, new Vector2(186f, 178f));
            HamonSkillTreePanel.Append(zombieSkillTooltip);*/

            zombieSkillPointsText = new UIText("Skill Points Available: ");
            SetElementSize(zombieSkillPointsText, new Vector2(102f, 38f));
            SetElementPosition(zombieSkillPointsText, new Vector2(0f, 295f));
            ZombieSkillTreePanel.Append(zombieSkillPointsText);

            zombieSkillTreeLeftArrow = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/VampireSkillTreeLeftArrow"));
            SetElementSize(zombieSkillTreeLeftArrow, new Vector2(32f, 32f));
            SetElementPosition(zombieSkillTreeLeftArrow, new Vector2(10f, 10f));
            zombieSkillTreeLeftArrow.OnClick += OnClickHamonSkillTreeUpArrow;
            ZombieSkillTreePanel.Append(zombieSkillTreeLeftArrow);

            zombieSkillTreeRightArrow = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/VampireSkillTreeRightArrow"));
            SetElementSize(zombieSkillTreeRightArrow, new Vector2(32f, 32f));
            SetElementPosition(zombieSkillTreeRightArrow, new Vector2(46f, 10f));
            zombieSkillTreeRightArrow.OnClick += OnClickHamonSkillTreeDownArrow;
            ZombieSkillTreePanel.Append(zombieSkillTreeRightArrow);

            zombieSkillTreeXButton = new UIImageButton(ModContent.GetTexture("JoJoStands/Extras/VampireSkillTreeXButton"));
            SetElementSize(zombieSkillTreeXButton, new Vector2(32f, 32f));
            SetElementPosition(zombieSkillTreeXButton, new Vector2(364f, 7f));
            zombieSkillTreeXButton.OnClick += OnClickHamonSkillTreeXButton;
            ZombieSkillTreePanel.Append(zombieSkillTreeXButton);

            skillTreeTextures[0] = ModContent.GetTexture("JoJoStands/UI/ZombieSkillTree_Page1");
            skillTreeTextures[1] = ModContent.GetTexture("JoJoStands/UI/ZombieSkillTree_Page2");
            skillTreeTextures[2] = ModContent.GetTexture("JoJoStands/UI/ZombieSkillTree_Page3");
            unknownSkillTexture = ModContent.GetTexture("JoJoStands/Extras/VampireIcon_Unknown");

            for (int b = 0; b < MaxButtonsPerPage; b++)
            {
                zombieSkillIcons[b] = new AdjustableButton(ModContent.GetTexture("JoJoStands/Extras/VampireIcon_Empty"), Vector2.Zero, new Vector2(30f, 30f), Color.White);
                SetElementSize(zombieSkillIcons[b], new Vector2(30f, 30f));
                zombieSkillIcons[b].OnClick += OnClickAnyIcon;        //This ties *all* of the buttons to the same method
                zombieSkillIcons[b].OnDoubleClick += OnDoubleClickAnyIcon;
                zombieSkillIcons[b].owner = ZombieSkillTreePanel;
                ZombieSkillTreePanel.Append(zombieSkillIcons[b]);
            }
            InitializeButtons(1);

            zombieSkillTooltips = new MouseTextPanel(256, 1, "Test Desc");       //Has to be at the end so it doesn't interfere with other button collisions
            zombieSkillTooltips.ownerPos = new Vector2(Main.screenWidth * ZombieSkillTreePanel.HAlign, Main.screenHeight * ZombieSkillTreePanel.VAlign) - new Vector2(ZombieSkillTreePanel.Width.Pixels / 2f, ZombieSkillTreePanel.Height.Pixels / 2f);
            ZombieSkillTreePanel.Append(zombieSkillTooltips);

            Append(ZombieSkillTreePanel);
        }

        private void OnClickHamonSkillTreeUpArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            pageLockBosses = new bool[2] { NPC.downedBoss1, NPC.downedBoss2 };
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

            for (int b = 0; b < MaxButtonsPerPage; b++)
            {
                zombieSkillIcons[b].focusedOn = false;
                if (listeningElement == zombieSkillIcons[b])
                {
                    buttonIndex = b;
                    buttonClickedOn = zombieSkillIcons[b];
                }
            }

            if (buttonClickedOn == null)
            {
                Main.NewText("Something went wrong with the Zombie Skill Tree.", Color.Red);
                Visible = false;
                return;
            }
            if (buttonClickedOn.invisible)
            {
                return;
            }

            Main.PlaySound(SoundID.MenuTick);
            buttonClickedOn.focusedOn = true;
            //zombieSkillTooltip.SetText(zombieSkillIconTooltips[buttonIndex]);
        }

        private void OnDoubleClickAnyIcon(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;
            Player player = Main.player[Main.myPlayer];
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            for (int b = 0; b < MaxButtonsPerPage; b++)
            {
                if (listeningElement == zombieSkillIcons[b])
                {
                    buttonIndex = b;
                    buttonClickedOn = zombieSkillIcons[b];
                }
                if (zombieSkillIcons[b].focusedOn)
                {
                    zombieSkillIcons[b].focusedOn = false;
                }
            }

            if (buttonClickedOn == null)
            {
                Main.NewText("Something went wrong with the Zombie Skill Tree.", Color.Red);
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
            if (zombiePlayer.vampireSkillPointsAvailable > 0)
            {
                if (!zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[buttonIndex]] || zombiePlayer.zombieSkillLevels[affectedSkillSlotIndexes[buttonIndex]] < affectedSkillSlotZombieSkillLevel[buttonIndex])      //To prevent ability degrading if it can happen
                {
                    buttonClickedOn.focusedOn = true;
                    buttonClickedOn.lockedInFocus = true;
                    zombiePlayer.learnedAnyZombieAbility = true;
                    zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[buttonIndex]] = true;
                    zombiePlayer.zombieSkillLevels[affectedSkillSlotIndexes[buttonIndex]] = affectedSkillSlotZombieSkillLevel[buttonIndex];
                    /*if (affectedSkillSlotHamonRequired[buttonIndex] != 0)
                    {
                        zombiePlayer.zombieAmountRequirements[affectedSkillSlotIndexes[buttonIndex]] = affectedSkillSlotHamonRequired[buttonIndex];
                    }*/
                    zombiePlayer.vampireSkillPointsAvailable -= 1;
                    //zombieSkillTooltip.SetText(zombieSkillIconTooltips[buttonIndex]);
                    Main.NewText("Skill Obtained!", Color.Red);
                }
            }
            else if (zombiePlayer.vampireSkillPointsAvailable <= 0)
            {
                Main.NewText("You don't have any skill points! (Get more by killing new types of enemies multiple times!)", Color.Red);
            }
            else if (zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[buttonIndex]])
            {
                Main.NewText("You've already obtained this skill before!", Color.Red);
            }
            InitializeButtons(currentShownPage);
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            spriteBatch.Draw(skillTreeTextures[currentShownPage - 1], ZombieSkillTreePanel.GetClippingRectangle(spriteBatch), Color.White);
        }

        private void InitializeButtons(int page)
        {
            for (int b = 0; b < MaxButtonsPerPage; b++)
            {
                zombieSkillIcons[b].SetImage(ModContent.GetTexture("JoJoStands/Extras/VampireIcon_Empty"));
                zombieSkillIcons[b].invisible = false;
                zombieSkillIcons[b].lockedInFocus = false;
                slotsToLock[b] = false;
                unobtainableSlots[b] = false;
                SetElementPosition(zombieSkillIcons[b], Vector2.Zero);
            }
            for (int i = 0; i < MaxButtonsPerPage; i++)
            {
                AdjustableButton icon = zombieSkillIcons[i];
                if (icon.lockedInFocus)
                {
                    icon.drawAlpha = 1f;
                    icon.drawColor = Color.Red;
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
                    SetElementPosition(zombieSkillIcons[0], new Vector2(352f, 206f));
                    zombieSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_0");
                    zombieSkillIcons[0].SetImage(zombieSkillIconImages[0]);
                    zombieSkillIconTooltips[0] = "Learn of your newfound abilities as a Zombie.";
                    zombieSkillIcons[0].lockedInFocus = true;        //Zombie is always learned

                    SetElementPosition(zombieSkillIcons[1], new Vector2(260f, 206f));
                    zombieSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_1");
                    zombieSkillIcons[1].SetImage(zombieSkillIconImages[1]);
                    zombieSkillIconTooltips[1] = "Your new structure gives you unparalleled strength and speed.\nWhen a zombie, your damage and movement speed is increased by 4%.";
                    affectedSkillSlotIndexes[1] = VampirePlayer.UndeadConstitution;
                    affectedSkillSlotZombieSkillLevel[1] = 1;
                    CheckForIconAbilityUnlocked(1);

                    SetElementPosition(zombieSkillIcons[2], new Vector2(134f, 206f));
                    zombieSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_2");
                    zombieSkillIcons[2].SetImage(zombieSkillIconImages[2]);
                    zombieSkillIconTooltips[2] = "Getting crafty with nearby materials proves to be of use.\nAllows you to right-click Dirt or Mud Blocks to rub yourself in those tiles, granting some protection against Sunburn.";
                    affectedSkillSlotIndexes[2] = VampirePlayer.ProtectiveFilm;
                    affectedSkillSlotZombieSkillLevel[2] = 1;
                    CheckForIconAbilityUnlocked(2);
                    CheckForIconLock(2, 1);

                    SetElementPosition(zombieSkillIcons[3], new Vector2(235f, 79f));
                    zombieSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_3");
                    zombieSkillIcons[3].SetImage(zombieSkillIconImages[3]);
                    zombieSkillIconTooltips[3] = "Being a zombie has made you much more attentive.\nAt night, every 20s, enemies at full health are highlighted and damage dealt to them while they are highlighted is multiplied by 1.2x.";
                    affectedSkillSlotIndexes[3] = VampirePlayer.UndeadPerception;
                    affectedSkillSlotZombieSkillLevel[3] = 1;
                    CheckForIconAbilityUnlocked(3);
                    CheckForIconLock(3, 1);

                    SetElementPosition(zombieSkillIcons[4], new Vector2(112f, 79f));
                    zombieSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_4");
                    zombieSkillIcons[4].SetImage(zombieSkillIconImages[4]);
                    zombieSkillIconTooltips[4] = "The taste of blood strengthens you.\nAllows you to use Blood Suck using your Zombie Abilities.";
                    affectedSkillSlotIndexes[4] = VampirePlayer.BloodSuck;
                    affectedSkillSlotZombieSkillLevel[4] = 1;
                    CheckForIconAbilityUnlocked(4);
                    if (!CheckForIconLock(4, 2, true))
                    {
                        CheckForIconLock(4, 3);
                    }

                    SetElementPosition(zombieSkillIcons[5], new Vector2(43f, 79f));
                    zombieSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_6");
                    zombieSkillIcons[5].SetImage(zombieSkillIconImages[5]);
                    zombieSkillIconTooltips[5] = "Your twisted, deteriorated form has many hidden advantages.\nAllows you to use the withering abilities. (Will become freely craftable once this skill is unlocked)";
                    affectedSkillSlotIndexes[5] = VampirePlayer.WitheringAbilities;
                    affectedSkillSlotZombieSkillLevel[5] = 1;
                    CheckForIconAbilityUnlocked(5);
                    CheckForIconLock(5, 4);

                    SetUnusedIconsInvisible(5);
                    break;

                case 2:
                    SetElementPosition(zombieSkillIcons[0], new Vector2(330f, 79f));
                    zombieSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_7");
                    zombieSkillIcons[0].SetImage(zombieSkillIconImages[0]);
                    zombieSkillIconTooltips[0] = "Your experience in savagery has sharpened your intellect, granting you knowledge on where to strike next.\nAll vampiric weapons have a 7% of inflicting Lacerated!";
                    affectedSkillSlotIndexes[0] = VampirePlayer.SavageInstincts;
                    affectedSkillSlotZombieSkillLevel[0] = 1;
                    CheckForIconAbilityUnlocked(0);
                    CheckForSpecificAbiltyUnlocked(0, VampirePlayer.WitheringAbilities);

                    SetElementPosition(zombieSkillIcons[1], new Vector2(249f, 79f));
                    zombieSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_1");
                    zombieSkillIcons[1].SetImage(zombieSkillIconImages[1]);
                    zombieSkillIconTooltips[1] = "Your new structure gives you unparalleled strength and speed.\nWhen a zombie, your damage and movement speed is increased by 7%.";
                    affectedSkillSlotIndexes[1] = VampirePlayer.UndeadConstitution;
                    affectedSkillSlotZombieSkillLevel[1] = 2;
                    CheckForIconAbilityUnlocked(1, 2);
                    CheckForIconLock(1, 0);

                    SetElementPosition(zombieSkillIcons[2], new Vector2(295f, 128f));
                    zombieSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_8");
                    zombieSkillIcons[2].SetImage(zombieSkillIconImages[2]);
                    zombieSkillIconTooltips[2] = "Being entombed has its perks.\nWhen a zombie, hold DOWN while standing still for 3 seconds to bury yourself underground. While underground, life regeneration is increased. Press JUMP to get out.";
                    affectedSkillSlotIndexes[2] = VampirePlayer.UndergroundRecovery;
                    affectedSkillSlotZombieSkillLevel[2] = 1;
                    CheckForIconAbilityUnlocked(2);
                    CheckForIconLock(2, 0);

                    SetElementPosition(zombieSkillIcons[3], new Vector2(294f, 171f));
                    zombieSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_9");
                    zombieSkillIcons[3].SetImage(zombieSkillIconImages[3]);
                    zombieSkillIconTooltips[3] = "Your heightened senses make reacting to incoming danger basic instinct.\n8% Chance to dodge incoming damage.";
                    affectedSkillSlotIndexes[3] = VampirePlayer.EvasiveInstincts;
                    affectedSkillSlotZombieSkillLevel[3] = 1;
                    CheckForIconAbilityUnlocked(3);
                    CheckForIconLock(3, 0);

                    SetElementPosition(zombieSkillIcons[4], new Vector2(216f, 171f));
                    zombieSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_5");
                    zombieSkillIcons[4].SetImage(zombieSkillIconImages[4]);
                    zombieSkillIconTooltips[4] = "The use of knives as a weapon shines out to you.\nAllows you to use the knife wielder abilities. (Will become freely craftable once this skill is unlocked)";
                    affectedSkillSlotIndexes[4] = VampirePlayer.KnifeWielder;
                    affectedSkillSlotZombieSkillLevel[4] = 1;
                    CheckForIconAbilityUnlocked(4);
                    CheckForIconLock(4, 3);

                    SetElementPosition(zombieSkillIcons[5], new Vector2(215f, 243f));
                    zombieSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_3");
                    zombieSkillIcons[5].SetImage(zombieSkillIconImages[5]);
                    zombieSkillIconTooltips[5] = "Being a zombie has made you much more attentive.\nAt night, every 10s, enemies at full health are highlighted and damage dealt to them while they are highlighted is multiplied by 1.4x.";
                    affectedSkillSlotIndexes[5] = VampirePlayer.UndeadPerception;
                    affectedSkillSlotZombieSkillLevel[5] = 2;
                    CheckForIconAbilityUnlocked(5, 2);
                    CheckForIconLock(5, 4);

                    SetElementPosition(zombieSkillIcons[6], new Vector2(135f, 171f));
                    zombieSkillIconImages[6] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_10");
                    zombieSkillIcons[6].SetImage(zombieSkillIconImages[6]);
                    zombieSkillIconTooltips[6] = "Those who don't fear you will have to be shown who the top is.\nWhile a boss is summoned: +8% Movement Speed\n+10% Damage\n+15% Jump Speed";
                    affectedSkillSlotIndexes[6] = VampirePlayer.TopOfTheChain;
                    affectedSkillSlotZombieSkillLevel[6] = 1;
                    CheckForIconAbilityUnlocked(6);
                    CheckForIconLock(6, 4);

                    SetElementPosition(zombieSkillIcons[7], new Vector2(101f, 105f));
                    zombieSkillIconImages[7] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_4");
                    zombieSkillIcons[7].SetImage(zombieSkillIconImages[7]);
                    zombieSkillIconTooltips[7] = "The taste of blood strengthens you.\nBlood Suck abilities have been improved.";
                    affectedSkillSlotIndexes[7] = VampirePlayer.BloodSuck;
                    affectedSkillSlotZombieSkillLevel[7] = 2;
                    CheckForIconAbilityUnlocked(7, 2);
                    CheckForIconLock(7, 6);

                    SetElementPosition(zombieSkillIcons[8], new Vector2(67f, 171f));
                    zombieSkillIconImages[8] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_11");
                    zombieSkillIcons[8].SetImage(zombieSkillIconImages[8]);
                    zombieSkillIconTooltips[8] = "Your insides are more than enough of a danger to your surroundings.\nAllows you to use the Entrails Abilities.";
                    affectedSkillSlotIndexes[8] = VampirePlayer.EntrailAbilities;
                    affectedSkillSlotZombieSkillLevel[8] = 1;
                    CheckForIconAbilityUnlocked(8);
                    CheckForIconLock(8, 6);

                    SetElementPosition(zombieSkillIcons[9], new Vector2(25f, 243f));
                    zombieSkillIconImages[9] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_7");
                    zombieSkillIcons[9].SetImage(zombieSkillIconImages[9]);
                    zombieSkillIconTooltips[9] = "Your experience in savagery has sharpened your intellect, granting you knowledge on where to strike next.\nAll vampiric weapons have a 12% of inflicting Lacerated!";
                    affectedSkillSlotIndexes[9] = VampirePlayer.SavageInstincts;
                    affectedSkillSlotZombieSkillLevel[9] = 2;
                    CheckForIconAbilityUnlocked(9, 2);
                    CheckForIconLock(9, 8);

                    SetUnusedIconsInvisible(10);
                    break;

                case 3:
                    SetElementPosition(zombieSkillIcons[0], new Vector2(332f, 178f));
                    zombieSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_13");
                    zombieSkillIcons[0].SetImage(zombieSkillIconImages[0]);
                    zombieSkillIconTooltips[0] = "Repeatedly seeing the actions of your enemies makes them much more predictable.\nVampiric Damage done to enemies is increased by the amount of times that type of enemy has been killed.";
                    affectedSkillSlotIndexes[0] = VampirePlayer.ExperiencedBeast;
                    affectedSkillSlotZombieSkillLevel[0] = 1;
                    CheckForIconAbilityUnlocked(0);
                    CheckForSpecificAbiltyUnlocked(0, VampirePlayer.SavageInstincts, 2);

                    SetElementPosition(zombieSkillIcons[1], new Vector2(237f, 159f));
                    zombieSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_8");
                    zombieSkillIcons[1].SetImage(zombieSkillIconImages[1]);
                    zombieSkillIconTooltips[1] = "Being entombed has its perks.\nWhen a zombie, hold DOWN while standing still for 3 seconds to bury yourself underground. While underground, life regeneration is greatly increased. Press JUMP to get out.";
                    affectedSkillSlotIndexes[1] = VampirePlayer.UndergroundRecovery;
                    affectedSkillSlotZombieSkillLevel[1] = 2;
                    CheckForIconAbilityUnlocked(1, 2);
                    CheckForIconLock(1, 0);

                    SetElementPosition(zombieSkillIcons[2], new Vector2(237f, 228f));
                    zombieSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_12");
                    zombieSkillIcons[2].SetImage(zombieSkillIconImages[2]);
                    zombieSkillIconTooltips[2] = "Your might cannot be bested, but dying in battle isn't a testament to that.\nWhen your health reaches 0, 30% chance to regain 30% of maximum health back. Maximum health is set to 30% of its original capacity.";
                    affectedSkillSlotIndexes[2] = VampirePlayer.FinalPush;
                    affectedSkillSlotZombieSkillLevel[2] = 1;
                    CheckForIconAbilityUnlocked(2);
                    CheckForIconLock(2, 1);


                    SetElementPosition(zombieSkillIcons[3], new Vector2(139f, 159f));
                    zombieSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_9");
                    zombieSkillIcons[3].SetImage(zombieSkillIconImages[3]);
                    zombieSkillIconTooltips[3] = "Your heightened senses make reacting to incoming danger basic instinct.\n12% Chance to dodge incoming damage.";
                    affectedSkillSlotIndexes[3] = VampirePlayer.EvasiveInstincts;
                    affectedSkillSlotZombieSkillLevel[3] = 3;
                    CheckForIconAbilityUnlocked(3, 3);
                    CheckForIconLock(3, 1);

                    SetElementPosition(zombieSkillIcons[4], new Vector2(140f, 96f));
                    zombieSkillIconImages[4] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_10");
                    zombieSkillIcons[4].SetImage(zombieSkillIconImages[4]);
                    zombieSkillIconTooltips[4] = "Those who don't fear you will have to be shown who the top is.\nWhile a boss is summoned: +11% Movement Speed\n+16% Damage\n+20% Jump Speed";
                    affectedSkillSlotIndexes[4] = VampirePlayer.TopOfTheChain;
                    affectedSkillSlotZombieSkillLevel[4] = 2;
                    CheckForIconAbilityUnlocked(4, 2);

                    SetElementPosition(zombieSkillIcons[5], new Vector2(140f, 27f));
                    zombieSkillIconImages[5] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_13");
                    zombieSkillIcons[5].SetImage(zombieSkillIconImages[5]);
                    zombieSkillIconTooltips[5] = "Repeatedly seeing the actions of your enemies makes them much more predictable.\nVampiric Damage done to enemies is increased by the amount of times that type of enemy has been killed.";
                    affectedSkillSlotIndexes[5] = VampirePlayer.ExperiencedBeast;
                    affectedSkillSlotZombieSkillLevel[5] = 2;
                    CheckForIconAbilityUnlocked(5, 2);
                    CheckForIconLock(5, 4);

                    SetElementPosition(zombieSkillIcons[6], new Vector2(238f, 96f));
                    zombieSkillIconImages[6] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_4");
                    zombieSkillIcons[6].SetImage(zombieSkillIconImages[6]);
                    zombieSkillIconTooltips[6] = "The use of knives as a weapon shines out to you.\nAllows you to use the knife wielder abilities. (Will become freely craftable once this skill is unlocked)";
                    affectedSkillSlotIndexes[6] = VampirePlayer.BloodSuck;
                    affectedSkillSlotZombieSkillLevel[6] = 3;
                    CheckForIconAbilityUnlocked(6, 3);

                    SetElementPosition(zombieSkillIcons[7], new Vector2(330f, 79f));
                    zombieSkillIconImages[7] = ModContent.GetTexture("JoJoStands/Extras/ZombieIcon_1");
                    zombieSkillIcons[7].SetImage(zombieSkillIconImages[7]);
                    zombieSkillIconTooltips[7] = "Your new structure gives you unparalleled strength and speed.\nWhen a zombie, your damage and movement speed is increased by 10%.";
                    affectedSkillSlotIndexes[7] = VampirePlayer.UndeadConstitution;
                    affectedSkillSlotZombieSkillLevel[7] = 3;
                    CheckForIconAbilityUnlocked(7, 3);
                    CheckForIconLock(7, 0);

                    CheckForIconLock(6, 7);
                    CheckForIconLock(4, 6);

                    SetUnusedIconsInvisible(8);
                    break;
            }

            for (int b = 0; b < MaxButtonsPerPage; b++)       //Done this way because if affectedSkillSlotIndexes changes to -1 while another icon needs to check it, it makes a broken icon chain
            {
                if (slotsToLock[b])
                {
                    if (unobtainableSlots[b])
                    {
                        zombieSkillIconTooltips[b] = "This skill cannot be unlocked because the alternate skill has already been unlocked.";
                        affectedSkillSlotIndexes[b] = -1;
                        continue;
                    }

                    zombieSkillIcons[b].SetImage(unknownSkillTexture);
                    zombieSkillIconTooltips[b] = "Unlock the skill before this skill to be able to see this skill.";
                    affectedSkillSlotIndexes[b] = -1;
                }
            }

            //Text wrapping
            if (!Main.gameMenu)
            {
                string[] stringSplitters = new string[1] { " " };
                for (int tooltipIndex = 0; tooltipIndex < zombieSkillIconTooltips.Length; tooltipIndex++)
                {
                    if (zombieSkillIconTooltips[tooltipIndex] != null && zombieSkillIconTooltips[tooltipIndex].Length != 0)
                    {
                        zombieSkillIconTooltips[tooltipIndex] = WrapText(zombieSkillIconTooltips[tooltipIndex], 26, stringSplitters);
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
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            bool learnedAbility = zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[iconIndex]];
            bool minimumLevelMet = zombiePlayer.zombieSkillLevels[affectedSkillSlotIndexes[iconIndex]] >= minimumLevel;
            if (learnedAbility && minimumLevelMet)
            {
                zombieSkillIcons[iconIndex].lockedInFocus = true;
            }

            return learnedAbility && minimumLevelMet;
        }

        /// <summary>
        /// Checks if the specified icon should be marked as "Unknown"
        /// </summary>
        /// <returns>Whether or not the icon is not locked.</returns>
        private bool CheckForIconLock(int iconIndex, int dependentIconIndex, bool chained = false, bool debug = false)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return false;

            Player player = Main.player[Main.myPlayer];
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            bool learnedAbility = false;
            bool minimumLevelMet = false;
            if (debug)
                Main.NewText(zombiePlayer.learnedZombieSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]) + "; " + affectedSkillSlotIndexes[dependentIconIndex]);

            if (zombiePlayer.learnedZombieSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]))
            {
                if (debug)
                    Main.NewText(iconIndex + "; Run");
                learnedAbility = zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[dependentIconIndex]];
                minimumLevelMet = zombiePlayer.zombieSkillLevels[affectedSkillSlotIndexes[dependentIconIndex]] >= affectedSkillSlotZombieSkillLevel[dependentIconIndex];
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
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            bool learnedAbility = zombiePlayer.learnedZombieSkills[abilityIndex];
            bool minimumLevelMet = zombiePlayer.zombieSkillLevels[abilityIndex] >= minimumLevel;
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
            VampirePlayer zombiePlayer = player.GetModPlayer<VampirePlayer>();

            bool learnedAbility = false;
            bool minimumLevelMet = false;

            if (zombiePlayer.learnedZombieSkills.ContainsKey(affectedSkillSlotIndexes[dependentIconIndex]))
            {
                learnedAbility = zombiePlayer.learnedZombieSkills[affectedSkillSlotIndexes[dependentIconIndex]];
                minimumLevelMet = zombiePlayer.zombieSkillLevels[affectedSkillSlotIndexes[dependentIconIndex]] >= affectedSkillSlotZombieSkillLevel[dependentIconIndex];
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
            for (int i = highestUsedIndex + 1; i < MaxButtonsPerPage; i++)
            {
                zombieSkillIcons[i].invisible = true;
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