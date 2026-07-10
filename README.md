# BREU DE DENTRO

Survival horror brutal em primeira pessoa, feito em Godot 4.7 Mono/C# com assets criados no Blender.

O jogador explora a Pensao Santa Luzia no interior do Nordeste brasileiro entre os anos 70/80. O fluxo atual comeca na trilha noturna, passa pela fachada da Pensao, entra no Quarto 07, segue por um corredor escuro, susto, porta final e chega a Sala dos Santos Secos.

## Comece aqui

1. [`docs/START_HERE.md`](docs/START_HERE.md)
2. [`docs/PROJECT_STATE.md`](docs/PROJECT_STATE.md)
3. [`docs/HANDOFF.md`](docs/HANDOFF.md)
4. [`docs/SPRINT_HISTORY.md`](docs/SPRINT_HISTORY.md)
5. [`docs/gameplay/NEXT_SPRINT_TASKS.md`](docs/gameplay/NEXT_SPRINT_TASKS.md)
6. [`docs/design/GAME_VISION.md`](docs/design/GAME_VISION.md)

## Estado atual

| Area | Status |
|------|--------|
| Trilha Noturna | Jogavel, leva para fachada |
| Fachada da Pensao | Integrada visualmente no fim da trilha; cena isolada mantida |
| Quarto 07 | Jogavel |
| Player FPS | Andar, correr, agachar, pular, lanterna |
| Audio | Pack v01 + passos por superficie + vento |
| Fase 1 | Quarto -> corredor -> susto -> porta final |
| Fase 2 | Sala dos Santos Secos jogavel inicial |
| Combate / IA | Placeholder apenas |

A cena principal do projeto ja aponta para `TrailIntro.tscn`, entao o botao Play testa a entrada completa da vertical slice.

## Cenas principais

- Trilha inicial: `res://scenes/levels/trail_intro/TrailIntro.tscn`
- Fachada isolada/teste: `res://scenes/levels/house_exterior/HouseExterior.tscn`
- Fase 1 / Quarto 07: `res://scenes/levels/demo_room/DemoRoom.tscn`
- Sala dos Santos Secos: `res://scenes/levels/ritual_room/RitualRoom.tscn`

## Fluxos atuais

```text
TrailIntro -> DemoRoom
```

```text
Quarto 07 -> porta do quarto -> corredor -> susto -> porta final -> RitualRoom
```

## Como testar

1. Abrir `BREU` no Godot 4.7 Mono.
2. Abrir uma das cenas principais.
3. Rodar com F6.
4. Clicar na janela/aba **Entrada** para dar foco.

Controles:

- `WASD`: mover
- `Shift`: correr
- `Ctrl`: agachar
- `Space`: pular
- `Mouse`: olhar
- `F`: lanterna
- `E`: interagir

Guias:

- [`docs/testing/PLAYTEST_PHASE_01_FLOW.md`](docs/testing/PLAYTEST_PHASE_01_FLOW.md)
- [`docs/testing/PLAYTEST_TRAIL_INTRO.md`](docs/testing/PLAYTEST_TRAIL_INTRO.md)
- [`docs/testing/PLAYTEST_HOUSE_EXTERIOR.md`](docs/testing/PLAYTEST_HOUSE_EXTERIOR.md)
- [`docs/testing/PLAYTEST_DEMO_ROOM.md`](docs/testing/PLAYTEST_DEMO_ROOM.md)
- [`docs/testing/PLAYTEST_RITUAL_ROOM.md`](docs/testing/PLAYTEST_RITUAL_ROOM.md)

## Build

```powershell
dotnet build BREU.sln
```

Godot usado nesta maquina:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe
```

## Documentacao de design

- Visao e lore: `docs/design/GAME_VISION.md`, `docs/design/STORY_AND_LORE.md`
- Level design: `docs/design/PHASE_01_LEVEL_DESIGN.md`, `docs/design/PHASE_02_LEVEL_DESIGN.md`
- Plano de producao: `docs/production/PHASE_01_02_SPRINT_PLAN.md`
- Historico: `docs/SPRINT_HISTORY.md`

## Continuidade

Antes de commit/push, atualizar:

- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/SPRINT_HISTORY.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`

Ver tambem `.cursor/rules/pre-commit-docs.mdc`.
