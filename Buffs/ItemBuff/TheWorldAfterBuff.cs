using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class TheWorldAfterBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("The World After Buff");
            Description.SetDefault("Time... has just finished stopping...");
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_stop"));
            if (player.HasBuff(mod.BuffType("TheWorldAfterBuff")))
            {
                MyPlayer.TheWorldEffect = false;
                MyPlayer.TheWorldAfterEffect = true;
            }
            else
            {
                player.AddBuff(mod.BuffType("TheWorldCoolDown"), 1800);
                MyPlayer.TheWorldAfterEffect = false;
            }
        }
    }
}