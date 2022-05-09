using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Accessories
{
    public class StandEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stand Emblem");
            Tooltip.SetDefault("15% increased stand damage");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            Mplayer.standDamageBoosts += 0.15f;
        }
    }
}