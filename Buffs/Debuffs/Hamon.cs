using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Hamon : ModBuff
    {

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Missing Organs!");
            Description.SetDefault("Parts of your body have been scraped away!");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Fire, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.lifeRegen -= 10;
        }
    }
}