using JoJoStands.Mounts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SlowDancerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slow Dancer");
            Description.SetDefault("A fast horse capable of generating large amounts of spin energy.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(MountType<SlowDancerMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}