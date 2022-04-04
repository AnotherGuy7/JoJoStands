using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class SunDroplet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A warm droplet... It seems to react to you.\nRight-click while holding more than 20 to gain a Hamon SKill Point.\nUsed for creating Hamon weapons.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.rare = 2;
            item.value = Item.buyPrice(0, 0, 0, 20);
        }

        private int clickTimer = 0;

        public override void HoldItem(Player player)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (clickTimer > 0)
            {
                clickTimer--;
            }
            if (player.whoAmI == item.owner)
            {
                if (Main.mouseRight && item.stack > 20 && clickTimer <= 0)
                {
                    hPlayer.skillPointsAvailable += 1;
                    item.stack -= 20;
                    clickTimer = 60;
                    Main.PlaySound(2, Style: 25, pitchOffset: -0.8f);
                }
            }
        }
    }
}