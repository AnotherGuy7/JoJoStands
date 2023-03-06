using JoJoStands.NPCs;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Networking
{
    public class EffectPacketHandler : PacketHandler
    {
        public const byte TimestopSync = 0;
        public const byte TimeskipSync = 1;
        public const byte BacktoZeroSync = 2;
        public const byte ForesightSync = 3;
        public const byte BitesTheDustSync = 4;

        public EffectPacketHandler(byte handlerType) : base(handlerType)
        { }

        public override void HandlePacket(BinaryReader reader, int fromWho)
        {
            byte messageType = reader.ReadByte();
            switch (messageType)
            {
                case TimestopSync:
                    ReceiveTimestop(reader, fromWho);
                    break;
                case TimeskipSync:
                    ReceiveTimeskip(reader, fromWho);
                    break;
                case BacktoZeroSync:
                    ReceiveBackToZero(reader, fromWho);
                    break;
                case ForesightSync:
                    ReceiveForesight(reader, fromWho);
                    break;
                case BitesTheDustSync:
                    ReceiveBTD(reader, fromWho);
                    break;
            }
        }

        /// <summary>
        /// Sends a packet with information on the timestop's state and who owns it.
        /// </summary>
        /// <param name="toWho">Who to send the packet to. It's best to leave this as 256. (The server)</param>
        /// <param name="fromWho"></param>
        /// <param name="timestopValue"></param>
        /// <param name="timestopOwner">The owner of the timestop. Leave as -1 if there is no owner.</param>
        public void SendTimestop(int toWho, int fromWho, bool timestopValue, short timestopOwner = -1)         //OR packet.Write(Main.player[fromWho].GetModPlayer<MyPlayer>().TheWorldEffect);  if you want to simplify using the method
        {
            ModPacket packet = CreatePacket(TimestopSync);
            packet.Write(timestopValue);
            packet.Write(timestopOwner);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveTimestop(BinaryReader reader, int fromWho)
        {
            bool timestopValue = reader.ReadBoolean();
            short timestopOwner = reader.ReadInt16();
            if (Main.netMode != NetmodeID.Server)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                    {
                        otherPlayer.GetModPlayer<MyPlayer>().timestopActive = timestopValue;
                        if (p == timestopOwner)
                            otherPlayer.GetModPlayer<MyPlayer>().timestopOwner = true;
                        if (!timestopValue && otherPlayer.GetModPlayer<MyPlayer>().timestopOwner)
                            otherPlayer.GetModPlayer<MyPlayer>().timestopOwner = false;

                    }
                }
                Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().timestopEffectDurationTimer = 60;
            }
            else
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                        npc.GetGlobalNPC<JoJoGlobalNPC>().frozenInTime = timestopValue;
                }
                SendTimestop(-1, fromWho, timestopValue, timestopOwner);
            }
        }

        public void SendTimeskip(int toWho, int fromWho, bool timeskipValue)
        {
            ModPacket packet = CreatePacket(TimeskipSync);
            packet.Write(timeskipValue);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveTimeskip(BinaryReader reader, int fromWho)
        {
            bool timeskipValue = reader.ReadBoolean();
            if (Main.netMode != NetmodeID.Server)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().timeskipActive = timeskipValue;
                }
            }
            else
            {
                SendTimeskip(-1, fromWho, timeskipValue);
            }
        }

        public void SendBackToZero(int toWho, int fromWho, bool backToZeroValue)
        {
            ModPacket packet = CreatePacket(BacktoZeroSync);
            packet.Write(backToZeroValue);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveBackToZero(BinaryReader reader, int fromWho)
        {
            bool backToZeroValue = reader.ReadBoolean();
            if (Main.netMode != NetmodeID.Server)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().backToZeroActive = backToZeroValue;
                }
            }
            else
            {
                SendBackToZero(-1, fromWho, backToZeroValue);
            }
        }

        public void SendForesight(int toWho, int fromWho, bool foresightValue)
        {
            ModPacket packet = CreatePacket(ForesightSync);
            packet.Write(foresightValue);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveForesight(BinaryReader reader, int fromWho)
        {
            bool foresightValue = reader.ReadBoolean();
            if (Main.netMode != NetmodeID.Server)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().epitaphForesightActive = foresightValue;
                }
            }
            else
            {
                SendForesight(-1, fromWho, foresightValue);
            }
        }

        public void SendBitesTheDust(int toWho, int fromWho, bool btdValue)
        {
            ModPacket packet = CreatePacket(BitesTheDustSync);
            packet.Write(btdValue);
            packet.Send(toWho, fromWho);
        }

        public void ReceiveBTD(BinaryReader reader, int fromWho)
        {
            bool btdValue = reader.ReadBoolean();
            if (Main.netMode != NetmodeID.Server)
            {
                for (int p = 0; p < Main.maxPlayers; p++)
                {
                    Player otherPlayer = Main.player[p];
                    if (otherPlayer.active)
                        otherPlayer.GetModPlayer<MyPlayer>().bitesTheDustActive = btdValue;
                }
            }
            else
            {
                SendBitesTheDust(-1, fromWho, btdValue);
            }
        }
    }
}