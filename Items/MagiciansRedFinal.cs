using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedFinal : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 2;
        public override string standProjectileName => "MagiciansRed";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/MagiciansRedT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magicians Red (Final Tier)");
            Tooltip.SetDefault("Shoot flaming ankhs at the enemies and right-click to grab an enemy!\nSpecial: Crossfire Hurricane!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 95;
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
                .AddIngredient(ModContent.ItemType<MagiciansRedT3>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ItemID.FireFeather)
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
