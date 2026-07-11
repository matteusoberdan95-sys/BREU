# Playtest — Player Movement Lab

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Sprint:** 02.1  
**Data:** 2026-07-11

---

## Sprint 02.1 — correções

### W/S invertido — causa

`Input.GetVector(neg_x, pos_x, neg_y, pos_y)` trata o 3º parâmetro como **eixo negativo Y**.

Chamada antiga:
```csharp
GetVector("move_left", "move_right", "move_forward", "move_backward")
```
→ W gerava `input.Y = -1`, S gerava `input.Y = +1` — invertido para movimento.

**Fix:**
```csharp
GetVector("move_left", "move_right", "move_backward", "move_forward")
```
+ direção horizontal com `-Basis.Z` (forward) e `Basis.X` (right), Y zerado.

### Novos sistemas

| Sistema | Controle | Script |
|---------|----------|--------|
| Lean lateral | Q / R | `PlayerLook.cs` |
| Look back | Shift + W + Alt | `PlayerLook.cs` |
| Head bob / sway | automático | `PlayerCameraEffects.cs` |
| Reset debug | F9 | `PlayerMovementLabController.cs` |

---

## Controles

| Tecla | Ação |
|-------|------|
| WASD | Mover |
| Mouse | Olhar |
| Shift | Sprint |
| C / Ctrl | Agachar |
| Q | Lean esquerda |
| R | Lean direita |
| Alt (correndo + W) | Olhar para trás |
| F | Lanterna |
| F9 | Reset spawn |
| Esc | Liberar mouse |

---

## Checklist — Movimento

- [x] W/S corrigido (ordem GetVector)
- [ ] W anda para frente (manual)
- [ ] S anda para trás (manual)
- [ ] A/D laterais corretos (manual)
- [ ] Shift corre (manual)
- [ ] Movimento segue yaw do player (manual)
- [ ] Sem tremor / preso (manual)

## Checklist — Lean Q/R

- [ ] Q inclina esquerda (manual)
- [ ] R inclina direita (manual)
- [ ] Soltar volta suave (manual)
- [ ] Parede lean test (x=2.8) limita offset (manual)

## Checklist — Look back Alt

- [ ] Shift + W + Alt olha para trás (manual)
- [ ] Corpo continua para frente (manual)
- [ ] Soltar Alt volta suave (manual)
- [ ] WASD não invertem (manual)

## Checklist — Head bob

- [ ] Idle sway sutil (manual)
- [ ] Walk bob leve (manual)
- [ ] Sprint bob maior (manual)
- [ ] Crouch bob menor (manual)
- [ ] A/D strafe tilt (manual)

## Checklist — Debug

- [ ] F9 reseta player (manual)
- [ ] Esc libera mouse (manual)
- [ ] F6 roda cena (manual)

---

## Geometria do lab

| Elemento | Uso |
|----------|-----|
| Chão 20×20 | Movimento base |
| Parede Z=-5 | Colisão frontal |
| Parede X=2.8 | Teste lean |
| Rampa X=6 | Subida |

---

## Sprint 02 status

**Em andamento** — aguarda playtest manual completo antes de Sprint 03.
