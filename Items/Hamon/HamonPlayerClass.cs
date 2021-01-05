using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System.Linq;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonIncreaseBonus = 0;

        public int hamonLevel = 0;
        public int amountOfHamon = 0;
        public int maxHamon = 60;
        public int hamonIncreaseCounter = 0;
        public int maxHamonCounter = 0;
        public int skillPointsAvailable = 2;

        public bool chargingHamon = false;
        public bool ajaStoneEquipped = false;

        //Adjustable skills
        public const int HamonSkillsLimit = 18;

        public const int BreathingRegenSkill = 0;
        public const int WaterWalkingSKill = 1;
        public const int WeaponsHamonImbueSkill = 2;

        //public bool[] learnedHamonSkills = new bool[HamonSkillsLimit];
        public Dictionary<int, bool> learnedHamonSkills = new Dictionary<int, bool>();
        public Dictionary<int, int> hamonAmountRequirements = new Dictionary<int, int>();


        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void OnEnterWorld(Player player)
        {
            if (learnedHamonSkills.Count == 0)
            {
                learnedHamonSkills.Add(BreathingRegenSkill, false);
                learnedHamonSkills.Add(WaterWalkingSKill, false);
                learnedHamonSkills.Add(WeaponsHamonImbueSkill, false);

                hamonAmountRequirements.Add(WaterWalkingSKill, 0);
                hamonAmountRequirements.Add(WeaponsHamonImbueSkill, 0);
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

            if (ajaStoneEquipped)           //Hamon charging stuff
            {
                maxHamon *= 2;
                maxHamonCounter = 120;
            }
            if (Mplayer.Vampire)
            {
                amountOfHamon = 0;
                hamonIncreaseCounter = 0;
            }

            if (!Mplayer.Vampire && player.breath == player.breathMax && amountOfHamon <= 60)       //in general, to increase Hamon while it can still be increased, no speeding up or decreasing
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
                Main.NewText("Broke");
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
                    regen *= 1.4f;
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

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (player.HasBuff(mod.BuffType("HamonWeaponImbueBuff")))
            {
                target.AddBuff(mod.BuffType("Sunburn"), 120);
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
                { "hamonSkillPoints", skillPointsAvailable }
            };
        }

        public override void Load(TagCompound tag)
        {
            IList<int> skillKeys = tag.GetList<int>("hamonSkillKeys");
            IList<bool> skillValues = tag.GetList<bool>("hamonSkillValues");
            IList<int> requirementKeys = tag.GetList<int>("hamonRequirementKeys");
            IList<int> requirementValues = tag.GetList<int>("hamonRequirementValues");
            skillPointsAvailable = tag.GetInt("hamonSkillPoints");
            /*for (int i = 0; i < keys.Count; i++)
            {
                if (learnedHamonSkills.ContainsKey(keys[i]))
                {
                    learnedHamonSkills[keys[i]] = values[i];
                }
            }*/
            if (learnedHamonSkills.Count != 0)
                learnedHamonSkills = skillKeys.Zip(skillValues, (key, value) => new {Key = key, Value = value}).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);

            if (hamonAmountRequirements.Count != 0)
                hamonAmountRequirements = requirementKeys.Zip(requirementValues, (key, value) => new { Key = key, Value = value }).ToDictionary(newKey => newKey.Key, newValue => newValue.Value);
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            hamonDamageBoosts = 1f;
            hamonKnockbackBoosts = 1f;
            hamonIncreaseBonus = 0;

            chargingHamon = false;
        }
    }
}