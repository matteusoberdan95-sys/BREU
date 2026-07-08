# BREU - Instrucoes para agentes Codex

Este projeto deve ser tratado como um jogo indie real em Godot 4.x .NET/C#.

## Ordem obrigatoria de leitura ao iniciar

1. `docs/PROJECT_STATE.md`
2. `docs/HANDOFF.md`
3. `docs/gameplay/NEXT_SPRINT_TASKS.md`
4. `docs/technical/TDD.md`
5. `docs/design/GDD.md`
6. `docs/design/IMPLEMENTATION_PLAN.md`

Depois leia os arquivos em `docs/agents` que correspondem a area da tarefa.

## Direcao atual

- Nome publico do jogo: BREU.
- Cenario inicial: Quarto 07 - Pensao Santa Luzia.
- Engine: Godot 4.x .NET.
- Linguagem: C#.
- Assets finais serao feitos no Blender e exportados como `.glb`.
- Nao gerar assets 3D finais agora; usar placeholders editaveis.

## Regras de implementacao

- Manter scripts pequenos e por responsabilidade.
- Preferir Resources para dados editaveis, como armas e inimigos.
- Nao misturar gameplay, UI, IA e narrativa no mesmo script.
- Antes de alterar arquitetura, atualizar `docs/technical/TDD.md`.
- Antes de trocar fluxo de sprint, atualizar `docs/gameplay/NEXT_SPRINT_TASKS.md`.
- Antes de encerrar, atualizar `docs/HANDOFF.md` com o que foi feito e o proximo passo.

## Verificacao minima

Sempre que alterar C#, rode:

```powershell
dotnet build BREU.sln
```

Quando o Godot estiver disponivel, validar tambem abrindo `DemoRoom.tscn` no editor.
