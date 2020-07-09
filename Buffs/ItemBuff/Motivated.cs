using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class Motivated : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Motivated");
            Description.SetDefault("You feel as if you and your stand can do anything!\n+10% Increased Stand Damage");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.05f;
        }
    }
}