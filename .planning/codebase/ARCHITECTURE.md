# Architecture

## High-Level Architecture
The project follows a standard Unity Component-Based Architecture, heavy on the `MonoBehaviour` usage.
It utilizes a structural division by "Levels" or "Features", with each major interaction or game mechanic encapsulated in its own scene/folder.

## Core Patterns

### 1. Singleton Managers
Most globally accessible objects use a basic Singleton pattern (e.g., `MainMenuHandler.Instance = this;`).
- `MainMenuHandler`
- `StageManager`, `TraceManager` (potentially)
- `UIManager`

### 2. Event Handlers & UI Bindings
Extensive use of direct UI binding through Unity Events (on UI Buttons) linking to methods like `ButtonHandler.cs` or `PanelHandler.cs`. 

### 3. Scene-Based Progression
The game is split vertically by levels:
- **Level 1**: Drag and drop basic implementation.
- **Level 2**: Numbers dragging (spawning, dropping on placeholders).
- **Level 3**: Counting and physics/basket interactions.
- **Level 4 / Tracing**: Specialized subsystem for drawing, stroke validation, and path smoothing.

## Tracing Subsystem (Level 4 & System/Tracing)
A distinct and more complex system for shape/letter tracing.
- It separates inputs (`InputHandler`), visual strokes (`StrokeRenderer`), mathematical validation (`StrokeValidator`), and smoothing (`LineSmoother`).
- This marks the most decoupled and modular architecture in the project.
