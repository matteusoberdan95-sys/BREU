# Sprint 01 — Fundação Godot mínima

**Status:** Concluída  
**Branch:** `reboot/breu-clean-start`  
**Data:** 2026-07-11

---

## Objetivo

Recriar fundação Godot 4.7 + C# executável, sem gameplay.

---

## Checklist

- [x] `project.godot` — Godot 4.7 mono, main scene bootstrap
- [x] `BREU.csproj` + `BREU.sln` + `GlobalUsings.cs`
- [x] `icon.svg`
- [x] Estrutura de pastas conforme `docs/technical/GODOT_PROJECT_STRUCTURE.md`
- [x] `scenes/levels/BootstrapEmpty.tscn` — Node3D + WorldEnvironment + DirectionalLight3D
- [x] Input map placeholder (WASD, sprint, crouch, interact, attack, pause)
- [x] Physics layers (World, Interactable, Trigger, Enemy, Player)
- [x] Sem autoloads, sem player, sem pensão, sem HUD
- [x] `dotnet build` OK
- [x] Godot abre cena bootstrap sem erro

---

## Cena oficial

`res://scenes/levels/BootstrapEmpty.tscn`

---

## Próxima sprint

**Sprint 02 — Player Controller limpo**

Ver `docs/production/SPRINT_ROADMAP.md`
