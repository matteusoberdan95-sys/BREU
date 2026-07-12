# Métricas de nível — Pensão Santa Luzia

**Sprint:** 18C  
**Cena:** `PensaoVerticalBlockout01.tscn`

## Alturas (metros)

| Métrica | Valor | Marker |
|---------|------:|--------|
| Piso do térreo | `0.00` | `Marker_FirstFloor_FloorY` |
| Topo do forro do térreo | `≈ 2.79` | `Marker_FirstFloor_CeilingY` |
| Espessura do forro do térreo | `0.38` | — |
| Topo do piso do 2º andar | `2.80` | `Marker_SecondFloor_FloorY` |
| Espessura da laje do 2º | `0.25`–`0.30` | — |
| Underside do teto do 2º | `5.80` | `Marker_SecondFloor_CeilingY` |
| Ala superior (mesmo piso) | `2.80` | `Marker_UpperWing_FloorY` |

## Regra de não-invasão

Nenhum mesh/collider do segundo andar ou da ala superior pode aparecer no volume jogável do térreo (abaixo do forro) ao olhar para cima.

`Ceiling_FirstFloor_Seal` + `Ceiling_Reception_Soffit` fecham entrada e recepção.

## Circulação mínima

| Elemento | Mínimo |
|----------|-------:|
| Largura de corredor | `2.0` m |
| Largura de porta | `1.2` m |
| Altura de porta | `2.1` m |
| Espessura de piso caminhável | `0.20`–`0.40` m |

## Interações

- Um `Area3D` por objeto
- Eixo horizontal típico ≤ `1.2` m
- Sem atravessar parede
