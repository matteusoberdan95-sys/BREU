# Baseline — Portas blockout (Pensão)

**Versão:** 1.2  
**Sprint:** 14E  
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

## Baseline 2.0 — Sprint 14C

O sistema anterior foi substituído por três cenas reutilizáveis em `scenes/props/doors/`:

| Cena | Contrato |
|---|---|
| `DoorFrameOpen.tscn` | Moldura, fechamento superior e folha aberta estática sem colisão bloqueadora |
| `DoorLocked.tscn` | Painel opaco fixo, colisão alinhada e interação local |
| `DoorUnlockHidePanel.tscn` | Ao destrancar, somente oculta o painel e desativa a colisão |

Uma única instância é permitida por vão. A folha aberta é uma malha estática já posicionada junto à parede; não há animação, movimento, escala em runtime ou colisão bloqueando a passagem.

### Ajustes cirúrgicos 14D

- Entrada principal: somente moldura, sem folha decorativa, painel ou blocker.
- Folhas abertas opcionais: `0,06 m` de espessura e afastamento adicional de `0,08 m` do plano da parede.
- Painéis fechados: visual e collider alinhados em `Z = -0,06 m`, fora do plano central da parede.
- Porta verde oficial: `Door_UpperBalcony_Locked`.
- Porta do depósito oculta imediatamente o prompt ao destrancar.

### Ajustes cirúrgicos 14E

- `Sign_PensaoSantaLuzia`: placa da entrada centralizada, rotação 0°, offset 0,055 m da fachada.
- `ConfigureLockedDoor`: moldura a +0,05 m; painel a -0,08 m; interação a -0,34 m.
- `ConfigureOpenDoor`: moldura a +0,05 m; folha decorativa opcional a +0,12 m adicional.
- Porta verde: somente `Door_UpperBalcony_Locked` no vão do corredor superior — sem duplicata na fachada da trilha.
- Corredor inútil do 2º andar fechado com `UpperStair_BackClosureWall`, `UpperLanding_BackSeal`, `UpperStair_NorthEastSeal`.
- Folhas decorativas instáveis: remover (`decorativeLeaf: false`) em vez de animar.

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
- Exterior (trilha): `UpperBalcony_TrailReadability` — piso e guarda-corpo apenas (sem painel verde duplicado)

---

## Helpers (builder)

| Método | Uso |
|--------|-----|
| `DoorFrameOpen.tscn` | Moldura de passagem aberta |
| `DoorLocked.tscn` | Porta trancada opaca + interação |
| `DoorUnlockHidePanel.tscn` | Porta do depósito ligada ao puzzle |
| `ConfigureOpenDoor` / `ConfigureLockedDoor` | Afastamento anti z-fighting |

---

## Playtest

`docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seções Sprint 14A e 14B
