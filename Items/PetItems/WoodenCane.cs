using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Items.PetItems
{
    public class WoodenCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An old, wooden cane that Was used by the coolest guy ever.\nSummons a Mini Robert E.O. Speedwagon");
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
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[mod.ProjectileType("SpeedWagonPet")] == 0)
            {
                player.AddBuff(mod.BuffType("SpeedWagonPetBuff"), 60);
                Projectile.NewProjectile(player.Center + new Vector2(0f, -6f), player.velocity, mod.ProjectileType("SpeedWagonPet"), 0, 0f, item.owner);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 12);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
