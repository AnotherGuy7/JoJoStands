using JoJoStands.Networking;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands
{
    public class JoJoStands : Mod
    {
        public static bool SoundsLoaded = false;
        public static bool FanStandsLoaded = false;
        public static Mod JoJoStandsSounds;

        internal static JoJoStands Instance => ModContent.GetInstance<JoJoStands>();        //internal so that it's only usable inside this specific project
        internal static CustomizableOptions customizableConfig;

        internal ToBeContinued TBCarrow;
        internal HamonBarState HamonBarInterface;
        internal GoldenSpinMeter GoldenSpinInterface;
        internal BulletCounter bulletCounter;
        internal AerosmithRadar aerosmithRadar;
        internal BetUI betUI;
        internal SexPistolsUI sexPistolsUI;
        internal VoidBar VoidBarUI;
        internal HamonSkillTree HamonSkillTreeUI;
        internal BadCompanyUnitsUI UnitsUI;
        internal ZombieSkillTree ZombieSkillTreeUI;
        internal StoneFreeAbilityWheel StoneFreeAbilityWheelUI;
        internal GoldExperienceAbilityWheel GoldExperienceAbilityWheelUI;
        internal GoldExperienceRequiemAbilityWheel GoldExperienceRequiemAbilityWheelUI;

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

        public static ModHotKey SpecialHotKey;
        public static ModHotKey SecondSpecialHotKey;
        public static ModHotKey StandOutHotKey;
        public static ModHotKey StandAutoModeHotKey;
        public static ModHotKey PoseHotKey;

        public static List<int> timestopImmune = new List<int>();
        public static List<int> standTier1List = new List<int>();
        public static List<int> christmasStands = new List<int>();
        public static List<char> testStandPassword = new List<char>();      //  :) Have *fuuuuuuun!*

        public override void Load()
        {
            JoJoStandsSounds = ModLoader.GetMod("JoJoStandsSounds");        //would just return null if nothing is there
            SoundsLoaded = JoJoStandsSounds != null;
            FanStandsLoaded = ModLoader.GetMod("JoJoFanStands") != null;

            HamonBarState.hamonBarTexture = ModContent.GetTexture("JoJoStands/UI/HamonBar");
            ToBeContinued.TBCArrowTexture = ModContent.GetTexture("JoJoStands/UI/TBCArrow");
            BulletCounter.bulletCounterTexture = ModContent.GetTexture("JoJoStands/UI/BulletCounter");
            AerosmithRadar.aerosmithRadarTexture = ModContent.GetTexture("JoJoStands/UI/AerosmithRadar");
            GoldenSpinMeter.goldenRectangleTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeter");
            GoldenSpinMeter.goldenRectangleSpinLineTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeterLine");
            SexPistolsUI.sexPistolsUITexture = ModContent.GetTexture("JoJoStands/UI/SexPistolsUI");
            VoidBar.VoidBarTexture = ModContent.GetTexture("JoJoStands/UI/VoidBar");
            VoidBar.VoidBarBarTexture = ModContent.GetTexture("JoJoStands/UI/VoidBarBar");

            standTier1List.Add(ItemType("AerosmithT1"));
            standTier1List.Add(ItemType("GoldExperienceT1"));
            standTier1List.Add(ItemType("HierophantGreenT1"));
            standTier1List.Add(ItemType("KillerQueenT1"));
            standTier1List.Add(ItemType("KingCrimsonT1"));
            standTier1List.Add(ItemType("MagiciansRedT1"));
            standTier1List.Add(ItemType("SexPistolsT1"));
            standTier1List.Add(ItemType("StarPlatinumT1"));
            standTier1List.Add(ItemType("StickyFingersT1"));
            standTier1List.Add(ItemType("TheWorldT1"));
            standTier1List.Add(ItemType("TuskAct1"));
            standTier1List.Add(ItemType("LockT1"));
            standTier1List.Add(ItemType("GratefulDeadT1"));
            standTier1List.Add(ItemType("TheHandT1"));
            standTier1List.Add(ItemType("WhitesnakeT1"));
            standTier1List.Add(ItemType("DollyDaggerT1"));
            standTier1List.Add(ItemType("CenturyBoyT1"));
            standTier1List.Add(ItemType("SilverChariotT1"));
            standTier1List.Add(ItemType("HermitPurpleT1"));
            standTier1List.Add(ItemType("BadCompanyT1"));
            standTier1List.Add(ItemType("CreamT1"));

            timestopImmune.Add(ProjectileType("TheWorldStandT2"));     //only the timestop capable stands as people shouldn't switch anyway
            timestopImmune.Add(ProjectileType("TheWorldStandT3"));
            timestopImmune.Add(ProjectileType("TheWorldStandFinal"));
            timestopImmune.Add(ProjectileType("StarPlatinumStandFinal"));
            //MyPlayer.stopImmune.Add(ProjectileType("StickyFingersFistExtended"));
            timestopImmune.Add(ProjectileType("RoadRoller"));
            timestopImmune.Add(ProjectileType("HamonPunches"));
            timestopImmune.Add(ProjectileType("Fists"));
            timestopImmune.Add(ProjectileType("GoldExperienceRequiemStand"));
            timestopImmune.Add(ProjectileType("TuskAct4Stand"));
            timestopImmune.Add(ProjectileType("TestStand"));
            timestopImmune.Add(ProjectileType("StarOnTheTreeStand"));

            christmasStands.Add(ItemType("StarOnTheTree"));
            christmasStands.Add(ItemType("KingChristmas"));

            testStandPassword.Add(Convert.ToChar(84));


            //Where we register the hotkeys
            SpecialHotKey = RegisterHotKey("Special Ability", "R");        // See https://docs.microsoft.com/en-us/previous-versions/windows/xna/bb197781(v%3dxnagamestudio.41) for special keys
            SecondSpecialHotKey = RegisterHotKey("Secondary Special Ability", "T");
            StandOutHotKey = RegisterHotKey("Stand Out", "G");
            PoseHotKey = RegisterHotKey("Pose", "V");
            StandAutoModeHotKey = RegisterHotKey("Stand Auto Mode", "C");

            if (!Main.dedServ)      //Manages resource loading cause the server isn't able to load resources
            {
                //UI Stuff
                HamonBarInterface = new HamonBarState();
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

                HamonSkillTreeUI = new HamonSkillTree();
                HamonSkillTreeUI.Activate();
                _hamonSkillTreeUI = new UserInterface();
                _hamonSkillTreeUI.SetState(HamonSkillTreeUI);

                UnitsUI = new BadCompanyUnitsUI();
                UnitsUI.Activate();
                _unitsUI = new UserInterface();
                _unitsUI.SetState(UnitsUI);

                ZombieSkillTreeUI = new ZombieSkillTree();
                ZombieSkillTreeUI.Activate();
                _zombieSkillTreeUI = new UserInterface();
                _zombieSkillTreeUI.SetState(ZombieSkillTreeUI);

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

                //Shader Stuff
                Ref<Effect> timestopShader = new Ref<Effect>(GetEffect("Effects/TimestopEffect"));      // The path to the compiled shader file.
                Filters.Scene["TimestopEffectShader"] = new Filter(new ScreenShaderData(timestopShader, "TimestopEffectShader"), EffectPriority.VeryHigh);
                Filters.Scene["TimestopEffectShader"].Load();
                Ref<Effect> greyscaleShader = new Ref<Effect>(GetEffect("Effects/Greyscale"));
                Filters.Scene["GreyscaleEffect"] = new Filter(new ScreenShaderData(greyscaleShader, "GreyscaleEffect"), EffectPriority.VeryHigh);
                Filters.Scene["GreyscaleEffect"].Load();
                Ref<Effect> greenShader = new Ref<Effect>(GetEffect("Effects/GreenEffect"));
                Filters.Scene["GreenEffect"] = new Filter(new ScreenShaderData(greenShader, "GreenEffect"), EffectPriority.VeryHigh);
                Filters.Scene["GreenEffect"].Load();
                Ref<Effect> redShader = new Ref<Effect>(GetEffect("Effects/RedEffect"));
                Filters.Scene["RedEffect"] = new Filter(new ScreenShaderData(redShader, "RedEffect"), EffectPriority.VeryHigh);
                Filters.Scene["RedEffect"].Load();
                Ref<Effect> colorChangeShader = new Ref<Effect>(GetEffect("Effects/ColorChangeEffect"));
                Filters.Scene["ColorChangeEffect"] = new Filter(new ScreenShaderData(colorChangeShader, "ColorChangeEffect"), EffectPriority.VeryHigh);
                Filters.Scene["ColorChangeEffect"].Load();
                Ref<Effect> voidGradientShader = new Ref<Effect>(GetEffect("Effects/VoidBarGradient"));
                GameShaders.Misc["JoJoStandsVoidGradient"] = new MiscShaderData(voidGradientShader, "VoidBarGradient");

                Effect timeskipShaderEffect = GetEffect("Effects/TimeSkipEffectShader");
                //timeskipShaderEffect.Parameters["backStarsImage"].SetValue(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonBackStars"));
                //timeskipShaderEffect.Parameters["frontStarsImage"].SetValue(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonFrontStars"));
                Ref<Effect> timeskipShader = new Ref<Effect>(timeskipShaderEffect);      // The path to the compiled shader file.
                Filters.Scene["TimeSkipEffectShader"] = new Filter(new ScreenShaderData(timeskipShader, "TimeSkipEffectShader"), EffectPriority.VeryHigh);
                Filters.Scene["TimeSkipEffectShader"].GetShader().UseImage(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonBackStars"), 0);
                Filters.Scene["TimeSkipEffectShader"].GetShader().UseImage(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonFrontStars"), 1);
                Filters.Scene["TimeSkipEffectShader"].Load();
                //Filters.Scene["TimeSkipEffectShader"].GetShader().Shader.Parameters["backStarsImage"].SetValue(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonBackStars"));
                //Filters.Scene["TimeSkipEffectShader"].GetShader().Shader.Parameters["frontStarsImage"].SetValue(ModContent.GetTexture("JoJoStands/Extras/KingCrimsonFrontStars"));
                Ref<Effect> biteTheDustEffectShader = new Ref<Effect>(GetEffect("Effects/BiteTheDustEffectShader"));
                Filters.Scene["BiteTheDustEffect"] = new Filter(new ScreenShaderData(biteTheDustEffectShader, "BiteTheDustEffectShader"), EffectPriority.VeryHigh);
                Filters.Scene["BiteTheDustEffect"].GetShader().UseImage(ModContent.GetTexture("JoJoStands/Extras/KillerQueenBTDImage"), 0);
                Filters.Scene["BiteTheDustEffect"].Load();

                //Misc
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/VMMusic"), ItemType("ViralMusicBox"), TileType("ViralMusicBoxTile"));
            }
        }

        public override void Unload()
        {
            SpecialHotKey = null;
            PoseHotKey = null;
            StandAutoModeHotKey = null;
            StandOutHotKey = null;
            customizableConfig = null;
            standTier1List.Clear();
            timestopImmune.Clear();
            testStandPassword.Clear();
            SoundsLoaded = false;
            FanStandsLoaded = false;
            JoJoStandsSounds = null;
            ToBeContinued.TBCArrowTexture = null;
            HamonBarState.hamonBarTexture = null;
            BulletCounter.bulletCounterTexture = null;
            AerosmithRadar.aerosmithRadarTexture = null;
            GoldenSpinMeter.goldenRectangleTexture = null;
            GoldenSpinMeter.goldenRectangleSpinLineTexture = null;
            SexPistolsUI.sexPistolsUITexture = null;
            VoidBar.VoidBarTexture = null;
            VoidBar.VoidBarBarTexture = null;
        }

        public override void AddRecipeGroups()
        {
            RecipeGroup willsGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + "Wills", new int[]
            {
                ItemType("WillToFight"),
                ItemType("WillToChange"),
                ItemType("WillToControl"),
                ItemType("WillToDestroy"),
                ItemType("WillToEscape"),
                ItemType("WillToProtect")
            });
            RecipeGroup.RegisterGroup("JoJoStandsWills", willsGroup);
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
                return;

            Player player = Main.LocalPlayer;
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.myPlayer != -1 && !Main.gameMenu)
            {
                if (player.active && mPlayer.ZoneViralMeteorite)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/VMMusic");
                    priority = MusicPriority.BiomeMedium;
                }
                if (JoJoStandsWorld.VampiricNight)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/VNight");
                    priority = MusicPriority.Event;
                }
            }
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            MyPlayer mPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            mPlayer.Draw(spriteBatch);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (HamonBarState.Visible)
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

            if (HamonSkillTree.Visible)
                _hamonSkillTreeUI.Update(gameTime);

            if (BadCompanyUnitsUI.Visible)
                _unitsUI.Update(gameTime);

            if (ZombieSkillTree.Visible)
                _zombieSkillTreeUI.Update(gameTime);

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
            if (HamonBarState.Visible)
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

            if (HamonSkillTree.Visible)
                _hamonSkillTreeUI.Draw(Main.spriteBatch, new GameTime());

            if (BadCompanyUnitsUI.Visible)
                _unitsUI.Draw(Main.spriteBatch, new GameTime());

            if (ZombieSkillTree.Visible)
                _zombieSkillTreeUI.Draw(Main.spriteBatch, new GameTime());

            if (StoneFreeAbilityWheel.visible)
                _stoneFreeAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            if (GoldExperienceAbilityWheel.visible)
                _goldExperienceAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            if (GoldExperienceRequiemAbilityWheel.visible)
                _goldExperienceRequiemAbilityWheelUI.Draw(Main.spriteBatch, new GameTime());

            return true;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}