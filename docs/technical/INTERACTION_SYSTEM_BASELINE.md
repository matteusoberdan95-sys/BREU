# BREU — Sistema de Interação (Baseline)

**Versão:** 1.0  
**Data:** 2026-07-11  
**Sprint:** 04 — sistema mínimo

---

## Visão geral

Interação em primeira pessoa via **raycast da câmera** + tecla **E**.

O sistema **não altera** movimentação, camera feel ou stamina. Apenas lê a mira e atualiza HUD.

```
Camera InteractionRaycast → collider (layer Interactable)
    → Interactable (IInteractable)
        → HUD prompt [E] ...
        → E pressionado → ShowMessage (3 s)
```

---

## Arquivos

| Arquivo | Papel |
|---------|-------|
| `scripts/interactions/IInteractable.cs` | Interface (`GetPromptText`, `Interact`) |
| `scripts/interactions/Interactable.cs` | Componente configurável no Inspector |
| `scripts/interactions/PlayerInteractionRaycast.cs` | Raycast + input E no Player |
| `scenes/test/InteractionLab.tscn` | Cena de teste oficial |

---

## IInteractable

```csharp
string GetPromptText();
void Interact(Node interactor);
```

---

## Interactable.cs

Filho de um corpo com colisão (`StaticBody3D` recomendado).

| Export | Descrição |
|--------|-----------|
| `PromptText` | Texto após `[E]` (ex.: `Ler placa`) |
| `InteractionMessage` | Mensagem HUD ao interagir |
| `OneShot` | Se true, desativa após primeiro uso |
| `InteractionId` | ID opcional para puzzles futuros |
| `HasBeenUsed` | Estado runtime (read-only) |

O host pai entra no grupo `"interactable"` em `_Ready`.

**Colisão recomendada:**
- `collision_layer = 2` (Interactable)
- `collision_mask = 0` — **não bloqueia o player** (player mask = World apenas)

---

## PlayerInteractionRaycast.cs

Nó filho de `Player.tscn` (não modifica `PlayerController`).

| Export | Default |
|--------|---------|
| `InteractionDistance` | `2.5` m |
| `DebugMode` | `false` — log no console só ao **mudar** alvo |

**RayCast3D** em `Camera3D/InteractionRaycast`:
- `target_position = (0, 0, -2.5)`
- `collision_mask = 2` (Interactable)

Fluxo:
1. `_PhysicsProcess` — atualiza mira
2. Encontrou interactable → `HUD.ShowInteractionPrompt`
3. Perdeu mira → `HUD.ClearInteractionPrompt`
4. `interact` (E) → `IInteractable.Interact(player)`

Debug log: `Looking at interactable: TestSign`

---

## HUD (extensão Sprint 04)

Baseline Sprint 03 preservado + **InteractionPromptPanel** (centro da tela).

| Método | Uso |
|--------|-----|
| `ShowInteractionPrompt(text)` | `[E] {text}` |
| `ClearInteractionPrompt()` | Esconde prompt |
| `ShowMessage(text, 3f)` | Toast inferior (já existia) |

---

## Como criar novo objeto interativo

1. Criar `StaticBody3D` (ou `Area3D`) com `CollisionShape3D` + mesh placeholder.
2. `collision_layer = 2`, `collision_mask = 0`.
3. Adicionar filho `Node` com script `Interactable.cs`.
4. Configurar `PromptText` e `InteractionMessage` no Inspector.
5. Opcional: `InteractionId` para estado de level futuro.

**Não** colocar lógica de movimento no interactable.

---

## Cena de teste

`res://scenes/test/InteractionLab.tscn`

| Objeto | Prompt | Mensagem |
|--------|--------|----------|
| TestSign | Ler placa | OFERTA DE TRABALHO - MINERAÇÃO - PENSÃO SANTA LUZIA. |
| TestBook | Examinar livro | Seu nome já está no registro. |
| TestLockedDoor | Tentar abrir porta | Está trancada. |

---

## Regras

- **Não** modificar `PlayerController`, `PlayerCameraFeel`, lean, look back, sprint/crouch/stamina.
- **Não** reescrever HUD base — apenas prompt/mensagem.
- Interação futura (portas, bilhete, fusível) **reutiliza** este pipeline.

---

## Próximo uso (Sprint 05+)

- Placa de emprego, livro de recepção, porta trancada na Pensão
- Puzzle depósito (Sprint 07) via `InteractionId` + level controller
