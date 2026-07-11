# Sprint 05 — Pensão Térreo Blockout 01

**Status:** ✅ Concluída (2026-07-11)  
**Branch:** `reboot/breu-clean-start`  
**Cena:** `scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

---

## Objetivo

Primeira cena real jogável — **somente térreo**, blockout Godot nativo, 100% navegável.

---

## Checklist

- [x] `PensaoTerreoBlockout01.tscn` com estrutura oficial
- [x] Trilha → varanda → recepção → corredor → quarto 102 → cozinha → depósito
- [x] Paredes 3m, corredor 2.4m, portas 1.4m
- [x] Sem teto, escada, 2º andar, GLB, Blender
- [x] Colisões manuais (StaticBody3D + BoxShape3D)
- [x] 5 interactables (placa, livro, quarto, cozinha, depósito)
- [x] Player + HUD instanciados
- [x] Iluminação playtest + fog leve
- [x] Baselines player/HUD/interação intactas
- [x] `dotnet build` OK

---

## DoD

- [x] Fluxo completo navegável (design)
- [x] Interações wired via `Interactable.cs`
- [ ] Aprovação usuário playtest — pendente

---

## Playtest

`docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md`

---

## Próxima sprint

**Sprint 06 — Playtest e correção do térreo**
