# BREU - Historico de sprints

Registro cronologico das sprints e alteracoes relevantes do projeto.

Ultima atualizacao: 2026-07-09

## Sprint 0 - Fundacao da vertical slice

**Commit:** `db3907a` - Initial BREU vertical slice foundation

### Entregas

- Estrutura base Godot 4.7 Mono/C#.
- Player FPS, lanterna, interacao e HUD minimo.
- Sistemas iniciais de inventario, armas, inimigos e docs.

## Sprint 1 - Integracao Blender Quarto 07

**Commit:** `d2bc0ce` - Integrar blockout Blender do Quarto 07 em DemoRoom

### Entregas

- Import do `quarto_07_blockout.glb` em `DemoRoom.tscn`.
- Pontos auxiliares: spawn, porta, martelo, bilhete e luz.
- Colisoes temporarias do quarto e moveis.

## Sprint 2 - Base jogavel do Quarto 07

**Commit:** `b10558b` - Prepare playable Quarto 07 vertical slice

### Entregas

- Bilhete interativo.
- Martelo coletavel + inventario + placeholder na mao.
- Porta interativa debug.
- Corredor placeholder e trigger de fim.
- Guia de playtest do DemoRoom.

## Sprint 3 - Corredor placeholder

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `CorridorPlaceholder` reestruturado em `DemoRoom.tscn`.
- Materiais do corredor.
- `DemoEndTrigger`.
- Ajustes do martelo na mao e HUD da lanterna.

## Sprint 4 - Organizacao GlobalUsings

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `GlobalUsings.cs` com namespaces principais.
- Limpeza de imports locais em scripts C#.

## Sprint 5 - Atmosfera inicial e tensao no corredor

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- Som de porta.
- HUD survival horror com mensagens temporarias.
- Radio/interferencia.
- `CorridorScareTrigger`.
- `EnemyPlaceholder`.
- `DemoRoomSequenceController`.

## Sprint 6 - UI narrativa e audio base

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `NoteReaderUI`.
- `AudioManager`, `AudioPaths`, `AudioResourceLoader`.
- Audio integrado em porta, radio, susto, pickup e lanterna.
- Porta final placeholder preparada.

## Sprint 6b - Audio pack conectado

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `.ogg` do audio pack v01 nos caminhos canonicos.
- `AmbienceController`.
- Ambiencias de quarto, corredor e vento.

## Sprint 7 - Passos, pulo e pouso

**Commit:** `b55fb18` - FEAT: passos, pulo e sons de movimento do player

### Entregas

- Input `jump` = Space.
- Pulo com custo de stamina.
- `PlayerFootstepAudio`.
- `PlayerStamina` no player.

## Sprint 8 - Superficie dos passos, stamina e agachamento

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `SurfaceType`, `SurfaceTag`, `PlayerGroundSurfaceDetector`.
- Passos por superficie.
- Stamina no sprint.
- Agachamento (`Ctrl`).

## Sprint 9 - Documentacao de visao

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `docs/design/GAME_VISION.md`.
- `docs/design/STORY_AND_LORE.md`.
- `docs/design/PHASE_01_LEVEL_DESIGN.md`.
- `docs/design/PHASE_02_LEVEL_DESIGN.md`.
- Guias de direcao de arte, inimigos, pilares e plano de producao.

## Sprint 10 - Porta final e transicao Fase 2

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `CorridorEndDoorInteractable`.
- `SceneTransitionController` autoload.
- `RitualRoom.tscn` placeholder.
- Fluxo `Quarto 07 -> corredor -> susto -> porta final -> RitualRoom`.

## Sprint 11 - Trilha Noturna blockout

**Commit:** `1c6222e` - FEAT: add trail intro playtest scene

**Data:** 2026-07-09 | **Status:** concluida

### Entregas

- `TrailIntro.tscn`.
- `trail_intro_blockout.glb`.
- Player no inicio da trilha.
- Colisoes temporarias.
- Luz da casa ao longe.
- Ambiencia de vento.
- `HouseEntryTrigger`.
- Guia `PLAYTEST_TRAIL_INTRO.md`.

## Sprint 12 - Fachada da Pensao Santa Luzia

**Data:** 2026-07-09 | **Status:** base criada

### Entregas

- `HouseExterior.tscn`.
- `pensao_santa_luzia_exterior_blockout.glb`.
- Player na frente da fachada.
- Colisoes temporarias da fachada: terreno, paredes, varanda, porta e limites.
- `EnterHouseTrigger` interativo para `HouseExterior -> DemoRoom`.
- `BackToTrailTrigger` informativo para retorno futuro.
- `HouseEntryTrigger` atualizado para `TrailIntro -> HouseExterior`.
- Casinha `house_placeholder_*` do GLB da trilha escondida em `TrailIntro.tscn`.
- `DistantHouseSilhouette` criada na trilha como forma escura simples com luz quente distante.
- Iluminacao noturna com lua e lampiao.
- Ambiencia externa com `wind_old_house_01.ogg`.
- Guia `PLAYTEST_HOUSE_EXTERIOR.md`.

### Fluxo

```text
TrailIntro -> HouseExterior -> DemoRoom
```

### Validacao

- `dotnet build BREU.sln` passou com 0 erros.
- Godot editor headless importou o GLB da fachada.

## Sprint 13 - Costura da Fase 1

**Data:** 2026-07-09 | **Status:** base criada

### Entregas

- Cena principal do projeto inicia em `TrailIntro.tscn`.
- `SceneTransitionController` atualizado com `ChangeSceneWithFade(scenePath, message)` e mensagem opcional.
- `CheckpointManager` em memoria como autoload.
- `PlayerSpawnResolver` aplicado em `TrailIntro`, `HouseExterior` e `DemoRoom`.
- `OneShotMessageTrigger` para frases curtas de atmosfera.
- `LightFlicker` aplicado na luz distante da silhueta da Pensao.
- Porta da fachada organizada como `EnterHouseDoor`, mantendo interacao por `E`.
- Guia `docs/testing/PLAYTEST_PHASE_01_FLOW.md`.

### Fluxo

```text
TrailIntro -> HouseExterior -> DemoRoom
```

## Proxima sprint recomendada

## Sprint 14 - Sala dos Santos Secos

**Data:** 2026-07-09 | **Status:** base criada

### Entregas

- `scenes/levels/ritual_room/RitualRoom.tscn`.
- `sala_santos_secos_blockout.glb` instanciado em `Environment`.
- Player, HUD, NoteReaderUI, AudioManager e PlayerSpawnResolver reutilizados.
- Colisoes temporarias da sala, mesa e porta de saida.
- Luzes `CandleLightMain`, `BackAltarLight` e `RoomDarkFill`.
- Ambiencia `room_tone_01.ogg` e `RadioStaticPoint`.
- `RitualNoteInteractable` com checkpoint `Ritual_Note_Read`.
- `OldKeyPickup` com estado local `HasOldKey`.
- `RitualRoomScareTrigger` com stinger, radio static, flicker e aparicao do `EnemyPlaceholder`.
- `RitualExitDoorTrigger` bloqueado.
- Porta final do corredor atualizada para `RitualRoom.tscn` nova.
- Guia `docs/testing/PLAYTEST_RITUAL_ROOM.md`.

### Fluxo

```text
TrailIntro -> HouseExterior -> DemoRoom -> RitualRoom
```

## Proxima sprint recomendada

## Sprint 15 - Fachada real integrada na TrailIntro

**Data:** 2026-07-10 | **Status:** base criada

### Entregas

- `TrailIntro.tscn` agora instancia o GLB visual da fachada em `Environment/HouseExteriorAtTrailEnd`.
- `DistantHouseSilhouette` preservada, mas oculta.
- `HouseEntryTrigger` preservado, mas desativado.
- Criado `Interactables/EnterPensionDoor` na trilha, usando `EnterHouseTrigger`.
- Porta da Pensao na trilha transiciona direto para `DemoRoom.tscn`.
- Colisoes auxiliares da fachada integradas em `Collisions/HouseExteriorCollisions`.
- `HouseFrontLanternLight` com `LightFlicker`.
- `HouseExterior.tscn` mantida como cena isolada de teste/comparacao.

### Fluxo

```text
TrailIntro -> DemoRoom -> RitualRoom
```

## Proxima sprint recomendada

## Sprint 16 - IA basica do EnemyPlaceholder

**Data:** 2026-07-10 | **Status:** base criada

### Entregas

- `EnemyPlaceholder.tscn` convertido para `CharacterBody3D`.
- `EnemyPlaceholderAI.cs` com estados `Dormant`, `Idle`, `Alert`, `Chasing`, `Attacking`, `Stunned`.
- Perseguicao direta simples com `MoveAndSlide`.
- Ataque simples com cooldown e dano.
- `PlayerHealth.cs` em `Player.tscn`.
- Audio 3D de respiracao, passos e growl.
- `RitualRoomScareTrigger` passa a chamar `ActivateEnemy()`.
- `ApplyStun()` e `ReceiveHit()` preparados.
- Ajustado playtest do inimigo: capsula/visual com origem nos pes, spawn da RitualRoom em `Vector3(-0.85, 0.05, 2.45)` e recuperacao de piso limitada ao ajuste inicial.

### Validacao esperada

```text
RitualRoom -> RitualScareTrigger -> EnemyPlaceholder aparece -> persegue -> ataca
```

## Proxima sprint recomendada

## Sprint 17 - Persistencia do martelo entre cenas

**Data:** 2026-07-10 | **Status:** base criada

### Entregas

- Criado `GameSession` como autoload em memoria.
- `HammerPickup` equipa o Martelo Enferrujado no `GameSession`.
- Criado `PlayerWeaponController` para reequilibrar Player/HUD/visual da arma ao carregar nova cena.
- `PlayerInventory` passou a sincronizar nome, durabilidade e durabilidade maxima da arma persistente.
- `OldKeyPickup` marca Chave Velha no `GameSession` sem limpar a arma equipada.
- Documentado teste de persistencia do martelo entre Quarto 07 e RitualRoom.

### Validacao esperada

```text
Quarto 07 -> pegar martelo -> RitualRoom -> HUD/visual ainda mostram Martelo Enferrujado 10/10
```

## Proxima sprint recomendada

## Sprint 18 - Combate basico com Martelo Enferrujado

**Data:** 2026-07-10 | **Status:** base criada

### Entregas

- Criado `PlayerMeleeAttack.cs`.
- Adicionado input `attack` no botao esquerdo.
- Ataque melee por raycast curto partindo da `Camera3D`.
- Hit em `EnemyPlaceholderAI.ReceiveHit(HammerDamage)`.
- Stun do inimigo usando `ApplyStun(StunDuration)`.
- Durabilidade do martelo reduzida no `GameSession` apenas quando acerta.
- HUD atualizado apos hit e quebra.
- Martelo volta para `Maos vazias` quando a durabilidade chega a 0.
- Feedback visual simples do martelo na mao via tween.
- Audio de hit usa fallback `corridor_hit_01.ogg` quando audio dedicado nao existe.
- Fix: raycast do martelo agora detecta bodies/areas, usa mask ampla, adiciona logs de debug e encontra `EnemyPlaceholderAI` mesmo quando o collider acertado e filho do inimigo.
- Fix: ataque agora usa hit volume esferico quando o raycast fino erra, melhorando acerto corpo a corpo em inimigo proximo.
- Fix: adicionada `EnemyHurtbox` ao `EnemyPlaceholder` e filtro de alvo para ignorar interactables/triggers no hit volume.

### Validacao esperada

```text
RitualRoom -> mirar EnemyPlaceholder -> clique esquerdo -> stun -> durabilidade 9/10 -> quebra em 0/10
```

## Proxima sprint recomendada

1. Testar manualmente a Sala dos Santos Secos isolada.
2. Ajustar escala/colisoes conforme o GLB no editor.
3. Criar objetivo com Chave Velha para liberar a porta de saida.
4. Integrar custo de stamina e feedback de vida/dano.
