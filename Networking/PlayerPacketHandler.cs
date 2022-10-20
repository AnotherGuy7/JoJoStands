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
        public const byte ArrowEarringInfo = 8;
        public const byte FistsEffectNPCInfo = 9;
        public const byte OtherPlayerDebuffInfo = 10;
        public const byte OtherPlayerExtraEffect = 11;
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
                case ArrowEarringInfo:
                    ReceiveArrowEarringInfo(reader, fromWho);
                    break;
                case FistsEffectNPCInfo:
                    ReceiveFistsEffectNPCInfo(reader, fromWho);
                    break;
                case OtherPlayerDebuffInfo:
                    ReceiveOtherPlayerDebuff(reader, fromWho);
                    break;
                case OtherPlayerExtraEffect:
                    ReceiveOtherPlayerExtraEffect(reader, fromWho);
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

        public void SendDeathLoopInfo(int toWho, int fromWho, byte whoAmI, int targetWhoAmI)
        {
            ModPacket packet = GetPacket(DeathLoopInfo, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetWhoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveDeathLoopInfo(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetWhoAmI = reader.ReadByte();

            Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().taggedForDeathLoop = 600;
            Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().deathLoopOwner = fromWho;
            if (Main.netMode == NetmodeID.Server)
                SendDeathLoopInfo(-1, fromWho, whoAmI, targetWhoAmI);
        }

        public void SendArrowEarringInfo(int toWho, int fromWho, byte whoAmI, int targetNPCwhoAmI, int damage, bool crit)
        {
            ModPacket packet = GetPacket(ArrowEarringInfo, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetNPCwhoAmI);
            packet.Write(damage);
            packet.Write(crit);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveArrowEarringInfo(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetNPCwhoAmI = reader.ReadInt32();
            int damage = reader.ReadInt32();
            bool crit = reader.ReadBoolean();

            Main.npc[targetNPCwhoAmI].StrikeNPC(damage, 0, 0, crit);
            if (Main.netMode == NetmodeID.Server)
                SendArrowEarringInfo(-1, fromWho, whoAmI, targetNPCwhoAmI, damage, crit);
        }

        public void SendFistsEffectNPCInfo(int toWho, int fromWho, byte whoAmI, int targetNPCwhoAmI, int fistWhoAmI, int stat1, int stat2, int stat3, float stat4, float stat5)
        {
            ModPacket packet = GetPacket(FistsEffectNPCInfo, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetNPCwhoAmI);
            packet.Write(fistWhoAmI);
            packet.Write(stat1);
            packet.Write(stat2);
            packet.Write(stat3);
            packet.Write(stat4);
            packet.Write(stat5);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveFistsEffectNPCInfo(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetNPCwhoAmI = reader.ReadInt32();
            int fistWhoAmI = reader.ReadInt32();
            int stat1 = reader.ReadInt32();
            int stat2 = reader.ReadInt32();
            int stat3 = reader.ReadInt32();
            float stat4 = reader.ReadSingle();
            float stat5 = reader.ReadSingle();

            if (fistWhoAmI == 0)
                Main.npc[targetNPCwhoAmI].velocity.X *= 0.2f;
            if (fistWhoAmI == 3)
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().affectedbyBtz = true;
            if (fistWhoAmI == 4)
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = stat1;
            if (fistWhoAmI == 8)
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = stat1;
            if (fistWhoAmI == 12)
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().CDstonePunch += 1;
            if (fistWhoAmI == 13)
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames = 30;
            if (fistWhoAmI == 15 && stat1 == 3)
            {
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = stat4;
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = stat5;
                if (Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze <= 15)
                    Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesFreeze += 30;
            }
            if (fistWhoAmI == 15 && stat1 == 1)
            {
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = stat4;
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = stat5;
                Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensivityMax = stat2;
                if (Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensivity < Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensivityMax)
                    Main.npc[targetNPCwhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensivity += stat3;
            }
            if (Main.netMode == NetmodeID.Server)
                SendFistsEffectNPCInfo(-1, fromWho, whoAmI, targetNPCwhoAmI, fistWhoAmI, stat1, stat2, stat3, stat4, stat5);
        }

        public void SendOtherPlayerDebuff(int toWho, int fromWho, byte whoAmI, int targetPlayerWhoAmI, int debuffType, int debuffTime)
        {
            ModPacket packet = GetPacket(OtherPlayerDebuffInfo, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetPlayerWhoAmI);
            packet.Write(debuffType);
            packet.Write(debuffTime);
            packet.Send(toWho, fromWho);
        }
        public void ReceiveOtherPlayerDebuff(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetPlayerWhoAmI = reader.ReadInt32();
            int debuffType = reader.ReadInt32();
            int debuffTime = reader.ReadInt32();

            Main.player[targetPlayerWhoAmI].AddBuff(debuffType, debuffTime);
            if (Main.netMode == NetmodeID.Server)
                SendOtherPlayerDebuff(-1, fromWho, whoAmI, targetPlayerWhoAmI, debuffType, debuffTime);
        }

        public void SendOtherPlayerExtraEffect(int toWho, int fromWho, byte whoAmI, int targetPlayerWhoAmI, int effectType, int int1, int int2, float float1, float float2)
        {
            ModPacket packet = GetPacket(OtherPlayerExtraEffect, fromWho);
            packet.Write(whoAmI);
            packet.Write(targetPlayerWhoAmI);
            packet.Write(effectType);
            packet.Write(int1);
            packet.Write(int2);
            packet.Write(float1);
            packet.Write(float2);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveOtherPlayerExtraEffect(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetPlayerWhoAmI = reader.ReadInt32();
            int effectType = reader.ReadInt32();
            int int1 = reader.ReadInt32();
            int int2 = reader.ReadInt32();
            float float1 = reader.ReadSingle();
            float float2 = reader.ReadSingle();

            if (effectType == 1) //echoes 3freeze
            {
                Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesDamageBoost = float1;
                if (Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesFreeze <= 15)
                    Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesFreeze += 30;
            }
            if (effectType == 2) //echoes act 1 debuff
            {
                Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesDamageBoost = float1;
                Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensivityMax = int1;
                if (Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensivity < Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensivityMax)
                    Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensivity += int2;
            }

            if (Main.netMode == NetmodeID.Server)
                SendOtherPlayerExtraEffect(-1, fromWho, whoAmI, targetPlayerWhoAmI, effectType, int1, int2, float1, float2);
        }
    }
}