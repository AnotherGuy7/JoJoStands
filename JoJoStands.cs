using JoJoStands.Networking;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public static ModHotKey SpecialHotKey;
        public static ModHotKey SecondSpecialHotKey;
        public static ModHotKey StandOut;
        public static ModHotKey StandAutoMode;
        public static ModHotKey PoseHotKey;
        public static Mod JoJoStandsSounds;
        public static bool SoundsLoaded = false;
        internal static JoJoStands Instance => ModContent.GetInstance<JoJoStands>();
        internal static CustomizableOptions customizableConfig;

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

        public override void Load()
        {
            JoJoStandsSounds = ModLoader.GetMod("JoJoStandsSounds");        //would just return null if nothing is there
            SoundsLoaded = JoJoStandsSounds != null;
            HamonBarState.hamonBarTexture = ModContent.GetTexture("JoJoStands/UI/HamonBar");
            ToBeContinued.TBCArrowTexture = ModContent.GetTexture("JoJoStands/UI/TBCArrow");
            BulletCounter.bulletCounterTexture = ModContent.GetTexture("JoJoStands/UI/BulletCounter");
            AerosmithRadar.aerosmithRadarTexture = ModContent.GetTexture("JoJoStands/UI/AerosmithRadar");
            GoldenSpinMeter.goldenRectangleTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeter");
            GoldenSpinMeter.goldenRectangleSpinLineTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeterLine");
            SexPistolsUI.sexPistolsUITexture = ModContent.GetTexture("JoJoStands/UI/SexPistolsUI");
            VoidBar.VoidBarTexture = ModContent.GetTexture("JoJoStands/UI/VoidBar");
            VoidBar.VoidBarBarTexture = ModContent.GetTexture("JoJoStands/UI/VoidBarBar");
            MyPlayer.standTier1List.Add(ItemType("AerosmithT1"));
            MyPlayer.standTier1List.Add(ItemType("GoldExperienceT1"));
            MyPlayer.standTier1List.Add(ItemType("HierophantGreenT1"));
            MyPlayer.standTier1List.Add(ItemType("KillerQueenT1"));
            MyPlayer.standTier1List.Add(ItemType("KingCrimsonT1"));
            MyPlayer.standTier1List.Add(ItemType("MagiciansRedT1"));
            MyPlayer.standTier1List.Add(ItemType("SexPistolsT1"));
            MyPlayer.standTier1List.Add(ItemType("StarPlatinumT1"));
            MyPlayer.standTier1List.Add(ItemType("StickyFingersT1"));
            MyPlayer.standTier1List.Add(ItemType("TheWorldT1"));
            MyPlayer.standTier1List.Add(ItemType("TuskAct1"));
            MyPlayer.standTier1List.Add(ItemType("LockT1"));
            MyPlayer.standTier1List.Add(ItemType("GratefulDeadT1"));
            MyPlayer.standTier1List.Add(ItemType("TheHandT1"));
            MyPlayer.standTier1List.Add(ItemType("WhitesnakeT1"));
            MyPlayer.standTier1List.Add(ItemType("DollyDaggerT1"));
            MyPlayer.standTier1List.Add(ItemType("CenturyBoyT1"));
            MyPlayer.standTier1List.Add(ItemType("SilverChariotT1"));
            MyPlayer.standTier1List.Add(ItemType("HermitPurpleT1"));
            MyPlayer.standTier1List.Add(ItemType("BadCompanyT1"));
            MyPlayer.standTier1List.Add(ItemType("CreamT1"));

            MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandT2"));     //only the timestop capable stands as people shouldn't switch anyway
            MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandT3"));
            MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandFinal"));
            MyPlayer.stopImmune.Add(ProjectileType("StarPlatinumStandFinal"));
            //MyPlayer.stopImmune.Add(ProjectileType("StickyFingersFistExtended"));
            MyPlayer.stopImmune.Add(ProjectileType("RoadRoller"));
            MyPlayer.stopImmune.Add(ProjectileType("HamonPunches"));
            MyPlayer.stopImmune.Add(ProjectileType("Fists"));
            MyPlayer.stopImmune.Add(ProjectileType("GoldExperienceRequiemStand"));
            MyPlayer.stopImmune.Add(ProjectileType("TuskAct4Minion"));


            // Registers a new hotkey
            SpecialHotKey = RegisterHotKey("Special Ability", "P");        // See https://docs.microsoft.com/en-us/previous-versions/windows/xna/bb197781(v%3dxnagamestudio.41) for special keys
            SecondSpecialHotKey = RegisterHotKey("Secondary Special Ability", "H");
            StandOut = RegisterHotKey("Stand Out", "G");
            PoseHotKey = RegisterHotKey("Pose", "V");
            StandAutoMode = RegisterHotKey("Stand Auto Mode", "L");

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
                sexPistolsUI = new SexPistolsUI();
                sexPistolsUI.Activate();
                _sexPistolsUI = new UserInterface();
                _sexPistolsUI.SetState(sexPistolsUI);
                _goldenSpinInterface.SetState(GoldenSpinInterface);
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

                //Misc
                AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/VMMusic"), ItemType("ViralMusicBox"), TileType("ViralMusicBoxTile"));
            }
        }

        public override void Unload()
        {
            ToBeContinued.TBCArrowTexture = null;
            HamonBarState.hamonBarTexture = null;
            BulletCounter.bulletCounterTexture = null;
            AerosmithRadar.aerosmithRadarTexture = null;
            GoldenSpinMeter.goldenRectangleTexture = null;
            GoldenSpinMeter.goldenRectangleSpinLineTexture = null;
            SexPistolsUI.sexPistolsUITexture = null;
            VoidBar.VoidBarTexture = null;
            VoidBar.VoidBarBarTexture = null;
            SpecialHotKey = null;
            PoseHotKey = null;
            StandAutoMode = null;
            StandOut = null;
            customizableConfig = null;
            MyPlayer.standTier1List.Clear();
            MyPlayer.stopImmune.Clear();
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
            {
                _hamonbarInterface.Update(gameTime);
            }
            if (ToBeContinued.Visible)
            {
                _tbcarrow.Update(gameTime);
            }
            if (BulletCounter.Visible)
            {
                _bulletcounter.Update(gameTime);
            }
            if (AerosmithRadar.Visible)
            {
                _aerosmithRadar.Update(gameTime);
            }
            if (BetUI.Visible)
            {
                _betUI.Update(gameTime);
            }
            if (GoldenSpinMeter.Visible)
            {
                _goldenSpinInterface.Update(gameTime);
            }
            if (SexPistolsUI.Visible)
            {
                _sexPistolsUI.Update(gameTime);
            }
            if (VoidBar.Visible)
            {
                _voidbarUI.Update(gameTime);
            }
            if (HamonSkillTree.Visible)
            {
                _hamonSkillTreeUI.Update(gameTime);
            }
            if (BadCompanyUnitsUI.Visible)
            {
                _unitsUI.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)     //from ExampleMod's ExampleUI
        {
            layers.Insert(5, new LegacyGameInterfaceLayer("JoJoStands: UI", DrawUI, InterfaceScaleType.UI));     //from Terraria Interface for Dummies, and Insert so it doesn't draw over everything
        }

        private bool DrawUI()       //also from Terraria Interface for Dummies
        {
            if (HamonBarState.Visible)
            {
                _hamonbarInterface.Draw(Main.spriteBatch, new GameTime());
            }
            if (ToBeContinued.Visible)
            {
                _tbcarrow.Draw(Main.spriteBatch, new GameTime());
            }
            if (BulletCounter.Visible)
            {
                _bulletcounter.Draw(Main.spriteBatch, new GameTime());
            }
            if (AerosmithRadar.Visible)
            {
                _aerosmithRadar.Draw(Main.spriteBatch, new GameTime());
            }
            if (BetUI.Visible)
            {
                _betUI.Draw(Main.spriteBatch, new GameTime());
            }
            if (GoldenSpinMeter.Visible)
            {
                _goldenSpinInterface.Draw(Main.spriteBatch, new GameTime());
            }
            if (SexPistolsUI.Visible)
            {
                _sexPistolsUI.Draw(Main.spriteBatch, new GameTime());
            }
            if (VoidBar.Visible)
            {
                _voidbarUI.Draw(Main.spriteBatch, new GameTime());
            }
            if (HamonSkillTree.Visible)
            {
                _hamonSkillTreeUI.Draw(Main.spriteBatch, new GameTime());
            }
            if (BadCompanyUnitsUI.Visible)
            {
                _unitsUI.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}