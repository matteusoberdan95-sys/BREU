# BREU

Survival horror em primeira pessoa feito em Godot 4.7 Mono/C#.

O objetivo atual e construir uma vertical slice pequena e jogavel: o jogador acorda no **Quarto 07 - Pensao Santa Luzia**, pega um martelo enferrujado, le um bilhete, abre a porta e sai para um corredor curto.

## Comece Aqui

Ao abrir este projeto no Codex, Cursor IDE, Cursor CLI ou qualquer editor, leia:

1. `docs/START_HERE.md`
2. `docs/PROJECT_STATE.md`
3. `docs/HANDOFF.md`
4. `docs/gameplay/NEXT_SPRINT_TASKS.md`
5. `docs/technical/TDD.md`
6. `docs/design/GDD.md`
7. `docs/design/IMPLEMENTATION_PLAN.md`

Depois leia apenas os arquivos relevantes em `docs/agents/`.

## Estado Atual

- Cena principal: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Cenario importado: `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`.
- Player FPS funcional com WASD, mouse look, sprint e lanterna.
- HUD minimo com prompt de interacao e arma equipada.
- Bilhete interativo com feedback no console.
- Martelo coletavel, registrado no inventario simples e visivel na mao como placeholder.
- Porta interativa em modo debug.
- Corredor placeholder conectado depois da porta.
- Trigger de fim de demo no final do corredor.
- Sem combate e sem inimigo ativos nesta etapa.

## Como Testar

1. Abrir a pasta `BREU` no Godot 4.7 Mono.
2. Abrir `res://scenes/levels/demo_room/DemoRoom.tscn`.
3. Rodar a cena com F6.
4. Clicar na aba/janela **Entrada** para dar foco.
5. Testar:
   - WASD para andar;
   - mouse para olhar;
   - Shift para correr;
   - F para lanterna;
   - E para interagir com bilhete, martelo e porta.

## Comandos Uteis

```powershell
dotnet build BREU.sln
```

Godot local usado nesta maquina:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe
```

Validacao headless:

```powershell
& 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64_console.exe' --headless --path . --quit
```

## Regras de Continuidade

Antes de encerrar uma sessao:

- atualize `docs/PROJECT_STATE.md`;
- atualize `docs/HANDOFF.md`;
- atualize `docs/gameplay/NEXT_SPRINT_TASKS.md` se prioridades mudarem;
- rode `dotnet build BREU.sln` se C# mudou;
- registre qualquer problema novo em `docs/HANDOFF.md`.

## Proximo Marco Recomendado

Trocar o corredor placeholder por uma cena modular definitiva e criar uma porta final/transicao. Depois disso, iniciar a camada de feedback visual/sonoro antes de combate.
