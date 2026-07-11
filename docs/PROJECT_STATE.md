# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 09A implementada  
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
| Sprint 08 | **✅ Aprovada** — escada isolada (`StairMovementLab`) |
| Sprint 09A | **🔄 Implementada** — escada integrada no térreo; playtest F6 pendente |

---

## Cena oficial

**F6 (Pensão):** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

**F6 (Escada lab):** `res://scenes/test/StairMovementLab.tscn`

**Escada:** integrada no álcove oeste — rampa invisível + patamar superior temporário. **Segundo andar completo ainda não criado.**

**Baseline escada:** `docs/technical/STAIR_RAMP_BASELINE.md` v1.1

**Baseline térreo:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` v1.4

---

## Sprint 09A — resumo (implementada)

- Escada no corredor oeste (entrada z ≈ -25,5) usando `StairRampAssembly`.
- Métricas idênticas ao lab aprovado (2,8 m × 5,8 m × 2,2 m, 14 degraus).
- `UpperLanding_Temporary` com barreiras — sem gameplay de 2º andar.
- Player / HUD / interação / puzzle **não alterados** nos scripts aprovados.
- Hotfix: `depositDepth` corrigido (valor positivo).

**Playtest:** `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md`

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |
| 07 Puzzle | `DEPOSIT_PUZZLE_BASELINE.md` |
| 08 Escada | `STAIR_RAMP_BASELINE.md` |

---

## Relatórios

- Térreo: `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
- Puzzle: `docs/testing/PENSAO_DEPOSIT_PUZZLE_PLAYTEST.md`
- Escada lab: `docs/testing/STAIR_MOVEMENT_LAB_PLAYTEST.md`
- Escada Pensão: `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`

---

## Próxima sprint recomendada

**Sprint 09 — Segundo andar blockout** (após aprovar playtest F6 da Sprint 09A).

Sem teto, sem inimigo, sem arte final.
