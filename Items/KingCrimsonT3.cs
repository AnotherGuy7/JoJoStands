using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KingCrimsonT3 : StandItemClass
    {
        public override int StandSpeed => 22;
        public override int StandType => 1;
        public override string StandProjectileName => "KingCrimson";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => KingCrimsonFinal.KingCrimsonTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/KingCrimsonT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("King Crimson (Tier 3)");
            Tooltip.SetDefault("Donut enemies with a powerful punch and hold right-click to block off enemies and reposition!\nSpecial: Skip 5 seconds of time!\nSecond Special: Use Epitaph for 4 seconds!\nPassive: Consecutive Donuts deal greater damage.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 124;
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
                .AddIngredient(ModContent.ItemType<KingCrimsonT2>())
                .AddIngredient(ItemID.SoulofFright, 4)
                .AddIngredient(ItemID.SoulofSight, 6)
                .AddIngredient(ItemID.HallowedBar, 10)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
