using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.EffectBuff
{
    public class ForeseenDebuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foreseen");
            Description.SetDefault("Your actions have already been seen...");
            Main.debuff[Type] = true;       //so that it can't be canceled
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        private Vector2[] savePositions = new Vector2[50];
        private int savePositionsIndex = -1;
        private int savePositionsMaxIndex = -1;
        private int saveTimer = 0;
        private bool foresightWoreOff = false;

        public override void UpdateBuffOnPlayer(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.AddBuff(Type, 2);
            if (player.HasBuff(Type) && mPlayer.epitaphForesightActive && !mPlayer.forceShutDownEffect)
            {
                saveTimer++;
                if (saveTimer >= 30)
                {
                    savePositionsIndex++;
                    savePositionsMaxIndex++;
                    savePositions[savePositionsIndex] = player.position;
                    saveTimer = 0;
                }
            }
            if (player.HasBuff(Type) && !mPlayer.epitaphForesightActive)
            {
                if (!foresightWoreOff)
                {
                    foresightWoreOff = true;
                    savePositionsIndex = 0;
                }
                player.controlUp = false;
                player.controlDown = false;
                player.controlLeft = false;
                player.controlRight = false;
                player.bodyVelocity = Vector2.Zero;
                player.controlUseItem = false;
                player.gravity = 0f;
                saveTimer++;
                if (saveTimer >= 30)
                {
                    savePositionsIndex++;
                    saveTimer = 0;
                }
                player.position = savePositions[savePositionsIndex];
                if (savePositionsIndex == savePositionsMaxIndex)
                    player.ClearBuff(Type);
            }
            if (savePositionsIndex >= 49)
            {
                savePositionsIndex = 0;
                savePositionsMaxIndex = 0;
            }
        }
    }
}