using JoJoStands.Projectiles;
using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class VampireAbilities : VampireDamageClass
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vampire Abilities");
            Tooltip.SetDefault("Left-click to lunge and hold right-click to grab an enemy and suck their blood!\nSpecial: Switch the abilities used for right-click!");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 51;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 12f;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
        }

        public const int GrabAndSuck = 0;
        public const int SpacerRipperStingyEyes = 1;
        public const int ZombieMinionSummoning = 2;

        private int useCool = 0;
        private int lungeChargeTimer = 0;
        private bool enemyBeingGrabbed = false;
        private int heldEnemyIndex = -1;
        private int heldEnemyTimer = 0;
        private int eyeLaserChargeUpTimer = 0;
        private int zombieSummonTimer = 0;

        private int abilityNumber = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != Main.myPlayer || !vPlayer.vampire || (mPlayer.standOut && !mPlayer.standAutoMode))
                return;

            if (useCool > 0)
                useCool--;

            bool specialPressed = false;
            if (!Main.dedServ)
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;

            if (specialPressed && player.whoAmI == Main.myPlayer)
            {
                abilityNumber++;
                if (abilityNumber >= 3)
                    abilityNumber = 0;

                if (abilityNumber == GrabAndSuck)
                    Main.NewText("Ability: Blood Absorbtion");
                if (abilityNumber == SpacerRipperStingyEyes)
                    Main.NewText("Ability: Space Ripper Stingy Eyes");
                if (abilityNumber == ZombieMinionSummoning)
                    Main.NewText("Ability: Zombie Minion Summoning");
            }

            vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
                lungeChargeTimer++;
                if (lungeChargeTimer < 180)
                    lungeChargeTimer = 180;
            }
            if (!Main.mouseLeft && lungeChargeTimer > 0 && useCool <= 0)
            {
                useCool += Item.useTime + (20 * (lungeChargeTimer / 30));
                int multiplier = lungeChargeTimer / 60;
                if (multiplier == 0)
                    multiplier = 1;
                if (Main.MouseWorld.X - player.position.X >= 0)
                    player.direction = 1;
                else
                    player.direction = -1;

                player.immune = true;
                player.immuneTime = 20;
                Vector2 launchVector = Main.MouseWorld - player.position;
                launchVector.Normalize();
                launchVector *= multiplier * 6;
                player.velocity += launchVector;
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<VampiricSlash>(), Item.damage * multiplier, Item.knockBack * multiplier, player.whoAmI);
                SoundStyle itemSound = new SoundStyle("Terraria/Sounds/Item_1");
                itemSound.Pitch = 0.2f;
                SoundEngine.PlaySound(itemSound, player.Center);
                lungeChargeTimer = 0;
            }
            if (Main.mouseRight && useCool <= 0)
            {
                if (abilityNumber == GrabAndSuck)
                {
                    if (!enemyBeingGrabbed)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active)
                            {
                                if (player.Distance(npc.Center) <= 1.6f * 16f && !npc.boss && !npc.immortal && !npc.hide)
                                {
                                    enemyBeingGrabbed = true;
                                    heldEnemyIndex = npc.whoAmI;
                                    vPlayer.enemyToIgnoreDamageFromIndex = npc.whoAmI;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        NPC heldNPC = Main.npc[heldEnemyIndex];
                        if (vPlayer.enemyToIgnoreDamageFromIndex == -1 || !heldNPC.active)
                        {
                            vPlayer.enemyToIgnoreDamageFromIndex = -1;
                            enemyBeingGrabbed = false;
                            heldEnemyIndex = -1;
                            heldEnemyTimer = 0;
                            useCool += 120;
                            player.immune = true;
                            player.immuneTime = 60;
                            return;
                        }

                        player.controlUp = false;
                        player.controlDown = false;
                        player.controlLeft = false;
                        player.controlRight = false;
                        player.controlJump = false;
                        player.velocity.X = 0f;
                        player.itemRotation = MathHelper.ToRadians(30f);

                        heldNPC.direction = -player.direction;
                        heldNPC.position = player.position + new Vector2(5f * player.direction, -2f - heldNPC.height / 3f);
                        heldNPC.velocity = new Vector2(0f, player.velocity.Y);
                        vPlayer.enemyToIgnoreDamageFromIndex = heldNPC.whoAmI;

                        heldEnemyTimer++;
                        if (heldEnemyTimer >= 60)
                        {
                            int suckAmount = (int)(heldNPC.lifeMax * 0.12f);
                            player.HealEffect(suckAmount);
                            player.statLife += suckAmount;
                            heldNPC.StrikeNPC(suckAmount, 0f, player.direction);
                            SoundStyle item3 = new SoundStyle("Terraria/Sounds/Item_3");
                            item3.Pitch = 0.2f;
                            SoundEngine.PlaySound(item3, player.Center);
                            heldEnemyTimer = 0;
                        }
                    }
                }
                else if (abilityNumber == SpacerRipperStingyEyes)
                {
                    eyeLaserChargeUpTimer++;
                    if (eyeLaserChargeUpTimer % 5 == 0)
                    {
                        int dustIndex = Dust.NewDust(player.Center + new Vector2(0f, -6f), 2, 2, 226, newColor: Color.MediumPurple);
                        Main.dust[dustIndex].noGravity = true;
                    }
                    if (eyeLaserChargeUpTimer >= 90)
                    {
                        if (eyeLaserChargeUpTimer % 15 == 0)
                        {
                            Vector2 shootVel = Main.MouseWorld - player.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= 12f;
                            int proj = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y - 20f, shootVel.X, shootVel.Y, ModContent.ProjectileType<SpaceRipperStingyEyes>(), 82, 4f, Main.myPlayer, 1f);
                            Main.projectile[proj].netUpdate = true;
                            SoundStyle item3 = new SoundStyle("Terraria/Sounds/Item_72");
                            item3.Pitch = 0.2f;
                            SoundEngine.PlaySound(item3, player.Center);
                        }
                        if (eyeLaserChargeUpTimer >= 165)
                        {
                            eyeLaserChargeUpTimer = 0;
                        }
                    }
                }
                else if (abilityNumber == ZombieMinionSummoning)
                {
                    zombieSummonTimer++;
                    if (zombieSummonTimer >= 6 * 60)
                    {
                        Vector2 randomPosition = new Vector2(player.position.X + Main.rand.Next(-4 * 16, (4 * 16) + 1), player.position.Y - (Main.screenHeight / 2f));
                        Projectile.NewProjectile(player.GetSource_FromThis(), randomPosition, Vector2.Zero, ModContent.ProjectileType<WarriorZombie>(), 37, 3f, player.whoAmI);
                        zombieSummonTimer = 0;
                    }
                    for (int d = 0; d < 11; d++)
                    {
                        int dustIndex = Dust.NewDust(player.Center + new Vector2(-2f, 8f), player.width, 2, DustID.Dirt);
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }
            if (!Main.mouseRight)
            {
                enemyBeingGrabbed = false;
                heldEnemyIndex = -1;
                heldEnemyTimer = 0;
                vPlayer.enemyToIgnoreDamageFromIndex = -1;
                eyeLaserChargeUpTimer = 0;
                zombieSummonTimer = 0;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tooltip != null)
            {
                string[] splitText = tooltip.Text.Split(' ');
                tooltip.Text = splitText.First() + " Vampiric " + splitText.Last();
            }

            string rightClickMessage = "";
            if (abilityNumber == GrabAndSuck)
            {
                rightClickMessage = "hold right-click to grab an enemy and suck their blood!";
            }
            else if (abilityNumber == SpacerRipperStingyEyes)
            {
                rightClickMessage = "hold right-click to use Space Ripper Stingy Eyes!";
            }
            else if (abilityNumber == ZombieMinionSummoning)
            {
                rightClickMessage = "hold right-click to spawn zombie minions!";
            }
            tooltips.Add(tooltip);

            TooltipLine tooltipAddition = new TooltipLine(Mod, "Speed", "Left-click to lunge and " + rightClickMessage + "\nSpecial: Switch the abilities used for right-click!");
            tooltips.Add(tooltipAddition);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddCondition(NetworkText.FromLiteral("VampireRequirement"), r => !Main.gameMenu && Main.LocalPlayer.GetModPlayer<VampirePlayer>().vampire)
                .Register();
        }
    }
}
