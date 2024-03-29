using System.IO;

namespace JoJoStands.Networking
{
	public class ModNetHandler
	{
		public const byte Effect = 0;
		public const byte Player = 1;
		public const byte World = 2;

		public static readonly EffectPacketHandler EffectSync = new EffectPacketHandler(Effect);
		public static readonly PlayerPacketHandler PlayerSync = new PlayerPacketHandler(Player);
		public static readonly WorldPacketHandler WorldSync = new WorldPacketHandler(World);

		public static void HandlePacket(BinaryReader reader, int fromWho)
		{
			byte messageType = reader.ReadByte();
			switch (messageType)
			{
				case Effect:
					EffectSync.HandlePacket(reader, fromWho);
					break;
				case Player:
					PlayerSync.HandlePacket(reader, fromWho);
					break;
				case World:
					WorldSync.HandlePacket(reader, fromWho);
					break;
			}
		}
	}
}