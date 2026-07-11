# HUD e Debug — Playtest (Sprint 03)

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Data:** 2026-07-11

---

## Pré-requisitos

- Godot 4.7 + .NET
- Branch `reboot/breu-clean-start`
- `dotnet build` sem erros

---

## Checklist — HUD

| Teste | Esperado | OK |
|-------|----------|-----|
| Iniciar lab (F6) | Painel inferior esquerdo: Vida, Stamina, Lanterna | ☐ |
| Sprint (Shift) | Barra e label de stamina descem | ☐ |
| Parar sprint | Stamina regenera após ~0,75 s | ☐ |
| F — lanterna | Label muda Ligada/Desligada | ☐ |
| Mensagem inicial | Texto central inferior por ~3,5 s | ☐ |
| F9 reset | Mensagem "Posicao resetada" | ☐ |

---

## Checklist — Debug

| Teste | Esperado | OK |
|-------|----------|-----|
| Painel superior direito | Mostra estado F10/F11 | ☐ |
| F10 (toggle inf.) | Mensagem debug + label lanterna "(debug inf.)" | ☐ |
| F10 OFF + F ligada 2 min | Bateria desce gradualmente | ☐ |
| F10 ON + F ligada 10+ min | Bateria **permanece** 100/100 | ☐ |
| F11 | Fog fica mais clara (densidade reduzida) | ☐ |
| F11 novamente | Fog volta ao normal | ☐ |

---

## Checklist — Regressão player

| Teste | Esperado | OK |
|-------|----------|-----|
| WASD | Direções corretas | ☐ |
| Sprint / crouch / lean / look back | Igual Sprint 02 aprovada | ☐ |
| Camera feel | Sem regressão perceptível | ☐ |

---

## Notas

- Autoload: `PlaytestDebugSettings` (`project.godot`)
- HUD: `scenes/ui/HUD.tscn` — instanciado em `PlayerMovementLab/UI/HUD`
- Baseline player: `docs/technical/PLAYER_CONTROLLER_BASELINE.md`
