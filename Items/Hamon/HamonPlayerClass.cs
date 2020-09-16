using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonIncreaseBonus = 0;

        public int hamonLevel = 0;
        public int HamonCounter = 0;
        public int maxHamon = 60;
        public int hamonIncreaseCounter = 0;
        public int maxHamonCounter = 0;

        public bool AjaStone = false;

        public override void ResetEffects()
        {
            ResetVariables();
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

            if (AjaStone)           //Hamon charging stuff
            {
                maxHamon *= 2;
                maxHamonCounter = 120;
            }
            if (Mplayer.Vampire)
            {
                HamonCounter = 0;
                hamonIncreaseCounter = 0;
            }

            if (hamonIncreaseCounter >= maxHamonCounter)      //the one that increases Hamon
            {
                if (AjaStone)       //or any other decrease-preventing accessories
                {
                    hamonIncreaseCounter = 0;
                    HamonCounter += 1;
                }
                if (HamonCounter < 60)
                {
                    hamonIncreaseCounter = 0;
                    HamonCounter += 1;
                }
            }
            if (hamonIncreaseCounter >= maxHamonCounter && HamonCounter > 60 && !AjaStone)      //the one that decreases Hamon
            {
                hamonIncreaseCounter = 0;
                HamonCounter -= 1;
            }
            if (!Mplayer.Vampire && player.breath == player.breathMax && HamonCounter <= 60)       //in general, to increase Hamon while it can still be increased, no speeding up decreasing
            {
                if (player.velocity != Vector2.Zero)
                {
                    hamonIncreaseCounter++;
                }
                else
                {
                    hamonIncreaseCounter += 2;
                }
                hamonIncreaseCounter += hamonIncreaseBonus;
                if (player.lavaWet && !player.lavaImmune)
                {
                    hamonIncreaseCounter--;
                }
            }
            if (!AjaStone)          //list maxHamonCounter decreasing things in here
            {
                maxHamonCounter = 240;
            }
            if (HamonCounter > 60 && HamonCounter < 120 && !AjaStone)      //every 6 seconds, while Hamon is at the UI's second row, it decreases. Only if you don't have the Aja Stone
            {
                maxHamonCounter = 360;
            }
            if (HamonCounter >= 120 && !AjaStone)      //same as top but every 3 seconds
            {
                maxHamonCounter = 180;
            }

            if (HamonCounter >= maxHamon)       //hamon limit stuff
            {
                HamonCounter = maxHamon;
            }
            if (HamonCounter <= 0)
            {
                HamonCounter = 0;
            }
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
        }
    }
}