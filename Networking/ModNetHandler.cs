using System.IO;

namespace JoJoStands.Networking
{
	internal class ModNetHandler
	{
		public const byte Effect = 0;
		public const byte Player = 1;

		internal static EffectPacketHandler effectSync = new EffectPacketHandler(Effect);
		internal static PlayerPacketHandler playerSync = new PlayerPacketHandler(Player);
		public static void HandlePacket(BinaryReader reader, int fromWho)
		{
			byte messageType = reader.ReadByte();
			switch (messageType)
			{
				case Effect:
					effectSync.HandlePacket(reader, fromWho);
					break;
				case Player:
					playerSync.HandlePacket(reader, fromWho);
					break;
			}
		}
	}
}