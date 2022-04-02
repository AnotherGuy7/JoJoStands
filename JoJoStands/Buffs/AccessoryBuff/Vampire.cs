using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using JoJoStands.Items.Vampire;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Vampire : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Vampire");
            Description.SetDefault("You are now a vampire... Stay away from the sun!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.allDamage *= 1.5f;
            player.moveSpeed *= 1.5f;
            player.meleeSpeed *= 1.5f;
            player.noFallDmg = true;
            player.jumpBoost = true;
            player.manaRegen += 4;
            player.statDefense = (int)(player.statDefense / 0.75);
            vPlayer.vampire = true;
            vPlayer.anyMaskForm = true;
            player.buffTime[buffIndex] = 2;
            if (player.HasBuff(mod.BuffType("Zombie")))
            {
                player.ClearBuff(mod.BuffType("Zombie"));
            }
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