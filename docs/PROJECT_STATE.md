# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

BREU esta na vertical slice jogavel do Quarto 07 com corredor placeholder, primeira camada de atmosfera/terror e HUD survival horror. O jogador percorre quarto → corredor → susto → fim da demo.

Historico completo: `docs/SPRINT_HISTORY.md`

## Stack

- Engine: Godot 4.7 Mono.
- Linguagem: C# / .NET 10.
- Cena principal: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Asset principal: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
- Usings globais: `GlobalUsings.cs` (camadas Engine → BREU).

## Estado jogavel atual

### Quarto 07

- Player FPS: WASD, mouse look, sprint, lanterna (bateria 100/100, dreno so ligada).
- HUD com painel escuro, prompt `[E]`, mensagens temporarias.
- Bilhete, martelo, porta interativos com feedback HUD + console.
- Martelo placeholder na mao (`WeaponHolder`).
- Porta abre com som preparado (`DoorAudioController` — streams ainda vazios).
- `DemoRoomSequenceController` rastreia progresso da demo.

### Corredor

- Placeholder modular (+Z) com materiais, colisoes e luz.
- `CorridorScareTrigger`: flicker, radio, silhueta inimigo (1x).
- `EnemyPlaceholder` no fim do corredor (sem ataque/dano).
- `CorridorEndTrigger` no final.

## Fora de escopo agora

- Combate completo e dano ao player.
- IA avancada / navmesh do inimigo.
- Modelo final do inimigo no Blender.
- UI dedicada de leitura do bilhete.
- Arquivos de audio reais (nos preparados).
- Corredor modular definitivo e porta final/transicao.

## Verificacao tecnica

```powershell
dotnet build BREU.sln
```

Resultado (2026-07-09): Build com sucesso. 0 erros. 0 avisos.

Godot headless: cena carrega sem erro.

## Proximo passo recomendado

1. Adicionar `.ogg` de porta, radio e susto.
2. UI de leitura do bilhete.
3. Corredor Blender + porta final/transicao.
4. Inimigo Blender + perseguicao simples.

## Arquivos principais

- `docs/SPRINT_HISTORY.md`
- `docs/HANDOFF.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `GlobalUsings.cs`
- `scenes/levels/demo_room/DemoRoom.tscn`
- `scenes/enemies/EnemyPlaceholder.tscn`
- `scripts/doors/DoorAudioController.cs`
- `scripts/horror/CorridorScareTrigger.cs`
- `scripts/horror/RadioInterferenceController.cs`
- `scripts/levels/DemoRoomSequenceController.cs`

## Como manter este arquivo

Atualize antes de todo commit/push (regra em `.cursor/rules/pre-commit-docs.mdc`).
