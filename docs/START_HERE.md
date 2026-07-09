# BREU - Start Here

Este arquivo e o ponto de entrada para Codex, Cursor IDE, Cursor CLI ou qualquer nova sessao de trabalho.

## Leitura Obrigatoria

1. `docs/PROJECT_STATE.md`
2. `docs/HANDOFF.md`
3. `docs/design/GAME_VISION.md`
4. `docs/gameplay/NEXT_SPRINT_TASKS.md`
5. `docs/technical/TDD.md`
6. `docs/design/GDD.md`

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

- andar, correr (stamina), agachar (`Ctrl`) e pular (`Space`);
- ouvir passos por superficie (concreto / madeira);
- usar lanterna com HUD de bateria;
- ler bilhete na `NoteReaderUI`;
- coletar martelo e abrir porta do quarto;
- percorrer corredor, disparar susto (Z ~5.5);
- interagir com porta final: trancada ate susto, depois `Entrar` → fade → `RitualRoom.tscn`.

Fluxo: **Quarto 07 → corredor → susto → porta final → Fase 2 placeholder**.

Ainda nao existe:

- combate funcional completo;
- IA de perseguicao;
- trilha de entrada e fachada da casa;
- Sala dos Santos Secos modelada (so placeholder);
- inimigo Blender final.

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
