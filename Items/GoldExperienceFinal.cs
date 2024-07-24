using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceFinal : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandIdentifierName => "GoldExperience";
        public override int StandTier => 4;
        public override Color StandTierDisplayColor => Color.Orange;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Experience (Final Tier)");
            /* Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use abilities!" +
                "\nSpecial: Opens/Hides the Ability Wheel" +
                "\nPassive: Barraging enemies may inflict them with Life Punch" +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 98;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            GoldExperienceAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 4);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldExperienceT3>())
                .AddIngredient(ItemID.ChlorophyteBar, 6)
                .AddIngredient(ItemID.Ectoplasm, 3)
                .AddIngredient(ItemID.LifeFruit, 2)
                .AddIngredient(ItemID.RedHusk, 2)
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
