using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class FrozeninTime : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frozen in Time");
            Description.SetDefault("You have been stopped along with time");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (player.HasBuff(Type))
            {
                player.controlUseItem = false;
                player.dashType = 0;
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
                player.maxRunSpeed = 0;
                player.moveSpeed = 0;
                player.mount._frameCounter = 2;
                if (JoJoStands.timestopOverrideStands.Contains(mPlayer.StandSlot.SlotItem.type))
                    mPlayer.ableToOverrideTimestop = true;
            }
            else
            {
                mPlayer.ableToOverrideTimestop = false;
                if (Main.netMode != NetmodeID.Server)
                    JoJoStandsShaders.DeactivateShader(JoJoStandsShaders.TimestopGreyscaleEffect);
            }
        }
    }
}