using System.IO;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public abstract class PacketHandler
    {
        internal byte HandlerType { get; set; }

        public abstract void HandlePacket(BinaryReader reader, int fromWho);

        protected PacketHandler(byte handlerType)
        {
            HandlerType = handlerType;
        }

        protected ModPacket CreatePacket(byte packetType)
        {
            ModPacket packet = JoJoStands.Instance.GetPacket();
            packet.Write(HandlerType);
            packet.Write(packetType);
            return packet;
        }
    }
}