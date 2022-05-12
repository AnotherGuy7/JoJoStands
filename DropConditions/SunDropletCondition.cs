using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class SunDropletCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops during daytime";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return Main.dayTime;

            return false;
        }
    }
}
