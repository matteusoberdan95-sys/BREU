# BREU — Pensão Térreo Blockout (Baseline)

**Versão:** 1.5  
**Data:** 2026-07-11  
**Sprint:** 05–06 aprovada · **09A** escada integrada — **aprovada**  
**Status:** OFICIAL — térreo + escada integrada; patamar superior placeholder

---

## Regra de congelamento

> **Não modificar** planta, fluxo ou layout geral do térreo **sem solicitação explícita do usuário** ou sprint dedicada (ex.: Sprint 07 puzzle, Sprint 08 escada).

O térreo foi aprovado na Sprint 05 (blockout jogável) e validado na Sprint 06 (fine playtest + colisão móveis + depósito selado).

Baselines paralelas congeladas:
- Player: `PLAYER_CONTROLLER_BASELINE.md`
- HUD: `HUD_DEBUG_BASELINE.md`
- Interação: `INTERACTION_SYSTEM_BASELINE.md`

---

## Regra de colisão — móveis vs props

| Tipo | Colisão física | Interação |
|------|----------------|-----------|
| **Móveis grandes** (cama, balcão, bancada, porta bloqueada) | `StaticBody3D` + `BoxShape3D` | Porta depósito: layer 3 + `Interactable` |
| **Props pequenos** (livro, placa) | **Sem** colisão que trave player | `Area3D` + raycast (layer 2) |

Nós oficiais:
- `Furniture_Room102_Bed_Collision`
- `Furniture_Reception_Counter_Collision`
- `Furniture_Kitchen_Counter_Collision`
- `Door_Deposit_Blocked` / `Door_Deposit_Blocking_Collision`

---

## Cena oficial

| Item | Valor |
|------|-------|
| Cena | `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn` |
| Builder | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Builder.cs` |
| Controller | `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Controller.cs` |
| Playtest | `docs/testing/PENSAO_TERREO_BLOCKOUT_01_PLAYTEST.md` |

---

## Métricas aprovadas (Sprint 06)

| Métrica | Valor |
|---------|-------|
| Capsule player | raio 0,35 m · altura 1,8 m |
| Corredor | 2,4 m livres |
| Portas | 1,4 m × 2,3 m |
| Paredes | 0,2 m espessura · 3,0 m altura |
| Piso visual | 0,20 m espessura |
| Piso colisão | 3 lajes contínuas (overlap 0,08 m) |
| Shell edificação | ~14 m × ~44 m |
| Raycast interação | 3,0 m (baseline Sprint 04) |
| Interactables | 5 pontos fixos |

---

## Layout (congelado)

```
Trilha → Varanda → Recepção → Corredor → Quarto 102 / Cozinha → Depósito → Escada (álcove oeste) → Patamar superior temporário
```

**Sprint 09A — escada integrada (aprovada 2026-07-11):**
- `StairWell` no álcove oeste (entrada corredor z ≈ -25,5)
- Padrão `StairRampAssembly` — rampa invisível + 14 degraus visuais sem colisão
- `UpperLanding_Temporary` @ y = 2,8 m — **placeholder** (não é 2º andar)
- **Térreo preservado** — trilha, recepção, corredor, quartos, cozinha intactos
- **Puzzle preservado** — chave, depósito, fusível, bilhete funcionais
- Player / HUD / interação **não alterados** nos scripts aprovados
- **Sem** teto · **sem** segundo andar completo · **sem** arte final

**Playtest escada:** `docs/testing/PENSAO_STAIR_INTEGRATION_PLAYTEST.md` — **aprovado**

**Colisão:** `Exterior_MainGround`, `Porch_MainFloor`, `PensionGroundFloor_MainFloor`

**Sprint 06 (fine playtest + hotfix):**
- Placa reposicionada; áreas interactable ampliadas; luzes playtest
- `Floor_Corridor_Readability`
- Colisão móveis grandes; depósito selado (alcoves + moldura + fundo 14 m)

---

## Exterior — placeholder aceito

O lote/trilha externo é **blockout temporário** aprovado na Sprint 06. Não é arte final.

| Pendência | Sprint futura |
|-----------|---------------|
| Barranco / desnível | 07+ / terreno |
| Vegetação | 15+ |
| Terreno artístico | 11 / 15 |
| Atmosfera avançada | 11 |
| Arte modular | 15 |
| Teto | 10 |
| Escada integrada | **09A** |
| Segundo andar completo | 09 |
| Puzzle depósito (chave/fusível) | **07** |

**Regra:** barranco, arte, teto, escada e 2º andar **não entram** sem sprint dedicada.

---

## Critérios Sprint 06 — aprovados (2026-07-11)

- [x] Rota principal completa
- [x] Colisão validada (incl. móveis grandes)
- [x] 5 interações funcionais
- [x] HUD intacto
- [x] Movimento aprovado intacto
- [x] Sem limbos grandes internos
- [x] Depósito selado visualmente
- [x] Sem bugs bloqueantes

---

## Commits de referência

| Commit | Descrição |
|--------|-----------|
| `d08184b` | feat: add pension ground floor blockout |
| `408ada1` | fix: close ground floor gaps |
| `2553f2e` | fix: resolve floor z-fighting |
| `77ae1c1` | fix: seal visual gaps |
| `3f71a0b` | feat: approve pension ground floor blockout baseline |
| `dda1949` | fix: fine tune pension ground floor playtest issues |
| `3a93223` | fix: add furniture collisions and seal deposit blockout |
| _(Sprint 06)_ | test: approve pension ground floor fine playtest |
