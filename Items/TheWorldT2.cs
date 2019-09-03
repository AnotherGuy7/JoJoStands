using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class TheWorldT2 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate! \nSpecial: Stop time for 2 seconds! \nSpecial: Stop time for 2 seconds!");
        }

        public override void SetDefaults()
        {
            item.damage = 73;
            item.width = 100;
            item.height = 8;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 6;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TheWorldFist");
            item.shootSpeed = 50f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            float numberProjectiles = 3 + Main.rand.Next(5);
            float rotation = MathHelper.ToRadians(45);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            if (JoJoStands.ItemHotKey.JustPressed && !player.HasBuff(mod.BuffType("TimeCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_start"));
                player.AddBuff(mod.BuffType("TheWorldBuff"), 120, true);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.GoldWatch);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.PlatinumWatch);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
