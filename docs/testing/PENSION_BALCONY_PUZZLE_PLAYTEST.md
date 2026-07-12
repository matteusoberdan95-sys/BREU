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

## Hotfix 17B — Acesso único + varanda real

**Problemas:**
- Porta marrom `Door_UpperBlocked` duplicava o vão e pedia outra chave.
- `Wall_Second_Front` sólida fechava a caixa atrás da porta verde (sem varanda).

**Correções:**
- Removida porta marrom / prompt *Tentar abrir porta* do vão da varanda.
- Porta verde única, largura total do vão.
- Gap em `Wall_Second_Front` + piso externo até `BalconyOuterZ` + guarda-corpos.
- Corredor curto `UpperBalconyWing_Corridor` com entrada limpa; stub trancado no fim.
- Trigger presença movido para Z≈−16,5 (longe da porta).

| Check | Resultado |
|-------|-----------|
| Uma só porta no vão | ☐ F6 |
| Destravar → varanda externa | ☐ F6 |
| Guarda-corpo / sem limbo | ☐ F6 |
| Prompt presença longe da porta | ☐ F6 |

---

## Sprint 17C — Ala da varanda + puzzle macabro

**Status:** 🔄 Implementada — aguardando F6  
**Docs:** `docs/design/PENSION_BALCONY_WING_PUZZLE.md`

### Rota completa

| Check | Resultado |
|-------|-----------|
| Nascer na trilha | ☐ |
| Entrar na pensão | ☐ |
| Fluxo chave → depósito → fusível | ☐ |
| Subir segundo andar | ☐ |
| Abrir porta verde com chave da varanda | ☐ |
| Acessar varanda + guarda-corpo / sem queda | ☐ |
| Voltar da varanda | ☐ |
| Pegar arame torto | ☐ |
| Entrar no banheiro | ☐ |
| Examinar espelho | ☐ |
| Ralo sem arame = mensagem correta | ☐ |
| Arame no ralo → chave | ☐ |
| Abrir quarto da proprietária | ☐ |
| Examinar caderno → evento macabro | ☐ |
| Mensagem Quarto 203 | ☐ |
| Quarto 203 bloqueado | ☐ |
| Sair e voltar OK | ☐ |

### Checklist técnico

| Check | Resultado |
|-------|-----------|
| Player / stamina / HUD / lanterna / F10–F11 | ☐ |
| Áudio / passos / respiração / fog | ☐ |
| Puzzle antigo + porta verde | ☐ |
| Dois cômodos acessíveis / sem limbo / sem z-fight novo | ☐ |
| Sem porta tremendo / sem inimigo / combate / chase | ☐ |

### Checklist emocional

| Check | Resultado |
|-------|-----------|
| Varanda = descoberta | ☐ |
| Banheiro = abandono / ralo desconfortável | ☐ |
| Quarto + caderno = impacto | ☐ |
| 203 prepara medo | ☐ |

