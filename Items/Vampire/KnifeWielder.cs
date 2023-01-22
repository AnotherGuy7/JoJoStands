using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class KnifeWielder : VampireDamageClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Knife Wielder Abilities (Zombie)");
            Tooltip.SetDefault("Left-click to lunge at enemies with knives and right-click to bury knives into you.\nSpecial: Shoot all of the knives inside of you outward!\nNote: 16 or more knives are required for Knife Amalgamation. Knife Amalgamation is required to use the Special.");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 19;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 9f;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
        }

        private int useCool = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != Main.myPlayer || !vPlayer.zombie || (mPlayer.standOut && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual))
                return;

            if (useCool > 0)
                useCool--;

            if (player.ownedProjectileCounts[ModContent.ProjectileType<KnifeSlashes>()] > 0)
                return;

            if (Main.mouseLeft && useCool <= 0)
            {
                useCool += 4;
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<KnifeSlashes>(), Item.damage, Item.knockBack, player.whoAmI);
            }

            if (Main.mouseRight && useCool <= 0 && !player.HasBuff(ModContent.BuffType<KnifeAmalgamation>()) && player.CountItem(ModContent.ItemType<Knife>()) >= 16)
            {
                useCool += 30;
                player.AddBuff(ModContent.BuffType<KnifeAmalgamation>(), 2 * 60 * 60);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " has tried to stick too many knives into themselves!"), 24, -player.direction);
                for (int i = 0; i < 16; i++)
                    player.ConsumeItem(ModContent.ItemType<Knife>());
            }

            bool specialJustPressed = false;
            if (!Main.dedServ)
                specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

            if (specialJustPressed && useCool <= 0 && player.HasBuff(ModContent.BuffType<KnifeAmalgamation>()))
            {
                int knivesToThrow = 16;
                for (int i = 0; i < knivesToThrow; i++)
                {
                    float angle = MathHelper.ToRadians((360f / knivesToThrow) * i);
                    Vector2 knifePos = player.Center + (angle.ToRotationVector2() * player.height);
                    Vector2 knifeVel = player.Center - knifePos;
                    knifeVel.Normalize();
                    knifeVel *= 14f;
                    Projectile.NewProjectile(player.GetSource_FromThis(), knifePos, knifeVel, ModContent.ProjectileType<Projectiles.KnifeProjectile>(), 47, 5f, player.whoAmI);
                }
                player.buffTime[player.FindBuffIndex(ModContent.BuffType<KnifeAmalgamation>())] -= 45 * 60;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddCondition(NetworkText.FromLiteral("ZombieRequirement"), r => !Main.gameMenu && Main.LocalPlayer.GetModPlayer<VampirePlayer>().zombie && Main.LocalPlayer.GetModPlayer<VampirePlayer>().HasSkill(Main.LocalPlayer, VampirePlayer.KnifeWielder))
                .Register();
        }
    }
}
