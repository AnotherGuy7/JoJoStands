﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
    [AutoloadEquip(EquipType.Head)]
    public class OkuyasuHair : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Okuyasu Wig");
            // Tooltip.SetDefault("A wig in the hairstyle of Okuyasu Nijimura, minus the facial scars.");
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 14;
            Item.rare = ItemRarityID.LightPurple;
            Item.vanity = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}