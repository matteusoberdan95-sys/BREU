# BREU - Import do Quarto 07

## Asset principal

Arquivo usado pela cena:

`res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`

Cena Godot que instancia o asset:

`res://scenes/levels/demo_room/DemoRoom.tscn`

O `.glb` deve ser tratado como asset importado do Blender. Nao editar a estrutura importada dentro do Godot para gameplay; criar nos auxiliares fora do asset.

## Estrutura da cena

`DemoRoom.tscn` organiza o blockout assim:

```text
DemoRoom
  Environment
    quarto_07_blockout
  SpawnPoints
    PlayerStart
  InteractionPoints
    DoorPoint
    HammerPickupPoint
    NotePoint
  Lighting
    RoomLightPoint
  Debug
    DemoEndTrigger
```

## Pontos auxiliares

- `PlayerStart`: fica perto da porta, dentro do quarto, olhando para o fundo do quarto. No Godot, o eixo frontal padrao do Marker3D e `-Z`, entao a rotacao inicial aponta para o fundo do quarto.
- `DoorPoint`: fica proximo de `door_01`.
- `HammerPickupPoint`: fica proximo de `hammer_handle` e `hammer_head`.
- `NotePoint`: fica proximo de `note_01` na mesa.
- `RoomLightPoint`: fica proximo de `lamp_bulb`/lampada pendurada.
- `DemoEndTrigger`: trigger temporario de debug no fim do corredor placeholder.

## Objetos que devem virar interativos depois

- `door_01`: deve virar porta interativa/trancavel.
- `hammer_handle` e `hammer_head`: devem compor o martelo coletavel.
- `note_01`: deve virar bilhete interativo com UI de leitura.

## Como substituir por uma nova versao do Blender

1. No Blender, manter escala real: 1 unidade = 1 metro.
2. Aplicar transforms antes de exportar.
3. Manter nomes importantes sempre que possivel:
   - `door_01`
   - `hammer_handle`
   - `hammer_head`
   - `note_01`
   - `lamp_bulb`
4. Exportar como `.glb`.
5. Substituir o arquivo em:

   `res://assets/blender_exports/quarto_07/quarto_07_blockout.glb`

6. Nao alterar o arquivo instanciado dentro da cena para adicionar gameplay. Se for preciso reposicionar interacoes, mover os marcadores auxiliares em `DemoRoom.tscn`.
7. Abrir `DemoRoom.tscn` no Godot e conferir se os pontos auxiliares ainda estao alinhados com porta, martelo, bilhete e lampada.

## Camera e luz do Blender

A camera do Blender e apenas preview de modelagem. A camera real do jogo sera a camera do player no Godot.

A luz do Blender pode ser usada como referencia visual, mas pode ser substituida por luz configurada no Godot. Nesta cena, `RoomLightPoint` ja existe como ponto de luz controlado pelo Godot.

## Observacao sobre organizacao

O projeto deve manter apenas a pasta canonica:

`res://assets/blender_exports/quarto_07/`

Exports antigos foram consolidados em `quarto_07`. Nas proximas exportacoes, prefira exportar direto para o caminho canonico.
