# Playtest — Pensão Térreo Blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 05 — **✅ APROVADA** (2026-07-11)  
**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md`

---

## Resultado — Aprovado pelo usuário

| Área | Status |
|------|--------|
| Navegação exterior (trilha → pensão) | ✅ |
| Entrada / varanda | ✅ |
| Recepção | ✅ |
| Corredor | ✅ |
| Quarto 102 | ✅ |
| Cozinha | ✅ |
| Depósito (bloqueio + interação) | ✅ |
| Interações (5 pontos) | ✅ |
| HUD / debug | ✅ |
| Ausência de quedas / limbo grandes | ✅ |

**Observação:** exterior permanece lote simples — aceito como blockout temporário.

---

## Fluxo validado

Trilha → Varanda → Recepção → Corredor → Quarto 102 / Cozinha → Depósito trancado

---

## Checklist — Movimentação preservada

| Teste | OK |
|-------|-----|
| W/S/A/D corretos | ✅ |
| Sprint | ✅ |
| Crouch | ✅ |
| Lean Q/R | ✅ |
| Look back | ✅ |
| Camera feel | ✅ |

---

## Checklist — Visual sealing (hotfix 2)

| Teste | OK |
|-------|-----|
| Chão externo não mostra limbo nas laterais imediatas | ✅ |
| Entrada da pensão sem fresta grande | ✅ |
| Piso interno cobre recepção / corredor / quarto / cozinha / depósito | ✅ |
| Sem frestas grandes entre piso e parede | ✅ |
| Paredes internas parecem fechadas | ✅ |
| Cômodos não mostram vazio/limbo pelas laterais | ✅ |
| Corredor sem buraco lateral | ✅ |
| Não existe z-fighting visível | ✅ |
| Gameplay / colisão / interação intactos | ✅ |

---

## Checklist — Navegação / colisão

| Teste | OK |
|-------|-----|
| Trilha → varanda sem queda | ✅ |
| Varanda → recepção sem queda | ✅ |
| Recepção / corredor / quarto / cozinha / depósito sem queda | ✅ |
| Pular (Space) nas transições — não cai | ✅ |
| Sem frestas grandes no chão | ✅ |
| Player nasce na trilha (z≈45) | ✅ |
| Anda até varanda | ✅ |
| Não cai do mapa | ✅ |
| Limites invisíveis OK | ✅ |
| Placa — prompt + E | ✅ |

---

## Checklist — Entrada / Recepção

| Teste | OK |
|-------|-----|
| Varanda acessível | ✅ |
| Porta principal livre | ✅ |
| Recepção circulável | ✅ |
| Balcão não bloqueia centro | ✅ |
| Livro — prompt + E | ✅ |

---

## Checklist — Corredor / Cômodos

| Teste | OK |
|-------|-----|
| Corredor ≥2.2m confortável | ✅ |
| Quarto 102 — entrar + virar câmera | ✅ |
| Quarto 102 — interação | ✅ |
| Cozinha — entrar + virar câmera | ✅ |
| Cozinha — interação | ✅ |
| Depósito — porta bloqueia | ✅ |
| Depósito — prompt + E | ✅ |

---

## Checklist — HUD / Debug

| Teste | OK |
|-------|-----|
| Vida / Stamina / Lanterna | ✅ |
| Prompt interação | ✅ |
| Mensagens | ✅ |
| F10 / F11 | ✅ |
| Sem caixa vazia | ✅ |

---

## Critério gate — ATINGIDO

**Térreo 100% navegável** — fluxo completo aprovado. Sprint 05 fechada.

**Próxima:** Sprint 06 — playtest e correção fina do térreo.

---

## Histórico de hotfixes

### Hotfix 2 — Visual Sealing Pass

| Problema | Correção |
|----------|----------|
| Limbo visível | Shell externo + paredes internas fechadas |
| Paredes soltas | Quarto/cozinha/corredor estendidos |
| Entrada/varanda | Laterais + fundo + cantos |
| Exterior/trilha | Base expandida + bermas |
| Piso fino | Lajes visuais 0,20 m |

### Hotfix 1 — Visual Blockout Clean

| Problema | Correção |
|----------|----------|
| Z-fighting | Pisos com Y distintos; trilha elevada |
| Pisos coplanares | `Floor_PensionGround_Main_Visual` único |
| Fresta piso/parede | Paredes embutidas + soleiras |

### Hotfix chão — Colisão contínua

| Problema | Correção |
|----------|----------|
| Vãos entre pisos | 3 lajes contínuas com overlap 0,08 m |
| Player caía ao pular | Topo colisão Y=0 em toda área |

**Lajes de colisão:** `Exterior_MainGround`, `Porch_MainFloor`, `PensionGroundFloor_MainFloor`

---

## Log

| Data | Nota |
|------|------|
| 2026-07-11 | **Sprint 05 aprovada** — térreo blockout jogável baseline congelada |
| 2026-07-11 | Hotfix 2 visual sealing aplicado |
| 2026-07-11 | Hotfix 1 visual de blockout aplicado |
