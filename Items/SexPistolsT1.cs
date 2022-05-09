using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.SexPistols;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SexPistolsT1 : StandItemClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sex Pistols (Tier 1)");
            Tooltip.SetDefault("Use a gun and have Sex Pistols kick the bullet!\nIncreases bullet damages by 5% and adds one penetration point.\nSpecial: Configure all Sex Pistols's placement!\nUsed in Stand Slot");
        }

        public override int standTier => 1;
        //In Manual Mode: You set 6 points that Sex Pistols will go to (in relation to the player) and whenever a bullet gets in that Sex Pistols's range, it gets kicked and redirected toward the nearest enemy. 
        //In Auto Mode: The Sex Pistols automatically kick the bullet whenever a new bullet is created.

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

            mPlayer.sexPistolsTier = standTier;
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
                .AddIngredient(ModContent.ItemType<StandArrow>())
                .AddIngredient(ModContent.ItemType<WillToFight>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}