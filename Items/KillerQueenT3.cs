using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class KillerQueenT3 : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/KillerQueenT1"; }
        }

        public float npcDistance = 0f;
        public float mouseDistance = 0f;
        public Vector2 savedPosition = Vector2.Zero;
        public bool touchedTile = false;
        public int timeAfterTouch = 0;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Killer Queen (1st Bomb Tier 3)");
			Tooltip.SetDefault("Shoot items that explode and right-click to trigger any block! \nRange: 14 blocks \nSpecial: Sheer Heart Attack!");
		}

		public override void SetDefaults()
		{
            item.damage = 94;//Around Mechs
            item.melee = true;      //turns out this is used for types, like if you have an armor that does +%5 melee damage, it would affect this, but not an armor with +%5 ranged damage
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = Item.buyPrice(0, 7, 50, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.crit = 35;
        }

        public override void HoldItem(Player player)
        {
            timeAfterTouch--;
            if (timeAfterTouch <= 0)
            {
                timeAfterTouch = 0;
            }
            if (!touchedTile)
            {
                mouseDistance = Vector2.Distance(Main.MouseWorld, player.Center);
            }
            if (touchedTile)
            {
                for (int i = 0; i < 200; i++)
                {
                    npcDistance = Vector2.Distance(Main.npc[i].Center, savedPosition);
                    if (npcDistance < 50f && touchedTile)       //or youd need to go from its center, add half its width to the direction its facing, and then add 16 (also with direction) -- Direwolf
                    {
                        int projectile = Projectile.NewProjectile(savedPosition, Vector2.Zero, ProjectileID.GrenadeIII, 13, 50f, Main.myPlayer);
                        Main.projectile[projectile].friendly = true;
                        Main.projectile[projectile].timeLeft = 2;
                        Main.projectile[projectile].netUpdate = true;
                        touchedTile = false;
                        savedPosition = Vector2.Zero;
                    }
                }
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2 && Collision.SolidCollision(Main.MouseWorld, 1, 1) && mouseDistance < 232f && timeAfterTouch <= 0 && !touchedTile)
            {
                timeAfterTouch = 45;
                savedPosition = Main.MouseWorld;
                touchedTile = true;
            }
            if (player.altFunctionUse == 2 && timeAfterTouch <= 0 && touchedTile)
            {
                int projectile = Projectile.NewProjectile(savedPosition, Vector2.Zero, ProjectileID.GrenadeIII, 87, 50f, Main.myPlayer);
                Main.projectile[projectile].friendly = true;
                Main.projectile[projectile].timeLeft = 2;
                Main.projectile[projectile].netUpdate = true;
                touchedTile = false;
                savedPosition = Vector2.Zero;
            }

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

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = 5;
                item.useTurn = true;
                item.useAnimation = 45;
                item.useTime = 45;
                item.width = 12;
                item.height = 12;
                item.shoot = mod.ProjectileType("KQBomb2Activator");
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
            }
            else
            {
                item.damage = 94;
                item.width = 32;
                item.height = 32;
                item.useTime = 11;
                item.useAnimation = 11;
                item.useStyle = 5;
                item.knockBack = 4;
                item.autoReuse = true;
                item.useTurn = true;
                item.shoot = mod.ProjectileType("KQBombFist");
                item.shootSpeed = 50f;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("KillerQueenT2"));
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddIngredient(mod.ItemType("Hand"), 1);
            recipe.AddIngredient(ItemID.SoulofMight, 8);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}