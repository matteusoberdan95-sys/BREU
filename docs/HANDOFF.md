# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 03 aprovada  
**Branch:** `reboot/breu-clean-start`

---

## Retomar em 60 segundos

1. `docs/PROJECT_STATE.md`
2. `docs/technical/PLAYER_CONTROLLER_BASELINE.md` — player congelado
3. `docs/technical/HUD_DEBUG_BASELINE.md` — HUD/debug congelado
4. `docs/production/SPRINT_ROADMAP.md` — Sprint 04

---

## Sprint 03 — fechada e aprovada

| Aprovado |
|----------|
| HUD (vida, stamina, lanterna, mensagens) |
| Debug F10/F11 |
| PlayerMovementLab limpo e testável |
| Movimentação intacta |
| HUD não atrapalha a tela |

**Baselines congeladas:** player + HUD/debug.

---

## Próxima ação

**Sprint 04 — Sistema de interação mínimo**

- `IInteractable`, tecla E, raycast
- Prompt no HUD (extensão)
- Cena teste com interactables

---

## Como testar

Godot 4.7 mono → `scenes/test/PlayerMovementLab.tscn` → **F6**

Checklists: `PLAYER_MOVEMENT_LAB_PLAYTEST.md`, `HUD_DEBUG_PLAYTEST.md`

---

## Regra de produção

> Player ✅ → HUD ✅ → **interação** → pensão.
