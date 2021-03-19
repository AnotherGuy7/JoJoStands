using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;

namespace JoJoStands.Networking
{
	public class EffectPacketHandler : PacketHandler
	{
		public const byte Timestop = 0;
		public const byte Timestop2 = 1;
		public const byte Timeskip = 2;
		public const byte Timeskip2 = 3;
		public const byte BacktoZero = 4;
		public const byte BacktoZero2 = 5;
		public const byte Foresight = 6;
		public const byte Foresight2 = 7;

		public EffectPacketHandler(byte handlerType) : base(handlerType)
		{
		}

		public override void HandlePacket(BinaryReader reader, int fromWho)
		{
			byte messageType = reader.ReadByte();
			switch (messageType)
			{
				case Timestop:
					ReceiveTimestop(reader, fromWho);
					break;
				case Timestop2:
					ReceiveTimestopFromAffected(reader, fromWho);
					break;
				case Timeskip:
					ReceiveTimeskip(reader, fromWho);
					break;
				case Timeskip2:
					ReceiveTimeskipFromAffected(reader, fromWho);
					break;
				case BacktoZero:
					ReceiveBTZ(reader, fromWho);
					break;
				case BacktoZero2:
					ReceiveBTZpFromAffected(reader, fromWho);
					break;
				case Foresight:
					ReceiveForesight(reader, fromWho);
					break;
				case Foresight2:
					ReceiveForesightFromAffected(reader, fromWho);
					break;
			}
		}

		public void SendTimestop(int toWho, int fromWho, bool timestopValue, int timestopOwner)         //OR packet.Write(Main.player[fromWho].GetModPlayer<MyPlayer>().TheWorldEffect);  if you want to simplify using the method
		{
			ModPacket packet = GetPacket(Timestop, fromWho);
			packet.Write(timestopValue);
			packet.Write(timestopOwner);
			packet.Send(toWho, fromWho);
		}

		public void SendTimestopBackToOwner(int toWho, int fromWho, bool timestopValue, int affectedPlayer)
		{
			ModPacket packet = GetPacket(Timestop2, fromWho);
			packet.Write(timestopValue);
			packet.Write(affectedPlayer);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveTimestop(BinaryReader reader, int fromWho)
		{
			bool timestopValue = reader.ReadBoolean();
			int timestopOwner = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[timestopOwner].GetModPlayer<MyPlayer>().TheWorldEffect = timestopValue;
				Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().TheWorldEffect = timestopValue;
				if (timestopValue)
				{
					Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().timestopEffectDurationTimer = 60;
				}
				else
				{
					Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().timestopEffectDurationTimer = 60;
					if (Filters.Scene["GreyscaleEffect"].IsActive())
					{
						Filters.Scene["GreyscaleEffect"].Deactivate();
					}
				}
				SendTimestopBackToOwner(timestopOwner, Main.myPlayer, timestopValue, Main.myPlayer);
			}
			else
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active)
					{
						npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().frozenInTime = timestopValue;
					}
				}
				SendTimestop(-1, fromWho, timestopValue, timestopOwner);
			}
		}

		public void ReceiveTimestopFromAffected(BinaryReader reader, int fromWho)
		{
			bool timestopValue = reader.ReadBoolean();
			int affectedPlayer = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[affectedPlayer].GetModPlayer<MyPlayer>().TheWorldEffect = timestopValue;
			}
			else
			{
				SendTimestopBackToOwner(-1, fromWho, timestopValue, affectedPlayer);
			}
		}




		public void SendTimeskip(int toWho, int fromWho, bool timeskipValue, int timeskipOwner)
		{
			ModPacket packet = GetPacket(Timeskip, fromWho);
			packet.Write(timeskipValue);
			packet.Write(timeskipOwner);
			packet.Send(toWho, fromWho);
		}

		public void SendTimeskipBackToOwner(int toWho, int fromWho, bool timeskipValue, int affectedPlayer)
		{
			ModPacket packet = GetPacket(Timeskip2, fromWho);
			packet.Write(timeskipValue);
			packet.Write(affectedPlayer);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveTimeskip(BinaryReader reader, int fromWho)
		{
			bool timeskipValue = reader.ReadBoolean();
			int timeskipOwner = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[timeskipOwner].GetModPlayer<MyPlayer>().TimeSkipEffect = timeskipValue;
				Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().TimeSkipEffect = timeskipValue;
				SendTimeskipBackToOwner(timeskipOwner, Main.myPlayer, timeskipValue, Main.myPlayer);
			}
			else
			{
				SendTimeskip(-1, fromWho, timeskipValue, timeskipOwner);
			}
		}

		public void ReceiveTimeskipFromAffected(BinaryReader reader, int fromWho)
		{
			bool timeskipValue = reader.ReadBoolean();
			int affectedPlayer = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[affectedPlayer].GetModPlayer<MyPlayer>().TimeSkipEffect = timeskipValue;
			}
			else
			{
				SendTimeskipBackToOwner(-1, fromWho, timeskipValue, affectedPlayer);
			}
		}




		public void SendBTZ(int toWho, int fromWho, bool BTZValue, int BTZOwner)
		{
			ModPacket packet = GetPacket(BacktoZero, fromWho);
			packet.Write(BTZValue);
			packet.Write(BTZOwner);
			packet.Send(toWho, fromWho);
		}

		public void SendBTZBackToOwner(int toWho, int fromWho, bool BTZValue, int affectedPlayer)
		{
			ModPacket packet = GetPacket(BacktoZero2, fromWho);
			packet.Write(BTZValue);
			packet.Write(affectedPlayer);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveBTZ(BinaryReader reader, int fromWho)
		{
			bool BTZValue = reader.ReadBoolean();
			int BTZOwner = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().backToZero = BTZValue;
				Main.player[BTZOwner].GetModPlayer<MyPlayer>().backToZero = BTZValue;
				SendBTZBackToOwner(BTZOwner, Main.myPlayer, BTZValue, Main.myPlayer);
			}
			else
			{
				SendBTZ(-1, fromWho, BTZValue, BTZOwner);
			}
		}

		public void ReceiveBTZpFromAffected(BinaryReader reader, int fromWho)
		{
			bool BTZValue = reader.ReadBoolean();
			int affectedPlayer = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[affectedPlayer].GetModPlayer<MyPlayer>().backToZero = BTZValue;
			}
			else
			{
				SendBTZBackToOwner(-1, fromWho, BTZValue, affectedPlayer);
			}
		}



		public void SendForesight(int toWho, int fromWho, bool foresightValue, int foresightOwner)
		{
			ModPacket packet = GetPacket(Foresight, fromWho);
			packet.Write(foresightValue);
			packet.Write(foresightOwner);
			packet.Send(toWho, fromWho);
		}

		public void SendForesightBackToOwner(int toWho, int fromWho, bool foresightValue, int affectedPlayer)
		{
			ModPacket packet = GetPacket(Foresight2, fromWho);
			packet.Write(foresightValue);
			packet.Write(affectedPlayer);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveForesight(BinaryReader reader, int fromWho)
		{
			bool foresightValue = reader.ReadBoolean();
			int foresightOwner = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[foresightOwner].GetModPlayer<MyPlayer>().epitaphForesight = foresightValue;
				Main.player[Main.myPlayer].GetModPlayer<MyPlayer>().epitaphForesight = foresightValue;
				SendForesightBackToOwner(foresightOwner, Main.myPlayer, foresightValue, Main.myPlayer);
			}
			else
			{
				SendForesight(-1, fromWho, foresightValue, foresightOwner);
			}
		}

		public void ReceiveForesightFromAffected(BinaryReader reader, int fromWho)
		{
			bool foresightValue = reader.ReadBoolean();
			int affectedPlayer = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[affectedPlayer].GetModPlayer<MyPlayer>().epitaphForesight = foresightValue;
			}
			else
			{
				SendForesightBackToOwner(-1, fromWho, foresightValue, affectedPlayer);
			}
		}
	}
}