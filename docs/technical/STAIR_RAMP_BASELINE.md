# Baseline — Escada com rampa invisível

**Versão:** 1.1  
**Sprint:** 08 (lab) + 09A (integração Pensão)  
**Data aprovação lab:** 2026-07-11  
**Status lab:** ✅ Aprovada — baseline congelada  
**Status integração Pensão:** 🔄 Implementada — playtest F6 pendente  
**Cena lab:** `res://scenes/test/StairMovementLab.tscn`  
**Cena Pensão:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`

---

## Princípio aprovado

| Camada | Função |
|--------|--------|
| **Degraus visuais** | Leitura espacial apenas — `MeshInstance3D`, **sem colisão** |
| **Rampa invisível** | Navegação FPS — `StaticBody3D` + `BoxShape3D` inclinado |
| **Pisos** | Colisão plana nos patamares inferior e superior |

O player **nunca** colide com degraus individuais. Toda subida/descida usa a rampa.

**Regra permanente:** ao integrar na Pensão, **manter o mesmo padrão** — degraus visuais decorativos + rampa invisível para navegação.

---

## Métricas aprovadas

| Parâmetro | Valor |
|-----------|-------|
| Altura total (`StairRise`) | **2,8 m** |
| Comprimento horizontal (`StairRun`) | **5,8 m** |
| Largura (`StairWidth`) | **2,2 m** |
| Degraus visuais | **14** |
| Altura visual por degrau | **0,20 m** |
| Profundidade visual por degrau | **~0,414 m** |
| Inclinação da rampa | **~25,8°** (`atan2(2,8 / 5,8)`) |
| Espessura rampa (colisão) | **0,22 m** |
| Comprimento inclinado da rampa | **~6,44 m** |

Constantes em `StairRampAssembly.cs` (compartilhado lab + Pensão).

---

## Integração na Pensão (Sprint 09A)

| Item | Valor |
|------|-------|
| Local | **Álcove oeste** do depósito / corredor |
| Entrada | Porta no corredor oeste em **z ≈ -25,5** (`Wall_Corridor_Left` gap) |
| Foot origin | **x ≈ -4,1**, **z ≈ -30,5** |
| Direção rampa | **+Z** (do fundo em direção ao corredor) |
| Patamar superior | `UpperLanding_Temporary` — **5 × 5 m** @ **y = 2,8 m** |
| Bloqueios topo | `UpperLanding_Blocker_Left/Right/Back` |
| Assembly | `StairRampAssembly.Build()` via `BuildStairIntegration()` |
| Removido | `Wall_StairFuture_Blocker`, `Wall_Deposit_AlcoveWest`, `Wall_Deposit_AlcoveSouthCapWest` |
| Luz | `StairWellLight` (OmniLight3D) |

**Segundo andar completo:** não criado — patamar é temporário.

**Playtest integração:** `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md`

```
StairMovementLab
  World (StairMovementLabBuilder)
    LowerFloor
      LowerFloor_Main          — colisão piso inferior
      LowerFloor_StairApproach — transição suave → rampa
    Collisions
      Stair_InvisibleRamp_Collision
    VisualSteps
      Stair_Step_01 … Stair_Step_14
    UpperFloor
      UpperFloor_Main
      UpperFloor_Rail_Left / Right / BackWall
    StairTest
      Stair_Guide_Left / Right (laterais baixas)
  PlayerSpawn
  Player
  HUD
```

---

## Rampa invisível

- Nó: `Stair_InvisibleRamp_Collision` (`StaticBody3D`)
- Shape: `BoxShape3D` rotacionado no eixo X
- Sem mesh visível
- Overlap nos patamares via `FloorOverlap` (0,12 m) e patch `LowerFloor_StairApproach`
- Piso superior estende-se até `z ≈ 5,68` para encostar no topo da rampa

---

## Regras para segundo andar (Sprint futura)

1. **Não** colidir degraus visuais — manter padrão rampa + treads decorativos.
2. Manter largura ≥ **2,0 m** e inclinação ≤ **~27°** salvo playtest contrário.
3. Transição piso → rampa → piso superior **sem degrau vertical** nem buraco.
4. Guarda-corpo com colisão no patamar superior antes de abrir vão.
5. **Não** alterar `PlayerController` nem `PlayerCameraFeel` para “consertar” escada.

---

## Nós principais (lab)
