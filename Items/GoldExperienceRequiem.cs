using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.GoldExperienceRequiem;
using JoJoStands.Tiles;
using JoJoStands.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class GoldExperienceRequiem : StandItemClass
    {
        public override int standSpeed => 11;
        public override int standType => 1;
        public override string standProjectileName => "GoldExperience";
        public override int standTier => 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gold Experience (Requiem)");
            Tooltip.SetDefault("Punch enemies at a really fast rate and right-click to use the chosen abilities!\nSpecial: Use Back to Zero to nullify the actions of all enemies who touch you!\nSecond Special: Switches the abilities used for right-click!\nUsed in Stand Slot");
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
