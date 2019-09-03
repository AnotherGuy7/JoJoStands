using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class SkippingTime : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Skipping Time");
            Description.SetDefault("Time is skipping");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("SkippingTime")))
            {
                player.immune = true;
                player.controlUseItem = false;
                player.AddBuff(BuffID.NightOwl, 2);
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].HasBuff(mod.BuffType("PreTimeSkip")) && !Main.player[i].HasBuff(mod.BuffType("SkippingTime")))
                    {
                        Main.player[i].velocity = PreTimeSkip.playerVelocity[i];
                        Main.player[i].AddBuff(BuffID.Obstructed, 2);
                        Main.player[i].controlUseItem = false;
                        Main.player[i].controlLeft = false;
                        Main.player[i].controlJump = false;
                        Main.player[i].controlRight = false;
                        Main.player[i].controlDown = false;
                        Main.player[i].controlQuickHeal = false;
                        Main.player[i].controlQuickMana = false;
                        Main.player[i].controlRight = false;
                        Main.player[i].controlUseTile = false;
                        Main.player[i].controlUp = false;
                        Main.player[i].controlMount = false;
                        Main.player[i].gravControl = false;
                        Main.player[i].gravControl2 = false;
                        Main.player[i].controlTorch = false;
                    }
                }
                player.GetModPlayer<MyPlayer>().TimeSkipEffect = true;
            }
            else
            {
                for (int i = 0; i < 255; i++)
                {
                    Array.Clear(PreTimeSkip.playerVelocity, i, 1);
                    if (Main.player[i].active && i != player.whoAmI)
                    {
                        Main.player[i].AddBuff(BuffID.Confused, 240);
                    }
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timeskip_end"));
                player.AddBuff(mod.BuffType("TimeCooldown"), 1800);
                player.GetModPlayer<MyPlayer>().TimeSkipEffect = false;
                PreTimeSkip.userIndex = -1;
            }
        }
    }
}