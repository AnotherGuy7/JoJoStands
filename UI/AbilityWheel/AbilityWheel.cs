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
    public class AbilityWheel : UIState
    {
        public static MyPlayer mPlayer;

        public virtual string centerTexturePath { get; }
        public virtual string[] abilityNames { get; }

        public virtual string[] abilityTextureNames { get; }


        public virtual string[] abilityDescriptions { get; }
        public virtual float wheelSpace { get; } = 50f;
        public virtual float textScale { get; } = 0.8f;
        public virtual int amountOfAbilities { get; }
        public virtual string buttonTexturePath { get; }

        public UIPanel abilityWheel;
        public AdjustableButton wheelCenter;
        public AdjustableButton[] abilityButtons;
        public MouseTextPanel abilityDescriptionsPanel;
        public UIText abilityNameText;
        public Vector2 wheelAlignPosition;
        public float wheelRotation = 0f;
        public int abilitiesShown = 0;

        private int inputTimer = 0;
        private int previousScrollValue = 0;
        private bool abilityChanged = false;
        private int alphaTimer = 0;

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
            abilityWheel.HAlign = 0.98f;
            abilityWheel.VAlign = 0.5f;
            abilityWheel.Width.Set(120f, 0f);
            abilityWheel.Height.Set(120f, 0f);
            abilityWheel.BackgroundColor = Color.Transparent;
            abilityWheel.BorderColor = Color.Transparent;
            abilityWheel.OnScrollWheel += MouseScroll;

            wheelCenter = new AdjustableButton(ModContent.Request<Texture2D>("JoJoStands/UI/AbilityWheel/WheelCenter"), Vector2.Zero, new Vector2(54f), Color.White, 1f, 1f);
            wheelCenter.SetButtonPosiiton(new Vector2(60f));
            wheelCenter.SetButtonSize(new Vector2(54f));
            wheelCenter.SetOverlayImage((Texture2D)ModContent.Request<Texture2D>(centerTexturePath), 0.15f);
            wheelCenter.owner = abilityWheel;
            wheelCenter.respondToFocus = false;
            abilityWheel.Append(wheelCenter);

            abilityButtons = new AdjustableButton[amountOfAbilities];
            for (int i = 0; i < amountOfAbilities; i++)
            {
                abilityButtons[i] = new AdjustableButton(ModContent.Request<Texture2D>("JoJoStands/UI/AbilityWheel/WheelPiece"), wheelCenter.buttonCenter + (IndexToRadianPosition(i, 6) * wheelSpace), new Vector2(38f), Color.White, 1f, 1f);
                abilityButtons[i].SetOverlayImage((Texture2D)ModContent.Request<Texture2D>(buttonTexturePath + abilityTextureNames[i]), 0.15f);
                abilityButtons[i].OnClick += ClickedAbility;
                abilityButtons[i].OnScrollWheel += MouseScroll;
                //abilityButtons[i].owner = abilityWheel;
                abilityWheel.Append(abilityButtons[i]);
            }

            abilityDescriptionsPanel = new MouseTextPanel(480, 1, "None", true, 24);
            abilityDescriptionsPanel.ownerPos = new Vector2(Main.screenWidth * abilityWheel.HAlign, Main.screenHeight * abilityWheel.VAlign);
            abilityWheel.Append(abilityDescriptionsPanel);

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

                for (int i = 0; i < abilitiesShown; i++)
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
            if (inputTimer > 0)
                inputTimer--;
            if (alphaTimer > 0)
                alphaTimer--;

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
            for (int i = 0; i < abilitiesShown; i++)
            {
                abilityButtons[i].SetButtonPosiiton(wheelCenter.screenPosition + (IndexToRadianPosition(i, abilitiesShown, wheelRotation) * wheelSpace));

                AdjustableButton button = abilityButtons[i];
                if (button.IsMouseHovering)
                {
                    abilityDescriptionsPanel.ShowText(abilityDescriptions[i]);
                    abilityDescriptionsPanel.ownerPos = wheelAlignPosition - new Vector2(abilityWheel.Width.Pixels / 2f, abilityWheel.Height.Pixels / 2f);      //On screen changes, it had to update
                }
            }

            if (inputTimer <= 0)
            {
                bool keyPressed = false;
                KeyboardState keyboardState = Keyboard.GetState();

                if (abilitiesShown >= 1 && (keyboardState.IsKeyDown(Keys.D1) || keyboardState.IsKeyDown(Keys.NumPad1)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 0;
                }
                if (abilitiesShown >= 2 && (keyboardState.IsKeyDown(Keys.D2) || keyboardState.IsKeyDown(Keys.NumPad2)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 1;
                }
                if (abilitiesShown >= 3 && (keyboardState.IsKeyDown(Keys.D3) || keyboardState.IsKeyDown(Keys.NumPad3)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 2;
                }
                if (abilitiesShown >= 4 && (keyboardState.IsKeyDown(Keys.D4) || keyboardState.IsKeyDown(Keys.NumPad4)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 3;
                }
                if (abilitiesShown >= 5 && (keyboardState.IsKeyDown(Keys.D5) || keyboardState.IsKeyDown(Keys.NumPad5)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 4;
                }
                if (abilitiesShown >= 6 && (keyboardState.IsKeyDown(Keys.D6) || keyboardState.IsKeyDown(Keys.NumPad6)))
                {
                    keyPressed = true;
                    mPlayer.chosenAbility = 5;
                }

                if (keyPressed)
                {
                    inputTimer += 3;

                    alphaTimer = 5 * 60;
                    abilityChanged = true;
                    abilityNameText.SetText(abilityNames[mPlayer.chosenAbility]);

                    for (int i = 0; i < abilitiesShown; i++)
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
            float alpha = 0.4f;
            if (Vector2.Distance(wheelAlignPosition + wheelCenter.buttonPosition, Main.MouseScreen) <= 60)
                alpha = 1f;

            if (alphaTimer > 0)
            {
                alpha = 1f;
                alphaTimer--;
            }

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