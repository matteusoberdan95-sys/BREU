# Sprint 03 — HUD e Debug

**Status:** ✅ Concluída (2026-07-11)  
**Branch:** `reboot/breu-clean-start`

---

## Objetivo

Feedback mínimo no HUD + flags de playtest, **sem alterar** o baseline congelado do player (movimento/camera feel).

---

## Checklist

### HUD
- [x] `scenes/ui/HUD.tscn` + `scripts/ui/HUDController.cs`
- [x] Vida (`PlayerHealth`)
- [x] Stamina (barra + label via `PlayerStamina.StaminaChanged`)
- [x] Lanterna (bateria + estado on/off via `PlayerFlashlight`)
- [x] Mensagens temporárias (`ShowMessage`)
- [x] Painel não bloqueia mouse look

### Debug
- [x] Autoload `PlaytestDebugSettings`
- [x] Flag `InfiniteLanternBattery` (default ON em playtest)
- [x] Flag `ReducedFog` (ajusta `WorldEnvironment` da cena)
- [x] F10: toggle lanterna infinita
- [x] F11: toggle fog reduzida
- [x] Painel debug no canto superior direito

### Player (extensões permitidas)
- [x] `PlayerHealth.cs` + nó em `Player.tscn`
- [x] `PlayerFlashlight.cs` — bateria + sinais (sem mudar movimento)
- [x] **Não** alterados: Controller, CameraFeel, LookBack, Lean, Crouch, Stamina, Look

### Integração
- [x] `PlayerMovementLab.tscn` — HUD + `WorldEnvironment` com fog
- [x] Mensagem de boas-vindas no lab
- [x] `dotnet build` OK

---

## DoD

- [x] HUD exibe vida, stamina e lanterna em tempo real
- [x] Mensagem temporária aparece e some sozinha
- [x] Com debug infinito ativo, lanterna **não zera** em 10+ min
- [x] F11 altera densidade de fog visivelmente no lab
- [x] Player baseline (movimento) intacto

---

## Playtest

Ver: `docs/testing/HUD_DEBUG_PLAYTEST.md`

**Cena:** `res://scenes/test/PlayerMovementLab.tscn` (F6)

---

## Próxima sprint

**Sprint 04 — Sistema de interação mínimo**
