# Project Overview
- Game Title: Shadowed
- High-Level Concept: A shadow-based gameplay experience where the player moves through 3D environments, jumping and interacting with light/shadow mechanics.
- Players: Single player
- Inspiration / Reference Games: Shadow puzzle-platformer games
- Tone / Art Direction: Dark, atmospheric 3D stylized environment
- Target Platform: StandaloneWindows64 (PC)
- Screen Orientation / Resolution: Landscape 1920x1080
- Render Pipeline: Scriptable Render Pipeline (PC_RPAsset)

# Game Mechanics
## Core Gameplay Loop
The player navigates levels by running and jumping in 3D/2.5D space, discovering and interacting with light sources and shadows (represented by shadow ranges and triggers).

## Controls and Input Methods
Input managed via Unity New Input System using `GameInputActions.inputactions`:
- **Move**: Axis input reading horizontal directional movement.
- **Jump**: Button input triggering vertical jump when grounded.
- **Aim**: Vector2 pointer/stick input reserved for shadow direction/aiming.

# UI
Minimalist in-game HUD (not modified during this refactor).

# Key Asset & Context
- `Assets/_Game/Input/GameInputActions.cs`: Generated C# wrapper for input action asset. Contains `Player` action map with `Move`, `Jump`, `Aim`.
- `Assets/_Game/Scripts/Input/PlayerInputReader.cs`: New component that owns `GameInputActions`, handles lifecycle (`Enable`/`Disable`), and exposes properties (`MoveInput`, `AimInput`, `JumpRequested`, `ConsumeJumpRequest()`). `JumpRequested` persists until `ConsumeJumpRequest()` is explicitly called (not auto-cleared at end of `Update()`).
- `Assets/_Game/Scripts/Player/PlayerMovement.cs`: Existing movement controller. Refactored to remove ownership of `GameInputActions` and rely on `PlayerInputReader`.
- `Assets/_Game/Scripts/Player/PlayerShadowInteraction.cs`: Existing shadow interaction script. Updated to reference `PlayerInputReader` to read `AimInput`.
- `Assets/_Game/Prefabs/Player.prefab`: Player prefab that will have `PlayerInputReader` attached. (Scene instances in `Prototype.unity` will inherit this change without creating scene overrides).

# Implementation Steps

### Step 1: Create `PlayerInputReader.cs`
- **Description**: Create new C# script at `Assets/_Game/Scripts/Input/PlayerInputReader.cs` under namespace `Shadowed.Input`. It instantiates `GameInputActions`, handles enabling/disabling in `OnEnable`/`OnDisable`, reads `Move`, `Jump`, and `Aim` inputs in `Update()`, and exposes `MoveInput` (`float`), `AimInput` (`Vector2`), `JumpRequested` (`bool`), and `ConsumeJumpRequest()` (`bool`). `JumpRequested` is set to `true` when `Jump.WasPressedThisFrame()` occurs and persists until `ConsumeJumpRequest()` clears it (it is NOT automatically reset in `Update()`).
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

### Step 2: Refactor `PlayerMovement.cs`
- **Description**: Refactor `Assets/_Game/Scripts/Player/PlayerMovement.cs` to remove direct creation and management of `GameInputActions`. Add `RequireComponent(typeof(PlayerInputReader))` attribute and cache `PlayerInputReader` reference in `Awake()`. Update `ReadInput()` to retrieve `MoveInput` and consume jump requests via `inputReader.ConsumeJumpRequest()`. Remove `aimInput` tracking from `PlayerMovement`.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: No

### Step 3: Refactor `PlayerShadowInteraction.cs`
- **Description**: Update `Assets/_Game/Scripts/Player/PlayerShadowInteraction.cs` to acquire a reference to `PlayerInputReader` on `Awake()`, enabling access to `AimInput` from `PlayerInputReader` for future shadow interaction features (shadow targeting logic will not be implemented yet).
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Parallelizable**: Yes

### Step 4: Attach `PlayerInputReader` Component to Player Prefab
- **Description**: Add `PlayerInputReader` component to `Assets/_Game/Prefabs/Player.prefab` only. Do not modify scene object in `Prototype.unity` directly to avoid redundant scene overrides.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 2, Step 3
- **Parallelizable**: No

### Step 5: Verification & Testing
- **Description**: Verify successful compilation in Editor. Test movement and jumping in Play Mode in `Prototype.unity`. Verify console log clean (no input leak warnings or missing component errors).
- **Assigned role**: developer
- **Dependencies**: Step 4
- **Parallelizable**: No

# Verification & Testing
- **Compilation Check**: Confirm no compilation errors exist after writing scripts.
- **Console Log Check**: Ensure no memory leak warnings from `GameInputActions` (`Disable` called properly when disabled/destroyed).
- **Play Mode Test**: Verify left/right movement, jump mechanics, and grounding behave identically to the pre-refactoring state.
- **Component Association**: Confirm `PlayerInputReader` is attached only to `Assets/_Game/Prefabs/Player.prefab` and inherited cleanly by prefab instances in scenes without overrides.
