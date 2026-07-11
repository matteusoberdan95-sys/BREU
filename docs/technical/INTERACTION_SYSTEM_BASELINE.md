# BREU — Sistema de Interação (Baseline)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 04 — aprovada e congelada  
**Status:** OFICIAL — não alterar sem nova sprint de interação

---

## Regra de congelamento

> **Não modificar** `IInteractable`, `Interactable`, `PlayerInteractionRaycast`, pipeline de raycast/prompt/mensagem ou `InteractionLab.tscn` **sem solicitação explícita do usuário** ou sprint dedicada de interação.

Baselines paralelas congeladas:
- Player: `PLAYER_CONTROLLER_BASELINE.md`
- HUD: `HUD_DEBUG_BASELINE.md`

Sprint 05+ (Pensão, puzzles) **criam novos interactables** consumindo este sistema — não reescrevem o core.

---

## Visão geral

Interação FPS via **raycast da câmera** + tecla **E**.

```
Camera InteractionRaycast (mask 3)
    → collider com CollisionShape3D
    → FindInteractable (árvore + grupo "interactable")
        → Interactable (PromptText, InteractionMessage)
            → HUD ShowInteractionPrompt / ShowMessage(3s)
```

**PlayerInteractionRaycast** — nó filho de `Player.tscn`; **não** altera movimentação.

| Export | Default |
|--------|---------|
| `InteractionDistance` | 3.0 m |
| `RaycastPath` | relativo ao **Player** → `HeadBase/.../Camera3D/InteractionRaycast` |
| `DebugMode` | log ao mudar alvo / ao pressionar E |

RayCast3D:
- `target_position = (0, 0, -3)`
- `collision_mask = 3` (World + Interactable)
- `CollideWithAreas = true`, `CollideWithBodies = true`, `Enabled = true`

---

## Collision layers

| Layer | Valor | Uso |
|-------|-------|-----|
| World | 1 | Chão, paredes, objetos físicos |
| Interactable | 2 | Áreas só de interação |
| World + Interactable | 3 | Físico + interativo (placa, porta) |

| Ator | Mask |
|------|------|
| Player | 1 (World) |
| InteractionRaycast | 3 |

---

## Como criar objeto interativo futuro

Checklist obrigatório:

1. **CollisionShape3D** no collider detectável pelo raycast (nunca só mesh)
2. Entrar no grupo **`interactable`** (automático via `Interactable._Ready` no host)
3. Filho `Node` com script **`Interactable.cs`**
4. Configurar no Inspector:
   - **`PromptText`** — ex.: `Ler placa` (HUD adiciona `[E]`)
   - **`InteractionMessage`** — texto exibido 3 s ao pressionar E
   - **`InteractionId`** — opcional, para puzzles/estado de level
   - **`OneShot`** — opcional, desativa após primeiro uso
5. Escolher layer conforme necessidade física:

| Caso | Estrutura | Layer |
|------|-----------|-------|
| Porta / placa (bloqueia + interage) | `StaticBody3D` + shape + `Interactable` | **3** |
| Livro / item pequeno (só interage) | `Area3D` + shape + `Interactable` | **2** |
| Parede / chão (só física) | `StaticBody3D` + shape | **1** |

### Exemplo — placa de emprego (Sprint 05+)

```
StaticBody3D: JobOfferSign
  collision_layer = 3
  collision_mask = 0
  CollisionShape3D
  MeshInstance3D
  Interactable
    PromptText = "Ler placa"
    InteractionMessage = "OFERTA DE TRABALHO - MINERAÇÃO - PENSÃO SANTA LUZIA."
    InteractionId = "job_offer_sign"
```

### Exemplo — bilhete / fusível (Sprint 07+)

```
Area3D ou StaticBody3D (layer 2 ou 3)
  CollisionShape3D
  Interactable
    PromptText = "Pegar bilhete"
    InteractionMessage = "..."
    InteractionId = "fuse_note"
    OneShot = true
```

---

## Arquivos do sistema

| Arquivo | Papel |
|---------|-------|
| `scripts/interactions/IInteractable.cs` | Interface |
| `scripts/interactions/Interactable.cs` | Componente |
| `scripts/interactions/PlayerInteractionRaycast.cs` | Raycast + input E |
| `scenes/test/InteractionLab.tscn` | Lab oficial aprovado |

---

## HUD (extensão aprovada)

| Método | Uso |
|--------|-----|
| `ShowInteractionPrompt(text)` | Exibe `[E] {text}` |
| `HideInteractionPrompt()` | Esconde prompt |
| `ShowMessage(text, 3f)` | Mensagem temporária inferior |

---

## Cena de teste aprovada

`res://scenes/test/InteractionLab.tscn`

| Objeto | Prompt | Mensagem |
|--------|--------|----------|
| TestSign | Ler placa | OFERTA DE TRABALHO - MINERAÇÃO - PENSÃO SANTA LUZIA. |
| TestBook | Examinar livro | Seu nome já está no registro. |
| TestLockedDoor | Tentar abrir porta | Está trancada. |

---

## Aprovação do usuário (2026-07-11)

- [x] InteractionLab funcionando
- [x] Prompt `[E]` ao mirar
- [x] E mostra mensagem correta
- [x] Placa, livro, porta trancada OK
- [x] HUD e movimentação intactos
- [x] Colisões básicas do lab OK

---

## Próximo uso

**Sprint 05–06:** interactables placeholder na Pensão térreo — **mesmo** `Interactable.cs` + `PlayerInteractionRaycast`.

**Sprint 07:** interações com **estado local da cena** (`PensaoPuzzleState`) — implementam `IInteractable` sem alterar o core:

| Exemplo | Comportamento |
|---------|---------------|
| Depósito trancado | Prompt/mensagem mudam com `HasDepositKey` |
| Depósito destrancado | Porta oculta + colisão off; prompt vazio |
| Chave / fusível | Pickup one-shot; props pequenos em `Area3D` |
| Bilhete | `Interactable` OneShot padrão |

**Regra:** novos puzzles de level usam scripts `IInteractable` dedicados ou `Interactable` — **não** modificar `PlayerInteractionRaycast` sem sprint de interação.
