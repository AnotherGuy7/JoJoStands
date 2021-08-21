using Terraria;
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
            item.damage = 104;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
            item.shoot = mod.ProjectileType("MatureStrayCatMinion");
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
                item.shoot = 0;
            else
                item.shoot = mod.ProjectileType("MatureStrayCatMinion");
            return true;
        }
    }
}
