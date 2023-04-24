using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeFinal : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 2;
        public override string StandProjectileName => "Whitesnake";
        public override int StandTier => 4;
        public static readonly Color WhitesnakeTierColor = new Color(59, 94, 124);
        public override Color StandTierDisplayColor => WhitesnakeTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Whitesnake (Final Tier)");
            /* Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSpecial: Take any enemy's discs!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click shoot enemies with a pistol!\nRemote Mode Special: Create an aura that puts enemies to sleep!\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 88;
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
                .AddIngredient(ModContent.ItemType<WhitesnakeT3>())
                .AddIngredient(ItemID.Ectoplasm, 7)
                .AddRecipeGroup("JoJoStandsCursedIchor", 5)
                .AddIngredient(ItemID.VialofVenom)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
