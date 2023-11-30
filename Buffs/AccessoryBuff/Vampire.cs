using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Vampire : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vampire");
            // Description.SetDefault("You are now a vampire... Stay away from the sun!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.moveSpeed *= 1.5f;
            player.noFallDmg = true;
            player.jumpBoost = true;
            player.manaRegen += 4;
            player.statDefense *= 1.25f;
            player.buffTime[buffIndex] = 2;
            player.GetDamage(DamageClass.Generic) *= 1.5f;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.5f;

            Vector3 lightLevel = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && (player.ZoneOverworldHeight || player.ZoneSkyHeight) && Main.tile[(int)player.Center.X / 16, (int)player.Center.Y / 16].WallType == 0)
                player.AddBuff(ModContent.BuffType<Sunburn>(), 2);
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            Vector3 lightLevel = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].WallType == 0)
                npc.AddBuff(ModContent.BuffType<Sunburn>(), 2);
        }
    }
}