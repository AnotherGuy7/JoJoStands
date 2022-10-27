using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class SoothingSpiritDisc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soothing Spirit Disc");
            Tooltip.SetDefault("Each stand attack that is a prime number deals additional damage depending on the player's health (maximum bonus - 60%)\nDamage reduced by 15% at full health");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().soothingSpiritDisc = true;
        }
    }
}