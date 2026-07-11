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
| Sprint 06 | **✅ Aprovada** — fine playtest + correções mínimas |
| Sprint 07 | **Próxima** — Puzzle simples do depósito |

---

## Cena oficial — Pensão térreo

**F6:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

Térreo estável como base antes de puzzle, escada ou verticalidade.

**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` (v1.1)

---

## Sprint 06 — resumo

**Bugs corrigidos:**
- Placa exterior reposicionada para alcance confortável da trilha
- Balcões recepção/cozinha sem colisão (evita prender em canto)
- Áreas de interação ampliadas (livro, quarto, cozinha)
- Iluminação playtest: depósito, quarto 102, cozinha; ambiente levemente mais claro
- Piso corredor levemente mais escuro para legibilidade

**Bugs restantes (não bloqueantes):**
- Exterior lote simples (barranco/terreno artístico — sprint futura)
- Horizonte/céu aberto (sem teto — sprint 10)
- Arte blockout cinza (sprint 15)

**Não alterado:** PlayerController, PlayerCameraFeel, HUD base, core de interação.

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
