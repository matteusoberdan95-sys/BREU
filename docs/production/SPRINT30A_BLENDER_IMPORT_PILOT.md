# Sprint 30A — piloto de importação Blender

## Escopo

Esta sprint importa somente `assets/models/props/pensao/prop_single_bed_old_01.glb`. Nenhum outro GLB do kit é instanciado no mapa. Geometria estrutural, colisões, navegação, portas, puzzle, IA, perseguições, safe zones, triggers e Quarto 203 permanecem fora do escopo.

## Asset e cena reutilizável

- GLB: `res://assets/models/props/pensao/prop_single_bed_old_01.glb`
- Cena de prop: `res://scenes/props/pensao/PropSingleBedOld01.tscn`
- Piloto: `World/VisualPolish/Sprint30A_BlenderAssetPilot`
- Local escolhido: Quarto 201
- Instância: `PropSingleBedOld01_Instance_Room201`
- Posição: `(-4,15; 2,80; -14,00)`
- Escala do wrapper: `1,0`
- Colisão: nenhuma

O GLB contém 18 meshes, 5 materiais, nenhuma câmera e nenhuma luz. O AABB autorado mede aproximadamente `1,04 m × 1,99 m × 0,95 m` (largura × comprimento × altura), com a base em `Y=0`; portanto não foi necessária correção de escala ou pivot no wrapper.

## Substituição controlada

A cama placeholder do Quarto 201 era formada pelo bloco `Furniture_Room201_Bed` e pelo acabamento visual `Room201_ThinMattress`. As duas peças continuam disponíveis para rollback em `Backup_Placeholders_Sprint30A`, mas o container inteiro permanece invisível. A instância Blender é a única cama visível nessa posição.

## Validação automática

- [x] projeto C# compila sem erros ou avisos;
- [x] importação Godot sem erro;
- [x] cena reutilizável carrega;
- [x] cena oficial carrega;
- [x] F9: `0 ERROR / 0 WARNING`;
- [x] deck congelado: `49/49`;
- [x] piloto contém exatamente 18 meshes do GLB e zero colisão ativa, navegação, câmera ou luz.

## Playtest manual obrigatório

- [ ] cama aparece no Quarto 201, apoiada no piso e alinhada à parede;
- [ ] escala e materiais estão corretos;
- [ ] não há cama placeholder duplicada;
- [ ] cama não atravessa parede/teto, porta, janela ou prompt;
- [ ] player entra e sai do Quarto 201 sem prender;
- [ ] Visible Collision Shapes confirma que a cama não criou collider;
- [ ] energia, Quarto 203, IA, duas perseguições, safe zone e eventos ambientais continuam funcionando;
- [ ] não há crash, teleporte entre andares ou alteração no deck/varanda.

## Problemas encontrados

O placeholder original possuía um `StaticBody3D`; apenas ocultá-lo manteria sua colisão ativa. O corpo foi preservado no backup com layer/mask zerados e `CollisionShape3D` desativado. Nenhum problema de escala, composição do GLB ou dependência externa foi encontrado. O resultado visual e a regressão completa de gameplay permanecem pendentes do playtest manual.

## Decisão para a Sprint 30B

Não expandir o kit até o checklist manual acima ser aprovado. Se o piloto passar, a próxima sprint pode substituir gradualmente outras camas ou importar um segundo prop, sempre um tipo por vez e com rollback localizado.
