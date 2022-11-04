using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class SiliconLifeformCarapace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silicon Lifeform Carapace");
            Tooltip.SetDefault("25% increased Stand dodge chance\n100% increased stand defence bonus\nLife is slowly drains");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 4);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().standDodgeBoosts += 25f;
            player.GetModPlayer<MyPlayer>().siliconLifeformCarapace = true;
        }
    }
}