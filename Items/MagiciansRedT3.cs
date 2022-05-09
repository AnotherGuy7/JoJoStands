using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedT3 : StandItemClass
    {
        public override int standSpeed => 16;
        public override int standType => 2;
        public override string standProjectileName => "MagiciansRed";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Tier 3)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 74;
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
                .AddIngredient(ModContent.ItemType<MagiciansRedT2>())
                .AddIngredient(ModContent.ItemType<WillToEscape>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddIngredient(ItemID.LivingFireBlock, 32)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
