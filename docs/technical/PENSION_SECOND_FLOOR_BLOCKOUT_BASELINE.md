# Baseline — Segundo andar blockout 01

**Versão:** 2.0 (rebuild limpo)  
**Sprint:** 10  
**Data:** 2026-07-11  
**Status:** Rebuild aplicado — playtest F6 pendente  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Builder:** `PensaoVerticalBlockout01Builder.cs`

**Cena térreo baseline (preservada):** `PensaoTerreoBlockout01.tscn`

---

## Regras de design

1. **Segundo andar proporcional ao térreo** — laje principal com mesma largura/profundidade do piso térreo (`14,08 × 44,58 m`, centro z = -10,75).
2. **Saída da escada nunca bloqueada** — nenhuma parede ou rail na frente da rampa (+Z).
3. **Abertura só na escada** — vão de `6,0 × 8,5 m` @ (-4,1, -27,4) para a escada passar; resto coberto por laje visual.
4. **Corredor superior no eixo do térreo** — x = 0, largura 2,4 m (igual corredor térreo).
5. **Raycast respeita parede** — `PlayerInteractionRaycast` com oclusão world-only.

---

## Layout

```
Escada (x≈-4,1) → UpperLanding_Main → UpperCorridor_Main (x=0) → Room201 / Room202 → UpperBlockedDoor
```

| Área | Descrição |
|------|-----------|
| **Floor_Second_Main** | Laje segmentada proporcional ao térreo — cobertura visual sobre interior |
| **UpperLanding_Main** | Patamar 3,5 × 3,5 m @ saída da rampa — livre para +Z |
| **UpperCorridor_Main** | Corredor 2,4 m, x = 0, z = -20 a -7,5 |
| **Room201** | Oeste (espelha quarto 102) — cama placeholder |
| **Room202** | Leste (espelha cozinha) — armário placeholder |
| **UpperBlockedDoor** | Porta trancada z = -7,5 — não abre nesta sprint |

---

## Métricas reais

| Parâmetro | Valor |
|-----------|-------|
| Piso superior (top) | **2,8 m** |
| Laje principal | **14,08 × 44,58 m** (igual térreo) |
| Vão escada | **6,0 × 8,5 m** @ (-4,1, -27,4) |
| Paredes | 3,0 m altura · 0,20 m espessura |
| Corredor superior | **2,4 m** @ x = 0 |
| Patamar | **3,5 × 3,5 m** mínimo |
| Portas | **1,4 m** |
| Quartos | ~5,6 × 4,0 m |
| Overlap cantos | 0,08 m |

---

## Escada (integração)

| Parâmetro | Valor |
|-----------|-------|
| Foot X | **-4,1** (alcova oeste) |
| Foot Z | **-30,5** |
| Topo rampa Z | **-24,7** |
| Rise | **2,8 m** |
| Padrão | `StairRampAssembly` — sem patamar temporário 09A |

Encaixe superior:
- `UpperLanding_StairBridge` conecta topo da rampa
- `UpperLanding_Main` + `UpperLanding_CorridorBridge` ligam ao corredor x = 0

---

## Nós principais

```
PensionSecondFloor/
  Floor_Second_Main_South
  Floor_Second_Main_NorthEast
  Floor_Second_Main_NorthCap
  UpperLanding_Main
  UpperLanding_StairBridge
  UpperLanding_CorridorBridge
  UpperCorridor_Main
  Wall_UpperCorridor_Left / Right
  Wall_Room201_*
  Wall_Room202_*
  Wall_UpperBlockedDoor_*
  Wall_Second_Front / Back / Left / Right
  Furniture_Room201_Bed
  Furniture_Room202_Cabinet
```

---

## Interações (Sprint 10)

| ID | Prompt | Local |
|----|--------|-------|
| `room_201` | Examinar quarto 201 | Room201 @ (-4,1, -14) |
| `room_202` | Examinar quarto 202 | Room202 @ (4,1, -17) |
| `room_203_locked` | Tentar abrir porta | UpperBlockedDoor @ (0, -7,5) |

Interações do térreo **preservadas** (5 pontos + puzzle).

---

## Limitações atuais

- **Sem teto** — sprint futura
- **Sem telhado**
- **Sem inimigo / combate**
- **Sem arte final / GLB**

---

## Playtest

`docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md`
