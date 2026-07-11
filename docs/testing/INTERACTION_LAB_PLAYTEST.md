# Playtest — Interaction Lab

**Cena:** `res://scenes/test/InteractionLab.tscn`  
**Sprint:** 04 — **APROVADA**  
**Data aprovação:** 2026-07-11  
**Baseline:** `docs/technical/INTERACTION_SYSTEM_BASELINE.md`

---

## Status

**InteractionLab aprovado pelo usuário.**  
Sistema de interação congelado — não alterar sem nova sprint de interação ou solicitação explícita.

---

## Histórico de correção (hotfix)

| Problema inicial | Correção |
|------------------|----------|
| Prompt não aparecia | Raycast resolvido a partir do Player |
| E sem mensagem | Raycast + HUD MessagePanel visibility |
| Player atravessava objetos | Layers 1/3 + paredes no lab |
| Caixa HUD vazia | MessagePanel oculto por default |

**Commit hotfix:** `e11147f`

---

## Checklist aprovado — Movimentação preservada

- [x] W/S/A/D corretos
- [x] Sprint funciona
- [x] Crouch funciona
- [x] Lean Q/R funciona
- [x] Look back funciona
- [x] Camera feel sem regressão

---

## Checklist aprovado — Colisões básicas

- [x] Chão bloqueia player
- [x] Paredes bloqueiam player
- [x] TestLockedDoor bloqueia passagem
- [x] TestBookTable bloqueia passagem
- [x] TestBook não trava player

---

## Checklist aprovado — Interação

- [x] Prompts aparecem ao mirar objetos
- [x] `[E] Ler placa` — TestSign
- [x] `[E] Examinar livro` — TestBook
- [x] `[E] Tentar abrir porta` — TestLockedDoor
- [x] E interage e mostra mensagem correta
- [x] Placa, livro e porta trancada funcionam
- [x] Sair da mira esconde prompt
- [x] E sem mira não quebra
- [x] Mensagem some após ~3 s
- [x] Sem caixa vazia no HUD

---

## Checklist aprovado — HUD

- [x] Vida / Stamina / Lanterna visíveis
- [x] Debug F10/F11 funciona
- [x] Prompt legível
- [x] Mensagem legível

---

## Próximo passo

Sprint 05 — Pensão térreo blockout 01 (reutilizar interactables para placeholders).
