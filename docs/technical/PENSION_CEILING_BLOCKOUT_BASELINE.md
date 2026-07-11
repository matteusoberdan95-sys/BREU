# Baseline — Teto e cobertura blockout (Pensão)

**Versão:** 1.1  
**Sprint:** 12 + 12A  
**Data:** 2026-07-11  
**Status:** ✅ **Aprovado** — fechamento superior blockout funcional  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Builder:** `PensaoVerticalBlockout01Builder.cs`

---

## Objetivo

Fechar visualmente a Pensão por cima sem arte final — teto/forro blockout + cobertura externa simples.

**Aprovado em 2026-07-11** após hotfix 12A. Ainda é **blockout**, não arte final.

---

## Alturas aprovadas

| Nível | Piso (top) | Teto (underside) | Altura livre |
|-------|------------|------------------|--------------|
| Térreo (varanda/frente) | 0,0 m | ~2,95 m | ~3,0 m |
| Térreo (sob laje 2º) | 0,0 m | ~2,57 m (laje) | ~2,6 m |
| 2º andar | 2,8 m | 5,8 m | ~3,0 m |
| Cobertura 2º pav. | — | ~5,8 m (base telhado) | — |
| Cobertura frente/varanda | — | ~3,0 m | — |

---

## Nós principais

```
PensionCeiling/
  Ceiling_FirstFloor_Main
  Ceiling_SecondFloor_Main + segmentos
  Ceiling_StairBox_Main          — poço escada (12A)
  Ceiling_StairBox_*             — selagens laterais (12A)
  Ceiling_UpperSouthRoom         — placeholder porta verde (12A)

Exterior/
  Roof_Blockout_Main             — 2º pavimento (z ≤ -5,8)
  Roof_Blockout_LowerFront       — frente/varanda (y≈3,0)
  Shell_FacadeUpper_*            — massa superior fachada (12A)
  Shell_FacadeUpper_Parapet
```

---

## Regras congeladas

1. **Tetos são visual-only** — sem colisão (evita prender câmera/player).
2. **Vão da escada fechado no teto interno (12A)** — `Ceiling_StairBox_Main` em y=5,8; selagens laterais.
3. **Não duplicar** teto onde `Floor_Second_Main` já funciona como laje vista de baixo.
4. **Blockout ≠ final** — sem telha, textura, beiral ou GLB nesta fase.
5. **Não refazer layout** do 2º andar sem nova sprint aprovada.
6. **Não refazer estrutura de teto/cobertura** sem nova sprint aprovada — fechamento superior **aprovado** como baseline.

---

## Telhado final — sprint futura

O telhado/cobertura atual é **placeholder blockout**. Telhado final (duas águas, beiral, materiais) fica para sprint futura de **arte**, não para hotfix estrutural.

---

## Pendências futuras (arte / polish)

- Telhado final com duas águas / beiral.
- Texturas e materiais finais.
- Teto modular com clipping zero (Sprint 14 roadmap).
- Janelas / claraboia no poço da escada (opcional narrativo).
- Ajuste fino anti z-fighting nos encontros teto/parede.

---

## Playtest

- `docs/testing/PENSION_CEILING_BLOCKOUT_PLAYTEST.md`
- `docs/testing/PENSION_CEILING_HOTFIX_12A.md` — aprovação final
