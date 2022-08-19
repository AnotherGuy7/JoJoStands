using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT2 : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "CrazyDiamond";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to flick a bullet! \nSpecial: Switch to Restoration Mode \nLeft-click in Restoration Mode to do healing punch and cause temporary damage to public property!\nRight-Click in Restoration Mode to repair your damage to the world. \nUsed in Stand Slot");
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
