# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

O fluxo inicial jogavel agora e:

```text
    TrailIntro -> DemoRoom -> corredor -> RitualRoom
```

A Trilha Noturna agora contem a fachada real da Pensao Santa Luzia no fim do caminho. A porta da Pensao leva direto para o Quarto 07. O Quarto 07 segue com bilhete, martelo, porta, corredor, susto, porta final e transicao para a Sala dos Santos Secos (`RitualRoom`).

A cena principal do projeto (`run/main_scene`) aponta para `TrailIntro.tscn`, para testar a entrada completa desde o inicio.

Historico: `docs/SPRINT_HISTORY.md`.

## Estado jogavel atual

### Player

- Movimento FPS: andar, correr com stamina, agachar (`Ctrl`) e pular (`Space`).
- Passos por superficie (`SurfaceTag` + grupos `surface_*`).
- Lanterna com HUD de bateria.
- HUD com stamina, lanterna, arma, prompts e mensagens temporarias.

### Trilha Noturna

- Cena: `scenes/levels/trail_intro/TrailIntro.tscn`.
- Asset: `assets/blender_exports/trail_intro/trail_intro_blockout.glb`.
- Player nasce em `Vector3(0, 1, 14)`.
- A fachada real da Pensao e instanciada como GLB visual em `Environment/HouseExteriorAtTrailEnd`.
- `Interactables/EnterPensionDoor` troca direto para `DemoRoom.tscn` com prompt `[E] Entrar na Pensao`.
- `HouseEntryTrigger` antigo foi mantido desativado como fallback.
- `DistantHouseSilhouette` foi mantida desativada como fallback visual.
- `SilhouetteMessageTrigger` mostra `A luz nao deveria estar acesa.`.
- `HouseFrontLanternLight` usa `LightFlicker` para oscilacao sutil do lampiao.

### Fachada da Pensao

- Cena: `scenes/levels/house_exterior/HouseExterior.tscn`.
- Asset: `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
- Status: cena isolada de teste/comparacao; nao faz mais parte obrigatoria do fluxo principal.
- Player nasce em `Vector3(0, 1, -8.5)`, no inicio do terreno da fachada, olhando para a porta.
- Colisoes temporarias: chao, paredes, varanda, blocker de porta e limites do terreno.
- Luzes: `MoonLight` fria e `FrontLanternLight` quente.
- Audio: `wind_old_house_01.ogg` em loop via `Audio/ExteriorAmbience`.
- `EnterHouseDoor` e interativo: mirar na porta e apertar `E` troca para `DemoRoom.tscn`.
- `DoorScentMessageTrigger` mostra `Tem cheiro de vela queimada.` perto da entrada.
- `BackToTrailTrigger` mostra mensagem, mas ainda nao troca cena.

### Quarto 07 -> Corredor -> Fase 2

1. Explorar quarto, bilhete (`NoteReaderUI`), martelo, porta do quarto.
2. Corredor (+Z): passos, susto em Z ~5.5, inimigo placeholder.
3. Porta final (Z ~9.1): trancada ate o susto -> `[E] Entrar` -> fade -> `scenes/levels/ritual_room/RitualRoom.tscn`.

### Sala dos Santos Secos

- Cena: `scenes/levels/ritual_room/RitualRoom.tscn`.
- Asset: `assets/blender_exports/ritual_room/sala_santos_secos_blockout.glb`.
- Player nasce em `Vector3(0, 1, -3.15)`, olhando para o centro da sala.
- Colisoes temporarias: chao, teto, paredes, mesa e porta de saida bloqueada.
- Luzes: `CandleLightMain`, `BackAltarLight` e `RoomDarkFill`.
- Audio: `RitualRoomAmbience` com `room_tone_01.ogg` e `RadioStaticPoint` 3D.
- Interativos: `RitualNotePoint`, `OldKeyPickupPoint` e `ExitDoorTrigger`.
- Susto: `RitualScareTrigger` pisca luzes, toca stinger, liga radio static e revela `EnemyPlaceholder`.
- Checkpoint: `RitualRoom_SantosSecos`.

### Sistemas

- `AudioManager` + pack v01 + `AmbienceController`.
- `SceneTransition` (autoload) para fade in/out entre cenas.
- `CheckpointManager` (autoload) registra checkpoints em memoria.
- `PlayerSpawnResolver` posiciona o player nos spawns de cada cena.
- `DemoRoomSequenceController` para estados da sequencia do Quarto 07.
- `EnterHouseTrigger` e usado na porta da Pensao integrada na trilha e na cena isolada da fachada.
- `HouseEntryTrigger` permanece apenas como fallback antigo desativado na trilha.

## Cenas principais

| Cena | Caminho |
|------|---------|
| Trilha inicial | `scenes/levels/trail_intro/TrailIntro.tscn` |
| Fachada isolada/teste | `scenes/levels/house_exterior/HouseExterior.tscn` |
| Demo / Fase 1 | `scenes/levels/demo_room/DemoRoom.tscn` |
| Sala dos Santos Secos | `scenes/levels/ritual_room/RitualRoom.tscn` |

## Fora de escopo ainda

- Combate funcional completo.
- IA de perseguicao avancada.
- Porta visual/animada na fachada.
- Inimigo Blender final.
- Sala dos Santos Secos finalizada/polida.
- Preservacao de estado/posicao entre cenas.

## Verificacao

- `dotnet build BREU.sln` - 0 erros (2026-07-09).
- Godot editor headless importou `pensao_santa_luzia_exterior_blockout.glb` (2026-07-09).

## Proximo passo

Validar manualmente com F6:

1. `TrailIntro.tscn`
2. caminhar ate a fachada real no fim da trilha;
3. mirar na porta e apertar `E`;
4. confirmar `DemoRoom.tscn`.

Depois, ajustar colisoes/escala da fachada e criar porta visual animada.

## Manutencao

Antes de commit/push: atualizar `docs/SPRINT_HISTORY.md`, verificar `.cursor/rules/pre-commit-docs.mdc` e usar commits convencionais quando aplicavel.
