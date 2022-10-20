using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.KillerQueenBTD;
using JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetT2 : StandItemClass
    {
        public override int standSpeed => 13;
        public override int standType => 1;
        public override string standProjectileName => "SoftAndWet";
        public override int standTier => 2;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a Plunder Bubble!\nSecond Special: Bubble Barrier!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 38;
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
                .AddIngredient(ModContent.ItemType<SoftAndWetT1>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddIngredient(ItemID.BubbleMachine)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
