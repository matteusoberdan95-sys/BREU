# BREU - Pipeline de assets Blender

## Escala

- 1 unidade Godot = 1 metro.
- Modelar assets ja pensando na escala real do cenario.
- Player placeholder tem cerca de 1.72 m de altura.

## Exportacao

- Exportar preferencialmente como `.glb`.
- Aplicar transforms no Blender antes de exportar.
- Usar origem/pivo correto para cada asset.
- Nomear objetos de forma limpa e especifica.

## Nomenclatura

Exemplos:

- `room_wall_01`
- `bed_iron_01`
- `mattress_dirty_01`
- `hammer_rusty_01`
- `radio_broken_01`
- `door_wood_old_01`

## Colisao

- Usar colisoes simples sempre que possivel.
- Separar colisores quando o mesh visual for complexo.
- Evitar geometria pesada em colisores.

## Materiais

- Materiais basicos devem vir com nomes claros.
- Texturas devem ser organizadas em `assets/textures`.
- Evitar materiais duplicados sem necessidade.

## Modularidade

- Paredes, portas, batentes, grades, moveis e props pequenos devem ser modulares.
- Props com gameplay precisam de pivo previsivel.
- Portas devem ter pivo no eixo de dobradica nas versoes finais.

## Entrega para Godot

- Exportar para `assets/blender_exports`.
- Instanciar em cenas de level ou props.
- Nunca substituir scripts de gameplay por logica embutida no asset.
