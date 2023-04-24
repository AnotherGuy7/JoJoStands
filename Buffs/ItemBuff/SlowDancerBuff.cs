using JoJoStands.Mounts;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SlowDancerBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slow Dancer");
            // Description.SetDefault("A fast horse capable of generating large amounts of spin energy.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.mount.SetMount(MountType<SlowDancerMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}