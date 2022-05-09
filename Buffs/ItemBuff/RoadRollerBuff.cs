using JoJoStands.Mounts;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Buffs.ItemBuff
{
    public class RoadRollerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Road Roller");
            Description.SetDefault("A destoryer of lands, capable of flattening the world. ROAD ROLLER DA!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(MountType<RoadRollerMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}