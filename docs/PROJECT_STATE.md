# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 08 implementada (playtest F6 pendente)  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **✅ Aprovada** — `PLAYER_CONTROLLER_BASELINE.md` |
| Sprint 03 | **✅ Aprovada** — `HUD_DEBUG_BASELINE.md` |
| Sprint 04 | **✅ Aprovada** — `INTERACTION_SYSTEM_BASELINE.md` |
| Sprint 05 | **✅ Aprovada** — térreo blockout jogável |
| Sprint 06 | **✅ Aprovada** — fine playtest térreo |
| Sprint 07 | **✅ Aprovada** — puzzle chave → depósito → fusível |
| Sprint 08 | **🔄 Implementada** — escada isolada (`StairMovementLab`); playtest F6 pendente |

---

## Cena oficial

**F6 (Pensão):** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

**F6 (Escada lab):** `res://scenes/test/StairMovementLab.tscn`

**Baseline escada:** `docs/technical/STAIR_RAMP_BASELINE.md`

**Puzzle aprovado:** chave velha (quarto 102) → depósito destrancado → fusível velho (+ bilhete).

**Baseline puzzle:** `docs/technical/DEPOSIT_PUZZLE_BASELINE.md`

---

## Sprint 07 — resumo (aprovada)

- Fluxo: trilha → pensão → quarto 102 → chave → depósito → fusível.
- Estado local: `HasDepositKey`, `IsDepositUnlocked`, `HasOldFuse`.
- Hotfix parede: `Wall_StairFuture_Blocker` — área futura inacessível.
- **Não alterado:** PlayerController, HUD base, core interação.

**Playtest:** `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |
| 07 Puzzle | `DEPOSIT_PUZZLE_BASELINE.md` |
| 08 Escada (lab) | `STAIR_RAMP_BASELINE.md` |

---

## Relatórios

- Térreo: `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
- Puzzle: `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`
- Escada lab: `docs/testing/STAIR_MOVEMENT_LAB_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`

---

## Próxima sprint recomendada

**Sprint 09 — Segundo andar blockout** (após aprovar playtest F6 da Sprint 08).

Escada **ainda não integrada** na Pensão.
