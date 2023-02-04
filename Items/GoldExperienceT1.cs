using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceT1 : StandItemClass
    {
        public override int StandSpeed => 14;
        public override int StandType => 1;
        public override string StandProjectileName => "GoldExperience";
        public override int StandTier => 1;
        public override Color StandTierDisplayColor => Color.Orange;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GoldExperienceFinal"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Experience (Tier 1)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a frog! \nSpecial: Switches the abilities used for right-click!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 16;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
