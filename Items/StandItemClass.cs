using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StandItemClass : ModItem
    {
        /// <summary>
        /// The stand's tier.
        /// </summary>
        public virtual int StandTier { get; } = 0;
        /// <summary>
        /// The Stands type. 
        /// </summary>
        public virtual int StandType { get; } = 0;
        /// <summary>
        /// The Stands speed. (in frames)
        /// </summary>
        public virtual int StandSpeed { get; }
        /// <summary>
        /// The stand's name.
        /// </summary>
        public virtual string StandProjectileName { get; }
        /// <summary>
        /// The amount of tiers to offset the display by. A Display Offset of 1 would show the tier one tier higher than the StandTier value.
        /// </summary>
        public virtual int StandTierDisplayOffset { get; } = 0;
        /// <summary>
        /// The amount that will be added to the scale while drawing.
        /// </summary>
        public virtual float StandTierDisplayScaleOffset { get; } = 0f;
        public virtual Color StandTierDisplayColor { get; } = Color.White;
        public static Texture2D standTierNumerals;

        private readonly Point StandTierNumeralSize = new Point(26, 20); 
        private readonly Vector2 StandTierNumeralOrigin = new Vector2(13, 10);

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            int speed = StandSpeed - mPlayer.standSpeedBoosts;
            string speedType = "";
            if (StandType == 1)
                speedType = "Punch Speed: ";
            else if (StandType == 2)
                speedType = "Shoot Speed: ";

            if (speed <= 2)
                speed = 2;

            if (StandType != 0)
            {
                TooltipLine tooltipAddition = new TooltipLine(Mod, "Speed", speedType + speed);
                TooltipLine dodgeTooltipAddition = new TooltipLine(Mod, "Dodge", (int)mPlayer.standDodgeChance + "% dodge chance");
                tooltips.Add(tooltipAddition);
                tooltips.Add(dodgeTooltipAddition);
            }
            if (StandProjectileName == "Cream")
            {
                TooltipLine creamWariningTooltipAddition = new TooltipLine(Mod, "Warning", "Warning! Cream's abilities are extremely destructive to the area around!");
                creamWariningTooltipAddition.OverrideColor = Color.Red;
                tooltips.Add(creamWariningTooltipAddition);
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

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Rectangle numeralSourceRect = new Rectangle(0, (StandTier + StandTierDisplayOffset) * StandTierNumeralSize.Y, StandTierNumeralSize.X, StandTierNumeralSize.Y);
            spriteBatch.Draw(standTierNumerals, position + new Vector2(24f), numeralSourceRect, StandTierDisplayColor, 0f, StandTierNumeralOrigin, scale + StandTierDisplayScaleOffset, SpriteEffects.None, 0f);
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
