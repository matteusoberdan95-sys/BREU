# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 03 aprovada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — player congelado |
| Sprint 03 | **✅ Aprovada pelo usuário** — HUD/debug congelado |
| PlayerMovementLab | **✅ Aprovado** |
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
| Movimentação geral | ✅ Aprovada |

**Regra:** Não alterar `PlayerController`, `PlayerCameraFeel`, look back, lean, sprint, crouch, stamina ou camera bob sem solicitação explícita ou nova sprint de player.

---

## HUD e Debug — baseline congelada

**HUD:** `res://scenes/ui/HUD.tscn`  
**Debug:** autoload `PlaytestDebugSettings`  
**Documentação:** `docs/technical/HUD_DEBUG_BASELINE.md`

| Sistema | Status |
|---------|--------|
| HUD visível e responsivo | ✅ Aprovado |
| Vida | ✅ Aprovado |
| Stamina | ✅ Aprovado |
| Lanterna | ✅ Aprovado |
| Debug F10/F11 | ✅ Aprovado |
| Mensagens temporárias | ✅ Aprovado |
| Não atrapalha a tela | ✅ Aprovado |

**Regra:** Não alterar HUD base, `PlaytestDebugSettings`, display de stamina/lanterna debug ou layout aprovado sem solicitação explícita ou sprint dedicada de HUD/debug.

---

## Cena principal

**F5:** `res://scenes/levels/BootstrapEmpty.tscn`  
**F6 (playtest movimento + HUD):** `res://scenes/test/PlayerMovementLab.tscn`

---

## Próxima sprint

**Sprint 04 — Sistema de interação mínimo**

- `IInteractable`, raycast, tecla E
- Prompt no HUD (extensão — sem reescrever baseline)
- Cena teste com 2–3 interactables

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/technical/PLAYER_CONTROLLER_BASELINE.md`
3. `docs/technical/HUD_DEBUG_BASELINE.md`
4. Executar Sprint 04

---

## Relatórios

- Sprint 02 + lab: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- Sprint 03 HUD: `docs/testing/HUD_DEBUG_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
