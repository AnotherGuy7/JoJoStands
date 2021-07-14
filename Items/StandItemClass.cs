using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StandItemClass : ModItem
    {
        /// <summary>
        /// The Stands speed. (in frames)
        /// </summary>
        public virtual int standSpeed { get; }
        /// <summary>
        /// The Stands type. 
        /// </summary>
        public virtual int standType { get; } = 0;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            int speed = standSpeed - mPlayer.standSpeedBoosts;
            string speedType = "";
            if (standType == 1)
            {
                speedType = "Punch Speed: ";
            }
            if (standType == 2)
            {
                speedType = "Shoot Speed: ";
            }
            if (speed <= 2)
            {
                speed = 2;
            }
            if (standType != 0)
            {
                TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", speedType + speed);
                tooltips.Add(tooltipAddition);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = (int)player.GetModPlayer<MyPlayer>().standCritChangeBoosts;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.StandSlot.Item.type != 0)
            {
                player.QuickSpawnItem(mPlayer.StandSlot.Item.type);
                mPlayer.StandSlot.Item.type = item.type;
                mPlayer.StandSlot.Item.SetDefaults(item.type);
            }
            if (mPlayer.StandSlot.Item.type == 0)
            {
                mPlayer.StandSlot.Item.type = item.type;
                mPlayer.StandSlot.Item.SetDefaults(item.type);
            }
        }
    }
}
