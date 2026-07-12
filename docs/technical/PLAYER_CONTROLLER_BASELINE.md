# BREU — Player Controller Baseline

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 02 — aprovada e congelada  
**Status:** OFICIAL — não alterar sem nova sprint de player

---

## Regra de congelamento

> **Não modificar** `PlayerController`, `PlayerCameraFeel`, `PlayerLookBack`, `PlayerLean`, `PlayerCrouch`, `PlayerStamina`, `PlayerLook` ou valores de movimento/camera feel **sem solicitação explícita do usuário** ou sprint dedicada ao player.

Sprint 03+ (HUD, interação, level) devem **consumir** este baseline, não reescrevê-lo.

---

## Cena e scripts

| Item | Caminho |
|------|---------|
| Player | `res://scenes/player/Player.tscn` |
| Lab de teste | `res://scenes/test/PlayerMovementLab.tscn` |
| Controller | `scripts/player/PlayerController.cs` |
| Camera feel | `scripts/player/PlayerCameraFeel.cs` |
| Look | `scripts/player/PlayerLook.cs` |
| Look back | `scripts/player/PlayerLookBack.cs` |
| Lean | `scripts/player/PlayerLean.cs` |
| Crouch | `scripts/player/PlayerCrouch.cs` |
| Stamina | `scripts/player/PlayerStamina.cs` |
| Flashlight | `scripts/player/PlayerFlashlight.cs` |

---

## Hierarquia de nós (Player.tscn)

```
CharacterBody3D: Player          ← PlayerController
  CollisionShape3D               ← Capsule r=0.35, h=1.8, Y=0.9
  PlayerStamina
  PlayerCrouch
  HeadBase                       ← altura crouch (Y 1.65 → 1.05)
    BodyMotionPivot              ← PlayerCameraFeel (bob/sway/inertia)
      LeanPivot                  ← PlayerLean (Q/R)
        LookBackPivot            ← PlayerLookBack (Alt/X)
          Camera3D               ← PlayerLook (pitch); yaw no Player
            Flashlight           ← PlayerFlashlight
```

**Movimento** usa yaw do `Player` (CharacterBody3D), nunca pivots visuais.

---

## Input map

| Ação | Tecla |
|------|-------|
| `move_forward` | W |
| `move_backward` | S |
| `move_left` | A |
| `move_right` | D |
| `sprint` | Shift |
| `crouch` | C, Ctrl |
| `lean_left` | Q |
| `lean_right` | R |
| `look_back` | Alt, X |
| `flashlight` | F |
| `interact` | E |
| `jump` | Space |
| `debug_reset_player` | F3 |
| `LevelSanityChecker` | F9 |
| `ui_cancel` | Esc |

**Nota movimento:** input lido via `GetActionStrength` (não `GetVector` puro) — Alt+W compatível no Windows.

---

## Valores — movimento (PlayerController)

| Parâmetro | Valor |
|-----------|-------|
| WalkSpeed | 4.0 |
| SprintSpeed | 6.5 |
| CrouchSpeed | 2.0 |
| Acceleration | 12.0 |
| Deceleration | 14.0 |
| AirControl | 0.35 |
| Gravity | 9.8 |
| JumpVelocity | 4.2 |
| SprintStaminaDrainPerSecond | 18.0 |

## Valores — stamina (PlayerStamina)

| Parâmetro | Valor |
|-----------|-------|
| MaxStamina | 100 |
| RegenPerSecond | 14.0 |
| RegenDelaySeconds | 0.75 |

## Valores — crouch (PlayerCrouch)

| Parâmetro | Valor |
|-----------|-------|
| StandingCapsuleHeight | 1.8 |
| CrouchingCapsuleHeight | 1.0 |
| CameraHeightStanding | 1.65 |
| CameraHeightCrouching | 1.05 |
| TransitionSpeed | 10.0 |

## Valores — lean (PlayerLean)

| Parâmetro | Valor |
|-----------|-------|
| LeanOffset | 0.32 m |
| LeanRollDegrees | 8.0 |
| LeanSpeed | 8.0 |
| LeanWallProbeDistance | 0.45 m |

## Valores — look back (PlayerLookBack)

| Parâmetro | Valor |
|-----------|-------|
| LookBackAngleDegrees | 165 |
| LookBackEnterSpeed | 8.0 |
| LookBackExitSpeed | 10.0 |
| ControllerPath | `../../../..` (Player) |

Ativação: sprint + forward + `look_back` (Alt ou X). Efeito **somente visual** no `LookBackPivot`.

## Valores — camera feel (PlayerCameraFeel, preset BreuDefault)

| Parâmetro | Valor |
|-----------|-------|
| IdleBreathAmountY | 0.006 |
| IdleBreathFrequency | 0.85 |
| WalkBobVertical | 0.030 |
| WalkBobHorizontal | 0.018 |
| WalkStepCycleMultiplier | 1.45 |
| SprintBobVertical | 0.050 |
| SprintBobHorizontal | 0.028 |
| SprintStepCycleMultiplier | 1.65 |
| CrouchBobVertical | 0.015 |
| CrouchStepCycleMultiplier | 1.10 |
| SprintMicroShakeAmount | 0.0 (off) |
| StrafeTiltDegrees | 1.2 |
| SprintStrafeTiltDegrees | 1.8 |
| InertiaSmooth | 7.5 |
| CameraReturnSmooth | 9.0 |
| LookBackBobReduction | 15% |

Bob baseado em **velocidade horizontal × step cycle multiplier**, não frequência fixa alta.

---

## Colisão

| Item | Valor |
|------|-------|
| Capsule radius | 0.35 |
| Capsule height | 1.8 |
| CollisionShape Y | 0.9 |
| Player layer | 16 (layer 5) |
| Mask | 1 (World) |
| Camera near | 0.05 |

---

## Próxima sprint permitida no player

Nenhuma alteração até nova sprint de player.  
**Sprint 03** adiciona HUD que **lê** stamina/sinais — não altera feel.

---

## Referências

- Playtest aprovado: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
- Estado: `docs/PROJECT_STATE.md`
- Roadmap: `docs/production/SPRINT_ROADMAP.md`
