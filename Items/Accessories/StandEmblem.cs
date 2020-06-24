using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
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
            item.width = 30;
            item.height = 30;
            item.accessory = true;
            item.rare = ItemRarityID.LightRed;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            Mplayer.standDamageBoosts += 0.15f;
        }
    }
}