﻿using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadFinal : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "GratefulDead";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Final Tier)");
            Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nSpecial: Spread Gas\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 90;
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
                .AddIngredient(ModContent.ItemType<GratefulDeadT3>())
                .AddIngredient(ItemID.ShroomiteBar, 14)
                .AddIngredient(ItemID.Ichor, 20)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GratefulDeadT3>())
                .AddIngredient(ItemID.ShroomiteBar, 14)
                .AddIngredient(ItemID.CursedFlame, 20)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
