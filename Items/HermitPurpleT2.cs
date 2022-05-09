using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class HermitPurpleT2 : StandItemClass
    {
        public override int standSpeed => 35;
        public override int standType => 1;
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/HermitPurpleT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermit Purple (Tier 2)");
            Tooltip.SetDefault("Left-click to use Hermit Purple as a whip and right-click to grab enemies and slowly crush them!\nPassive: Enemies are hurt when they hurt you and get inflicted Sunburn!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 81;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standType = 1;
            mPlayer.hermitPurpleTier = standTier;
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<HermitPurpleT1>())
                .AddIngredient(ItemID.HellstoneBar, 7)
                .AddIngredient(ItemID.Amethyst, 3)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 2)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
