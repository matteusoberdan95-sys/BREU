# Playtest — Pensão Térreo Blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 05 ✅ + 06 🔧 (hotfix colisão/depósito)  
**Baseline:** `docs/technical/PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` (v1.2)

---

## Sprint 06 Hotfix — Furniture Collision and Deposit Sealing

| Área | Resultado | Problema | Correção | Status |
|------|-----------|----------|----------|--------|
| Quarto 102 | ✅ | Cama atravessável | `Furniture_Room102_Bed_Collision` | ✅ |
| Recepção | ✅ | Balcão atravessável | `Furniture_Reception_Counter_Collision` | ✅ |
| Cozinha | ✅ | Bancada atravessável | `Furniture_Kitchen_Counter_Collision` | ✅ |
| Depósito — porta | ✅ | Colisão OK; renomeada | `Door_Deposit_Blocked` + `Door_Deposit_Blocking_Collision` | ✅ |
| Depósito — paredes | ✅ | Frestas laterais; limbo visível | Alcoves + moldura + fundo largo | ✅ |
| Interações | ✅ | — | Area3D pequenos intactos; porta layer 3 | ✅ |
| Navegação | ✅ | — | Passagens principais livres | ✅ |

### Checklist hotfix

| Teste | OK |
|-------|-----|
| Player NÃO atravessa cama | ✅ |
| Player NÃO atravessa balcão recepção | ✅ |
| Player NÃO atravessa bancada cozinha | ✅ |
| Player NÃO atravessa porta depósito | ✅ |
| Prompt depósito + mensagem "Está trancado." | ✅ |
| Paredes depósito fechadas (sem limbo lateral) | ✅ |
| HUD / movimento / interação regressão | ✅ |

---

## Sprint 06 — Fine Playtest

**Rota testada:** PlayerSpawn → trilha → placa → varanda → recepção → livro → corredor → quarto 102 → cozinha → depósito → retorno → saída.

| Área | Resultado | Problema encontrado | Correção aplicada | Status |
|------|-----------|---------------------|-------------------|--------|
| Trilha | ✅ | Placa lateral demais (x=2,8); difícil mirar da trilha | Placa movida para (1,15; 37,5) | ✅ |
| Entrada | ✅ | — | — | ✅ |
| Recepção | ✅ | Balcão atravessável (hotfix) | Colisão `Furniture_Reception_Counter_Collision` | ✅ |
| Corredor | ✅ | Leitura fraca vs recepção | Piso corredor mais escuro (+0,015 m Y) | ✅ |
| Quarto 102 | ✅ | Cama atravessável (hotfix) | Colisão cama + área interação ampliada | ✅ |
| Cozinha | ✅ | Bancada atravessável (hotfix) | Colisão bancada + volume interação | ✅ |
| Depósito | ✅ | Frestas visuais + limbo lateral (hotfix) | Alcoves, moldura, fundo 14 m | ✅ |
| HUD | ✅ | — | — | ✅ |
| Interação | ✅ | Livro/quarto/cozinha com hitbox pequena | Áreas ampliadas (sem mudar prompts) | ✅ |
| Colisão | ✅ | Móveis grandes sem colisão (hotfix) | `FurnitureCollisions` + depósito selado | ✅ |
| Limites externos | ✅ | — | Boundaries x=±22, z=52/-35 intactos | ✅ |

**Regressão Sprints 02–05:** movimento, HUD, interação e layout geral intactos.

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
| 2026-07-11 | Hotfix: colisão móveis + vedação depósito |
| 2026-07-11 | Sprint 06 fine playtest — ajustes mínimos |
| 2026-07-11 | **Sprint 05 aprovada** — térreo blockout jogável baseline congelada |
| 2026-07-11 | Hotfix 2 visual sealing aplicado |
| 2026-07-11 | Hotfix 1 visual de blockout aplicado |
