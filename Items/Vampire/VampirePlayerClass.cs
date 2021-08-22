using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JoJoStands.Items.Vampire
{
    public class VampirePlayer : ModPlayer
    {
        public bool zombie = false;
        public bool vampire = false;
        public bool perfectBeing = false;
        public bool anyMaskForm = false;
        public bool dyingVampire = false;

        public bool noSunBurning = false;
        public bool enemyIgnoreItemInUse = false;
        public bool stopOnHitNPC = false;
        public int enemyToIgnoreDamageFromIndex = -1;
        public int enemySearchTimer = 0;

        public float vampiricDamageMultiplier = 1f;
        public float vampiricKnockbackMultiplier = 1f;
        public float lifeStealMultiplier = 1f;
        public float lifeStealPercentLoss = 0f;
        public float lifeStealPercentLossTimer = 0;
        public float sunburnRegenTimeMultiplier = 0f;
        public float sunburnDamageMultiplier = 1f;
        public float sunburnMoveSpeedMultiplier = 0.5f;

        public bool wearingDiosScarf = false;

        public int[] enemiesKilled = new int[500];
        public int vampireSkillPointsAvailable = 1;

        public const int ExpectedAmountOfZombieSkills = 6;
        public const int UndeadConstitution = 0;
        public const int UndeadPerception = 1;
        public const int ProtectiveFilm = 2;
        public const int BloodSuck = 3;
        public const int BrutesStrength = 4;
        public const int KnifeWielder = 5;
        public bool learnedAnyZombieAbility = false;
        public Dictionary<int, bool> learnedZombieSkills = new Dictionary<int, bool>();
        public Dictionary<int, int> zombieSkillLevels = new Dictionary<int, int>();

        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void OnEnterWorld(Player player)
        {
            if (learnedZombieSkills.Count != ExpectedAmountOfZombieSkills)
            {
                RebuildZombieAbilitiesDictionaries();
            }
            if (enemiesKilled.Length == 0)
            {
                enemiesKilled = new int[Main.maxNPCTypes];
            }
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
            noSunBurning = false;
            vampiricDamageMultiplier = 1f;
            vampiricKnockbackMultiplier = 1f;
            lifeStealMultiplier = 1f;
            sunburnRegenTimeMultiplier = 0f;
            sunburnDamageMultiplier = 1f;
            sunburnMoveSpeedMultiplier = 0.5f;

            wearingDiosScarf = false;
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

            ManageAbilities();
            if (lifeStealPercentLossTimer > 0)
            {
                lifeStealPercentLossTimer--;
            }
            else
            {
                if (lifeStealPercentLoss > 0f)
                {
                    lifeStealPercentLoss -= 0.01f;
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (anyMaskForm && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                StealHealthFrom(target, damage);
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            JoJoGlobalNPC jojoNPC = target.GetGlobalNPC<JoJoGlobalNPC>();
            bool itemIsVampireItem = item.modItem is VampireDamageClass;
            if (target.life == target.lifeMax && (item.melee || itemIsVampireItem) && jojoNPC.zombieHightlightTimer > 0)
            {
                damage = (int)(damage * 1.2f);
                knockback *= 1.2f;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (anyMaskForm && proj.type == mod.ProjectileType("Fists") && target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                StealHealthFrom(target, damage);
            }
        }

        private void ManageAbilities()
        {
            if (!anyMaskForm)
                return;

            if (zombie)
            {
                if (!learnedAnyZombieAbility || learnedZombieSkills.Count == 0)
                    return;

                if (learnedZombieSkills[UndeadConstitution])
                {
                    float percentageBoost = 0.035f * zombieSkillLevels[UndeadConstitution];
                    player.moveSpeed *= percentageBoost;
                    player.allDamageMult *= percentageBoost;
                }

                if (!Main.dayTime && learnedZombieSkills[UndeadPerception])
                {
                    enemySearchTimer++;
                    if (enemySearchTimer >= 20 * 60)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.immortal && !npc.hide && npc.life == npc.lifeMax)
                            {
                                npc.GetGlobalNPC<JoJoGlobalNPC>().zombieHightlightTimer += 5 * 60;
                            }
                        }
                        enemySearchTimer = 0;
                    }
                }
            }
        }

        public void StealHealthFrom(NPC target, int damage)
        {
            if (target.lifeMax > 5 && !target.friendly && !target.dontTakeDamage && !target.immortal)
            {
                int newDamage = damage / 4;
                int calculatedLifeSteal = (int)((newDamage * lifeStealMultiplier) * (1f - lifeStealPercentLoss));
                if (calculatedLifeSteal < player.statLifeMax - player.statLife)
                {
                    player.statLife += calculatedLifeSteal;
                    player.HealEffect(calculatedLifeSteal, true);
                }
                if (calculatedLifeSteal >= player.statLifeMax - player.statLife)
                {
                    calculatedLifeSteal = player.statLifeMax - player.statLife;
                    player.statLife += (int)(calculatedLifeSteal);
                    player.HealEffect(calculatedLifeSteal, true);
                }

                if (lifeStealPercentLoss < 0.97f)
                {
                    lifeStealPercentLoss += 0.03f;
                }
                lifeStealPercentLossTimer = 300;
            }
        }

        public void RebuildZombieAbilitiesDictionaries()
        {
            learnedZombieSkills.Clear();
            zombieSkillLevels.Clear();

            learnedZombieSkills.Add(UndeadConstitution, false);
            learnedZombieSkills.Add(UndeadPerception, false);
            learnedZombieSkills.Add(ProtectiveFilm, false);
            learnedZombieSkills.Add(BloodSuck, false);
            learnedZombieSkills.Add(BrutesStrength, false);
            learnedZombieSkills.Add(KnifeWielder, false);

            zombieSkillLevels.Add(UndeadConstitution, 0);
            zombieSkillLevels.Add(UndeadPerception, 0);
            zombieSkillLevels.Add(ProtectiveFilm, 0);
            zombieSkillLevels.Add(BloodSuck, 0);
            zombieSkillLevels.Add(BrutesStrength, 0);
            zombieSkillLevels.Add(KnifeWielder, 0);

            learnedAnyZombieAbility = false;
            Main.NewText("Rebuilt Vampire Skills Dictionaries.", Color.Red);
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

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (wearingDiosScarf)
            {
                if (npc.TypeName.Contains("Zombie") || npc.TypeName.Contains("Undead") || npc.TypeName.Contains("Skeleton") || npc.type == NPCID.Zombie)
                {
                    damage = (int)(damage * 0.85f);
                }
            }
            if (player.HasBuff(mod.BuffType("KnifeAmalgamation")))
            {
                npc.StrikeNPC(29, 2f, -npc.direction);
            }
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
            return true;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "vampireSkillPointsAvailable", vampireSkillPointsAvailable },
                { "learnedAnyZombieAbility", learnedAnyZombieAbility },
                { "zombieSkillKeys", learnedZombieSkills.Keys.ToList() },
                { "zombieSkillValues", learnedZombieSkills.Values.ToList() },
                { "zombieLevelKeys", zombieSkillLevels.Keys.ToList() },
                { "zombieLevelValues", zombieSkillLevels.Values.ToList() },
                { "enemiesKilled", enemiesKilled }
            };
        }

        public override void Load(TagCompound tag)
        {
            vampireSkillPointsAvailable = tag.GetInt("vampireSkillPointsAvailable");
            learnedAnyZombieAbility = tag.GetBool("learnedAnyZombieAbility");
            IList<int> zombieSkillKeys = tag.GetList<int>("zombieSkillKeys");
            IList<bool> zombieSkillValues = tag.GetList<bool>("zombieSkillValues");
            IList<int> zombieLevelKeys = tag.GetList<int>("zombieLevelKeys");
            IList<int> zombieLevelValues = tag.GetList<int>("zombieLevelValues");
            enemiesKilled = tag.GetIntArray("enemiesKilled");

            learnedZombieSkills = zombieSkillKeys.Zip(zombieSkillValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);
            zombieSkillLevels = zombieLevelKeys.Zip(zombieLevelValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        public static readonly PlayerLayer ProtectiveFilmLayer = new PlayerLayer("JoJoStands", "ProtectiveFilmLayer", PlayerLayer.FrontAcc, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            VampirePlayer vampirePlayer = drawPlayer.GetModPlayer<VampirePlayer>();
            if (drawPlayer.active && drawPlayer.HasBuff(mod.BuffType("ProtectiveFilmBuff")))
            {
                Texture2D texture = mod.GetTexture("Extras/ProtectiveFilmLayer");
                int drawX = (int)drawInfo.position.X;
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2f - 1f);
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, color * alpha, drawPlayer.fullRotation, drawPlayer.Size / 2f, 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer KnivesLayer = new PlayerLayer("JoJoStands", "KnivesLayer", PlayerLayer.FrontAcc, delegate (PlayerDrawInfo drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            VampirePlayer vampirePlayer = drawPlayer.GetModPlayer<VampirePlayer>();
            if (drawPlayer.active && drawPlayer.HasBuff(mod.BuffType("KnifeAmalgamation")))
            {
                Texture2D texture = mod.GetTexture("Extras/KnivesLayer");
                int drawX = (int)drawInfo.position.X;
                int drawY = (int)(drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2f - 1f);
                Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
                SpriteEffects effects = SpriteEffects.None;
                if (drawPlayer.direction == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                float alpha = (255 - drawPlayer.immuneAlpha) / 255f;
                Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                DrawData data = new DrawData(texture, position, drawPlayer.bodyFrame, color * alpha, drawPlayer.fullRotation, drawPlayer.Size / 2f, 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

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
            layers.Add(ProtectiveFilmLayer);
            layers.Add(KnivesLayer);
        }
    }
}