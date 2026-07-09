# BREU - Proximas tarefas

## Como usar este arquivo

Este e o backlog vivo do projeto. Ao abrir o projeto no Codex, Cursor IDE ou Cursor CLI, leia primeiro:

1. `docs/START_HERE.md`
2. `docs/PROJECT_STATE.md`
3. `docs/HANDOFF.md`

Ao terminar uma tarefa, atualize `docs/PROJECT_STATE.md` e registre o handoff em `docs/HANDOFF.md`.

## Estado atual da vertical slice

O Quarto 07 ja esta jogavel em primeira pessoa:

- Player anda, olha, corre e usa lanterna.
- HUD minimo mostra prompts de interacao.
- Bilhete e martelo funcionam.
- Martelo aparece na mao como placeholder apos coleta.
- Porta abre em modo debug.
- Corredor placeholder esta conectado.
- Trigger de fim de demo existe no fim do corredor.
- Combate e inimigo ainda nao devem entrar sem pedido explicito.

## Sprint 1 - Fechamento da base jogavel

Prioridade imediata:

- Ajustar visualmente no editor as areas `HammerPickup`, `NoteInteractable` e `DoorInteractable`.
- Ajustar colisao dos moveis se o player prender ou atravessar algo.
- Trocar o corredor placeholder por uma cena modular definitiva.
- Criar porta final/transicao no fim do corredor.
- Criar UI de leitura do bilhete em vez de apenas console.
- Criar feedback minimo de interacao: som curto ou destaque simples.

## Sprint 2 - Quarto demo

- Substituir placeholders principais por blocos mais fieis feitos no Blender.
- Criar material inicial de parede mofada.
- Criar radio placeholder com som chiado.
- Melhorar layout do quarto para leitura visual.
- Revisar luz amarela fraca e sombras.

## Sprint 3 - Inventario e arma sem combate

- Evoluir `PlayerInventory` para itens simples.
- Trocar martelo placeholder na mao por asset final/animado.
- Preparar durabilidade visual do martelo no HUD.
- Ainda nao ativar dano/combate sem tarefa explicita.

## Sprint 4 - Combate

- Trocar RayCast de ataque por ShapeCast/Area temporizada.
- Criar ataque do martelo com custo de stamina.
- Adicionar feedback sonoro e visual para impacto.
- Balancear stamina, dano e durabilidade.
- Adicionar fallback para soco quando o martelo quebrar.

## Sprint 5 - Inimigo

- Criar vida do player e feedback de dano.
- Melhorar percepcao do inimigo por visao/som.
- Adicionar patrulha basica.
- Adicionar animacoes placeholder.
- Criar estados de investigacao e perda de alvo.

## Sprint 6 - Atmosfera

- Ambiencia: vento, radio, madeira rangendo, passos no corredor.
- Decals: sangue seco, mofo, marcas de unha, sujeira.
- Props finais do Quarto 07.
- Checklist de QA e performance.
