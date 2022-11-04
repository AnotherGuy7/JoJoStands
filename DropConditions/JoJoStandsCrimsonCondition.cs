using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class JoJoStandsCrimsonCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in the Crimson";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return info.player.ZoneCrimson;

            return false;
        }
    }
}
