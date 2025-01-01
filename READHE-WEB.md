Originally created by  @Icecovery, was maintained by  @flywlyx, he hasn't updated it for a year., thread is here:  https://forum.kerbalspaceprogram.com/index.php?/topic/179556-17x-comfortable-landing-continued-v161/.  As usual, if @flywlyx returns, I'd be happy to return control to  him.

I was asked to adopt this, and since he didn't respond to my attempt contact, I've decided to adopt it.  See below for my changes

 

 
https://i.imgur.com/ynQjKEY.png

Comfortable Landing can keep your Kerbals safe and feel comfortable even in the last minute of a mission!

It adds retro rockets, buoys, and airbags into stock command pod and command pod of other mod.

And they will automatically deploy when certain conditions are met.

Full Pictures Album + GIF
https://i.imgur.com/zazaBlW.png

https://i.imgur.com/1zdNnIg.png

https://i.imgur.com/9dcXO4P.png

https://i.imgur.com/Hm9O8P8.png

 

Thanks @Kottabos made this great video for Comfortable Landing!


https://youtu.be/To6tpvxiIvw

 

 

Features:

Automatic deploy buoys or airbags, start landing burns.

Supported mod except for stock (list below) and more mod support coming soon.

 

Future features:

More Mod supports.

 

How to use:

To activate Comfortable Landing, press the Activate Pre-Landing Mode button before landing or splashing.
The retro rockets will start at 5 meters from the ground/water.
The buoys will be inflated after splashing and got bigger buoyancy.
The airbags will be inflated at 500 meters from the ground/water, and it will deflate after landing (but not splashing). The crash tolerance of part will increases when it was inflated.
Comfortable Landing will stop work after landing or splashing.
 

 

Add retro rockets/buoys/airbags to your mod:

Github Wiki Page: click here

 

Supported mod:

Stock (Mk1-2/Mk1/Mk2/Mk1-3)
Bluedog Design Bureau (Kane-11-3/Kane-11-5/Leo-M-63E/Leo-M-3VS)
Near Future Spacecraft (Mk3-9)
FASA (Apollo Float Ring/Apollo Float Round/Gemini/Gemini(White))
Notantares Space Industries (Porkalike Gemini-style "Mk1-A" Command Pod)(Config not included)
Tantares (VA/Soyuz)
Corvus CF (Corvus Command Pod )(Thanks forum user Aerospacesmith for providing this cfg!)
HOYO CSM (HOYO Command Module)
Icecovery's Chinese Spacecraft Pack (Shenzhou re-entry module)
 

Standalone parts：

Universal Airbag Module
Universal Buoy Module 
Universal Retro Rocket Module
Universal Landing Skirt Module
(Support rescale by TweakScale)
If you haven't installed TweakScale, will provide 0.625m, 1.25m, 2.5m and 3.75m size.
 

Author:

Models, Textures, Import, Configs: Icecovery
Plugins, Debug: Icecovery, 01010101lzy
 

Dependencies

Module Manager

Recommend

TweakScale
Delete GameData\ComfortableLanding\Configs folder if you want to remove integrated functions.

 

Availability

Source: https://github.com/linuxgurugamer/Comfortable-Landing
Download (BETA): https://github.com/linuxgurugamer/Comfortable-Landing/releases
License: GPLv3
 

Changes by Linuxgurugamer

 

Fixed Bluedog Mercury and Gemini capsules (BDB 1.7.2)
Adoption by Linuxgurugamer
Added configs for mk1pod_v2
Fixed bug where it could be activated while landed or splashed
Added message when trying to active while landed or splashed
Added config for ReStock & fixed an offset issue in contributed patch
Added Namespace to all code files
Refactored code to move all module-specific code into that module, leaving all common code in CL_ControlTool_Internal.  
The three PartModules now inherit from CL_ControlTool_Internal
Added dummy CL_ControlTool to allow compatibility for other parts
Removed the CL_ControlTool module from all configs
Made air bag default over a short period of time rather than instantaneous

