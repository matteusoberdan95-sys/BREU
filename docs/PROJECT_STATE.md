# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 02 aprovada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada pelo usuário** |
| PlayerMovementLab | **✅ Aprovado** |
| Movimentação base | **🔒 Congelada** — ver baseline |
| Sprint 03 | **Próxima** — HUD e Debug |

---

## Player — baseline congelada

**Cena:** `res://scenes/player/Player.tscn`  
**Teste oficial:** `res://scenes/test/PlayerMovementLab.tscn`  
**Documentação:** `docs/technical/PLAYER_CONTROLLER_BASELINE.md`

| Sistema | Status |
|---------|--------|
| WASD + mouse | ✅ Aprovado |
| Sprint + stamina | ✅ Aprovado |
| Crouch | ✅ Aprovado |
| Lean Q/R | ✅ Aprovado |
| Look back Alt/X | ✅ Aprovado |
| Camera feel (BreuDefault) | ✅ Aprovado |

**Regra:** Não alterar `PlayerController`, `PlayerCameraFeel`, look back, lean, sprint, crouch, stamina ou camera bob sem solicitação explícita ou nova sprint de player.

---

## Cena principal

**F5:** `res://scenes/levels/BootstrapEmpty.tscn`  
**F6 (playtest movimento):** `res://scenes/test/PlayerMovementLab.tscn`

---

## Próxima sprint

**Sprint 03 — HUD e Debug**

- HUD: vida, stamina, lanterna (display)
- `PlaytestDebugSettings`: flags de debug
- **Sem** alterar player baseline

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/technical/PLAYER_CONTROLLER_BASELINE.md`
3. Executar Sprint 03

---

## Relatórios

- Sprint 02: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
