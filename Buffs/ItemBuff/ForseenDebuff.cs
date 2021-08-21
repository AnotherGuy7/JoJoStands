using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class ForeseenDebuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Foreseen");
            Description.SetDefault("Your actions have already been seen...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
            Main.buffNoTimeDisplay[Type] = true;
        }

        public Vector2[] savePositions = new Vector2[50];
        public int savePositionsIndex = -1;
        public int savePositionsMaxIndex = -1;
        public int saveTimer = 0;
        public bool foresightWoreOff = false;

        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            player.AddBuff(mod.BuffType(Name), 2);
            if (player.HasBuff(mod.BuffType(Name)) && mPlayer.epitaphForesightActive)
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
            if (player.HasBuff(mod.BuffType(Name)) && !mPlayer.epitaphForesightActive)
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
                {
                    player.ClearBuff(mod.BuffType(Name));
                }
            }
            if (savePositionsIndex >= 49)
            {
                savePositionsIndex = 0;
                savePositionsMaxIndex = 0;
            }
        }
    }
}