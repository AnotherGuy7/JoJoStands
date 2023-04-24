using JoJoStands.Buffs.PetBuffs;
using JoJoStands.Projectiles.Pets.Part1;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.PetItems
{
    public class WoodenCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("An old, wooden cane that Was used by the coolest guy ever.\nSummons a Mini Robert E.O. Speedwagon");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(silver: 50);
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<SpeedWagonPet>()] == 0)
            {
                player.AddBuff(ModContent.BuffType<SpeedWagonPetBuff>(), 60);
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(0f, -6f), player.velocity, ModContent.ProjectileType<SpeedWagonPet>(), 0, 0f, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 12)
                .AddIngredient(ItemID.Silk, 3)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
