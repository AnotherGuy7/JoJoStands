using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace JoJoStands.Networking
{
    public class PlayerPacketHandler : PacketHandler
    {
        public const byte PoseMode = 0;
        public const byte StandOut = 1;     //needed because some stands don't spawn without it
        public const byte StandAutoMode = 2;
        public const byte CBLayer = 3;
        public const byte Yoshihiro = 4;
        public const byte DyeItem = 5;
        public const byte SexPistolPosition = 6;
        public const byte DeathLoopInfo = 7;
        //public const byte Sounds = 6;

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        {
        }

        public override void HandlePacket(BinaryReader reader, int fromWho)     //decides what happens when a packet is received, it looks for the byte sent with the packet and uses the proper method
        {
            byte messageType = reader.ReadByte();
            switch (messageType)
            {
                case PoseMode:
                    ReceivePoseMode(reader, fromWho);
                    break;
                case StandOut:
                    ReceiveStandOut(reader, fromWho);
                    break;
                case StandAutoMode:
                    ReceiveStandAutoMode(reader, fromWho);
                    break;
                case CBLayer:
                    ReceiveCBLayer(reader, fromWho);
                    break;
                case SexPistolPosition:
                    ReceiveSexPistolPosition(reader, fromWho);
                    break;
                /*case Yoshihiro:
					ReceiveYoshihiroSpawn(reader, fromWho);
					break;*/
                case DyeItem:
                    ReceiveDyeItem(reader, fromWho);
                    break;
                case DeathLoopInfo:
                    ReceiveDeathLoopInfo(reader, fromWho);
                    break;
            }
        }

        public void SendPoseMode(int toWho, int fromWho, bool poseModeValue, byte whoAmI)        //send the packet whenever its called to
        {
            ModPacket packet = GetPacket(PoseMode, fromWho);
            packet.Write(poseModeValue);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceivePoseMode(BinaryReader reader, int fromWho)       //HandlePacket leads the packet here and it is read and applied
        {
            bool poseModeVal = reader.ReadBoolean();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                Main.player[whoAmI].GetModPlayer<MyPlayer>().posing = poseModeVal;
            }
            else
            {
                SendPoseMode(-1, fromWho, poseModeVal, whoAmI);
            }
        }

        public void SendStandOut(int toWho, int fromWho, bool standOutValue, byte whoAmI)
        {
            ModPacket packet = GetPacket(StandOut, fromWho);
            packet.Write(standOutValue);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveStandOut(BinaryReader reader, int fromWho)
        {
            bool standOutVal = reader.ReadBoolean();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                Main.player[whoAmI].GetModPlayer<MyPlayer>().standOut = standOutVal;
                if (!standOutVal)
                    Main.player[whoAmI].GetModPlayer<MyPlayer>().standTier = 0;
            }
            else
            {
                SendStandOut(-1, fromWho, standOutVal, whoAmI);
            }
        }

        public void SendStandAutoMode(int toWho, int fromWho, bool autoModeValue, byte whoAmI)
        {
            ModPacket packet = GetPacket(StandAutoMode, fromWho);
            packet.Write(autoModeValue);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveStandAutoMode(BinaryReader reader, int fromWho)
        {
            bool autoModeVal = reader.ReadBoolean();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                Main.player[whoAmI].GetModPlayer<MyPlayer>().standAutoMode = autoModeVal;
            }
            else
            {
                SendStandAutoMode(-1, fromWho, autoModeVal, whoAmI);
            }
        }

        public void SendCBLayer(int toWho, int fromWho, bool visibility, byte whoAmI)
        {
            ModPacket packet = GetPacket(CBLayer, fromWho);
            packet.Write(visibility);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveCBLayer(BinaryReader reader, int fromWho)
        {
            bool visibiltyValue = reader.ReadBoolean();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                Main.player[whoAmI].GetModPlayer<MyPlayer>().showingCBLayer = visibiltyValue;
            }
            else
            {
                SendCBLayer(-1, fromWho, visibiltyValue, whoAmI);
            }
        }

        /*public void SendYoshihiroToSpawn(int toWho, int fromWho, int NPCType, Vector2 position)
		{
			ModPacket packet = GetPacket(Yoshihiro, fromWho);
			packet.Write(NPCType);
			packet.WriteVector2(position);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveYoshihiroSpawn(BinaryReader reader, int fromWho)
		{
			int type = reader.ReadInt32();
			Vector2 pos = reader.ReadVector2();
			if (Main.netMode == NetmodeID.Server)
			{
				if (!NPC.AnyNPCs(type))
					NPC.NewNPC((int)pos.X, (int)pos.Y, type);
				SendYoshihiroToSpawn(-1, fromWho, type, pos);
			}
		}*/

        public void SendDyeItem(int toWho, int fromWho, int dyeItemType, byte whoAmI)
        {
            ModPacket packet = GetPacket(DyeItem, fromWho);
            packet.Write(dyeItemType);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveDyeItem(BinaryReader reader, int fromWho)
        {
            int dyeItemType = reader.ReadInt32();
            byte oneWhoEquipped = reader.ReadByte();
            Main.player[oneWhoEquipped].GetModPlayer<MyPlayer>().StandDyeSlot.SlotItem.type = dyeItemType;
            Main.player[oneWhoEquipped].GetModPlayer<MyPlayer>().StandDyeSlot.SlotItem.SetDefaults(dyeItemType);
            if (Main.netMode == NetmodeID.Server)
                SendDyeItem(-1, fromWho, dyeItemType, oneWhoEquipped);
        }

        /*public void SendSoundInstance(int toWho, int fromWho, string soundName, Vector2 pos, int travelDist = 10, SoundState state = SoundState.Paused)
		{
			ModPacket packet = GetPacket(Sounds, fromWho);
			packet.Write(soundName);
			packet.WriteVector2(pos);
			packet.Write(travelDist);
			packet.Write((byte)state);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveSoundInstance(BinaryReader reader, int fromWho)
		{
			string sound = reader.ReadString();
			Vector2 pos = reader.ReadVector2();
			int travelDist = reader.ReadInt32();
			SoundState state = (SoundState)reader.ReadByte();
			JoJoStands.PlaySound(sound, pos, travelDist);
			if (Main.netMode == NetmodeID.Server)
			{
				SendSoundInstance(-1, fromWho, sound, pos);
			}
		}*/

        public void SendSexPistolPosition(int toWho, int fromWho, byte whoAmI, byte index, Vector2 pos)
        {
            ModPacket packet = GetPacket(DyeItem, fromWho);
            packet.Write(whoAmI);
            packet.Write(index);
            packet.Write((int)pos.X);
            packet.Write((int)pos.Y);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveSexPistolPosition(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            byte index = reader.ReadByte();
            int posX = reader.ReadInt32();
            int posY = reader.ReadInt32();

            Main.player[whoAmI].GetModPlayer<MyPlayer>().sexPistolsOffsets[index] = new Vector2(posX, posY);
            if (Main.netMode == NetmodeID.Server)
                SendSexPistolPosition(-1, fromWho, whoAmI, index, new Vector2(posX, posY));
        }

        public void SendDeathLoopInfo(int toWho, int fromWho, byte whoAmI, byte targetWhoAmI, int targetNPCType)
        {
            ModPacket packet = GetPacket(DeathLoopInfo, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetWhoAmI);
            packet.Write(targetNPCType);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveDeathLoopInfo(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            byte targetWhoAmI = reader.ReadByte();
            int targetNPCType = reader.ReadInt32();

            Main.player[fromWho].GetModPlayer<MyPlayer>().deathLoopNPCWhoAmI = targetWhoAmI;
            Main.player[fromWho].GetModPlayer<MyPlayer>().deathLoopNPCType = targetNPCType;
            if (Main.netMode == NetmodeID.Server)
                SendDeathLoopInfo(-1, fromWho, whoAmI, targetWhoAmI, targetNPCType);
        }
    }
}