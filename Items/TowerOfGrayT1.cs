using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class TowerOfGrayT1 : StandItemClass
    {
        public override int StandSpeed => 12;
        public override int StandType => 2;
        public override string StandProjectileName => "TowerOfGray";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.RosyBrown;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tower of Gray (Tier 1)");
            Tooltip.SetDefault("Pierce your enemies with a sharp stinger! \nSpecial: Remote Control \nPassive: Attack ignores 10 enemy defence \nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().towerOfGrayTier = StandTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
