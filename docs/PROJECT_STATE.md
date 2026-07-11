# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 05 hotfix visual aplicado  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02–04 | **✅ Aprovadas** — baselines congeladas |
| Sprint 05 | **🔧 Hotfix visual** — gameplay OK, blockout limpo pendente playtest |

---

## Notas Sprint 05

- **Gameplay:** térreo navegável, interações OK, colisão contínua mantida.
- **Hotfix visual:** z-fighting removido, pisos visuais consolidados, soleiras de porta, paredes embutidas no piso.
- **Aprovação Sprint 05:** pendente playtest visual + navegação.

| Sprint 06 | **Próxima** — Playtest e correção térreo |

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

~~Vãos no chão~~ e ~~z-fighting visual~~ — hotfixes aplicados. Playtest manual para aprovar Sprint 05.

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
