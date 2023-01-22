using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Weave : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Weave");
            Description.SetDefault("These strings create a sheet as strong as metal when woven together... and you're wrapped in it!");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.GetModPlayer<MyPlayer>().stoneFreeWeaveAbilityActive)
                player.buffTime[buffIndex] = 2;
        }
    }
}