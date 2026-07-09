# BREU - Start Here

Este arquivo e o ponto de entrada para Codex, Cursor IDE, Cursor CLI ou qualquer nova sessao de trabalho.

## Leitura obrigatoria

1. `docs/PROJECT_STATE.md`
2. `docs/HANDOFF.md`
3. `docs/SPRINT_HISTORY.md`
4. `docs/design/GAME_VISION.md`
5. `docs/gameplay/NEXT_SPRINT_TASKS.md`
6. `docs/technical/TDD.md`
7. `docs/design/GDD.md`

Depois leia os docs de agentes em `docs/agents/` relacionados a tarefa atual.

## Contexto rapido

- Projeto: BREU.
- Engine: Godot 4.7 Mono.
- Linguagem: C# / .NET 10.
- Cena principal atual: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Cena de teste da entrada: `res://scenes/levels/trail_intro/TrailIntro.tscn`.
- Cenario atual: Trilha Noturna, Quarto 07, corredor e `RitualRoom` placeholder.
- Asset do Quarto 07: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
- Asset da Trilha Noturna: `res://assets/blender_exports/trail_intro/trail_intro_blockout.glb`.

## Estado jogavel atual

O jogador consegue:

- testar a Trilha Noturna antes da Pensao, com player, colisoes, vento, luz da casa e trigger de chegada;
- andar, correr com stamina, agachar (`Ctrl`) e pular (`Space`);
- ouvir passos por superficie;
- usar lanterna com HUD de bateria;
- ler bilhete na `NoteReaderUI`;
- coletar martelo e abrir porta do quarto;
- percorrer corredor, disparar susto e interagir com a porta final;
- fazer fade para `RitualRoom.tscn`.

Fluxo atual da demo principal:

```text
Quarto 07 -> corredor -> susto -> porta final -> RitualRoom
```

Fluxo atual da trilha:

```text
TrailIntro -> chegada na casa (mensagem HUD, sem transicao ainda)
```

Ainda nao existe:

- combate funcional completo;
- IA de perseguicao avancada;
- fachada da casa;
- Sala dos Santos Secos modelada;
- inimigo Blender final.

## Validacao minima

Sempre que alterar C#:

```powershell
dotnet build BREU.sln
```

Quando possivel, tambem validar:

```powershell
& 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe' --headless --path . --quit
```

## Ao finalizar trabalho

Atualize:

- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/SPRINT_HISTORY.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md` se prioridades mudaram
- docs especificos da area alterada

Depois rode build e registre o resultado.
