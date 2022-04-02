using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.PlayerBuffs
{
    public class StrongWill : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Strong Will");
            Description.SetDefault("You feel as if you and your stand can do anything!\n+10% Increased Stand Damage");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.1f;
        }
    }
}