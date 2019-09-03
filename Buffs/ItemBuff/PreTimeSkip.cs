using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class PreTimeSkip : ModBuff
    {
        public static int userIndex = -1;
        public static Vector2[] playerVelocity = new Vector2[255];

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
            userIndex = player.whoAmI;
            if (player.HasBuff(mod.BuffType("PreTimeSkip")))
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && !Main.player[i].HasBuff(mod.BuffType("PreTimeSkip")) && !Main.player[i].HasBuff(mod.BuffType("SkippingTime")))
                    {
                        playerVelocity[i] = Main.player[i].velocity;
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
                    }
                }
                player.GetModPlayer<MyPlayer>().TimeSkipPreEffect = true;
            }
            else
            {
                if (player.HeldItem.type == mod.ItemType("KingCrimsonT2"))
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 180);
                }
                if (player.HeldItem.type == mod.ItemType("KingCrimsonT3"))
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 300);
                }
                if (player.HeldItem.type == mod.ItemType("KingCrimsonFinal"))
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 600);
                }
                if (player.HeldItem.type != mod.ItemType("KingCrimsonT2") && player.HeldItem.type != mod.ItemType("KingCrimsonT3") && player.HeldItem.type != mod.ItemType("KingCrimsonFinal"))
                {
                    player.AddBuff(mod.BuffType("SkippingTime"), 180);
                }
                player.GetModPlayer<MyPlayer>().TimeSkipPreEffect = false;
            }

        }
    }
}