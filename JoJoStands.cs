using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace JoJoStands
{
	public class JoJoStands : Mod
	{
        public static ModHotKey ItemHotKey;
        public static ModHotKey AccessoryHotKey;
        public static ModHotKey StandControl;
        public static ModHotKey StandControlUp;
        public static ModHotKey StandControlDown;
        public static ModHotKey StandControlLeft;
        public static ModHotKey StandControlRight;
        public static ModHotKey StandControlAttack;
        public static ModHotKey PoseHotKey;
        static internal JoJoStands Instance;
        internal static CustomizableOptions customizableConfig;

        internal UserInterface Bet;     //for later use
        private UserInterface _hamonbarInterface;
        private UserInterface _tbcarrow;
        private UserInterface _bulletcounter;

        internal ToBeContinued TBCarrow;
        internal HamonBarState HamonBarInterface;
        internal BulletCounter bulletCounter;

        public JoJoStands()
        {
            Instance = this;
        }

        public override void Load()
		{
            HamonBarState.hamonBarTexture = ModContent.GetTexture("JoJoStands/UI/HamonBar");
            ToBeContinued.TBCArrowTexture = ModContent.GetTexture("JoJoStands/UI/TBCArrow");
            BulletCounter.bulletCounterTexture = ModContent.GetTexture("JoJoStands/UI/BulletCounter");
            Items.SexPistolsFinal.usesound = GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Shoot");

            // Registers a new hotkey
            ItemHotKey = RegisterHotKey("Item Special", "P"); // See https://docs.microsoft.com/en-us/previous-versions/windows/xna/bb197781(v%3dxnagamestudio.41) for special keys
            AccessoryHotKey = RegisterHotKey("Accessory Special", "k");
            StandControl = RegisterHotKey("Stand Control", "j");
            StandControlUp = RegisterHotKey("Stand Control Up", "Up");
            StandControlDown = RegisterHotKey("Stand Control Down", "Down");
            StandControlLeft = RegisterHotKey("Stand Control Left", "Left");
            StandControlRight = RegisterHotKey("Stand Control Right", "Right");
            StandControlAttack = RegisterHotKey("Stand Control Attack", "RightShift");
            PoseHotKey = RegisterHotKey("Pose", "v");


            //UI Stuff
            if (!Main.dedServ)
            {
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
            }
        }

        public override void Unload()
        {
            ToBeContinued.TBCArrowTexture = null;
            HamonBarState.hamonBarTexture = null;
            BulletCounter.bulletCounterTexture = null;
            Items.SexPistolsFinal.usesound = null;
            Instance = null;
            ItemHotKey = null;
            AccessoryHotKey = null;
            StandControl = null;
            StandControlUp = null;
            StandControlDown = null;
            StandControlLeft = null;
            StandControlRight = null;
            StandControlAttack = null;
            PoseHotKey = null;
            customizableConfig = null;
            base.Unload();
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
            return true;
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)      //from ExampleMod
        {
            JoJoMessageType msgType = (JoJoMessageType)reader.ReadByte();
            switch (msgType)
            {
                case JoJoMessageType.SyncPlayer:
                    byte playernumber = reader.ReadByte();
                    MyPlayer player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    player.TheWorldEffect = reader.ReadBoolean();
                    player.TimeSkipEffect = reader.ReadBoolean();
                    player.BackToZero = reader.ReadBoolean();
                    player.poseMode = reader.ReadBoolean();
                    // SyncPlayer will be called automatically, so there is no need to forward this data to other clients.
                    break;
                case JoJoMessageType.TheWorld:
                    playernumber = reader.ReadByte();
                    player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    player.TheWorldEffect = reader.ReadBoolean();
                    // Unlike SyncPlayer, here we have to relay/forward these changes to all other connected clients
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.TheWorld);
                        packet.Write(playernumber);
                        packet.Write(player.TheWorldEffect);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.Timeskip:
                    playernumber = reader.ReadByte();
                    player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    player.TimeSkipEffect = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.Timeskip);
                        packet.Write(playernumber);
                        packet.Write(player.TimeSkipEffect);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.BacktoZero:
                    playernumber = reader.ReadByte();
                    player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
                    player.BackToZero = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.BacktoZero);
                        packet.Write(playernumber);
                        packet.Write(player.BackToZero);
                        packet.Send(-1, playernumber);
                    }
                    break;
                case JoJoMessageType.PoseMode:      //the only thing that works
                    playernumber = reader.ReadByte();
                    player = Main.player[playernumber].GetModPlayer<MyPlayer>();
                    player.poseMode = reader.ReadBoolean();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        var packet = GetPacket();
                        packet.Write((byte)JoJoMessageType.PoseMode);
                        packet.Write(playernumber);
                        packet.Write(player.poseMode);
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
            PoseMode
        }
    }
}