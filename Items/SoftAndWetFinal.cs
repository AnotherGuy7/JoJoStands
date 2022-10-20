using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetFinal : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "SoftAndWet";
        public override int standTier => 4;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a Plunder Bubble!\nPassive: Bubble Generation\nSpecial: Bubble Bombs!\nSecond Special: Bubble Barrier!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 79;
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
                .AddIngredient(ModContent.ItemType<SoftAndWetT3>())
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 2)
                .AddIngredient(ItemID.Dynamite, 5)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
