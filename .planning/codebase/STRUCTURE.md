# Structure

## Directory Map

The standard Unity directory structure is observed:

- `Assets/`
  - `Editor/` - Custom editor extensions (if any).
  - `Font/` - Font assets.
  - `Image/` - UI and external image assets.
  - `Materials/` - Unity Materials.
  - `OutlineHuruf/` - Specific outline sprites/assets for letters.
  - `Prefabs/` - Reusable GameObjects.
  - `Scenes/` - Game levels and menus.
  - `Sprite/` - 2D sprites.
  - `TextMesh Pro/` - Generated TMPro resources.
  - `script/` - **The core codebase**.

### `Assets/script/` Folder Breakdown
- `Asasa/` - Sub-level staging (e.g., `StageManagerLv4.cs`).
- `Level1/` - Basic drag-and-drop mechanics (`DragDrop.cs`, `DropZone.cs`).
- `Level2/` - Number dropping/spawning (`NumberSpawner.cs`, `LevelCompleteManager.cs`).
- `Level3/` - Counting mechanics.
- `Level4/` - Tracing UI and mechanics (`SimpleDraw.cs`, `LetterButton.cs`, `LineSmoother.cs`).
- `MainMenu/` - Main menu canvas, buttons, and state logic (`MainMenuHandler.cs`).
- `System/` - Core reusable mechanics:
  - `System/Tracing/` - A dedicated internal library for standardizing the stroke drawing, path validation, and touch input processing.
  - Common controllers like `levelControl.cs` and `SceneLoader.cs`.
- `ThemeSelect/` - Theme/Category selection logic.
