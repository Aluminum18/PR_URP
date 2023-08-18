# PR_URP
Implement common mechanics used by RPG game
# Content
- [Xray](#xray)
- [Flexible position movement joystick](#flexible-position-movement-joystick)
- [Recycle Audio Source](#recycle-audio-source)
- [Near projectile sound](#near-projectile-soud)

## Xray
Common mechanic used in stealth games. Props in scene partially changes their material to create Xray effect

https://github.com/Aluminum18/PR_URP/assets/14157400/682e1810-79e0-492c-aeea-8d9bf9873b85

**Implementation:**
- Mark area (pixels) that needed to be changed material with specific value (eg. 99) in Stencil buffer
- Use Shader graph to create xray effect for character (green effect), create CharacterXray material from this shader. Create other EnvironmentXray material for environmnent objects (grey one)
- Use URP Renderer feature to switch material.
  - For character: Parts that marked by value 99 in Stencil and behind other objects, render them by CharacterXray. Otherwise, keep assigned material.
  - For environment: Parts that marked by value 99 in Stencil, render them by EnvironmentXray.

## Flexible position movement joystick
Movement joystick that always follows player finger would provide a comfortable control experience.

## Recycle Audio Source
Every frame, Unity iterates all active Audio Sources in scene for updating its Audio System. Reusing AudioSource would reduce CPU overhead.

https://github.com/Aluminum18/PR_URP/assets/14157400/b4992b9a-dbb4-4736-9ee2-6ba4baa4a340

## Near projectile sound

https://github.com/Aluminum18/PR_URP/assets/14157400/6a5e35f1-dde8-4a7f-9834-75875ab702d2

