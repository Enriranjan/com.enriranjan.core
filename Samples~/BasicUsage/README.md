# Basic Usage Sample

Minimal end-to-end example of the Core architecture: a pure C# system, a
service that wraps it, and the Unity-side bootstrap/installer/controller/view
that wire it into a scene.

Samples live under `Samples~` (note the trailing `~`) so Unity ignores this
folder by default and it is never imported automatically with the package.
Import it from **Package Manager > Core > Samples > Import**, which copies
this folder into the consuming project's `Assets/Samples/` directory.

## What's here

`Scripts/` contains:

- `FakeScoreSystem` - pure C# system holding the score. Stands in for a real
  gameplay system.
- `ScoreService` - service (use case) wrapping `FakeScoreSystem`, exposing
  `CurrentScore` and `RegisterPoints(int)` to controllers.
- `ScoreView` - passive `MonoBehaviour` view. No game logic: forwards a
  button press to its listener and displays whatever score it's told to.
- `ScoreController` - pure C# controller implementing `ScoreView.IListener`.
  Owns no Unity references beyond the view it was handed.
- `ExampleApplicationBootstrap` - concrete `ApplicationBootstrap`. Creates
  `FakeScoreSystem` and `ScoreService` and registers the service.
- `ExampleSceneContextInstaller` - concrete `SceneContextInstaller`. Reads
  `ScoreService` from the locator and creates the `ScoreController`, wiring it
  to a `ScoreView` in the scene.

## Setting it up in a project

1. Create a **Bootstrap** scene containing an empty GameObject with
   `ExampleApplicationBootstrap` attached. Set its `Initial Scene Name` field
   to the name of the gameplay scene created in step 2, and add both scenes
   to Build Settings.
2. Create a **Gameplay** scene containing:
   - A GameObject with `ExampleSceneContextInstaller`, referencing...
   - ...a `ScoreView` in the same scene (e.g. on a UI Canvas), optionally
     wired to a Button's `OnClick` calling `OnAddPointsButtonPressed`.
3. Press Play from the **Bootstrap** scene (not the Gameplay scene directly -
   see `SceneContextInstaller`'s guard for what happens otherwise).

See [Documentation~/core.md](../../Documentation~/core.md) for the full
architecture and lifecycle this sample demonstrates.

## Adding more samples

To add another sample, create a sibling folder next to `BasicUsage/` and
register it in the `samples` array of `package.json`:

```json
{
  "displayName": "Another Sample",
  "description": "What it demonstrates.",
  "path": "Samples~/AnotherSample"
}
```
