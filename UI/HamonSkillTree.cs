using JoJoStands.Items.Hamon;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public UIText hamonSkillTooltip;
        public UIImageButton hamonSkillTreeUpArrow;
        public UIImageButton hamonSkillTreeDownArrow;
        public UIImageButton hamonSkillTreeXButton;
        private Texture2D[] skillTreeTextures = new Texture2D[3];


        public int currentShownPage = 1;
        public const float UIScale = 1f;
        public AdjustableButton[] hamonSkillIcons = new AdjustableButton[10];
        public Vector2[] hamonSkillIconPositions = new Vector2[10];
        public Texture2D[] hamonSkillIconImages = new Texture2D[10];
        public string[] hamonSkillIconTooltips = new string[10];
        public int[] affectedSkillSlotIndexes = new int[HamonPlayer.HamonSkillsLimit];      //This is an array of indexes for the slot that goes true
        public int[] affectedSkillSlotHamonRequired = new int[HamonPlayer.HamonSkillsLimit];

        public static void OpenHamonSkillTree()
        {
            Visible = true;
            JoJoStands.Instance.HamonSkillTreeUI.InitializeButtons(1);
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < hamonSkillIcons.Length; i++)
            {
                AdjustableButton icon = hamonSkillIcons[i];
                if (icon.lockedInFocus)
                {
                    icon.drawAlpha = 1f;
                    icon.drawColor = Color.Yellow;
                }
            }

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
            SetElementSize(HamonSkillTreePanel, new Vector2(300f, 226f));
            HamonSkillTreePanel.BackgroundColor = new Color(0, 0, 0, 0);       //make it invisible so that the image is there itself
            HamonSkillTreePanel.BorderColor = new Color(0, 0, 0, 0);

            hamonSkillTooltip = new UIText("Click on any button to view its info.");
            SetElementSize(hamonSkillTooltip, new Vector2(102f, 38f));
            SetElementPosition(hamonSkillTooltip, new Vector2(186f, 178f));
            HamonSkillTreePanel.Append(hamonSkillTooltip);

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
            SetElementPosition(hamonSkillTreeXButton, new Vector2(240f, 4f));
            hamonSkillTreeXButton.OnClick += OnClickHamonSkillTreeXButton;
            HamonSkillTreePanel.Append(hamonSkillTreeXButton);

            skillTreeTextures[0] = ModContent.GetTexture("JoJoStands/UI/HamonSkillTree");

            for (int b = 0; b < hamonSkillIcons.Length; b++)
            {
                hamonSkillIcons[b] = new AdjustableButton(ModContent.GetTexture("JoJoStands/Extras/HamonIcon_Empty"), Vector2.Zero, new Vector2(30f, 30f), Color.White);
                SetElementSize(hamonSkillIcons[b], new Vector2(30f, 30f));
                hamonSkillIcons[b].OnClick += OnClickAnyIcon;        //This ties *all* of the buttons to the same method
                hamonSkillIcons[b].OnDoubleClick += OnDoubleClickAnyIcon;
                hamonSkillIcons[b].owner = HamonSkillTreePanel;
                HamonSkillTreePanel.Append(hamonSkillIcons[b]);
            }
            InitializeButtons(1);

            Append(HamonSkillTreePanel);
        }

        private void OnClickHamonSkillTreeUpArrow(UIMouseEvent evt, UIElement listeningElement)
        {
            currentShownPage++;
            Main.PlaySound(SoundID.MenuTick);
            if (currentShownPage >= 4)
            {
                currentShownPage = 3;
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
        }

        private void OnClickAnyIcon(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;

            Main.PlaySound(SoundID.MenuTick);
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

            buttonClickedOn.focusedOn = true;
            hamonSkillTooltip.SetText(hamonSkillIconTooltips[buttonIndex]);
        }

        private void OnDoubleClickAnyIcon(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            Main.PlaySound(SoundID.MenuTick);
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

            if (!hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[buttonIndex]])
            {
                buttonClickedOn.focusedOn = true;
                buttonClickedOn.lockedInFocus = true;
                hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[buttonIndex]] = true;
                if (affectedSkillSlotHamonRequired[buttonIndex] != 0)
                {
                    hamonPlayer.hamonAmountRequirements[affectedSkillSlotIndexes[buttonIndex]] = affectedSkillSlotHamonRequired[buttonIndex];
                }
                hamonSkillTooltip.SetText(hamonSkillIconTooltips[buttonIndex]);
                Main.NewText("Skill Obtained!", Color.Yellow);
            }
            else
            {
                Main.NewText("You've already obtained this skill before!", Color.Yellow);
            }
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)       //from ExampleMod's ExampleUI
        {
            spriteBatch.Draw(skillTreeTextures[currentShownPage - 1], HamonSkillTreePanel.GetClippingRectangle(spriteBatch), Color.White);
        }

        private void InitializeButtons(int page)
        {
            switch (page)
            {
                case 1:
                    SetElementPosition(hamonSkillIcons[0], new Vector2(126f, 158f));
                    hamonSkillIconTooltips[0] = "Learn to control your Hamon.";
                    hamonSkillIconImages[0] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_0");
                    hamonSkillIcons[0].SetImage(hamonSkillIconImages[0]);
                    hamonSkillIcons[0].lockedInFocus = true;        //This is because Hamon is always already learned

                    SetElementPosition(hamonSkillIcons[1], new Vector2(54f, 120f));
                    hamonSkillIconImages[1] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_1");
                    hamonSkillIcons[1].SetImage(hamonSkillIconImages[1]);
                    hamonSkillIconTooltips[1] = "The Hamon you breathe in focuses itself to heal your body.\nIncreased Life Regen when Hamon Breathing.";
                    affectedSkillSlotIndexes[1] = HamonPlayer.BreathingRegenSkill;
                    CheckForIconAbilityUnlocked(1);

                    SetElementPosition(hamonSkillIcons[2], new Vector2(191f, 120f));
                    hamonSkillIconImages[2] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_4");
                    hamonSkillIcons[2].SetImage(hamonSkillIconImages[2]);
                    hamonSkillIconTooltips[2] = "Your Hamon automatically focuses itself enough to help you keep your balance.\nWhen your Hamon is past 40, you will be able to water walk.";
                    affectedSkillSlotIndexes[2] = HamonPlayer.WaterWalkingSKill;
                    affectedSkillSlotHamonRequired[2] = 40;
                    CheckForIconAbilityUnlocked(2);

                    SetElementPosition(hamonSkillIcons[3], new Vector2(123f, 63f));
                    hamonSkillIconImages[3] = ModContent.GetTexture("JoJoStands/Extras/HamonIcon_2");
                    hamonSkillIcons[3].SetImage(hamonSkillIconImages[3]);
                    hamonSkillIconTooltips[3] = "Your Hamon can now be injected into weapons.\nWhen your Hamon is past 60, double-tap Special while holding a melee weapon to imbue it with Hamon.";
                    affectedSkillSlotIndexes[3] = HamonPlayer.WeaponsHamonImbueSkill;
                    affectedSkillSlotHamonRequired[3] = 60;
                    CheckForIconAbilityUnlocked(3);

                    //Slot 4 Position: 64, 26
                    //Slot 5 Position: 174, 26
                    SetUnusedIconsInvisible(4);
                    break;
                case 2:
                    break;
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

        private void CheckForIconAbilityUnlocked(int iconIndex)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return;

            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if (hamonPlayer.learnedHamonSkills[affectedSkillSlotIndexes[iconIndex]])
            {
                hamonSkillIcons[iconIndex].lockedInFocus = true;
            }
        }

        private void SetUnusedIconsInvisible(int numberUsed)
        {
            for (int i = numberUsed - 1; i < hamonSkillIcons.Length; i++)
            {
                hamonSkillIcons[i].drawAlpha = 0f;
            }
        }
    }
}