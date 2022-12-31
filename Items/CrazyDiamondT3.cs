using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT3 : StandItemClass
    {
        public override int StandSpeed => 10;
        public override int StandType => 1;
        public override string StandProjectileName => "CrazyDiamond";
        public override int StandTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Tier 3)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate and right-click to flick a bullet!\nSpecial: Switch to Restoration Mode\nSecond Special: Unleash a Blind Rage on your enemies!\nLeft-click in Restoration Mode to perform a restorative punch or cause temporary damage to public property!\nRight-Click in Restoration Mode to repair your damage to the world or to uncraft the item you hold! \nSecond Special in Restoration Mode: Choose any creature nearby and heal it completely!\nUsed in Stand Slot");
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
