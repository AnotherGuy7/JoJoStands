using JoJoStands.Buffs.PetBuffs;
using JoJoStands.Projectiles.Pets.Part1;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.PetItems
{
    public class OrnateGlassofWine : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("A glass of wine that never spills.\nSummons a Mini Will A. Zeppeli");
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
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<ZeppeliPet>()] == 0)
            {
                player.AddBuff(ModContent.BuffType<ZeppeliPetBuff>(), 60);
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(0f, -6f), player.velocity, ModContent.ProjectileType<ZeppeliPet>(), 0, 0f, player.whoAmI);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Glass, 8)
                .AddIngredient(ItemID.BlueBerries, 3)
                .AddTile(TileID.Kegs)
                .Register();
        }
    }
}
