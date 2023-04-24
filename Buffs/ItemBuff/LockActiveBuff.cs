using Terraria;

namespace JoJoStands.Buffs.ItemBuff
{
    public class LockActiveBuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lock");
            // Description.SetDefault("Enemies that hit you will feel the weight of their guilt.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.GetModPlayer<MyPlayer>().standAccessory)     //rather than having to check the Stand Slot for any 4 items
                player.buffTime[buffIndex] = 10;
        }
    }
}