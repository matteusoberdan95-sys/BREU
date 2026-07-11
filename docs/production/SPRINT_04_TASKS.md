# Sprint 04 — Sistema de Interação Mínimo

**Status:** ✅ Aprovada pelo usuário (2026-07-11)  
**Branch:** `reboot/breu-clean-start`  
**Baseline:** `docs/technical/INTERACTION_SYSTEM_BASELINE.md`

---

## Objetivo

Interação FPS limpa com tecla E, prompt no HUD e mensagem temporária — sem pensão, puzzle ou inventário.

---

## Checklist

### Sistema
- [x] `IInteractable.cs`
- [x] `Interactable.cs` (PromptText, InteractionMessage, OneShot, InteractionId)
- [x] `PlayerInteractionRaycast.cs` (2.5 m, layer Interactable)
- [x] RayCast3D em `Camera3D` — sem alterar PlayerController/CameraFeel

### HUD
- [x] `InteractionPromptLabel` — extensão mínima
- [x] Mensagem 3 s via `ShowMessage` existente

### Cena teste
- [x] `InteractionLab.tscn` — chão, luz, 3 objetos
- [x] TestSign, TestBook, TestLockedDoor
- [x] HUD instanciado

### Debug
- [x] Log ao mudar alvo (`DebugMode`)

### Preservação
- [x] Player movement baseline intacto
- [x] HUD base intacto (só prompt adicionado)
- [x] `dotnet build` OK

---

## DoD

- [x] E + mira mostra prompt e mensagem
- [x] Objetos não bloqueiam player (layer 2 only)
- [x] Documentação + playtest checklist

---

## Playtest

`scenes/test/InteractionLab.tscn` — ver `INTERACTION_LAB_PLAYTEST.md`

---

## Próxima sprint

**Sprint 05 — Pensão térreo blockout 01**
