# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 14 em validação; Sprint 14B executada (playtest F6 pendente)  
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
| Sprint 13 | **✅ Aprovada** — atmosfera base |
| Sprint 14 | **🔄 Em validação** — portas/quartos/leitura narrativa (playtest F6 pendente) |
| Sprint 14A | **⏸️ Substituída** — hotfix parcial (playtest falhou) |
| Sprint 14B | **🔄 Executada** — sistema de portas blockout estável (playtest F6 pendente) |

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
| 13 Atmosfera base | `PENSION_ATMOSPHERE_BASELINE.md` |
| 14 Leitura narrativa | `PENSION_ROOM_READABILITY_BASELINE.md` |
| 14A Portas blockout | `PENSION_DOOR_BLOCKOUT_BASELINE.md` |

---

## Sprint 14 — resumo (em validação)

Blockout narrativo — portas/molduras, props simples e interações de texto.

- **Portas/molduras** em entrada, recepção, quartos, cozinha, depósito, escada, 2º andar.
- **Props:** balcão, chaves, cama, mala, fogão, prateleiras, caixas, anotação, marcas de arrasto.
- **Interações** narrativas em recepção, quartos, cozinha, depósito, escada — puzzle **preservado**.
- **Atmosfera S13, geometria S05–12A, player/HUD/interação** — não alterados.

**Baseline:** `docs/technical/PENSION_ROOM_READABILITY_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

---

## Sprint 14A — resumo (executada)

Hotfix de leitura de portas sem alterar atmosfera, movimento ou layout principal.

- **Depósito:** porta reestruturada; destravar oculta painel e desativa colisão (sem animação/scale).
- **Molduras** claras em entrada, 102, cozinha, 201, 202.
- **Porta verde** renomeada/redefinida como `Door_UpperBalcony_Locked` (varanda bloqueada).
- **Varanda placeholder** frontal com piso e guarda-corpos.

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14A

---

## Próxima sprint — Sprint 15 (Vertical slice da Pensão)

**Objetivo:** Experiência contínua trilha → pensão → puzzle → escada → 2º andar — sem inimigo.

**Entregas planejadas:**
- Playtest rota completa 15–20 min
- Correções de soft-lock se encontrados

**DoD:** Fluxo completo jogável; zero regressão dos sistemas aprovados.

**Não fazer:** inimigo; combate; arte final.

**Roadmap:** `docs/production/SPRINT_ROADMAP.md` — Sprint 15

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

**Cena alvo:** `PensaoVerticalBlockout01.tscn`
