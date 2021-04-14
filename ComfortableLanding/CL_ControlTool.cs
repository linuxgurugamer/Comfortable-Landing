using UnityEngine;
using System;
using KSP.Localization;
//Update for 1.4.1

public class CL_ControlTool : PartModule
{
    CL_LandingBurn burn;
    CL_Buoy buoy;
    CL_AirBag airbag;

    [KSPField]
    public bool IsActivate = false;
    [KSPField]
    private bool alreadyFired = false;
    [KSPField]
    private bool alreadyInflated = false;
    [KSPField]
    private bool alreadyInflatedAirBag = false;
    [KSPField]
    private bool alreadyDeflatedAirBag = false;

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

    public override void OnStart(PartModule.StartState state)
    {
        burn = part.Modules["CL_LandingBurn"] as CL_LandingBurn;
        if (burn == null)
        {
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Not detected CL_LandingBurn");
        }

        buoy = part.Modules["CL_Buoy"] as CL_Buoy;
        if (buoy == null)
        {
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Not detected CL_Buoy");
        }

        airbag = part.Modules["CL_AirBag"] as CL_AirBag;
        if (airbag == null)
        {
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Not detected CL_AirBag");
        }

        if (burn == null && buoy == null && airbag == null)
        {
            Events["Deactivate"].guiActive = false;
            Events["Activate"].guiActive = false;
            IsActivate = false;
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Not detected any CL Module.");
        }

        ///* Not yet resolved
        if (buoy != null && airbag != null)//They are repetitive.
        {
            airbag = null;//So it will not activate in FixedUpdate.
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Detected CL_Buoy and CL_ Airbag, only activate CL_Buoy.");
        }
        //*/

    }

    public void FixedUpdate()
    {
        if (IsActivate == true)
        {
            //Landing Burn
            if (burn != null)
            {
                if (alreadyFired == false)
                {
                    if (BurnRay())
                    {
                        alreadyFired = true;
                    }
                }
                else
                {
                    if (this.vessel.situation != Vessel.Situations.LANDED && this.vessel.situation != Vessel.Situations.SPLASHED && burn.triggered)
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
            }
            //Inflate Buoy
            if (buoy != null)
            {
                if (alreadyInflated == false)
                {
                    if (vessel.Splashed)
                    {
                        buoy.Inflate();
                        alreadyInflated = true;
                    }
                }
            }
            //Airbag
            if (airbag != null)
            {
                //Inflate Airbag
                if (alreadyInflatedAirBag == false)
                {
                    if (vessel.radarAltitude <= airbag.inflateAltitude)
                    {
                        airbag.Inflate();
                        alreadyInflatedAirBag = true;
                    }
                }
                //Deflate Airbag
                if (alreadyInflatedAirBag == true && alreadyDeflatedAirBag == false)
                {
                    if (vessel.Landed)
                    {
                        //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Landed!");
                        airbag.Deflate();
                        alreadyDeflatedAirBag = true;
                    }
                    else if (vessel.Splashed && airbag.damageAfterSplashed == true)
                    {
                        //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Splashed with damage");
                        airbag.Deflate();
                        alreadyDeflatedAirBag = true;
                    }
                    else if (vessel.Splashed && airbag.damageAfterSplashed == false)
                    {
                        //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Splashed with out damage");
                        airbag.Touchdown();
                        alreadyDeflatedAirBag = true;

                    }
                }
            }
            //Check
            if (vessel.Landed || vessel.Splashed)
            {
                Events["Deactivate"].guiActive = false;
                Events["Activate"].guiActive = false;
                IsActivate = false;
                Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>The vessel has landed or splashed, deactivate pre-landing mode.");
            }
        }
    }

    public bool BurnRay()
    {
        if (vessel.radarAltitude <= burn.burnAltitude)
        {
            RaycastHit hit;
            Ray rcray = new Ray(this.part.transform.position, FlightGlobals.getGeeForceAtPosition(this.vessel.transform.position));
            if (Physics.Raycast(rcray, out hit) && hit.distance < Math.Abs(this.vessel.verticalSpeed) / 2)
            {
                burn.Fire();
                burn.triggered = true;
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