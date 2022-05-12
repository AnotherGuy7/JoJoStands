using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vampire
{
    public abstract class VampireDamageClass : ModItem
    {
        public virtual void SafeSetDefaults()
        { }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tooltip != null)
            {
                string[] splitText = tooltip.Text.Split(' ');
                tooltip.Text = splitText.First() + " Vampiric " + splitText.Last();
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            damage *= vPlayer.vampiricLevel * 0.12f;
            damage *= vPlayer.vampiricDamageMultiplier;
        }

        public override void ModifyWeaponKnockback(Player player, ref StatModifier knockback)
        {
            knockback *= player.GetModPlayer<VampirePlayer>().vampiricKnockbackMultiplier;
        }
    }
}