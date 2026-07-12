# Baseline — Portas blockout (Pensão)

**Versão:** 2.0  
**Sprint:** 14Z  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Padrão 14Z — reset destrutivo

Objetivo: **estabilidade sem flicker**. Não ajustar meshes bugados — remover.

### Tipo A — Porta aberta
- **Somente vão limpo** na parede (sem moldura, sem painel, sem folha)
- `Header_*` acima da abertura fecha o buraco até o teto
- Sem colisão bloqueando passagem

### Tipo B — Porta trancada (blocker)
- Bloco opaco inline (`AddLockedDoorBlocker` / `AddDepositDoorBlocker`)
- **Sem prefab**, **sem moldura**, **sem** `DoorFrameOpen.tscn`
- Painel nomeado `*_Blocker`, offset `Z = -0,08 m`
- Colisão alinhada ao painel (`BlockerDepth = 0,2 m`)

### Tipo C — Depósito (destravável)
```
Door_Deposit/
  Door_Deposit_Blocker     — some ao destrancar
  BlockingBody/BlockingShape
  InteractionArea
  BlockoutUnlockHideDoor
```

**Ao destrancar:**
- `Door_Deposit_Blocker.Visible = false`
- `BlockingShape.Disabled = true`
- `InteractionArea.Monitorable = false`

---

## Blockers permitidos (somente 3)

| Nó | Uso |
|----|-----|
| `Door_Deposit_Blocker` | Depósito — puzzle chave |
| `Door_UpperBalcony_Blocker` | Varanda verde — bloqueia |
| `Door_UpperBlocked_Blocker` | Porta superior — bloqueia |

Nenhum outro mesh de porta/painel/moldura/placa é gerado em runtime.

---

## Headers de vão aberto

| Nó | Local |
|----|-------|
| `Header_Entrance_Main` | Fachada principal |
| `Header_Room102` | Corredor → quarto 102 |
| `Header_Kitchen` | Corredor → cozinha |
| `Header_Room201` | 2º andar → quarto 201 |
| `Header_Room202` | 2º andar → quarto 202 |

Altura do header: `WallHeight - DoorHeight` (0,7 m), encaixado nas laterais do vão.

---

## Placas

**Removidas temporariamente** na 14Z (inclui `Sign_Pensao_Main_Exterior`). Arte de placa fica para sprint futura.

---

## Porta verde / varanda

- Nó: `Door_UpperBalcony` + `Door_UpperBalcony_Blocker`
- Painel verde opaco (`_matDoorBalcony`), metade esquerda do vão
- Prompt: **Tentar abrir varanda**

## Porta bloqueada superior

- Nó: `Door_UpperBlocked` + `Door_UpperBlocked_Blocker`
- Painel opaco (`_matDoor`), metade direita do vão
- Prompt: **Tentar abrir porta**

---

## Proibido (blockout)

- Molduras decorativas (`Door_*_Frame`, `DoorFrameOpen`)
- Placas (`Sign_*`, `Placa_*`)
- Folhas abertas, infill, painéis decorativos
- Prefabs de porta com moldura embutida
- Material transparente em painel de porta
- Duas superfícies coplanares no mesmo vão
- Animar porta com `Scale` ou tremor

---

## Helpers (builder)

| Método | Uso |
|--------|-----|
| `AddDoorHeaderXWall` / `AddDoorHeaderZWall` | Fecha vão acima da porta |
| `AddLockedDoorBlocker` | Porta trancada inline |
| `AddDepositDoorBlocker` | Depósito + puzzle |
| `BuildWallWithDoorGap` | Parede com vão + header opcional |

---

## Playtest

`docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14Z
