# Baseline — Escada com rampa invisível

**Versão:** 1.2  
**Sprint:** 08 (lab) + 09A (integração Pensão)  
**Data aprovação lab:** 2026-07-11  
**Data aprovação integração:** 2026-07-11  
**Status:** ✅ **Aprovada** — lab + Pensão — baseline congelada  
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

**Regra permanente:** manter o mesmo padrão em qualquer escada futura — degraus decorativos + rampa invisível.

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

## Integração na Pensão (Sprint 09A — aprovada)

| Item | Valor |
|------|-------|
| Local | **Álcove oeste** do depósito / corredor |
| Entrada | Porta no corredor oeste em **z ≈ -25,5** |
| Foot origin | **x ≈ -4,1**, **z ≈ -30,5** |
| Direção rampa | **+Z** (do fundo em direção ao corredor) |
| Patamar superior | `UpperLanding_Temporary` — **5 × 5 m** @ **y = 2,8 m** |
| Bloqueios topo | `UpperLanding_Blocker_Left/Right/Back` |
| Assembly | `StairRampAssembly.Build()` via `BuildStairIntegration()` |
| Luz | `StairWellLight` (OmniLight3D) |

**Plataforma superior:** temporária — placeholder até Sprint de segundo andar.

**Segundo andar completo:** sprint futura — **não** expandir patamar sem sprint dedicada.

**Playtest:** `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md` — **aprovado 2026-07-11**

---

## Rampa invisível

- Nó: `Stair_InvisibleRamp_Collision` (`StaticBody3D`)
- Shape: `BoxShape3D` rotacionado no eixo X
- Sem mesh visível
- Overlap nos patamares via `FloorOverlap` (0,12 m) e patch `Stair_Approach_Floor`

---

## Regras para segundo andar (Sprint futura)

1. **Não** colidir degraus visuais — manter padrão rampa + treads decorativos.
2. Manter largura ≥ **2,0 m** e inclinação ≤ **~27°** salvo playtest contrário.
3. Transição piso → rampa → piso superior **sem degrau vertical** nem buraco.
4. Guarda-corpo com colisão no patamar superior antes de abrir vão.
5. **Não** alterar `PlayerController` nem `PlayerCameraFeel` para “consertar” escada.
6. Substituir `UpperLanding_Temporary` por layout real em sprint de 2º andar.

---

## Nós principais (lab)

```
StairMovementLab
  World (StairMovementLabBuilder → StairRampAssembly)
    Stair_InvisibleRamp_Collision
    Stair_Step_01 … Stair_Step_14
    UpperLanding_Temporary (+ blockers)
```

## Nós principais (Pensão)

```
PensionGroundFloor/StairWell
  StairAssembly (foot @ x≈-4.1, z≈-30.5)
    Stair_InvisibleRamp_Collision
    Stair_Step_01 … Stair_Step_14
    UpperLanding_Temporary (+ blockers)
```

---

## Baselines congeladas (não alterar sem nova sprint)

- `PLAYER_CONTROLLER_BASELINE.md`
- `HUD_DEBUG_BASELINE.md`
- `INTERACTION_SYSTEM_BASELINE.md`
- `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md`
- **Este documento**

---

## Playtests

- Lab: `docs/testing/STAIR_MOVEMENT_LAB_PLAYTEST.md` — aprovado 2026-07-11
- Pensão: `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md` — aprovado 2026-07-11
