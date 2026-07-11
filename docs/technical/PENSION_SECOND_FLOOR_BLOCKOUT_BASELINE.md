# Baseline — Segundo andar blockout 01

**Versão:** 1.0  
**Sprint:** 10  
**Data:** 2026-07-11  
**Status:** Implementado — playtest F6 pendente  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Builder:** `PensaoVerticalBlockout01Builder.cs`

**Cena térreo baseline (preservada):** `PensaoTerreoBlockout01.tscn`

---

## Layout

```
Escada → UpperLanding → Corredor superior → Quarto 201 / Quarto 202 → Porta bloqueada (203)
```

| Área | Descrição |
|------|-----------|
| **UpperLanding** | Chegada da escada @ y = 2,8 m, guarda-corpos |
| **UpperCorridor** | Corredor 2,4 m, eixo x ≈ -4,1 |
| **Room 201** | Oeste do corredor — cama placeholder |
| **Room 202** | Leste do corredor — armário placeholder |
| **UpperBlockedDoor** | Porta trancada z ≈ -7,5 — não abre nesta sprint |

---

## Métricas

| Parâmetro | Valor |
|-----------|-------|
| Piso superior (top) | **2,8 m** (compatível com escada) |
| Paredes | 3,0 m altura · 0,20 m espessura |
| Corredor superior | **2,4 m** |
| Portas | **1,4 m** |
| Quartos | ~3,8 × 4,0 m |
| Overlap cantos | 0,08 m |

---

## Nós principais

```
PensionSecondFloor/
  Floor_Second_Main
  Floor_Second_StairTransition
  UpperLanding_Rail_*
  Wall_Second_Corridor_*
  Wall_Room201_*
  Wall_Room202_*
  Wall_UpperBlockedDoor_*
  Furniture_Room201_Bed
  Furniture_Room202_Cabinet
```

---

## Interações (Sprint 10)

| ID | Prompt | Local |
|----|--------|-------|
| `room_201` | Examinar quarto 201 | Quarto 201 |
| `room_202` | Examinar quarto 202 | Quarto 202 |
| `room_203_locked` | Tentar abrir porta | Porta bloqueada superior |

Interações do térreo **preservadas** (5 pontos + puzzle).

---

## Escada

- Padrão `StairRampAssembly` — rampa invisível, sem patamar temporário
- Piso superior conecta via `Floor_Second_StairTransition`
- **Não alterar** PlayerController / PlayerCameraFeel

---

## Limitações atuais

- **Sem teto** — sprint futura
- **Sem telhado**
- **Sem inimigo / combate**
- **Sem arte final / GLB**
- Patamar temporário da 09A **substituído** por layout real nesta cena vertical

---

## Playtest

`docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md`
