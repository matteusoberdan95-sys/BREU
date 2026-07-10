# BREU - Balanceamento da RitualRoom

Ultima atualizacao: 2026-07-10

Documento de referencia para o polimento da Sala dos Santos Secos (Sprint H).

## Player

| Parametro | Valor |
|-----------|-------|
| MaxHealth | 100 |
| DamageInvulnerabilityTime | 0.7s |

## EnemyPlaceholder

| Parametro | Valor |
|-----------|-------|
| Damage | 10 |
| AttackCooldown | 2.0s |
| ChaseSpeed | 1.65 |
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

## Feedback de dano

| Parametro | Valor |
|-----------|-------|
| FlashDuration | 0.35s |
| MaxAlpha | 0.35 |
| MessageCooldown | 1.5s |
| CameraShakeStrength | 0.04 |
| CameraShakeDuration | 0.12s |

## Objetivo de balanceamento

- O player deve sobreviver a varios golpes (cerca de 10 hits com vida cheia, considerando invulnerabilidade).
- O martelo deve dar respiro via stun, mas nao eliminar a ameaca (10 hits de durabilidade vs perseguicao continua).
- O inimigo deve pressionar e assustar sem parecer injusto (velocidade moderada, cooldown de ataque de 2s).

## Arquivos relacionados

- `scripts/enemies/EnemyPlaceholderAI.cs`
- `scripts/player/PlayerMeleeAttack.cs`
- `scripts/player/PlayerHealth.cs`
- `scripts/ui/DamageOverlay.cs`
- `scripts/horror/RitualRoomScareTrigger.cs`
