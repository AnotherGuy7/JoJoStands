﻿using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandT1 : StandItemClass
    {
        public override int StandSpeed => 14;
        public override int StandType => 1;
        public override string StandIdentifierName => "TheHand";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => TheHandFinal.TheHandTierColor;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Hand (Tier 1)");
            // Tooltip.SetDefault("Punch enemies at a really fast rate!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
