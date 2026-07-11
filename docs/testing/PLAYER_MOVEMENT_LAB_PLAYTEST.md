# Playtest — Player Movement Lab

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Sprint:** 02.2  
**Data:** 2026-07-11

---

## Sprint 02.2 — Horror Body Camera Feel

**Preset ativo:** `BreuDefault` (Preset C)

### Hierarquia de câmera

```
HeadBase (altura crouch)
  BodyMotionPivot  → PlayerCameraFeel (bob/sway/inércia/landing)
    LeanPivot      → PlayerLean (Q/R)
      LookBackPivot → PlayerLookBack (Alt)
        PitchPivot  → PlayerLook (mouse pitch)
          Camera3D + Flashlight
```

### Valores principais (Breu Default)

| Parâmetro | Valor |
|-----------|-------|
| WalkBobVertical | 0.045 |
| WalkBobFrequency | 7.5 |
| SprintBobVertical | 0.095 |
| SprintBobFrequency | 11.5 |
| SprintShoulderSway | 0.070 |
| SprintMicroShakeAmount | 0.010 |
| CrouchBobVertical | 0.022 |
| IdleBreathAmountY | 0.012 |
| LookBackBobReduction | 15% |

Presets alternativos no Inspector: `Subtle` (RE7), `OutlastInspired`.

---

## Controles

| Tecla | Ação |
|-------|------|
| WASD | Mover |
| Mouse | Olhar |
| Shift | Sprint |
| C / Ctrl | Agachar |
| Q / R | Lean |
| Alt + Shift + W | Look back |
| F9 | Reset |
| Esc | Liberar mouse |

---

## Checklist — Movimento base

- [x] W/S corrigido (GetVector — Sprint 02.1)
- [ ] W frente / S trás / A/D (manual)
- [ ] Sprint / crouch (manual)

## Checklist — Camera feel (02.2)

- [ ] Idle: respiração quase imperceptível
- [ ] Walk: balanço corporal leve
- [ ] Sprint: ombro/torso forte, corrida física
- [ ] Crouch: bob menor e contido
- [ ] Strafe: inclinação leve A/D
- [ ] Parar de correr: recuperação suave
- [ ] Sem enjoo / jitter

## Checklist — Compatibilidade

- [ ] Lean Q/R + bob simultâneos
- [ ] Look back Alt + sprint bob (reduzido 15%)
- [ ] Mouse look normal
- [ ] Lanterna segue câmera
- [ ] F9 reset / F6 roda cena

---

## Geometria do lab

| Elemento | Uso |
|----------|-----|
| Chão 40×20 | Reta longa para sprint |
| Parede Z=-9 | Colisão frontal |
| Parede X=2.8 | Lean |
| Rampa X=6 | Subida / landing leve |

---

## Sprint 02 status

**Em andamento** — aguarda aprovação do usuário após playtest F6.

Sprint 03 bloqueada.

---

## Histórico

### Sprint 02.1
- Fix GetVector W/S
- Lean, look back, bob básico

### Sprint 02.0
- Fix spawn/tunnel, cena mínima
