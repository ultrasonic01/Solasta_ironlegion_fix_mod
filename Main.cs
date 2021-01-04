using System;
using System.Reflection;
using UnityModManagerNet;
using HarmonyLib;
using I2.Loc;

namespace Solasta_IronLegion_Fix
{
    public class Main
    {
        [System.Diagnostics.Conditional("DEBUG")]
        public static void Log(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        public static void Error(Exception ex)
        {
            if (logger != null) logger.Error(ex.ToString());
        }
        public static void Error(string msg)
        {
            if (logger != null) logger.Error(msg);
        }

        public static UnityModManager.ModEntry.ModLogger logger;
        public static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;
                var harmony = new Harmony(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
            return true;
        }
        [HarmonyPatch(typeof(MainMenuScreen), "RuntimeLoaded")]
        static class MainMenuScreen_RuntimeLoaded_Patch
        {
            static void Postfix()
            {
                try
                {
                    // Open Iron Legion feat data
                    // var ironlegionfeat = DatabaseRepository.GetDatabase<FeatDefinition>().GetElement("MightOfTheIronLegion");
                    var ironlegionarmor = DatabaseRepository.GetDatabase<FeatureDefinitionProficiency>().GetElement("ProficiencyMightOfTheIronLegionArmor");
                    var HAcategory = DatabaseRepository.GetDatabase<ArmorCategoryDefinition>().GetElement("HeavyArmorCategory");
                    // var existingproftype = ironlegionarmor.ProficiencyType;
                    // var existingprof = ironlegionarmor.Proficiencies;
                    var newproficiency = new FeatureDefinitionProficiency();
                    AccessTools.Field(ironlegionarmor.GetType(), "proficiencyType").SetValue(newproficiency, 2);
                    AccessTools.Field(ironlegionarmor.GetType(), "proficiencies").SetValue(newproficiency, HAcategory);
                }
                catch (Exception ex)
                {
                    Error(ex);
                    throw;
                }
            }
        }
    }
}



