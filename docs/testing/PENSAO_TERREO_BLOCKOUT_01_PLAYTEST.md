# Playtest — Pensão Térreo Blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 05 + hotfix chão  
**Data:** 2026-07-11

---

## Sprint 05 Hotfix — Floor Gap Fix

| Problema | Correção |
|----------|----------|
| Vãos entre pisos fragmentados | 3 lajes contínuas de colisão com overlap 0,08 m |
| Player caía ao pular (Space) | Topo de colisão alinhado em Y=0 em toda área |
| Frestas trilha/varanda/recepção | `Exterior_MainGround` + `Porch_MainFloor` + `PensionGroundFloor_MainFloor` |
| Pisos visuais separados | Overlays sem colisão (`*_VisualFloor`) |

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

## Checklist — Navegação sem buracos (hotfix)

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
