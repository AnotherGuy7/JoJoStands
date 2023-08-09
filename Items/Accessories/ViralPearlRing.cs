using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class ViralPearlRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Viral Pearl Ring");
            // Tooltip.SetDefault("Right Click to remove the Pearl. Be careful not to break it!");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 2);
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
                int projIndex = Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<ViralPearlProjectile>(), 0, 0f, player.whoAmI);
                Main.projectile[projIndex].netUpdate = true;
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