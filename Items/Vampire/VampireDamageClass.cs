using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Items.Vampire
{
    public abstract class VampireDamageClass : ModItem
    {
        public virtual void SafeSetDefaults()
        {}

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            Item.melee = false;
            Item.ranged = false;
            Item.magic = false;
            Item.thrown = false;
            Item.summon = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tooltip != null)
            {
                string[] splitText = tooltip.text.Split(' ');
                tooltip.text = splitText.First() + " Vampiric " + splitText.Last();
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            mult *= vPlayer.vampiricLevel * 0.12f;
            mult *= vPlayer.vampiricDamageMultiplier;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            knockback *= player.GetModPlayer<VampirePlayer>().vampiricKnockbackMultiplier;
        }
    }
}