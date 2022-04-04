using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.PlayerBuffs
{
    public class CoordinatedEyes : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Coordinated Eyes");
            Description.SetDefault("Your eyes are able to focus on everything, thus, your stand can now go even farther. (+1 Tile Range Radius)");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standRangeBoosts += 32f;
        }
    }
}