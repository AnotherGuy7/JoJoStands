using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
 
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
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MyPlayer>().Vampire = true;
            player.AddBuff(mod.BuffType("Vampire"), 2, true);
            player.allDamage *= 1.5f;
            player.moveSpeed *= 1.5f;
            player.meleeSpeed *= 1.5f;
            player.noFallDmg = true;
            player.jumpBoost = true;
            player.manaRegen += 2;
            player.statDefense = (int)(player.statDefense / 0.75);

            Vector3 lightLevel = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3();     //from projectile aiStyle 67, line 21033 in Projectile.cs
            if (lightLevel.Length() > 1.3f  && Main.dayTime && player.ZoneOverworldHeight && Main.tile[(int)player.Center.X / 16, (int)player.Center.Y / 16].wall == 0)
            {
                player.AddBuff(mod.BuffType("Sunburn"), 2, true);
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