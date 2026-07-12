# Playtest — Puzzle da Varanda (Sprint 17)

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-12  
**Status:** 🔄 Implementado — aguardando F6

## Fluxo

1. Trilha → Pensão → chave depósito → fusível  
2. Mensagem pós-fusível sobre varanda  
3. 2º andar → porta verde bloqueada  
4. Quarto 201 → ler anotação  
5. Recepção → pegar chave da varanda  
6. Destravar porta verde → varanda → ala leste  
7. Quarto 203 + caderno da proprietária → voltar

## Itens / mensagens

| Item | Prompt | Mensagem |
|------|--------|----------|
| Nota 201 | Ler anotação | Chave perto da recepção |
| Chave (sem nota) | Examinar | Nada chama atenção |
| Chave (com nota) | Pegar chave da varanda | Chave enferrujada |
| Porta (sem chave) | Tentar abrir varanda | Emperrada / chave certa |
| Porta (com chave) | Destravar varanda | A porta cede |

## Locais

- Nota: `Interact_Note_OwnerBalcony` (201)  
- Chave: `Interact_BalconyKey` (recepção)  
- Porta: `Door_UpperBalcony`  
- Ala: `UpperBalconyWing` / `Room_203` / `Room_OwnerOffice`

## Checklist navegação

| Check | Resultado |
|-------|-----------|
| Porta verde bloqueia antes da chave | ☐ |
| Porta libera sem flicker/tremor | ☐ |
| Varanda sem queda/limbo | ☐ |
| Ala / 203 / office navegáveis | ☐ |
| Volta à Pensão OK | ☐ |

## Regressão

| Check | Resultado |
|-------|-----------|
| Movimento / HUD / áudio / fog | ☐ |
| Puzzle depósito/fusível | ☐ |
| Escada / 2º andar | ☐ |
| Sem inimigo/combat/chase | ☐ |

## Bugs encontrados

_Pendente F6._
