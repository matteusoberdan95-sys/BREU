# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

Vertical slice do Quarto 07 com corredor, susto, porta final interativa e transicao para Fase 2 (`RitualRoom` placeholder). A primeira cena da Trilha Noturna tambem existe para testar a caminhada inicial ate a Pensao Santa Luzia.

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
- Player nasce em `Vector3(0, 1, 14)`, no inicio da trilha mostrado no playtest.
- Colisoes temporarias: chao da trilha e bloqueios laterais.
- Luzes: `MoonLight` fria e `DistantHouseLight` amarelada perto da casa.
- Audio: `wind_old_house_01.ogg` em loop via `Audio/TrailAmbience`.
- Trigger: `HouseEntryTrigger` imprime chegada e mostra mensagem no HUD, sem trocar cena ainda.

### Quarto 07 -> Corredor -> Fase 2

1. Explorar quarto, bilhete (`NoteReaderUI`), martelo, porta do quarto.
2. Corredor (+Z): passos, susto em Z ~5.5, inimigo placeholder.
3. Porta final (Z ~9.1): trancada ate o susto -> `[E] Entrar` -> fade -> `RitualRoom.tscn`.

### Sistemas

- `AudioManager` + pack v01 + `AmbienceController`.
- `SceneTransition` (autoload) para fade in/out entre cenas.
- `DemoRoomSequenceController` para estados da sequencia do Quarto 07.
- `HouseEntryTrigger` para chegada na Pensao Santa Luzia.

## Cenas principais

| Cena | Caminho |
|------|---------|
| Trilha inicial | `scenes/levels/trail_intro/TrailIntro.tscn` |
| Demo / Fase 1 | `scenes/levels/demo_room/DemoRoom.tscn` |
| Fase 2 placeholder | `scenes/levels/phase_02/RitualRoom.tscn` |

## Fora de escopo ainda

- Combate funcional completo.
- IA de perseguicao avancada.
- Fachada da casa.
- Inimigo Blender final.
- Sala dos Santos Secos modelada.

## Verificacao

- `dotnet build BREU.sln` - 0 erros (2026-07-09).
- Godot editor headless importou `trail_intro_blockout.glb` e `wind_old_house_01.ogg` (2026-07-09).
- A carga direta de `TrailIntro.tscn` via `--scene` derrubou o executavel headless nesta maquina; validar a cena manualmente com F6.

## Proximo passo

Sprint B: ajustar escala/orientacao da Trilha Noturna no editor e criar `HouseExterior.tscn`. Depois, conectar `HouseEntryTrigger` a uma transicao real para a fachada.

Ver `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Manutencao

Antes de commit/push: atualizar `docs/SPRINT_HISTORY.md`, verificar `.cursor/rules/pre-commit-docs.mdc` e usar commits convencionais quando aplicavel.
