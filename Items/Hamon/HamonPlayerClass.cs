using JoJoStands.Buffs.ItemBuff;
using JoJoStands.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public static bool HamonEffects = true;

        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonIncreaseBonus = 0;

        public int hamonLevel = 0;
        public int hamonStage = 0;
        public int amountOfHamon = 0;
        public int maxHamon = 60;
        public int hamonIncreaseCounter = 0;
        public int hamonDecreaseCounter = 0;
        public int maxHamonCounter = 0;
        public int hamonLayerFrame = 0;
        public int hamonLayerFrameCounter = 0;
        public int hamonAuraStandTimer = 0;

        public bool passiveRegen = false;
        public bool chargingHamon = false;
        public bool ajaStoneEquipped = false;
        public bool learnedHamon = false;


        public override void ResetEffects()
        {
            ResetVariables();
        }

        private void ResetVariables()
        {
            hamonDamageBoosts = 1f;
            hamonKnockbackBoosts = 1f;
            hamonIncreaseBonus = 0;
            maxHamonCounter = 300;
            hamonStage = 0;

            chargingHamon = false;
            passiveRegen = true;
            ajaStoneEquipped = false;
        }

        public override void OnEnterWorld(Player Player)
        {
            if (learnedHamon)
                HamonBar.ShowHamonBar();
            else
            {
                if (HamonBar.visible)
                    HamonBar.HideHamonBar();
            }
        }

        public override void PreUpdate()
        {
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

            if (!learnedHamon)
                return;

            if (amountOfHamon <= 0)
                hamonStage = 0;
            else if (amountOfHamon <= 60)
                hamonStage = 1;
            else if (amountOfHamon > 60 && amountOfHamon <= 120)
                hamonStage = 2;
            else if (amountOfHamon > 120)
                hamonStage = 3;

            if (ajaStoneEquipped)           //Hamon charging stuff
                maxHamon *= 2;
            if (Player.velocity.X == 0f)
                hamonIncreaseBonus += 1;

            if (passiveRegen)
            {
                if (amountOfHamon < 60 && Player.breath == Player.breathMax)       //in general, to increase Hamon while it can still be increased, no speeding up or decreasing
                {
                    hamonIncreaseCounter += 1 + hamonIncreaseBonus;
                }
                if (hamonIncreaseCounter >= maxHamonCounter)      //the one that increases Hamon
                {
                    if (amountOfHamon < 60)
                        amountOfHamon += 1;

                    hamonIncreaseCounter = 0;
                }
            }
            if (hamonStage > 1)
            {
                hamonDecreaseCounter++;
                if (hamonDecreaseCounter > 240 / (hamonStage - 1))
                {
                    amountOfHamon -= 1;
                    hamonDecreaseCounter = 0;
                }
            }
            amountOfHamon = (int)MathHelper.Clamp(amountOfHamon, 0, maxHamon);
        }

        public override void PreUpdateBuffs()
        {
            if (hamonStage == 2)
                Player.AddBuff(ModContent.BuffType<HamonChargedII>(), 2);
            else if (hamonStage == 1)
                Player.AddBuff(ModContent.BuffType<HamonChargedI>(), 2);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("learnedHamon", learnedHamon);
        }

        public override void LoadData(TagCompound tag)
        {
            learnedHamon = tag.GetBool("learnedHamon");
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }
    }
}