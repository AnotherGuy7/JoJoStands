using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class WhitesnakeFinal : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 2;
        public override string standProjectileName => "Whitesnake";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/WhitesnakeT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Whitesnake (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right click to throw some acid!\nSpecial: Take any enemy's discs!\nSecond Special: Remote Control" +
                "\nWhile in Remote Mode: Left-click to move and right-click shoot enemies with a pistol!\nRemote Mode Special: Create an aura that puts enemies to sleep!\nUsed in Stand Slot");
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
                .AddIngredient(ItemID.CursedFlame, 5)
                .AddIngredient(ItemID.VialofVenom)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();

            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WhitesnakeT3>())
                .AddIngredient(ItemID.Ectoplasm, 7)
                .AddIngredient(ItemID.Ichor, 5)
                .AddIngredient(ItemID.VialofVenom)
                .AddIngredient(ModContent.ItemType<TaintedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
