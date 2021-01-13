using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items.PetItems
{
    public class TatteredLetter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An old, tattered letter.\nSummons a Mini Dio Brando");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(silver: 50);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = ItemRarityID.Blue;
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("DioPet")] == 0)
            {
                player.AddBuff(mod.BuffType("DioPetBuff"), 60);
                Projectile.NewProjectile(player.Center + new Vector2(0f, -6f), player.velocity, mod.ProjectileType("DioPet"), 0, 0f, item.owner);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TatteredCloth, 3);
            recipe.AddIngredient(ItemID.Ruby);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
