using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands
{
    public class JoJoStandsSystem : ModSystem
    {
        public static ToBeContinued TBCarrow;
        public static HamonBar HamonBarInterface;
        public static GoldenSpinMeter GoldenSpinInterface;
        public static BulletCounter bulletCounter;
        public static AerosmithRadar aerosmithRadar;
        public static BetUI betUI;
        public static SexPistolsUI sexPistolsUI;
        public static VoidBar VoidBarUI;
        public static BadCompanyUnitsUI UnitsUI;
        public static StoneFreeAbilityWheel StoneFreeAbilityWheelUI;
        public static GoldExperienceAbilityWheel GoldExperienceAbilityWheelUI;
        public static GoldExperienceRequiemAbilityWheel GoldExperienceRequiemAbilityWheelUI;

        private UserInterface _betUI;
        private UserInterface _hamonbarInterface;
        private UserInterface _goldenSpinInterface;
        private UserInterface _tbcarrow;
        private UserInterface _bulletcounter;
        private UserInterface _aerosmithRadar;
        private UserInterface _sexPistolsUI;
        private UserInterface _voidbarUI;
        private UserInterface _hamonSkillTreeUI;
        private UserInterface _unitsUI;
        private UserInterface _zombieSkillTreeUI;
        private UserInterface _stoneFreeAbilityWheelUI;
        private UserInterface _goldExperienceAbilityWheelUI;
        private UserInterface _goldExperienceRequiemAbilityWheelUI;

        public override void OnModLoad()
        {
            if (!Main.dedServ)      //Manages resource loading cause the server isn't able to load resources
            {
                //UI Stuff
                HamonBarInterface = new HamonBar();
                HamonBarInterface.Activate();
                _hamonbarInterface = new UserInterface();
                _hamonbarInterface.SetState(HamonBarInterface);

                TBCarrow = new ToBeContinued();
                TBCarrow.Activate();
                _tbcarrow = new UserInterface();
                _tbcarrow.SetState(TBCarrow);

                bulletCounter = new BulletCounter();
                bulletCounter.Activate();
                _bulletcounter = new UserInterface();
                _bulletcounter.SetState(bulletCounter);

                aerosmithRadar = new AerosmithRadar();
                aerosmithRadar.Activate();
                _aerosmithRadar = new UserInterface();
                _aerosmithRadar.SetState(aerosmithRadar);

                betUI = new BetUI();
                betUI.Activate();
                _betUI = new UserInterface();
                _betUI.SetState(betUI);

                GoldenSpinInterface = new GoldenSpinMeter();
                GoldenSpinInterface.Activate();
                _goldenSpinInterface = new UserInterface();
                _goldenSpinInterface.SetState(GoldenSpinInterface);

                sexPistolsUI = new SexPistolsUI();
                sexPistolsUI.Activate();
                _sexPistolsUI = new UserInterface();
                _sexPistolsUI.SetState(sexPistolsUI);

                VoidBarUI = new VoidBar();
                VoidBarUI.Activate();
                _voidbarUI = new UserInterface();
                _voidbarUI.SetState(VoidBarUI);

                UnitsUI = new BadCompanyUnitsUI();
                UnitsUI.Activate();
                _unitsUI = new UserInterface();
                _unitsUI.SetState(UnitsUI);

                StoneFreeAbilityWheelUI = new StoneFreeAbilityWheel();
                StoneFreeAbilityWheelUI.Activate();
                _stoneFreeAbilityWheelUI = new UserInterface();
                _stoneFreeAbilityWheelUI.SetState(StoneFreeAbilityWheelUI);

                GoldExperienceAbilityWheelUI = new GoldExperienceAbilityWheel();
                GoldExperienceAbilityWheelUI.Activate();
                _goldExperienceAbilityWheelUI = new UserInterface();
                _goldExperienceAbilityWheelUI.SetState(GoldExperienceAbilityWheelUI);

                GoldExperienceRequiemAbilityWheelUI = new GoldExperienceRequiemAbilityWheel();
                GoldExperienceRequiemAbilityWheelUI.Activate();
                _goldExperienceRequiemAbilityWheelUI = new UserInterface();
                _goldExperienceRequiemAbilityWheelUI.SetState(GoldExperienceRequiemAbilityWheelUI);
            }
        }

        public override void Unload()
        {
            TBCarrow = null;
            HamonBarInterface = null;
            GoldenSpinInterface = null;
            bulletCounter = null;
            aerosmithRadar = null;
            betUI = null;
            sexPistolsUI = null;
            VoidBarUI = null;
            UnitsUI = null;
            StoneFreeAbilityWheelUI = null;
            for (int i = 0; i < StoneFreeAbilityWheel.stoneFreeAbilityWheel.abilityButtons.Length; i++)
                StoneFreeAbilityWheel.stoneFreeAbilityWheel.abilityButtons[i] = null;
            StoneFreeAbilityWheel.stoneFreeAbilityWheel.abilityDescriptionsPanel = null;
            StoneFreeAbilityWheel.stoneFreeAbilityWheel = null;
            GoldExperienceAbilityWheelUI = null;
            for (int i = 0; i < GoldExperienceAbilityWheel.goldExperienceAbilityWheel.abilityButtons.Length; i++)
                GoldExperienceAbilityWheel.goldExperienceAbilityWheel.abilityButtons[i] = null;
            GoldExperienceAbilityWheel.goldExperienceAbilityWheel.abilityDescriptionsPanel = null;
            GoldExperienceAbilityWheel.goldExperienceAbilityWheel = null;
            GoldExperienceRequiemAbilityWheelUI = null;
            for (int i = 0; i < GoldExperienceRequiemAbilityWheel.goldExperienceRequiemAbilityWheel.abilityButtons.Length; i++)
                GoldExperienceRequiemAbilityWheel.goldExperienceRequiemAbilityWheel.abilityButtons[i] = null;
            GoldExperienceRequiemAbilityWheel.goldExperienceRequiemAbilityWheel.abilityDescriptionsPanel = null;
            GoldExperienceRequiemAbilityWheel.goldExperienceRequiemAbilityWheel = null;
            AbilityWheel.mPlayer = null;

            _betUI = null;
            _hamonbarInterface = null;
            _goldenSpinInterface = null;
            _tbcarrow = null;
            _bulletcounter = null;
            _aerosmithRadar = null;
            _sexPistolsUI = null;
            _voidbarUI = null;
            _hamonSkillTreeUI = null;
            _unitsUI = null;
            _zombieSkillTreeUI = null;
            _stoneFreeAbilityWheelUI = null;
            _goldExperienceAbilityWheelUI = null;
            _goldExperienceRequiemAbilityWheelUI = null;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            MyPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            mPlayer.Draw(spriteBatch);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (HamonBar.visible)
                _hamonbarInterface.Update(gameTime);

            if (ToBeContinued.Visible)
                _tbcarrow.Update(gameTime);

            if (BulletCounter.Visible)
                _bulletcounter.Update(gameTime);

            if (AerosmithRadar.Visible)
                _aerosmithRadar.Update(gameTime);

            if (BetUI.Visible)
                _betUI.Update(gameTime);

            if (GoldenSpinMeter.Visible)
                _goldenSpinInterface.Update(gameTime);

            if (SexPistolsUI.Visible)
                _sexPistolsUI.Update(gameTime);

            if (VoidBar.Visible)
                _voidbarUI.Update(gameTime);

            if (BadCompanyUnitsUI.Visible)
                _unitsUI.Update(gameTime);

            if (StoneFreeAbilityWheel.visible)
                _stoneFreeAbilityWheelUI.Update(gameTime);

            if (GoldExperienceAbilityWheel.visible)
                _goldExperienceAbilityWheelUI.Update(gameTime);

            if (GoldExperienceRequiemAbilityWheel.visible)
                _goldExperienceRequiemAbilityWheelUI.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)     //from ExampleMod's ExampleUI
        {
            layers.Insert(5, new LegacyGameInterfaceLayer("JoJoStands: UI", DrawUI, InterfaceScaleType.UI));     //from Terraria Interface for Dummies, and Insert so it doesn't draw over everything
        }

        private bool DrawUI()       //also from Terraria Interface for Dummies
        {
            if (HamonBar.visible)
                _hamonbarInterface.Draw(Main.spriteBatch, new GameTime());

            if (ToBeContinued.Visible)
                _tbcarrow.Draw(Main.spriteBatch, new GameTime());

            if (BulletCounter.Visible)
                _bulletcounter.Draw(Main.spriteBatch, new GameTime());

            if (AerosmithRadar.Visible)
                _aerosmithRadar.Draw(Main.spriteBatch, new GameTime());

            if (BetUI.Visible)
                _betUI.Draw(Main.spriteBatch, new GameTime());

            if (GoldenSpinMeter.Visible)
                _goldenSpinInterface.Draw(Main.spriteBatch, new GameTime());

            if (SexPistolsUI.Visible)
                _sexPistolsUI.Draw(Main.spriteBatch, new GameTime());

            if (VoidBar.Visible)
                _voidbarUI.Draw(Main.spriteBatch, new GameTime());

            if (BadCompanyUnitsUI.Visible)
                _unitsUI.Draw(Main.spriteBatch, new GameTime());

            if (StoneFreeAbilityWheel.visible)
                _stoneFreeAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            if (GoldExperienceAbilityWheel.visible)
                _goldExperienceAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            if (GoldExperienceRequiemAbilityWheel.visible)
                _goldExperienceRequiemAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            return true;
        }
    }
}
