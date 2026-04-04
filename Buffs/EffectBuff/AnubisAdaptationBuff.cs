using JoJoStands.Projectiles.PlayerStands.Anubis;
using Terraria;
using Terraria.ModLoader;
namespace JoJoStands.Buffs.EffectBuff
{
    public class AnubisAdaptationBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Keep the buff alive as long as the stand is out.
            // The actual stat application happens in AnubisStand.AI()
            // via MyPlayer fields — nothing needed here except the
            // duration refresh, which the stand handles each frame.
        }

        public override void ModifyBuffText(ref string name, ref string tip, ref int rare)
        {
            Player player = Main.LocalPlayer;
            int stacks = 0;
            int maxStacks = 0;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (!p.active || p.owner != player.whoAmI) continue;
                if (p.ModProjectile is AnubisStand anubis)
                {
                    stacks = anubis.AdaptationStacks;
                    maxStacks = anubis.MaxAdaptationStacks;
                    break;
                }
            }

            float dmgBonus = stacks * 0.01f * 100f;
            float speedBonus = stacks * 0.008f * 100f;
            float critBonus = stacks * 0.20f;

            tip = $"Melee damage: +{dmgBonus:0}%\n" +
                  $"Attack speed: +{speedBonus:0.0}%\n" +
                  $"Crit chance:  +{critBonus:0.0}%\n" +
                  $"Adaptation stacks: {stacks} / {maxStacks}";
        }
    }
}