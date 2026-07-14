# Sprint 30B — substituição dos props principais por Blender

## Escopo preservado

A Sprint 30B é exclusivamente visual. Não altera paredes, pisos, tetos, escada, varanda/deck, portas físicas, navegação, puzzle, IA, perseguições, safe zones, triggers ou Quarto 203. O checkpoint anterior à implementação é `dbfa007`.

## GLBs importados e cenas reutilizáveis

Todos os 13 GLBs solicitados existiam em `assets/models/props/pensao/` e receberam uma cena em `scenes/props/pensao/`:

| GLB | Cena |
|---|---|
| `prop_nightstand_old_01.glb` | `PropNightstandOld01.tscn` |
| `prop_broken_chair_01.glb` | `PropBrokenChair01.tscn` |
| `prop_reception_counter_01.glb` | `PropReceptionCounter01.tscn` |
| `prop_electric_panel_old_01.glb` | `PropElectricPanelOld01.tscn` |
| `prop_window_old_01.glb` | `PropWindowOld01.tscn` |
| `prop_kitchen_table_old_01.glb` | `PropKitchenTableOld01.tscn` |
| `prop_kitchen_stove_old_01.glb` | `PropKitchenStoveOld01.tscn` |
| `prop_sink_old_01.glb` | `PropSinkOld01.tscn` |
| `prop_bucket_old_01.glb` | `PropBucketOld01.tscn` |
| `prop_old_suitcase_01.glb` | `PropOldSuitcase01.tscn` |
| `prop_torn_curtain_01.glb` | `PropTornCurtain01.tscn` |
| `prop_drain_old_01.glb` | `PropDrainOld01.tscn` |
| `prop_broken_mirror_01.glb` | `PropBrokenMirror01.tscn` |

Cada wrapper usa `Node3D > Visual > GLB`, `OptionalCollision` desativado e `Metadata`. Nenhum wrapper possui `RigidBody3D`, colisão automática, trimesh, convex, câmera ou luz.

## Organização e substituições

O container oficial é `World/VisualPolish/Sprint30B_BlenderProps`, com os setores `Reception`, `Kitchen`, `Bathroom`, `TechnicalRoom`, `Bedrooms`, `Windows`, `Backup_Placeholders_Sprint30B` e `DebugMarkers`.

- Recepção: balcão e cadeira substituem os placeholders nas mesmas coordenadas.
- Cozinha: mesa, fogão, pia e balde substituem mesa/fogão/counter/balde blockout.
- Banheiro: ralo, espelho quebrado e balde substituem somente os visuais antigos.
- Sala técnica: o painel Blender substitui apenas `PanelVisual`; `TechnicalPanel`, `InteractionArea` e `UpperWingPuzzleInteraction` permanecem intactos.
- Quartos: mala do 201 e criado-mudo do 204 substituem placeholders existentes.
- Janelas: somente as janelas falsas já aprovadas dos Quartos 201 e 202 foram substituídas; a cortina rasgada Blender acompanha a janela do 201.
- Quarto 203: nenhuma instância, visual, porta, narrativa, interação ou trigger foi alterado.

O script `Sprint30BBlenderProps` aplica o mapa após os builders terminarem. Para props físicos antigos, somente as `GeometryInstance3D` são ocultadas; os `BoxShape3D` aprovados continuam sendo a autoridade física. Para props visuais, o node antigo recebe `visible=false`. O container de backup registra os setores e a quantidade de placeholders ocultados para rollback.

## Colisão

Novas colisões na Sprint 30B: **zero**.

- Props pequenos, decorativos e de parede: sem colisão.
- Balcão, cadeira, mesa e fogão: reutilizam os colliders simples já validados dos placeholders, sem criar shape novo.
- Painel e ralo: os `Area3D` funcionais antigos permanecem em seus nodes originais; o GLB é somente visual.

## Escala e alinhamento

Todos os wrappers usam escala `1:1`. Os 13 GLBs possuem base em `Y=0` e dimensões coerentes em metros. As instâncias de piso usam o `Y` do piso existente; janelas, cortina, espelho e painel recebem somente a rotação de parede necessária. Nenhum modelo foi colocado por tentativa em área nova: cada posição vem de um placeholder ou janela falsa existente.

## Validação automática

- [x] `dotnet build`: 0 erro / 0 aviso.
- [x] Importação dos 13 GLBs e carga das 13 cenas reutilizáveis.
- [x] Cena oficial carregada em Godot headless.
- [x] F9: `0 ERROR / 0 WARNING`.
- [x] Deck congelado: `49/49`.
- [x] 38 paredes superiores com colliders locais pareados.
- [x] Árvore 30B com 15 instâncias visuais e zero node de física/gameplay.
- [x] 27 visuais placeholder ocultados; colliders e lógica preservados.
- [x] Painel técnico e ralo mantêm suas `InteractionArea` originais.

## Playtest manual obrigatório

- [ ] Materiais, escala, apoio no piso e orientação dos 15 props.
- [ ] Nenhum placeholder duplicado ou z-fighting.
- [ ] Recepção e cozinha continuam navegáveis; prompt da cozinha funciona.
- [ ] Ralo permanece visível e retira a chave com o arame.
- [ ] Painel continua destravando e aceitando os dois fusíveis.
- [ ] Quartos 201/202 continuam livres e suas janelas não atravessam parede.
- [ ] Quarto 203 e seu evento permanecem intactos.
- [ ] IA, duas perseguições, safe zone, esconderijos e eventos ambientais funcionam.
- [ ] Visible Collision Shapes confirma zero collider novo e preservação dos shapes antigos.
- [ ] Retorno ao térreo sem teleporte, queda, player preso ou crash.

O commit final `feat: replace main pension placeholders with blender props` só pode ser criado após aprovação desse percurso manual.

## Próximo passo sugerido

Após validação, substituir gradualmente os props restantes ou iniciar refinamento de materiais/texturas. Não ampliar a Sprint 30B para o Quarto 203 nem para geometria estrutural.

