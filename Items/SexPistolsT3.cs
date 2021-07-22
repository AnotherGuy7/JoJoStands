using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items
{
    public class SexPistolsT3 : StandItemClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/SexPistolsT1"; }
        }

        public override int standTier => 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sex Pistols (Tier 3)");
            Tooltip.SetDefault("Use a gun and have Sex Pistols kick the bullet!\nIncreases bullet damages by 15% and adds two penetration points.\nSpecial: Configure all Sex Pistols's placement!\nSecond Special: Bullet Kick Frenzy\nUsed in Stand Slot");
        }

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
            recipe.AddIngredient(mod.ItemType("SexPistolsT2"));
            recipe.AddIngredient(ItemID.PalladiumOre, 20);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(ItemID.MeteorShot, 160);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SexPistolsT2"));
            recipe.AddIngredient(ItemID.CobaltOre, 20);
            recipe.AddIngredient(ItemID.Topaz, 2);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 2);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 2);
            recipe.AddIngredient(ItemID.MeteorShot, 160);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}