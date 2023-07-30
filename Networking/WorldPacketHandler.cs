using JoJoStands.NPCs;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public class WorldPacketHandler : PacketHandler
    {
        public const byte VampiricNight = 0;
        public const byte EnemySync = 1;

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
                case EnemySync:
                    ReceiveEnemySync(reader, fromWho);
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
            JoJoStandsWorld.VampiricNight = vampiricNightActive;
            if (Main.netMode == NetmodeID.Server)
                SendVampiricNight(-1, fromWho, vampiricNightActive);
        }

        public void SendEnemySync(int toWho, int fromWho, byte effectIndex, bool state, int info, int enemyIndex)
        {
            ModPacket packet = CreatePacket(EnemySync);
            packet.Write(effectIndex);
            packet.Write(state);
            packet.Write(info);
            packet.Write(enemyIndex);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveEnemySync(BinaryReader reader, int fromWho)
        {
            byte effectIndex = reader.ReadByte();
            bool state = reader.ReadBoolean();
            int info = reader.ReadInt32();
            int enemyIndex = reader.ReadInt32();

            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.npc[enemyIndex] != null && Main.npc[enemyIndex].active)
                    Main.npc[enemyIndex].GetGlobalNPC<JoJoGlobalNPC>().ReceiveEffect(effectIndex, state, info);
            }
            else
            {
                SendEnemySync(-1, fromWho, effectIndex, state, info, enemyIndex);
            }
        }
    }
}