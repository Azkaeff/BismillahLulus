# Conventions

## Coding Style & Patterns

### 1. Naming Conventions
- **Classes**: PascalCase (e.g., `MainMenuHandler`, `StarContainerController`). Some exceptions exist (`levelControl` uses camelCase).
- **Public Variables**: camelCase or camelCase with underscores (e.g., `shapeSprites`, `ItemPrefab_TextBased`). Frequent deviations occur between PascalCase and camelCase for public/inspector exposed fields.
- **Private Variables**: Often prefixed with an underscore or just camelCase (`_item`, `panelName`).
- **File Names**: Usually match class names, though casing sometimes differs between the file on disk and the class inside.

### 2. Inspector Visualization
- Heavy use of `public` variables over `[SerializeField] private`. This exposes fields publicly where it may not strictly be required from an OOP perspective, typical in prototype Unity projects.
- `[HideInInspector]` used occasionally to hide public state variables (e.g., `panelName`).

### 3. State Management
- Simple `switch` statements over strings or enums to handle categories (e.g., `"membaca"`, `"menulis"`, `"berhitung"`).
- Magic numbers are frequently used to map ASCII values directly to UI elements (e.g., `i + 65` for 'A', `48 + i` for numbers).

### 4. Code Health
- High coupling between UI handling and Game Logic. Classes like `MainMenuHandler` handle both the UI presentation (instantiating prefabs) and the core game state.
