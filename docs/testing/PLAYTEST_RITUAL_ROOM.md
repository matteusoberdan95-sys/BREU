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

A chave ainda nao entra em inventario completo. O script guarda apenas estado local (`HasOldKey`).

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

O inimigo nao persegue e nao causa dano nesta sprint.

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
- A chave usa estado local e ainda nao integra com inventario persistente.
- O inimigo placeholder aparece, mas nao persegue nem ataca.
- A porta de saida nao troca de cena.
- A sala usa o GLB importado como visual; gameplay fica em nos auxiliares Godot.
