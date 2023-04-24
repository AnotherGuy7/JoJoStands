using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class TowerOfGrayT2 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 2;
        public override string StandProjectileName => "TowerOfGray";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.RosyBrown;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TowerOfGrayT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Tower of Gray (Tier 2)");
            /* Tooltip.SetDefault("Pierce your enemies with a sharp stinger and tear through them with right-click!" +
                "\nSpecial: Remote Control" +
                "\nPassive: Attack ignores 20 enemy defence" +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
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
                .AddIngredient(ModContent.ItemType<TowerOfGrayT1>())
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.Bone, 14)
                .AddIngredient(ItemID.Stinger, 5)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
