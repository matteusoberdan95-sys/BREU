# BREU - Handoff entre Codex e Cursor

Ultima atualizacao: 2026-07-09

## Como retomar

1. `docs/START_HERE.md`
2. `docs/PROJECT_STATE.md`
3. `docs/SPRINT_HISTORY.md`
4. `docs/gameplay/NEXT_SPRINT_TASKS.md`
5. `docs/technical/TDD.md`

## Ultimo commit pendente / recente

Sprints 3–5 registradas em `docs/SPRINT_HISTORY.md`:

- **Sprint 3:** corredor placeholder, martelo na mao, HUD lanterna.
- **Sprint 4:** `GlobalUsings.cs` por camadas.
- **Sprint 5:** atmosfera (porta som, HUD novo, susto, radio, inimigo placeholder).

## Estado atual

DemoRoom jogavel de ponta a ponta:

- Quarto 07 com interacoes completas.
- Corredor +Z sem limbo.
- Primeiro susto no meio do corredor (Z ~5.5).
- Silhueta inimigo no fim (Z ~8.2), some apos ~1.75s.
- Trigger de fim da demo no final.
- Audio: nos prontos, streams nulos nao quebram o jogo.

## Sistemas novos (Sprint 5)

| Sistema | Arquivo |
|---------|---------|
| Som da porta | `scripts/doors/DoorAudioController.cs` |
| HUD survival horror | `scripts/ui/HUDController.cs`, `scenes/ui/HUD.tscn` |
| Radio | `scripts/horror/RadioInterferenceController.cs` |
| Susto corredor | `scripts/horror/CorridorScareTrigger.cs` |
| Inimigo placeholder | `scripts/enemies/EnemyPlaceholder.cs` |
| Sequencia demo | `scripts/levels/DemoRoomSequenceController.cs` |

## Como testar

1. Abrir `DemoRoom.tscn` → F6 → foco em **Entrada**.
2. Coletar martelo / ler bilhete → mensagens HUD.
3. Abrir porta → console `DoorAudio: som de abrir nao configurado.` (sem .ogg).
4. Corredor +Z até Z ~5.5 → susto (luz, radio, silhueta).
5. Seguir até Z ~8.7 → fim da demo.

Detalhes: `docs/testing/PLAYTEST_DEMO_ROOM.md`

## Validacao

```powershell
dotnet build BREU.sln
```

2026-07-09: 0 erros, 0 avisos. Godot headless OK.

## Bugs/limitacoes conhecidas

- Sem arquivos `.ogg` ainda.
- `PlayerStamina` nao instanciado no Player — label fixa no HUD.
- Inimigo placeholder sem combate, dano ou navmesh.
- Porta sem animacao/pivo real.
- Bilhete sem UI dedicada.

## Proximo passo

Adicionar audio real, UI do bilhete, corredor Blender e substituir inimigo placeholder.

## Regra de continuidade

Antes de **todo commit/push**, atualizar `docs/SPRINT_HISTORY.md` + `PROJECT_STATE.md` + `HANDOFF.md` + `NEXT_SPRINT_TASKS.md`. Ver `.cursor/rules/pre-commit-docs.mdc`.
