# Changelog

All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0] - 2026-07-08

### Added

- `EnriRanjan.Core` (`Runtime/Core/`, `noEngineReferences: true`): pure C#
  foundation.
  - `ITickable` - per-frame update contract for pure C# objects.
  - `IReferenceLocator` / `ReferenceLocator` - type-keyed registry of
    systems/services/providers; read access is public, write access
    (`Register<T>`, `Clear`) is `internal`, restricted via
    `InternalsVisibleTo` to `EnriRanjan.Core.Unity` and this package's tests.
  - `EngineInterfaces.ITransformable` and `EngineInterfaces.IAudioPlayer`,
    with engine-agnostic `Vector3Data` / `QuaternionData` value types.
- `EnriRanjan.Core.Unity` (`Runtime/Unity/`): Unity integration layer.
  - `ApplicationBootstrap` - abstract `MonoBehaviour` composition root;
    creates the `ReferenceLocator`, calls `InstallBindings()`, then loads the
    initial scene; drives registered `ITickable`s from its `Update`.
  - `SceneContextInstaller` - abstract `MonoBehaviour` scene composition
    root; reads the locator populated by the bootstrap and creates the
    scene's controllers.
  - `Adapters.TransformableAdapter` and `Adapters.AudioPlayerAdapter` -
    engine adapters wrapping `Transform` and `AudioSource`.
- Tests: pure NUnit coverage for `ReferenceLocator` and `ITickable` under
  `Tests/Runtime/`, plus an Editor test verifying `ApplicationBootstrap`
  registers bindings and tickables correctly.
- `Samples~/BasicUsage`: end-to-end example (`FakeScoreSystem`,
  `ScoreService`, `ScoreController`, `ScoreView`, and concrete
  bootstrap/installer subclasses).
- Documentation: architecture layers, composition lifecycle and API
  reference in `Documentation~/core.md` and the README.
