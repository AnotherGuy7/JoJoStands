using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using Terraria.ID;

namespace JoJoStands.Items.Accessories
{
    public class ViralPearlRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Pearl Ring>();
            Tooltip.SetDefault("Right Click to remove the Pearl. Be careful not to break it!>();
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.maxStack = 1;
            Item.consumable = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<ViralPearl>(), 0, 0f, player.whoAmI);
                Main.projectile[proj].netUpdate = true;
                Item.TurnToAir();
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}