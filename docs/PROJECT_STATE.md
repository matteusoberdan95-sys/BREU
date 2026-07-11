# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 06 aprovada  
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
| Sprint 06 | **✅ Aprovada** — fine playtest + colisão móveis + depósito |
| Sprint 07 | **Próxima** — Puzzle simples do depósito |

---

## Cena oficial — Pensão térreo

**F6:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

Térreo validado como base estável antes de puzzle, escada ou verticalidade.

**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` (v1.3)

---

## Sprint 06 — resumo (aprovada)

- Rota principal completa; colisão, interações e HUD validados.
- Fine playtest: placa, iluminação, legibilidade corredor, hitboxes.
- Hotfix: colisão móveis grandes; depósito selado visualmente.
- Exterior/lote fechado aceito como placeholder temporário.
- **Não alterado:** PlayerController, PlayerCameraFeel, HUD base, core interação.

**Pendências futuras (não bloqueantes):** barranco, vegetação, arte, teto, escada, 2º andar.

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |

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
