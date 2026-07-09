# BREU - Estado do projeto

Ultima atualizacao: 2026-07-09

## Resumo rapido

BREU esta na base jogavel da vertical slice. A cena principal `DemoRoom.tscn` ja permite andar pelo Quarto 07, interagir com bilhete/martelo/porta, sair para um corredor placeholder e chegar a um trigger de fim de demo.

## Stack

- Engine: Godot 4.7 Mono.
- Linguagem: C#.
- Runtime alvo: .NET 10.
- Cena principal: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Asset principal: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.

## Estado jogavel atual

- Player FPS com movimento WASD, mouse look, sprint e lanterna.
- HUD minimo com prompt `[E] <acao>` e arma equipada.
- Bilhete interativo com texto no console.
- Martelo coletavel.
- Inventario simples registra martelo, nome equipado e durabilidade.
- Martelo placeholder aparece na mao/camera apos coleta.
- Porta do Quarto 07 abre em modo debug.
- Abertura da porta leva a um corredor placeholder.
- Trigger de fim de demo no fim do corredor.
- Colisoes temporarias para quarto, moveis, porta e corredor.

## Fora de escopo agora

- Combate ativo.
- Inimigo ativo dentro do fluxo atual.
- UI final de leitura do bilhete.
- Animacao/pivo/som real da porta.
- Corredor modular definitivo.
- Audio/ambiencia.

## Verificacao tecnica

Ultimos comandos validados:

```powershell
dotnet build BREU.sln
```

Resultado:

- Build com sucesso.
- 0 erros.
- 0 avisos.

Godot headless validado com:

```powershell
& 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe' --headless --path . --quit
```

Resultado:

- Cena carregou sem erro.

## Observacoes importantes

- Ao rodar no editor, clicar na aba/janela **Entrada** para dar foco ao input.
- O HUD deve ignorar mouse; `PlayerLook` usa `_Input` para nao perder mouse look.
- O Quarto 07 usa apenas a pasta `assets/blender_exports/quarto_07`.
- O GLB importado nao deve ser editado para gameplay; usar nos auxiliares no Godot.

## Proximo passo recomendado

1. Abrir `DemoRoom.tscn` no Godot 4.7 Mono.
2. Validar visualmente prompts do HUD, martelo na mao, porta e corredor.
3. Ajustar areas/colisoes auxiliares se necessario.
4. Trocar corredor placeholder por uma cena modular definitiva.
5. Criar porta final/transicao no fim do corredor.
6. Criar UI de leitura do bilhete.

## Arquivos principais

- `README.md`
- `AGENTS.md`
- `.cursorrules`
- `.cursor/rules/breu-project.mdc`
- `docs/START_HERE.md`
- `docs/HANDOFF.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`
- `docs/technical/TDD.md`
- `docs/testing/PLAYTEST_DEMO_ROOM.md`
- `project.godot`
- `BREU.csproj`
- `scenes/levels/demo_room/DemoRoom.tscn`
- `scenes/player/Player.tscn`
- `scenes/ui/HUD.tscn`

## Como manter este arquivo

Atualize sempre que:

- Uma sprint mudar de estado.
- Uma mecanica for concluida.
- Um bug importante for descoberto.
- O projeto for testado em outra maquina/ferramenta.
- O Godot importar ou falhar ao importar o projeto.
