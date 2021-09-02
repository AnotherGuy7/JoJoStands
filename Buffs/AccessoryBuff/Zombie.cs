using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using JoJoStands.Items.Vampire;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Zombie : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Zombie");
            Description.SetDefault("You are now a zombie!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
            canBeCleared = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.allDamage *= 1.1f;
            player.moveSpeed *= 1.2f;
            player.meleeSpeed *= 1.1f;
            player.jumpBoost = true;
            player.manaRegen += 2;
            player.statDefense = (int)(player.statDefense / 0.75);
            player.GetModPlayer<VampirePlayer>().zombie = true;
            player.buffTime[buffIndex] = 2;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Vector3 lightLevel = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].wall == 0)
            {
                npc.AddBuff(mod.BuffType("Sunburn"), 2);
            }
        }
    }
}