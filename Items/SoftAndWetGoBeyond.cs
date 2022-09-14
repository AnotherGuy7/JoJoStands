using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond;
using JoJoStands.Tiles;
using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetGoBeyond : StandItemClass
    {
        public override int standSpeed => 9;
        public override int standType => 1;
        public override string standProjectileName => "SoftAndWet";
        public override int standTier => 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Go Beyond!)");
            Tooltip.SetDefault("Punch enemies at a fast rate and right-click to create a non-existent bubble!\nSpecial: Explosive Spin!");
        }
        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }
        public override void SetDefaults()
        {
            Item.damage = 118;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }


        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<SoftAndWetGoBeyondStand>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<SoftAndWetFinal>())
                .AddIngredient(ModContent.ItemType<KillerQueenFinal>())
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
