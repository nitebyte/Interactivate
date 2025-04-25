![InteractivateImage](https://github.com/user-attachments/assets/792bab96-5dd4-4a0f-b0b6-394b89346c5b)
# Interactivate for Unity

**Interactivate** is an advanced, modular toolkit for creating powerful, designer-friendly interactive objects and systems in Unity. Supporting DOTween-powered transitions, inspector-driven configuration, run-time interaction, and full extensibility, Interactivate is built to empower game developers, technical artists, UI/UX teams, and level designers with zero-boilerplate, robust interactive behaviors.

---

## Table of Contents

- [Features](#features)
- [Folder Structure](#folder-structure)
- [Installation](#installation)
- [Quick Start Guide](#quick-start-guide)
- [Core Concepts & Components](#core-concepts--components)
- [Extensibility & API](#extensibility--api)
- [Compatibility](#compatibility)
- [Upgrading from Previous Versions](#upgrading-from-previous-versions)
- [Example Code](#example-code)
- [Changelog](#changelog)
- [License](#license)

---

## Features

- 🔄 **Inspector-Driven Triggers**: Flexible, modular trigger sets with configurable input (button/axis/toggle/hold), value tracking, clamping, and usage/reset limits.
- 🎮 **Powerful Manipulation Presets**: Animate and control any object’s movement, rotation, and scale on interaction or hover, using per-preset directions, clamps, multi-axis, world/local space, and custom DOTween Ease curves.
- 🟡 **Live FX & Audio**: Trigger visual FX, sounds, UI changes, and tag/layer modifications—instantly and responsively.
- ✨ **Advanced Hover Animations**: Any number of hover effects per object, with target lists, oscillation, valley/peak waits, unique delays, and multi-axis blend, each with custom in/out ease.
- 🟦 **Dynamic Outline Rendering**: Outline any interactable object robustly (shader-powered, per-renderer and per-submesh, never miss geometry).
- 🟢 **Dynamic Data Fields**: Attach arbitrary string-data key/value pairs to any interactable, for gameplay/UI/inventory/questing use.
- 🔧 **Extensible Public API**: Access and modify keys, axes, values, and data live at runtime.
- 🌍 **Localization-Ready**: Data-driven fields can be routed through a customizable localization hook for multi-language support.

---

## Folder Structure

```
Assets/
└── Plugins/
    └── Nightbyte/
        └── Interactivate/
            ├── Materials/
            │   └── outline
            ├── Scripts/
            │   ├── AudioPreset.cs
            │   ├── AxisRef.cs
            │   ├── Enums.cs
            │   ├── FXPreset.cs
            │   ├── HoverTransformPreset.cs
            │   ├── InputAxisAttribute.cs
            │   ├── InputAxisDrawer.cs
            │   ├── Interactivate.cs
            │   ├── InteractivateCaster.cs
            │   ├── Localizer.cs
            │   ├── ObjectManipulationPreset.cs
            │   ├── TagLayerChange.cs
            │   ├── TextManipulationSet.cs
            │   ├── TriggerSet.cs
            │   ├── TweenExtensions.cs
            │   └── ValueLinkedManipulation.cs
            └── Shaders/
                └── outline
```

---

## Installation

1. **Clone or Download** this repository into your Unity project's `Assets/Plugins/Nightbyte/Interactivate/` directory.  
2. **Install DOTween**:
   - Import [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) from the Unity Asset Store.
   - Open the DOTween Utility Panel from Tools > Demigiant > DOTween Utility Panel and click “Setup DOTween…”.
3. **Add Materials and Shaders**:
   - Ensure the provided `outline` shader and material are present in the respective folders for outline effects to function.
4. **Attach Components**:
   - Add the `Interactivate` component to any GameObject with a Collider in your scene to make it interactive.
   - For camera-based raycast interaction, add `InteractivateCaster` to your main camera (or another suitable object).

---

## Quick Start Guide

Ready to make your objects interactive with minimal setup? Follow these quick steps to bring powerful, modular, designer-friendly interaction to your Unity project:

---

### 1. Make an Object Interactive

- **Select a GameObject**  
  - Choose any GameObject in your scene that includes a Collider component. (Physically interactive elements require a Collider—BoxCollider, SphereCollider, MeshCollider, etc.)
- **Add the Interactivate Component**  
  - In the Inspector, click “Add Component” and select `Interactivate`.  
  - The component will initialize with sections for Triggers, Hover Effects, Data Fields, Outline, and more.
- **Optional:**  
  - Organize your scene by grouping interactive props, doors, levers, etc., under a logical parent for efficiency.

---

### 2. Configure Triggers & Manipulations

- **Create a Trigger Set**  
  - In the “Trigger Sets” list, click (+) to add a new trigger.
  - Set a `setName` for easy reference (e.g., `OpenDoor`, `Pickup`, `ActivatePanel`).
- **Choose Input Type**  
  - Select input mode: _Button_, _Axis_ (scrollwheel, joystick), or advanced input like toggle/hold.
  - Assign the key or axis in the Inspector (use the Axis dropdown for auto-validation).
- **Configure Usage & Values**  
  - Set max uses, time to reset, toggle remove or destroy after expiry, clamp values, etc., as needed.
- **Add Manipulation Presets**  
  - Under each Trigger Set, add one or more `ObjectManipulationPreset`:
    - Choose the target GameObjects to move, rotate, or scale.
    - Set manipulation axis (`X`, `Y`, `Z`, `XY`, etc.), amount, duration, clamp range, and DOTween Ease.
    - Configure additional settings like world/local space, delays, clamping, and loops.

---

### 3. Set Up Hover Effects

- **Add Hover Manipulations**  
  - Expand “Hover Manipulations”.
  - Add as many `HoverTransformPreset` entries as you need:
    - Assign one or more target GameObjects to animate when hovered.
    - Choose manipulation type (Move/Rotate/Scale), axis/axes, amount, duration, per-preset fade/delay, World/Local space, and custom DOTween Ease curves.
    - Toggle oscillation for “ping-pong” hover effects, adding valley/peak delays for nuanced bounce or pulse.
- **Configure Audio & FX**  
  - In “Hover Options” you can also assign audio FX (list), sound volume, looping, fade out, and visual FX prefabs (particles, glows, etc.) for hover states.

---

### 4. Raycast Activation & Camera Setup

- **Add the InteractivateCaster**  
  - Assign the `InteractivateCaster` component to your Camera (usually the main player camera).
- **Configure Raycasting**  
  - Raycast distance, layer mask, and check interval can be set in Inspector for best fit (e.g., first-person use, top-down, VR pointer).
- **UI Integration**  
  - Utilize UI field bindings to dynamically show/hide or update UI elements (like tooltips or on-screen prompts) when the player gazes at an interactive object.

---

### 5. UI, Data Fields, and Advanced Bindings

- **Dynamic Data Fields**  
  - Add any number of key/value pairs under “Dynamic Fields” (e.g., `ItemName: Key`, `Action: Unlock`, `QuestStage: 2`).
  - Retrieve or update these fields via code to enable context-sensitive UI or gameplay.
- **UI Bindings (Optional)**  
  - Use Data Fields alongside InteractivateCaster’s UIFieldBinding system to dynamically display interactable names, actions, prompts, etc. in the UI.
  - UI elements (TextMeshPro, panels, icons) can animate in/out, fade, or scale according to custom defined DOTween presets.
- **Localization**  
  - Data Fields can be auto-localized by assigning a delegate to `Localizer.OnLocalize`.

---

### 6. Going Further

- **Public API**  
  - Programmatically set/get key bindings, axes, trigger values, or custom field data at runtime from your own game scripts.
  - Example: update locked/unlocked state from quest progress, change key binding on player input menu, serialize trigger or data state for save/load.
- **Runtime Flexibility**  
  - Add, remove, or update trigger/hover/config via script or in response to gameplay.
  - All configuration is fully inspector- and code-friendly!

---

### Next Steps

- See the [Example Code](#example-code) section for ready-to-use snippets.
- Explore all options in the Inspector—every property has a tooltip and is grouped for clarity.
- For advanced usage, reference API docs or dig into the helper scripts for extension points.

---

**Tip:** Want maximum flexibility? Every list—triggers, manipulations, audio, FX, data—can hold unlimited entries and be configured for individual behaviors, input, and visual result. Mix and match for rapid iteration!

---

## Core Concepts & Components

Interactivate is structured around robust modular components and data-driven presets, ensuring every part of interactive object behavior is easily configurable, extensible, and maintainable.

---

### **Interactivate (MonoBehaviour)**

- _Role_: The primary component to add to any GameObject with a Collider to make it interactive.
- _Responsibilities_:  
  - Manages and applies all trigger logic, hover effects, object manipulations, animations, audio/FX activation, usage counters, and custom data fields.
  - Orchestrates all preset systems (triggers, hover, value-linked manipulation) as well as outline and inspector integrations.
- _Key API Points_:
  - `SetHoverState(bool)`: Programmatically enter/exit hover state.
  - `triggerSets`, `hoverPresets`: Lists referencing all TriggerSet and HoverTransformPreset instances.
  - Public utility methods to remap/rebind triggers, update values, and access custom data.

---

### **HoverTransformPreset**

- _Role_: Describes a single hover effect or animated response to mouse/gaze/selection.
- _Fields & Configuration_:
  - `targets`: One or more Transforms to animate.
  - `manipulationType`: Move, Rotate, or Scale.
  - `axis`: Which axis (X, Y, Z or combinations like XY, XZ, YZ, XYZ) is affected.
  - `amount`: How much to move, rotate or scale per trigger.
  - `duration`: How long each animation step takes.
  - `delay`: Delay before first animation and, if oscillating, delay at the “valley” of oscillation.
  - `oscillate`: If enabled, creates continuous PingPong animation while hovered (with customizable `waitAtPeak` and delay at valley).
  - `easeIn`, `easeOut`: DOTween Ease type for forward and reverse movement.
  - `worldSpace`: Whether the animation is in local or world space.
- 
- _Behavior_:
  - Each preset is independent; you can combine effects (e.g. one object moves, another scales, another rotates).
  - *Oscillation* causes continuous back/forth/expand/contract/rocking etc. as long as the object is hovered.

---

### **TriggerSet**

- _Role_: Encapsulates an interaction “trigger”—i.e., a distinct interactive action or state change, optionally with its own conditions and outputs.
- _Fields & Configuration_:
  - `setName`: Unique name/ID for lookup and API.
  - **Input Settings**:  
    - Supports axis (continuous/analog; e.g. Mouse ScrollWheel), button (KeyCode), or toggle/hold/while-held input.
    - Fire modes: On key down, up, while held, toggle, or hold for duration.
    - Axis settings offer min/max clamps, inversion, and value increments per input tick.
  - **Usage**:
    - Max uses, reset time (auto-resets usage counter), remove-on-expire, destroy-on-expire flags.
  - **Value**:
    - Integer state, optional min/max and live clamping.
  - **Manipulation, FX, Audio, Tag/Layer, Text Presets**:
    - Lists of all actions to perform when this trigger fires. Fully editor and code configurable.
  - **Value-Linked Manipulation**:
    - Live-action block binding the trigger value to a manipulation effect via mapping/min/max/curve.
  - **Events**:
    - UnityEvents for inspector-based callbacks.

---

### **Manipulation Preset Classes**

Interactivate uses several preset types, each providing a clean separation of interactive behaviors:

- **ObjectManipulationPreset**
  - Move/rotate/scale one or more Transforms, with custom direction, range (min/max), clamp, axis, duration, tween, world/local.
  - Supports per-action loop, delay, and logic for animation blending with other actions.

- **ValueLinkedManipulation**
  - Bind an internal value (often set by trigger/axis/buttons) to a manipulation on one or more targets.
  - Use case: Dials, levers, volume sliders, progressive unlocks.

- **TextManipulationSet**
  - Animate UI text, changing the displayed string, color, size, style or font.
  - Tweens text and color with DOTween, and allows for scrambling, fade, and other DOTween text features.

---

### **Dynamic Outline**

- Uses a custom, resource-efficient outline shader applied as an additional material to _every_ renderer (and submesh!) on the interactive object/its children.
- Outline is only visible when hover/selection is active.
- Highly robust: never misses faces, never z-fights, and supports color/thickness.
- Outline effect is triggered by SetHoverState and managed by the system (no user intervention necessary).

---

### **Audio/FX/TagLayer Presets**

- **AudioPreset**  
  - Play one or multiple AudioClips with support for fade-in/fade-out, looping, and volume.
  - Can “cut” (stop) other AudioSources upon playing for clarity/simplicity.
- **FXPreset**
  - Enables/disables or pulses visual FX GameObjects or components.
  - Can be used for particle, mesh swap, or script-driven FX (e.g., activating particle systems, glow effects, etc.)
- **TagLayerChange**
  - Changes tags/layers on any number of GameObjects upon trigger fire, useful for AI targeting, culling, navigation, physics, etc.

---

### **DataDisplayField**

- Per-object key/value string dictionary.
- Store arbitrary fields such as “ItemType=Potion”, “QuestStage=3”, or “Owner=Player1”.
- Used for UI integration, stat systems, external conditions, quest/logic, and more.
- Exposed in inspector and via code for quick reads, writes, and runtime adjustment.

---

### **Input Mapping Helpers & Utilities**

- **AxisRef**: Efficient, no-branch struct for axis lookup, tracking, and manipulation — all move/rotate/scale axes unified.
- **InputAxisAttribute/InputAxisDrawer**: Provides dropdowns listing all project-defined input axes in the inspector for error-free configuration.
- **TweenExtensions**: Unified helpers for generating DOTween tweens/sequences, with defaults and all the correct ease/loop settings.
- **Enums**: Robust Axis/ManipulationType/ButtonActivation enums for all type-safe selections in code and inspector.
- **Localizer**: Static delegate-based single-point localization, enabling drop-in translation support for DataDisplayFields and UI fields.

---

## Extensibility & API

Interactivate is designed for maximum flexibility—every core system is both inspector-friendly and fully accessible via public API, allowing code-driven or tool-driven customizations, procedural creation, UI integration, and external system interoperability.

### Core Namespace

All runtime and helper code resides in the namespace:

```csharp
using Nightbyte.Interactivate;
```

This keeps your project’s global namespace clean and future-proofs your code from name collisions with other packages or plugins.

---

### Public API

#### Input & Rebinding

You can get and set input triggers at runtime to allow dynamic control mapping, unlock game settings menus, or support per-player configurations:

```csharp
// Change an interactable's "Open" action to KeyCode.F at runtime:
myInteractable.SetKey("Open", KeyCode.F);

// Retrieve assigned key for a named trigger set:
KeyCode useKey = myInteractable.GetKey("Use");

// Rebind an axis for fine-grained input
myInteractable.SetAxis("ScrollZoom", "Mouse ScrollWheel");

// Retrieve runtime axis assignment
string axisName = myInteractable.GetAxis("Rotation");
```

_Use case:_  
Allow users to remap interaction controls or axes during runtime via your own UI.

---

#### Trigger Values

Read or set the current value for a trigger set (typically for value-linked dials, levers, sliders, doors, etc.):

```csharp
// Get the current value of a named trigger ("Lever")
int leverValue = myInteractable.GetValue("Lever");

// Set the state (forcibly, e.g. from save/load or external logic)
myInteractable.SetValue("Lever", 3);
```

_Use case:_  
Persist trigger values in savegames or coordinate them across multiplayer/networked objects.

---

#### Dynamic Data Fields

Attach arbitrary named string key/value pairs, retrieve or set them at runtime:

```csharp
// Set a custom data field
myInteractable.SetDataField("QuestStatus", "Complete");

// Get a field's value (returns null if not found)
string itemType = myInteractable.GetDataField("ItemType");
```

_Use case:_  
Store quest states, inventory tags, custom parameters, localizations, or any extra per-object data without schema changes.

---

#### Value-Linked & Live Manipulation

Programmatically drive TriggerSet and ValueLinkedManipulation logic:

```csharp
// Programmatically increment a value (triggers all linked manipulations)
myInteractable.SetValue("Dial", myInteractable.GetValue("Dial") + 1);

// Directly fire a trigger set by name
void ActivateByName(Interactivate inter, string setName) {
    if (inter.Find(setName, out var set)) inter.Fire(set);
}
```

---

#### Hover/Outline/FX Controls

You can programmatically set hover states or toggle outline visibility at any time:

```csharp
myInteractable.SetHoverState(true);  // Enable hover outline and effects

// Custom behaviour: hide outline only without triggering hover logic
// (Outline materials can also be adjusted directly if desired.)
```

---

#### Full Custom Interactions

You can add, remove, or configure triggers/manipulation/hover FX at runtime—not just in the Inspector:

```csharp
var newTrigger = new TriggerSet();
newTrigger.setName = "Secret";
newTrigger.manipulationPresets.Add(customManipulation);
myInteractable.triggerSets.Add(newTrigger);
```

---

### Localization Integration

Interactivate supports runtime localization for all data-driven UI fields out-of-the-box:

**Setup:**

```csharp
// Provide your own lookup function (e.g. from CSV, ScriptableObject, Google Sheets, etc)
Nightbyte.Interactivate.Localizer.OnLocalize = key => MyLocalizationTable.GetLocalizedValue(key);
```

**Usage:**

Any UI data field bound via InteractivateCaster will pass its text value through this localization delegate _if enabled_ in the inspector.

This lets you:
- Translate object names, actions, item types, quest tags, etc.
- Use a central/local/global language or translation system.
- Update translations at runtime with no code changes; just change your localization data.

---

### Advanced: Full Preset Scripting

All preset blocks (TriggerSets, HoverTransformPresets, Manipulations, FX/Audio/Text) are inspections and code-accessible C# classes/lists.
- You can construct/clone/add/remove them at runtime.
- All fields are public or serializable, so can be built procedurally or loaded from user-generated data.

**Example: Add a new Move manipulation to all interactables in a zone**

```csharp
foreach (var interactable in FindObjectsOfType<Interactivate>()) {
    var move = new ObjectManipulationPreset { manipulationType = ManipulationType.Move, axis = Axis.Y, amount = 3 };
    interactable.triggerSets[0].manipulationPresets.Add(move);
}
```

---

### Inspector Extensibility

- All helpers (AxisRef, TweenExtensions, InputAxisAttribute/Drawer, Enums) are usable in custom scripts.
- For custom inspectors/validators, reference the same property names and lists as visible in the default Interactivate Inspector.

---

### Editor/Tooling

- Interactivate is compatible with custom editors, property drawers, and tool-driven workflows.
- All fields are `[SerializeField]`, and most lists/fields support reordering, copy-paste, and prefab/ScriptableObject referencing.

---

**Need more code examples?**  
See the [Example Code](#example-code) section, or open an issue or discussion on GitHub.

---

## Compatibility

- Requires [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) (MIT license, free version compatible).
- Unity 2022.3 LTS or newer.
- Suitable for built-in, URP, and HDRP pipelines, with minor shader adjustments if required for outline visuals.

---

## Upgrading from Previous Versions

- **From Pre-2.0 (Legacy)**
    - Remove any old Interactivate.cs (1.x).
    - Replace with modular scripts found in `/Scripts/`, ensuring you update to DOTween-powered methods and reconfigure Inspector fields and data layouts.
    - Material-swap on hover is no longer supported; use manipulation and FX/audio for feedback.
- **Custom editors from legacy may not be compatible**—see future releases for updated editor tools.

---

## Example Code

This section provides example scripts using Interactivate’s public API, inspector-based data, and modular presets. Each snippet can be copy-pasted directly into your Unity project (add as new C# scripts or insert into existing classes where needed).

---

### Basic: Interactivate Usage

#### 1. Interacting with an Object by Name

```csharp
using Nightbyte.Interactivate;
using UnityEngine;

public class InteractOnKey : MonoBehaviour
{
    public string interactableName = "MyDoor";
    public KeyCode interactionKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            var interactable = GameObject.Find(interactableName)?.GetComponent<Interactivate>();
            if (interactable != null)
            {
                interactable.SetKey("OpenDoor", interactionKey);
                interactable.SetHoverState(true); // e.g., highlight or animate
                // Call a trigger set by name, if needed:
                // interactable.Fire( ... );  // See advanced example
            }
        }
    }
}
```

---

### Input Remapping & Dynamic Controls

#### 2. Change Key Mapping at Runtime

```csharp
using UnityEngine;
using Nightbyte.Interactivate;

public class RebindInteractKey : MonoBehaviour
{
    public Interactivate interactable;
    void Start()
    {
        interactable.SetKey("Activate", KeyCode.F); // Change trigger to 'F'
    }
}
```

#### 3. Dynamic Axis Remapping

```csharp
// Example: User switches input axis for zoom control dynamically
interactable.SetAxis("Zoom", "Mouse ScrollWheel");
```

---

### Working with Data Fields

#### 4. Setting and Getting Data Fields

```csharp
using Nightbyte.Interactivate;
using UnityEngine;

public class DataFieldDemo : MonoBehaviour
{
    public Interactivate interactable;

    void Start()
    {
        interactable.SetDataField("QuestItem", "GoldKey");
        string val = interactable.GetDataField("QuestItem"); // "GoldKey"
        Debug.Log($"Quest Item: {val}");
    }
}
```

#### 5. Modify Data Fields at Runtime (Save/Load Example)

```csharp
string savedQuestState = PlayerPrefs.GetString("quest_state", "NotStarted");
interactable.SetDataField("QuestState", savedQuestState);
```

---

### Value and Usage System

#### 6. Syncing Value-Linked Manipulation with External Gameplay

```csharp
// Example: Set a lever state's value and instantly update visuals
interactable.SetValue("Lever", 5);
```

#### 7. Check Usage Count

```csharp
if (interactable.GetValue("OpenDoor") >= 3)
{
    Debug.Log("Max uses reached!");
}
```

---

### Triggering Animations & Effects

#### 8. Trigger an Effect Programmatically

```csharp
public void OpenAllDoors()
{
    foreach (var door in FindObjectsOfType<Interactivate>())
    {
        door.SetValue("OpenDoor", 1); // Or door.Fire( ... ) as needed
    }
}
```

---

### Hover & UI Integration

#### 9. Manual Hover from Raycast

```csharp
using UnityEngine;
using Nightbyte.Interactivate;

public class ManualHoverRay : MonoBehaviour
{
    public Camera cam;
    Interactivate last;
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var ia = hit.collider.GetComponent<Interactivate>();
            if (ia != last)
            {
                if (last != null) last.SetHoverState(false);
                if (ia != null) ia.SetHoverState(true);
                last = ia;
            }
        }
        else if (last != null)
        {
            last.SetHoverState(false);
            last = null;
        }
    }
}
```

---

### Localization for UI

#### 10. Assign Localization Function

```csharp
using Nightbyte.Interactivate;

void Awake()
{
    Localizer.OnLocalize = myKey => MyLocalizationManager.Lookup(myKey);
}
```
_Bind this for UI menu/tooltip support; UIFieldBinding in InteractivateCaster can then localize dynamically from your string table or system._

---

### UI Field Binding Setup

#### 11. Show Data In UI on Hover (With Localizer)

```csharp
// In a MonoBehaviour/UI manager:
void Start()
{
    InteractivateCaster caster = Camera.main.GetComponent<InteractivateCaster>();
    foreach (var binding in caster.bindings)
        binding.applyLocalization = true; // Now data fields will auto-localize!
}
```

---

### Editor/Scriptable Configuration

#### 12. Adding Extra Trigger Sets and Manipulations via Script

```csharp
var TS = new Nightbyte.Interactivate.TriggerSet();
TS.setName = "BlowUp";
TS.input.activationKey = KeyCode.B;
var explodeFX = new FXPreset(); /* ... Configure FX ... */
TS.fxPresets.Add(explodeFX);
interactable.triggerSets.Add(TS);
```

---

### Combining Everything

#### 13. Complete Custom Runtime Interactor

```csharp
// (Attach to controller/camera/player root)
using Nightbyte.Interactivate;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    Interactivate current;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 5f))
        {
            var ia = hit.collider.GetComponent<Interactivate>();
            if (ia != current)
            {
                if (current != null) current.SetHoverState(false);
                current = ia;
                if (current != null) current.SetHoverState(true);
            }
            if (current != null && Input.GetKeyDown(KeyCode.E))
                current.SetValue("Activate", current.GetValue("Activate") + 1);
        }
        else if (current != null)
        {
            current.SetHoverState(false);
            current = null;
        }
    }
}
```

---

#### More Examples

- ScriptableObject-based presets and runtime setup
- Runtime UI tooltip using InteractivateCaster UIFieldBinding
- Multiplying hover/trigger effects with multiple objects
- Saving/loading full state of all Interactivate objects in a scene

---

## FAQ

<details>
<summary>Q: What Unity versions are supported? What about DOTween?</summary>

**A:**  
Interactivate is tested on Unity 2022.3 LTS and later and should work in all modern actively supported Unity versions. DOTween (free or Pro) is a hard requirement for all animation/tweening; get it from [the Asset Store](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676).

</details>

<details>
<summary>Q: Does Interactivate work with URP/HDRP? What about custom pipeline shaders?</summary>

**A:**  
Yes—outline and FX systems work with Built-In, URP, and HDRP. You may need to adjust the `outline` shader for SRP compatibility (see [Demigiant forum](http://dotween.demigiant.com/) and Unity guides for porting custom effects/shaders).

</details>

<details>
<summary>Q: Why is DOTween required? What if I want to use something else?</summary>

**A:**  
Interactivate relies on DOTween because of its robustness, performance, visual quality, and reliability across all Unity versions.  
If you want to substitute a different tween solution, you would need to reimplement `TweenExtensions` and any direct DOTween API use. This is not recommended unless you have a deep understanding of both frameworks.

</details>

<details>
<summary>Q: Can I trigger interactions or manipulations from my own code?</summary>

**A:**  
Yes! Use `SetKey`, `SetAxis`, `SetValue`, manipulate `triggerSets`, or call `SetHoverState(bool)` to trigger behaviors in code any time.

</details>

<details>
<summary>Q: What’s the difference between Trigger Sets and Hover Manipulations?</summary>

**A:**  
- **Trigger Sets**: Fired by input (button, axis, condition), generally for purposeful/user actions (open, pick up, interact).  
- **Hover Manipulations**: Activate when the object is under player focus/gaze/cursor, often for feedback, highlighting, or “ambient” effects.

</details>

<details>
<summary>Q: My outlines sometimes disappear or behave inconsistently—why?</summary>

**A:**  
- Make sure outline materials and shaders are present in the `/Materials` and `/Shaders` folders.  
- Multiple renderers or objects with shared meshes might need additional care if you modify renderers at runtime.

</details>

<details>
<summary>Q: Can I create new triggers, hover effects, or data fields at runtime, not just in the inspector?</summary>

**A:**  
Yes! All configuration lists are `[SerializeField]` and accessible in code.  
Example:  
```csharp
var ts = new TriggerSet();
ts.setName = "MyNewTrigger";
// ... configure as desired
myInteractable.triggerSets.Add(ts);
```

</details>

<details>
<summary>Q: How do I display dynamic data (item name, action prompt, etc.) in my custom UI?</summary>

**A:**  
- Populate Data Fields per object (e.g. ItemName, Action, Description).
- Use InteractivateCaster’s UIFieldBinding to display these values dynamically in the UI—full support for TextMeshPro and most Unity UI components.
- Localization ready: simply implement `Localizer.OnLocalize`.

</details>

<details>
<summary>Q: What happens when a trigger “expires” (usage runs out)?</summary>

**A:**  
Triggers can either disable or destroy their GameObject (`removeOnExpire` / `destroyOnExpire`), depending on which flags you enable.  
Auto-reset will reactivate after the reset period, unless destroyed.

</details>

<details>
<summary>Q: Can I localize data/UI fields?</summary>

**A:**  
Yes! Assign a method to `Localizer.OnLocalize` (e.g. a dictionary/table lookup function), and toggle `applyLocalization` per UI field binding.

</details>

<details>
<summary>Q: Is multiplayer/networking supported?</summary>

**A:**  
No built-in network/multiplayer logic is present; however, all functions are accessible for you to sync/lock/intercept via your own multiplayer system.  
Hint: Use API to synchronize trigger states, values, and fields over the network.

</details>

<details>
<summary>Q: Can I animate other components or non-transform fields?</summary>

**A:**  
For custom behaviors (shader parameters, physics, camera, etc.)—extend or clone the preset classes, or use DOTween directly either in derived scripts or as part of new preset modules.

</details>

<details>
<summary>Q: Does Interactivate support VR or touch input?</summary>

**A:**  
Yes—with custom raycasters or input devices (XR, AR, mobile)—simply call appropriate trigger methods (e.g., `SetHoverState`) or fire triggers manually based on your input.  
Unity’s XR/AR modules work out-of-the-box if you set up InteractivateCaster with your pointer or gaze system.

</details>

---

## Troubleshooting

If something isn’t working as expected, consult the following guide:

### Interactions Not Firing / Object Not Reacting

- Does the GameObject have a Collider and the Interactivate script attached?
- Check that the “Trigger Sets” actually have input assigned and are enabled.
- Confirm layer masks in InteractivateCaster are not filtering out your object.
- Ensure object is not disabled or destroyed (watch for expiry settings).

### Hover/Outline Not Appearing

- Make sure Hover is enabled in the Interactivate Inspector.
- Shader and material for outline must be present and assigned as shipped.
- Check if the “Outline” color/thickness aren’t set to zero.
- For faded/partially visible outlines: make sure mesh normals and UVs are valid; reimport or clean meshes if necessary.

### FX, Audio, or UI Not Showing

- Double-check that FX and Audio Presets are populated and that objects/clips are assigned.
- For FX, ensure referenced GameObjects aren’t disabled by default (unless meant to be controlled fully by Interactivate).
- For UI, make sure field IDs in UIFieldBinding match those in your Data Fields exactly (case-sensitive).

### Value or Usage System Problems

- Value-linked manipulations require a valid field in “ValueLinkedManipulation”.
- If clamping is enabled, check min/max values in the Inspector: misconfiguration here can cause values to “stick.”
- Usage counters only reset after the defined `resetTime`—make sure it’s not zero or negative.

### Inspector/Editor Issues

- If fields are missing, try reimporting or moving the entire Interactivate folder under `/Plugins/Nightbyte/`.
- Custom editors may not appear if placed outside an `/Editor/` folder.
- Changes not saving? Check for `[NonSerialized]`, `[HideInInspector]`, or other Unity attribute conflicts in your version.

### Performance

- Very large numbers of interactive objects? Pool, share, and reuse outline materials wherever practical.
- Ensure that DOTween is not running unnecessary tweens (OnDisable auto-kills for safety; inspect with DOTween’s debug options if needed).

### DOTween-Related Errors

- Ensure DOTween is present, setup (Tools → Demigiant → DOTween Utility Panel → Setup DOTween) and there are no compile errors.
- If custom shaders or strange animation glitches appear, make sure DOTween and Unity are both up to date.

### Miscellaneous

- Multi-mesh/Renderer props: Confirm all relevant parts are children of the root object with Interactivate.
- Runtime API calls yield null? Check for naming/typos (all searches are case-sensitive and require exact name matching on sets/fields).

---

Still stuck?  
Open a GitHub issue with a minimal repro project, error log, or relevant inspector screenshot and the dev will help you resolve it!

---

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the full project history and per-version details.

---

## License

Released under the [NIGHTBYTE LTD. UNIVERSAL PUBLIC CODE LICENSE v1.0](LICENSE.md).

---

**Made with ❤️ by Nightbyte**
