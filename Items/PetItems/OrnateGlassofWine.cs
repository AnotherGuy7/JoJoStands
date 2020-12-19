using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items.PetItems
{
    public class OrnateGlassofWine : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A glass of wine that never spills.\nSummons a Mini Will A. Zepppeli");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(silver: 50);
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.rare = ItemRarityID.Blue;
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("ZeppeliPet")] == 0)
            {
                player.AddBuff(mod.BuffType("ZeppeliPetBuff"), 60);
                Projectile.NewProjectile(player.Center + new Vector2(0f, -6f), player.velocity, mod.ProjectileType("ZeppeliPet"), 0, 0f, item.owner);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 8);
            recipe.AddIngredient(ItemID.BlueBerries, 3);
            recipe.AddTile(TileID.Kegs);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
