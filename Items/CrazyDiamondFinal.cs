using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondFinal : StandItemClass
    {
        public override int StandSpeed => 9;
        public override int StandType => 1;
        public override string StandProjectileName => "CrazyDiamond";
        public override int StandTier => 4;
        public static readonly Color CrazyDiamondTierColor = new Color(163, 211, 231);
        public override Color StandTierDisplayColor => CrazyDiamondTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Final Tier)");
            Tooltip.SetDefault(
                "Left-click to punch enemies at a really fast rate and right-click to flick a bullet!" +
                "\nSpecial: Switch to Restoration Mode" +
                "\nSecond Special: Enter an unstoppable rage!" +
                "\nLeft-click in Restoration Mode to perform a restorative barrage" +
                "\nRight-click on an NPC or player while in Restoration Mode to restore them to their natural states." +
                "\nRight-clicking elsewhere will restore the states of all other impacted objects." +
                "\nPunching tiles while in Resotration Mode breaks the tiles." +
                "\nWhile in a Rampage, restorations will become improper, turning restored enemies to stone." +
                "\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 105;
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
                .AddIngredient(ModContent.ItemType<CrazyDiamondT3>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.LargeDiamond, 1)
                .AddIngredient(ItemID.LifeCrystal, 7)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
