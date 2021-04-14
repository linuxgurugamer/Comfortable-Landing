using UnityEngine;
using System;
using KSP.Localization;
//Update for 1.4.1

namespace ComfortableLanding
{

    public class CL_ControlTool_Internal : PartModule
    {

        [KSPField]
        public bool IsActivate = false;
        [KSPField]
        public bool alreadyFired = false;
        [KSPField]
        public bool alreadyInflated = false;
        [KSPField]
        public bool alreadyInflatedAirBag = false;
        [KSPField]
        public bool alreadyDeflatedAirBag = false;

        [KSPEvent(name = "Activate", guiName = "Activate Pre-Landing Mode", active = true, guiActive = true)]
        public void Activate()
        {
            if (vessel.LandedOrSplashed)
                ScreenMessages.PostScreenMessage("<color=#00ff00ff>Cannot be activated while landed!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            else
            {
                IsActivate = true;
                Events["Deactivate"].guiActive = true;
                Events["Activate"].guiActive = false;
            }
        }

        [KSPEvent(name = "Deactivate", guiName = "Deactivate Pre-Landing Mode", active = true, guiActive = false)]
        public void Deactivate()
        {
            IsActivate = false;
            Events["Deactivate"].guiActive = false;
            Events["Activate"].guiActive = true;

        }

        [KSPAction("Toggle Pre-Landing Mode", KSPActionGroup.None)]
        public void ActionToggle(KSPActionParam param)
        {
            if (IsActivate == false)
            {
                if (vessel.LandedOrSplashed)
                    ScreenMessages.PostScreenMessage("<color=#00ff00ff>Cannot be activated while landed!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
                else
                    Activate();
            }
            else
            {
                Deactivate();
            }
        }

        [KSPAction("Activate Pre-Landing Mode", KSPActionGroup.None)]
        public void ActionActivate(KSPActionParam param)
        {
            if (vessel.LandedOrSplashed)
                ScreenMessages.PostScreenMessage("<color=#00ff00ff>Cannot be activated while landed!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            else
                Activate();
        }

        [KSPAction("Deactivate Pre-Landing Mode", KSPActionGroup.None)]
        public void ActionDeactivate(KSPActionParam param)
        {
            Deactivate();
        }

        //*/




        public void CheckLandedOrSplashed()
        {
            if (vessel.Landed || vessel.Splashed)
            {
                Events["Deactivate"].guiActive = false;
                Events["Activate"].guiActive = false;
                IsActivate = false;
                Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>The vessel has landed or splashed, deactivate pre-landing mode.");
            }
        }


    }
}