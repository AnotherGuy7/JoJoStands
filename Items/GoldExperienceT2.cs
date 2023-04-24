using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceT2 : StandItemClass
    {
        public override int StandSpeed => 13;
        public override int StandType => 1;
        public override string StandProjectileName => "GoldExperience";
        public override int StandTier => 2;
        public override Color StandTierDisplayColor => Color.Orange;

        public override string Texture
        {
            get { return Mod.Name + "/Items/GoldExperienceFinal"; }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Experience (Tier 2)");
            /* Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities!" +
                "\nSpecial: Opens/Hides the Ability Wheel" +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 41;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            GoldExperienceAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 2);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldExperienceT1>())
                .AddRecipeGroup("JoJoStandsGold-TierBar", 12)
                .AddIngredient(ItemID.Acorn, 20)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ModContent.ItemType<WillToControl>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
