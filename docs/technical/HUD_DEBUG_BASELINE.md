# BREU — HUD e Debug (Sprint 03)

**Versão:** 1.0  
**Data:** 2026-07-11

---

## Arquivos

| Item | Caminho |
|------|---------|
| HUD | `scenes/ui/HUD.tscn` |
| Controller | `scripts/ui/HUDController.cs` |
| Debug flags | `scripts/debug/PlaytestDebugSettings.cs` (autoload) |
| Vida | `scripts/player/PlayerHealth.cs` |
| Lanterna | `scripts/player/PlayerFlashlight.cs` (bateria + sinais) |

---

## HUD

`HUDController` encontra o player via grupo `"player"` e conecta:

- `PlayerHealth.HealthChanged` → label Vida
- `PlayerStamina.StaminaChanged` → barra + label
- `PlayerFlashlight.BatteryChanged` / `LanternToggled` → label Lanterna

**API pública:**

```csharp
HUDController.FindActive(GetTree())?.ShowMessage("Texto", 3.5f);
```

Grupo: `"game_hud"`

---

## PlaytestDebugSettings

Autoload em `project.godot`.

| Flag | Default | Tecla |
|------|---------|-------|
| `InfiniteLanternBattery` | `true` | F10 |
| `ReducedFog` | `false` | F11 |

Fog: localiza `WorldEnvironment` na cena atual e ajusta `FogDensity`.

---

## Integração em levels

```
LevelRoot
├── WorldEnvironment
├── Player          ← instance Player.tscn
└── UI
    └── HUD         ← instance HUD.tscn
```

Debug global via autoload; não é obrigatório nó `Debug/PlaytestDebug` na cena.

---

## Regra

Não alterar scripts congelados do baseline de movimento. HUD **consome** sinais; não modifica feel.
