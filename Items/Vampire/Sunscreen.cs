using JoJoStands.Buffs.ItemBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class Sunscreen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunscreen");
            Tooltip.SetDefault("For all the hot days you spend wandering.");
            SacrificeTotal = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(silver: 2, copper: 50);
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.maxStack = 99;
            Item.buffTime = 3 * 60 * 60;
            Item.buffType = ModContent.BuffType<SunscreenBuff>();
        }
    }
}
