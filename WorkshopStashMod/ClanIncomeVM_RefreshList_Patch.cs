using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories;
using System.Reflection.Emit;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement;
using System;

namespace WorkshopStashMod
{
    [HarmonyPatch(typeof(ClanIncomeVM), "RefreshList")]
    public class ClanIncomeVM_RefreshList_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructionsIn)
        {
            foreach (var instruction in instructionsIn)
            {
                if (instruction.opcode == OpCodes.Newobj)
                {
                    var originalVMCtor = typeof(ClanFinanceWorkshopItemVM).GetConstructor(new Type[] { typeof(Workshop), typeof(Action<ClanFinanceIncomeItemBaseVM>), typeof(Action) });
                    if (instruction.operand == originalVMCtor)
                    {
                        instruction.operand = typeof(ClanFinanceWorkshopItemExpandedVM).GetConstructor(new Type[] { typeof(Workshop), typeof(Action<ClanFinanceIncomeItemBaseVM>), typeof(Action) });
                    }
                }

                yield return instruction;
            }
        }
    }
}
