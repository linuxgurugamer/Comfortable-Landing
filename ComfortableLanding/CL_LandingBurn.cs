using UnityEngine;
using System;
using KSP.Localization;

namespace ComfortableLanding
{

    public class CL_LandingBurn : CL_ControlTool_Internal
    {
        ModuleEngines engine;

        [KSPField]
        public double burnAltitude = 200.0;
        [KSPField]
        public bool triggered = false;

        public override void OnStart(PartModule.StartState state)
        {
            engine = part.Modules["ModuleEngines"] as ModuleEngines;
            if (engine == null)
                engine = part.Modules["ModuleEnginesFX"] as ModuleEngines;
            if (engine == null)
                engine = part.Modules["ModuleEnginesRF"] as ModuleEngines;
            if (engine == null)
                Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Engine Missing!");
        }
        public void Fire()
        {
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Landing Burn!");
            ScreenMessages.PostScreenMessage("<color=#00ff00ff>[ComfortableLanding]Landing Burn!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            engine.Activate();
        }

        public void FixedUpdate()
        {
            if (!IsActivate)
                return;
            if (alreadyFired == false)
            {
                if (BurnRay())
                {
                    alreadyFired = true;
                }
            }
            else
            {
                if (this.vessel.situation != Vessel.Situations.LANDED && this.vessel.situation != Vessel.Situations.SPLASHED && triggered)
                {
                    foreach (Part p in this.vessel.parts)
                    {
                        if ((p.physicalSignificance == Part.PhysicalSignificance.FULL) && (p.rb != null))
                        {
                            Vector3 gee = FlightGlobals.getGeeForceAtPosition(this.vessel.transform.position);
                            p.AddForce(-gee.normalized * p.rb.mass * ((float)Math.Min(Math.Abs(this.vessel.verticalSpeed), gee.magnitude) + gee.magnitude));
                        }
                    }
                    Debug.Log(this.vessel.srf_velocity.magnitude);
                }
            }
            CheckLandedOrSplashed();

        }

        public bool BurnRay()
        {
            if (vessel.radarAltitude <= burnAltitude)
            {
                RaycastHit hit;
                Ray rcray = new Ray(this.part.transform.position, FlightGlobals.getGeeForceAtPosition(this.vessel.transform.position));
                if (Physics.Raycast(rcray, out hit) && hit.distance < Math.Abs(this.vessel.verticalSpeed) / 2)
                {
                    Fire();
                    triggered = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}