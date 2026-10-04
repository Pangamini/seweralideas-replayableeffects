# Changelog

## [0.2.0]
### Changed
- Effects are pooled by `SeweralIdeas.ObjectPooling` (in `seweralideas-unityutils`). A `ReplayableEffect` is a `Poolable`: call `Spawn(scene, position, rotation[, fastForward])` on the prefab, and the instance that plays returns to the pool by itself when its duration is up.
- `PrewarmEffect` takes the effect prefab, and plays its instances once to warm them up.

### Removed
- `EffectPool` (the ScriptableObject), `PooledEffect` and `EffectsManager`: the pooling is `ObjectPool`'s and `ObjectPoolManager`'s now.

## [0.1.0]
### Package created
