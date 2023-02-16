using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public class WorldPacketHandler : PacketHandler
    {
        public const byte VampiricNight = 0;

        public WorldPacketHandler(byte handlerType) : base(handlerType)
        { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            byte messageType = reader.ReadByte();
            switch (messageType)
            {
                case VampiricNight:
                    ReceiveVampiricNight(reader, fromWho);
                    break;
            }
        }

        public void SendVampiricNight(int toWho, int fromWho, bool active)         //OR packet.Write(Main.player[fromWho].GetModPlayer<MyPlayer>().TheWorldEffect);  if you want to simplify using the method
        {
            ModPacket packet = CreatePacket(VampiricNight);
            packet.Write(active);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveVampiricNight(BinaryReader reader, int fromWho)
        {
            bool vampiricNightActive = reader.ReadBoolean();
            if (Main.netMode != NetmodeID.Server)
            {
                JoJoStandsWorld.VampiricNight = vampiricNightActive;
            }
            else
            {
                JoJoStandsWorld.VampiricNight = vampiricNightActive;
                SendVampiricNight(-1, fromWho, vampiricNightActive);
            }
        }
    }
}