# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 04 aprovada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — player congelado |
| Sprint 03 | **✅ Aprovada** — HUD/debug congelado |
| Sprint 04 | **✅ Aprovada pelo usuário** — interação congelada |
| InteractionLab | **✅ Aprovado** |
| Sprint 05 | **Próxima** — Pensão térreo blockout 01 |

---

## Player — baseline congelada

**Documentação:** `docs/technical/PLAYER_CONTROLLER_BASELINE.md`  
**Teste:** `scenes/test/PlayerMovementLab.tscn`

**Regra:** Não alterar `PlayerController`, `PlayerCameraFeel`, lean, look back, sprint/crouch/stamina ou camera bob sem solicitação explícita.

---

## HUD — baseline congelada

**Documentação:** `docs/technical/HUD_DEBUG_BASELINE.md`

**Regra:** Não alterar HUD base, debug F10/F11 ou layout aprovado sem solicitação explícita. Prompt `[E]` faz parte da extensão aprovada.

---

## Interação — baseline congelada

**Documentação:** `docs/technical/INTERACTION_SYSTEM_BASELINE.md`  
**Teste:** `scenes/test/InteractionLab.tscn`

| Sistema | Status |
|---------|--------|
| Prompt `[E]` ao mirar | ✅ Aprovado |
| E → mensagem 3 s | ✅ Aprovado |
| TestSign / TestBook / TestLockedDoor | ✅ Aprovados |
| Colisões básicas do lab | ✅ Aprovadas |
| HUD + movimentação intactos | ✅ Aprovados |

**Regra:** Não alterar `IInteractable`, `Interactable`, `PlayerInteractionRaycast` ou pipeline de interação sem solicitação explícita ou sprint dedicada.

---

## Cenas de teste

| Cena | Uso |
|------|-----|
| `PlayerMovementLab.tscn` | Regressão movimento + HUD |
| `InteractionLab.tscn` | Regressão interação |
| `BootstrapEmpty.tscn` | F5 bootstrap |

---

## Próxima sprint

**Sprint 05 — Pensão térreo blockout 01**

- Trilha, varanda, recepção, corredor, quarto 102, cozinha, depósito trancado
- Blockout cinza — sem teto, escada ou 2º andar
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
