using UnityEngine;
using KSP.Localization;


namespace ComfortableLanding
{

    public class CL_AirBag : CL_ControlTool_Internal
    {
        ModuleAnimateGeneric InflateAnim;

        AudioClip inflateSound;
        AudioSource audioSource;
        AudioClip deflateSound;
        AudioSource audioSource2;

        [KSPField]
        public double inflateAltitude = 50.0;
        [KSPField]
        public float crashToleranceAfterInflated = 45.0f;

        public string DeflateTransformName = "DeflateTransform";
        public Vector3 deflateScale = new Vector3(1.0f, 1.0f, 1.0f);
        public string InflateTransformName = "InflateTransform";
        public Vector3 inflateScale = new Vector3(0.1f, 0.1f, 0.1f);
        public bool damageAfterSplashed = true;
       // public float buoyancyAfterInflated = 1.2f;

        public string inflateSoundPath = "ComfortableLanding/Sounds/Inflate_A";
        public string deflateSoundPath = "ComfortableLanding/Sounds/Touchdown";

        public float volume = 1.0f;
        public float volume2 = 1.0f;

        private Transform DeflateTransform = null;
        private Transform InflateTransform = null;
        private float originalCrashTolerance = 0.0f;

        //public string animName = null;
        //public int animLayer = 0;

        public override void OnStart(StartState state)
        {
            InflateAnim = part.Modules["ModuleAnimateGeneric"] as ModuleAnimateGeneric;
            if (InflateAnim == null)
                Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Animation Missing!");
            /*
            foreach (ModuleAnimateGeneric anim in part.Modules)
            {
                if (anim.animationName == animName && anim.layer == animLayer)
                {
                    InflateAnim = anim;
                    break;
                }
            }
            */
            if (InflateAnim == null)
                Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Animation Missing!");

            DeflateTransform = base.part.FindModelTransform(DeflateTransformName);
            InflateTransform = base.part.FindModelTransform(InflateTransformName);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.bypassListenerEffects = true;
            audioSource.minDistance = .3f;
            audioSource.maxDistance = 1000;
            audioSource.priority = 10;
            audioSource.dopplerLevel = 0;
            audioSource.spatialBlend = 1;
            inflateSound = GameDatabase.Instance.GetAudioClip(inflateSoundPath);
            audioSource.clip = inflateSound;
            audioSource.loop = false;
            audioSource.time = 0;
            if (volume < 0)
            {
                volume = 0.0f;
            }
            else if (volume > 1)
            {
                volume = 1.0f;
            }
            audioSource.volume = volume;

            audioSource2 = gameObject.AddComponent<AudioSource>();
            audioSource2.bypassListenerEffects = true;
            audioSource2.minDistance = .3f;
            audioSource2.maxDistance = 1000;
            audioSource2.priority = 10;
            audioSource2.dopplerLevel = 0;
            audioSource2.spatialBlend = 1;
            deflateSound = GameDatabase.Instance.GetAudioClip(deflateSoundPath);
            audioSource2.clip = deflateSound;
            audioSource2.loop = false;
            audioSource2.time = 0;
            if (volume2 < 0)
            {
                volume2 = 0.0f;
            }
            else if (volume2 > 1)
            {
                volume2 = 1.0f;
            }
            audioSource.volume = volume2;

            this.part.buoyancyUseSine = false;
            originalCrashTolerance = this.part.crashTolerance;
            originalCOB = this.part.CenterOfBuoyancy;
        }

        public void Inflate()
        {
            ScreenMessages.PostScreenMessage("<color=#00ff00ff>[ComfortableLanding]Inflate!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            audioSource.PlayOneShot(inflateSound);
            InflateAnim.allowManualControl = true;
            InflateAnim.animSpeed = 1;
            InflateAnim.Toggle();
            //InflateAnim.allowManualControl = false;
            this.part.crashTolerance = crashToleranceAfterInflated;//This is an really airbag!
            this.part.CenterOfBuoyancy = COBAfterInflated;
            ApplyBuoyancySetting();
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Inflate!");
        }

        public void Deflate()
        {
            ScreenMessages.PostScreenMessage("<color=#00ff00ff>[ComfortableLanding]Deflate!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            audioSource2.PlayOneShot(deflateSound);

            InflateAnim.allowManualControl = true;
            InflateAnim.animSpeed = -3;
            InflateAnim.Toggle();
            //DeflateTransform.localScale = deflateScale;
            //InflateTransform.localScale = inflateScale;
            this.part.crashTolerance = originalCrashTolerance;//Not an airbag any more.
            ResetBuoyanceSetting();
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Touchdown!");
        }

        public void Touchdown()
        {
            ScreenMessages.PostScreenMessage("<color=#00ff00ff>[ComfortableLanding]Touchdown!</color>", 3f, ScreenMessageStyle.UPPER_CENTER);
            Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Touchdown!");
        }

        public void FixedUpdate()
        {
            if (!IsActivate)
                return;
            //Inflate Airbag
            if (alreadyInflatedAirBag == false)
            {
                if (vessel.radarAltitude <= inflateAltitude)
                {
                    Inflate();
                    alreadyInflatedAirBag = true;
                }
            }
            //Deflate Airbag
            if (alreadyInflatedAirBag == true && alreadyDeflatedAirBag == false)
            {
                if (vessel.Landed)
                {
                    //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Landed!");
                    Deflate();
                    alreadyDeflatedAirBag = true;
                }
                else if (vessel.Splashed && damageAfterSplashed == true)
                {
                    //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Splashed with damage");
                    Deflate();
                    alreadyDeflatedAirBag = true;
                }
                else if (vessel.Splashed && damageAfterSplashed == false)
                {
                    //Debug.Log("<color=#FF8C00ff>[Comfortable Landing]</color>Splashed with out damage");
                    Touchdown();
                    alreadyDeflatedAirBag = true;

                }
            }
            CheckLandedOrSplashed();

        }

    }
}