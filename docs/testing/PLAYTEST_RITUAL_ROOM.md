# BREU - Playtest da Sala dos Santos Secos

## Como executar isolada

1. Abrir o projeto no Godot 4.7 Mono.
2. Abrir `res://scenes/levels/ritual_room/RitualRoom.tscn`.
3. Rodar com F6.
4. Clicar na janela/aba **Entrada** se o mouse nao capturar.

## Como chegar pelo fluxo completo

Rodar a Main Scene do projeto:

```text
TrailIntro -> DemoRoom -> RitualRoom
```

No `DemoRoom`, abrir a porta do Quarto 07, entrar no corredor, disparar o susto e interagir com a porta final. A porta final deve mostrar fade com:

```text
A madeira cede para um comodo quente e escuro.
```

Destino:

`res://scenes/levels/ritual_room/RitualRoom.tscn`

## Onde o player nasce

`PlayerSpawn`:

```text
X 0
Y 1.0
Z -3.15
```

O spawn olha para o centro da sala. O `PlayerSpawnResolver` registra o checkpoint:

```text
RitualRoom_SantosSecos
```

## Como testar colisoes

Colisoes temporarias:

- `Collisions/FloorCollision`
- `Collisions/CeilingCollision`
- `Collisions/WallLeftCollision`
- `Collisions/WallRightCollision`
- `Collisions/WallBackCollision`
- `Collisions/FrontWallLeftCollision`
- `Collisions/FrontWallRightCollision`
- `Collisions/TableCollision`
- `Collisions/ExitDoorBlockerCollision`

Teste:

1. Andar em volta da mesa ritual.
2. Tentar atravessar paredes e porta de saida.
3. Confirmar que o player nao cai abaixo do chao.
4. Confirmar que a mesa bloqueia sem prender o player.

## Como testar o bilhete

Interativo:

`Interactables/RitualNotePoint`

1. Mirar no bilhete.
2. Confirmar prompt `[E] Ler bilhete`.
3. Apertar `E`.
4. Confirmar abertura da `NoteReaderUI`.
5. Confirmar checkpoint/debug `Ritual_Note_Read`.

Texto esperado:

```text
Nao mexam nos santos secos. Eles escutam quando a vela apaga. O hospede do Quarto 07 ja atravessou a porta.
```

## Como testar a chave

Interativo:

`Interactables/OldKeyPickupPoint`

1. Mirar na chave.
2. Confirmar prompt `[E] Pegar Chave Velha`.
3. Apertar `E`.
4. Confirmar mensagem `Chave Velha coletada.`.
5. Confirmar debug `Item coletado: Chave Velha`.

A chave tambem marca `GameSession.HasOldKey`. Ela nao remove o martelo equipado.

## Como validar martelo persistente

Para testar a RitualRoom com arma:

1. Rodar o fluxo completo desde `TrailIntro`.
2. Entrar no Quarto 07.
3. Pegar o `Martelo Enferrujado`.
4. Confirmar no HUD:

```text
Arma: Martelo Enferrujado 10/10
```

5. Entrar na `RitualRoom` pela porta final do corredor.
6. Confirmar que o HUD continua mostrando o martelo.
7. Confirmar que o visual placeholder do martelo continua na mao.
8. Pegar a `Chave Velha` e confirmar que o martelo continua equipado.

## Como testar o susto

Trigger:

`Triggers/RitualScareTrigger`

Sequencia esperada ao cruzar a area perto do centro/frente da sala:

1. HUD mostra `As velas tremem sem vento.`
2. `CandleLightMain` e `BackAltarLight` piscam por **1.5s** (flicker dramatico).
3. `scare_stinger_01.ogg` toca, se carregado.
4. `RadioStaticPoint` chia por **2 a 4s**.
5. `EnemyPlaceholder` aparece no fundo/lateral da sala.
6. Inimigo fica parado olhando por **AlertDuration (1.2s)**.
7. Inimigo inicia perseguicao em velocidade moderada.

Logs permitidos:

```text
EnemyPlaceholder ativado.
EnemyPlaceholder iniciou perseguicao.
```

Nao deve haver spam de posicao, raycast ou recuperacao de piso.

## EnemyPlaceholder

Cena:

`res://scenes/enemies/EnemyPlaceholder.tscn`

Script:

`res://scripts/enemies/EnemyPlaceholderAI.cs`

Posicao na RitualRoom:

```text
X -0.85
Y 0.05
Z 2.45
```

Estado inicial:

- `Visible = false`
- `StartDormant = true`
- `CanChase = true`
- `LockVerticalMovement = true`, temporario para impedir que o placeholder afunde no piso.
- `EnemyHurtbox` fica no grupo `enemy_hurtbox` e liga/desliga junto com o inimigo.

Ao cruzar `Triggers/RitualScareTrigger`:

1. As luzes piscam.
2. O radio chia.
3. O inimigo aparece.
4. O inimigo toca growl, olha para o player e entra em `Alert`.
5. Depois entra em `Chasing` e caminha diretamente ate o player.
6. Se chegar perto o suficiente, entra em `Attacking`.

### Como testar perseguicao

1. Disparar o susto.
2. Recuar da mesa e observar o inimigo vindo em direcao ao player.
3. Confirmar que ele nasce visivel, acima do piso e sem ficar preso no canto.
4. Tentar usar mesa/colisoes para confirmar que o `CharacterBody3D` nao atravessa paredes.
5. Escutar respiracao em loop e passos durante a perseguicao.

### Como testar ataque do inimigo

1. Deixar o inimigo chegar perto.
2. Confirmar growl/ataque em intervalo de **2.0s**.
3. Confirmar HUD e console:

```text
Vida 90/100
Player tomou dano: 10
EnemyPlaceholder atacou player.
```

4. Confirmar flash vermelho escuro, tremor leve de camera e mensagem HUD (com cooldown):

```text
Voce foi atingido.
```

Quando a vida chegar a zero, a tela de morte deve abrir:

```text
VOCE MORREU
A casa ainda esta ouvindo.
Tentar novamente
```

O player deve parar de se mover, o mouse deve ficar livre para clicar no botao e o inimigo deve parar de perseguir/atacar.

## Morte e respawn

Fluxo:

1. Entrar na `RitualRoom` com ou sem martelo.
2. Disparar o `RitualScareTrigger`.
3. Deixar o `EnemyPlaceholder` atacar ate a vida chegar a `0/100`.
4. Confirmar flash vermelho leve a cada dano.
5. Confirmar tela `VOCE MORREU`.
6. Clicar `Tentar novamente`.

Resultado esperado:

- a cena do ultimo checkpoint recarrega;
- o player volta para o checkpoint salvo;
- a vida volta para `100/100`;
- o HUD atualiza `Vida`, `Stamina`, `Lanterna` e `Arma`;
- o inimigo e os triggers da sala voltam ao estado inicial por recarregamento da cena;
- o controle e o mouse look voltam apos o respawn.

Checkpoint usado na entrada da sala:

```text
RitualRoom_SantosSecos
```

O `CheckpointManager` tambem salva um snapshot simples do `GameSession`. Se o player entrou na RitualRoom com o Martelo Enferrujado `10/10`, morrer e tentar novamente restaura esse estado. Se entrou com `6/10`, volta com `6/10`. A Chave Velha coletada depois do checkpoint pode voltar a nao estar coletada ao recarregar a cena.

## Combate basico com martelo

Antes de testar combate, pegue o martelo no Quarto 07 e confirme que `GameSession` manteve a arma na RitualRoom.

Fluxo:

1. Rodar o jogo desde `TrailIntro`.
2. Entrar no Quarto 07.
3. Pegar o `Martelo Enferrujado`.
4. Confirmar HUD `Arma: Martelo Enferrujado 10/10`.
5. Ir para a `RitualRoom`.
6. Disparar o susto para ativar o `EnemyPlaceholder`.
7. Mirar no inimigo e clicar com o botao esquerdo do mouse.

Resultado esperado ao acertar:

- o martelo faz um movimento curto na mao;
- se houver audio disponivel, toca som de impacto;
- console imprime `EnemyPlaceholder recebeu hit: 10`;
- console imprime `EnemyPlaceholder stunned por 1.25s`;
- inimigo entra em `Stunned` por **1.25s** com feedback visual (escala + recuo);
- HUD muda para `Arma: Martelo Enferrujado 9/10`;
- depois do stun, o inimigo volta a perseguir.

Resultado esperado ao errar:

- ataque faz feedback visual/swing;
- durabilidade nao diminui por enquanto.

## Correcao de hit detection do martelo

O ataque usa duas etapas:

1. raycast da `Camera3D` para frente, mantido como debug;
2. hit volume em esfera na frente da camera, para nao depender de um raio fino;
3. fallback por grupo `enemies`, caso a colisao fisica nao devolva a hurtbox.

Valores atuais:

```text
AttackRange = 2.0
AttackRadius = 0.8
AttackForwardOffset = 1.0
AttackAngleDot = 0.25
AttackCooldown = 0.85
DebugMelee = false
```

Nesta fase, o ataque nao restringe layer/mask: ele colide com bodies e areas e depois procura um `EnemyPlaceholderAI` no collider ou nos parents.

Logs esperados ao acertar (com `DebugMelee = false`, apenas logs do inimigo):

```text
EnemyPlaceholder recebeu hit: 10
EnemyPlaceholder stunned por 1.25s
```

Com `DebugMelee = true` no inspector, logs extras de raycast/hit volume voltam a aparecer.

Regras atuais:

- o martelo deve balancar sempre que houver arma equipada;
- a durabilidade so reduz se o raycast ou hit volume encontrar `EnemyPlaceholderAI`;
- o inimigo esta no grupo `enemies`;
- a hurtbox do inimigo esta no grupo `enemy_hurtbox`;
- o root do inimigo e `CharacterBody3D` com `CollisionShape3D` capsule;
- se o raycast fino falhar, o hit volume ainda pode acertar inimigo proximo na frente do player;
- bilhete, chave e triggers sao ignorados porque nao resolvem para `EnemyPlaceholderAI`.

Teste de quebra:

1. Acertar o inimigo ate a durabilidade chegar a `0/10`.
2. Confirmar mensagem `O Martelo Enferrujado quebrou.`.
3. Confirmar HUD `Arma: Maos vazias`.
4. Clicar novamente e confirmar mensagem `Voce esta de maos vazias.`.

### Como testar stun via metodo/debug

O teste manual com clique esquerdo ja chama `ReceiveHit(HammerDamage)`. Para teste tecnico isolado, ainda e possivel chamar diretamente:

Para teste tecnico, chamar em debug o metodo do inimigo:

```csharp
ReceiveHit(10)
```

Resultado esperado:

- console imprime `EnemyPlaceholder recebeu hit.`;
- inimigo entra em `Stunned`;
- para por alguns segundos;
- volta para `Chasing`.

## Como testar a porta de saida bloqueada

Trigger/interativo:

`Triggers/ExitDoorTrigger`

1. Chegar perto da porta de saida.
2. **Sem Chave Velha**, confirmar mensagem:

```text
Esta trancada.
```

3. Pegar a `Chave Velha` e tentar de novo. Confirmar:

```text
A chave gira, mas alguma coisa segura a porta por dentro.
```

4. Mirar e apertar `E` para confirmar o mesmo feedback.
5. Confirmar som de porta/tranca se o asset existir.

## Polimento e balanceamento da RitualRoom

Valores atuais documentados em `docs/design/RITUAL_ROOM_BALANCE.md`.

### Inimigo (EnemyPlaceholderAI)

| Parametro | Valor |
|-----------|-------|
| Damage | 10 |
| AttackCooldown | 2.0s |
| ChaseSpeed | 1.65 |
| AttackRange | 1.25 |
| StunDuration | 1.25s |
| AlertDuration | 1.2s |

### Martelo (PlayerMeleeAttack)

| Parametro | Valor |
|-----------|-------|
| AttackCooldown | 0.85s |
| AttackRange | 2.0 |
| AttackRadius | 0.8 |
| DurabilityCostPerHit | 1 (so ao acertar) |

### Como testar susto

1. F6 em `RitualRoom.tscn` ou fluxo completo.
2. Cruzar `RitualScareTrigger`.
3. Confirmar sequencia: mensagem -> luzes 1.5s -> radio 2-4s -> stinger -> inimigo -> pausa 1.2s -> perseguicao.

### Como testar dano

1. Deixar inimigo acertar.
2. Confirmar `Vida` desce, flash vermelho (0.35s), tremor de camera, mensagem com cooldown.

### Como testar stun

1. Acertar inimigo com martelo.
2. Confirmar stun 1.25s, feedback visual, durabilidade -1.

### Como testar morte

1. Deixar vida chegar a 0.
2. Confirmar `VOCE MORREU`, subtitulo, fade in, stinger de morte (ou fallback).
3. Clicar `Tentar novamente` -> `Checkpoint restaurado.` no console.

## Problemas conhecidos

- Colisoes ainda sao caixas temporarias.
- O inimigo ainda e placeholder simples: corpo/cabeca/capsula, sem modelo final do Blender.
- A IA persegue diretamente, sem navmesh; pode ficar limitada por moveis ate criarmos pathfinding.
- O movimento vertical do inimigo esta travado de proposito durante o prototipo.
- A chave ja marca `GameSession`, mas ainda nao abre objetivo/porta.
- O ciclo de morte e respawn ja funciona, mas ainda e prototipo: recarrega a cena inteira do checkpoint.
- O checkpoint ainda fica apenas em memoria; nao existe save em disco.
- O ataque do martelo usa raycast + hit volume e ainda nao tem animacao final.
- O hit volume prioriza `enemy_hurtbox`; interactables nao gastam durabilidade.
- A durabilidade so cai quando o golpe acerta.
- `DebugMelee` desligado por padrao; ligar no inspector para depurar hit detection.
- `player_hurt_01.ogg` e `death_stinger_01.ogg` ainda nao existem; fallbacks usam `corridor_hit_01` e `scare_stinger_01`.
- O som de swing ainda depende de asset futuro; o hit usa fallback se o audio dedicado nao existir.
- A porta de saida nao troca de cena.
- A sala usa o GLB importado como visual; gameplay fica em nos auxiliares Godot.
