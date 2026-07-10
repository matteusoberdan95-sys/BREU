# BREU - Handoff

Ultima atualizacao: 2026-07-09

## Retomar

1. Ler `docs/START_HERE.md`.
2. Ler `docs/PROJECT_STATE.md`.
3. Ler `docs/SPRINT_HISTORY.md`.
4. Ler `docs/gameplay/NEXT_SPRINT_TASKS.md`.
5. Para direcao macro, ler `docs/design/GAME_VISION.md` e `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Ultimas entregas

### Sprint D - Sala dos Santos Secos

- Criada `scenes/levels/ritual_room/RitualRoom.tscn`.
- Instanciado `assets/blender_exports/ritual_room/sala_santos_secos_blockout.glb`.
- Reutilizado `Player.tscn`, HUD, NoteReaderUI, AudioManager e EnemyPlaceholder.
- Adicionadas colisoes temporarias de sala, mesa e porta de saida.
- Adicionadas luzes `CandleLightMain`, `BackAltarLight` e `RoomDarkFill`.
- Criados `RitualNoteInteractable.cs`, `OldKeyPickup.cs`, `RitualRoomScareTrigger.cs` e `RitualExitDoorTrigger.cs`.
- Porta final do corredor agora troca para `scenes/levels/ritual_room/RitualRoom.tscn`.
- Guia novo: `docs/testing/PLAYTEST_RITUAL_ROOM.md`.

### Decisao de fluxo - Fachada integrada na Trilha

- `TrailIntro.tscn` agora instancia o GLB visual da fachada em `Environment/HouseExteriorAtTrailEnd`.
- `DistantHouseSilhouette` ficou preservada, mas oculta.
- `HouseEntryTrigger` antigo ficou preservado, mas desativado.
- Nova entrada principal: `Interactables/EnterPensionDoor`.
- A porta da Pensao na trilha troca direto para `DemoRoom.tscn`.
- `HouseExterior.tscn` continua existindo apenas como cena isolada de teste/comparacao.

### Sprint C.5 - Costura da Fase 1

- Cena principal do projeto agora inicia em `TrailIntro.tscn`.
- `SceneTransition` ganhou `ChangeSceneWithFade(scenePath, message)` e mensagem opcional durante fade.
- Criado `PlayerSpawnResolver.cs` para posicionar o player nos marcadores de spawn ao abrir/trocar cena.
- Criado `CheckpointManager.cs` como autoload em memoria.
- Criados `OneShotMessageTrigger.cs` e `LightFlicker.cs`.
- Mensagens narrativas adicionadas na trilha, fachada e inicio do quarto.
- `EnterHouseTrigger` foi organizado como `EnterHouseDoor` na cena da fachada.
- Guia novo: `docs/testing/PLAYTEST_PHASE_01_FLOW.md`.

### Sprint C - Fachada da Pensao

- Criada `scenes/levels/house_exterior/HouseExterior.tscn`.
- Instanciado `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
- Player reutilizado em `Vector3(0, 1, -8.5)`, no inicio do terreno da fachada, olhando para a porta.
- Colisoes temporarias: chao, paredes, varanda, blocker de porta e limites.
- Luzes: `MoonLight` e `FrontLanternLight`.
- Ambiencia: `Audio/ExteriorAmbience` com `wind_old_house_01.ogg`.
- Criados `EnterHouseTrigger.cs` e `BackToTrailTrigger.cs`.
- `EnterHouseTrigger` agora e interativo: mirar na porta e apertar `E` para ir ao Quarto 07.
- Historico: `HouseEntryTrigger.cs` ja foi usado para `TrailIntro -> HouseExterior`; no fluxo atual ele esta desativado na trilha.
- Escondidos na cena da trilha os meshes `house_placeholder_*` do GLB antigo.
- Criada `DistantHouseSilhouette` em `TrailIntro.tscn`, silhueta simples e sem colisao para servir como objetivo visual distante.
- Criado guia `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`.
- Atualizado `docs/testing/PLAYTEST_TRAIL_INTRO.md`.

### Fluxo inicial

```text
TrailIntro -> DemoRoom -> RitualRoom
```

### Demo Room / Fase 1

- Player em primeira pessoa com stamina, agachamento, pulo, lanterna e passos por superficie.
- Quarto 07 com bilhete, martelo, porta do quarto, corredor, susto, porta final e transicao para `RitualRoom.tscn`.

## Testar fluxo completo

1. Abrir `res://scenes/levels/trail_intro/TrailIntro.tscn`.
2. Rodar com F6.
3. Caminhar ate a casa, em direcao a `Z negativo`.
4. Confirmar que a fachada real aparece no fim da trilha.
5. Mirar na porta e apertar `E` no prompt `[E] Entrar na Pensao`.
6. Confirmar transicao para `DemoRoom.tscn`.
7. Abrir a porta do Quarto 07, atravessar o corredor e disparar o susto.
8. Mirar na porta final e apertar `E`.
9. Confirmar transicao para `RitualRoom.tscn`.

Guias:

- `docs/testing/PLAYTEST_TRAIL_INTRO.md`
- `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `docs/testing/PLAYTEST_PHASE_01_FLOW.md`
- `docs/testing/PLAYTEST_RITUAL_ROOM.md`

## Validacao feita

- `dotnet build BREU.sln`: sucesso, 0 erros.
- Godot editor headless importou o GLB da fachada.

Observacao: o editor headless emite erros de cache/config em `AppData` nesta maquina, mas importa os recursos do projeto.

## Proximo recomendado

1. Testar manualmente `RitualRoom.tscn` isolada.
2. Testar fluxo completo `TrailIntro -> DemoRoom -> RitualRoom`.
3. Ajustar escala/colisoes da Sala dos Santos Secos conforme o GLB.
4. Integrar Chave Velha ao inventario e criar objetivo para liberar a saida.
