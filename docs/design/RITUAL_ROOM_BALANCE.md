# BREU - Balanceamento da RitualRoom

Ultima atualizacao: 2026-07-10

Documento de referencia para o polimento da Sala dos Santos Secos (Sprint H / H.5).

## Player

| Parametro | Valor |
|-----------|-------|
| MaxHealth | 100 |
| DamageInvulnerabilityTime | 0.7s |

## EnemyPlaceholder

| Parametro | Valor |
|-----------|-------|
| Damage | 15 |
| AttackCooldown | 1.9s |
| ChaseSpeed | 1.55 |
| NavigationUpdateInterval | 0.2s |
| MoveSpeed | 1.1 |
| AttackRange | 1.25 |
| StunDuration | 1.25s |
| AlertDuration | 1.2s |
| LoseSightGraceTime | 1.5s |

## Martelo Enferrujado

| Parametro | Valor |
|-----------|-------|
| Durability | 10 |
| AttackCooldown | 0.85s |
| AttackRange | 2.0 |
| AttackRadius | 0.8 |
| HammerDamage | 10 |
| DurabilityCostPerHit | 1 (somente ao acertar) |

## Collision layers (3D)

| Layer | Bit | Uso |
|-------|-----|-----|
| World | 1 | Chao, paredes, mesa, porta bloqueada |
| Interactable | 2 | Bilhete, chave, porta interativa |
| Trigger | 4 | Susto, fim de corredor, triggers de area |
| Enemy | 8 | `EnemyPlaceholder` corpo + hurtbox |
| Player | 16 | Reservado para uso futuro; player ainda usa layer 1 por padrao |

## Feedback de dano

| Parametro | Valor |
|-----------|-------|
| FlashDuration | 0.35s |
| MaxAlpha | 0.35 |
| HurtAudio | `player_hurt_01.ogg` (fallback: `corridor_hit_01.ogg`) |
| DeathAudio | `death_stinger_01.ogg` (fallback: `scare_stinger_01.ogg`) |

## Objetivo de balanceamento

- O player deve sobreviver a varios golpes, mas morrer em ~7 hits com dano 15.
- O martelo deve dar respiro via stun, mas nao eliminar a ameaca.
- O inimigo deve pressionar sem parecer injusto.

## Navegacao, contorno e obstaculos

| Componente | Papel |
|------------|-------|
| `NavigationAgent3D` | Pathfinding geral na RitualRoom |
| `TableAvoidance` (inline na IA) | Contorno lateral quando mesa bloqueia linha direta |
| `TableCollision` grupos | `enemy_path_blocker` + `melee_blocker` |
| `HasMeleeLineOfSight` | Martelo so acerta com linha livre ate o inimigo |
| `HasAttackLineOfSightToPlayer` | Inimigo nao ataca atraves da mesa |

Comportamento:

- Mesa continua sendo o obstaculo principal da sala.
- Se inimigo e player estao em lados opostos da mesa, a IA escolhe o corredor lateral de menor custo.
- Waypoint lateral fica travado por **0.8s** para evitar oscilacao.
- Se ficar preso por **1.2s**, troca de lado e aplica empurrao lateral leve.
- `NavigationAgent3D` permanece ativo, mas contorno da mesa tem prioridade quando bloqueado.

### Colisao da mesa

| Parametro | Valor |
|-----------|-------|
| Position | `X 0, Y 0.45, Z -0.75` |
| Size | `2.0 x 0.7 x 1.05` |
| Grupos | `enemy_path_blocker`, `melee_blocker` |

## Problemas conhecidos

- Ainda nao ha navmesh dedicado fora da RitualRoom; nesta sala o inimigo usa `NavigationAgent3D` + contorno manual da mesa.
- Colisoes ainda sao caixas temporarias alinhadas ao GLB.
- Se o bake falhar, a IA cai no fallback de perseguicao direta com desvio anti-stuck.
- Navmesh/pathfinding final fica para sprint futura, se necessario.
- Apenas `TableCollision` esta em `melee_blocker` por enquanto; paredes bloqueiam fisicamente mas nao pelo grupo.

## Arquivos relacionados

- `scripts/enemies/EnemyPlaceholderAI.cs`
- `scripts/player/PlayerMeleeAttack.cs`
- `scripts/levels/RitualRoomEnvironmentSetup.cs`
- `scenes/levels/ritual_room/RitualRoom.tscn`
