using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT3 : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandIdentifierName => "CrazyDiamond";
        public override int StandTier => 3;
        public override Color StandTierDisplayColor => CrazyDiamondFinal.CrazyDiamondTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crazy Diamond (Tier 3)");
            /* Tooltip.SetDefault(
                "Left-click to punch enemies at a really fast rate and right-click to flick a bullet!" +
                "\nSpecial: Switch to Restoration Mode" +
                "\nSecond Special: Enter an unstoppable rage!" +
                "\nLeft-click in Restoration Mode to perform a restorative barrage" +
                "\nRight-click on an NPC or player while in Restoration Mode to restore them to their natural states." +
                "\nRight-clicking elsewhere will restore the states of all other impacted objects." +
                "\nPunching tiles while in Resotration Mode breaks the tiles." +
                "\nWhile in a Rampage, restorations will become improper, turning restored enemies to stone." +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 84;
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
                .AddIngredient(ModContent.ItemType<CrazyDiamondT2>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.Diamond, 4)
                .AddIngredient(ItemID.LifeCrystal, 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 4)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
