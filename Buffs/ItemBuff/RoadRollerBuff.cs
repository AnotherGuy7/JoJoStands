using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class RoadRollerBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Road Roller");
            Description.SetDefault("A destoryer of lands, capable of flattening the world. ROAD ROLLER DA!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("RoadRollerMount"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}