using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GratefulDeadT2 : StandItemClass
    {
        public override int standSpeed => 14;
        public override int standType => 1;
        public override string standProjectileName => "GratefulDead";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GratefulDeadT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Grateful Dead (Tier 2)");
            Tooltip.SetDefault("Punch enemies to make them age and right-click to grab them!\nMore effective on hot biomes.\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 41;
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
                .AddIngredient(ModContent.ItemType<GratefulDeadT1>())
                .AddIngredient(ItemID.Bone, 15)
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
