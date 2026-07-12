# Métricas de nível — Pensão Santa Luzia

**Sprint:** 18B  
**Cena:** `PensaoVerticalBlockout01.tscn`

## Alturas (metros)

| Métrica | Valor | Marker |
|---------|------:|--------|
| Piso do térreo (topo) | `0.00` | — |
| Altura de parede térreo | `3.00` | — |
| Topo do forro do térreo (selo) | `≈ 2.79` | `Marker_FirstFloor_CeilingHeight` |
| Espessura do forro do térreo | `0.38` | — |
| Topo do piso do 2º andar | `2.80` | `Marker_SecondFloor_FloorHeight` |
| Espessura da laje do 2º | `0.25` (colisão) / `0.20–0.30` (visuais) | — |
| Ala superior (mesmo piso) | `2.80` | `Marker_UpperWing_FloorHeight` |
| Underside do teto do 2º | `5.80` | — |

## Regra de não-invasão

Nenhum mesh/collider do **segundo andar** ou **ala superior** pode aparecer no volume jogável do térreo ao olhar para cima.

Consequência prática (18B):
- o forro do térreo (`Ceiling_FirstFloor_Seal`) fecha **abaixo** do topo do piso superior;
- o player no térreo deve ver forro opaco, não lajes/paredes superiores.

## Circulação mínima

| Elemento | Mínimo |
|----------|-------:|
| Largura de corredor | `2.0` m (alvo atual ≈ `2.4`) |
| Largura de porta | `1.2` m (alvo atual ≈ `1.4`) |
| Altura de porta | `2.1` m (alvo atual ≈ `2.3`) |
| Espessura de piso caminhável | `0.20`–`0.40` m |

## Áreas de interação

- Um único `Area3D` por porta/prop.
- Eixo horizontal típico ≤ `1.2` m.
- Sem atravessar parede.
- Prompt específico, não genérico duplicado.
