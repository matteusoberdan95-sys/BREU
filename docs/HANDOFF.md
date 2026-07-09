# BREU - Handoff

Ultima atualizacao: 2026-07-09

## Retomar

1. `docs/START_HERE.md` → `docs/PROJECT_STATE.md` → `docs/SPRINT_HISTORY.md`
2. Visao do jogo: `docs/design/GAME_VISION.md` → `docs/production/PHASE_01_02_SPRINT_PLAN.md`
3. Tarefas: `docs/gameplay/NEXT_SPRINT_TASKS.md`

## Ultimas entregas

### Gameplay

- **Passos por superficie** — `PlayerGroundSurfaceDetector`, `SurfaceTag`, madeira no corredor (`WoodTestFloor`).
- **Stamina** — sprint drena; pulo custa 12; HUD sincroniza ao conectar.
- **Agachamento** — `Ctrl` segurar; movimento lento; sem sprint/pulo.
- **Porta final** — `CorridorEndDoorInteractable`; trancada ate susto; transicao com fade.
- **Fase 2 placeholder** — `scenes/levels/phase_02/RitualRoom.tscn`.

### Documentacao

- Visao narrativa, Fases 1 e 2, direcao de arte, inimigos, pilares, plano de producao em `docs/design/` e `docs/production/`.

## Fluxo da demo (Fase 1)

```
Quarto 07 → porta do quarto → corredor → susto (Z~5.5) → porta final (Z~9.1) → RitualRoom
```

## Testar rapido

1. F6 em `DemoRoom.tscn`
2. Bilhete (`E`), martelo, porta do quarto
3. Corredor + susto; porta final: trancada antes / `Entrar` depois
4. Fade para `RitualRoom.tscn`

Guia completo: `docs/testing/PLAYTEST_DEMO_ROOM.md`

## Proximo

- Sprint B: trilha noturna antes do quarto
- Sprint D: Sala dos Santos Secos (Blender + Godot)
- Sprint E: IA placeholder (perseguicao, stun)
