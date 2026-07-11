# BREU — Roadmap de Sprints (Reboot)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Regra:** uma sprint = um objetivo claro + DoD + checkpoint Git

---

## Visão rápida

```
S00 Auditoria → S01 Fundação → S02 Player → S03 HUD → S04 Interação
    → S05 Pensão térreo → S06 Playtest térreo → S07 Puzzle depósito
    → S08 Escada isolada → S09 2º andar → S10 Teto
    → S11 Atmosfera → S12 Vertical slice → S13 Inimigo → S14 Combate → S15 Arte modular
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

**Objetivo:** Aprovar térreo antes de qualquer verticalidade.

**Entregas:**
- Correções de escala, portas, colisões.
- Vídeo curto de validação (30–90 s).
- Relatório QA.

**Gate:** **Não avançar** se térreo não estiver perfeito.

---

## Sprint 07 — Puzzle simples do depósito

**Objetivo:** Estado mínimo narrativo/gameplay.

**Entregas:**
- Bilhete → fusível/chave → depósito trancado.
- `HasFuse` / feedback HUD.
- Porta depósito com colisão ligada/desligada.

**DoD:** Puzzle completável em playtest; sem regressão de navegação.

---

## Sprint 08 — Escada isolada de teste

**Objetivo:** Subida confiável isolada do resto.

**Entregas:**
- Cena ou zona isolada `scenes/test/StairSandbox.tscn`.
- Rampa **somente colisão invisível**.
- Sem teto; sem 2º andar complexo.

**DoD:** Subir/desce 10x sem clipping de câmera.

---

## Sprint 09 — Segundo andar blockout

**Objetivo:** Layout superior navegável.

**Entregas:**
- Corredor superior, gerente, banheiro, quarto trancado.
- Colisão simples (`Floor02_MainWalkableCollision`).
- Guarda-corpo no vão da escada.
- **Sem teto.**

**DoD:** Térreo + escada + 2º andar ida e volta.

---

## Sprint 10 — Teto e câmera FPS

**Objetivo:** Teto modular sem clipping.

**Entregas:**
- Peças de teto encaixadas; altura validada (≥ 2,4 m acima da cabeça).
- Teste escada + corredor superior.

**DoD:** Zero frames com câmera dentro de geometria de teto.

---

## Sprint 11 — Atmosfera inicial

**Objetivo:** Noite legível + modo debug visual.

**Entregas:**
- Luzes omni/directional mínimas.
- Fog leve + override playtest.
- Contraste suficiente para QA ver layout.

**DoD:** Navegação legível com e sem fog debug.

---

## Sprint 12 — Vertical slice da Pensão

**Objetivo:** Experiência contínua sem inimigo.

**Fluxo:** trilha → pensão → térreo → puzzle → escada → 2º andar → pista principal.

**DoD:** Playtest completo 15–20 min sem soft-lock.

---

## Sprint 13 — Primeiro inimigo / encontro controlado

**Objetivo:** Tensão + fuga ou confronto simples.

**Entregas:**
- Inimigo placeholder.
- Trigger de encontro.
- Sem combate complexo ainda (fuga OK).

---

## Sprint 14 — Combate base

**Objetivo:** Loop ataque → dano → feedback.

**Entregas:**
- Hitbox, dano player/inimigo, stun simples.
- HUD de vida reativo.

---

## Sprint 15 — Arte modular inicial

**Objetivo:** Substituir blocos por módulos.

**Gate:** Gameplay da Pensão **aprovado** antes de Blender.

**Entregas:**
- Primeiros módulos de parede/piso/porta.
- Pipeline documentado; GLB só visual, colisão manual.

---

## Sprints futuras (backlog)

- Trilha completa com ônibus/lotação (cutscene/gameplay).
- Audio diegético (rádio, passos, porta).
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
