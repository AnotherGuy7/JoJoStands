using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class TowerOfGrayT3 : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 2;
        public override string StandIdentifierName => "TowerOfGray";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => Color.RosyBrown;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TowerOfGrayT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tower of Gray (Tier 3)");
            /* Tooltip.SetDefault("Pierce your enemies with a sharp stinger and tear through them with right-click!" +
                "\nSpecial: Remote Control" +
                "\nSecond Special: Pierce every enemy in the area with tongue-tearing stinger!" +
                "\nPassive: Attack ignores 30 enemy defense" +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 36;
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
                .AddIngredient(ModContent.ItemType<TowerOfGrayT2>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.PixieDust, 20)
                .AddIngredient(ItemID.Stinger, 10)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 1)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
