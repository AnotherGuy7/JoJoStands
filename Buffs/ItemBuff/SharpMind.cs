using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class SharpMind : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sharp Mind");
            Description.SetDefault("Your mental reflexes have sharpened and thus, your Stand Speed increased by 1");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 1;
        }
    }
}