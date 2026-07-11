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
8. `docs/visual/VISUAL_DIRECTION.md`

Depois leia os docs de agentes em `docs/agents/` relacionados a tarefa atual.

## Contexto rapido

- Projeto: BREU.
- Engine: Godot 4.7 Mono.
- Linguagem: C# / .NET 10.
- Cena principal atual: `res://scenes/levels/trail_intro/TrailIntro.tscn`.
- Cenas de teste do inicio: `TrailIntro.tscn` e `HouseExterior.tscn` isolada.
- Cenario atual: Trilha Noturna, Fachada da Pensao, Quarto 07, corredor e Sala dos Santos Secos.
- Asset da trilha: `res://assets/blender_exports/trail_intro/trail_intro_blockout.glb`.
- Asset da fachada: `res://assets/blender_exports/house_exterior/pensao_santa_luzia_exterior_blockout.glb`.
- Asset do Quarto 07: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
- Asset da Sala dos Santos Secos: `res://assets/blender_exports/ritual_room/sala_santos_secos_blockout.glb`.
- Direcao visual: `res://docs/visual/VISUAL_DIRECTION.md`.
- Pipeline grafico: `res://docs/visual/GRAPHICS_PIPELINE.md`.

## Estado jogavel atual

O jogador consegue:

- iniciar na Trilha Noturna;
- chegar na fachada real da Pensao Santa Luzia integrada ao fim da trilha;
- entrar direto no Quarto 07 pela porta da Pensao;
- andar, correr com stamina, agachar (`Ctrl`) e pular (`Space`);
- ouvir passos por superficie;
- usar lanterna com HUD de bateria;
- ler bilhete na `NoteReaderUI`;
- coletar martelo e abrir porta do quarto;
- percorrer corredor, disparar susto e interagir com a porta final;
- fazer fade para `RitualRoom.tscn`;
- ler o bilhete ritual, pegar a Chave Velha e disparar o susto da Sala dos Santos Secos.

Fluxo atual:

```text
TrailIntro -> DemoRoom -> corredor -> RitualRoom
```

Ainda nao existe:

- combate funcional completo;
- IA de perseguicao avancada;
- porta visual/animada na fachada;
- Sala dos Santos Secos polida/final;
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
