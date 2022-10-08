using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class EchoesACT3 : StandItemClass
    {
        public override int standSpeed => 8;
        public override int standType => 1;
        public override string standProjectileName => "Echoes";
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Echoes (ACT 3)");
            Tooltip.SetDefault("Left-click to punch enemies at a really fast rate! \nRight-click: Three Freeze! Pin any enemy to the ground! Pinned enemy takes damage! \nSpecial: Three Freeze Barrage! Normal attack inflict Three Freeze effect for a short time! \nSecond Special: Switch to previous acts!");
        }

        public override void SetDefaults()
        {
            Item.damage = 68;
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
                .AddIngredient(ModContent.ItemType<EchoesACT2>())
                .AddIngredient(ItemID.Ectoplasm, 8)
                .AddIngredient(ItemID.EncumberingStone)
                .AddIngredient(ItemID.Emerald, 7)
                .AddIngredient(ModContent.ItemType<CaringLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
