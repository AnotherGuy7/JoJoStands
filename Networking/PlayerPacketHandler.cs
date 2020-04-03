using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Networking
{
	internal class PlayerPacketHandler : PacketHandler
	{
		public const byte PoseMode = 0;
		public const byte StandOut = 1;		//needed because some stands don't spawn without it
		public const byte StandAutoMode = 2;
		public const byte CBLayer = 3;
		
		public PlayerPacketHandler(byte handlerType) : base(handlerType)
		{
		}

		public override void HandlePacket(BinaryReader reader, int fromWho)		//decides what happens when a packet is received, it looks for the byte sent with the packet and uses the proper method
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
			}
		}

		public void SendPoseMode(int toWho, int fromWho, bool poseModeValue, int whoAmI)		//send the packet whenever its called to
		{
			ModPacket packet = GetPacket(PoseMode, fromWho);
			packet.Write(poseModeValue);
			packet.Write(whoAmI);
			packet.Send(toWho, fromWho);
		}

		public void ReceivePoseMode(BinaryReader reader, int fromWho)		//HandlePacket leads the packet here and it is read and applied
		{
			bool poseModeVal = reader.ReadBoolean();
			int whoAmI = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[whoAmI].GetModPlayer<MyPlayer>().poseMode = poseModeVal;

			}
			else
			{
				SendPoseMode(-1, fromWho, poseModeVal, whoAmI);

			}
		}

		public void SendStandOut(int toWho, int fromWho, bool standOutValue, int whoAmI)
		{
			ModPacket packet = GetPacket(StandOut, fromWho);
			packet.Write(standOutValue);
			packet.Write(whoAmI);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveStandOut(BinaryReader reader, int fromWho)
		{
			bool standOutVal = reader.ReadBoolean();
			int whoAmI = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[whoAmI].GetModPlayer<MyPlayer>().StandOut = standOutVal;

			}
			else
			{
				SendStandOut(-1, fromWho, standOutVal, whoAmI);

			}
		}

		public void SendStandAutoMode(int toWho, int fromWho, bool autoModeValue, int whoAmI)
		{
			ModPacket packet = GetPacket(StandAutoMode, fromWho);
			packet.Write(autoModeValue);
			packet.Write(whoAmI);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveStandAutoMode(BinaryReader reader, int fromWho)
		{
			bool autoModeVal = reader.ReadBoolean();
			int whoAmI = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[whoAmI].GetModPlayer<MyPlayer>().StandAutoMode = autoModeVal;

			}
			else
			{
				SendStandAutoMode(-1, fromWho, autoModeVal, whoAmI);

			}
		}

		public void SendCBLayer(int toWho, int fromWho, bool visibility, int whoAmI)
		{
			ModPacket packet = GetPacket(CBLayer, fromWho);
			packet.Write(visibility);
			packet.Write(whoAmI);
			packet.Send(toWho, fromWho);
		}

		public void ReceiveCBLayer(BinaryReader reader, int fromWho)
		{
			bool visibiltyValue = reader.ReadBoolean();
			int whoAmI = reader.ReadInt32();
			if (Main.netMode != NetmodeID.Server)
			{
				Main.player[whoAmI].GetModPlayer<MyPlayer>().showingCBLayer = visibiltyValue;

			}
			else
			{
				SendCBLayer(-1, fromWho, visibiltyValue, whoAmI);
			}
		}
	}
}