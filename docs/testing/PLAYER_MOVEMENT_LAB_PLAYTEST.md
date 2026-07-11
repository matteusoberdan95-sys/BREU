# Playtest — Player Movement Lab

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Sprint:** 02 — **APROVADA**  
**Data aprovação:** 2026-07-11  
**Baseline:** `docs/technical/PLAYER_CONTROLLER_BASELINE.md`

---

## Status

**PlayerMovementLab aprovado pelo usuário.**  
Movimentação base congelada — não alterar sem nova sprint de player.

---

## Checklist aprovado — Movimento

- [x] W frente correto
- [x] S trás correto
- [x] A/D laterais corretos
- [x] Movimento segue yaw do player
- [x] Sem tremor / preso no chão

## Checklist aprovado — Sprint

- [x] Shift corre
- [x] Stamina consome e regenera
- [x] Sprint para sem stamina

## Checklist aprovado — Crouch

- [x] C / Ctrl agacha
- [x] Altura câmera e cápsula corretas
- [x] Não atravessa teto baixo

## Checklist aprovado — Lean

- [x] Q lean esquerda
- [x] R lean direita
- [x] Volta suave ao soltar
- [x] Funciona andando e correndo

## Checklist aprovado — Look back

- [x] Shift + W + Alt olha para trás
- [x] Shift + W + X (alternativo)
- [x] Player continua correndo para frente
- [x] Velocidade não reduz
- [x] Soltar Alt/X volta suave
- [x] Mouse look intacto

## Checklist aprovado — Camera feel

- [x] Idle: respiração sutil
- [x] Walk: balanço leve e fluido
- [x] Sprint: peso corporal, sem metronômetro
- [x] Crouch: bob menor
- [x] Strafe: inclinação leve
- [x] Sem enjoo / jitter

---

## Controles (referência)

| Tecla | Ação |
|-------|------|
| WASD | Mover |
| Mouse | Olhar |
| Shift | Sprint |
| C / Ctrl | Agachar |
| Q / R | Lean |
| Alt / X | Look back (sprint + W) |
| F | Lanterna |
| F9 | Reset spawn |
| Esc | Liberar mouse |

---

## Histórico de correções (Sprint 02)

| Fase | Problema | Resolução |
|------|----------|-----------|
| 02.0 | Spawn em túnel, W/S invertido | Lab mínimo, GetVector corrigido |
| 02.1 | Lean, look back, bob inicial | Pivots modulares |
| 02.2 | Bob exagerado, Alt quebra sprint | Bob por velocidade, input físico |
| 02.2b | Look back não ativava | ControllerPath corrigido |

---

## Próximo passo

Sprint 03 — HUD e Debug (sem mexer no player baseline).
