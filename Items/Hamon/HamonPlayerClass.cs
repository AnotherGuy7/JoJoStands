using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    // This class stores necessary player info for our custom damage class, such as damage multipliers and additions to knockback and crit.
    public class HamonPlayerClass : ModPlayer
    {
        public static HamonPlayerClass ModPlayer(Player player)
        {
            return player.GetModPlayer<HamonPlayerClass>();
        }

        // Vanilla only really has damage multipliers in code
        // And crit and knockback is usually just added to
        // As a modder, you could make separate variables for multipliers and simple addition bonuses
        public float HamonDamage = 1f;
        public float HamonKnockback;
        public int HamonCrit;
        public int hamonLevel;

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
            if (NPC.downedBoss1)    //eye of cthulu. I had to do this with levels cause I couldn't get it to only add to an amount, instead, it continuously added it
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 72;
            }
            if (NPC.downedBoss2)      //the crimson/corruption bosses
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 84;
            }
            if (NPC.downedBoss3)       //skeletron
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 96;
            }
            if (Main.hardMode)      //wall of flesh
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 108;
            }
            if (NPC.downedMechBoss1)
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 120;
            }
            if (NPC.downedMechBoss2)
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 132;
            }
            if (NPC.downedMechBoss3)
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 144;
            }
            if (NPC.downedPlantBoss)       //plantera
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 156;
            }
            if (NPC.downedGolemBoss)
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 168;
            }
            if (NPC.downedMoonlord)     //you are an expert with hamon by moon lord
            {
                player.GetModPlayer<MyPlayer>().maxHamon = 180;
            }
            base.PreUpdate();
        }

        private void ResetVariables()
        {
            HamonDamage = 1f;
            HamonKnockback = 0f;
            HamonCrit = 0;
        }
    }
}