using JoJoStands.Items;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class DollyDaggerActiveBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dolly Dagger");
            Description.SetDefault("When hit, a certain percentage of damage is reflected back to the enemy.");
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standAccessory)     //rather than having to check the Stand Slot for any 4 items
            {
                if (mPlayer.StandSlot.Item.type == ModContent.ItemType<DollyDaggerT1>())
                    player.endurance += 0.35f;
                if (mPlayer.StandSlot.Item.type == ModContent.ItemType<DollyDaggerT2>())
                    player.endurance += 0.7f;

                player.buffTime[buffIndex] = 10;
            }
        }
    }
}