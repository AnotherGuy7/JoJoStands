using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace JoJoStands.Items
{
    public class SoftAndWetFinal : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "SoftAndWet";
        public override int standTier => 4;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Final Tier)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a Plunder Bubble!\n Passive: Bubble Generation\nSpecial: Bubble Bombs!\nSecond Special: Bubble Barrier!\nUsed in Stand Slot");
        }
        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }
        public override void SetDefaults()
        {
            Item.damage = 96;
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
                .AddIngredient(ModContent.ItemType<WillToFight>(), 3)
                .AddIngredient(ModContent.ItemType<WillToChange>(), 3)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddIngredient(ItemID.Dynamite, 5)
                .AddIngredient(ItemID.ChlorophyteBar, 10)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
