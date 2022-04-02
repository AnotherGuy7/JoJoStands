using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class SlowDancerBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Slow Dancer");
            Description.SetDefault("A fast horse capable of generating large amounts of spin energy.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("SlowDancerMount"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}