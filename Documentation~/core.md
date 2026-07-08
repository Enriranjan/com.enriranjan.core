# Core

Architectural core: pure C# foundation with ReferenceLocator, ApplicationBootstrap, ITickable and engine abstraction interfaces. Keeps game logic engine-agnostic — Unity is just the view.

Package: `com.enriranjan.core`

## Table of Contents

- [Installation](#installation)
- [Getting Started](#getting-started)
- [Concepts](#concepts)
- [Layers](#layers)
- [Lifecycle](#lifecycle)
- [API Reference](#api-reference)
- [Samples](#samples)
- [Changelog](#changelog)

## Installation

See the main [README](../README.md#instalación) for installation instructions
(Git URL or embedded package).

## Getting Started

The smallest working setup is:

1. An empty **Bootstrap** scene with a GameObject holding a concrete
   subclass of `ApplicationBootstrap` that overrides `InstallBindings()` to
   create your systems/services/providers and `Register()` them.
2. A gameplay scene with a GameObject holding a concrete subclass of
   `SceneContextInstaller` that overrides `InstallScene(IReferenceLocator)` to
   create controllers and inject the services they need.
3. Build Settings includes both scenes; the project is launched from
   **Bootstrap**.

See [Samples~/BasicUsage](../Samples~/BasicUsage/README.md) for a complete,
runnable version of this with a `ScoreService`.

## Concepts

- **Unity is only the view layer.** Systems and the application layer are
  plain C#, with no dependency on `UnityEngine`.
- **Services** are the use cases of the application. They live for the whole
  session and are created once, by the bootstrap.
- **`ApplicationBootstrap`** (`MonoBehaviour`, `DontDestroyOnLoad`) lives in
  the Bootstrap scene. It creates systems/services/providers, registers them
  in the `ReferenceLocator`, and only once everything is initialized does it
  load the game's initial scene. It is the **only** type that writes into the
  locator.
- **`SceneContextInstaller`** (`MonoBehaviour`, one per scene) is the
  **only** type that reads the locator. It creates controllers and injects
  services into them. Because `ApplicationBootstrap` only loads a scene after
  `InstallBindings()` finishes, its `Awake` can assume everything is already
  initialized.
- **Controllers** are plain C# and implement a view's `IListener` interface.
- **Views** are passive `MonoBehaviour`s holding a reference to their
  listener (conventionally named `myListener`); they contain no game logic.
- **`ITickable`** exposes `Tick(float deltaTime)`. The bootstrap
  automatically registers any `ITickable` it creates and calls `Tick` on it
  from its own `Update` - the only point where Unity's time source enters the
  lower layers.
- **Golden rule: dependencies only point downward.** No lower layer knows
  about Unity.

## Layers

```
┌─────────────────────────────────────────────────────────────┐
│ Unity (view)                                                 │
│   Views (MonoBehaviour, passive, "myListener")                │
│   SceneContextInstaller (reads the locator, creates controllers)│
│   ApplicationBootstrap (writes the locator, drives ITickable)  │
│   Engine adapters (TransformableAdapter, AudioPlayerAdapter…)  │
│                          ▲ implements engine interfaces        │
├──────────────────────────┼────────────────────────────────────┤
│ Application / Systems (pure C#)                                │
│   Controllers (implement View.IListener)                       │
│   Services (use cases, created by the bootstrap)                │
│   Systems (domain logic)                                        │
│   ReferenceLocator / IReferenceLocator / ITickable               │
│   Engine interfaces (ITransformable, IAudioPlayer…)               │
└─────────────────────────────────────────────────────────────┘

Dependencies only point downward: Unity depends on the pure layer,
never the other way around.
```

Assemblies:

- `EnriRanjan.Core` (`Runtime/Core/`) - `noEngineReferences: true`. Strict
  pure C#: `ITickable`, `IReferenceLocator`, `ReferenceLocator`, and the
  engine-agnostic interfaces under `EngineInterfaces/`
  (`ITransformable`, `IAudioPlayer`, and their `Vector3Data`/`QuaternionData`
  value types).
- `EnriRanjan.Core.Unity` (`Runtime/Unity/`) - references
  `EnriRanjan.Core`, `noEngineReferences: false`. Contains
  `ApplicationBootstrap`, `SceneContextInstaller`, and engine adapters under
  `Adapters/` (`TransformableAdapter`, `AudioPlayerAdapter`).

`ReferenceLocator.Register<T>` and `.Clear()` are `internal`; only
`EnriRanjan.Core.Unity` (via `ApplicationBootstrap.Register<T>`) and this
package's test assemblies can call them - the compiler, not convention,
enforces that only the bootstrap writes to the locator.

## Lifecycle

```
1. Player presses Play on the Bootstrap scene.
2. ApplicationBootstrap.Awake():
   a. DontDestroyOnLoad(gameObject)
   b. creates a new ReferenceLocator
   c. calls the abstract InstallBindings() →
      concrete bootstrap creates systems/services/providers and calls
      Register<T>() for each; ITickable instances are also queued for Update.
   d. sets IsReady = true, raises the Ready event
   e. loads the initial scene (SceneManager.LoadScene, Single mode)
3. Initial scene loads. SceneContextInstaller.Awake():
   a. asserts ApplicationBootstrap.IsReady (guards against opening the scene
      directly instead of via Bootstrap - see the class for the exact
      Editor/Player behavior)
   b. calls the abstract InstallScene(IReferenceLocator) →
      concrete installer reads services with locator.Get<T>() and creates
      controllers, injecting those services and the scene's views.
4. Controllers implement View.IListener; views forward input to their
   listener and expose methods for the controller to update what's displayed.
5. Every frame, ApplicationBootstrap.Update() calls Tick(Time.deltaTime) on
   every registered ITickable.
```

## API Reference

### `EnriRanjan.Core` (pure C#)

- `ITickable` - `void Tick(float deltaTime)`.
- `IReferenceLocator` - `T Get<T>()`, `bool TryGet<T>(out T)`, `bool Contains<T>()`.
- `ReferenceLocator : IReferenceLocator` - the concrete registry. `Register<T>`
  and `Clear` are internal.
- `EngineInterfaces.ITransformable` - `Position`, `Rotation`, `Scale`
  (`Vector3Data`/`QuaternionData`, engine-agnostic value types).
- `EngineInterfaces.IAudioPlayer` - `Play(string clipId)`, `Stop()`, `Volume`.

### `EnriRanjan.Core.Unity`

- `ApplicationBootstrap` (abstract `MonoBehaviour`) - override
  `InstallBindings()`; call `Register<T>(instance)` for each
  system/service/provider. Static `IsReady` / `Ready` event exposed for
  other systems that need to know once composition is complete.
- `SceneContextInstaller` (abstract `MonoBehaviour`) - override
  `InstallScene(IReferenceLocator locator)`.
- `Adapters.TransformableAdapter : ITransformable` - wraps a `Transform`.
- `Adapters.AudioPlayerAdapter : IAudioPlayer` (`MonoBehaviour`) - wraps an
  `AudioSource` plus a serialized clip-id-to-`AudioClip` list.

## Samples

See [Samples~/BasicUsage](../Samples~/BasicUsage/README.md) for a full
worked example (`FakeScoreSystem`, `ScoreService`, `ScoreController`,
`ScoreView`, and concrete bootstrap/installer subclasses).

## Changelog

See [CHANGELOG.md](../CHANGELOG.md).
