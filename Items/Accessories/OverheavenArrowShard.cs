using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using JoJoStands.Items.Seasonal;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Balloon)]
    public class OverheavenArrowShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overheaven Arrow Shard");
            Tooltip.SetDefault("Cheats only\nUniquie effect on some stands\nEffect also works in vanity slot");
            SacrificeTotal = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 11;
            Item.rare = -13;
            Item.accessory = true;
            Item.vanity = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().overHeaven = true;
        }
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<MyPlayer>().overHeaven = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            string effect = "None";

            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CrazyDiamondT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CrazyDiamondT2>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CrazyDiamondT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CrazyDiamondFinal>())
                effect = "Healing abilities works on owner!";

            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheHandFinal>())
                effect = "Increased damage of scrape abilities!";

            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT1>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT2>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<CreamFinal>())
                effect = "Void Gauge regenerates faster and lasts longer!";

            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarPlatinumFinal>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<StarOnTheTree>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldFinal>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TheWorldT2>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonFinal>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonT3>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingCrimsonT2>() || mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<KingClaus>())
                effect = "Increased duration of time related abilites!";

            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<TuskAct4>())
                effect = "Always ACT 4!";

            TooltipLine tooltipAddition = new TooltipLine(Mod, "Effect", "Effect: " + effect);
            if (effect == "None")
                tooltipAddition.OverrideColor = Microsoft.Xna.Framework.Color.Red;
            if (effect != "None")
                tooltipAddition.OverrideColor = Microsoft.Xna.Framework.Color.Green;

            tooltips.Add(tooltipAddition);
        }
    }
}