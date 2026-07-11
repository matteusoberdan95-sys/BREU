# Baseline — Teto e cobertura blockout (Pensão)

**Versão:** 1.0  
**Sprint:** 12  
**Data:** 2026-07-11  
**Status:** Implementado — playtest F6 pendente  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Builder:** `PensaoVerticalBlockout01Builder.cs`

---

## Objetivo

Fechar visualmente a Pensão por cima sem arte final — teto/forro blockout + cobertura externa simples.

---

## Alturas aprovadas

| Nível | Piso (top) | Teto (underside) | Altura livre |
|-------|------------|------------------|--------------|
| Térreo (varanda/frente) | 0,0 m | ~2,95 m | ~3,0 m |
| Térreo (sob laje 2º) | 0,0 m | ~2,57 m (laje) | ~2,6 m |
| 2º andar | 2,8 m | 5,8 m | ~3,0 m |
| Cobertura | — | ~6,1 m (base telhado) | — |

---

## Nós principais

```
PensionCeiling/
  Ceiling_FirstFloor_Main          — varanda/frente (sem laje acima)
  Ceiling_SecondFloor_Main         — corredor + quartos
  Ceiling_SecondFloor_StairEastBand
  Ceiling_SecondFloor_Main_NorthEast
  Ceiling_SecondFloor_Main_NorthCap
  Ceiling_StairBox_WestCap

Exterior/
  Roof_Blockout_Main
  Roof_Blockout_Ridge
```

---

## Regras

1. **Tetos são visual-only** — sem colisão (evita prender câmera/player).
2. **Vão da escada aberto no teto** — buraco alinhado ao poço; telhado fecha externamente.
3. **Não duplicar** teto onde `Floor_Second_Main` já funciona como laje vista de baixo.
4. **Blockout ≠ final** — sem telha, textura, beiral ou GLB nesta sprint.
5. **Não refazer layout** do 2º andar sem nova sprint.

---

## Pendências futuras

- Teto modular com clipping zero (Sprint futura pós-playtest).
- Telhado final com duas águas / beiral.
- Texturas e materiais finais.
- Janelas / claraboia no poço da escada (opcional narrativo).

---

## Playtest

`docs/testing/PENSION_CEILING_BLOCKOUT_PLAYTEST.md`
