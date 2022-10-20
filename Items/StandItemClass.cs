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
        /// <summary>
        /// The stand's name.
        /// </summary>
        public virtual string standProjectileName { get; }
        /// <summary>
        /// The stand's tier.
        /// </summary>
        public virtual int standTier { get; } = 0;

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
                TooltipLine tooltipAddition = new TooltipLine(Mod, "Speed", speedType + speed);
                TooltipLine tooltipAddition2 = new TooltipLine(Mod, "Dodge", (int)mPlayer.standDodgeBoosts + "% dodge chance");
                tooltips.Add(tooltipAddition);
                tooltips.Add(tooltipAddition2);
            }
            if (standProjectileName == "Cream")
            {
                TooltipLine tooltipAdditionCream = new TooltipLine(Mod, "Warning", "Warning! Cream's abilities are extremely destructive to the area around!");
                tooltipAdditionCream.OverrideColor = Microsoft.Xna.Framework.Color.Red;
                tooltips.Add(tooltipAdditionCream);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit)
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
            if (mPlayer.StandSlot.SlotItem.type != 0)
            {
                player.QuickSpawnItem(player.GetSource_FromThis(), mPlayer.StandSlot.SlotItem.type);
                mPlayer.StandSlot.SlotItem.type = Item.type;
                mPlayer.StandSlot.SlotItem.SetDefaults(Item.type);
            }
            if (mPlayer.StandSlot.SlotItem.type == 0)
            {
                mPlayer.StandSlot.SlotItem.type = Item.type;
                mPlayer.StandSlot.SlotItem.SetDefaults(Item.type);
            }
        }

        /// <summary>
        /// Determines whether or not the Stand will spawn using normal spawn code.
        /// </summary>
        /// <returns>Whether or not this Item will override standard stand spawning code.</returns>
        public virtual bool ManualStandSpawning(Player player)
        {
            return false;
        }
    }
}
