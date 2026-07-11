# Baseline — Portas blockout (Pensão)

**Versão:** 1.1  
**Sprint:** 14B  
**Data:** 2026-07-11  
**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

---

## Padrões

### Tipo A — Porta aberta
- **Somente moldura** (`Door_*_Frame`)
- Sem painel no vão
- Sem colisão bloqueando passagem
- Player passa livremente

### Tipo B — Porta trancada
- Painel **opaco** (`_matDoor` ou `_matDoorBalcony`)
- `StaticBody3D` com `CollisionLayer = WorldLayer` (1)
- `CollisionShape3D` com espessura ≥ `WallThickness`
- `Area3D` pequena na frente do painel (`*_InteractArea`)
- Sem animação, transparência, scale ou tremor

### Tipo C — Depósito (destravável)
```
Door_Deposit/
  Door_Deposit_Frame
  Door_Deposit_Panel          — some ao destrancar
  Door_Deposit_Blocking/
    Door_Deposit_Collision    — desativa ao destrancar
  Door_Deposit_InteractArea   — Area3D local
  DepositDoorInteraction
```

**Ao destrancar:**
- `Door_Deposit_Panel.Visible = false`
- `Door_Deposit_Collision.Disabled = true`
- `Door_Deposit_InteractArea.Monitorable = false`
- Moldura permanece

---

## Proibido (blockout)

- Animar porta com `Scale`
- Material transparente em painel de porta
- Porta duplicada no mesmo vão (moldura + folha + painel)
- Folha aberta (`Door_*_Leaf`) — removida na 14B
- `AddInteractableBody` com `_matInteractable` para portas
- Colisão fina (< 0,2 m) em porta fechada
- Painel invisível com colisão ativa (ou vice-versa)

---

## Porta verde / varanda

- Nó: `Door_UpperBalcony_Locked`
- Painel verde opaco (`_matDoorBalcony`)
- Colisão ativa, não atravessável
- Prompt: **Tentar abrir varanda**
- Interior: `UpperBalcony_Placeholder` + guarda-corpos
- Exterior (trilha): `UpperBalcony_TrailReadability`

---

## Helpers (builder)

| Método | Uso |
|--------|-----|
| `AddDoorFrameInZWallLocal` / `AddDoorFrameInXWallLocal` | Moldura porta aberta |
| `AddLockedDoorPanelZWall` | Porta trancada opaca + interação |
| `BuildDepositDoorAssembly` | Depósito puzzle |

---

## Playtest

`docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seções Sprint 14A e 14B
