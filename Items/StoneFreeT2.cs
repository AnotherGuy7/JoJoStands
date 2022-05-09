using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StoneFreeT2 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 1;
        public override string standProjectileName => "StoneFree";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/StoneFreeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Free (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click two tiles to create a string trap!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 39;
            Item.width = 50;
            Item.height = 50;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ItemID.HellstoneBar, 16)
                .AddIngredient(ItemID.Silk, 8)
                .AddIngredient(ItemID.Spike, 16)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>())
                .AddTile(ModContent.TileType<RemixTableTile>())
        }
    }
}
