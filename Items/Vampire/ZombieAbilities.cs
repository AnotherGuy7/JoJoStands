using JoJoStands.NPCs;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class ZombieAbilities : VampireDamageClass
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zombie Abilities");
            // Tooltip.SetDefault("Left-click to lunge at enemies! Hold left-click to charge up the lunge and make it stronger!");
            Item.ResearchUnlockCount = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 21;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.consumable = false;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.knockBack = 7f;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
        }

        private int useCool = 0;
        private int lungeChargeTimer = 0;
        private bool enemyBeingGrabbed = false;
        private int heldEnemyIndex = -1;
        private int heldEnemyTimer = 0;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (player.whoAmI != Main.myPlayer || !vPlayer.zombie || (mPlayer.standOut && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual))
                return;

            if (useCool > 0)
                useCool--;

            vPlayer.enemyIgnoreItemInUse = true;
            if (Main.mouseLeft && useCool <= 0)
            {
                lungeChargeTimer++;
                if (lungeChargeTimer > 180)
                    lungeChargeTimer = 180;
            }
            if (!Main.mouseLeft && lungeChargeTimer > 0 && useCool <= 0)
            {
                useCool += Item.useTime + (20 * (lungeChargeTimer / 30));
                int multiplier = lungeChargeTimer / 60;
                if (multiplier == 0)
                {
                    multiplier = 1;
                }
                if (Main.MouseWorld.X - player.position.X >= 0)
                {
                    player.direction = 1;
                }
                else
                {
                    player.direction = -1;
                }
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
            if (Main.mouseRight && useCool <= 0 && vPlayer.HasSkill(player, VampirePlayer.BloodSuck))
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
                    heldNPC.GetGlobalNPC<JoJoGlobalNPC>().vampireUserLastHitIndex = player.whoAmI;

                    heldEnemyTimer++;
                    if (heldEnemyTimer >= 60)
                    {
                        int suckAmount = (int)(heldNPC.lifeMax * 0.08f);
                        player.HealEffect(suckAmount);
                        player.statLife += suckAmount;
                        NPC.HitInfo hitInfo = new NPC.HitInfo()
                        {
                            Damage = suckAmount,
                            Knockback = 0f,
                            HitDirection = player.direction
                        };
                        heldNPC.StrikeNPC(hitInfo);
                        SoundStyle item3 = new SoundStyle("Terraria/Sounds/Item_3");
                        item3.Pitch = 0.2f;
                        SoundEngine.PlaySound(item3, player.Center);
                        heldEnemyTimer = 0;
                    }
                }
            }
            else
            {
                enemyBeingGrabbed = false;
                heldEnemyIndex = -1;
                heldEnemyTimer = 0;
                vPlayer.enemyToIgnoreDamageFromIndex = -1;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (!vPlayer.learnedAnyZombieAbility)
                return;

            if (vPlayer.HasSkill(player, VampirePlayer.BloodSuck))
            {
                TooltipLine secondaryUseTooltip = new TooltipLine(JoJoStands.Instance, "Secondary Use", "Hold right-click to grab an enemy and suck their blood!");
                tooltips.Add(secondaryUseTooltip);
            }
        }

        public override void AddRecipes()
        {
            Condition condition = new Condition("Mods.JoJoStands.Conditions.ZombieAbilitiesCondition", () => !Main.gameMenu && Main.LocalPlayer.GetModPlayer<VampirePlayer>().zombie);
            CreateRecipe()
                .AddCondition(condition)
                .Register();
        }
    }
}
