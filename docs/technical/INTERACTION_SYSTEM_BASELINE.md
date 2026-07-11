# BREU — Sistema de Interação (Baseline)

**Versão:** 1.1  
**Data:** 2026-07-11  
**Sprint:** 04 — sistema mínimo + hotfix colisão/raycast

---

## Visão geral

Interação em primeira pessoa via **raycast da câmera** + tecla **E**.

O sistema **não altera** movimentação, camera feel ou stamina. Apenas lê a mira e atualiza HUD.

```
Camera InteractionRaycast → collider (World e/ou Interactable)
    → FindInteractable (parent/children/grupo)
        → Interactable (IInteractable)
            → HUD prompt [E] ...
            → E → ShowMessage (3 s)
```

---

## Collision layers

| Layer | Valor | Uso |
|-------|-------|-----|
| **World** | 1 | Chão, paredes, porta/mesa/sign físicos |
| **Interactable** | 2 | Áreas de interação (ex.: livro) |
| **World + Interactable** | 3 | Objeto físico **e** interativo (placa, porta) |

| Ator | Layer | Mask |
|------|-------|------|
| Player | 16 (Player) | **1** (World) |
| InteractionRaycast | — | **3** (World + Interactable) |

Player **não** colide com layer 2 pura — livro usa `Area3D` layer 2.

---

## Arquivos

| Arquivo | Papel |
|---------|-------|
| `scripts/interactions/IInteractable.cs` | Interface |
| `scripts/interactions/Interactable.cs` | Componente Inspector |
| `scripts/interactions/PlayerInteractionRaycast.cs` | Raycast + E |
| `scenes/test/InteractionLab.tscn` | Lab oficial |

---

## Estrutura correta de objeto interativo

### Físico + interativo (porta, placa)

```
StaticBody3D: TestLockedDoor
  collision_layer = 3
  collision_mask = 0
  CollisionShape3D          ← obrigatório
  MeshInstance3D
  Interactable (Node)       ← script aqui ou detectável na árvore
```

### Só interação (livro pequeno)

```
Node3D: TestBook
  MeshInstance3D
  Area3D: InteractionArea
    collision_layer = 2
    CollisionShape3D
    Interactable (Node)
```

### Mundo sem interação (parede, mesa)

```
StaticBody3D
  collision_layer = 1
  CollisionShape3D
  MeshInstance3D
```

**Regras:**
- Todo interactable precisa `CollisionShape3D` no collider detectável
- Host entra no grupo `"interactable"`
- `Interactable.cs` como filho do collider ou do root

---

## PlayerInteractionRaycast.cs

Nó filho de `Player` — **RaycastPath é relativo ao Player**, não ao script.

| Export | Default |
|--------|---------|
| `InteractionDistance` | `3.0` m |
| `DebugMode` | log ao mudar alvo + ao pressionar E |

**RayCast3D** (`Camera3D/InteractionRaycast`):
- `target_position = (0, 0, -3)`
- `collision_mask = 3`
- `CollideWithAreas = true`
- `CollideWithBodies = true`
- `Enabled = true`

### FindInteractable

Ao acertar collider, busca `IInteractable`:
1. No próprio nó
2. Em filhos
3. Subindo pais (até 8 níveis)
4. Em nós do grupo `"interactable"` com filho `Interactable`

---

## HUD (extensão Sprint 04)

| Método | Uso |
|--------|-----|
| `ShowInteractionPrompt(text)` | `[E] {text}` — evita duplicar `[E]` |
| `HideInteractionPrompt()` | Esconde prompt |
| `ShowMessage(text, 3f)` | Painel inferior — **oculto** sem mensagem |

---

## Hotfix Sprint 04 (2026-07-11)

| Bug | Fix |
|-----|-----|
| Raycast null | Path resolvido via `GetParent()` (Player) |
| Sem prompt/mensagem | Raycast funcional + HUD panel visibility |
| Atravessar paredes | Paredes layer 1; objetos físicos layer 3 |
| Caixa HUD vazia | `MessagePanel.visible = false` por default |

---

## Regras

- **Não** modificar `PlayerController`, `PlayerCameraFeel`, movimento congelado.
- **Não** reescrever HUD base — só prompt/mensagem.
- Interação **não** deve alterar player movement.

---

## Cena de teste

`res://scenes/test/InteractionLab.tscn` — chão, 4 paredes, 3 interactables.

**Sprint 04 aprovada somente após playtest desta cena.**
