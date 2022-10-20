using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TuskAct2 : StandItemClass
    {
        public override int standSpeed => 35;
        public override int standType => 2;
        public override int standTier => 2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tusk (ACT 2)");
            Tooltip.SetDefault("Hold left-click to shoot and control a spinning nail and right-click to spin your nails in front of you to use as a melee weapon!\nSecond Special: Switch to previous acts!");
        }

        public override void SetDefaults()
        {
            Item.damage = 87;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.standType = 2;
            mPlayer.equippedTuskAct = standTier;
            mPlayer.tuskActNumber = standTier;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TuskAct1>())
                .AddIngredient(ItemID.Hellstone, 12)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}