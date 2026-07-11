# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 05 hotfix 2 (visual sealing) aplicado  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02–04 | **✅ Aprovadas** — baselines congeladas |
| Sprint 05 | **🔧 Hotfix 2 sealing** — gameplay OK, fechamento visual pendente playtest |
| Sprint 06 | **Bloqueada** — gate: térreo visualmente fechado |

---

## Notas Sprint 05

- **Gameplay:** térreo navegável, interações OK, colisão contínua mantida (3 lajes inalteradas).
- **Hotfix 1:** z-fighting removido, pisos visuais consolidados, soleiras de porta.
- **Hotfix 2:** shell externo, paredes internas fechadas, piso principal 0,20 m, bermas na trilha.
- **Aprovação Sprint 05:** pendente playtest visual + navegação.

---

## Cena oficial — Pensão térreo

**F6 (playtest pensão):** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

| Área | Status |
|------|--------|
| Trilha + exterior | ✅ Blockout |
| Varanda + entrada | ✅ Aberta |
| Recepção + balcão | ✅ Navegável |
| Corredor 2.4m | ✅ |
| Quarto 102 | ✅ |
| Cozinha | ✅ |
| Depósito trancado | ✅ Porta bloqueia |

**5 interactables:** placa, livro, quarto, cozinha, depósito.

---

## Baselines congeladas

Player · HUD · Interação — ver docs `technical/*_BASELINE.md`

---

## Bugs conhecidos

~~Vãos no chão~~, ~~z-fighting~~ e ~~limbo visível por frestas laterais~~ — hotfixes 1+2 aplicados. Playtest manual para aprovar Sprint 05.

---

## Próxima sprint

**Sprint 06 — Pensão térreo playtest e correção**

Gate: térreo perfeito antes de escada/2º andar.

---

## Cenas de teste

| Cena | Uso |
|------|-----|
| `PensaoTerreoBlockout01.tscn` | Pensão térreo |
| `PlayerMovementLab.tscn` | Regressão movimento |
| `InteractionLab.tscn` | Regressão interação |

---

## Relatórios

- Térreo: `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
