using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class CrazyDiamondT3 : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "CrazyDiamond";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/CrazyDiamondT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crazy Diamond (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to flick a bullet! \nSpecial: Switch to Restoration Mode \nSecond Special: Unleash Blind Rage on your opponent and seal him in stone! \nLeft-click in Restoration Mode to do healing punch and cause temporary damage to public property!\nRight-Click in Restoration Mode to repair your damage to the world. \nSecond Special in Restoration Mode: Choose any creature nearby and heal it completely! \nUsed in Stand Slot");
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
