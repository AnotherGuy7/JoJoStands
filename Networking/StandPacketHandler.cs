using JoJoStands.Projectiles.PlayerStands;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public class StandPacketHandler : PacketHandler
    {
        public const byte StandData = 0;

        public StandPacketHandler(byte handlerType) : base(handlerType)
        { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            byte messageType = reader.ReadByte();
            switch (messageType)
            {
                case StandData:
                    ReadStandData(reader, fromWho);
                    break;

                default:
                    Main.NewText("An unknown Stand packet type has been received. Discarding data.");
                    break;
            }
        }

        public static void SendStandData(byte dataType, int standWhoAmI, int playerWhoAmI, int value1 = 0, int value2 = 0, int value3 = 0)
        {
            ModNetHandler.standSync.SendStandPacket(256, playerWhoAmI, dataType, standWhoAmI, value1, value2, value3);
        }

        public void SendStandPacket(int toWho, int fromWho, byte dataType, int standWhoAmI, int value1, int value2, int value3)
        {
            ModPacket packet = GetPacket(StandData, fromWho);
            packet.Write(dataType);
            packet.Write(standWhoAmI);
            packet.Write(value1);
            packet.Write(value2);
            packet.Write(value3);
            packet.Send(toWho, fromWho);
        }

        public void ReadStandData(BinaryReader reader, int fromWho)       //HandlePacket leads the packet here and it is read and applied
        {
            byte dataType = reader.ReadByte();
            int standWhoAmI = reader.ReadInt32();
            int value1 = reader.ReadInt32();
            int value2 = reader.ReadInt32();
            int value3 = reader.ReadInt32();
            if (Main.netMode != NetmodeID.Server)
            {
                if (!Main.projectile[standWhoAmI].active)       //Projectile IDs are not synced. :l
                    return;

                StandClass stand = Main.projectile[standWhoAmI].ModProjectile as StandClass;
                stand.ReceiveStandData(dataType, value1, value2, value3);
                Main.NewText("G");
            }
            else
            {
                SendStandPacket(-1, fromWho, dataType, standWhoAmI, value1, value2, value3);
            }
        }
    }
}