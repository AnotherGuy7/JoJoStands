using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondFinal : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "CrazyDiamond";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Final Tier)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nSpecial: Switch to Restoration Mode\nSecond Special: Unleash a Blind Rage on your enemies!\nLeft-click in Restoration Mode to perform a restorative punch or cause temporary damage to public property!\nRight-Click in Restoration Mode to repair your damage to the world or to uncraft the item you hold!\nSecond Special in Restoration Mode: Choose any creature nearby and heal it completely!\nUsed in Stand Slot");
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
