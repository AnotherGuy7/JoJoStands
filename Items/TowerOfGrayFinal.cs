using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class TowerOfGrayFinal : StandItemClass
    {
        public override int StandSpeed => 9;
        public override int StandType => 2;
        public override string StandProjectileName => "TowerOfGray";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => Color.RosyBrown;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TowerOfGrayT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tower of Gray (Final Tier)");
            Tooltip.SetDefault("Pierce your enemies with a sharp stinger and tear right through them with right-click!" +
                "\nSpecial: Remote Control" +
                "\nSecond Special: Pierce every enemy in the area with tongue-tearing stinger!" +
                "\nPassive: Attack ignores 40 enemy defence" +
                "\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 54;
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
                .AddIngredient(ModContent.ItemType<TowerOfGrayT3>())
                .AddIngredient(ItemID.BeetleHusk, 16)
                .AddIngredient(ItemID.HerculesBeetle, 1)
                .AddIngredient(ItemID.Stinger, 15)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
