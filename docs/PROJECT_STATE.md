# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 06 hotfix (colisão/depósito)  
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
| Sprint 06 | **🔧 Hotfix** — colisão móveis + depósito selado; validação F6 pendente |
| Sprint 07 | **Próxima** — Puzzle simples do depósito |

---

## Cena oficial — Pensão térreo

**F6:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

Térreo estável como base antes de puzzle, escada ou verticalidade.

**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` (v1.2)

---

## Sprint 06 — hotfix colisão/depósito

**Correções:**
- Colisão em cama, balcão recepção, bancada cozinha (`FurnitureCollisions`)
- Depósito: paredes alcove + moldura porta + fundo 14 m; `Door_Deposit_Blocked`
- Props pequenos (livro) permanecem Area3D sem colisão física

**Sprint 06 anterior (fine playtest):**
- Placa reposicionada; áreas interactable ampliadas
- Iluminação depósito/quarto/cozinha; piso corredor legível

**Não alterado:** PlayerController, HUD, core interação.

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
