using JoJoStands.Buffs.PetBuffs;
using JoJoStands.Projectiles.Pets.Part1;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(silver: 50);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Blue;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<DioPet>()] == 0)
            {
                player.AddBuff(ModContent.BuffType<DioPetBuff>(), 60);
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center + new Vector2(0f, -6f), player.velocity, ModContent.ProjectileType<DioPet>(), 0, 0f, player.whoAmI);
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<DioPetBuff>(), 60);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TatteredCloth, 3)
                .AddIngredient(ItemID.Ruby)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
