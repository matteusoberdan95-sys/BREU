# Sprint 18A — Laje sólida + expansão da ala superior

**Cena:** `PensaoVerticalBlockout01.tscn`  
**Fontes:** `areas/BalconyWing.tscn` + `areas/UpperWingExpansion.tscn`

## Problema

A mureta frontal foi removida, mas o espaço liberado não tinha piso/colisão confiável — o player caía para o térreo.

## Laje sólida

| Node | Função |
|------|--------|
| `UpperWing_SolidFloor` | Laje principal (mesh + `StaticBody3D`, layer 1, espessura 0.30) |
| `UpperWing_ConnectorFloor` | Liga a varanda/porta verde à laje |
| `UpperWing_FreeWalkableFloor` | Reforço na `BalconyWing` (mesmo nível, layer 1) |
| `UpperBalcony_FrontWalkway` | Faixa frontal de varanda com guarda-corpo só na borda externa |

Markers de teste:
- `Marker_UpperFloor_Start`
- `Marker_UpperFloor_Middle`
- `Marker_UpperFloor_End`

Topo alinhado a Y≈2.80 (piso do 2º andar).

## Cômodos

| Node | Conteúdo |
|------|----------|
| `Room_204` | Cama, criado, armário, marcas; interações cama/marcas |
| `Room_UpperSharedBathroom` | Pia, vaso, espelho (reflexo atrasado) |
| `Room_LinenCloset` | Lençóis → segundo fusível (`HasUpperFuse`) |
| `Room_UpperGeneratorPanel` | Painel → instala fusível (`IsUpperPowerRestored`) |
| `Door_Room205_Blocked` | Porta bloqueada (não abre) |
| `Door_Room203_Blocked` | Mantido; reage após energia superior |

## Puzzle do segundo fusível

```
Rouparia → HasUpperFuse
  → Painel superior → IsUpperPowerRestored
  → luzes piscam + arranhão
  → Quarto 203: "Algo bate..." / "Eu não estou sozinho aqui."
```

203 **não abre** nesta sprint.

## Preservado

Porta verde, banheiro/proprietária da `BalconyWing`, Quarto 203, puzzle térreo, player/HUD/áudio/fog.
