using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class LockActiveBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Lock");
            Description.SetDefault("Enemies that hit you will feel the weight of their guilt.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<MyPlayer>().standAccessory)
            {
                player.buffTime[buffIndex] = 10;
            }
        }
    }
}