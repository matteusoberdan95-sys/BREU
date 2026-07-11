# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 04 concluída  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — player congelado |
| Sprint 03 | **✅ Aprovada** — HUD/debug congelado |
| Sprint 04 | **✅ Concluída** — interação mínima |
| Sprint 05 | **Próxima** — Pensão térreo blockout 01 |

---

## Player — baseline congelada

**Documentação:** `docs/technical/PLAYER_CONTROLLER_BASELINE.md`  
**Teste movimento:** `scenes/test/PlayerMovementLab.tscn`

Movimentação, camera feel, sprint/crouch/stamina/lean/look back — **congelados**.

---

## HUD — baseline congelada

**Documentação:** `docs/technical/HUD_DEBUG_BASELINE.md`  
**Extensão Sprint 04:** prompt `[E]` — ver interação abaixo.

---

## Interação (Sprint 04)

**Cena teste:** `res://scenes/test/InteractionLab.tscn`  
**Documentação:** `docs/technical/INTERACTION_SYSTEM_BASELINE.md`

| Sistema | Status |
|---------|--------|
| Raycast câmera 2.5 m | ✅ |
| IInteractable + Interactable | ✅ |
| Prompt HUD | ✅ |
| Mensagem 3 s | ✅ |
| 3 objetos teste | ✅ |
| Debug mira (console) | ✅ |

---

## Cenas de teste

| F6 sugerido | Cena |
|-------------|------|
| Movimento + HUD | `PlayerMovementLab.tscn` |
| Interação | `InteractionLab.tscn` |

**F5:** `BootstrapEmpty.tscn`

---

## Próxima sprint

**Sprint 05 — Pensão térreo blockout 01**

- Trilha, varanda, recepção, corredor, quarto 102, cozinha, depósito
- Blockout cinza — **sem** teto, escada ou 2º andar
- Reutilizar sistema de interação para placeholders

Ver: `docs/production/SPRINT_ROADMAP.md`

---

## Como retomar

1. `docs/HANDOFF.md`
2. `docs/technical/INTERACTION_SYSTEM_BASELINE.md`
3. Executar Sprint 05

---

## Relatórios

- Movimento: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- HUD: `docs/testing/HUD_DEBUG_PLAYTEST.md`
- Interação: `docs/testing/INTERACTION_LAB_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
