# Playtest — Interaction Lab

**Cena:** `res://scenes/test/InteractionLab.tscn`  
**Sprint:** 04  
**Data:** 2026-07-11

---

## Pré-requisitos

- Godot 4.7 + .NET
- `dotnet build` OK
- Branch `reboot/breu-clean-start`

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

## Checklist — Interação

| Teste | Esperado | OK |
|-------|----------|-----|
| Mirar TestSign | `[E] Ler placa` | ☐ |
| E na placa | Mensagem OFERTA DE TRABALHO... | ☐ |
| Mirar TestBook | `[E] Examinar livro` | ☐ |
| E no livro | Mensagem registro | ☐ |
| Mirar TestLockedDoor | `[E] Tentar abrir porta` | ☐ |
| E na porta | Mensagem Está trancada. | ☐ |
| Sair da mira | Prompt some | ☐ |
| Mensagem | Some após ~3 s | ☐ |
| E sem mira | Nada quebra | ☐ |
| Raycast | Não trava player | ☐ |
| Objetos | Não empurram player | ☐ |

---

## Checklist — HUD

| Teste | Esperado | OK |
|-------|----------|-----|
| Vida / Stamina / Lanterna | Visíveis | ☐ |
| Prompt interação | Legível, centro inferior | ☐ |
| Mensagem interação | Legível, centro inferior | ☐ |
| F10 / F11 debug | Funcionam | ☐ |

---

## Debug console

Com `EnableInteractionDebug` no lab:

```
Looking at interactable: Interactable  (ou nome do host)
```

Log só ao **trocar** alvo — sem spam por frame.

---

## Baseline

`docs/technical/INTERACTION_SYSTEM_BASELINE.md`
