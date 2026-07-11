# Playtest — Pensão Térreo Blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 05 + hotfix visual blockout  
**Data:** 2026-07-11

---

## Sprint 05 Hotfix — Visual Blockout Clean

| Problema | Correção |
|----------|----------|
| Z-fighting piso externo | 1 laje exterior + trilha **elevada** (Y distintos) |
| Pisos interiores coplanares | **1** `Floor_Interior_Main` em vez de 5 overlays |
| Fresta piso/parede | Paredes embutidas 0,06 m no piso; soleiras nas portas |
| Recortes em portas | `DoorThresholds` visuais |

---

| Problema | Correção |
|----------|----------|
| Vãos entre pisos fragmentados | 3 lajes contínuas de colisão com overlap 0,08 m |
| Player caía ao pular (Space) | Topo de colisão alinhado em Y=0 em toda área |
| Z-fighting / piso piscando | Pisos visuais consolidados, trilha elevada, sem coplanares |
| Fresta piso/parede | Paredes embutidas + soleiras de porta |

**Lajes de colisão:**
- `Exterior_MainGround` — trilha + approach
- `Porch_MainFloor` — varanda + entrada
- `PensionGroundFloor_MainFloor` — recepção → depósito (inclui quarto/cozinha)

---

## Fluxo esperado

Trilha → Varanda → Recepção → Corredor → Quarto 102 / Cozinha → Depósito trancado

---

## Checklist — Movimentação preservada

| Teste | OK |
|-------|-----|
| W/S/A/D corretos | ☐ |
| Sprint | ☐ |
| Crouch | ☐ |
| Lean Q/R | ☐ |
| Look back | ☐ |
| Camera feel | ☐ |

---

## Checklist — Visual blockout (hotfix)

| Teste | OK |
|-------|-----|
| Piso externo não pisca (z-fighting) | ☐ |
| Trilha legível sem duas cores brigando | ☐ |
| Sem frestas grandes piso/parede | ☐ |
| Portas/cômodos visualmente fechados | ☐ |
| Gameplay/colision/interação intactos | ☐ |

---

| Teste | OK |
|-------|-----|
| Trilha → varanda sem queda | ☐ |
| Varanda → recepção sem queda | ☐ |
| Recepção / corredor / quarto / cozinha / depósito sem queda | ☐ |
| Pular (Space) nas transições — não cai | ☐ |
| Sem frestas grandes no chão | ☐ |

---

| Teste | OK |
|-------|-----|
| Player nasce na trilha (z≈45) | ☐ |
| Anda até varanda | ☐ |
| Não cai do mapa | ☐ |
| Limites invisíveis OK | ☐ |
| Placa — prompt + E | ☐ |

---

## Checklist — Entrada / Recepção

| Teste | OK |
|-------|-----|
| Varanda acessível | ☐ |
| Porta principal livre | ☐ |
| Recepção circulável | ☐ |
| Balcão não bloqueia centro | ☐ |
| Livro — prompt + E | ☐ |

---

## Checklist — Corredor / Cômodos

| Teste | OK |
|-------|-----|
| Corredor ≥2.2m confortável | ☐ |
| Quarto 102 — entrar + virar câmera | ☐ |
| Quarto 102 — interação | ☐ |
| Cozinha — entrar + virar câmera | ☐ |
| Cozinha — interação | ☐ |
| Depósito — porta bloqueia | ☐ |
| Depósito — prompt + E | ☐ |

---

## Checklist — HUD / Debug

| Teste | OK |
|-------|-----|
| Vida / Stamina / Lanterna | ☐ |
| Prompt interação | ☐ |
| Mensagens | ☐ |
| F10 / F11 | ☐ |
| Sem caixa vazia | ☐ |

---

## Critério gate

**Térreo 100% navegável** — sem atravessar paredes, sem soft-lock.

Sprint 06 só após aprovação deste checklist.

---

## Log

| Data | Nota |
|------|------|
| 2026-07-11 | Hotfix visual de blockout aplicado — playtest visual pendente |
