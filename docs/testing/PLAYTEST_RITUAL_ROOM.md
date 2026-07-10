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

Ao cruzar a area perto do centro/frente da sala:

- HUD mostra `As velas tremem sem vento.`
- toca `scare_stinger_01.ogg`, se carregado;
- `RadioStaticPoint` chia por alguns segundos;
- `CandleLightMain` e `BackAltarLight` piscam por 2 segundos;
- `EnemyPlaceholder` aparece no fundo/lateral da sala;
- console imprime `RitualRoomScareTrigger ativado.`
- o console nao deve ficar repetindo mensagens de recuperacao de piso.

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
2. Confirmar growl/ataque em intervalo.
3. Confirmar console:

```text
Player tomou dano: 12
```

4. Confirmar mensagem HUD:

```text
Voce foi atingido.
```

Morte ainda nao tem tela propria. Se a vida chegar a zero, o console deve imprimir:

```text
Player morreu. TODO: implementar tela de morte.
```

### Como testar stun via metodo/debug

Antes de testar combate real do player, pegue o martelo no Quarto 07 e confirme que `GameSession` manteve a arma na RitualRoom. O ataque do player com martelo ainda entra na proxima etapa.

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
2. Confirmar mensagem:

```text
A porta esta trancada. Alguma coisa precisa ser feita primeiro.
```

3. Mirar e apertar `E` para confirmar o mesmo feedback.
4. Confirmar debug `Ritual exit door bloqueada.`

## Problemas conhecidos

- Colisoes ainda sao caixas temporarias.
- O inimigo ainda e placeholder simples: corpo/cabeca/capsula, sem modelo final do Blender.
- A IA persegue diretamente, sem navmesh; pode ficar limitada por moveis ate criarmos pathfinding.
- O movimento vertical do inimigo esta travado de proposito durante o prototipo.
- A chave ja marca `GameSession`, mas ainda nao abre objetivo/porta.
- O dano do player e simples; ainda nao ha tela de morte.
- O stun esta preparado por metodo, mas ainda nao conectado ao martelo.
- A porta de saida nao troca de cena.
- A sala usa o GLB importado como visual; gameplay fica em nos auxiliares Godot.
