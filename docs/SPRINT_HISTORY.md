# BREU - Historico de sprints

Registro cronologico das sprints e alteracoes relevantes do projeto.  
**Atualize este arquivo antes de todo commit/push** (ver `.cursor/rules/pre-commit-docs.mdc`).

Ultima atualizacao: 2026-07-09

---

## Sprint 0 — Fundacao da vertical slice

**Commit:** `db3907a` — Initial BREU vertical slice foundation

### Entregas

- Estrutura base Godot 4.7 Mono/C#.
- Player FPS (`PlayerController`, `PlayerLook`, `FlashlightController`, `PlayerInteractor`).
- Sistemas iniciais de inventario, armas, inimigos (stubs) e HUD minimo.
- Cena `DemoRoom.tscn` inicial e documentacao base (`docs/`, `AGENTS.md`).

---

## Sprint 1 — Integracao Blender Quarto 07

**Commit:** `d2bc0ce` — Integrar blockout Blender do Quarto 07 em DemoRoom

### Entregas

- Import do `quarto_07_blockout.glb` em `DemoRoom.tscn`.
- Pontos auxiliares: spawn, porta, martelo, bilhete, luz.
- Colisoes temporarias do quarto e moveis.
- Documentacao de pipeline Blender (`docs/blender_pipeline/QUARTO_07_IMPORT.md`).

---

## Sprint 2 — Base jogavel do Quarto 07

**Commit:** `b10558b` — Prepare playable Quarto 07 vertical slice

### Entregas

- Bilhete interativo (`InteractableNote`).
- Martelo coletavel (`HammerPickup`) + inventario + placeholder na mao.
- Porta interativa debug (`DoorInteractable`).
- Primeira versao do corredor placeholder e trigger de fim.
- Guia de playtest (`docs/testing/PLAYTEST_DEMO_ROOM.md`).

---

## Sprint 3 — Corredor placeholder (conexao e polish)

**Data:** 2026-07-09 | **Status:** concluida

### Objetivo

Conectar o corredor apos a porta, corrigir queda no limbo, ajustar martelo na mao e HUD da lanterna.

### Entregas

- `CorridorPlaceholder` reestruturado em `DemoRoom.tscn` (visuais, colisoes, luz, `CorridorDarkZone`).
- Materiais do corredor: `materials/mat_corridor_*.tres`.
- `CorridorEndTrigger` + `scripts/levels/DemoEndTrigger.cs`.
- Martelo na mao: `WeaponHolder` / `EquippedHammerVisual` com escala e posicao ajustadas.
- `FlashlightController` + `HUDController`: bateria 100/100, dreno so com lanterna ligada.
- Corredor alinhado ao eixo **+Z** (porta `door_01` do GLB em Z positivo).

### Arquivos principais

- `scenes/levels/demo_room/DemoRoom.tscn`
- `scenes/player/Player.tscn`
- `scripts/player/FlashlightController.cs`
- `scripts/ui/HUDController.cs`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`

---

## Sprint 4 — Organizacao GlobalUsings

**Data:** 2026-07-09 | **Status:** concluida

### Objetivo

Centralizar `using` por camadas e limpar imports nos scripts C#.

### Entregas

- `GlobalUsings.cs` na raiz com camadas: Engine, System, Player, Inventory, Weapons, Interaction, Enemies, Levels, Ui, Doors, Horror, Debug.
- Remocao de `using` locais em todos os scripts `scripts/**/*.cs`.

---

## Sprint 5 — Atmosfera inicial e tensao no corredor

**Data:** 2026-07-09 | **Status:** concluida

### Objetivo

Primeira camada de terror funcional: som de porta, HUD melhorado, susto, radio e inimigo placeholder.

### Entregas

| Tarefa | Implementacao |
|--------|----------------|
| Porta com som | `scripts/doors/DoorAudioController.cs` + no `DoorAudio` na porta |
| HUD survival horror | `scenes/ui/HUD.tscn` + `HUDController` com painel, prompt e mensagens temporarias |
| Radio/interferencia | `Horror/RadioInterference` + `RadioInterferenceController.cs` |
| Primeiro susto | `CorridorScareTrigger` em Z 5.5 (+ flicker de luz) |
| Inimigo placeholder | `scenes/enemies/EnemyPlaceholder.tscn` + `EnemyPlaceholder.cs` em Z 8.2 |
| Sequencia da demo | `DemoRoomSequenceController.cs` (estados note/martelo/porta/susto) |
| Integracao | `DoorInteractable`, `HammerPickup`, `InteractableNote`, `PlayerInteractor` |

### Fora de escopo (mantido)

- Combate completo.
- IA avancada / navmesh.
- Modelo final do inimigo no Blender.
- Arquivos `.ogg` reais (nos de audio preparados com fallback seguro).

### Assets de audio pendentes

Ver secao "Assets futuros necessarios" em `docs/testing/PLAYTEST_DEMO_ROOM.md`.

---

## Proxima sprint recomendada

1. Adicionar arquivos `.ogg` (ver `assets/audio/AUDIO_ASSETS_NEEDED.md`).
2. Porta final / transicao de cena.
3. Ambience loops (quarto e corredor).
4. Inimigo Blender + perseguicao simples.

---

## Sprint 6 — UI narrativa e audio base

**Data:** 2026-07-09 | **Status:** concluida

### Objetivo

Polimento de atmosfera e leitura narrativa: UI do bilhete, sistema de audio opcional, fim do corredor preparado para transicao.

### Entregas

- Pastas `assets/audio/` + `AUDIO_ASSETS_NEEDED.md`.
- `NoteReaderUI.tscn` / `NoteReaderUI.cs` — leitura com E/Esc, bloqueio de movimento.
- `AudioManager`, `AudioPaths`, `AudioResourceLoader` — streams null-safe.
- Integracao audio: porta, radio, susto, martelo, lanterna.
- `CorridorEndDoorPlaceholder` + `DemoEndTrigger` com mensagem HUD.
- `docs/technical/AUDIO_SYSTEM.md`, `docs/design/ATMOSPHERE_GUIDE.md`.
- Regra `conventional-commits.mdc` (FEAT, FIX, DOCS, etc.).
- No `DemoRoom`: no `UI` com HUD, NoteReaderUI e AudioManager.

---

## Sprint 6b — Audio pack conectado

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `.ogg` do `breu_de_dentro_audio_pack_v01` copiados para caminhos canonicos.
- `AmbienceController` — quarto, vento e corredor (ao abrir porta).
- Porta, radio, susto, lanterna, pickup e inimigo tocando audio real.
- `AudioResourceLoader` com suporte a loop para ambiencia/radio.

---

## Sprint 7 — Passos, pulo e pouso

**Commit:** `b55fb18` — FEAT: passos, pulo e sons de movimento do player

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- Input `jump` = Space.
- Pulo baixo (`JumpVelocity` 4.0) com custo de stamina e sem double jump.
- `PlayerFootstepAudio` — passos walk/sprint, jump_start, land_soft/heavy.
- `PlayerStamina` adicionado ao `Player.tscn`.

---

## Sprint 8 — Superficie dos passos, stamina e agachamento

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `SurfaceType`, `SurfaceTag`, `PlayerGroundSurfaceDetector` + `GroundRay` no player.
- Passos concreto/madeira conforme chao; `WoodTestFloor` temporario no corredor.
- Stamina: dreno no sprint (`DrainPerSecond`); HUD sincroniza valor ao conectar.
- Agachamento (`Ctrl`): velocidade reduzida, camera/colisao mais baixas, sem sprint/pulo.
- `GlobalUsings`: namespace `BREU.Scripts.World`.

---

## Sprint 9 — Documentacao de visao (Fases 1 e 2)

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `docs/design/GAME_VISION.md` — visao geral do jogo.
- `docs/design/STORY_AND_LORE.md` — premissa, culto, O Breu, entidades.
- `docs/design/PHASE_01_LEVEL_DESIGN.md` — trilha, fachada, Quarto 07, corredor.
- `docs/design/PHASE_02_LEVEL_DESIGN.md` — Sala dos Santos Secos, O Hospede, quintal.
- `docs/design/SCENARIO_ART_DIRECTION.md`, `ENEMY_DESIGN.md`, `GAMEPLAY_PILLARS.md`.
- `docs/production/PHASE_01_02_SPRINT_PLAN.md` — sprints A–F de producao.

---

## Sprint 10 — Porta final e transicao Fase 2

**Data:** 2026-07-09 | **Status:** concluida

### Objetivo

Consolidar fim da Fase 1: porta interativa no corredor, fade entre cenas, placeholder da Fase 2.

### Entregas

- `CorridorEndDoorInteractable` — trancada ate `HasTriggeredCorridorScare`; depois `[E] Entrar`.
- `SceneTransitionController` (autoload) — fade out/in + `ChangeScene`.
- `scenes/levels/phase_02/RitualRoom.tscn` + `RitualRoomController`.
- Fluxo: Quarto 07 → corredor → susto → porta final → RitualRoom.
- `DemoEndTrigger` sem HUD automatico (porta cuida da mensagem).

### Arquivos principais

- `scripts/interaction/CorridorEndDoorInteractable.cs`
- `scripts/levels/SceneTransitionController.cs`
- `autoload/SceneTransition.tscn`
- `scenes/levels/phase_02/RitualRoom.tscn`
- `scenes/levels/demo_room/DemoRoom.tscn`

---

## Proxima sprint recomendada

1. Sprint B — trilha de entrada (`TrailIntro.tscn`).
2. Sprint D — Sala dos Santos Secos no Blender (substituir RitualRoom placeholder).
3. Sprint E — IA do inimigo placeholder (perseguicao, stun).
