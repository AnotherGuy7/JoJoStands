using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.SexPistols;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SexPistolsT2 : StandItemClass
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/SexPistolsT1"; }
        }

        public override int StandTier => 2;
        public override Color StandTierDisplayColor => SexPistolsFinal.SexPistolsTierColor;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sex Pistols (Tier 2)");
            Tooltip.SetDefault("Use a gun and have Sex Pistols kick the bullet!\nIncreases bullet damages by 10% and adds one penetration point.\nSpecial: Configure all Sex Pistols's placement!\nAuto Mode: Bullets that go near enemies will automatically be re-adjusted toward the enemy!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = 5;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.sexPistolsTier = StandTier;
            mPlayer.standType = 2;
            mPlayer.poseSoundName = "SexPistolsIsDesignedToKill";
            for (int i = 0; i < 6; i++)
            {
                Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<SexPistolsStand>(), 0, 0f, Main.myPlayer, i + 1);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SexPistolsT1>())
                .AddRecipeGroup("JoJoStandsIron-TierBar", 20)
                .AddIngredient(ItemID.FallenStar, 4)
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddIngredient(ModContent.ItemType<WillToProtect>())
                .AddRecipeGroup("JoJoStandsSilverBullet", 50)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}