using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Accessories
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
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            hamonPlayer.hamonDamageBoosts += 0.15f;
        }
    }
}