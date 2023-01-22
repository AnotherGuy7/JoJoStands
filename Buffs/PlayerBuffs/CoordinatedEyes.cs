using Terraria;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class CoordinatedEyes : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Coordinated Eyes");
            Description.SetDefault("Your eyes are able to focus on everything, thus, your stand can now go even farther. (+1 Tile Range Radius)");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standRangeBoosts += 32f;
        }
    }
}