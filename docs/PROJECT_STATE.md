# BREU — Estado do projeto

**Última atualização:** 2026-07-11  
**Fase:** REBOOT GREENFIELD — Sprint 02.2 em andamento  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02 | **Em andamento** (02.2 — horror camera feel) |
| Sprint 03 | **Bloqueada** |
| Cena de teste | `res://scenes/test/PlayerMovementLab.tscn` |

---

## Player — status

| Componente | Status |
|------------|--------|
| `PlayerController` | ✅ Movimento, stamina, landing impact |
| `PlayerCameraFeel` | ✅ Bob, sway, inertia, sprint shake |
| `PlayerLean` | ✅ Q/R separado |
| `PlayerLookBack` | ✅ Alt sprint look-back |
| `PlayerLook` | ✅ Mouse yaw/pitch |
| `PlayerCrouch` | ✅ Altura HeadBase |

**Preset câmera:** `BreuDefault`

---

## Sprint 02.2 — entregas

- Hierarquia pivots (BodyMotion → Lean → LookBack → Pitch → Camera)
- `PlayerCameraFeel.cs` com 3 presets exportados
- Removido `PlayerCameraEffects.cs` (substituído)
- Lab: chão 40×20 para sprint longo
- Landing bob preparado

**Playtest manual:** pendente aprovação do usuário.

---

## Próximo passo

1. F6 em `PlayerMovementLab.tscn`
2. Validar checklist Sprint 02.2
3. Usuário aprova → Sprint 02 concluída → Sprint 03 (HUD)

Ver: `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
