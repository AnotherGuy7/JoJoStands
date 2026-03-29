using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.CraftingMaterials
{
    public class DormantVirusSample : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.buyPrice(0, 0, 7, 0);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(180, 80, 255, lightColor.A);
        }

        public override void PostUpdate()
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(Item.position, Item.width, Item.height, DustID.PurpleTorch);
                Main.dust[dust].velocity *= 0.4f;
                Main.dust[dust].scale = Main.rand.NextFloat(0.6f, 1.0f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = new Color(180, 80, 255);
            }
        }
    }
}