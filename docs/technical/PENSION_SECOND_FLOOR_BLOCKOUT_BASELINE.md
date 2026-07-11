# Baseline — Segundo andar blockout 01

**Versão:** 3.0 (aprovada)  
**Sprint:** 10  
**Data:** 2026-07-11  
**Status:** ✅ **Aprovada** — blockout cinza navegável  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Builder:** `PensaoVerticalBlockout01Builder.cs`

**Cena térreo baseline (preservada):** `PensaoTerreoBlockout01.tscn`

---

## Status de aprovação

O segundo andar foi **aprovado como blockout funcional** (caixa cinza). Visualmente cru — aceito para esta sprint.

**Ainda fora de escopo (não implementar sem nova sprint):**
- Arte final / texturas finais
- Teto final / telhado
- Portas finais (modelo)
- Móveis finais / GLB / Blender
- Inimigo / combate

**Regra congelada:** **não refazer layout do segundo andar** sem nova sprint aprovada. Hotfixes mínimos só se quebrar navegação ou regressão do térreo.

---

## Regras de design (congeladas)

1. **Segundo andar proporcional ao térreo** — laje principal `14,08 × 44,58 m`; único buraco = vão da escada.
2. **Saída da escada nunca bloqueada** — circulação escada → landing → corredor x = 0 livre.
3. **Corredor superior no eixo do térreo** — x = 0, largura 2,4 m.
4. **Caixa de escada** — `StairBox_Wall_*` + `Stairwell_Rail_*` no vão (5,8 × 8,2 m).
5. **Raycast respeita parede** — oclusão world-only em `PlayerInteractionRaycast`.
6. **Anti-fresta** — paredes 0,20 m × 3,0 m; overlap 0,08 m nos cantos.
7. **Porta vs parede** — vão só onde há circulação; parede sólida onde não há passagem.
8. **Corredor nunca bloqueado por capa norte transversal** — lição do hotfix 3.

---

## Layout

```
Escada (x≈-4,1) → UpperLanding_Main → UpperCorridor_Main (x=0) → Room201 / Room202 → UpperBlockedDoor
```

| Área | Descrição |
|------|-----------|
| **Floor_Second_Main** | Laje segmentada proporcional ao térreo |
| **UpperLanding_Main** | Patamar 5,4 × 3,2 m — liga escada ao corredor |
| **UpperCorridor_Main** | Corredor 2,4 m @ x = 0, z = -19,5 a -7,5 |
| **StairBox_Wall_*** | Caixa do poço — paredes 3,0 m |
| **Stairwell_Rail_*** | Guarda-corpos 1,05 m na borda sul do vão |
| **Room201 / Room202** | Quartos placeholder + interação |
| **UpperBlockedDoor** | Porta trancada z = -7,5 |

---

## Métricas reais

| Parâmetro | Valor |
|-----------|-------|
| Piso superior (top) | **2,8 m** |
| Laje principal | **14,08 × 44,58 m** |
| Vão escada | **5,8 × 8,2 m** @ (-4,1, -27,4) |
| Corredor superior | **2,4 m** @ x = 0 |
| Patamar | **5,4 × 3,2 m** |
| Portas (blockout) | **1,4 m** |
| Quartos | ~5,6 × 4,0 m |

---

## Escada (integração)

| Parâmetro | Valor |
|-----------|-------|
| Foot X / Z | **-4,1** / **-30,5** |
| Topo rampa Z | **-24,7** |
| Rise | **2,8 m** |
| Padrão | `StairRampAssembly` — sem patamar temporário 09A |

Encaixe superior: `UpperLanding_StairBridge` + `UpperLanding_Main` → corredor x = 0.

---

## Nós principais

```
PensionSecondFloor/
  Floor_Second_Main_* / UpperLanding_Main / UpperCorridor_Main
  StairBox_Wall_West / East / North / EastWing
  Stairwell_Rail_Left / Right / Front_Side_West / Front_Side_East
  Wall_UpperCorridor_Left / Right
  Wall_Room201_* / Wall_Room202_*
  Wall_UpperBlockedDoor_*
  Wall_Second_SouthFlank_West / East
  Wall_Second_Front / Back / Left / Right
```

---

## Interações (Sprint 10)

| ID | Prompt | Local |
|----|--------|-------|
| `room_201` | Examinar quarto 201 | Room201 |
| `room_202` | Examinar quarto 202 | Room202 |
| `room_203_locked` | Tentar abrir porta | UpperBlockedDoor |

Interações do térreo **preservadas** (5 pontos + puzzle).

---

## Limitações aceitas (blockout)

- Sem teto / telhado
- Sem arte final / GLB / Blender
- Sem inimigo / combate
- Visual cru — aprovado como blockout cinza

---

## Playtest

`docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md` — **aprovado 2026-07-11**
