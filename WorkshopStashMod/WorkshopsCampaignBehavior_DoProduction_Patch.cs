using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using System.Reflection.Emit;
using System.Reflection;

namespace WorkshopStashMod
{
    [HarmonyPatch(typeof(WorkshopsCampaignBehavior), "DoProduction")]
    public class WorkshopsCampaignBehavior_DoProduction_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructionsIn)
        {
            var sufficientInputsMethod = typeof(WorkshopsCampaignBehavior).GetMethod("DetermineTownHasSufficientInputs", BindingFlags.Static | BindingFlags.NonPublic);

            foreach (var instruction in instructionsIn)
            {
                if (instruction.Calls(sufficientInputsMethod))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    instruction.operand = typeof(WorkshopsCampaignBehavior_DoProduction_Patch).GetMethod("DetermineTownHasSufficientInputsReplacement", BindingFlags.Static | BindingFlags.Public);
                }
                yield return instruction;
            }
        }

        public static bool DetermineTownHasSufficientInputsReplacement(WorkshopType.Production production, Town town, out int inputMaterialCost, Workshop workshop)
        {
            ItemRoster stashRoster = null;
            if (workshop.Owner == Hero.MainHero)
            {
                var stash = MBObjectManager.Instance.GetObject<TownWorkshopStash>(x => x.Town == town);

                if (stash != null && stash.InputFromStash)
                {
                    stashRoster = stash.Stash;
                }
            }

            IReadOnlyList<ValueTuple<ItemCategory, int>> inputs = production.Inputs;
            inputMaterialCost = 0;
            foreach (ValueTuple<ItemCategory, int> valueTuple in inputs)
            {
                ItemCategory itemCategory = valueTuple.Item1;
                int val1 = valueTuple.Item2;

                if (stashRoster != null)
                {
                    for (int index = 0; index < stashRoster.Count; ++index)
                    {
                        ItemObject itemAtIndex = stashRoster.GetItemAtIndex(index);
                        if (itemAtIndex.ItemCategory == itemCategory)
                        {
                            int elementNumber = stashRoster.GetElementNumber(index);
                            int num = Math.Min(val1, elementNumber);
                            val1 -= num;
                        }
                    }
                }

                ItemRoster itemRoster = town.Owner.ItemRoster;
                for (int index = 0; index < itemRoster.Count; ++index)
                {
                    ItemObject itemAtIndex = itemRoster.GetItemAtIndex(index);
                    if (itemAtIndex.ItemCategory == itemCategory)
                    {
                        int elementNumber = itemRoster.GetElementNumber(index);
                        int num = Math.Min(val1, elementNumber);
                        val1 -= num;
                        inputMaterialCost += town.GetItemPrice(itemAtIndex, null, false) * num;
                    }
                }
                if (val1 > 0)
                    return false;
            }
            return true;
        }
    }
}
