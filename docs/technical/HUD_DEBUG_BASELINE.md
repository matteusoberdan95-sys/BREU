# BREU — HUD e Debug Baseline

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 03 — aprovada e congelada  
**Status:** OFICIAL — não alterar sem nova sprint de HUD/debug

---

## Regra de congelamento

> **Não modificar** `HUD.tscn`, `HUDController`, layout aprovado, `PlaytestDebugSettings`, teclas F10/F11 ou display de vida/stamina/lanterna/debug **sem solicitação explícita do usuário** ou sprint dedicada de HUD/debug.

Sprint 04+ (interação, levels) podem **estender** o HUD (ex.: prompt `[E]`) — não reescrever o baseline.

Player baseline (`PLAYER_CONTROLLER_BASELINE.md`) permanece congelada em paralelo.

---

## Arquivos

| Item | Caminho |
|------|---------|
| HUD cena | `scenes/ui/HUD.tscn` |
| HUD script | `scripts/ui/HUDController.cs` |
| Debug flags | `scripts/debug/PlaytestDebugSettings.cs` (autoload) |
| Vida (fonte) | `scripts/player/PlayerHealth.cs` |
| Stamina (fonte) | `scripts/player/PlayerStamina.cs` — **somente leitura via sinal** |
| Lanterna (fonte) | `scripts/player/PlayerFlashlight.cs` — **somente leitura via sinal** |

---

## Estrutura do HUD (HUD.tscn)

```
CanvasLayer: HUD                    ← HUDController (layer 0)
└── Root (Control, full rect, mouse_filter=IGNORE)
    ├── StatusPanel (PanelContainer, canto inferior esquerdo)
    │   └── Margin → VBox
    │       ├── HealthLabel         ← "Vida current/max"
    │       ├── StaminaBar          ← ProgressBar
    │       ├── StaminaLabel        ← "Stamina current/max"
    │       └── LanternLabel        ← bateria + Ligada/Desligada
    ├── MessagePanel (centro inferior)
    │   └── MessageLabel            ← mensagens temporárias
    └── DebugPanel (canto superior direito)
        └── DebugLabel              ← estado F10/F11
```

**Estilo:** painéis escuros semi-transparentes; texto legível; **sem obstruir o centro da tela**.

---

## Informações exibidas

| Elemento | Fonte | Formato |
|----------|-------|---------|
| Vida | `PlayerHealth.HealthChanged` | `Vida {current}/{max}` |
| Stamina | `PlayerStamina.StaminaChanged` | Barra + `Stamina {current}/{max}` |
| Lanterna | `PlayerFlashlight.BatteryChanged` + `LanternToggled` | `Lanterna {current}/{max} — Ligada/Desligada` (+ `(debug inf.)` se F10 ON) |
| Mensagem | `HUDController.ShowMessage()` | Texto central, auto-hide ~3,5 s |
| Debug | `PlaytestDebugSettings` (polling) | `Debug F10: lanterna … \| F11: fog …` |

---

## Teclas debug

| Tecla | Ação | Flag |
|-------|------|------|
| **F10** | Toggle lanterna infinita | `InfiniteLanternBattery` (default `true`) |
| **F11** | Toggle fog reduzida | `ReducedFog` (default `false`) |

Autoload: `PlaytestDebugSettings` em `project.godot`.

Fog: localiza `WorldEnvironment` na cena ativa e ajusta `FogDensity` (`0.045` normal → `0.008` reduzida no lab).

---

## API pública (extensões permitidas)

```csharp
// Mensagem temporária — usado por levels, debug, interação futura
HUDController.FindActive(GetTree())?.ShowMessage("Texto", 3.5f);
```

Grupos:
- `"game_hud"` — instância ativa do HUD
- `"playtest_debug_settings"` — autoload debug

---

## Integração em levels

```
LevelRoot
├── WorldEnvironment              ← fog testável com F11
├── Player                        ← instance Player.tscn
└── UI
    └── HUD                       ← instance HUD.tscn
```

**Cena de teste oficial:** `res://scenes/test/PlayerMovementLab.tscn` (F6)

---

## Aprovação do usuário (2026-07-11)

- [x] HUD visível e responsivo
- [x] Vida, stamina e lanterna corretas
- [x] Debug F10/F11 visível e funcional
- [x] PlayerMovementLab limpo e testável
- [x] Movimentação do player intacta
- [x] HUD não atrapalha a tela

---

## Próxima extensão esperada

**Sprint 04:** prompt de interação `[E] …` — adicionar ao HUD **sem** alterar painéis aprovados.
