using Terraria;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class SharpMind : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharp Mind");
            Description.SetDefault("Your mental reflexes have sharpened and thus, your Stand Crit Chance increased by 10%");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10f;
        }
    }
}