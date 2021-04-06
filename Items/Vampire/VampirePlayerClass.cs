using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public class VampirePlayer : ModPlayer
    {
        public bool zombie = false;
        public bool vampire = false;
        public bool perfectBeing = false;
        public bool anyMaskForm = false;
        public bool dyingVampire = false;

        public bool weakenedSunBurning = false;
        public bool noSunBurning = false;
        public bool enemyIgnoreItemInUse = false;
        public bool stopOnHitNPC = false;
        public int enemyToIgnoreDamageFromIndex = -1;

        public float vampiricDamageMultiplier = 1f;
        public float vampiricKnockbackMultiplier = 1f;

        public override void ResetEffects()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            zombie = false;
            vampire = false;
            perfectBeing = false;
            anyMaskForm = false;

            if (!enemyIgnoreItemInUse)
            {
                enemyToIgnoreDamageFromIndex = -1;
            }
            enemyIgnoreItemInUse = false;
            stopOnHitNPC = false;
            weakenedSunBurning = false;
            noSunBurning = false;
            vampiricDamageMultiplier = 1f;
            vampiricKnockbackMultiplier = 1f;
        }

        public override void PreUpdate()
        {
            anyMaskForm = zombie || vampire || perfectBeing;
            if (zombie || vampire)
            {
                if (!noSunBurning)
                {
                    Vector3 lightLevel = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3();
                    if (lightLevel.Length() > 1.3f && Main.dayTime && player.ZoneOverworldHeight && Main.tile[(int)player.Center.X / 16, (int)player.Center.Y / 16].wall == 0)
                    {
                        player.AddBuff(mod.BuffType("Sunburn"), 2, true);
                    }
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (anyMaskForm && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                if (newDamage < player.statLifeMax - player.statLife)
                {
                    player.statLife += newDamage;
                    player.HealEffect(newDamage, true);
                }
                if (newDamage >= player.statLifeMax - player.statLife)
                {
                    int healthReduction = player.statLifeMax - player.statLife;
                    int healingAmount = newDamage - healthReduction;
                    player.statLife += healingAmount;
                    player.HealEffect(healingAmount, true);
                }
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (anyMaskForm && proj.type == mod.ProjectileType("Fists") && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                if (newDamage < player.statLifeMax - player.statLife)
                {
                    player.statLife += newDamage;
                    player.HealEffect(newDamage, true);
                }
                if (newDamage >= player.statLifeMax - player.statLife)
                {
                    int healthReduction = player.statLifeMax - player.statLife;
                    int healingAmount = newDamage - healthReduction;
                    player.statLife += healingAmount;
                    player.HealEffect(healingAmount, true);
                }
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (npc.whoAmI == enemyToIgnoreDamageFromIndex)
            {
                return false;
            }
            return true;
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            enemyToIgnoreDamageFromIndex = -1;
        }

        public override void UpdateBadLifeRegen()
        {
            if (zombie || vampire)
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                if (player.lifeRegenTime > 0)
                {
                    player.lifeRegenTime = 0;
                }
                if (player.lifeRegenCount > 0)
                {
                    player.lifeRegenCount = 0;
                }
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (vampire && !dyingVampire)
            {
                dyingVampire = true;
                player.AddBuff(mod.BuffType("DyingVampire"), 60);
                player.statLife = 50;
                return false;
            }
            if (dyingVampire)
            {
                player.ClearBuff(mod.BuffType("DyingVampire"));
                dyingVampire = false;
            }
            if (MyPlayer.SecretReferences && player.ZoneSkyHeight && perfectBeing)
            {
                int karsText = Main.rand.Next(0, 3);
                if (karsText == 0)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " couldn't to become a bird in time and has frozen in space... then eventually stopped thinking...");
                }
                else if (karsText == 1)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " was unable to change directions in time... then eventually stopped thinking...");
                }
                else if (karsText == 2 && player.Male)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " became half-mineral, half-animal and floated forever through space, and though he wished for death, he was unable to die... then " + player.name + " eventually stopped thinking");
                }
                else if (karsText == 2 && !player.Male)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " became half-mineral, half-animal and floated forever through space, and though she wished for death, she was unable to die... then " + player.name + " eventually stopped thinking");
                }
            }
            vampire = false;
            return true;
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (dyingVampire)
            {
                PlayerLayer.Legs.visible = false;
                PlayerLayer.Body.visible = false;
                PlayerLayer.Skin.visible = false;
                PlayerLayer.Arms.visible = false;
                PlayerLayer.HeldItem.visible = false;
                PlayerLayer.ShieldAcc.visible = false;
                PlayerLayer.ShoeAcc.visible = false;
                PlayerLayer.BalloonAcc.visible = false;
                PlayerLayer.Wings.visible = false;
            }
        }
    }
}