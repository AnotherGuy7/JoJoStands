using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = ItemRarityID.LightPurple;
        }

        public override bool ManualStandSpawning(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.sexPistolsTier = standTier;
            mPlayer.poseSoundName = "SexPistolsIsDesignedToKill";
            for (int i = 0; i < 6; i++)
            {
                Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("SexPistolsStand"), 0, 0f, Main.myPlayer, i + 1);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StandArrow"));
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}