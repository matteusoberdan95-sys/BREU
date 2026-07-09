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

O HUD usa painel escuro no canto inferior esquerdo (`StatusPanel`) com:

- stamina;
- lanterna;
- arma equipada.

Prompt de interacao central: `[E] <acao>` em `InteractionPrompt`.

Mensagens temporarias em `MessagePanel` (coleta, bilhete, susto, radio).

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

`DemoRoom/CorridorPlaceholder`

O corredor comeca logo apos a porta aberta e segue para **+Z** (a `door_01` do GLB fica em Z positivo, ~3.24m).

Estrutura:

- `CorridorFloor`, `CorridorCeiling`, `CorridorLeftWall`, `CorridorRightWall`, `CorridorEndWall`
- `CorridorCollisions` com chao, paredes, teto e parede final
- `CorridorLight_01` com luz amarelada fraca
- `CorridorDarkZone` preparado para escurecimento futuro
- `CorridorEndTrigger` no fim do corredor

Materiais em `res://materials/mat_corridor_*.tres` (mais escuros que o quarto).

Teste:

1. Abrir a porta com `E`.
2. Andar em direcao ao fundo do quarto (+Z).
3. Confirmar chao, paredes, teto e parede final visiveis.
4. Tentar atravessar paredes — o player deve ser bloqueado.
5. Chegar perto do fim do corredor (`CorridorEndTrigger` em Z ~8.7).

Console esperado no fim:

```text
Fim da demo atual: o corredor continua na proxima sprint.
```

Observacao: o corredor ainda e placeholder feito com `BoxMesh` no Godot. Sera substituido por asset modular do Blender.

## Sprint — Corredor Placeholder

### Abrir a porta

1. Ir ate `DemoRoom/InteractionPoints/DoorInteractable`.
2. Mirar na porta e apertar `E`.
3. `DoorClosedCollision` desativa e `door_01` some.

### Entrar no corredor

1. Apos abrir a porta, andar para +Z.
2. O corredor comeca em Z ~3.2 e tem 6m de comprimento.
3. O player nao deve cair no limbo — ha colisao de chao em `CorridorCollisions/CorridorFloorCollision`.

### CorridorEndTrigger

- Caminho: `DemoRoom/CorridorPlaceholder/CorridorEndTrigger`
- Posicao aproximada: `X 0, Y 1.0, Z 8.7`
- Ao entrar na area, imprime fim da demo no console (sem pausar nem trocar cena).

### Testar colisao do corredor

- Andar contra paredes laterais e parede final.
- Pular contra o teto opcional (`CorridorCeilingCollision`).

### Testar martelo ajustado na mao

1. Coletar o martelo.
2. Confirmar escala menor no canto inferior direito (`WeaponHolder/EquippedHammerVisual`).
3. O martelo nao deve cobrir o centro da tela.

### Testar HUD da lanterna

1. No inicio, HUD deve mostrar `Lanterna 100/100` com lanterna desligada.
2. Apertar `F` para ligar — bateria comeca a drenar.
3. Apertar `F` novamente — bateria para de drenar.
4. Com bateria em 0, lanterna desliga e nao religa ate recarga futura.

## Sprint — Atmosfera inicial e corredor

### HUD novo

Caminho: `DemoRoom/UI/HUD`

O HUD usa painel escuro semi-transparente no canto inferior esquerdo:

- `StatusPanel/StaminaLabel`
- `StatusPanel/FlashlightLabel`
- `StatusPanel/WeaponLabel`

Prompt de interacao central: `Root/InteractionPrompt`

Mensagens temporarias: `Root/MessagePanel/MessageLabel`

Teste:

1. Mirar em bilhete/martelo/porta — prompt `[E] ...` no centro.
2. Coletar martelo — mensagem `Martelo Enferrujado coletado.`
3. Ler bilhete — mensagem `Bilhete encontrado.`

### Som da porta

Caminho: `DemoRoom/InteractionPoints/DoorInteractable/DoorAudio`

Script: `res://scripts/doors/DoorAudioController.cs`

Teste:

1. Abrir a porta com `E`.
2. Sem arquivos de audio configurados, o console deve mostrar:
   `DoorAudio: som de abrir nao configurado.`
3. Quando os `.ogg` forem adicionados, o som toca em 3D na porta.

### Radio / interferencia

Caminho: `DemoRoom/Horror/RadioInterference`

Script: `res://scripts/horror/RadioInterferenceController.cs`

Teste:

1. O radio nao inicia sozinho (`StartsActive = false`).
2. Ao passar pelo susto do corredor, o radio pulsa por ~3.5s.
3. Sem audio real, o HUD mostra: `O radio chia em algum lugar...`
4. Console esperado:
   `Radio: interferencia iniciada.`

### Primeiro susto no corredor

Caminho: `DemoRoom/CorridorPlaceholder/ScareTriggers/CorridorScareTrigger`

Posicao aproximada: `X 0, Y 1.0, Z 5.5` (meio do corredor, eixo +Z)

Teste:

1. Abrir a porta e entrar no corredor.
2. Andar ate o meio do corredor (Z ~5.5).
3. Confirmar:
   - mensagem HUD `Voce ouviu isso?`
   - luz `CorridorLight_01` pisca por ~2s e termina em ~0.25
   - radio/interferencia ativa
   - silhueta aparece no fim do corredor
   - apos ~1.75s a silhueta some (se `HideEnemyAfterScare = true`)
4. O susto dispara apenas uma vez.

Console esperado:

```text
CorridorScare: primeiro susto disparado.
EnemyPlaceholder: presenca detectada no corredor.
DemoSequence: primeiro susto do corredor disparado.
```

### EnemyPlaceholder

Caminho: `DemoRoom/CorridorPlaceholder/EnemyPlaceholder`

Cena: `res://scenes/enemies/EnemyPlaceholder.tscn`

Posicao aproximada: `X 0, Y 0, Z 8.2`

- Inicia invisivel e inativo.
- Ativado pelo `CorridorScareTrigger`.
- Silhueta escura (capsula + esfera), ~1.8m.
- **Nao ataca, nao causa dano, sem IA avancada.**
- Modelo final sera feito no Blender depois.

### DemoRoomSequenceController

Caminho: `DemoRoom/DemoRoomSequenceController`

Estados rastreados (console):

- `HasReadNote`
- `HasPickedHammer`
- `HasOpenedDoor`
- `HasTriggeredCorridorScare`

Ainda nao bloqueia progressao.

### Assets futuros necessarios

Colocar em `res://assets/audio/`:

| Arquivo | Uso |
|---------|-----|
| `sfx/doors/door_open_old_wood.ogg` | Abrir porta |
| `sfx/doors/door_close_old_wood.ogg` | Fechar porta |
| `sfx/doors/door_locked_rattle.ogg` | Porta trancada |
| `sfx/radio/radio_static_loop.ogg` | Loop de interferencia |
| `sfx/radio/radio_whisper_01.ogg` | Sussurro do radio |
| `sfx/horror/scare_stinger_01.ogg` | Stinger do susto |
| `sfx/enemy/enemy_breath_01.ogg` | Respiracao (futuro) |
| `sfx/enemy/enemy_step_01.ogg` | Passos (futuro) |

## Sprint — UI narrativa e audio base

### Leitura do bilhete (UI dedicada)

Caminho: `DemoRoom/UI/NoteReaderUI`

Cena: `res://scenes/ui/NoteReaderUI.tscn`

Teste:

1. Mirar no bilhete e apertar `E`.
2. Abre painel central estilo papel velho com fundo escurecido.
3. Titulo: `Bilhete do Quarto 07`.
4. Fechar com `E` ou `Esc` — movimento e mouse look voltam.
5. Console ainda imprime o texto do bilhete.

### AudioManager

Caminho: `DemoRoom/UI/AudioManager`

Documentacao: `docs/technical/AUDIO_SYSTEM.md`

Lista de arquivos: `assets/audio/AUDIO_ASSETS_NEEDED.md`

Teste sem `.ogg`:

- Porta, martelo, lanterna e susto nao quebram o jogo.
- Console: `AudioManager: stream nao configurado.` ou mensagens especificas da porta/radio.

### Fim do corredor

Caminho: `DemoRoom/CorridorPlaceholder/CorridorEndTrigger`

Visual: `CorridorEndDoorPlaceholder` (porta escura no fim)

Teste:

1. Chegar ao fim (Z ~8.7).
2. HUD: `A porta no fim do corredor esta trancada.`
3. Console: `Fim da demo atual. Proxima sprint: porta final/transicao.`

### Direcao de atmosfera

Ver `docs/design/ATMOSPHERE_GUIDE.md`.

### .ogg ainda necessarios

Todos os arquivos do pack v0.1 estao em `res://assets/audio/` (copiados de `breu_de_dentro_audio_pack_v01`).

Teste de audio:

1. Abrir porta — som de madeira (`door_open_old_wood.ogg`).
2. Coletar martelo — `pickup_item.ogg`.
3. Lanterna F — `flashlight_click.ogg`.
4. Susto no corredor — stinger + radio static + sussurro.
5. Inimigo placeholder — respiracao + rosnado ao ativar.
6. Ambiencia — tom do quarto e vento ao iniciar; tom do corredor ao abrir a porta.

## Estado Esperado do Playtest

O jogador deve conseguir:

- aparecer dentro do Quarto 07;
- olhar com o mouse;
- andar com WASD;
- correr com Shift;
- ligar/desligar lanterna;
- ver prompts no HUD;
- ler o bilhete na UI dedicada (`NoteReaderUI`);
- coletar o martelo;
- ver o martelo na mao;
- abrir a porta (com feedback de audio quando configurado);
- atravessar para o corredor placeholder;
- passar pelo primeiro susto do corredor;
- ver silhueta placeholder no escuro;
- chegar ao fim e ver mensagem de porta trancada.

## Problemas Conhecidos

- Porta nao tem animacao ou pivo real (som preparado, streams ainda vazios).
- Martelo na mao e placeholder, sem animacao e sem combate.
- Corredor placeholder modular no Godot (sera trocado por asset Blender).
- Inimigo placeholder — sem combate, sem dano, sem navmesh.
- Arquivos `.ogg` ainda nao adicionados (sistema preparado com fallback seguro).
- `PlayerStamina` ainda nao esta na cena do player (label fixa no HUD).
- Transicao de cena no fim do corredor ainda nao implementada.

## Proximos Passos

- Adicionar arquivos `.ogg` (ver `assets/audio/AUDIO_ASSETS_NEEDED.md`).
- Porta final interativa / transicao de cena.
- Ambience loops (quarto e corredor).
- Modelar inimigo no Blender e substituir `EnemyPlaceholder`.
- IA simples de perseguicao (sem combate completo ainda).
