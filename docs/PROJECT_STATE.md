# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 12A aprovada  
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
| Sprint 11 | **✅ Aprovada** — playtest fino 2º andar |
| Sprint 12 | **✅ Aprovada** — teto/cobertura blockout |
| Sprint 12A | **✅ Aprovada** — hotfix fechamento fachada/escada/porta verde |

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

**Playtest blockout:** `docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md`

---

## Sprint 11 — resumo (aprovada)

- Playtest fino do segundo andar — rota completa ida/volta validada.
- Escada, landing, corredor, quartos 201/202, porta bloqueada — OK.
- Regressão térreo, puzzle depósito, HUD, movimento — OK.
- Layout do 2º andar **congelado**; sem refatoração nesta fase.

**Playtest:** `docs/testing/PENSAO_SECOND_FLOOR_FINE_PLAYTEST.md`

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
| 11 Playtest 2º andar | `PENSAO_SECOND_FLOOR_FINE_PLAYTEST.md` |
| 12 Teto blockout | `PENSION_CEILING_BLOCKOUT_BASELINE.md` |
| 12A Hotfix fechamento | `PENSION_CEILING_HOTFIX_12A.md` |

---

## Sprint 12 / 12A — resumo (aprovadas)

Fechamento superior blockout da Pensão Santa Luzia — **aprovado como placeholder funcional**.

- **Teto/cobertura:** térreo, 2º andar, poço da escada e cobertura externa em dois níveis.
- **Casca externa:** `Shell_FacadeUpper_*`, `Roof_Blockout_Main`, `Roof_Blockout_LowerFront` — fachada lê bloco fechado.
- **Escada:** caixa melhor fechada (`Ceiling_StairBox_Main` + selagens); navegação preservada.
- **Porta verde:** cômodo placeholder sul fechado (`Wall_UpperSouthRoom_*`).
- **Regressão:** térreo, puzzle depósito, HUD, lanterna, interação e movimentação — OK.
- **Não é arte final** — telhado/texturas finais ficam para sprint futura.

**Baseline:** `docs/technical/PENSION_CEILING_BLOCKOUT_BASELINE.md`  
**Playtest hotfix:** `docs/testing/PENSION_CEILING_HOTFIX_12A.md`

---

## Próxima sprint — Sprint 13 (Atmosfera base da Pensão)

**Objetivo:** Noite legível + modo debug visual — sem alterar layout aprovado.

**Entregas planejadas:**
- Fog leve + override playtest (`PlaytestDebugSettings`).
- Contraste e leitura noturna refinados.
- Validação visual com teto já fechado.

**DoD:** Navegação legível com e sem fog debug; zero regressão puzzle/HUD/movimento/teto.

**Não fazer:** arte final; GLB; inimigo; refazer 2º andar ou teto.

**Roadmap:** `docs/production/SPRINT_ROADMAP.md` — Sprint 13

**Cena alvo:** `PensaoVerticalBlockout01.tscn`
