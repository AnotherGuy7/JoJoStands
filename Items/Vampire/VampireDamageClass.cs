using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public abstract class VampireDamageClass : ModItem
    {
        public virtual void SafeSetDefaults()
        {}

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
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