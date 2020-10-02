using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.PlayerBuffs
{
    public class SharpMind : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sharp Mind");
            Description.SetDefault("Your mental reflexes have sharpened and thus, your Stand Crit Chance increased by 10%");
            Main.buffNoTimeDisplay[Type] = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 10f;
        }
    }
}