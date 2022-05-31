using JoJoStands.Projectiles.Minions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class StrayCat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stray Cat");
            Tooltip.SetDefault("An odd plant that is somehow a cat. It can fire bubbles invisible to the eye and is capable of causing meowsive damage.");
        }

        public override void SetDefaults()
        {
            Item.damage = 104;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.maxStack = 1;
            Item.knockBack = 2f;
            Item.value = 0;
            Item.noUseGraphic = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.shoot = ModContent.ProjectileType<MatureStrayCatMinion>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                Item.shoot = ProjectileID.None;
            else
                Item.shoot = ModContent.ProjectileType<MatureStrayCatMinion>();
            return true;
        }
    }
}
