using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Pierced : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Pierced!");
            Description.SetDefault("You have been pierced by an arrow shard!");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.piercedTimer--;
            if (player.whoAmI == Main.myPlayer)
            {
                if (mPlayer.piercedTimer <= 0 || player.buffTime[buffIndex] <= 2)
                {
                    player.QuickSpawnItem(player.GetSource_FromThis(), Main.rand.Next(JoJoStands.standTier1List.ToArray()));
                    mPlayer.piercedTimer = 36000;
                    player.ClearBuff(Type);
                }
            }
        }
    }
}