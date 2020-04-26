using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace JoJoStands.Buffs.ItemBuff
{
    public class FrozeninTime : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Frozen in Time");
            Description.SetDefault("You have been stopped along with time");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType(Name)))
            {
                player.controlUseItem = false;
                player.dash *= 0;       //tried to get them gray but the stayed gray forever...Don't try again
                player.bodyVelocity = Vector2.Zero;
                player.controlLeft = false;
                player.controlJump = false;
                player.controlRight = false;
                player.controlDown = false;
                player.controlQuickHeal = false;
                player.controlQuickMana = false;
                player.controlRight = false;
                player.controlUseTile = false;
                player.controlUp = false;
                player.maxRunSpeed *= 0;
                player.moveSpeed *= 0;
            }
            else
            {
                if (Filters.Scene["GreyscaleEffect"].IsActive())
                {
                    Filters.Scene["GreyscaleEffect"].Deactivate();
                }
            }
        }
    }
}