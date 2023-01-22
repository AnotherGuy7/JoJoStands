using Terraria;

namespace JoJoStands.Buffs.PlayerBuffs
{
    public class StrongWill : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Strong Will");
            Description.SetDefault("You feel as if you and your stand can do anything!\n+10% Increased Stand Damage");
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.1f;
        }
    }
}