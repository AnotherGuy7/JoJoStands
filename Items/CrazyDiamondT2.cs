using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT2 : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandProjectileName => "CrazyDiamond";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => CrazyDiamondFinal.CrazyDiamondTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crazy Diamond (Tier 2)");
            /* Tooltip.SetDefault(
                "Left-click to punch enemies at a really fast rate and right-click to flick a bullet!" +
                "\nSpecial: Switch to Restoration Mode" +
                "\nLeft-click in Restoration Mode to perform a restorative barrage" +
                "\nRight-click in restoration mode to restore the states of all impacted objects." +
                "\nPunching tiles while in Resotration Mode breaks the tiles." +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 52;
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
                .AddIngredient(ModContent.ItemType<CrazyDiamondT1>())
                .AddIngredient(ItemID.HellstoneBar, 18)
                .AddIngredient(ItemID.Diamond, 2)
                .AddIngredient(ItemID.LifeCrystal, 1)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
