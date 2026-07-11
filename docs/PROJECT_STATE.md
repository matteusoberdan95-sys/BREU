# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 02.1 em andamento  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **Em andamento** (02.1 — feel + direção) |
| Sprint 03 | **Bloqueada** |
| Cena de teste | `res://scenes/test/PlayerMovementLab.tscn` |

---

## Sprint 02.1 — entregas

| Item | Status |
|------|--------|
| W/S corrigido (GetVector) | ✅ |
| Lean Q/R estilo Outlast | ✅ implementado |
| Look back Alt (sprint+W) | ✅ implementado |
| Head bob / sway | ✅ `PlayerCameraEffects.cs` |
| Reset F9 | ✅ |
| Playtest manual completo | ⏳ pendente |

---

## Player — scripts

| Script | Função |
|--------|--------|
| `PlayerController.cs` | Movimento, stamina, yaw-based direction |
| `PlayerLook.cs` | Mouse, lean, look-back |
| `PlayerCameraEffects.cs` | Head bob, strafe tilt |
| `PlayerCrouch.cs` | Agachar |
| `PlayerStamina.cs` | Stamina sprint |

---

## Próximo passo

1. F6 em `PlayerMovementLab.tscn` — validar checklist completo
2. Marcar Sprint 02 concluída se todos passarem
3. Então Sprint 03 (HUD)

Ver: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
