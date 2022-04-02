using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class Pierced : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Pierced!");
            Description.SetDefault("You have been pierced by an arrow shard!");
            Main.debuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.piercedTimer--;
            if (player.whoAmI == Main.myPlayer)
            {
                if (mPlayer.piercedTimer <= 0 || player.buffTime[buffIndex] <= 2)
                {
                    player.QuickSpawnItem(Main.rand.Next(JoJoStands.standTier1List.ToArray()));
                    mPlayer.piercedTimer = 36000;
                    player.ClearBuff(mod.BuffType(Name));
                }
            }
        }
    }
}