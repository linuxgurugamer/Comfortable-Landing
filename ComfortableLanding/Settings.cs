
using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using KSP.Localization;

namespace ComfortableLanding
{
    // http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
    // search for "Mod integration into Stock Settings

    public class CL_Settings : GameParameters.CustomParameterNode
    {
        public override string Title { get { return Localizer.Format("Comfortable Landing"); } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Comfortable Landing"; } }
        public override string DisplaySection { get { return ""; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return false; } }


        [GameParameters.CustomParameterUI("Enable Automatic Activation")]
        public bool automaticActivation = true;


        [GameParameters.CustomParameterUI("Debug mode", 
            toolTip ="Enables displaying the buoyancy adjustment value and a button to apply any changes, useful for debugging and configuring new values")]
        public bool debugMode = false;


        public override void SetDifficultyPreset(GameParameters.Preset preset) { }
        public override bool Enabled(MemberInfo member, GameParameters parameters) { return true; }
        public override bool Interactible(MemberInfo member, GameParameters parameters) { return true; }
        public override IList ValidValues(MemberInfo member) { return null; }
    }

}
