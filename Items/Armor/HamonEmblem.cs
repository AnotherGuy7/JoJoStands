using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Armor
{
    public class HamonEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Emblem");
            Tooltip.SetDefault("15% increased hamon damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.accessory = true;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            hamonPlayer.hamonDamageBoosts += 0.15f;
        }
    }
}