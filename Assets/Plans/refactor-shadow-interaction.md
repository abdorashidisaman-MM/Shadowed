# Project Overview
- Game Title: Shadowed
- High-Level Concept: 2.5D stealth game using 3D Unity systems featuring light and shadow mechanics. Player locomotion is constrained to the X/Y gameplay plane.
- Players: Single player
- Inspiration / Reference Games: Stealth puzzle/action games using light and shadow interaction mechanics.
- Tone / Art Direction: 2.5D / Dark / Shadow theme
- Target Platform: Standalone Windows PC
- Screen Orientation / Resolution: Landscape (1920x1080)
- Render Pipeline: Universal Render Pipeline (URP - PC_RPAsset)

# Game Mechanics
## Core Gameplay Loop
The player navigates environments containing shadow sources (`ShadowRange`). When entering a shadow range trigger, the shadow zone connects visually to the player via a `LineRenderer` and registers itself with the player's `PlayerShadowInteraction` component. When exiting, the connection is broken and the shadow zone unregisters.

## Controls and Input Methods
2.5D movement constrained to the X/Y gameplay plane using Unity New Input System.

# UI
N/A (Backend refactoring of script interaction logic; no UI changes required).

# Key Asset & Context
- `Assets/_Game/Scripts/Player/PlayerShadowInteraction.cs`: Component attached to the Player GameObject. Manages the collection of `ShadowRange` instances the player is currently occupying.
- `Assets/_Game/Scripts/Objects/ShadowRange.cs`: Component attached to shadow trigger GameObjects. Detects player presence, handles line rendering connection to player, and registers/unregisters with `PlayerShadowInteraction`.

### Existing Code Analysis Summary
- `PlayerShadowInteraction.cs`:
  - Contains method names `SetCurrentShadow` and `ClearCurrentShadow` which do not reflect the list operation (`nearbyShadows.Add` / `nearbyShadows.Remove`).
  - Contains empty unused `Start()` and `Update()` methods.
  - Lacks null validation on parameters.
- `ShadowRange.cs`:
  - Includes problematic imports (`using System.Numerics;` causing potential type conflicts with Unity types, `using Unity.VisualScripting;`).
  - Evaluates `LayerMask.NameToLayer("Player")` on every trigger enter/exit event instead of caching.
  - Directly calls `GetComponent<PlayerShadowInteraction>()` without checking if it returned `null` before invoking methods.
  - Lacks safety checks on `connectionLine` in `Update()`.
  - Uses PascalCase for private field `Interaction`.

# Implementation Steps

### Step 1: Refactor `PlayerShadowInteraction.cs`
- **Description**:
  1. Rename registration methods to align with list operations:
     - `SetCurrentShadow(ShadowRange shadow)` -> `AddShadow(ShadowRange shadow)`
     - `ClearCurrentShadow(ShadowRange shadow)` -> `RemoveShadow(ShadowRange shadow)`
  2. Add parameter null checks before modifying `nearbyShadows`.
  3. Expose `nearbyShadows` strictly as `public IReadOnlyList<ShadowRange> NearbyShadows => nearbyShadows;` (preventing external mutable List access).
  4. Clean up unused imports and remove empty `Start()` and `Update()` boilerplate methods.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

### Step 2: Refactor `ShadowRange.cs`
- **Description**:
  1. Remove unnecessary/problematic namespaces (`System.Numerics`, `Unity.VisualScripting`, `System`).
  2. Cache `playerLayerIndex` in `Awake()` using `LayerMask.NameToLayer("Player")`.
  3. Cache and guard `connectionLine` (`LineRenderer`) in `Awake()`.
  4. In `OnTriggerEnter` and `OnTriggerExit`:
     - Compare `other.gameObject.layer` against cached `playerLayerIndex`.
     - Safely acquire `PlayerShadowInteraction` using `other.TryGetComponent<PlayerShadowInteraction>(out var interaction)`.
     - Call updated registration methods (`interaction.AddShadow(this)` and `interaction.RemoveShadow(this)`).
  5. In `Update()`: add guard `if (connectionLine != null && playerTransform != null)` before setting position endpoints.
  6. Implement `OnDisable()` cleanup: if disabled while registered with the player, call `interaction.RemoveShadow(this)` to prevent stale entries in `NearbyShadows`, then disable `connectionLine` and clear player references (`playerTransform = null`, `interaction = null`).
  7. Rename private field `Interaction` to `interaction` for standard C# camelCase naming.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

### Step 3: Verification & Compilation Check
- **Description**:
  1. Verify zero compilation warnings or errors in Unity project context.
  2. Ensure scene references and components on `Player` and `Cube` / `Cube1` objects operate cleanly without null reference exceptions.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

# Verification & Testing
1. **Compilation Check**: Perform static syntax and reference verification using `GetConsoleLogs` / C# compilation check.
2. **Safety Check**: Verify with a read-only script inspection that:
   - `System.Numerics` is removed from `ShadowRange.cs`.
   - `LayerMask.NameToLayer("Player")` is cached in `Awake()`.
   - `TryGetComponent` is used for `PlayerShadowInteraction`.
   - Method names in `PlayerShadowInteraction` are `AddShadow` / `RemoveShadow`.
3. **PlayMode / Scene State Check**: Validate that `Player` GameObject retains `PlayerShadowInteraction` and shadow cube GameObjects (`Cube`, `Cube1`) retain `ShadowRange` components.
