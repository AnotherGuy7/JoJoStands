using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public static bool HamonEffects = true;

        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonIncreaseBonus = 0;

        public int hamonLevel = 0;
        public int amountOfHamon = 0;
        public int maxHamon = 60;
        public int hamonIncreaseCounter = 0;
        public int maxHamonCounter = 0;
        public int skillPointsAvailable = 1;
        public int hamonLayerFrame = 0;
        public int hamonLayerFrameCounter = 0;
        public int defensiveHamonLayerFrame = 0;
        public int defensiveHamonLayerFrameCounter = 0;
        public int defensiveAuraDownDoublePressTimer = 0;
        public int hamonOverChargeSpecialDoublePressTimer = 0;

        public bool passiveRegen = false;
        public bool chargingHamon = false;
        public bool ajaStoneEquipped = false;
        public bool defensiveHamonAuraActive = false;
        public bool defensiveAuraCanPressDownAgain = false;

        //Adjustable skills
        public const int HamonSkillsLimit = 18;

        public const int BreathingRegenSkill = 0;
        public const int WaterWalkingSKill = 1;
        public const int WeaponsHamonImbueSkill = 2;
        public const int HamonItemHealing = 3;
        public const int DefensiveHamonAura = 4;
        public const int HamonShockwave = 5;
        public const int HamonOvercharge = 6;
        public const int HamonHerbalGrowth = 7;
        public const int PassiveHamonRegenBoost = 8;

        //public bool[] learnedHamonSkills = new bool[HamonSkillsLimit];
        public Dictionary<int, bool> learnedHamonSkills = new Dictionary<int, bool>();
        public Dictionary<int, int> hamonAmountRequirements = new Dictionary<int, int>();
        public Dictionary<int, int> hamonSkillLevels = new Dictionary<int, int>();


        public override void ResetEffects()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            hamonDamageBoosts = 1f;
            hamonKnockbackBoosts = 1f;
            hamonIncreaseBonus = 0;

            chargingHamon = false;
            passiveRegen = true;
        }

        public override void OnEnterWorld(Player player)
        {
            if (learnedHamonSkills.Count == 0)
            {
                learnedHamonSkills.Add(BreathingRegenSkill, false);
                learnedHamonSkills.Add(WaterWalkingSKill, false);
                learnedHamonSkills.Add(WeaponsHamonImbueSkill, false);
                learnedHamonSkills.Add(HamonItemHealing, false);
                learnedHamonSkills.Add(DefensiveHamonAura, false);
                learnedHamonSkills.Add(HamonShockwave, false);
                learnedHamonSkills.Add(HamonOvercharge, false);
                learnedHamonSkills.Add(HamonHerbalGrowth, false);
                learnedHamonSkills.Add(PassiveHamonRegenBoost, false);

                hamonSkillLevels.Add(BreathingRegenSkill, 0);
                hamonSkillLevels.Add(WaterWalkingSKill, 0);
                hamonSkillLevels.Add(WeaponsHamonImbueSkill, 0);
                hamonSkillLevels.Add(HamonItemHealing, 0);
                hamonSkillLevels.Add(DefensiveHamonAura, 0);
                hamonSkillLevels.Add(HamonShockwave, 0);
                hamonSkillLevels.Add(HamonOvercharge, 0);
                hamonSkillLevels.Add(HamonHerbalGrowth, 0);
                hamonSkillLevels.Add(PassiveHamonRegenBoost, 0);

                //Only skills that need hamon to be used should add to the requirements dictionary
                hamonAmountRequirements.Add(WaterWalkingSKill, 0);
                hamonAmountRequirements.Add(WeaponsHamonImbueSkill, 0);
                hamonAmountRequirements.Add(HamonItemHealing, 0);
                hamonAmountRequirements.Add(DefensiveHamonAura, 0);
                hamonAmountRequirements.Add(HamonShockwave, 0);
                hamonAmountRequirements.Add(HamonHerbalGrowth, 0);
            }
        }

        public override void PreUpdate()
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            if (NPC.downedBoss1)      //It's written this way so that by the time it gets to the bottom it would have the actual Hamon Level
            {
                hamonLevel = 1;
            }
            if (NPC.downedBoss2)      //the crimson/corruption bosses
            {
                hamonLevel = 2;
            }
            if (NPC.downedBoss3)       //skeletron
            {
                hamonLevel = 3;
            }
            if (Main.hardMode)      //wall of flesh
            {
                hamonLevel = 4;
            }
            if (NPC.downedMechBoss1)
            {
                hamonLevel = 5;
            }
            if (NPC.downedMechBoss2)
            {
                hamonLevel = 6;
            }
            if (NPC.downedMechBoss3)
            {
                hamonLevel = 7;
            }
            if (NPC.downedPlantBoss)       //plantera
            {
                hamonLevel = 8;
            }
            if (NPC.downedGolemBoss)
            {
                hamonLevel = 9;
            }
            if (NPC.downedMoonlord)     //you are an expert with hamon by moon lord
            {
                hamonLevel = 10;
            }

            switch (hamonLevel)     //done this way cause different things will be done with it
            {
                case 1:
                    maxHamon = 72;
                    break;
                case 2:
                    maxHamon = 84;
                    break;
                case 3:
                    maxHamon = 96;
                    break;
                case 4:
                    maxHamon = 108;
                    hamonIncreaseBonus += 1;
                    break;
                case 5:
                    maxHamon = 120;
                    hamonIncreaseBonus += 1;
                    break;
                case 6:
                    maxHamon = 132;
                    hamonIncreaseBonus += 1;
                    break;
                case 7:
                    maxHamon = 144;
                    hamonIncreaseBonus += 2;
                    break;
                case 8:
                    maxHamon = 156;
                    hamonIncreaseBonus += 2;
                    break;
                case 9:
                    maxHamon = 168;
                    hamonIncreaseBonus += 2;
                    break;
                case 10:
                    maxHamon = 180;
                    hamonIncreaseBonus += 3;
                    break;
            }

            ManageAbilities();
            if (ajaStoneEquipped)           //Hamon charging stuff
            {
                maxHamon *= 2;
                maxHamonCounter = 120;
            }
            if (Mplayer.vampire)
            {
                amountOfHamon = 0;
                hamonIncreaseCounter = 0;
            }
            if (passiveRegen)
            {
                if (learnedHamonSkills.ContainsKey(PassiveHamonRegenBoost) && learnedHamonSkills[PassiveHamonRegenBoost])
                {
                    hamonIncreaseBonus += hamonSkillLevels[PassiveHamonRegenBoost];
                }

                if (!Mplayer.vampire && player.breath == player.breathMax && amountOfHamon <= 60)       //in general, to increase Hamon while it can still be increased, no speeding up or decreasing
                {
                    if (player.velocity.X == 0f)
                    {
                        hamonIncreaseCounter++;
                    }
                    else
                    {
                        hamonIncreaseCounter += 2;
                    }

                    hamonIncreaseCounter += hamonIncreaseBonus;
                }
                if (hamonIncreaseCounter >= maxHamonCounter)      //the one that increases Hamon
                {
                    if (ajaStoneEquipped)       //or any other decrease-preventing accessories
                    {
                        hamonIncreaseCounter = 0;
                        amountOfHamon += 1;
                    }
                    if (amountOfHamon < 60)
                    {
                        hamonIncreaseCounter = 0;
                        amountOfHamon += 1;
                    }
                }
                if (hamonIncreaseCounter >= maxHamonCounter && amountOfHamon > 60 && !ajaStoneEquipped)      //the one that decreases Hamon
                {
                    hamonIncreaseCounter = 0;
                    amountOfHamon -= 1;
                }
                if (!ajaStoneEquipped)          //list maxHamonCounter decreasing things in here
                {
                    maxHamonCounter = 240;
                }
                if (amountOfHamon > 60 && amountOfHamon < 120 && !ajaStoneEquipped)      //every 6 seconds, while Hamon is at the UI's second row, it decreases. Only if you don't have the Aja Stone
                {
                    maxHamonCounter = 360;
                }
                if (amountOfHamon >= 120 && !ajaStoneEquipped)      //same as top but every 3 seconds
                {
                    maxHamonCounter = 180;
                }
            }

            if (amountOfHamon >= maxHamon)       //hamon limit stuff
            {
                amountOfHamon = maxHamon;
            }
            if (amountOfHamon <= 0)
            {
                amountOfHamon = 0;
            }

            /*if (learnedHamonSkills.Count == 0)
            {
                Main.NewText("Broke; " + learnedHamonSkills.Count);
            }*/
        }

        public override void MeleeEffects(Item item, Rectangle hitbox)
        {
            if (player.HasBuff(mod.BuffType("HamonWeaponImbueBuff")))
            {
                Vector2 hitboxPosition = new Vector2(hitbox.X, hitbox.Y);
                Dust.NewDust(hitboxPosition, hitbox.Width, hitbox.Height, 169);
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (chargingHamon)
            {
                if (learnedHamonSkills.ContainsKey(BreathingRegenSkill) && learnedHamonSkills[BreathingRegenSkill])
                {
                    regen *= 1.2f + (0.2f * hamonSkillLevels[BreathingRegenSkill]);
                }
            }
        }

        public override void PreUpdateMovement()
        {
            if (learnedHamonSkills.ContainsKey(WaterWalkingSKill) && learnedHamonSkills[WaterWalkingSKill])
            {
                if (amountOfHamon > hamonAmountRequirements[WaterWalkingSKill])
                {
                    player.waterWalk2 = true;
                }
            }
        }

        private void ManageAbilities()
        {
            if (!learnedHamonSkills.ContainsKey(HamonHerbalGrowth) || !hamonAmountRequirements.ContainsKey(HamonHerbalGrowth))        //Just checking if something exists in the dictionary, if not, just skipping
                return;

            if (defensiveAuraDownDoublePressTimer > 0)
                defensiveAuraDownDoublePressTimer--;

            if (defensiveHamonAuraActive)
            {
                passiveRegen = false;
                if (amountOfHamon <= hamonAmountRequirements[DefensiveHamonAura])
                {
                    defensiveHamonAuraActive = false;
                }
                if (player.controlDown && defensiveAuraDownDoublePressTimer <= 0 && player.velocity == Vector2.Zero)
                {
                    defensiveHamonAuraActive = false;
                }
            }
            else
            {
                if (amountOfHamon >= hamonAmountRequirements[DefensiveHamonAura] && player.velocity == Vector2.Zero)
                {
                    if (player.controlDown && defensiveAuraDownDoublePressTimer <= 0)
                    {
                        defensiveAuraDownDoublePressTimer += 30;
                    }
                    if (!player.controlDown && defensiveAuraDownDoublePressTimer > 0)
                    {
                        defensiveAuraCanPressDownAgain = true;
                    }
                    if (player.controlDown && defensiveAuraDownDoublePressTimer > 0 && defensiveAuraCanPressDownAgain && learnedHamonSkills[DefensiveHamonAura])
                    {
                        defensiveHamonAuraActive = true;
                        defensiveAuraCanPressDownAgain = false;
                        defensiveAuraDownDoublePressTimer += 30;
                    }
                }
            }

            if (learnedHamonSkills[HamonHerbalGrowth])
            {
                if (player.velocity.Y > 0f)
                {
                    if (player.controlUp && amountOfHamon > 5 && player.mount.Type == -1 && !WorldGen.SolidTile((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f) + 2))
                    {
                        player.mount.SetMount(mod.MountType("LeafGliderMount"), player);
                    }
                }
                /*int targetCoordX = (int)(player.position.X / 16f);
                int targetCoordY = (int)(player.position.X / 16f);
                if (TileLoader.IsSapling(Main.tile[targetCoordX, targetCoordY].type))
                {
                    Dust.NewDust(player.position, 5, 5, 169);
                    if (Main.rand.Next(1, 151) == 1)
                    {
                        if (WorldGen.GrowTree(targetCoordX, targetCoordY))
                        {
                            WorldGen.TreeGrowFXCheck(targetCoordX, targetCoordY);
                        }
                    }
                }*/
            }

            if (learnedHamonSkills[HamonOvercharge])
            {
                bool specialJustPressed = false;
                if (!Main.dedServ)
                    specialJustPressed = JoJoStands.SpecialHotKey.JustPressed;

                if (specialJustPressed && player.HeldItem.type == ItemID.None)
                {
                    if (hamonOverChargeSpecialDoublePressTimer <= 0)
                    {
                        hamonOverChargeSpecialDoublePressTimer = 30;
                    }
                    else
                    {
                        if (amountOfHamon <= hamonAmountRequirements[HamonOvercharge] && player.statLife > (int)(player.statLifeMax * 0.25f))
                        {
                            amountOfHamon += 30 * hamonSkillLevels[HamonOvercharge];
                            player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " overcharged themselves with Hamon."), (int)(player.statLifeMax * 0.05), player.direction);
                        }
                        hamonOverChargeSpecialDoublePressTimer = 0;
                    }
                }
            }

            if (learnedHamonSkills[HamonShockwave])
            {
                if (amountOfHamon > hamonAmountRequirements[HamonShockwave] && player.controlDown && player.velocity.Y > 0f && WorldGen.SolidTile((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f) + 2))
                {
                    Projectile.NewProjectile(player.Center + new Vector2(0f, 5f), new Vector2(0.01f * player.direction, 0f), mod.ProjectileType("HamonShockwaveSpike"), 32 * hamonSkillLevels[HamonShockwave], 4f * hamonSkillLevels[HamonShockwave], player.whoAmI, player.direction, hamonSkillLevels[HamonShockwave]);
                    amountOfHamon -= hamonAmountRequirements[HamonShockwave];
                }
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (player.HasBuff(mod.BuffType("HamonWeaponImbueBuff")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 120 * (player.GetModPlayer<HamonPlayer>().hamonSkillLevels[WeaponsHamonImbueSkill] - 1));
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (defensiveHamonAuraActive)
            {
                damage = (int)(damage * 0.95f);
                amountOfHamon -= 3;
				
				if (Main.rand.Next(0, 7) == 0)
				{
					npc.AddBuff(mod.BuffType("Sunburn"), 80 * hamonSkillLevels[DefensiveHamonAura]);
				}
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "hamonSkillKeys", learnedHamonSkills.Keys.ToList() },
                { "hamonSkillValues", learnedHamonSkills.Values.ToList() },
                { "hamonRequirementKeys", hamonAmountRequirements.Keys.ToList() },
                { "hamonRequirementValues", hamonAmountRequirements.Values.ToList() },
                { "hamonLevelKeys", hamonSkillLevels.Keys.ToList() },
                { "hamonLevelValues", hamonSkillLevels.Values.ToList() },
                { "hamonSkillPoints", skillPointsAvailable }
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<int> skillKeys = tag.GetList<int>("hamonSkillKeys");
            IList<bool> skillValues = tag.GetList<bool>("hamonSkillValues");
            IList<int> requirementKeys = tag.GetList<int>("hamonRequirementKeys");
            IList<int> requirementValues = tag.GetList<int>("hamonRequirementValues");
            IList<int> levelKeys = tag.GetList<int>("hamonLevelKeys");
            IList<int> levelValues = tag.GetList<int>("hamonLevelValues");
            skillPointsAvailable = tag.GetInt("hamonSkillPoints");
            /*for (int i = 0; i < keys.Count; i++)
            {
                if (learnedHamonSkills.ContainsKey(keys[i]))
                {
                    learnedHamonSkills[keys[i]] = values[i];
                }
            }*/

            //if (learnedHamonSkills.Count != 0)
                learnedHamonSkills = skillKeys.Zip(skillValues, (key, value) => new {Key = key, Value = value}).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);

            //if (hamonAmountRequirements.Count != 0)
                hamonAmountRequirements = requirementKeys.Zip(requirementValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);

            //if (hamonSkillLevels.Count != 0)
                hamonSkillLevels = levelKeys.Zip(levelValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        public static readonly PlayerLayer HamonChargesFront = new PlayerLayer("JoJoStands", "HamonChargesFront", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            HamonPlayer hamonPlayer = drawPlayer.GetModPlayer<HamonPlayer>();
            SpriteEffects effects = SpriteEffects.None;
            if (drawPlayer.active && hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 3 && drawPlayer.velocity == Vector2.Zero)
            {
                Texture2D texture = mod.GetTexture("Extras/HamonChargeI");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);
                if (hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 2)
                {
                    texture = mod.GetTexture("Extras/HamonChargeII");
                }
                if (hamonPlayer.amountOfHamon >= hamonPlayer.maxHamon / 1.5)
                {
                    texture = mod.GetTexture("Extras/HamonChargeIII");
                }

                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                    effects = SpriteEffects.FlipHorizontally;
                }
                int frameHeight = texture.Height / 7;

                hamonPlayer.hamonLayerFrameCounter++;
                if (hamonPlayer.hamonLayerFrameCounter >= 6)
                {
                    hamonPlayer.hamonLayerFrame += 1;
                    hamonPlayer.hamonLayerFrameCounter = 0;
                    if (hamonPlayer.hamonLayerFrame >= 7)
                    {
                        hamonPlayer.hamonLayerFrame = 0;
                    }
                }

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameHeight * hamonPlayer.hamonLayerFrame, texture.Width, frameHeight), Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
                Main.playerDrawData.Add(data);
            }
        });

        public static readonly PlayerLayer DefensiveHamonAuraLayer = new PlayerLayer("JoJoStands", "DefensiveHamonAuraLayer", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)     //gotten from ExampleMod's MyPlayer, but I understand what is happening
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("JoJoStands");
            HamonPlayer hamonPlayer = drawPlayer.GetModPlayer<HamonPlayer>();
            if (drawPlayer.active && hamonPlayer.defensiveHamonAuraActive)
            {
                Texture2D auraTexture = mod.GetTexture("Extras/DefensiveHamonAura");
                int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X - 1f);
                int drawY = (int)(drawInfo.position.Y + 20f - Main.screenPosition.Y);

                if (drawPlayer.direction == -1)
                {
                    drawX += 2;
                }
                int frameHeight = auraTexture.Height / 7;
                hamonPlayer.defensiveHamonLayerFrameCounter++;
                if (hamonPlayer.defensiveHamonLayerFrameCounter >= 8)
                {
                    hamonPlayer.defensiveHamonLayerFrame += 1;
                    hamonPlayer.defensiveHamonLayerFrameCounter = 0;
                    if (hamonPlayer.defensiveHamonLayerFrame >= 7)
                    {
                        hamonPlayer.defensiveHamonLayerFrame = 0;
                    }
                }

                Vector2 position = new Vector2(drawX, drawY);
                Rectangle animRect = new Rectangle(0, frameHeight * hamonPlayer.defensiveHamonLayerFrame, auraTexture.Width, frameHeight);
                Color color = Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y + drawPlayer.height / 2f) / 16f));
                Vector2 origin = new Vector2(auraTexture.Width / 2f, frameHeight / 2f);

                DrawData drawData = new DrawData(auraTexture, position, animRect, color, drawPlayer.bodyRotation, origin, 1f, SpriteEffects.None, 0);
                Main.playerDrawData.Add(drawData);
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (player.dead || (player.mount.Type != -1))
            {
                HamonChargesFront.visible = false;
                DefensiveHamonAuraLayer.visible = false;
            }
            else
            {
                HamonChargesFront.visible = HamonEffects;
                DefensiveHamonAuraLayer.visible = true;
            }
			
			int miscEffectsLayer = layers.FindIndex(l => l == PlayerLayer.MiscEffectsBack);
            if (miscEffectsLayer > -1)
            {
                layers.Insert(miscEffectsLayer + 1, DefensiveHamonAuraLayer);
            }

            layers.Add(HamonChargesFront);
        }
    }
}