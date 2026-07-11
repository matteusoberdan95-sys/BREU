# Playtest — Stair Movement Lab

**Cena:** `res://scenes/test/StairMovementLab.tscn`  
**Sprint:** 08 — Escada isolada de teste — **APROVADA**  
**Data aprovação:** 2026-07-11  
**Baseline:** `docs/technical/STAIR_RAMP_BASELINE.md`

---

## Status

**StairMovementLab aprovado pelo usuário.**  
Escada validada em cena isolada — **não integrada na Pensão Santa Luzia.**

---

## Checklist final aprovado

- [x] Player sobe a escada sem travar
- [x] Player desce a escada sem quicar
- [x] Player chega no topo
- [x] Player anda na plataforma superior
- [x] Player não cai do topo
- [x] Player não fica preso nas laterais
- [x] Degraus visuais não têm colisão ruim
- [x] Rampa invisível é a colisão principal
- [x] PlayerController não foi alterado
- [x] HUD continua funcionando

---

## Checklist — Movimento base

- [x] Player nasce no piso inferior (`PlayerSpawn` z = -8)
- [x] W/S/A/D continuam corretos
- [x] Sprint continua funcionando
- [x] Crouch continua funcionando
- [x] Lean Q/R continua funcionando
- [x] Look back continua funcionando
- [x] Camera feel continua agradável

---

## Checklist — Subida

- [x] Player consegue chegar até a escada
- [x] Player entra na escada sem travar
- [x] Player sobe até o topo
- [x] Player não quica nos degraus
- [x] Player não fica preso na rampa
- [x] Player não atravessa visual estranho
- [x] Câmera não bate em geometria

---

## Checklist — Topo

- [x] Player chega no piso superior
- [x] Player consegue andar no piso superior
- [x] Player consegue virar a câmera
- [x] Player não cai do topo
- [x] Player não fica preso na transição

---

## Checklist — Descida

- [x] Player consegue descer pela escada
- [x] Player não quica descendo
- [x] Player não escorrega de forma absurda
- [x] Player não cai atravessando a rampa
- [x] Player volta ao piso inferior

---

## Checklist — Colisão

- [x] Degraus visuais não prendem o player
- [x] Rampa invisível (`Stair_InvisibleRamp_Collision`) é a colisão principal
- [x] Piso inferior tem colisão
- [x] Piso superior tem colisão
- [x] Barreiras/limites impedem queda (`UpperFloor_Rail_*`, `UpperFloor_BackWall`)

---

## Regressão — Player / HUD

- [x] Movimento aprovado preservado (Sprint 02)
- [x] HUD e debug F10/F11 funcionando (Sprint 03)
- [x] Nenhuma alteração em `PlayerController` ou `PlayerCameraFeel`

---

## Critério principal

**Aprovado:** subir e descer suaves confirmados pelo usuário.

---

## Não testar nesta sprint

- Integração na Pensão
- Segundo andar real
- Teto
- Inimigo / combate
- Interação

---

## Reset

**F9** — volta ao spawn inferior.
