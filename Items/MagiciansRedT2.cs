using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedT2 : StandItemClass
    {
        public override int StandSpeed => 18;
        public override int StandType => 2;
        public override string StandProjectileName => "MagiciansRed";
        public override int StandTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Tier 2)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to bind an enemy!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 48;
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
                .AddIngredient(ModContent.ItemType<MagiciansRedT1>())
                .AddIngredient(ItemID.HellstoneBar, 8)
                .AddIngredient(ItemID.Fireblossom, 3)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
