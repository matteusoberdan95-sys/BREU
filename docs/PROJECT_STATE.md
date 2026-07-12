# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 16 implementada com pack áudio v2 (F6 pendente)  
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
| Sprint 14 | **✅ Aprovada** — blockout narrativo limpo (14Z) |
| Sprint 14A–14F | **⏸️ Substituídas** — iterações de portas/placas absorvidas pela 14Z |
| Sprint 15 | **✅ Aprovada** — eventos narrativos one-shot sem inimigo |
| Sprint 16 | **🔄 Implementada** — áudio ambiente pack v2 (F6 pendente) |

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

## Sprint 14B — resumo (executada)

Substituição do sistema de portas quebrado (14/14A) por padrão blockout estável.

- **Removidas** folhas `Door_*_Leaf`, portas interactable verdes, duplicatas de moldura.
- **Porta aberta** = somente moldura (entrada, 102, cozinha, 201, 202).
- **Porta trancada** = painel opaco + colisão WorldLayer + Area3D local.
- **Depósito** = painel some + colisão desativa (sem animação); interação em `Door_Deposit_InteractArea`.
- **Varanda** = `Door_UpperBalcony_Locked` + placeholder interior + leitura na trilha.

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.1  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14B

---

## Sprint 14C — sistema de portas reconstruído

Estado técnico: implementado e compilado sem erros. Há três prefabs estáveis; as passagens abertas usam somente moldura; o depósito preserva chave e mensagens; e a porta verde possui um único painel opaco. Player, HUD, atmosfera, fog, fachada e escada não foram alterados.

Pendente: executar F6 e aprovar visualmente a rota e as interações.

Revisão visual posterior: corrigidos sobreposição da travessa do depósito, placa com texto fora da madeira, vão lateral da caixa da escada e prompts de porta disparados por paredes/piso distantes.

Sprint 14D: auditoria formal criada em `docs/testing/PENSION_DOOR_AUDIT_14D.md`; entrada principal sem folha ou bloqueio; placa deslocada para fora do eixo da trilha; painéis fechados afastados do plano da parede; porta verde renomeada para o nó oficial.

Hotfix 14D: `UpperBalcony_BackWall` estava usando altura local de primeiro andar e aparecia como um bloco diante da passagem do térreo. Reposicionado para `Y = 4,25`, no segundo andar.

## Sprint 14F — resumo (executada)

Limpeza definitiva — remover duplicatas em vez de empilhar correções.

- **Placa única** `Sign_Pensao_Main_Exterior` fora da fachada; removida placa interna que clipava a entrada.
- **Portas abertas** = moldura mínima inline (3 peças); sem infill, folhas ou painéis.
- **Painéis permitidos** apenas: `Door_Deposit_Panel`, `Door_UpperBalcony_Panel`.
- **JobOfferSign** na trilha = interação sem mesh (sem placa 3D duplicada).
- Corredor inútil do 2º andar **permanece fechado** (14E).

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.3  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14F

## Sprint 14Z — reset final (executada)

Limpeza destrutiva — remover meshes instáveis em vez de ajustar.

- **Placas removidas** temporariamente (entrada limpa).
- **Portas abertas** = vão limpo + `Header_*` (sem moldura).
- **Blockers inline** apenas: `Door_Deposit_Blocker`, `Door_UpperBalcony_Blocker`, `Door_UpperBlocked_Blocker`.
- Prefabs/molduras de porta **não** instanciados em runtime.
- Corredor inútil do 2º andar **permanece fechado** (14E).

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v2.0  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14Z

## Sprint 14 — APROVADA (blockout narrativo limpo)

**Data de aprovação:** 2026-07-11  
**Cena oficial:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

**Aprovado:**
- Pensão navegável (térreo + 2º andar + escada)
- Atmosfera preservada
- Puzzle chave → depósito → fusível preservado
- Portas decorativas bugadas removidas/simplificadas (vãos limpos + headers)
- Placas problemáticas removidas (arte de placa fica para sprint futura)
- Cômodos com leitura básica por vãos/headers

**Não avançar nesta sprint:** portas bonitas, molduras decorativas, placas finais, layout, atmosfera, player, HUD, puzzle.

**Baseline de portas:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v2.0  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

## Sprint 15 — APROVADA (eventos narrativos simples)

**Data de aprovação:** 2026-07-11  
**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

- Eventos one-shot funcionando (entrada, pós-chave, pós-fusível, escada, corredor, porta)
- Mensagens narrativas + flicker de luz; sem loop
- Puzzle, atmosfera, HUD, player, escada e 2º andar preservados
- **Sem inimigo / combate / chase**

**Baseline:** `docs/technical/PENSION_NARRATIVE_EVENTS_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_SIMPLE_NARRATIVE_EVENTS_PLAYTEST.md`

## Sprint 16 — Áudio ambiente base (implementada / pack v2)

**Status:** 🔄 Implementada — playtest F6 pendente  
**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

- Pack v2 em `assets/audio/pensao/` (loops, one-shots, flashlight, passos/respiração)
- `PensionAudioManager` + zonas + crossfade
- One-shots nos eventos Sprint 15
- Click da lanterna conectado (sem mudar lógica/bateria)
- Passos/respiração **cadastrados para Sprint 17** (não wired)
- Sem inimigo / combate / chase

**Baseline:** `docs/technical/PENSION_AUDIO_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md`  
**Assets:** `docs/audio/PENSION_AUDIO_ASSET_LIST.md`

## Próxima sprint recomendada — Sprint 17 (Footsteps e respiração do player)

Após F6 da Sprint 16 aprovada.

**Não avançar automaticamente** para inimigo/chase.
