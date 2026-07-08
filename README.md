# BREU

Survival horror em primeira pessoa feito em Godot 4.x .NET/C#.

O primeiro objetivo e uma vertical slice pequena: o jogador acorda no Quarto 07 da Pensao Santa Luzia, pega um martelo enferrujado, le um bilhete, sai para um corredor curto e enfrenta o primeiro `Enemy_Hospede`.

## Leia primeiro em qualquer ferramenta

Ao abrir este projeto no Codex, Cursor IDE ou Cursor CLI, leia estes arquivos nesta ordem:

1. `docs/PROJECT_STATE.md`
2. `docs/HANDOFF.md`
3. `docs/gameplay/NEXT_SPRINT_TASKS.md`
4. `docs/technical/TDD.md`
5. `docs/design/GDD.md`
6. `docs/design/IMPLEMENTATION_PLAN.md`

Depois leia apenas os docs de agentes relevantes ao trabalho atual em `docs/agents`.

## Estado atual

- Projeto Godot/.NET criado.
- Cena principal: `res://scenes/levels/demo_room/DemoRoom.tscn`.
- Build C# validado com `dotnet build BREU.sln`.
- Ainda falta validar a importacao visual no editor Godot, porque o executavel `godot` nao estava no PATH na primeira sessao.

## Comandos uteis

```powershell
dotnet build BREU.sln
```

Para jogar/testar, abrir esta pasta no Godot 4.x .NET e rodar `DemoRoom.tscn`.

## Regra de continuidade

Antes de encerrar qualquer sessao de trabalho, atualize:

- `docs/PROJECT_STATE.md`
- `docs/HANDOFF.md`
- `docs/gameplay/NEXT_SPRINT_TASKS.md`

Assim o projeto pode continuar em outro computador, no Codex, Cursor IDE ou Cursor CLI sem perder contexto.
