using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AerosmithT2 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 2;
        public override string standProjectileName => "Aerosmith";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/AerosmithT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aerosmith (Tier 2)");
            Tooltip.SetDefault("Left-click to move and right-click to shoot bullets at the enemies!\nSpecial: Drop a bomb on enemies!\nThe farther the stand is from you, the less damage it does.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 42;
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
                .AddIngredient(ModContent.ItemType<AerosmithT1>())
                .AddIngredient(ItemID.Hellstone, 18)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
