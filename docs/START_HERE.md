# BREU - Start Here

Este arquivo e o ponto de entrada para Codex, Cursor IDE, Cursor CLI ou qualquer nova sessao de trabalho.

## Leitura Obrigatoria

1. `docs/PROJECT_STATE.md`
2. `docs/HANDOFF.md`
3. `docs/gameplay/NEXT_SPRINT_TASKS.md`
4. `docs/technical/TDD.md`
5. `docs/design/GDD.md`
6. `docs/design/IMPLEMENTATION_PLAN.md`

Depois leia os docs de agentes em `docs/agents/` relacionados a tarefa atual.

## Contexto Rapido

- Projeto: BREU.
- Engine: Godot 4.7 Mono.
- Linguagem: C# / .NET 10.
- Cena principal: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Cenario atual: Quarto 07 - Pensao Santa Luzia.
- Asset principal: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.

## Estado Jogavel Atual

O jogador consegue:

- andar em primeira pessoa;
- olhar com mouse;
- correr;
- ligar/desligar lanterna;
- ver prompts de interacao no HUD;
- ler o bilhete;
- coletar o martelo;
- ver o martelo placeholder na mao;
- abrir a porta;
- sair para um corredor placeholder;
- chegar a um trigger de fim de demo.

Ainda nao existe:

- combate ativo;
- inimigo ativo na demo room;
- UI final de leitura de bilhete;
- animacao real de porta;
- corredor final modular;
- audio/ambiencia.

## Validacao Minima

Sempre que alterar C#:

```powershell
dotnet build BREU.sln
```

Quando possivel, tambem validar:

```powershell
& 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe' --headless --path . --quit
```

## Ao Finalizar Trabalho

Atualize:

- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md` se prioridades mudaram
- docs especificos da area alterada

Depois rode build e registre o resultado.
