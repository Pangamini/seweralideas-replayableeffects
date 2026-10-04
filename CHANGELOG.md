# Changelog

## [0.2.0]
Needs `seweralideas-unityutils` 0.2.0 or later (`SeweralIdeas.ObjectPooling`).

### Added
- `PrewarmEffect`: plays an effect once, a little way in, for a frame, to warm up its particle systems and shaders.

### Changed
- Effects are pooled by `SeweralIdeas.ObjectPooling`. `ReplayableEffect` is a `Poolable`: call `Spawn(scene, position, rotation[, fastForward])` (or with a `Transform`) on the prefab, and the pooled instance that plays releases itself when its duration is up. `Stop()` ends it early.
- The pools are per scene, under that scene's `ObjectPoolManager`, and are gone when it unloads. They used to live in a `DontDestroyOnLoad` object, so fill them with a `PoolPrewarmer` (from `SeweralIdeas.ObjectPooling`) in the scene that plays the effects.
- `ReplayableEffect.Play()` still plays the components of the instance in place. An instance that is not pooled (one placed in a scene to look at) does not release itself, nor is it destroyed, when it is done.

### Removed
- `EffectPool` (the ScriptableObject and its assets), `PooledEffect` and `EffectsManager`: the pooling is `ObjectPool`'s and `ObjectPoolManager`'s now. Fields that held an `EffectPool` hold the `ReplayableEffect` prefab instead.

## [0.1.0]
### Package created
