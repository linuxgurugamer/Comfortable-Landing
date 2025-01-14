﻿using UnityEngine;
using System;
using System.Linq;
using KSP.Localization;

namespace ComfortableLanding
{

    public class CL_ControlTool_Internal : PartModule
    {

        [KSPField]
        public bool IsActivate = false;
        [KSPField]
        public bool alreadyFired = false;
        [KSPField]
        public bool burnCompleted = false;
        [KSPField]
        public bool alreadyInflated = false;
        [KSPField]
        public bool alreadyInflatedAirBag = false;
        [KSPField]
        public bool alreadyDeflatedAirBag = false;

        [KSPField]
        internal bool automaticActivation = true;
        [KSPField]
        internal float originalBuoyancy = -1.0f;

        [KSPField]
        public Vector3 COMBeforeInflated = new Vector3(0.0f, 0.0f, 0.0f);
        [KSPField]
        public Vector3 COBAfterInflated = new Vector3(0.0f, 0.0f, 0.0f);
        [KSPField]
        public Vector3 COMAfterInflated = new Vector3(0.0f, 0.0f, 0.0f);
        [KSPField]
        public bool changeCOM = false;
        [KSPField]
        public Vector3 originalCOB = new Vector3(0.0f, 0.0f, 0.0f);

        [KSPField(guiName = "Buoyancy After Inflated", guiFormat = "F1", guiActive = true, isPersistant = true), UI_FloatRange(minValue = 0.1f, maxValue = 2.0f, stepIncrement = 0.1f)]
        public float buoyancyAfterInflated = 1.2f;


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


        [KSPEvent(guiName = "Enable automatic activation", active = true, guiActive = false)]
        public void EnableAutomaticActivation()
        {
            DoEnableAutomaticActivation();
        }

        [KSPEvent( guiName = "Disable automatic activation", active = true, guiActive = true)]
        public void DisableAutomaticActivation()
        {
            DoDisableAutomaticActivation();
        }

        [KSPAction("Toggle automatic activation", KSPActionGroup.None)]
        public void ToggleAutomaticActivation(KSPActionParam param)
        {
            automaticActivation = !automaticActivation;
        }

        void DoEnableAutomaticActivation()
        {
            automaticActivation = true;
            Events["EnableAutomaticActivation"].guiActive = false;
            Events["DisableAutomaticActivation"].guiActive = true;
        }
        void DoDisableAutomaticActivation()
        {
            automaticActivation = false;
            Events["EnableAutomaticActivation"].guiActive = true;
            Events["DisableAutomaticActivation"].guiActive = false;
        }

        [KSPEvent(guiName = "Apply Buoyancy Setting", active = true, guiActive = true)]
        public void ApplyBuoyancySetting()
        {
            if (originalBuoyancy == -1)
                originalBuoyancy = this.part.buoyancy;
            this.part.buoyancy = originalBuoyancy *  buoyancyAfterInflated;
            this.part.CenterOfBuoyancy = COBAfterInflated;
            if (changeCOM == true)
            {
                COMBeforeInflated = this.part.CoMOffset;
                this.part.CoMOffset = COMAfterInflated;
            }
        }
        public void ResetBuoyanceSetting()
        {
            this.part.buoyancy = originalBuoyancy;
            this.part.CenterOfBuoyancy = originalCOB;
            if (changeCOM == true)
            {
                this.part.CoMOffset = COMBeforeInflated;
            }

        }
        //*/

        public void Start()
        {
            GameEvents.onStageSeparation.Add(onStageSeparation);
            if (!HighLogic.CurrentGame.Parameters.CustomParams<CL_Settings>().automaticActivation)
            {
                DisableAutomaticActivation();
            }
            if (!HighLogic.CurrentGame.Parameters.CustomParams<CL_Settings>().debugMode)
             {
                Fields["buoyancyAfterInflated"].guiActive = false;
                Events["ApplyBuoyancySetting"].guiActive = false;
            }
        }
        public void OnDestroy()
        {
            GameEvents.onStageSeparation.Remove(onStageSeparation);
        }

        AttachNode GetNode(string nodeId, Part p)
        {
            foreach (var attachNode in p.attachNodes.Where(an => an != null))
            {
                if (p.srfAttachNode != null && attachNode == p.srfAttachNode)
                    continue;
                if (attachNode.id == nodeId)
                    return attachNode;

            }
            return null;
        }

        void onStageSeparation(EventReport er)
        {
            if (HighLogic.CurrentGame.Parameters.CustomParams<CL_Settings>().automaticActivation && automaticActivation)
            {
                var bottomNode = GetNode("bottom", this.part);
                if (bottomNode != null)
                {
                    if (bottomNode.attachedPart == null)
                    {
                        if (!vessel.LandedOrSplashed)
                        {
                            Activate();
                            ScreenMessages.PostScreenMessage("<color=#00ff00ff>Comfortable Landing Auto-Activated</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
                        }
                    }
                }
            }
        }

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