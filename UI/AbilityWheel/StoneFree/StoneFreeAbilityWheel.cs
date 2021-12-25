using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class StoneFreeAbilityWheel : AbilityWheel
    {
        private static StoneFreeAbilityWheel stoneFreeAbilityWheel;

        private const int AmountOfAbilities = 6;
        /*private const int Ability_Summon = 0;
        private const int Ability_ExtendedBarrage = 1;
        private const int Ability_Traps = 2;
        private const int Ability_Bind = 3;
        private const int Abliity_TiedTogether = 4;
        private const int Ability_Weave = 5;*/

        private const float WheelSpace = 50f;
        private const float TextScale = 0.8f;

        public AdjustableButton[] abilityButtons;
        public AdjustableButton wheelCenter;
        public MouseTextPanel abilityDescriptionsPanel;
        public UIText abilityNameText;

        private int inputTimer = 0;
        private float wheelRotation = 0f;
        private int previousScrollValue = 0;
        private int nameTextShowTimer = 0;
        private bool abilityChanged = false;
        private Vector2 textMeasurement;

        private string[] abilityNames = new string[AmountOfAbilities]
        {
            "Summon",
            "Extended Barrage",
            "String Traps",
            "Bind",
            "Tied Together",
            "Weave"
        };

        private string[] abilityTextureNames = new string[AmountOfAbilities]
{
            "Summon",
            "ExtendedBarrage",
            "StringTraps",
            "Bind",
            "TiedTogether",
            "Weave"
};


        private string[] abilityDescriptions = new string[AmountOfAbilities]
        {
            "Summons Stone Free. (This ability is the same as Stand Out)",
            "Allows Stone Free to turn its arms into strings, letting its punched travel farther but hit slightly weaker.",
            "Allows Stone Free to tie strings onto two tiles to create a string trap. Enemies are hurt by the string and enemy projectiles are stopped by the string.",
            "Allows Stone Free to tie an enemy up. Certain enemies can move desprite being tied up.",
            "Allows Stone Free to tie and drag enemies.",
            "Allows Stone Free to create a condensed string shield."
        };

        public static void OpenAbilityWheel(MyPlayer modPlayer)
        {
            visible = true;
            mPlayer = modPlayer;

            StoneFreeAbilityWheel wheel = stoneFreeAbilityWheel;
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                wheel.abilityButtons[i].SetButtonPosiiton(wheel.wheelCenter.buttonPosition + (wheel.IndexToRadianPosition(i, 6, wheel.wheelRotation) * WheelSpace));
            }
        }

        public static void CloseAbilityWheel()
        {
            visible = false;
        }

        public override void OnInitialize()
        {
            abilityWheel = new UIPanel();
            abilityWheel.HAlign = 0.98f;
            abilityWheel.VAlign = 0.5f;
            abilityWheel.Width.Set(120f, 0f);
            abilityWheel.Height.Set(120f, 0f);
            abilityWheel.BackgroundColor = Color.Transparent;
            abilityWheel.BorderColor = Color.Transparent;
            abilityWheel.OnScrollWheel += MouseScroll;
            stoneFreeAbilityWheel = this;

            wheelCenter = new AdjustableButton(ModContent.GetTexture("JoJoStands/UI/AbilityWheel/WheelCenter"), Vector2.Zero, new Vector2(54f), Color.White, 1f, 1f);
            wheelCenter.SetButtonPosiiton(new Vector2(60f));
            wheelCenter.SetButtonSize(new Vector2(54f));
            wheelCenter.SetOverlayImage(ModContent.GetTexture("JoJoStands/Items/StoneFreeT1"), 0.15f);
            wheelCenter.owner = abilityWheel;
            wheelCenter.respondToFocus = false;
            abilityWheel.Append(wheelCenter);

            abilityButtons = new AdjustableButton[AmountOfAbilities];
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                abilityButtons[i] = new AdjustableButton(ModContent.GetTexture("JoJoStands/UI/AbilityWheel/WheelPiece"), wheelCenter.buttonCenter + (IndexToRadianPosition(i, 6) * WheelSpace), new Vector2(38f), Color.White, 1f, 1f);
                abilityButtons[i].SetOverlayImage(ModContent.GetTexture("JoJoStands/UI/AbilityWheel/StoneFree/" + abilityTextureNames[i]), 0.15f);
                abilityButtons[i].OnClick += ClickedAbility;
                abilityButtons[i].OnScrollWheel += MouseScroll;
                //abilityButtons[i].owner = abilityWheel;
                abilityWheel.Append(abilityButtons[i]);
            }

            abilityDescriptionsPanel = new MouseTextPanel(480, 1, "None", true, 24);
            abilityDescriptionsPanel.ownerPos = new Vector2(Main.screenWidth * abilityWheel.HAlign, Main.screenHeight * abilityWheel.VAlign);
            abilityWheel.Append(abilityDescriptionsPanel);

            abilityNameText = new UIText("A", TextScale);
            abilityNameText.HAlign = 0.6f;
            abilityNameText.Left.Pixels = 0f;
            abilityNameText.Top.Pixels = 100f;
            abilityNameText.Width.Pixels = 180f;
            abilityNameText.Height.Pixels = 20f;
            abilityWheel.Append(abilityNameText);

            Append(abilityWheel);
        }

        private void MouseScroll(UIMouseEvent evt, UIElement listeningElement)
        {
            if (IsMouseHovering && inputTimer <= 0)
            {
                inputTimer += 3;
                int newScrollValue = Mouse.GetState().ScrollWheelValue;
                int wheelDifference = previousScrollValue - newScrollValue;
                previousScrollValue = newScrollValue;

                if (wheelDifference > 0)
                {
                    mPlayer.chosenAbility -= 1;
                    if (mPlayer.chosenAbility < 0)
                        mPlayer.chosenAbility = 5;
                }
                else
                {
                    mPlayer.chosenAbility += 1;
                    if (mPlayer.chosenAbility > 5)
                        mPlayer.chosenAbility = 0;
                }
                abilityChanged = true;

                nameTextShowTimer = 5 * 60;
                abilityNameText.SetText(abilityNames[mPlayer.chosenAbility]);
                textMeasurement = Main.fontMouseText.MeasureString(abilityNames[mPlayer.chosenAbility]) * TextScale;

                for (int i = 0; i < AmountOfAbilities; i++)
                {
                    AdjustableButton button = abilityButtons[i];
                    button.drawColor = Color.White;
                    if (i == mPlayer.chosenAbility)
                        button.drawColor = Color.Yellow;
                }
            }
        }

        private void ClickedAbility(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;

            for (int b = 0; b < AmountOfAbilities; b++)
            {
                if (listeningElement == abilityButtons[b])
                {
                    buttonIndex = b;
                    buttonClickedOn = abilityButtons[b];
                }
                if (abilityButtons[b].focusedOn)
                {
                    abilityButtons[b].focusedOn = false;
                }
            }

            mPlayer.chosenAbility = buttonIndex;
            buttonClickedOn.focusedOn = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (inputTimer > 0)
                inputTimer--;

            /*if (Math.Abs(storedWheelRotation) > 0.02f)
            {
                storedWheelRotation *= 0.80f;
                wheelRotation += storedWheelRotation;

                for (int i = 0; i < AmountOfAbilities; i++)
                    abilityButtons[i].SetButtonPosiiton(wheelCenter.buttonPosition + (IndexToRadianPosition(i, 6, wheelRotation) * WheelSpace));
            }*/

            /*if (nameTextShowTimer > 0)
            {
                nameTextShowTimer--;

                float posMult = MathHelper.Clamp(nameTextShowTimer, 0f, 120f) / 120f;
                //abilityNameText.Left.Pixels = textMeasurement.X + x;
                abilityNameText.HAlign = x;
            }*/
            if (abilityChanged)
            {
                float currentAngle = (wheelCenter.screenPosition - abilityButtons[mPlayer.chosenAbility].screenPosition).ToRotation();
                if (currentAngle > 0.1f)
                    wheelRotation -= MathHelper.ToRadians(120f);
                else if (currentAngle < 0.1f)
                    wheelRotation += MathHelper.ToRadians(120f);

                if (Math.Abs(currentAngle) <= 0.1f)
                {
                    wheelRotation += -currentAngle / 2f;
                    abilityChanged = false;
                }
            }

            abilityDescriptionsPanel.visible = false;
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                abilityButtons[i].SetButtonPosiiton(wheelCenter.screenPosition + (IndexToRadianPosition(i, 6, wheelRotation) * WheelSpace));

                AdjustableButton button = abilityButtons[i];
                if (button.IsMouseHovering)
                {
                    Main.NewText("Hovering");
                    abilityDescriptionsPanel.ShowText(abilityDescriptions[i]);
                    abilityDescriptionsPanel.ownerPos = new Vector2(Main.screenWidth * abilityWheel.HAlign, Main.screenHeight * abilityWheel.VAlign) - new Vector2(abilityWheel.Width.Pixels / 2f, abilityWheel.Height.Pixels / 2f);      //On screen changes, it had to update
                }
            }

            if (inputTimer <= 0)
            {
                bool keyPressed = false;
                KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Keys.D1) || keyboardState.IsKeyDown(Keys.NumPad1))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 0;
                }
                if (keyboardState.IsKeyDown(Keys.D2) || keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 1;
                }
                if (keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.NumPad3))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 2;
                }
                if (keyboardState.IsKeyDown(Keys.D4) || keyboardState.IsKeyDown(Keys.NumPad4))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 3;
                }
                if (keyboardState.IsKeyDown(Keys.D5) || keyboardState.IsKeyDown(Keys.NumPad5))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 4;
                }
                if (keyboardState.IsKeyDown(Keys.D6) || keyboardState.IsKeyDown(Keys.NumPad6))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 5;
                }

                if (keyPressed)
                {
                    inputTimer += 3;

                    abilityChanged = true;
                    nameTextShowTimer = 5 * 60;
                    abilityNameText.SetText(abilityNames[mPlayer.chosenAbility]);
                    textMeasurement = Main.fontMouseText.MeasureString(abilityNames[mPlayer.chosenAbility]) * TextScale;


                    for (int i = 0; i < AmountOfAbilities; i++)
                    {
                        AdjustableButton button = abilityButtons[i];
                        button.drawColor = Color.White;
                        if (i == mPlayer.chosenAbility)
                            button.drawColor = Color.Yellow;
                    }
                }
            }


            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < AmountOfAbilities; i++)
            {
                AdjustableButton button = abilityButtons[i];
                Vector2 wheelPos = wheelCenter.screenPosition;
                Vector2 vectorDifference = button.screenPosition - wheelPos;
                button.rotation = vectorDifference.ToRotation() + MathHelper.ToRadians(90f);
            }
        }

        /*private Vector2 GetPos(UIElement elmn, SpriteBatch spriteBatch)
        {
            Vector2 pos = new Vector2(elmn.GetClippingRectangle(spriteBatch).X, elmn.GetClippingRectangle(spriteBatch).Y);
            return pos;
        }*/
    }
}