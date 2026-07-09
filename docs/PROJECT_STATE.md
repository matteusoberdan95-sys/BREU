# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

O fluxo inicial jogavel agora e:

```text
TrailIntro -> HouseExterior -> DemoRoom -> corredor -> RitualRoom placeholder
```

A Trilha Noturna leva para a fachada da Pensao Santa Luzia. A fachada leva para o Quarto 07. O Quarto 07 segue com bilhete, martelo, porta, corredor, susto, porta final e transicao para Fase 2 (`RitualRoom` placeholder).

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
- Trigger `HouseEntryTrigger` em `Z -12.9` troca para `HouseExterior.tscn`, antes da antiga casinha placeholder do GLB da trilha.
- `DistantHouseSilhouette` em `Z -16.8` cria uma silhueta simples da Pensao como objetivo visual, sem carregar a fachada real.
- `SilhouetteMessageTrigger` mostra `A luz nao deveria estar acesa.`.
- A luz distante usa `LightFlicker` para oscilacao sutil.

### Fachada da Pensao

- Cena: `scenes/levels/house_exterior/HouseExterior.tscn`.
- Asset: `assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
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
3. Porta final (Z ~9.1): trancada ate o susto -> `[E] Entrar` -> fade -> `RitualRoom.tscn`.

### Sistemas

- `AudioManager` + pack v01 + `AmbienceController`.
- `SceneTransition` (autoload) para fade in/out entre cenas.
- `CheckpointManager` (autoload) registra checkpoints em memoria.
- `PlayerSpawnResolver` posiciona o player nos spawns de cada cena.
- `DemoRoomSequenceController` para estados da sequencia do Quarto 07.
- `HouseEntryTrigger`, `EnterHouseTrigger` e `BackToTrailTrigger` para fluxo inicial.

## Cenas principais

| Cena | Caminho |
|------|---------|
| Trilha inicial | `scenes/levels/trail_intro/TrailIntro.tscn` |
| Fachada | `scenes/levels/house_exterior/HouseExterior.tscn` |
| Demo / Fase 1 | `scenes/levels/demo_room/DemoRoom.tscn` |
| Fase 2 placeholder | `scenes/levels/phase_02/RitualRoom.tscn` |

## Fora de escopo ainda

- Combate funcional completo.
- IA de perseguicao avancada.
- Porta visual/animada na fachada.
- Inimigo Blender final.
- Sala dos Santos Secos modelada.
- Preservacao de estado/posicao entre cenas.

## Verificacao

- `dotnet build BREU.sln` - 0 erros (2026-07-09).
- Godot editor headless importou `pensao_santa_luzia_exterior_blockout.glb` (2026-07-09).

## Proximo passo

Validar manualmente com F6:

1. `TrailIntro.tscn`
2. caminhar ate a casa;
3. confirmar `HouseExterior.tscn`;
4. caminhar ate a porta;
5. confirmar `DemoRoom.tscn`.

Depois, ajustar colisoes/escala da fachada e criar porta visual animada.

## Manutencao

Antes de commit/push: atualizar `docs/SPRINT_HISTORY.md`, verificar `.cursor/rules/pre-commit-docs.mdc` e usar commits convencionais quando aplicavel.
