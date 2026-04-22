# Testing

## QA Strategy
Currently, there is no formal testing strategy. The project appears to rely entirely on **Manual Playtesting** inside the Unity Editor and on actual deployed devices.

## Automated Tests
- **Missing**: There are no `Tests` or `PlayModeTests` directories.
- The `Unity Test Framework` is either not used or not configured to run any custom unit/integration tests for the core logic.

## Validation Mechanisms
- **Stroke/Trace Validation**: Inside `Level 4` / `Tracing System`, there is programmatic geometric validation of paths/strokes (`StrokeValidator.cs`, `PointerAlignmentChecker.cs`). This acts as the closest thing to an automated validation logic within the gameplay itself.

## Recommendations
- Implement Unity Test Framework.
- Start by adding EditMode tests for isolated pure C# logic (like `LineSmoother.cs` or `Path.cs`).
- Use PlayMode tests to simulate Drag and Drop mechanics for Level 1/2/3 to prevent regressions.
