using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class MagiciansRedFinal : StandItemClass
    {
        public override int StandSpeed => 14;
        public override int StandType => 2;
        public override string StandProjectileName => "MagiciansRed";
        public override int StandTier => 4;
        public static readonly Color MagiciansRedTierColor = new Color(255, 207, 40);
        public override Color StandTierDisplayColor => MagiciansRedTierColor;

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
