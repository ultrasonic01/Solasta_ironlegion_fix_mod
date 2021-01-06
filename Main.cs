using System;
using System.Reflection;
using System.Collections;
using UnityModManagerNet;
using HarmonyLib;
using UnityEngine;
using UnityEditor;
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
                    var ironlegionarmor = (FeatureDefinitionProficiency)DatabaseRepository.GetDatabase<FeatureDefinition>().GetElement("ProficiencyMightOfTheIronLegionArmor");
                    // var newproficiency = ScriptableObject.CreateInstance<FeatureDefinitionProficiency>();
                    // AccessTools.Field(ironlegionarmor.GetType(), "proficiencyType").SetValue(ironlegionarmor, 2); Line 57 is option 1 to edit proficiency type. 
                    var newproficiency2 = RuleDefinitions.ProficiencyType.Armor;
                    var newproflist = ironlegionarmor.Proficiencies;
                    AccessTools.Field(ironlegionarmor.GetType(), "proficiencyType").SetValue(ironlegionarmor, newproficiency2); // Line 58 and 60 is option 2 to edit proficiency type. 
                    // newproflist.Remove("LongswordType");
                    // newproflist.Remove("ShortswordType");
                    newproflist.Clear();
                    newproflist.Add("HeavyArmorCategory");
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



