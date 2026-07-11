# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 05 aprovada  
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
| Sprint 06 | **Próxima** — Playtest e correção fina do térreo |

---

## Cena oficial — Pensão térreo

**F6 (playtest pensão):** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

| Área | Status |
|------|--------|
| Trilha + exterior | ✅ Blockout (lote simples aceito) |
| Varanda + entrada | ✅ Navegável |
| Recepção + balcão | ✅ Navegável |
| Corredor 2.4m | ✅ |
| Quarto 102 | ✅ |
| Cozinha | ✅ |
| Depósito trancado | ✅ Porta bloqueia + interação |

**5 interactables:** placa, livro, quarto, cozinha, depósito.

**Baseline congelada:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md`

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |

---

## Notas Sprint 05 (aprovada)

- Caixa cinza jogável: trilha → depósito sem queda em navegação normal.
- Hotfixes 1+2: colisão contínua, z-fighting removido, shell visual fechado.
- Exterior permanece blockout temporário (sem barranco/vegetação/arte).
- **Não alterar** térreo sem Sprint 06 ou pedido explícito.

---

## Pendências reais (Sprint 06+)

- Correção fina de playtest (escala, cantos, exterior).
- Barrancos, terreno externo, atmosfera, arte, teto, escada, 2º andar — sprints futuras.

---

## Cenas de teste

| Cena | Uso |
|------|-----|
| `PensaoTerreoBlockout01.tscn` | Pensão térreo (oficial) |
| `PlayerMovementLab.tscn` | Regressão movimento |
| `InteractionLab.tscn` | Regressão interação |

---

## Relatórios

- Térreo: `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`
- Histórico: `docs/SPRINT_HISTORY.md`
