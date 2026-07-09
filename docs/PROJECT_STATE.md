# BREU - Estado do projeto

Ultima atualizacao: 2026-07-08

## Resumo rapido

BREU esta na fundacao da vertical slice. O projeto Godot 4.x .NET/C# foi criado com cenas placeholder, scripts iniciais, resources de armas e documentacao de arquitetura/agentes.

## Onde estamos

Sprint atual: Sprint 1 - Fechamento da base jogavel.

Status:

- Estrutura de pastas criada.
- Projeto Godot/.NET criado.
- Cena principal criada: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Player FPS criado com movimento, camera, stamina, lanterna e interacao.
- HUD minimo criado.
- Sistema de arma improvisada criado com durabilidade.
- Resources criados: soco, martelo enferrujado, faca velha e tabua rachada.
- Porta interativa criada.
- Bilhete e martelo coletavel criados no Quarto 07.
- Corredor placeholder criado.
- `Enemy_Hospede` criado com perseguicao simples, ataque por debug, dano, stun e morte.
- Docs de agentes, GDD, TDD, pipeline Blender e backlog criados.
- Blockout Blender do Quarto 07 integrado como asset importado em `DemoRoom.tscn`.
- Documento de importacao criado em `docs/blender_pipeline/QUARTO_07_IMPORT.md`.

## Ultima verificacao tecnica

Comando rodado:

```powershell
dotnet build BREU.sln
```

Resultado:

- Build com sucesso.
- 0 erros.
- 0 avisos.

## Ponto de atencao

Na primeira sessao, `godot`, `godot4` e `Godot_v4*` nao foram encontrados no PATH. Por isso, a validacao visual/importacao no editor ainda nao foi feita por linha de comando.

## Proximo passo recomendado

1. Abrir a pasta `BREU` no Godot 4.x .NET.
2. Deixar o editor importar `assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
3. Abrir `DemoRoom.tscn` e conferir alinhamento de `PlayerStart`, `DoorPoint`, `HammerPickupPoint`, `NotePoint` e `RoomLightPoint`.
4. Instanciar player/HUD/gameplay usando os pontos auxiliares da cena.
5. Registrar problemas encontrados em `docs/HANDOFF.md`.

## Arquivos principais

- `project.godot`
- `BREU.csproj`
- `scenes/levels/demo_room/DemoRoom.tscn`
- `scenes/player/Player.tscn`
- `scenes/enemies/Enemy_Hospede.tscn`
- `scenes/levels/corridor/Corridor.tscn`
- `scenes/props/Door.tscn`
- `scenes/ui/HUD.tscn`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`
- `docs/technical/TDD.md`
- `docs/design/GDD.md`

## Como manter este arquivo

Atualize este arquivo sempre que:

- Uma sprint mudar de status.
- Uma mecanica for concluida.
- Um bug importante for descoberto.
- O projeto for aberto/testado em outro computador.
- O Godot importar ou falhar ao importar o projeto.
