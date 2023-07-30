using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StrayCat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Stray Cat");
            // Tooltip.SetDefault("An odd plant that is somehow a cat. It can fire bubbles invisible to the eye and is capable of causing meowsive damage.");
        }

        public override void SetDefaults()
        {
            Item.damage = 104;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
            /*Item.shoot = ModContent.ProjectileType<MatureStrayCatMinion>();
            Item.buffType = ModContent.BuffType<StrayCatBuff>();
            Item.buffTime = 120;*/
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (!Collision.SolidCollision(Main.MouseWorld, 16, 16))
                {
                    if (player.maxMinions - player.slotsMinions <= 0)
                    {
                        for (int p = 0; p < Main.maxProjectiles; p++)
                        {
                            Projectile projectile = Main.projectile[p];
                            if (projectile.active && projectile.type == ModContent.ProjectileType<MatureStrayCatMinion>() && projectile.owner == player.whoAmI)
                            {
                                projectile.Kill();
                                break;
                            }
                        }
                    }
                    player.AddBuff(ModContent.BuffType<StrayCatBuff>(), 2);
                    int strayCatIndex = Projectile.NewProjectile(player.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MatureStrayCatMinion>(), 0, 0f, player.whoAmI);
                    Main.projectile[strayCatIndex].spriteDirection = Main.projectile[strayCatIndex].direction = Main.MouseWorld.X > player.Center.X ? 1 : -1;
                    Main.projectile[strayCatIndex].netUpdate = true;
                }
            }
            return true;
        }
    }
}
