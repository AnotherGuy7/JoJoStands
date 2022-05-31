using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class HamonEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon Emblem");
            Tooltip.SetDefault("15% increased hamon damage");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Hamon.HamonPlayer hamonPlayer = player.GetModPlayer<Hamon.HamonPlayer>();
            hamonPlayer.hamonDamageBoosts += 0.15f;
        }
    }
}