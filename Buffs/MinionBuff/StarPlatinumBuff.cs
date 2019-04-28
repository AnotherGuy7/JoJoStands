using System;
using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.MinionBuff
{
    public class StarPlatinumBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Star Platinum");
            Description.SetDefault("Star Platinum will help you fight!");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (player.ownedProjectileCounts[mod.ProjectileType("StarPlatinumMinion")] > 0)
            {
                modPlayer.StarPlatinumMinion = true;
            }
            if (!modPlayer.StarPlatinumMinion)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}