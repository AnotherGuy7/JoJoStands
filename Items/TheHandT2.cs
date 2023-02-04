using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheHandT2 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandProjectileName => "TheHand";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => TheHandFinal.TheHandTierColor;

        public override string Texture
        {
            get { return Mod.Name + "/Items/TheHandT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Hand (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to scrape away space!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
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
                .AddIngredient(ModContent.ItemType<TheHandT1>())
                .AddIngredient(ItemID.HellstoneBar, 13)
                .AddIngredient(ModContent.ItemType<WillToDestroy>())
                .AddIngredient(ModContent.ItemType<WillToChange>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
