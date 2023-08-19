# PR_URP
Implement common mechanics used by RPG game
# Content
- [Xray](#xray)
- [Flexible position movement joystick](#flexible-position-movement-joystick)
- [Adapt skill cast time with weapon](#adapt-skill-cast-time-with-weapon)
- [Recycle Audio Source](#recycle-audio-source)
- [Near projectile sound](#near-projectile-sound)
- [IK Animation](#ik-animation)
- [Testable with ScriptableObject](#testable-with-scriptableobject)

## Xray
Common mechanic used in stealth games. Props in scene and character partially changes their material to create Xray effect. Implemented by using Shader Graph, Stencil and URP renderer feature.

https://github.com/Aluminum18/PR_URP/assets/14157400/682e1810-79e0-492c-aeea-8d9bf9873b85

## Flexible position movement joystick
Movement joystick that always follows player finger would provide a comfortable control experience.

https://github.com/Aluminum18/PR_URP/assets/14157400/d5039150-fce1-4e84-a715-09d6093d884c

## Adapt skill cast time with weapon
Casting time of skill is depend on frame delay of weapon character is using.

https://github.com/Aluminum18/PR_URP/assets/14157400/0ef05bec-d3fc-4eaf-8131-220c6f55b500

## Recycle Audio Source
Every frame, Unity iterates all active Audio Sources in scene for updating its Audio System. Reusing AudioSource would reduce CPU overhead.

https://github.com/Aluminum18/PR_URP/assets/14157400/b4992b9a-dbb4-4736-9ee2-6ba4baa4a340

## Near projectile sound

"Whiz" sound for projectiles to make them more powerful.

https://github.com/Aluminum18/PR_URP/assets/14157400/b29b12d5-b4c4-4d03-82d4-4bd9397f8469

## IK Animation
Animation is made by animating IK. Animation Rigging package is used. Useful for creating simple animations, modifying existing animations to align with scenario (such as positioning character hand to interact with item when doing "pick up" animation). Below are IK animations that applied to Idle animation.

https://github.com/Aluminum18/PR_URP/assets/14157400/759cb5a5-9173-45a5-bec6-581300261efb

## Testable with ScriptableObject
Testing single module independenly. Useful for teamwork. You can test your module by modifying your module's input before module providing input value is completed. See original idea [here](https://www.youtube.com/watch?v=raQ3iHhE_Kk&ab_channel=Unity). I've implemented a similar system for myself [here](https://github.com/Aluminum18/unity-lib#scriptableobject-variables-and-events).

Below are 2 usecases in this project

Testing Heal bar UI and character behavior when taking damage by modifying character hp value.

https://github.com/Aluminum18/PR_URP/assets/14157400/46a63381-c6b8-4fc9-8469-1680a6089144

Testing Shoot behavior by raising "Shoot command". When UI is completed, just delegate raising command logic to coresponding button.

https://github.com/Aluminum18/PR_URP/assets/14157400/8e08c017-9d9e-4b33-9be7-ff060a08e341


