# Playtest — Stair Movement Lab

**Cena:** `res://scenes/test/StairMovementLab.tscn`  
**Sprint:** 08 — Escada isolada de teste  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/STAIR_RAMP_BASELINE.md`

---

## Status

**StairMovementLab implementado.** Validar com F6 antes de marcar sprint como aprovada.

---

## Checklist — Movimento base

- [ ] Player nasce no piso inferior (`PlayerSpawn` z = -8)
- [ ] W/S/A/D continuam corretos
- [ ] Sprint continua funcionando
- [ ] Crouch continua funcionando
- [ ] Lean Q/R continua funcionando
- [ ] Look back continua funcionando
- [ ] Camera feel continua agradável

---

## Checklist — Subida

- [ ] Player consegue chegar até a escada
- [ ] Player entra na escada sem travar
- [ ] Player sobe até o topo
- [ ] Player não quica nos degraus
- [ ] Player não fica preso na rampa
- [ ] Player não atravessa visual estranho
- [ ] Câmera não bate em geometria

---

## Checklist — Topo

- [ ] Player chega no piso superior
- [ ] Player consegue andar no piso superior
- [ ] Player consegue virar a câmera
- [ ] Player não cai do topo
- [ ] Player não fica preso na transição

---

## Checklist — Descida

- [ ] Player consegue descer pela escada
- [ ] Player não quica descendo
- [ ] Player não escorrega de forma absurda
- [ ] Player não cai atravessando a rampa
- [ ] Player volta ao piso inferior

---

## Checklist — Colisão

- [ ] Degraus visuais não prendem o player
- [ ] Rampa invisível (`Stair_InvisibleRamp_Collision`) é a colisão principal
- [ ] Piso inferior tem colisão
- [ ] Piso superior tem colisão
- [ ] Barreiras/limites impedem queda (`UpperFloor_Rail_*`, `UpperFloor_BackWall`)

---

## Critério principal

A escada só passa se **subir e descer for suave** (10+ ciclos ida e volta).

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
