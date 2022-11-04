using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class UnderbossPhone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Underboss Phone");
            Tooltip.SetDefault("Every fifth hit is a little stronger\nThis attack completely ignores defense");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().underbossPhone = true;
        }
    }
}