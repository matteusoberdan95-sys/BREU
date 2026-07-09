# BREU - Handoff entre Codex, Cursor IDE e Cursor CLI

Ultima atualizacao: 2026-07-08

## Contexto para retomar

Este projeto foi iniciado como fundacao da vertical slice de BREU. A intencao e permitir continuidade entre maquinas e ferramentas sem depender da memoria da conversa.

Ao iniciar uma nova sessao, leia primeiro:

1. `README.md`
2. `docs/PROJECT_STATE.md`
3. `docs/gameplay/NEXT_SPRINT_TASKS.md`
4. `docs/technical/TDD.md`
5. O arquivo de agente em `docs/agents` relacionado ao trabalho.

## O que foi feito na ultima sessao

- Criado projeto Godot 4.x .NET/C#.
- Criada estrutura de pastas.
- Criadas cenas placeholder:
  - `Player.tscn`
  - `DemoRoom.tscn`
  - `Corridor.tscn`
  - `Enemy_Hospede.tscn`
  - `Door.tscn`
  - `HUD.tscn`
- Criados scripts iniciais:
  - `PlayerController.cs`
  - `PlayerLook.cs`
  - `PlayerStamina.cs`
  - `FlashlightController.cs`
  - `PlayerInteractor.cs`
  - `IInteractable.cs`
  - `PickupItem.cs`
  - `WeaponData.cs`
  - `WeaponController.cs`
  - `MeleeWeapon.cs`
  - `UnarmedWeapon.cs`
  - `Door.cs`
  - `EnemyAI.cs`
  - `EnemyHealth.cs`
  - `HUDController.cs`
- Criados resources de armas.
- Criada documentacao inicial e docs de agentes.
- Adicionadas instrucoes para Codex e Cursor:
  - `AGENTS.md`
  - `.cursorrules`
  - `.cursor/rules/breu-project.mdc`
- Integrado o blockout Blender do Quarto 07 em `DemoRoom.tscn` como asset importado.
- Criado `docs/blender_pipeline/QUARTO_07_IMPORT.md`.

## Onde continuar

Prioridade imediata:

1. Abrir no Godot 4.x .NET.
2. Ver se o GLB `assets/blender_exports/quarto_07/quarto_07_blockout.glb` importa corretamente.
3. Abrir `DemoRoom.tscn` e conferir a estrutura de importacao do Quarto 07.
4. Ajustar pontos auxiliares se a escala/posicao do Blender mudar.
5. Reinstanciar gameplay usando `PlayerStart`, `DoorPoint`, `HammerPickupPoint`, `NotePoint` e `RoomLightPoint`.
6. Ajustar pivo/colisao da porta.
7. Implementar vida do player e dano real do inimigo.

## Definicao de pronto da Sprint 1

- Player anda e olha ao redor.
- Mouse e capturado corretamente.
- Lanterna liga/desliga e bateria atualiza no HUD.
- Stamina consome ao correr e atacar.
- Prompt de interacao aparece.
- Bilhete pode ser lido/interagido.
- Martelo pode ser coletado/equipado.
- Porta abre.
- Corredor e acessivel.
- `Enemy_Hospede` persegue e pode receber dano.
- Martelo perde durabilidade e quebra.
- Soco funciona apos quebra do martelo.
- Build C# passa sem erro.

## Checklist antes de encerrar qualquer sessao

- [ ] Rodar `dotnet build BREU.sln` se C# mudou.
- [ ] Atualizar `docs/PROJECT_STATE.md`.
- [ ] Atualizar este `docs/HANDOFF.md`.
- [ ] Atualizar `docs/gameplay/NEXT_SPRINT_TASKS.md` se prioridades mudaram.
- [ ] Registrar bugs conhecidos abaixo.

## Bugs conhecidos

- Validacao visual no Godot ainda nao foi feita nesta maquina via linha de comando.
- O arquivo GLB estava inicialmente em `assets/blender_exports/quarto_07/quarto-07/quarto_07_blockout.glb`; foi criada copia canonica em `assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
- Ataque do inimigo ainda nao aplica dano real no player.
- Porta ainda usa rotacao simples e precisa de revisao de pivo/colisao no editor.
- UI de bilhete ainda e debug log, nao tela dedicada.

## Notas para outro computador

- Abrir a pasta inteira `BREU`, nao apenas `scenes`.
- Usar Godot 4.x .NET.
- Se o SDK do Godot reclamar da versao, verificar `BREU.csproj`.
- Se a maquina nao tiver .NET 10, instalar ou ajustar de forma coordenada no TDD.
