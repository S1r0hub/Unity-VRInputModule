# Unity-VRPointerVive

This repository is based on the following:  
https://github.com/wacki/Unity-VRInputModule

The goal is a laser pointer for a VR experience with HTC Vive using Unity and SteamVR 2.0.  
To achieve this, some scripts had to be modified.  
Also, this repository will only focus on the HTC Vive with SteamVR.  

![Preview Image 01](Images/Preview_01.jpg)
![Preview Image 02](Images/Preview_02.jpg)


<br/>

# How To Setup

## 1. Option: Using SteamVR "Player" Prefab

If you are using the prefab provided by Valve,  
simply add the `LaserPointerInputModule` script to the game object `InputModule` inside the "Player" prefab.  
There is already a `EventSystem` script attached to it.  
Ensure to enable `Send Navigation Events` on it.  

After that, you can add the `ViveUILaserPointer` script to the desired controller/hand.  
They are usually located here in the editor game object hierarchy: *Player/SteamVRObjects/*.  
Their name is either `LeftHand` or `RightHand`.  
**INFO:** You don't have to add the script directly to them if you want!  
You can simply add the script to any game object.  
But something **you always have to do** is selecting the desired controller on the script!  

## 2. Option: Own Setup

Ensure you use Valve's `Hand` script for your controllers  
and that you add the `LaserPointerInputModule` script to the game object that holds an `EventSystem` component!  

After that, you can add the `ViveUILaserPointer` script to the desired controller/hand as explained in "Option 1".


<br/>

# Possible Issues

> I created a new scene and added the script but nothing is working!

Well, there could be several reasons for this.  
One important thing that could be responsible for this behaviour is that  
there is no `EventSystem` component attached to the game object you have the `LaserPointerInputModule` attached to.  
Another one could be that there are multiple `EventSystem` components in the scene  
that prevent the call of the `Process` function.  


<br/>

# Updates

## Status 22.11.2018

- fix: visible hit point (issue was just the size, no bug)
- added SteamVR pickup functionality (new scripts: ViveUILaserPointerPickup and UILaserPointerPickup)
  - UILaserPointerPickup does the exact same as UILaserPointer BUT does not use the "Update" function (only difference)
- added prefabs/examples for the pickup functionality

## Status 18.11.2018

- code modied to function as supposed.  
- added **enable/disable feature** for the laser pointer  
- added sample scene (see `Scenes` folder) 

## Status 17.11.2018

The code and functionality is currently not tested.  
Testing and improvements will follow in some days.  


<br/>

# How It Works
(At least how I think it works from inspecting the code... - correct me please if I am wrong)

First, the laser pointer will cast a ray.  
To cast the ray, we use `Physics.Raycast`.  
As you can read in the documentation, this casts a ray `against all colliders in the scene`.  
This type of ray does **not** hit any UI element.  
It only hits/detects objects that have a collider component added to them.  
But we don't really want a Collider for UI Elements.  
They could block other objects in VR.  

So how are we detecting if we hit a UI-Element?  

The first ray is only to detect/set the correct distance to objects in the world we hit.  
It has nothing to do with functionality on the user interface.  
To detect hits with UI elements, there is **another ray**.  
This ray is casted different to the previous one.  
To cast it, we take the EventSystem from the `BaseInputModule` and use [`EventSystem.RaycastAll`](https://docs.unity3d.com/ScriptReference/EventSystems.EventSystem.RaycastAll.html).  
As you can read in the docs, it casts a ray `into the scene using all configured` [`BaseRaycasters`](https://docs.unity3d.com/ScriptReference/EventSystems.BaseRaycaster.html).  
And as you can read there as well, default Raycasters are `PhysicsRaycaster, Physics2DRaycaster, GraphicRaycaster`.  

Calling the beforementioned method requires a `PointerEventData` to be passed.  
The [position](https://docs.unity3d.com/ScriptReference/EventSystems.PointerEventData-position.html) of this data (**pointerEvent.position**) is set by us using a self-created **UICamera**.  
This camera is set for each canvas in the world to be the `Event Camera` or also known as [**Canvas.worldCamera**](https://docs.unity3d.com/ScriptReference/Canvas-worldCamera.html).  
Having a look at the docs as well tells us the following about it:  
`Also used as the Camera that events will be sent through for a World Space [[Canvas].).`  
As we always make this camera look at the same as our laser points at,  
we can simply use the center of what the camera is seeing as done in this line of code:  
```
data.pointerEvent.position = new Vector2(UICamera.pixelWidth * 0.5f, UICamera.pixelHeight * 0.5f);
```
As a result, we can get the correct UI element hit by the ray.  
We then simply use [`BaseInputModule.FindFirstRaycast`](https://docs.unity3d.com/ScriptReference/EventSystems.BaseInputModule.FindFirstRaycast.html) to get the first *valid* raycast result.  

After that, we check if the hit distance is less than the distance we got by the first raycast and update the length of the laser accordingly.  
We then handle a bunch of events. For example the `HandlePointerExitAndEnter` event.  
Depending on the laser inputs (toggle button pressed...) we use `ExecuteEvents` to notify affected game objects about events like pointer down, up, click or drag.  
