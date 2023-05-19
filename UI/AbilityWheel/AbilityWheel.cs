using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using static JoJoStands.UI.GlobalMouseTextPanel;

namespace JoJoStands.UI
{
    public class AbilityWheel : UIState
    {
        public static MyPlayer mPlayer;
        public static float VerticalAlignmentPercentage = 0.5f;
        private readonly int WheelFadeTime = 4 * 60;
        private readonly int WheelFadeDivider = 3 * 60;
        public readonly Vector2 AbilityWheelSize = new Vector2(54f);

        public virtual string centerTexturePath { get; }
        public virtual string[] abilityNames { get; }

        public virtual string[] abilityTextureNames { get; }


        public virtual string[] abilityDescriptions { get; }
        public virtual float wheelSpace { get; } = 50f;
        public virtual float textScale { get; } = 0.8f;
        public virtual int amountOfAbilities { get; }
        public virtual string buttonTexturePath { get; }

        private readonly Dictionary<int, Keys[]> NumberToKeysDict = new Dictionary<int, Keys[]>
        {
            { 0, new Keys[2] { Keys.D1, Keys.NumPad1 } },
            { 1, new Keys[2] { Keys.D2, Keys.NumPad2 } },
            { 2, new Keys[2] { Keys.D3, Keys.NumPad3 } },
            { 3, new Keys[2] { Keys.D4, Keys.NumPad4 } },
            { 4, new Keys[2] { Keys.D5, Keys.NumPad5 } },
            { 5, new Keys[2] { Keys.D6, Keys.NumPad6 } },
            { 6, new Keys[2] { Keys.D7, Keys.NumPad7 } },
            { 7, new Keys[2] { Keys.D8, Keys.NumPad8 } },
            { 8, new Keys[2] { Keys.D9, Keys.NumPad9 } },
            { 9, new Keys[2] { Keys.D0, Keys.NumPad0 } }
        };

        public UIPanel abilityWheel;
        public AdjustableButton wheelCenter;
        public AdjustableButton[] abilityButtons;
        public UIText abilityNameText;
        public Vector2 wheelCenterPosition;
        public float wheelRotation = 0f;
        public int abilitiesShown = 0;

        private int inputTimer = 0;
        private int previousScrollValue = 0;
        private bool abilityChanged = false;
        private int alphaTimer = 5 * 60;

        /*public override void OnInitialize()
        {
            abilityWheel = new UIPanel();
            abilityWheel.HAlign = 1f;
            abilityWheel.VAlign = 0.5f;
            abilityWheel.Width.Set(180f, 0f);
            abilityWheel.Height.Set(180f, 0f);

            Append(abilityWheel);
        }*/
        public override void OnInitialize()
        {
            abilityWheel = new UIPanel();
            abilityWheel.HAlign = HAlign;
            abilityWheel.VAlign = VAlign;
            abilityWheel.Width.Set(120f, 0f);
            abilityWheel.Height.Set(120f, 0f);
            abilityWheel.BackgroundColor = Color.Transparent;
            abilityWheel.BorderColor = Color.Transparent;

            wheelCenter = new AdjustableButton(ModContent.Request<Texture2D>("JoJoStands/UI/AbilityWheel/WheelCenter", AssetRequestMode.ImmediateLoad), Vector2.Zero, AbilityWheelSize, Color.White, 1f, 1f);
            wheelCenter.SetButtonPosiiton(new Vector2(60f));
            wheelCenter.SetButtonSize(AbilityWheelSize);
            wheelCenter.SetOverlayImage((Texture2D)ModContent.Request<Texture2D>(centerTexturePath, AssetRequestMode.ImmediateLoad), 0.15f);
            wheelCenter.owner = abilityWheel;
            wheelCenter.respondToFocus = false;
            abilityWheel.Append(wheelCenter);

            abilityButtons = new AdjustableButton[amountOfAbilities];
            for (int i = 0; i < amountOfAbilities; i++)
            {
                abilityButtons[i] = new AdjustableButton(ModContent.Request<Texture2D>("JoJoStands/UI/AbilityWheel/WheelPiece", AssetRequestMode.ImmediateLoad), wheelCenter.buttonCenter + (IndexToRadianPosition(i, 6) * wheelSpace), new Vector2(38f), Color.White, 1f, 1f);
                abilityButtons[i].SetOverlayImage((Texture2D)ModContent.Request<Texture2D>(buttonTexturePath + abilityTextureNames[i], AssetRequestMode.ImmediateLoad), 0.15f);
                abilityButtons[i].OnLeftClick += ClickedAbility;
                //abilityButtons[i].owner = abilityWheel;
                abilityWheel.Append(abilityButtons[i]);
            }

            abilityNameText = new UIText("A", textScale);
            abilityNameText.Left.Pixels = 0f;
            abilityNameText.Top.Pixels = 100f;
            abilityNameText.Width.Pixels = 300f;
            abilityNameText.Height.Pixels = 20f;
            Append(abilityNameText);

            ExtraInitialize();

            Append(abilityWheel);
        }

        public virtual void ExtraInitialize()
        { }

        private void ClickedAbility(UIMouseEvent evt, UIElement listeningElement)
        {
            int buttonIndex = -1;
            AdjustableButton buttonClickedOn = null;

            for (int b = 0; b < abilitiesShown; b++)
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
            Vector2 placementVector = Vector2.Transform(new Vector2(Main.screenWidth - (AbilityWheelSize.X / 2f), Main.screenHeight * VerticalAlignmentPercentage), Main.UIScaleMatrix);
            abilityWheel.Left.Set(placementVector.X, 0f);
            abilityWheel.Top.Set(placementVector.Y, 0f);
            if (inputTimer > 0)
                inputTimer--;
            if (alphaTimer > 0)
                alphaTimer--;
            wheelCenterPosition = new Vector2(placementVector.X, placementVector.Y);

            int newScrollValue = Mouse.GetState().ScrollWheelValue;
            int wheelDifference = previousScrollValue - newScrollValue;
            if (wheelDifference != 0 && Vector2.Distance(wheelCenterPosition, Main.MouseScreen) < 60f && inputTimer <= 0)
            {
                //inputTimer += 3;
                previousScrollValue = newScrollValue;
                if (wheelDifference > 0)
                {
                    mPlayer.chosenAbility -= 1;
                    if (mPlayer.chosenAbility < 0)
                        mPlayer.chosenAbility = abilitiesShown - 1;
                }
                else
                {
                    mPlayer.chosenAbility += 1;
                    if (mPlayer.chosenAbility > abilitiesShown - 1)
                        mPlayer.chosenAbility = 0;
                }
                abilityChanged = true;
                abilityNameText.SetText(abilityNames[mPlayer.chosenAbility]);
                alphaTimer = WheelFadeTime;

                for (int i = 0; i < abilitiesShown; i++)
                {
                    AdjustableButton button = abilityButtons[i];
                    button.drawColor = Color.Gray;
                    if (i == mPlayer.chosenAbility)
                        button.drawColor = Color.White;
                }
            }

            if (abilityChanged)
            {
                float currentAngle = (wheelCenter.screenPosition - abilityButtons[mPlayer.chosenAbility].screenPosition).ToRotation();
                if (currentAngle > 0.05f)
                    wheelRotation -= MathHelper.ToRadians(290f);
                else if (currentAngle < -0.05f)
                    wheelRotation += MathHelper.ToRadians(290f);

                if (Math.Abs(currentAngle) <= 0.05f)
                {
                    wheelRotation += -currentAngle / 2f;
                    abilityChanged = false;
                }
            }

            if (JoJoStands.AbilityWheelDescriptions)
            {
                bool mouseHoveringOverButton = false;
                for (int i = 0; i < abilitiesShown; i++)
                {
                    abilityButtons[i].SetButtonPosiiton(wheelCenter.screenPosition + (IndexToRadianPosition(i, abilitiesShown, wheelRotation) * wheelSpace));

                    AdjustableButton button = abilityButtons[i];
                    if (button.IsButtonHoveredOver())
                    {
                        globalMouseTextPanel.ShowText(abilityDescriptions[i], 32);
                        mouseHoveringOverButton = true;
                    }
                }
                globalMouseTextPanel.visible = mouseHoveringOverButton;
            }

            if (inputTimer <= 0)
            {
                bool keyPressed = false;
                KeyboardState keyboardState = Keyboard.GetState();
                for (int i = 0; i < abilitiesShown; i++)
                {
                    keyPressed = keyboardState.IsKeyDown(NumberToKeysDict[i][0]) || keyboardState.IsKeyDown(NumberToKeysDict[i][1]);
                    if (keyPressed)
                    {
                        mPlayer.chosenAbility = i;
                        break;
                    }
                }

                if (keyPressed)
                {
                    inputTimer += 3;

                    alphaTimer = WheelFadeTime;
                    abilityChanged = true;
                    abilityNameText.SetText(abilityNames[mPlayer.chosenAbility]);

                    for (int i = 0; i < abilitiesShown; i++)
                    {
                        AdjustableButton button = abilityButtons[i];
                        button.drawColor = Color.Gray;
                        if (i == mPlayer.chosenAbility)
                            button.drawColor = Color.White;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float alpha = MathHelper.Clamp(0.6f * (alphaTimer / (float)WheelFadeDivider), 0f, 0.6f) + 0.4f;
            if (Vector2.Distance(wheelCenterPosition + wheelCenter.buttonPosition, Main.MouseScreen) <= 60)
                alpha = 1f;

            for (int i = 0; i < abilitiesShown; i++)
            {
                AdjustableButton button = abilityButtons[i];
                Vector2 wheelPos = wheelCenter.screenPosition;
                Vector2 vectorDifference = button.screenPosition - wheelPos;
                button.rotation = vectorDifference.ToRotation() + MathHelper.ToRadians(90f);
                button.drawAlpha = alpha;
            }
            wheelCenter.drawAlpha = alpha;
            abilityNameText.TextColor = Color.White * alpha;
        }

        public Vector2 IndexToRadianPosition(int index, int total)
        {
            float angle = (360f / (float)total) * index;
            angle = MathHelper.ToRadians(angle);

            return angle.ToRotationVector2();
        }

        public Vector2 IndexToRadianPosition(int index, int total, float angleAdd)
        {
            float angle = (360f / (float)total) * index;
            angle += angleAdd;
            angle = MathHelper.ToRadians(angle);

            return angle.ToRotationVector2();
        }
    }
}