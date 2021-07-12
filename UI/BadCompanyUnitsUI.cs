using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    internal class BadCompanyUnitsUI : UIState
    {
        public DragableUIPanel UnitsUIPanel;
        public static bool Visible;

        private UIText unitsLeftText;
        private UIText soldiersActiveText;
        private UIImage soldierTexture;
        private UIImageButton addSoldierButton;
        private UIImageButton subtractSoldierButton;
        private UIText tanksActiveText;
        private UIImage tankTexture;
        private UIImageButton addTankButton;
        private UIImageButton subtractTankButton;
        private UIText choppersActiveText;
        private UIImage chopperTexture;
        private UIImageButton addChopperButton;
        private UIImageButton subtractChopperButton;

        private float buttonPadding = 0.14f;
        private float textDistanceFromTexture = 0.3f;

        public override void Update(GameTime gameTime)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier != 0)
                Visible = false;

            unitsLeftText.SetText("Units Left: " + mPlayer.badCompanyUnitsLeft);
            soldiersActiveText.SetText(mPlayer.badCompanySoldiers.ToString());
            tanksActiveText.SetText(mPlayer.badCompanyTanks.ToString());
            choppersActiveText.SetText(mPlayer.badCompanyChoppers.ToString());

            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            UnitsUIPanel = new DragableUIPanel();
            UnitsUIPanel.HAlign = 0.9f;
            UnitsUIPanel.VAlign = 0.9f;
            UnitsUIPanel.Width.Set(180f, 0f);
            UnitsUIPanel.Height.Set(180f, 0f);

            unitsLeftText = new UIText("Units Left: ");
            unitsLeftText.Top.Set(0f, 0f);
            unitsLeftText.Left.Set(20f, 0f);
            unitsLeftText.Width.Set(40f, 0f);
            unitsLeftText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(unitsLeftText);

            Texture2D soldierTextureImage = ModContent.GetTexture("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanySoldier_Prone");
            soldierTexture = new UIImage(soldierTextureImage);
            soldierTexture.HAlign = 0.1f;
            soldierTexture.VAlign = 0.5f;
            soldierTexture.Width.Set(soldierTextureImage.Width, 0f);
            soldierTexture.Height.Set(soldierTextureImage.Height, 0f);
            UnitsUIPanel.Append(soldierTexture);

            soldiersActiveText = new UIText("0");
            soldiersActiveText.HAlign = soldierTexture.HAlign;
            soldiersActiveText.VAlign = soldierTexture.VAlign + textDistanceFromTexture;
            soldiersActiveText.Width.Set(20f, 0f);
            soldiersActiveText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(soldiersActiveText);

            Texture2D leftButtonTexture = ModContent.GetTexture("JoJoStands/Extras/LeftArrow");
            subtractSoldierButton = new UIImageButton(leftButtonTexture);
            subtractSoldierButton.HAlign = soldiersActiveText.HAlign - buttonPadding;
            subtractSoldierButton.VAlign = soldiersActiveText.VAlign;
            subtractSoldierButton.Width.Set(leftButtonTexture.Width, 0f);
            subtractSoldierButton.Height.Set(leftButtonTexture.Height, 0f);
            subtractSoldierButton.OnClick += OnClickSubtractSoldierButton;
            UnitsUIPanel.Append(subtractSoldierButton);

            Texture2D rightButtonTexture = ModContent.GetTexture("JoJoStands/Extras/RightArrow");
            addSoldierButton = new UIImageButton(rightButtonTexture);
            addSoldierButton.HAlign = soldiersActiveText.HAlign + buttonPadding;
            addSoldierButton.VAlign = soldiersActiveText.VAlign;
            addSoldierButton.Width.Set(rightButtonTexture.Width, 0f);
            addSoldierButton.Height.Set(rightButtonTexture.Height, 0f);
            addSoldierButton.OnClick += OnClickAddSoldierButton;
            UnitsUIPanel.Append(addSoldierButton);

            Texture2D tankTextureImage = ModContent.GetTexture("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanyTank");
            tankTexture = new UIImage(tankTextureImage);
            tankTexture.HAlign = 0.5f;
            tankTexture.VAlign = 0.5f;
            tankTexture.Width.Set(tankTextureImage.Width, 0f);
            tankTexture.Height.Set(tankTextureImage.Height, 0f);
            UnitsUIPanel.Append(tankTexture);

            tanksActiveText = new UIText("0");
            tanksActiveText.HAlign = tankTexture.HAlign;
            tanksActiveText.VAlign = tankTexture.VAlign + textDistanceFromTexture;
            tanksActiveText.Width.Set(20f, 0f);
            tanksActiveText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(tanksActiveText);

            subtractTankButton = new UIImageButton(leftButtonTexture);
            subtractTankButton.HAlign = tanksActiveText.HAlign - buttonPadding;
            subtractTankButton.VAlign = tanksActiveText.VAlign;
            subtractTankButton.Width.Set(leftButtonTexture.Width, 0f);
            subtractTankButton.Height.Set(leftButtonTexture.Height, 0f);
            subtractTankButton.OnClick += OnClickSubtractTankButton;
            UnitsUIPanel.Append(subtractTankButton);

            addTankButton = new UIImageButton(rightButtonTexture);
            addTankButton.HAlign = tanksActiveText.HAlign + buttonPadding;
            addTankButton.VAlign = tanksActiveText.VAlign;
            addTankButton.Width.Set(rightButtonTexture.Width, 0f);
            addTankButton.Height.Set(rightButtonTexture.Height, 0f);
            addTankButton.OnClick += OnClickAddTankButton;
            UnitsUIPanel.Append(addTankButton);

            Texture2D chopperTextureImage = ModContent.GetTexture("JoJoStands/Extras/UnitsUIChopper");
            chopperTexture = new UIImage(chopperTextureImage);
            chopperTexture.HAlign = 0.9f;
            chopperTexture.VAlign = 0.5f;
            chopperTexture.Width.Set(chopperTextureImage.Width, 0f);
            chopperTexture.Height.Set(chopperTextureImage.Height, 0f);
            UnitsUIPanel.Append(chopperTexture);

            choppersActiveText = new UIText("0");
            choppersActiveText.HAlign = chopperTexture.HAlign;
            choppersActiveText.VAlign = chopperTexture.VAlign + textDistanceFromTexture;
            choppersActiveText.Width.Set(20f, 0f);
            choppersActiveText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(choppersActiveText);

            subtractChopperButton = new UIImageButton(leftButtonTexture);
            subtractChopperButton.HAlign = choppersActiveText.HAlign - buttonPadding;
            subtractChopperButton.VAlign = choppersActiveText.VAlign;
            subtractChopperButton.Width.Set(leftButtonTexture.Width, 0f);
            subtractChopperButton.Height.Set(leftButtonTexture.Height, 0f);
            subtractChopperButton.OnClick += OnClickSubtractChopperButton;
            UnitsUIPanel.Append(subtractChopperButton);

            addChopperButton = new UIImageButton(rightButtonTexture);
            addChopperButton.HAlign = choppersActiveText.HAlign + buttonPadding;
            addChopperButton.VAlign = choppersActiveText.VAlign;
            addChopperButton.Width.Set(rightButtonTexture.Width, 0f);
            addChopperButton.Height.Set(rightButtonTexture.Height, 0f);
            addChopperButton.OnClick += OnClickAddChopperButton;
            UnitsUIPanel.Append(addChopperButton);

            Append(UnitsUIPanel);
        }

        private void OnClickSubtractSoldierButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanySoldiers > 0)
            {
                mPlayer.badCompanySoldiers--;
            }
        }

        private void OnClickAddSoldierButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanyUnitsLeft >= 1)
            {
                mPlayer.badCompanySoldiers++;
            }
        }

        private void OnClickSubtractTankButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanyTier >= 2)
            {
                if (mPlayer.badCompanyTanks > 0)
                {
                    mPlayer.badCompanyTanks--;
                }
            }
        }

        private void OnClickAddTankButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanyTier >= 2)
            {
                if (mPlayer.badCompanyUnitsLeft >= 4)
                {
                    mPlayer.badCompanyTanks++;
                }
            }
        }

        private void OnClickSubtractChopperButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanyTier >= 3)
            {
                if (mPlayer.badCompanyChoppers > 0)
                {
                    mPlayer.badCompanyChoppers--;
                }
            }
        }

        private void OnClickAddChopperButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.badCompanyTier == 0)
                return;

            if (mPlayer.badCompanyTier >= 3)
            {
                if (mPlayer.badCompanyUnitsLeft >= 6)
                {
                    mPlayer.badCompanyChoppers++;
                }
            }
        }
    }
}