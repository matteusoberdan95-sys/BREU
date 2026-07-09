# BREU - Handoff

Ultima atualizacao: 2026-07-09

## Retomar

1. Ler `docs/START_HERE.md`.
2. Ler `docs/PROJECT_STATE.md`.
3. Ler `docs/SPRINT_HISTORY.md`.
4. Ler `docs/gameplay/NEXT_SPRINT_TASKS.md`.
5. Para direcao macro, ler `docs/design/GAME_VISION.md` e `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Ultimas entregas

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
- Atualizado `HouseEntryTrigger.cs` para trocar `TrailIntro -> HouseExterior`.
- Escondidos na cena da trilha os meshes `house_placeholder_*` do GLB antigo.
- Criada `DistantHouseSilhouette` em `TrailIntro.tscn`, silhueta simples e sem colisao para servir como objetivo visual distante.
- Criado guia `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`.
- Atualizado `docs/testing/PLAYTEST_TRAIL_INTRO.md`.

### Fluxo inicial

```text
TrailIntro -> HouseExterior -> DemoRoom
```

### Demo Room / Fase 1

- Player em primeira pessoa com stamina, agachamento, pulo, lanterna e passos por superficie.
- Quarto 07 com bilhete, martelo, porta do quarto, corredor, susto, porta final e transicao para `RitualRoom.tscn`.

## Testar fluxo completo

1. Abrir `res://scenes/levels/trail_intro/TrailIntro.tscn`.
2. Rodar com F6.
3. Caminhar ate a casa, em direcao a `Z negativo`.
4. Entrar no `HouseEntryTrigger` perto de `Z -12.9`.
5. Confirmar transicao para `HouseExterior.tscn`.
6. Na fachada, caminhar ate a porta.
7. Mirar na porta e apertar `E` no prompt `[E] Entrar na Pensao`.
8. Confirmar transicao para `DemoRoom.tscn`.

Guias:

- `docs/testing/PLAYTEST_TRAIL_INTRO.md`
- `docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `docs/testing/PLAYTEST_PHASE_01_FLOW.md`

## Validacao feita

- `dotnet build BREU.sln`: sucesso, 0 erros.
- Godot editor headless importou o GLB da fachada.

Observacao: o editor headless emite erros de cache/config em `AppData` nesta maquina, mas importa os recursos do projeto.

## Proximo recomendado

1. Testar manualmente o fluxo completo pelo Play principal do Godot.
2. Ajustar colisoes da trilha/fachada conforme o GLB.
3. Criar porta visual animada na fachada.
4. Evoluir `CheckpointManager` para salvar/restaurar estado quando fizer sentido.
