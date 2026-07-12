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

## Hotfix 17A — Porta verde no teto

**Diagnóstico:** `Door_UpperBalcony` e `Door_UpperBlocked` somavam `SecondFloorTopY` no root **e** de novo no `panelCenterY` / Area3D → painel e prompt ~Y 6,6 (teto).

**Correção:**
- Porta recreada com root no piso do 2º andar; painel/Area em Y local (padrão depósito).
- Area3D na altura do peito (~1,45 m local), à frente da porta (corredor).
- `Marker_UpperBalconyDoor_Position` no vão.
- Porta cinza da direita também corrigida (mesmo bug de Y).
- Prompt: *Tentar abrir varanda* olhando reto; não no teto.

| Check | Resultado |
|-------|-----------|
| Porta verde visível na altura certa | ☐ F6 |
| Prompt olhando reto | ☐ F6 |
| Prompt não no teto | ☐ F6 |
| Destravar → passagem | ☐ F6 |
