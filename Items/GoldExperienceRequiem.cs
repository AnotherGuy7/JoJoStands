using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceRequiem : StandItemClass
    {
        public override int StandSpeed => 11;
        public override int StandType => 1;
        public override string StandProjectileName => "GoldExperienceRequiem";
        public override int StandTier => 5;
        public override Color StandTierDisplayColor => Color.Orange;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gold Experience (Requiem)");
            /* Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use the chosen abilities!" +
                "\nSpecial: Use Back to Zero to nullify the actions of all enemies who touch you!" +
                "\nSecond Special: Opens/Hides the Ability Wheel" +
                "\nUsed in Stand Slot"); */
        }

        public override void SetDefaults()
        {
            Item.damage = 138;
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<GoldExperienceRequiemStand>(), 0, 0f, Main.myPlayer);
            GoldExperienceRequiemAbilityWheel.OpenAbilityWheel(player.GetModPlayer<MyPlayer>(), 4);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GoldExperienceFinal>())
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<DeterminedLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
