# Exportacao Blender -> Godot

## Regras

1. Unidade: `1 unidade = 1 metro`.

2. Eixos:

Blender:

- X esquerda/direita
- Y frente/fundo
- Z altura

Godot:

- X esquerda/direita
- Y altura
- Z frente/fundo

3. Antes de exportar:

- renomear objetos;
- organizar collections;
- aplicar scale;
- salvar `.blend`;
- exportar `.glb`.

4. Nome de arquivos:

```text
cenario_nome_blockout.glb
enemy_nome_blockout.glb
prop_nome_v01.glb
```

5. Nao exportar:

- cameras preview;
- luzes preview;
- objetos ocultos desnecessarios;
- markers que nao serao usados.

6. Exportar:

- meshes;
- materiais base;
- markers uteis, se necessario;
- modelo separado de logica.

7. No Godot:

- criar colisao separada;
- nao depender da colisao importada;
- configurar luz no Godot;
- testar escala com player.
