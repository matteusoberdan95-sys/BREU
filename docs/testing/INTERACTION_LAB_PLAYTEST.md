# Playtest — Interaction Lab

**Cena:** `res://scenes/test/InteractionLab.tscn`  
**Sprint:** 04 + hotfix  
**Data:** 2026-07-11

---

## Histórico de falha / correção

### Falha inicial (Sprint 04 v1)

| Problema | Causa |
|----------|-------|
| Prompt `[E]` não aparecia | `PlayerInteractionRaycast` resolvia `RaycastPath` relativo ao nó errado — raycast null |
| E não mostrava mensagem | Mesma causa + painel de mensagem sempre visível/vazio |
| Player atravessava objetos | Objetos só na layer 2; sem paredes; player mask = World (1) não colidia |
| Caixa HUD vazia | `MessagePanel` visível sem texto |

### Hotfix aplicado

- Raycast resolvido a partir do **Player** (pai de `PlayerInteractionRaycast`)
- Raycast `collision_mask = 3` (World + Interactable)
- Chão/paredes/porta/mesa/sign: layers corretas (1 ou 3)
- Livro: `Area3D` layer 2 para interação sem bloquear
- HUD: `MessagePanel` oculto até haver mensagem; `HideInteractionPrompt()` adicionado
- Lab: 4 paredes com colisão World

---

## Checklist — Movimentação preservada

| Teste | Esperado | OK |
|-------|----------|-----|
| W/S/A/D | Direções corretas | ☐ |
| Sprint (Shift) | Funciona | ☐ |
| Crouch (C/Ctrl) | Funciona | ☐ |
| Lean Q/R | Funciona | ☐ |
| Look back Alt/X | Funciona | ☐ |
| Camera feel | Sem regressão | ☐ |

---

## Checklist — Colisão

| Teste | Esperado | OK |
|-------|----------|-----|
| Chão | Player não cai | ☐ |
| Paredes (N/S/E/W) | Player não atravessa | ☐ |
| TestLockedDoor | Bloqueia passagem | ☐ |
| TestBookTable | Bloqueia passagem | ☐ |
| TestBook (pequeno) | Não trava player | ☐ |

---

## Checklist — Interação

| Teste | Esperado | OK |
|-------|----------|-----|
| Mirar TestSign | `[E] Ler placa` | ☐ |
| E na placa | Mensagem OFERTA DE TRABALHO... | ☐ |
| Mirar TestBook | `[E] Examinar livro` | ☐ |
| E no livro | Mensagem registro | ☐ |
| Mirar TestLockedDoor | `[E] Tentar abrir porta` | ☐ |
| E na porta | Está trancada. | ☐ |
| Sair da mira | Prompt some | ☐ |
| E sem mira | Nada quebra | ☐ |
| Sem caixa vazia | Painéis só com texto | ☐ |
| Mensagem | Some após ~3 s | ☐ |

---

## Checklist — HUD

| Teste | Esperado | OK |
|-------|----------|-----|
| Vida / Stamina / Lanterna | Visíveis | ☐ |
| F10 / F11 | Funcionam | ☐ |
| Prompt | Legível (centro) | ☐ |
| Mensagem | Legível (inferior) | ☐ |

---

## Debug console

```
Interaction target: TestSign | prompt: Ler placa
Interacted with: TestSign
No interactable target.
```

---

## Baseline

`docs/technical/INTERACTION_SYSTEM_BASELINE.md`

**Sprint 04 só aprovada após este checklist passar.**
