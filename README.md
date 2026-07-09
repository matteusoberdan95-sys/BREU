# BREU DE DENTRO

Survival horror brutal em primeira pessoa — Godot 4.7 Mono/C#, assets no Blender.

O jogador explora a **Pensão Santa Luzia** no interior do Nordeste (anos 70/80): quarto preparado, bilhete ameaçador, corredor escuro, susto e porta final que leva à **Fase 2 — Sala dos Santos Secos** (placeholder).

## Comece aqui

1. [`docs/START_HERE.md`](docs/START_HERE.md)
2. [`docs/PROJECT_STATE.md`](docs/PROJECT_STATE.md)
3. [`docs/design/GAME_VISION.md`](docs/design/GAME_VISION.md)
4. [`docs/HANDOFF.md`](docs/HANDOFF.md)
5. [`docs/gameplay/NEXT_SPRINT_TASKS.md`](docs/gameplay/NEXT_SPRINT_TASKS.md)

## Estado atual (2026-07-09)

| Area | Status |
|------|--------|
| Quarto 07 (Blender GLB) | Jogavel |
| Player FPS | Andar, correr, agachar, pular, lanterna |
| Audio | Pack v01 + passos por superficie |
| Fase 1 | Quarto → corredor → susto → porta final |
| Fase 2 | `RitualRoom.tscn` placeholder |
| Combate / IA | Placeholder apenas |

### Fluxo da demo

```
Quarto 07 → porta do quarto → corredor → susto (Z~5.5) → porta final (Z~9.1) → RitualRoom
```

### Cenas

- **Fase 1:** `res://scenes/levels/demo_room/DemoRoom.tscn`
- **Fase 2:** `res://scenes/levels/phase_02/RitualRoom.tscn`

## Como testar

1. Abrir `BREU` no Godot 4.7 Mono.
2. F6 em `DemoRoom.tscn`.
3. Controles:
   - `WASD` — mover | `Shift` — correr | `Ctrl` — agachar | `Space` — pular
   - `Mouse` — olhar | `F` — lanterna | `E` — interagir
4. Guia completo: [`docs/testing/PLAYTEST_DEMO_ROOM.md`](docs/testing/PLAYTEST_DEMO_ROOM.md)

## Build

```powershell
dotnet build BREU.sln
```

Godot (esta maquina):

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe
```

## Documentacao de design

- Visao e lore: `docs/design/GAME_VISION.md`, `STORY_AND_LORE.md`
- Level design Fases 1–2: `docs/design/PHASE_01_LEVEL_DESIGN.md`, `PHASE_02_LEVEL_DESIGN.md`
- Plano de producao: `docs/production/PHASE_01_02_SPRINT_PLAN.md`
- Historico: `docs/SPRINT_HISTORY.md`

## Continuidade

Antes de commit/push, atualizar `docs/SPRINT_HISTORY.md`, `PROJECT_STATE.md`, `HANDOFF.md` e `NEXT_SPRINT_TASKS.md` (ver `.cursor/rules/pre-commit-docs.mdc`).
