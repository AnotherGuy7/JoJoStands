using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SilverChariotT3 : StandItemClass
    {
        public override int standSpeed => 6;
        public override int standType => 1;
        public override string standProjectileName => "SilverChariot";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SilverChariotT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver Chariot (Tier 3)");
            Tooltip.SetDefault("Left-click to stab enemies and right-click to parry enemies and projectiles away!\nSpecial: Take SC's armor off! (Reduces player defense by 40%)\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 51;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverChariotT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.SilverBar, 12)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverChariotT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.TungstenBar, 12)
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverChariotT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.SilverBar, 12)
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SilverChariotT2>())
                .AddIngredient(ItemID.HallowedBar, 14)
                .AddIngredient(ItemID.TungstenBar, 12)
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
