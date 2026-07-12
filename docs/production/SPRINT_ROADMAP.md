# BREU — Roadmap de Sprints (Reboot)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Regra:** uma sprint = um objetivo claro + DoD + checkpoint Git

---

## Visão rápida

```
S00 Auditoria → S01 Fundação → S02 Player → S03 HUD → S04 Interação
    → S05 Pensão térreo → S06 Playtest térreo → S07 Puzzle depósito
    → S08 Escada isolada → S09A Escada na Pensão → S10 2º andar → S11 Playtest 2º andar
    → S12 Teto blockout → S13 Atmosfera → S14 Portas/narrativa → S15 Vertical slice → S16 Inimigo → S17 Combate → S18 Arte modular
```

---

## Sprint 00 — Auditoria e limpeza do projeto

**Objetivo:** Garantir base honesta antes de escrever código.

**Entregas:**
- Relatório do estado real do repositório (disco vs Git).
- Branch `reboot/breu-clean-start`.
- `project.godot` apontando para cena que existe.
- Zero referência a cena deletada na main scene.
- `docs/PROJECT_STATE.md` atualizado.

**Não fazer:** gameplay, pensão, player (exceto stub mínimo se projeto não abrir).

**DoD:** Ver `docs/production/SPRINT_00_TASKS.md`.

---

## Sprint 01 — Fundação mínima Godot

**Status:** ✅ Concluída (2026-07-11)

**Objetivo:** Projeto Godot 4.7 + C# abre e roda sem erro.

**Entregas:**
- `BREU.csproj`, `project.godot`, `GlobalUsings.cs`.
- Estrutura de pastas conforme `GODOT_PROJECT_STRUCTURE.md`.
- Cena oficial bootstrap: `scenes/levels/BootstrapEmpty.tscn` (Node3D + WorldEnvironment mínimo).
- Input map básico (placeholder).

**DoD:** F5/F6 abre cena bootstrap; `dotnet build` OK.

---

## Sprint 02 — Player Controller limpo

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Movimentação FPS confiável.

**Entregas:**
- `scenes/player/Player.tscn` + scripts modulares
- Sprint + stamina, crouch, lean, look back, camera feel
- `scenes/test/PlayerMovementLab.tscn`
- Baseline: `docs/technical/PLAYER_CONTROLLER_BASELINE.md`

**DoD:** Aprovado pelo usuário — baseline congelada.

---

## Sprint 03 — HUD e Debug

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Feedback mínimo + ferramentas de playtest.

**Entregas:**
- HUD: vida, stamina, lanterna (display).
- `PlaytestDebugSettings`: lanterna infinita, fog reduzida (flags).
- Mensagens temporárias no HUD.

**DoD:** Lanterna não zera em 10 min com debug ativo.

---

## Sprint 04 — Sistema de interação mínimo

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Loop investigativo básico — E + prompt + mensagem.

**Entregas:**
- `IInteractable`, `Interactable`, `PlayerInteractionRaycast`
- Prompt no HUD aprovado
- `InteractionLab.tscn` (3 objetos teste)

**DoD:** E mostra texto; HUD exibe prompt; player intacto.

**Não fazer:** inventário complexo.

---

## Sprint 05 — Pensão térreo blockout 01

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Térreo 100% navegável — caixa cinza.

**Entregas:**
- Trilha, varanda, recepção, corredor 2,4 m, quarto 102, cozinha, depósito trancado.
- Colisões manuais; blockout visual fechado (hotfixes 1+2).
- **Sem teto, sem escada, sem 2º andar, sem GLB/Blender**.
- Interações placeholder (5 pontos).
- Baseline: `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md`
- Cena oficial: `PensaoTerreoBlockout01.tscn`

**DoD:** Fluxo trilha → depósito aprovado; HUD e interação intactos; exterior simples aceito.

---

## Sprint 06 — Pensão térreo playtest e correção

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Validar térreo como base estável antes de puzzle/verticalidade.

**Entregas:**
- Fine playtest rota completa documentado e aprovado
- Correções: interação, iluminação, legibilidade, colisão móveis, depósito selado
- Baseline térreo v1.3 congelada
- Player/HUD/interação intactos

**DoD:** Rota principal, colisão, interações, HUD e movimento validados; exterior placeholder aceito.

**Gate atingido:** térreo estável para Sprint 07 (puzzle depósito).

---

## Sprint 07 — Puzzle simples do depósito

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Primeiro loop de gameplay — chave → depósito → fusível.

**Entregas:**
- `PensaoPuzzleState` (HasDepositKey, IsDepositUnlocked, HasOldFuse)
- Porta depósito stateful; chave quarto 102; fusível + bilhete
- Hotfix `Wall_StairFuture_Blocker`
- Baseline: `docs/technical/DEPOSIT_PUZZLE_BASELINE.md`

**DoD:** Fluxo quarto → chave → depósito → fusível aprovado; sem regressão.

**Gate atingido:** puzzle funcional; térreo + interação intactos.

---

## Sprint 08 — Escada isolada de teste

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Subida confiável isolada do resto.

**Entregas:**
- `scenes/test/StairMovementLab.tscn`
- Rampa **somente colisão invisível** (`Stair_InvisibleRamp_Collision`)
- 14 degraus visuais sem colisão
- Baseline: `docs/technical/STAIR_RAMP_BASELINE.md`

**DoD:** Subir/desce suaves aprovados pelo usuário; player/HUD intactos.

**Gate atingido:** modelo técnico de escada validado; pronto para integração na Pensão.

**Não feito (proposital):** integração na Pensão; teto; 2º andar real.

---

## Sprint 09A — Integrar escada no térreo da Pensão

**Status:** ✅ Aprovada (2026-07-11)

**Objetivo:** Colocar escada aprovada no layout do térreo, mantendo padrão rampa invisível.

**Entregas:**
- `StairWell` + `StairRampAssembly` em `PensaoTerreoBlockout01Builder`
- Entrada corredor oeste z ≈ -25,5; patamar `UpperLanding_Temporary`
- Luz `StairWellLight`; baseline v1.5 térreo + v1.2 escada

**DoD:** Subir/desce na Pensão + regressão puzzle/HUD/movimento — **aprovado pelo usuário**.

**Gate atingido:** escada integrada; patamar placeholder pronto para Sprint 09 (2º andar).

**Não feito (proposital):** segundo andar completo; teto; arte final.

---

## Sprint 10 — Segundo andar blockout 01

**Status:** ✅ Concluída — aprovada 2026-07-11

**Objetivo:** Layout superior navegável conectado à escada aprovada.

**Entregas:**
- `PensaoVerticalBlockout01.tscn` (cópia vertical — térreo baseline preservado)
- Corredor superior, quartos 201/202, porta bloqueada 203
- `Floor_Second_Main`, caixa de escada, guarda-corpos, 3 interações
- Baseline: `PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

**DoD:** ✅ Térreo + escada + 2º andar ida e volta — validado F6.

**Não feito (proposital):** teto; telhado; inimigo; arte final.

---

## Sprint 11 — Playtest fino 2º andar

**Status:** ✅ Concluída — aprovada 2026-07-11

**Objetivo:** Validar rota completa térreo ↔ 2º andar após blockout Sprint 10.

**Entregas:**
- Playtest fino documentado e aprovado
- Layout 2º andar congelado
- `docs/testing/PENSAO_SECOND_FLOOR_FINE_PLAYTEST.md`

**DoD:** ✅ Escada + corredor + quartos + regressão térreo/puzzle/HUD/movimento.

**Não feito:** refatoração layout; teto; arte final.

---

## Sprint 12 — Teto e fechamento superior

**Status:** ✅ Concluída — aprovada via hotfix 12A (2026-07-11)

**Objetivo:** Fechar visualmente a Pensão por cima sem arte final.

**Entregas:**
- Teto/forro blockout térreo + 2º andar + poço escada
- Cobertura externa em dois níveis (`Roof_Blockout_Main` + `Roof_Blockout_LowerFront`)
- Casca externa `Shell_FacadeUpper_*`
- Luzes mínimas para playtest com teto fechado
- Baseline + playtest docs

**DoD:** ✅ Interior fechado por cima; escada/navegação/puzzle intactos; sem limbo crítico.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 12A — Hotfix fechamento teto/casca/frestas

**Status:** ✅ Concluída — aprovada 2026-07-11

**Objetivo:** Corrigir fachada flutuante, frestas da escada e área aberta da porta verde.

**Entregas:**
- Telhado em dois níveis + massa superior de fachada
- `Ceiling_StairBox_Main` + selagens laterais
- Cômodo placeholder sul (`Wall_UpperSouthRoom_*`)
- `docs/testing/PENSION_CEILING_HOTFIX_12A.md`

**DoD:** ✅ Fachada fechada; escada navegável; regressão OK.

---

## Sprint 13 — Atmosfera base da Pensão

**Status:** ✅ Concluída — aprovada 2026-07-11

**Objetivo:** Atmosfera inicial de terror sem alterar estrutura aprovada.

**Entregas:**
- `Pension_WorldEnvironment` — fog suave via Environment (anti-quadrados)
- Iluminação exterior/interior/2º andar graduada
- F11: ciclo fog normal / off / debug
- Baseline + playtest docs

**DoD:** ✅ Mais atmosférica; neblina sem artefatos; navegação/puzzle/HUD intactos.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 14 — Portas, quartos e leitura narrativa

**Status:** ✅ Concluída / aprovada (2026-07-11) — blockout narrativo limpo

**Objetivo:** Blockout narrativo — leitura de cômodos, portas, pistas.

**Entregas:**
- Leitura de cômodos por vãos limpos + headers (portas decorativas removidas por instabilidade)
- Interações narrativas (recepção, quartos, cozinha, depósito, escada)
- Puzzle depósito preservado
- `docs/technical/PENSION_ROOM_READABILITY_BASELINE.md`
- `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v2.1
- `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

**DoD:** ✅ Cômodos identificáveis; interações OK; zero regressão; estabilidade sem z-fighting.

**Nota:** Portas bonitas e placas finais ficam para sprint futura de arte. Não reabrir portas na Sprint 15.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 15 — Evento narrativo simples (sem inimigo)

**Status:** ✅ Concluída / aprovada (2026-07-11)

**Objetivo:** Tensão narrativa one-shot sem inimigo, combate ou chase.

**Entregas:**
- `PensionNarrativeEvents` + `NarrativeTrigger3D` + `LightFlickerOneShot`
- 6 eventos one-shot (entrada, chave, fusível, escada, corredor, porta)
- Mensagens HUD + flicker; sem áudio runtime
- `docs/technical/PENSION_NARRATIVE_EVENTS_BASELINE.md`
- `docs/testing/PENSION_SIMPLE_NARRATIVE_EVENTS_PLAYTEST.md`

**DoD:** ✅ Eventos disparam 1×; zero regressão; sem inimigo.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 16 — Áudio funcional base da Pensão

**Status:** ✅ Concluída / aprovada (2026-07-12)

**Objetivo:** Camada de áudio funcional (ambience, eventos, gotas, passos, respiração), sem inimigo.

**Entregas:**
- Pack v2 `assets/audio/pensao/`
- `PensionAudioManager` + zonas + crossfade + F7 debug
- One-shots Sprint 15 + flashlight click
- `PlayerFootstepAudio` + superfícies Wood / DirtGravel
- `PlayerBreathingAudio` (normal + panting)
- Docs: baseline, asset list, playtest funcional

**DoD:** ✅ Áudio estável sem regressão; sem inimigo/combat.  
**Ressalva:** terra/cascalho em backlog de refinamento.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 17 — Puzzle da varanda e ala superior trancada

**Status:** 🔄 Implementada — playtest F6 pendente (2026-07-12)

**Objetivo:** Porta verde jogável + pequena ala superior; preparar tensão sem inimigo.

**Entregas:**
- `BlockoutBalconyDoor` + flags `HasReadBalconyNote` / `HasBalconyKey` / `IsBalconyUnlocked`
- Nota 201 + chave recepção + `PensaoBalconyPuzzleSetup`
- `UpperBalconyWing` blockout (varanda, 203, office)
- Eventos narrativos sutis + zonas de áudio
- Docs design / playtest / interaction baseline

**DoD:** Puzzle completo; varanda/ala navegáveis; zero regressão 02–16; sem inimigo.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

### Sprint 17C — Ala da varanda + puzzle macabro da proprietária

**Status:** 🔄 Implementada — playtest F6 pendente (2026-07-12)

**Objetivo:** Banheiro + quarto da proprietária jogáveis; puzzle arame→ralo→chave; caderno dispara tensão; Quarto 203 como gancho.

**Entregas:**
- Geometria `Room_UpperBathroom` / `Room_OwnerBedroom` + guarda-corpo da varanda
- Flags 17C + `PensaoBalconyWingPuzzleSetup` + `BlockoutOwnerBedroomDoor`
- `EventOwnerLedgerReveal` + `Door_Room203_Blocked`
- Docs design / playtest / PROJECT_STATE

**DoD:** Dois cômodos acessíveis; puzzle completo; evento do caderno; 203 bloqueado; zero regressão; sem inimigo.

**Cena alvo:** `PensaoVerticalBlockout01.tscn`

---

## Sprint 18 — Primeiro susto controlado sem inimigo físico

**Status:** ⏳ Após Sprint 17C aprovada

**Objetivo:** Susto / tensão controlada **sem** inimigo físico, combate ou chase.

---

## Sprint 19 — Primeiro inimigo / encontro controlado

**Status:** ⏳ Após Sprint 18

**Objetivo:** Tensão + fuga ou confronto simples.

**Entregas:**
- Inimigo placeholder.
- Trigger de encontro.
- Sem combate complexo ainda (fuga OK).

---

## Sprint 19 — Combate base

**Objetivo:** Loop ataque → dano → feedback.

**Entregas:**
- Hitbox, dano player/inimigo, stun simples.
- HUD de vida reativo.

---

## Sprint 20 — Arte modular inicial

**Objetivo:** Substituir blocos por módulos.

**Gate:** Gameplay da Pensão **aprovado** antes de Blender.

**Entregas:**
- Primeiros módulos de parede/piso/porta.
- Pipeline documentado; GLB só visual, colisão manual.

---

## Sprints futuras (backlog)

### Backlog técnico/artístico (áudio — pós Sprint 16)

- Refinar passos na terra/cascalho andando.
- Refinar passos na terra/cascalho correndo.
- Avaliar novos samples de terra mais naturais.
- `player_run_step_*` reservado para chase/pânico futuro.

### Outros

- Trilha completa com ônibus/lotação (cutscene/gameplay).
- Checkpoint / morte / retry.
- Streaming entre exterior e interior.
- Inimigo “Hóspede Seco” definitivo.
- Prefeito / polícia / narrativa expandida.

---

## Controle de versão por sprint

| Sprint | Branch sugerida | Tag opcional |
|--------|-----------------|--------------|
| 00 | `reboot/breu-clean-start` | `reboot-s00` |
| 01 | `reboot/s01-foundation` | `reboot-s01` |
| 05 | `reboot/s05-pensao-terreo` | `reboot-s05` |
| 12 | `reboot/s12-vertical-slice` | `reboot-s12` |

Cada sprint termina com **commit + atualização de `PROJECT_STATE.md`**.
