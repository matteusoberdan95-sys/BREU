# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 10 implementada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02–04 | **✅ Aprovadas** — player, HUD, interação |
| Sprint 05–06 | **✅ Aprovadas** — térreo blockout |
| Sprint 07 | **✅ Aprovada** — puzzle depósito |
| Sprint 08 | **✅ Aprovada** — escada lab |
| Sprint 09A / 09B | **✅ Aprovadas** — escada integrada + playtest |
| Sprint 10 | **🔄 Em validação** — hotfix acesso 2º andar + raycast; playtest F6 pendente |

---

## Cenas

**F6 térreo (baseline preservada):** `PensaoTerreoBlockout01.tscn`

**F6 vertical (térreo + 2º andar):** `PensaoVerticalBlockout01.tscn`

**Lab escada:** `StairMovementLab.tscn`

**Baseline 2º andar:** `docs/technical/PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

---

## Sprint 10 — resumo (em validação)

- Nova cena `PensaoVerticalBlockout01.tscn` — **não altera** `PensaoTerreoBlockout01.tscn`.
- 2º andar: `UpperLanding_Main`, `UpperCorridor_Main`, quartos 201/202, `UpperBlockedDoor`.
- **Hotfix 2026-07-11:** saída da escada liberada; raycast respeita oclusão por parede.
- Segundo andar **só será aprovado** se saída da escada estiver livre e corredor superior navegável.
- Escada conectada @ y = 2,8 m; puzzle térreo preservado.
- Sem teto, sem inimigo, sem arte final.

**Playtest:** `docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md`

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |
| 07 Puzzle | `DEPOSIT_PUZZLE_BASELINE.md` |
| 08–09A Escada | `STAIR_RAMP_BASELINE.md` |

---

## Próxima sprint recomendada

**Sprint 11 — Teto e câmera FPS** (após aprovar playtest F6 da Sprint 10).

Sem inimigo, sem arte final, sem GLB.
