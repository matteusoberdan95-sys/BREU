# BREU - Playtest do DemoRoom

## Como executar

1. Abrir o projeto no Godot 4.7 Mono.
2. Abrir `res://scenes/levels/demo_room/DemoRoom.tscn`.
3. Pressionar F6 para executar a cena atual.
4. Clicar em **Entrada** ou na janela do jogo para garantir foco.

## Controles

- `W`: andar para frente.
- `S`: andar para tras.
- `A`: andar para a esquerda.
- `D`: andar para a direita.
- `Shift`: correr.
- `Mouse`: olhar ao redor.
- `F`: ligar/desligar a lanterna.
- `E`: interagir com objeto mirado.
- `Esc`: liberar o mouse.
- `Clique esquerdo`: capturar o mouse novamente.

## Sistema de Coordenadas

O player nasce no centro do Quarto 07:

`X 0, Y 1.0, Z 0`

No Godot, `Y` e altura. No Blender, a altura era `Z`.

## Colisoes Temporarias

As colisoes auxiliares ficam em:

`DemoRoom/Collisions`

Principais volumes:

- `FloorCollision`
- `WallLeftCollision`
- `WallRightCollision`
- `WallBackLeftCollision`
- `WallBackRightCollision`
- `WallBackTopCollision`
- `DoorClosedCollision`
- `WallFrontCollision`
- `FurnitureCollisions/BedCollision`
- `FurnitureCollisions/TableMainCollision`
- `FurnitureCollisions/SmallTableCollision`
- `FurnitureCollisions/ChairCollision`

Para testar, andar contra cama, mesa, criado-mudo, cadeira, paredes e porta fechada.

## HUD

O HUD minimo deve mostrar:

- prompt `[E] <acao>` ao mirar em um interativo;
- estado da lanterna;
- arma atual.

Ao coletar o martelo, o HUD deve mostrar:

`Arma: Martelo Enferrujado 10/10`

## Interacoes

As interacoes usam:

`Player/CameraPivot/Camera3D/InteractionRay`

Alcance: 2.5m.

### Bilhete

No:

`DemoRoom/InteractionPoints/NoteInteractable`

Teste:

1. Andar ate a mesa.
2. Mirar no bilhete ou na area acima dele.
3. Confirmar prompt `[E] Ler bilhete`.
4. Apertar `E`.

Console esperado:

```text
Bilhete encontrado:
Quarto 07 ocupado. Nao abrir depois das 22h. Se a luz apagar, nao responda as batidas.
```

## Martelo

No:

`DemoRoom/InteractionPoints/HammerPickup`

Teste:

1. Andar ate perto do martelo.
2. Mirar no martelo.
3. Confirmar prompt `[E] Pegar Martelo Enferrujado`.
4. Apertar `E`.

Resultado esperado:

- Martelo some do cenario importado.
- Martelo placeholder aparece na mao/camera.
- HUD mostra `Arma: Martelo Enferrujado 10/10`.

Console esperado:

```text
Inventario: Martelo Enferrujado equipado. Durabilidade: 10/10.
Martelo Enferrujado coletado. Durabilidade: 10/10.
```

## Porta

No:

`DemoRoom/InteractionPoints/DoorInteractable`

Teste:

1. Andar ate a porta.
2. Mirar na porta.
3. Confirmar prompt `[E] Abrir porta`.
4. Apertar `E`.

Resultado esperado:

- `DoorClosedCollision` desativa.
- `door_01` tenta ficar invisivel.
- O player consegue entrar no corredor placeholder.

Console esperado:

```text
Porta aberta. Corredor placeholder liberado.
```

## Corredor Placeholder

No:

`DemoRoom/Environment/CorridorPlaceholder`

Teste:

1. Abrir a porta.
2. Andar para fora do quarto.
3. Confirmar que ha chao, paredes laterais, teto e bloqueio final.
4. Chegar perto do fim do corredor.

Console esperado:

```text
Fim da demo placeholder. Corredor conectado, proxima etapa: transicao/porta final.
```

## Estado Esperado do Playtest

O jogador deve conseguir:

- aparecer dentro do Quarto 07;
- olhar com o mouse;
- andar com WASD;
- correr com Shift;
- ligar/desligar lanterna;
- ver prompts no HUD;
- ler o bilhete;
- coletar o martelo;
- ver o martelo na mao;
- abrir a porta;
- atravessar para o corredor placeholder;
- chegar ao fim temporario da demo.

## Problemas Conhecidos

- HUD e minimo.
- Bilhete ainda nao tem tela dedicada.
- Porta nao tem animacao, pivo real ou som.
- Martelo na mao e placeholder, sem animacao e sem combate.
- Corredor e placeholder, sem encontro, audio, porta final real ou transicao.
- Colisoes podem precisar de ajuste fino no editor.

## Proximos Passos

- Trocar corredor placeholder por cena modular definitiva.
- Criar porta final/transicao.
- Criar UI de leitura do bilhete.
- Trocar martelo placeholder por asset final/animado.
- Adicionar feedback sonoro/visual de interacao.
