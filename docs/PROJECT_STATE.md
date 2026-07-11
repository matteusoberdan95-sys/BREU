# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 03 concluída  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — player congelado |
| Sprint 03 | **✅ Concluída** — HUD e Debug |
| Sprint 04 | **Próxima** — Interação mínima |

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

**Regra:** Não alterar `PlayerController`, `PlayerCameraFeel`, look back, lean, sprint, crouch, stamina ou camera bob sem solicitação explícita.

---

## HUD e Debug (Sprint 03)

**HUD:** `res://scenes/ui/HUD.tscn`  
**Debug:** autoload `PlaytestDebugSettings`  
**Documentação:** `docs/technical/HUD_DEBUG_BASELINE.md`

| Sistema | Status |
|---------|--------|
| Vida | ✅ Display |
| Stamina | ✅ Barra + label |
| Lanterna | ✅ Bateria + estado |
| Mensagens temporárias | ✅ |
| Debug F10/F11 | ✅ Lanterna inf. / fog |

---

## Cena principal

**F5:** `res://scenes/levels/BootstrapEmpty.tscn`  
**F6 (playtest movimento + HUD):** `res://scenes/test/PlayerMovementLab.tscn`

---

## Próxima sprint

**Sprint 04 — Sistema de interação mínimo**

- `IInteractable`, raycast, tecla E
- Prompt no HUD
- Cena teste com 2–3 interactables

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/technical/HUD_DEBUG_BASELINE.md`
3. Executar Sprint 04

---

## Relatórios

- Sprint 02: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- Sprint 03: `docs/testing/HUD_DEBUG_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
