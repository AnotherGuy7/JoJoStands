using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using JoJoStands.Networking;

namespace JoJoStands
{
	public class JoJoStands : Mod
	{
        public static bool killSounds = false;

        public static ModHotKey SpecialHotKey;
        public static ModHotKey StandOut;
        public static ModHotKey StandAutoMode;
        public static ModHotKey PoseHotKey;
        public static Mod JoJoStandsSounds;
        internal static JoJoStands Instance => ModContent.GetInstance<JoJoStands>();
        internal static CustomizableOptions customizableConfig;

        private UserInterface _betUI;
        private UserInterface _hamonbarInterface;
        private UserInterface _goldenSpinInterface;
        private UserInterface _tbcarrow;
        private UserInterface _bulletcounter;
        private UserInterface _aerosmithRadar;

        internal ToBeContinued TBCarrow;
        internal HamonBarState HamonBarInterface;
        internal GoldenSpinMeter GoldenSpinInterface;
        internal BulletCounter bulletCounter;
        internal AerosmithRadar aerosmithRadar;
        internal BetUI betUI;

        public override void Load()
		{
            JoJoStandsSounds = ModLoader.GetMod("JoJoStandsSounds");        //would just return null if nothing is there
            HamonBarState.hamonBarTexture = ModContent.GetTexture("JoJoStands/UI/HamonBar");
            ToBeContinued.TBCArrowTexture = ModContent.GetTexture("JoJoStands/UI/TBCArrow");
            BulletCounter.bulletCounterTexture = ModContent.GetTexture("JoJoStands/UI/BulletCounter");
            AerosmithRadar.aerosmithRadarTexture = ModContent.GetTexture("JoJoStands/UI/AerosmithRadar");
            GoldenSpinMeter.goldenRectangleTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeter");
            GoldenSpinMeter.goldenRectangleSpinLineTexture = ModContent.GetTexture("JoJoStands/UI/GoldenSpinMeterLine");
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
                _goldenSpinInterface.SetState(GoldenSpinInterface);

                //Shader Stuff
                Ref<Effect> timestopShader = new Ref<Effect>(GetEffect("Effects/TimestopEffect")); // The path to the compiled shader file.
                Filters.Scene["TimestopEffectShader"] = new Filter(new ScreenShaderData(timestopShader, "TimestopEffectShader"), EffectPriority.VeryHigh);
                Filters.Scene["TimestopEffectShader"].Load();
                Ref<Effect> greyscaleShader = new Ref<Effect>(GetEffect("Effects/Greyscale"));
                Filters.Scene["GreyscaleEffect"] = new Filter(new ScreenShaderData(greyscaleShader, "GreyscaleEffect"), EffectPriority.VeryHigh);
                Filters.Scene["GreyscaleEffect"].Load();
                Ref<Effect> greenShader = new Ref<Effect>(GetEffect("Effects/GreenEffect"));
                Filters.Scene["GreenEffect"] = new Filter(new ScreenShaderData(greenShader, "GreenEffect"), EffectPriority.VeryHigh);
                Filters.Scene["GreenEffect"].Load();

                /*MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandT2"));     //only the timestop capable stands as people shouldn't switch anyway
                MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandT3"));
                MyPlayer.stopImmune.Add(ProjectileType("TheWorldStandFinal"));
                MyPlayer.stopImmune.Add(ProjectileType("StarPlatinumStandFinal"));/*/
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
            Player player = Main.player[Main.myPlayer];
            if (Main.myPlayer != -1 && !Main.gameMenu)
                if (player.active && player.GetModPlayer<MyPlayer>().ZoneViralMeteorite)
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/VMMusic");
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            MyPlayer Mplayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            Mplayer.Draw(spriteBatch);
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
            return true;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }

        /*public override void HandlePacket(BinaryReader reader, int whoAmI)      //from ExampleMod
        {
            JoJoMessageType msgType = (JoJoMessageType)reader.ReadByte();
            switch (msgType)
            {
                case JoJoMessageType.SyncPlayer:
                    byte playernumber = reader.ReadByte();
                    MyPlayer modPlayer = Main.player[playernumber].GetModPlayer<MyPlayer>();      //use Main.myPlayer to apply bools and playerNumber to read bools
                    modPlayer.TheWorldEffect = reader.ReadBoolean();
                    modPlayer.TimeSkipEffect = reader.ReadBoolean();
                    modPlayer.BackToZero = reader.ReadBoolean();
                    modPlayer.poseMode = reader.ReadBoolean();
                    modPlayer.StandOut = reader.ReadBoolean();
                    modPlayer.StandAutoMode = reader.ReadBoolean();
                    modPlayer.Foresight = reader.ReadBoolean();
					modPlayer.showingCBLayer = reader.ReadBoolean();
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    break;
                case JoJoMessageType.TheWorld:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    modPlayer.TheWorldEffect = reader.ReadBoolean();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.TheWorld);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.TheWorldEffect);
                        packet.Send(-1, playernumber);
                    }
                    //Main.NewText("HandlePacket", Color.Red);
                    break;
                case JoJoMessageType.Timeskip:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    modPlayer.TimeSkipEffect = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.Timeskip);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.TimeSkipEffect);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.BacktoZero:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();       //setting YOUR (Main.myPlayer) value to what's read
                    modPlayer.BackToZero = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.BacktoZero);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.BackToZero);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.PoseMode:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[playernumber].GetModPlayer<MyPlayer>();        //ONLY READING the value
                    modPlayer.poseMode = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.PoseMode);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.poseMode);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.StandOut:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[playernumber].GetModPlayer<MyPlayer>();
                    modPlayer.StandOut = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.StandOut);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.StandOut);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.StandAutoMode:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[playernumber].GetModPlayer<MyPlayer>();
                    modPlayer.StandAutoMode = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.StandAutoMode);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.StandAutoMode);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.Foresight:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    modPlayer.Foresight = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.Foresight);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.Foresight);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.CBLayer:
                    playernumber = reader.ReadByte();
                    modPlayer = Main.player[playernumber].GetModPlayer<MyPlayer>();
                    modPlayer.Foresight = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.CBLayer);
                        packet.Write(playernumber);
                        packet.Write(modPlayer.showingCBLayer);
                        packet.Send(-1, playernumber);
                    }
                    break;
                default:
                    Logger.WarnFormat("JoJoStands: Unknown Message type:" + msgType);
                    break;
            }
        }

        internal enum JoJoMessageType : byte
        {
            SyncPlayer,
            TheWorld,
            Timeskip,
            BacktoZero,
            PoseMode,
            StandOut,
            StandAutoMode,
            Foresight,
            CBLayer
        }*/
    }
}