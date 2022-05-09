using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeT3 : StandItemClass
    {
        public override int standSpeed => 12;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Tier 3)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSpecial: Take any enemy's discs!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click shoot enemies with a pistol!\nRemote Mode Special: Create an aura that puts enemies to sleep!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 69;
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
                .AddIngredient(ModContent.ItemType<WhitesnakeT2>())
                .AddIngredient(ItemID.SoulofNight, 14)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.Diamond, 2)
                .AddIngredient(ItemID.CursedFlame, 3)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WhitesnakeT2>())
                .AddIngredient(ItemID.SoulofNight, 14)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.Diamond, 2)
                .AddIngredient(ItemID.Ichor, 3)
                .AddIngredient(ModContent.ItemType<WillToControl>(), 2)
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
