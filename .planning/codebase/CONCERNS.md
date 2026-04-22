# Concerns & Tech Debt

## Hardcoded Values (Magic Numbers & Strings)
- The codebase relies heavily on hardcoded strings (e.g., `"membaca"`, `"menulis"`) to switch states or load scenes. This makes renaming categories risky.
- Hardcoded integers are used to derive characters using ASCII manipulation (e.g., `48 + i` for '1', `i + 65` for 'A'). If the order changes, this will silently fail.

## Public Variables & Encapsulation
- Variables are often declared `public` merely to expose them to the Unity Inspector, rather than utilizing `[SerializeField] private`. This pollutes the public API of components, increasing the risk of unwanted external modifications.

## Component Coupling
- High coupling between UI instantiation and logic processing. For example, `MainMenuHandler` directly manipulates Transform trees (`Destroy(child.gameObject)`) and manually instantiates new GameObjects while also keeping track of the selected state.
- Separation of Concerns could be improved by using MVP (Model-View-Presenter) or an event-based architecture (Action/UnityEvent) to decouple UI updates from game logic.

## Singleton Over-Usage
- Frequent reliance on Singletons (e.g., `public static MainMenuHandler Instance;`), usually initialized in `Awake()` without checking for pre-existing instances or ensuring cross-scene persistence robustly.

## Scalability
- "Level X" prefixing limits structural scalability. If the game extends to 50 levels, this flat organizational structure may become unmanageable.
