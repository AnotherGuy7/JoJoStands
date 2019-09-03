using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
 
namespace JoJoStands.Buffs.NPCBuff
{
    public class AffectedByBtZ : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Back to Zero");
            Description.SetDefault("Your actions actually never happened at all!");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().affectedbyBtz = true;
            npc.AddBuff(mod.BuffType(Name), 1);
            int newDust = Dust.NewDust(npc.position, npc.width, npc.height, 220);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity = Vector2.Zero;
        }
    }
}