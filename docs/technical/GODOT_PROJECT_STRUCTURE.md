# BREU — Estrutura do Projeto Godot

**Versão:** 1.0  
**Data:** 2026-07-11  
**Engine:** Godot 4.7 + C# (.NET)

---

## Princípios

1. **Uma cena oficial** por fase — declarada em `PROJECT_STATE.md`.
2. **Colisão manual** em levels — nunca delegada a GLB importado.
3. **Scripts por domínio** — player não conhece level específico.
4. **Testes isolados** em `scenes/test/` — nunca poluir cena oficial.
5. **Docs junto do código** — sprint alterou gameplay → atualizar PROJECT_STATE.

---

## Árvore de pastas (alvo)

```
BREU/
├── project.godot
├── BREU.csproj
├── BREU.sln
├── GlobalUsings.cs
├── icon.svg
│
├── scenes/
│   ├── player/
│   │   └── Player.tscn
│   ├── ui/
│   │   ├── HUD.tscn
│   │   ├── DamageOverlay.tscn
│   │   └── DeathScreen.tscn
│   ├── levels/
│   │   ├── BootstrapEmpty.tscn          # Sprint 01 — até ter cena real
│   │   └── pensao_santa_luzia/
│   │       └── PensaoTerreo_Blockout_v1.tscn   # Sprint 05+ (nome final TBD)
│   ├── system/
│   │   ├── SceneTransition.tscn
│   │   └── CheckpointManager.tscn
│   └── test/
│       ├── PlayerSandbox.tscn
│       └── StairMovementLab.tscn
│
├── scripts/
│   ├── player/
│   ├── ui/
│   ├── interaction/
│   ├── levels/
│   │   └── pensao_santa_luzia/
│   ├── shared/              # helpers reutilizáveis (BlockoutSolid, etc.)
│   ├── debug/
│   ├── audio/
│   ├── fx/
│   ├── system/
│   └── enemies/             # Sprint 13+
│
├── assets/
│   ├── materials/
│   │   └── blockout/        # Sprint 05 — mats cinza
│   ├── audio/
│   ├── textures/
│   ├── models/              # Sprint 15+ — GLB visual only
│   └── blender/             # Sprint 15+ — fonte, não runtime obrigatório
│
├── materials/               # legado OK se organizado; preferir assets/materials
├── shaders/
│   └── fx/
├── resources/
│   └── weapons/             # Sprint 14+
│
└── docs/
    ├── agents/
    ├── production/
    ├── testing/
    ├── technical/
    ├── visual/
    ├── gameplay/
    └── design/
```

---

## Convenções de nomenclatura

| Tipo | Padrão | Exemplo |
|------|--------|---------|
| Cena level | `{Local}_{Fase}_{Versao}.tscn` | `PensaoTerreo_Blockout_v1.tscn` |
| Script builder | `{Local}{Fase}Builder.cs` | `PensaoTerreoBuilder.cs` |
| Colisão estática | `{Zona}Collision` | `InteriorFloorCollision` |
| Interactable | `{Objeto}Interactable` ou Area na cena | `DepositInteractable` |
| Test lab | `{Feature}MovementLab.tscn` | `StairMovementLab.tscn` |
| Branch | `reboot/s{NN}-{slug}` | `reboot/s05-pensao-terreo` |

---

## Cena tipo — Level blockout

```
LevelRoot (Node3D)
├── PlayerSpawnResolver
├── RespawnResolver
├── World (Builder script)
│   ├── Exterior
│   └── Pension
│       └── GroundFloor
├── StaticGameplayCollisions    # populado em _Ready do builder
├── LevelController             # estado puzzle, mensagens
├── Interactions                # Area3D + Interactable
├── Lighting
├── Atmosphere
│   ├── WorldEnvironment
│   └── PostProcess             # opcional Sprint 11+
├── PlayerSpawn (Marker3D)
├── Debug
│   └── PlaytestDebug
├── Player                      # instance
└── UI
    └── HUD
```

**Proibido na fase blockout térreo:**
- Nó `Floor02`
- `StairRamp` visual
- `Ceiling` / `Roof` interior
- GLB instanciado com colisão importada

---

## Autoloads (alvo — Sprint 01+)

| Nome | Caminho | Sprint |
|------|---------|--------|
| SceneTransition | `scenes/system/SceneTransition.tscn` | 01+ |
| CheckpointManager | `scenes/system/CheckpointManager.tscn` | 12+ |
| GameSession | `scripts/system/GameSession.cs` | 07+ |

Recriar apenas quando necessário — não restaurar autoloads órfãos na Sprint 00.

---

## Namespaces C#

```
BREU.Scripts.Player
BREU.Scripts.Ui
BREU.Scripts.Interaction
BREU.Scripts.Levels.PensaoSantaLuzia
BREU.Scripts.Levels.PensaoSantaLuzia.Shared   # BlockoutSolid
BREU.Scripts.Debug
BREU.Scripts.System
```

---

## `.gitignore` mínimo

```
.godot/
*.tmp
bin/
obj/
.vs/
```

---

## O que NÃO commitar

- Cache `.godot/` (exceto se time decidir o contrário — padrão: ignorar)
- Cenas `_archive/`, `*_backup*`, `*_old*`
- GLB experimental em `assets/models/` antes da Sprint 15
- Scripts `*Fix*.cs`, `*Patch*.cs`, `*Temp*.cs` sem entrada em doc

---

## Migração do estado atual (Sprint 00)

O disco local **não contém** `project.godot` nem cenas — apenas `docs/`. Git contém histórico até commit `de07ae6`.

**Opções (decisão Sprint 00):**
1. **Greenfield:** recriar estrutura vazia conforme este doc.
2. **Restore seletivo:** `git checkout de07ae6 -- project.godot BREU.csproj` etc., depois **deletar** cenas/scripts inválidos com checklist.

Recomendação do Technical Director: **greenfield** para gameplay; cherry-pick apenas shaders/audio se validados.
