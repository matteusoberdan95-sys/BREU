# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

Vertical slice do **Quarto 07** com corredor, susto, porta final interativa e transicao para **Fase 2 (RitualRoom placeholder)**. Historico: `docs/SPRINT_HISTORY.md`.

## Estado jogavel atual

### Player

- Movimento FPS: andar, correr (drena stamina), agachar (`Ctrl`), pular (`Space`).
- Passos por superficie (raycast): concreto / madeira (`SurfaceTag` + grupos `surface_*`).
- Lanterna com HUD de bateria; stamina no HUD (sprint e pulo).

### Quarto 07 → Corredor → Fase 2

1. Explorar quarto, bilhete (`NoteReaderUI`), martelo, porta do quarto.
2. Corredor (+Z): passos, susto em Z ~5.5, inimigo placeholder.
3. Porta final (Z ~9.1): trancada ate o susto → `[E] Entrar` → fade → `RitualRoom.tscn`.

### Sistemas

- `AudioManager` + pack v01 + `AmbienceController`.
- `SceneTransition` (autoload) — fade in/out entre cenas.
- `DemoRoomSequenceController` — estados da sequencia (bilhete, martelo, porta, susto).

### Documentacao de design (nova)

- `docs/design/GAME_VISION.md`, `STORY_AND_LORE.md`, `PHASE_01/02_LEVEL_DESIGN.md`
- `docs/design/SCENARIO_ART_DIRECTION.md`, `ENEMY_DESIGN.md`, `GAMEPLAY_PILLARS.md`
- `docs/production/PHASE_01_02_SPRINT_PLAN.md`

## Cenas principais

| Cena | Caminho |
|------|---------|
| Demo / Fase 1 | `scenes/levels/demo_room/DemoRoom.tscn` |
| Fase 2 placeholder | `scenes/levels/phase_02/RitualRoom.tscn` |

## Fora de escopo (ainda)

- Combate funcional completo, IA de perseguicao avancada.
- Inimigo Blender final, trilha de entrada, fachada da casa.
- Sala dos Santos Secos modelada (substituir placeholder RitualRoom).

## Verificacao

`dotnet build BREU.sln` — 0 erros (2026-07-09).

## Proximo passo

Sprint B — trilha de entrada; Sprint D — Sala dos Santos Secos no Blender. Ver `docs/production/PHASE_01_02_SPRINT_PLAN.md`.

## Manutencao

Antes de commit/push: `docs/SPRINT_HISTORY.md`, `.cursor/rules/pre-commit-docs.mdc`, `.cursor/rules/conventional-commits.mdc`.
