# Core

Architectural core for Unity projects: pure C# foundation with
`ReferenceLocator`, `ApplicationBootstrap`, `ITickable` and engine
abstraction interfaces. Unity is treated as a view/adapter layer over
engine-agnostic systems and application code - see
[Documentation~/core.md](Documentation~/core.md) for the full architecture,
lifecycle and API reference.

Package: `com.enriranjan.core`

## Architecture, in short

- Unity is only the view layer. Systems and the application layer are plain
  C#, with no dependency on `UnityEngine`.
- **Services** (use cases) live for the whole session and are created once,
  by the bootstrap.
- **`ApplicationBootstrap`** (`MonoBehaviour`, `DontDestroyOnLoad`) lives in
  an empty Bootstrap scene: creates systems/services/providers, registers
  them in the `ReferenceLocator`, and only then loads the game's initial
  scene. It is the only type that writes into the locator.
- **`SceneContextInstaller`** (one per scene) is the only type that reads
  the locator; it creates controllers and injects services into them.
- **Controllers** are plain C# and implement a view's `IListener` interface;
  **views** are passive `MonoBehaviour`s that only forward input and display
  state.
- **`ITickable.Tick(float deltaTime)`** is how the bootstrap's `Update`
  drives per-frame logic in the lower layers - the only point where Unity's
  time source crosses the boundary.
- Golden rule: dependencies only point downward. No lower layer knows about
  Unity.

Full diagrams and the composition lifecycle (Bootstrap scene → `InstallBindings`
→ scene load → `SceneContextInstaller.InstallScene` → controllers → views)
are in [Documentation~/core.md](Documentation~/core.md). A runnable example
lives in [Samples~/BasicUsage](Samples~/BasicUsage/README.md).

## Structure

```
package.json          UPM manifest: id, version, dependencies, samples.
README.md              This file.
CHANGELOG.md           Version history (Keep a Changelog + SemVer).
LICENSE.md             MIT license.
.gitignore             Ignores caches/build artifacts, but NOT *.meta.
.gitattributes          Normalizes line endings, marks text/binary types.
Editor/                Editor-only tooling (can use UnityEditor/UnityEngine).
Runtime/Core/           EnriRanjan.Core - strict pure C#, noEngineReferences: true.
Runtime/Unity/          EnriRanjan.Core.Unity - Unity integration layer.
Tests/Runtime/          Plain NUnit tests for Runtime/Core (no UnityTest).
Tests/Editor/           NUnit tests that need the Unity engine (Editor mode).
Samples~/               Optional examples, importable from Package Manager.
Documentation~/         Long-form documentation, not imported as an asset.
```

Folders ending in `~` (`Samples~`, `Documentation~`) are ignored by Unity's
importer: they never appear as assets or get a `.meta`, which is exactly
what's wanted for samples (copied explicitly on import) and for Markdown/
image documentation.

## The two Runtime assemblies

- `Runtime/Core/EnriRanjan.Core.asmdef` has `"noEngineReferences": true`,
  which tells the Unity compiler to **forbid** any reference to
  `UnityEngine`/`UnityEditor` in that assembly - code that tries to use a
  Unity API there simply won't compile. This keeps the core testable with
  plain NUnit and reusable outside Unity.
- `Runtime/Unity/EnriRanjan.Core.Unity.asmdef` references `EnriRanjan.Core`
  and has `"noEngineReferences": false`, since its whole job is to be the
  Unity integration layer (`ApplicationBootstrap`, `SceneContextInstaller`,
  engine adapters).
- `Editor/EnriRanjan.Core.Editor.asmdef` also uses Unity freely, for editor
  tooling.

When adding new code, keep systems and application logic under
`Runtime/Core/`; anything that touches a `MonoBehaviour`, `Transform`,
`AudioSource`, etc. belongs under `Runtime/Unity/` (or `Editor/` if it's
editor-only tooling).

## `.meta` files are committed

Unlike a regular Unity project (where ignoring `.meta` is sometimes debated),
in a **package** the `.meta` files **must** be in the repository:

- They contain the stable GUIDs Unity uses to resolve references (prefabs,
  assets, scripts) between the package and the project consuming it.
- If they're missing or regenerated, any project with serialized references
  to this package's assets loses them.
- `.gitignore` is written for a **package repo**, not a full Unity project:
  it ignores caches (`Library/`, `Temp/`, IDE folders...) but never `*.meta`.

When you add a new file inside Unity (with the package as embedded/local),
Unity generates its `.meta` automatically - just add it to the commit too.

## Versioning

This package follows [SemVer](https://semver.org/) and
[Keep a Changelog](https://keepachangelog.com/):

- Each release is documented in `CHANGELOG.md` under `## [x.y.z] - date`.
- The version is also updated in `package.json` (`"version"`).
- A **git tag** is created with the same number, prefixed with `v`
  (`git tag v1.0.0`), since that's what allows installing a specific version
  by Git URL:

  ```
  https://github.com/enriranjan/core.git#v1.0.0
  ```

  Without `#tag`, a Git URL points at the default branch (usually `main`),
  which is useful during development but not reproducible for consumers.

## Installation

### a) By Git URL (to consume the package in another project)

In the consuming project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.enriranjan.core": "https://github.com/enriranjan/core.git#v0.1.0"
  }
}
```

You can also add it from the editor: **Package Manager → "+" → Install
package from git URL...** and paste the same URL with `#tag`.

### b) As an embedded package (while developing the package itself)

Clone (or place) this repository directly inside the consuming project's
`Packages/` folder:

```
<UnityProject>/Packages/com.enriranjan.core/
```

Unity automatically detects any folder under `Packages/` containing a
`package.json` as an "embedded" package: it shows up in the Package Manager,
is fully editable from the project, and needs no `manifest.json` entry. This
is the recommended mode while iterating on the package itself, since changes
are seen instantly and can be committed/pushed from that same checkout.

## Adding samples

Samples live under `Samples~/<SampleName>/` (with the trailing `~` so Unity
doesn't auto-import them) and are declared in `package.json`:

```json
"samples": [
  {
    "displayName": "Basic Usage",
    "description": "Minimal example of use.",
    "path": "Samples~/BasicUsage"
  }
]
```

Users import them from **Package Manager → Core → Samples → Import**, which
copies the folder into the consuming project's `Assets/Samples/...`. Add
another sample by creating a sibling folder and a matching entry in the
`samples` array.

## Documentation and changelog

- Full architecture, lifecycle and API reference:
  [Documentation~/core.md](Documentation~/core.md)
- Version history: [CHANGELOG.md](CHANGELOG.md)
