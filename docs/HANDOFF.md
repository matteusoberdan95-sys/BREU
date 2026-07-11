# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 03 concluída  
**Branch:** `reboot/breu-clean-start`

---

## Retomar em 60 segundos

1. `docs/PROJECT_STATE.md`
2. `docs/technical/PLAYER_CONTROLLER_BASELINE.md` — player congelado
3. `docs/technical/HUD_DEBUG_BASELINE.md` — HUD + debug
4. `docs/production/SPRINT_ROADMAP.md` — Sprint 04

---

## Sprint 03 — fechada

| Entregue |
|----------|
| HUD (vida, stamina, lanterna, mensagens) |
| `PlaytestDebugSettings` (F10 lanterna inf., F11 fog) |
| `PlayerHealth` + bateria na lanterna |
| Lab com HUD + fog para teste |

**Player baseline intacta** — Controller, CameraFeel, etc. não alterados.

---

## Próxima ação

**Sprint 04 — Sistema de interação mínimo**

- `IInteractable`, tecla E, raycast
- Prompt no HUD
- Cena teste com interactables

---

## Como testar

Godot 4.7 mono → `scenes/test/PlayerMovementLab.tscn` → **F6**

Checklist: `docs/testing/HUD_DEBUG_PLAYTEST.md`

---

## Regra de produção

> Player baseline congelada → HUD ✅ → interação → pensão.
