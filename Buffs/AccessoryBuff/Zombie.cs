using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.Vampire;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Zombie : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Zombie");
            Description.SetDefault("You are now a zombie!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.moveSpeed *= 1.2f;
            player.jumpBoost = true;
            player.manaRegen += 2;
            player.statDefense = (int)(player.statDefense / 0.75);
            vPlayer.zombie = true;
            vPlayer.anyMaskForm = true;
            player.buffTime[buffIndex] = 2;
            player.GetDamage(DamageClass.Generic) *= 1.1f;
            player.GetAttackSpeed(DamageClass.Generic) *= 1.1f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Vector3 lightLevel = Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16).ToVector3();
            if (lightLevel.Length() > 1.3f && Main.dayTime && Main.tile[(int)npc.Center.X / 16, (int)npc.Center.Y / 16].WallType == 0)
                npc.AddBuff(ModContent.BuffType<Sunburn>(), 2);
        }
    }
}