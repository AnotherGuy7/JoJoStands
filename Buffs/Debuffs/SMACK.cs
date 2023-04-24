using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class SMACK : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("SMACK!");
            // Description.SetDefault("It's just unbearable! The sounds in your head are getting louder!");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.echoesSoundIntensity > mPlayer.echoesSoundIntensityMax)
                mPlayer.echoesSoundIntensity = mPlayer.echoesSoundIntensityMax;
            if (mPlayer.echoesSmackDamageTimer > 0)
                mPlayer.echoesSmackDamageTimer--;
            if (mPlayer.echoesSmackDamageTimer == 0)
            {
                mPlayer.echoesSmackDamageTimer = 120;
                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                float volume = 0.6f;
                int soundDamage = (int)(mPlayer.echoesSoundIntensity * mPlayer.echoesDamageBoost) / 2;
                volume *= 4;
                soundDamage *= 2;

                punchSound.Volume = volume;
                punchSound.Pitch = 0f;
                punchSound.PitchVariance = 0.2f;
                SoundEngine.PlaySound(punchSound, player.Center);
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " could no longer live."), (int)Main.rand.NextFloat((int)(soundDamage * 0.85f), (int)(soundDamage * 1.15f)) + player.statDefense, 0, true, false, false);
                if (Main.rand.NextFloat(1, 100) <= 15)
                    player.AddBuff(BuffID.Confused, 180);
            }

            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }

        public override void OnBuffEnd(Player player)
        {
            player.GetModPlayer<MyPlayer>().echoesSmackDamageTimer = 120;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            JoJoGlobalNPC jojoNPC = npc.GetGlobalNPC<JoJoGlobalNPC>();
            if (jojoNPC.echoesSoundIntensity > jojoNPC.echoesSoundMaxIntensity)
                jojoNPC.echoesSoundIntensity = jojoNPC.echoesSoundMaxIntensity;
            if (jojoNPC.echoesSmackDamageTimer > 0)
                jojoNPC.echoesSmackDamageTimer--;

            int defence = jojoNPC.echoesSmackCritChance ? 4 : 2;
            if (jojoNPC.echoesSmackDamageTimer <= 0)
            {
                jojoNPC.echoesSmackDamageTimer += 60;
                SoundStyle punchSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Punch_land");
                int soundDamage = (int)(jojoNPC.echoesSoundIntensity * jojoNPC.echoesDamageBoost);
                float volume = 0.6f;
                volume *= 4;
                soundDamage *= 2;

                punchSound.Volume = volume;
                punchSound.Pitch = 0f;
                punchSound.PitchVariance = 0.2f;
                SoundEngine.PlaySound(punchSound, npc.Center);
                jojoNPC.echoesSmackCritChance = Main.rand.NextFloat(1, 100 + 1) <= jojoNPC.echoesCrit;
                int soundDamage2 = (int)Main.rand.NextFloat((int)(soundDamage * 0.85f), (int)(soundDamage * 1.15f)) + npc.defense / defence;
                npc.StrikeNPC(soundDamage2, 0f, 0, jojoNPC.echoesSmackCritChance);
                if (Main.rand.NextFloat(1, 100) <= 15 && !npc.boss)
                    npc.AddBuff(BuffID.Confused, 300);

                if (jojoNPC.echoesDebuffOwner != -1)
                {
                    Player ownerDebuff = Main.player[jojoNPC.echoesDebuffOwner];
                    MyPlayer modOwnerDebuff = ownerDebuff.GetModPlayer<MyPlayer>();
                    if (ownerDebuff.dead || !ownerDebuff.active)
                        jojoNPC.echoesDebuffOwner = -1;
                    if (npc.type == NPCID.Golem || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead)
                    {
                        if (jojoNPC.echoesSmackCritChance)
                            modOwnerDebuff.echoesACT3EvolutionProgress += soundDamage2 * 2;
                        if (!jojoNPC.echoesSmackCritChance)
                            modOwnerDebuff.echoesACT3EvolutionProgress += soundDamage2;
                    }
                    if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer)
                    {
                        if (jojoNPC.echoesSmackCritChance)
                            modOwnerDebuff.echoesACT2EvolutionProgress += soundDamage2 * 2;
                        if (!jojoNPC.echoesSmackCritChance)
                            modOwnerDebuff.echoesACT2EvolutionProgress += soundDamage2;
                    }
                }
            }
        }

        public override void OnBuffEnd(NPC npc)
        {
            npc.GetGlobalNPC<JoJoGlobalNPC>().echoesSmackDamageTimer = 60;
        }
    }
}