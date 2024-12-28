using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class StrayCatBuff : JoJoBuff
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
            player.buffTime[buffIndex] = 2;
        }
    }
}