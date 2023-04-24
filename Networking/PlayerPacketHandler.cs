using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public class PlayerPacketHandler : PacketHandler
    {
        public const byte PoseMode = 0;
        public const byte StandOut = 1;     //needed because some stands don't spawn without it
        public const byte StandControlStyle = 2;
        public const byte DyeItem = 4;
        public const byte SexPistolPosition = 5;
        public const byte DeathLoopInfo = 6;
        public const byte ArrowEarringInfo = 7;
        public const byte StandEffectInfo = 8;
        public const byte OtherPlayerDebuffInfo = 9;
        public const byte OtherPlayerExtraEffect = 10;
        //public const byte Sounds = 6;

        public PlayerPacketHandler(byte handlerType) : base(handlerType)
        { }

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
                case StandControlStyle:
                    ReceiveStandControlStyle(reader, fromWho);
                    break;
                case SexPistolPosition:
                    ReceiveSexPistolPosition(reader, fromWho);
                    break;
                case DyeItem:
                    ReceiveDyeItem(reader, fromWho);
                    break;
                case DeathLoopInfo:
                    ReceiveDeathLoopInfo(reader, fromWho);
                    break;
                case ArrowEarringInfo:
                    ReceiveArrowEarringInfo(reader, fromWho);
                    break;
                case StandEffectInfo:
                    ReceiveStandEffectInfo(reader, fromWho);
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
            ModPacket packet = CreatePacket(PoseMode);
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

        public void SendStandOut(int toWho, int fromWho, bool standOutValue, string standName, byte standTier, byte whoAmI)
        {
            ModPacket packet = CreatePacket(StandOut);
            packet.Write(standOutValue);
            packet.Write(standName);
            packet.Write(standTier);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveStandOut(BinaryReader reader, int fromWho)
        {
            bool standOutVal = reader.ReadBoolean();
            string standName = reader.ReadString();
            byte standTier = reader.ReadByte();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                if (!standOutVal)
                {
                    Main.player[whoAmI].GetModPlayer<MyPlayer>().standTier = 0;
                    Main.player[whoAmI].GetModPlayer<MyPlayer>().standName = "";
                }
                else
                {
                    Main.player[whoAmI].GetModPlayer<MyPlayer>().standTier = standTier;
                    Main.player[whoAmI].GetModPlayer<MyPlayer>().standName = standName;
                }
                Main.player[whoAmI].GetModPlayer<MyPlayer>().standOut = standOutVal;
            }
            else
            {
                SendStandOut(-1, fromWho, standOutVal, standName, standTier, whoAmI);
            }
        }

        public void SendStandControlStyle(int toWho, int fromWho, MyPlayer.StandControlStyle standControlStyle, byte whoAmI)
        {
            ModPacket packet = CreatePacket(StandControlStyle);
            packet.Write((byte)standControlStyle);
            packet.Write(whoAmI);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveStandControlStyle(BinaryReader reader, int fromWho)
        {
            byte standControlStyle = reader.ReadByte();
            byte whoAmI = reader.ReadByte();
            if (Main.netMode != NetmodeID.Server)
            {
                Main.player[whoAmI].GetModPlayer<MyPlayer>().standControlStyle = (MyPlayer.StandControlStyle)standControlStyle;
            }
            else
            {
                SendStandControlStyle(-1, fromWho, (MyPlayer.StandControlStyle)standControlStyle, whoAmI);
            }
        }

        public void SendDyeItem(int toWho, int fromWho, int dyeItemType, byte whoAmI)
        {
            ModPacket packet = CreatePacket(DyeItem);
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
			ModPacket packet = CreatePacket(Sounds, fromWho);
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
            ModPacket packet = CreatePacket(DyeItem);
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
            ModPacket packet = CreatePacket(DeathLoopInfo);
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
            ModPacket packet = CreatePacket(ArrowEarringInfo);
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

        public void SendStandEffectInfo(int toWho, int fromWho, byte whoAmI, int targetWhoAmI, int fistWhoAmI, int stat1 = 0, int stat2 = 0, int stat3 = 0, float stat4 = 0f, float stat5 = 0f)
        {
            ModPacket packet = CreatePacket(StandEffectInfo);
            packet.Write(whoAmI);
            packet.Write(targetWhoAmI);
            packet.Write(fistWhoAmI);
            packet.Write(stat1);
            packet.Write(stat2);
            packet.Write(stat3);
            packet.Write(stat4);
            packet.Write(stat5);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveStandEffectInfo(BinaryReader reader, int fromWho)
        {
            byte whoAmI = reader.ReadByte();
            int targetWhoAmI = reader.ReadInt32();
            int fistWhoAmI = reader.ReadInt32();
            int stat1 = reader.ReadInt32();
            int stat2 = reader.ReadInt32();
            int stat3 = reader.ReadInt32();
            float stat4 = reader.ReadSingle();
            float stat5 = reader.ReadSingle();

            if (fistWhoAmI == 0)
                Main.npc[targetWhoAmI].velocity.X *= 0.2f;
            if (fistWhoAmI == 3)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().affectedbyBtz = true;
            if (fistWhoAmI == 4)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = stat1;
            if (fistWhoAmI == 5)
            {
                bool crit = false;
                if (stat1 != 0)
                    modifiers.SetCrit();
                Main.npc[targetWhoAmI].StrikeNPC(stat2, 7f, stat3, crit);
            }
            if (fistWhoAmI == 6 && stat1 == 1)
            {
                Main.projectile[targetWhoAmI].penetrate -= 1;
                if (Main.projectile[targetWhoAmI].penetrate <= 0)
                    Main.projectile[targetWhoAmI].Kill();
            }
            if (fistWhoAmI == 6 && stat1 == 2)
                Main.npc[targetWhoAmI].StrikeNPC(stat2, stat4, stat3);
            if (fistWhoAmI == 8)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner = stat1;
            if (fistWhoAmI == 9)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().whitesnakeDISCImmune += 1;
            if (fistWhoAmI == 10 && stat1 == 1)
                Main.projectile[targetWhoAmI].velocity *= -1;
            if (fistWhoAmI == 10 && stat1 == 2)
                Main.npc[targetWhoAmI].StrikeNPC(stat2, 6f, stat3);
            if (fistWhoAmI == 12)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().crazyDiamondPunchCount += 1;
            if (fistWhoAmI == 13)
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().towerOfGrayImmunityFrames = 30;
            if (fistWhoAmI == 15 && stat1 == 3)
            {
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = stat4;
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = stat5;
                if (Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer <= 15)
                    Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesThreeFreezeTimer += 30;
            }
            if (fistWhoAmI == 15 && stat1 == 1)
            {
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesCrit = stat4;
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesDamageBoost = stat5;
                Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundMaxIntensity = stat2;
                if (Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensity < Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundMaxIntensity)
                    Main.npc[targetWhoAmI].GetGlobalNPC<JoJoGlobalNPC>().echoesSoundIntensity += stat3;
            }
            if (Main.netMode == NetmodeID.Server)
                SendStandEffectInfo(-1, fromWho, whoAmI, targetWhoAmI, fistWhoAmI, stat1, stat2, stat3, stat4, stat5);
        }

        public void SendOtherPlayerDebuff(int toWho, int fromWho, byte whoAmI, int targetPlayerWhoAmI, int debuffType, int debuffTime)
        {
            ModPacket packet = CreatePacket(OtherPlayerDebuffInfo);
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
            ModPacket packet = CreatePacket(OtherPlayerExtraEffect);
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
                Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensityMax = int1;
                if (Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensity < Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensityMax)
                    Main.player[targetPlayerWhoAmI].GetModPlayer<MyPlayer>().echoesSoundIntensity += int2;
            }
            if (effectType == 3) //SHA explosion
                Main.player[targetPlayerWhoAmI].Hurt(PlayerDeathReason.ByCustomReason(Main.player[targetPlayerWhoAmI].name + " had too high a heat signurature."), int1, int2);

            if (Main.netMode == NetmodeID.Server)
                SendOtherPlayerExtraEffect(-1, fromWho, whoAmI, targetPlayerWhoAmI, effectType, int1, int2, float1, float2);
        }
    }
}