using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands.UI
{
    public class BadCompanyUnitsUI : UIState
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
        private UIImageButton subtractAllSoldiersButton;
        private UIImageButton subtractAllTanksButton;
        private UIImageButton subtractAllChoppersButton;

        private float buttonPadding = 0.14f;
        private float textDistanceFromTexture = 0.3f;

        public override void Update(GameTime gameTime)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            if (!mPlayer.standOut)
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
            UnitsUIPanel.Left.Set(Main.screenWidth * 0.9f, 0f);
            UnitsUIPanel.Top.Set(Main.screenHeight * 0.9f, 0f);
            UnitsUIPanel.Width.Set(180f, 0f);
            UnitsUIPanel.Height.Set(180f, 0f);

            unitsLeftText = new UIText("Units Left: ");
            unitsLeftText.Top.Set(0f, 0f);
            unitsLeftText.Left.Set(20f, 0f);
            unitsLeftText.Width.Set(40f, 0f);
            unitsLeftText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(unitsLeftText);

            Asset<Texture2D> soldierTextureImage = ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/BadCompany/BadCompanySoldier_Prone", AssetRequestMode.ImmediateLoad);
            soldierTexture = new UIImage(soldierTextureImage);
            soldierTexture.HAlign = 0.1f;
            soldierTexture.VAlign = 0.5f;
            soldierTexture.Width.Set(soldierTextureImage.Value.Width, 0f);
            soldierTexture.Height.Set(soldierTextureImage.Value.Height, 0f);
            UnitsUIPanel.Append(soldierTexture);

            soldiersActiveText = new UIText("0");
            soldiersActiveText.HAlign = soldierTexture.HAlign;
            soldiersActiveText.VAlign = soldierTexture.VAlign + textDistanceFromTexture;
            soldiersActiveText.Width.Set(20f, 0f);
            soldiersActiveText.Height.Set(20f, 0f);
            UnitsUIPanel.Append(soldiersActiveText);

            Asset<Texture2D> leftButtonTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/LeftArrow", AssetRequestMode.ImmediateLoad);
            subtractSoldierButton = new UIImageButton(leftButtonTexture);
            subtractSoldierButton.HAlign = soldiersActiveText.HAlign - buttonPadding;
            subtractSoldierButton.VAlign = soldiersActiveText.VAlign;
            subtractSoldierButton.Width.Set(leftButtonTexture.Value.Width, 0f);
            subtractSoldierButton.Height.Set(leftButtonTexture.Value.Height, 0f);
            subtractSoldierButton.OnLeftClick += OnClickSubtractSoldierButton;
            UnitsUIPanel.Append(subtractSoldierButton);

            Asset<Texture2D> rightButtonTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/RightArrow", AssetRequestMode.ImmediateLoad);
            addSoldierButton = new UIImageButton(rightButtonTexture);
            addSoldierButton.HAlign = soldiersActiveText.HAlign + buttonPadding;
            addSoldierButton.VAlign = soldiersActiveText.VAlign;
            addSoldierButton.Width.Set(rightButtonTexture.Value.Width, 0f);
            addSoldierButton.Height.Set(rightButtonTexture.Value.Height, 0f);
            addSoldierButton.OnLeftClick += OnClickAddSoldierButton;
            UnitsUIPanel.Append(addSoldierButton);

            Asset<Texture2D> tankTextureImage = ModContent.Request<Texture2D>("JoJoStands/Extras/UnitsUITank", AssetRequestMode.ImmediateLoad);
            tankTexture = new UIImage(tankTextureImage);
            tankTexture.HAlign = 0.5f;
            tankTexture.VAlign = 0.5f;
            tankTexture.Width.Set(tankTextureImage.Value.Width, 0f);
            tankTexture.Height.Set(tankTextureImage.Value.Height, 0f);
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
            subtractTankButton.Width.Set(leftButtonTexture.Value.Width, 0f);
            subtractTankButton.Height.Set(leftButtonTexture.Value.Height, 0f);
            subtractTankButton.OnLeftClick += OnClickSubtractTankButton;
            UnitsUIPanel.Append(subtractTankButton);

            addTankButton = new UIImageButton(rightButtonTexture);
            addTankButton.HAlign = tanksActiveText.HAlign + buttonPadding;
            addTankButton.VAlign = tanksActiveText.VAlign;
            addTankButton.Width.Set(rightButtonTexture.Value.Width, 0f);
            addTankButton.Height.Set(rightButtonTexture.Value.Height, 0f);
            addTankButton.OnLeftClick += OnClickAddTankButton;
            UnitsUIPanel.Append(addTankButton);

            Asset<Texture2D> chopperTextureImage = ModContent.Request<Texture2D>("JoJoStands/Extras/UnitsUIChopper", AssetRequestMode.ImmediateLoad);
            chopperTexture = new UIImage(chopperTextureImage);
            chopperTexture.HAlign = 0.9f;
            chopperTexture.VAlign = 0.5f;
            chopperTexture.Width.Set(chopperTextureImage.Value.Width, 0f);
            chopperTexture.Height.Set(chopperTextureImage.Value.Height, 0f);
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
            subtractChopperButton.Width.Set(leftButtonTexture.Value.Width, 0f);
            subtractChopperButton.Height.Set(leftButtonTexture.Value.Height, 0f);
            subtractChopperButton.OnLeftClick += OnClickSubtractChopperButton;
            UnitsUIPanel.Append(subtractChopperButton);

            addChopperButton = new UIImageButton(rightButtonTexture);
            addChopperButton.HAlign = choppersActiveText.HAlign + buttonPadding;
            addChopperButton.VAlign = choppersActiveText.VAlign;
            addChopperButton.Width.Set(rightButtonTexture.Value.Width, 0f);
            addChopperButton.Height.Set(rightButtonTexture.Value.Height, 0f);
            addChopperButton.OnLeftClick += OnClickAddChopperButton;
            UnitsUIPanel.Append(addChopperButton);

            Asset<Texture2D> XButtonTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/XButton", AssetRequestMode.ImmediateLoad);
            subtractAllSoldiersButton = new UIImageButton(XButtonTexture);
            subtractAllSoldiersButton.HAlign = soldiersActiveText.HAlign - 0.03f;
            subtractAllSoldiersButton.VAlign = soldiersActiveText.VAlign + 0.2f;
            subtractAllSoldiersButton.Width.Set(XButtonTexture.Value.Width, 0f);
            subtractAllSoldiersButton.Height.Set(XButtonTexture.Value.Height, 0f);
            subtractAllSoldiersButton.OnLeftClick += OnClickSubtractAllSoldiers;
            UnitsUIPanel.Append(subtractAllSoldiersButton);

            subtractAllTanksButton = new UIImageButton(XButtonTexture);
            subtractAllTanksButton.HAlign = tanksActiveText.HAlign;
            subtractAllTanksButton.VAlign = tanksActiveText.VAlign + 0.2f;
            subtractAllTanksButton.Width.Set(XButtonTexture.Value.Width, 0f);
            subtractAllTanksButton.Height.Set(XButtonTexture.Value.Height, 0f);
            subtractAllTanksButton.OnLeftClick += OnClickSubtractAllTanks;
            UnitsUIPanel.Append(subtractAllTanksButton);

            subtractAllChoppersButton = new UIImageButton(XButtonTexture);
            subtractAllChoppersButton.HAlign = choppersActiveText.HAlign + 0.03f;
            subtractAllChoppersButton.VAlign = choppersActiveText.VAlign + 0.2f;
            subtractAllChoppersButton.Width.Set(XButtonTexture.Value.Width, 0f);
            subtractAllChoppersButton.Height.Set(XButtonTexture.Value.Height, 0f);
            subtractAllChoppersButton.OnLeftClick += OnClickSubtractAllChoppers;
            UnitsUIPanel.Append(subtractAllChoppersButton);

            Append(UnitsUIPanel);
        }

        private void OnClickSubtractAllSoldiers(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!Main.gamePaused)
                mPlayer.badCompanySoldiers = 0;
        }

        private void OnClickSubtractAllTanks(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!Main.gamePaused)
                mPlayer.badCompanyTanks = 0;
        }

        private void OnClickSubtractAllChoppers(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (!Main.gamePaused)
                mPlayer.badCompanyChoppers = 0;
        }

        private void OnClickSubtractSoldierButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.badCompanySoldiers > 0 && !Main.gamePaused)
                mPlayer.badCompanySoldiers--;
        }

        private void OnClickAddSoldierButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.badCompanyUnitsLeft >= 1 && !Main.gamePaused)
                mPlayer.badCompanySoldiers++;
        }

        private void OnClickSubtractTankButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.standTier >= 2)
            {
                if (mPlayer.badCompanyTanks > 0 && !Main.gamePaused) 
                    mPlayer.badCompanyTanks--;
            }
        }

        private void OnClickAddTankButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.standTier >= 2 && !Main.gamePaused)
            {
                if (mPlayer.badCompanyUnitsLeft >= 4)
                    mPlayer.badCompanyTanks++;
            }
        }

        private void OnClickSubtractChopperButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.standTier >= 3)
            {
                if (mPlayer.badCompanyChoppers > 0 && !Main.gamePaused)
                    mPlayer.badCompanyChoppers--;
            }
        }

        private void OnClickAddChopperButton(UIMouseEvent evt, UIElement listeningElement)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standTier == 0)
                return;

            if (mPlayer.standTier >= 3)
            {
                if (mPlayer.badCompanyUnitsLeft >= 6 && !Main.gamePaused)
                    mPlayer.badCompanyChoppers++;
            }
        }
    }
}