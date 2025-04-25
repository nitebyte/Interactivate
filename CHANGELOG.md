# Changelog

All notable changes to Interactivate will be documented in this file.

---

## [2.0.0] – 2025-04-22

### Added
- Modular, inspector-driven Hover Manipulation Presets. Any number of presets, each with custom targets, manipulation type, axis, direction, “oscillation”, custom in/out ease, per-preset delay (for fade/bounce, etc!). True multi-GO, multi-type hover support.
- Data-driven preset “Delay” field: Each hover effect can now use a unique delay before/after movements, and between oscillation valleys.

### Changed
- Removed all material-swap logic from hover.
- Fade and delay now exist per hover set, not globally.
- Full conversion: all hover transforms/effects now use AxisHelper and TweenExtensions.
- 2.0 milestone: considered “feature-complete” for presetable/multi-object hover manipulations.

---

## [1.9.0] – 2025-04-21

### Added
- UIFieldBinding can now optionally use a localization delegate for any dynamic string from an interactable's data fields.
- All runtime and helper classes wrapped in Nightbyte.Interactivate namespace to avoid collisions.

### Changed
- Refactored ValueLinkedManipulation to use AxisHelper for all axis assignment and Vector3 masking.
- All core scripts placed under “Scripts” subfolder for modular deployment.

---

## [1.8.0] – 2025-04-20

### Added
- Complete per-axis object manipulation in hover and trigger—supports X, Y, Z, XY, XZ, YZ, XYZ in both local/world space.
- Scale/position/rotation presets now allow individual oscillation, wait at peak, and custom Easing (per action!).

### Fixed
- Fixed issue where DOTween sequences could “snap” or finish out-of-order if re-hovering rapidly on the same object.

---

## [1.7.1] – 2025-04-19

### Fixed
- Mesh outline draws cleanly when toggling outline from code or multi-selection (editor and runtime).
- Audio presets using fade-in/fade-out won’t “cut off” or leave AudioSource stuck muted on disable or destroy.
- Tag/layer triggers now check for nulls and replace tags/layers only as configured.

---

## [1.7.0] – 2025-04-18

### Added
- Flexible FX, Audio, Tag/Layer, and TextPreset blocks per trigger.
- Trigger preset system can now fully replace tag/layer at runtime, not just set once at start.

### Changed
- Do not allocate new AudioSources unless none on the object—prevents AudioSource spam.
- Added “removeOnExpire” and “destroyOnExpire” per trigger for easier object lifecycle handling.

---

## [1.6.1] – 2025-04-17

### Fixed
- Clamp logic for trigger internal values now always enforces [min, max] both on set and increment.
- Axis increments using Mouse/ScrollWheel now respect invert and clamp, and increment per input-frame.

---

## [1.6.0] – 2025-04-17

### Added
- Internal integer value system per trigger, min/max configurable, dynamically updated via both axis and button input.
- Inspector-friendly foldouts and headers for all trigger and hover sections.
- Usage-count and reset-time configurable per trigger.

---

## [1.5.0] – 2025-04-16

### Added
- Object manipulation now supports “clamped” triggers: movement/rotation/scale within [min, max] range.
- TriggerSet input logic overhauled: axis/button/toggle/hold/fires work independently per trigger for full modularity.
- Value-linked manipulation block: real-time value affects a manipulation (move/rotate/scale) on a target with ease/animation.

### Changed
- Cleaned up axis masking and attribute-lookup code paths for efficiency; less branching, more systematic.

---

## [1.4.1] – 2025-04-15

### Fixed
- Removed mesh/material memory leaks by processing only unique meshes, sharing outline materials.

### Changed
- DataDisplayFields: now accessible via Get/Set API and indexed lookup.
- AudioPreset: allow fade-in/fade-out to chain, stopping or looping clips independently.

---

## [1.4.0] – 2025-04-15

### Added
- Hover transform system: move, scale, or rotate targets on hover, with oscillation and “wait at peak”, using new transform helpers.
- Trigger manipulations and hover behaviors now use DOTween for all animations, removing all previous manual animation logic.

---

## [1.3.1] – 2025-04-14

### Fixed
- ValueLinkedManipulation and TriggerSet now update reliably in Inspector and at runtime.

### Changed
- Outline behavior improved for editor and play mode; no double-processing, no z-index or blend mistakes.

---

## [1.3.0] – 2025-04-14

### Added
- TriggerSet supports maxUses, per-use events, and reset timers.
- Public API for runtime key/axis/value remapping and live data field access.

---

## [1.2.0] – 2025-04-13

### Added
- Custom key/value dynamic field system, accessible at runtime.
- Early custom inspector with grouped foldouts and optional editor logo/banner.

---

## [1.1.0] – 2025-04-12

### Changed
- Improved early hover system: triggers per-object “hover in” and “hover out” animations, FX, and audio on SetHoverState calls.
- Fixed (original code) bug where usage limits failed to reset after timeout, forcing relaunch or object replacement.

---

## [1.0.0] – 2025-04-12

### Added
- Initial release: All-in-one 1,776-line Interactivate.cs monolith.
- Single-object activation, trigger, data, FX, audio, and flattened hover support.
- Coroutine/lerp-based movement, rotation, and scaling.
- Usage limit, usage-reset counter, destroy/remove on expire.
- Rudimentary outline/highlight via temporary material swapping.
- Inspector properties for all fields; custom editor with logo/banner.

---

**Built by Nightbyte / Nitebyte Software**

---

**Note:** DOTween is required as of 1.4.0. Minimum Unity version is 2022.3 LTS for v2.x series.