using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonPlayer : ModPlayer
    {
        public static HamonPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<HamonPlayer>();
        }
        public float hamonDamageBoosts = 1f;
        public float hamonKnockbackBoosts = 1f;
        public int hamonLevel = 0;
        public int HamonCounter = 0;
        public int maxHamon = 60;
        public int counter = 0;
        public int maxHamonCounter = 0;

        public bool AjaStone = false;


        public override void ResetEffects()
        {
            ResetVariables();
        }

        public override void UpdateDead()
        {
            ResetVariables();
        }

        public override void PreUpdate()
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (NPC.downedBoss1)
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


            if (hamonLevel == 1)
            {
                hamonPlayer.maxHamon = 72;
            }
            if (hamonLevel == 2)
            {
                hamonPlayer.maxHamon = 84;
            }
            if (hamonLevel == 3)
            {
                hamonPlayer.maxHamon = 96;
            }
            if (hamonLevel == 4)
            {
                hamonPlayer.maxHamon = 108;
            }
            if (hamonLevel == 5)
            {
                hamonPlayer.maxHamon = 120;
            }
            if (hamonLevel == 6)
            {
                hamonPlayer.maxHamon = 132;
            }
            if (hamonLevel == 7)
            {
                hamonPlayer.maxHamon = 144;
            }
            if (hamonLevel == 8)
            {
                hamonPlayer.maxHamon = 156;
            }
            if (hamonLevel == 9)
            {
                hamonPlayer.maxHamon = 168;
            }
            if (hamonLevel == 10)
            {
                hamonPlayer.maxHamon = 180;
            }

            if (AjaStone)           //Hamon charging stuff
            {
                maxHamon *= 2;
                maxHamonCounter = 120;
            }
            if (Mplayer.Vampire)
            {
                HamonCounter = 0;
                counter = 0;
            }
            if (counter >= maxHamonCounter)      //the one that increases Hamon
            {
                if (AjaStone)       //or any other decrease-preventing accessories
                {
                    counter = 0;
                    HamonCounter += 1;
                }
                if (HamonCounter <= 60)
                {
                    counter = 0;
                    HamonCounter += 1;
                }
            }
            if (counter >= maxHamonCounter && HamonCounter > 60 && !AjaStone)      //the one that decreases Hamon
            {
                counter = 0;
                HamonCounter -= 1;
            }
            if (!Mplayer.Vampire && player.breath == player.breathMax && HamonCounter <= 60)       //in general, to increase Hamon while it can still be increased, no speeding up decreasing
            {
                if (player.velocity.X != 0f || player.velocity.Y != 0f)
                {
                    counter++;
                }
                if (player.velocity.X == 0f && player.velocity.Y == 0f)
                {
                    counter += 2;
                }
                if (player.lavaWet && !player.lavaImmune)
                {
                    counter--;
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

        private void ResetVariables()
        {
            hamonDamageBoosts = 1f;
            hamonKnockbackBoosts = 1f;
        }
    }
}