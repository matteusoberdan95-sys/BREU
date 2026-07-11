# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 02 aprovada  
**Branch:** `reboot/breu-clean-start`

---

## Retomar em 60 segundos

1. `docs/PROJECT_STATE.md`
2. `docs/technical/PLAYER_CONTROLLER_BASELINE.md` — **baseline congelada**
3. `docs/production/SPRINT_ROADMAP.md` — Sprint 03
4. `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`

---

## Sprint 02 — fechada

Movimentação FPS aprovada pelo usuário. **Não mexer no player** sem solicitação explícita.

| Aprovado |
|----------|
| WASD, sprint, crouch, lean, look back, camera feel |

---

## Próxima ação

**Sprint 03 — HUD e Debug**

- HUD: vida, stamina, lanterna (display)
- Flags de debug para playtest
- **Não** alterar PlayerController / PlayerCameraFeel

---

## Como testar player (regressão)

Godot 4.7 mono → `scenes/test/PlayerMovementLab.tscn` → **F6**

---

## Regra de produção

> Primeiro jogável, depois bonito.  
> Player baseline congelada → HUD → interação → pensão.
