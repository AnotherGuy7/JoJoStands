using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class KillerQueenT3 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 1;
        public override string StandProjectileName => "KillerQueen";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.LightPink;

        public override string Texture
        {
            get { return Mod.Name + "/Items/KillerQueenT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Queen (1st Bomb Tier 3)");
            Tooltip.SetDefault("Left-click to punch and right-click to trigger any block! \nRange: 14 blocks \nSpecial: Sheer Heart Attack!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 59;
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
                .AddIngredient(ModContent.ItemType<KillerQueenT2>())
                .AddIngredient(ItemID.HallowedBar, 6)
                .AddIngredient(ModContent.ItemType<Hand>(), 1)
                .AddIngredient(ItemID.SoulofMight, 8)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddIngredient(ModContent.ItemType<WillToEscape>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}