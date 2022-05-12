using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.Audio;
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
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.rare = 2;
            Item.value = Item.buyPrice(0, 0, 0, 20);
        }

        private int clickTimer = 0;

        public override void HoldItem(Player player)
        {
            HamonPlayer hPlayer = player.GetModPlayer<HamonPlayer>();
            if (clickTimer > 0)
                clickTimer--;

            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.mouseRight && Item.stack > 20 && clickTimer <= 0)
                {
                    hPlayer.skillPointsAvailable += 1;
                    Item.stack -= 20;
                    clickTimer = 60;
                    SoundEngine.PlaySound(2, Style: 25, pitchOffset: -0.8f);
                }
            }
        }
    }
}