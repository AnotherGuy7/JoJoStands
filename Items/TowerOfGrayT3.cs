using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace JoJoStands.Items
{
    public class TowerOfGrayT3 : StandItemClass
    {
        public override int standSpeed => 10;
        public override int standType => 2;
        public override string standProjectileName => "TowerOfGray";
        public override int standTier => 3;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TowerOfGrayT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tower of Gray (Tier 3)");
            Tooltip.SetDefault("Pierce your enemies with a sharp stinger and tear through them with right-click! \nSpecial: Remote Control \nSecond Special: Pierce every enemy in the area with tongue-tearing stinger! \nPassive: Attack ignores 30 enemy defence \nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 36;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            player.GetModPlayer<MyPlayer>().towerOfGrayTier = standTier;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TowerOfGrayT2>())
                .AddIngredient(ItemID.HallowedBar, 12)
                .AddIngredient(ItemID.PixieDust, 20)
                .AddIngredient(ItemID.Stinger, 20)
                .AddIngredient(ModContent.ItemType<WillToDestroy>(), 4)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
