# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 10 aprovada  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — player controller |
| Sprint 03 | **✅ Aprovada** — HUD |
| Sprint 04 | **✅ Aprovada** — interação |
| Sprint 05 | **✅ Aprovada** — térreo blockout |
| Sprint 06 | **✅ Aprovada** — térreo tuning |
| Sprint 07 | **✅ Aprovada** — puzzle depósito |
| Sprint 08 | **✅ Aprovada** — escada lab |
| Sprint 09A | **✅ Aprovada** — escada integrada |
| Sprint 09B | **✅ Aprovada** — playtest escada |
| Sprint 10 | **✅ Aprovada** — segundo andar blockout navegável |

---

## Cena atual (F6)

**Vertical (térreo + 2º andar):** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

**Térreo baseline preservada:** `PensaoTerreoBlockout01.tscn`

**Lab escada:** `StairMovementLab.tscn`

**Baseline 2º andar:** `docs/technical/PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

---

## Sprint 10 — resumo (aprovada)

- Segundo andar aprovado como **blockout cinza navegável** — visualmente cru, funcionalmente validado.
- Player sobe escada, acessa piso superior, navega corredor + quartos 201/202 + porta bloqueada.
- Vão da escada com caixa (`StairBox_Wall_*`) + guarda-corpos (`Stairwell_Rail_*`).
- Térreo, puzzle depósito, HUD, PlayerController e camera feel **preservados**.
- Sem teto, sem telhado, sem arte final, sem inimigo.

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
| 10 Segundo andar | `PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md` |

---

## Próxima sprint recomendada

**Sprint 11 — Playtest fino do segundo andar**

Sem refazer layout do 2º andar. Sem arte final, GLB, inimigo ou combate nesta fase.
