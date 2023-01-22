using JoJoStands.Mounts;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Buffs.ItemBuff
{
    public class RoadRollerBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Road Roller");
            Description.SetDefault("A destroyer of lands, capable of flattening the world. ROAD ROLLER DA!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.mount.SetMount(MountType<RoadRollerMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}