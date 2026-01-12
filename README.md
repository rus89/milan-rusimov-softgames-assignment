# Softgames Unity Developer Assignment

[🎮 **Play the WebGL Build Here**](https://rus89.github.io/milan-rusimov-softgames-assignment/)

This project contains the implementation of the three required tasks: Ace of Shadows, Magic Words, and Phoenix Flame. It is built with Unity 6 and focuses on clean architecture, decoupling, and resilience.

## 🏗 Architecture Overview
The project follows a Service Locator pattern to manage dependencies and separate Logic from Presentation.

- **Bootstrap**: The entry point (`_Bootstrap` scene) initializes core services (`AudioService`, `SceneLoaderService`, `MagicWordsService`) before loading the application.
- **Decoupling**:
  - **Logic (Controllers)**: Handle state, data fetching, and business rules. They are unaware of specific View implementations.
  - **Views**: "Dumb" components that strictly render data provided by Controllers.
  - **Services**: Handle cross-cutting concerns (Audio, Scene Management) and Data I/O.
- **Async/Await**: Used extensively via `UniTask` for non-blocking, efficient asynchronous operations (API calls, delays, sequence orchestration).

## 🚀 Getting Started
1. Open the project in Unity 6.
2. Crucial: Start playing from the `_Bootstrap` scene.
   - _This ensures all Services are registered correctly. Starting directly in a task scene may cause dependency errors._
3. Use the Main Menu to navigate between tasks.

## 🃏 Task 1: Ace of Shadows
A card stacking simulation demonstrating 2D sorting and sequence management.

- **Key Tech**:
  - Interleaved Sorting: Uses `Order In Layer = Index * 2` to allow shadows to render correctly between cards without breaking batching.
  - **PrimeTween**: Used for high-performance, allocation-free tweens.
- **Editor Testing Feature**:
  - The card movement can be slow (144 seconds).
  - **In Editor Only**: Select the `AceOfShadowsController` GameObject and use the `Simulation Speed` slider to speed up time up to 100x for rapid verification.

## 💬 Task 2: Magic Words
A dynamic chat system that consumes a remote API to render dialogue and avatars.

- **Data Handling**:
  - **"Last-One-Wins" Strategy**: Solves duplicate data issues in the API (e.g., the "Sheldon" conflict) by allowing valid entries to overwrite broken ones.
- **Resilience**:
  - **Missing Avatars**: "Neighbour" entries (missing from API) gracefully fallback to a default silhouette.
  - **Broken URLs**: 404/Timeout errors are caught silently, showing a default avatar instead of crashing or showing white boxes.
- **UI Architecture**:
  - **Dynamic Layouts**: Chat bubbles automatically swap alignment (Left vs. Right) and color based on the API's `position` property.
  - **Emoji Parsing**: A custom `EmojiParser` converts text tags (e.g., `{satisfied}`) into high-quality TMP Sprites (EmojiOne).
  - **Smart Layouts**: Bubbles feature a "Max Width" clamp, forcing text to wrap while allowing the bubble to shrink for short messages.

## 🔥 Task 3: Phoenix Flame
A particle effect demo showcasing Animator-driven logic.

- **Requirement**: "Control fire colour using an animator controller."
- **Implementation**:
  - The UI Button triggers an Animator State change.
  - **StateMachineBehaviours** on the Animator states drive the logic, keeping the Controller script reactive and clean.
  - Colors are interpolated in `Update()` for smooth transitions, even if the Animator state snaps instantly.
- **Visuals**: Uses a multi-layered Particle System (Core, Sparks, Glow) in **Linear Color Space** for additive blending accuracy.

---

## 🛠 External Assets & Libraries
- [**UniTask**](https://github.com/Cysharp/UniTask): For efficient async/await integration in Unity.
- [**PrimeTween**](https://github.com/KyryloKuzyk/PrimeTween): For high-performance, GC-free tweening.
- [**Noto Emoji**](https://fonts.google.com/noto/specimen/Noto+Emoji): For Unicode support.
- [**GUI Pro - Casual Game | 2D GUI**](https://assetstore.unity.com/packages/2d/gui/gui-pro-casual-game-176695): For visual presentation
- [**FREE Casual Game SFX Pack**](https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116): For SFX
