using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class SoothingSpiritDisc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soothing Spirit Disc");
            Tooltip.SetDefault("Stand attack grants Holy Protection");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 4);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().soothingSpiritDisc = true;
        }
    }
}