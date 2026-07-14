# Sprint 31B — art pass pesado de degradação

## Escopo implementado

Todo o passe está isolado em `World/VisualPolish/Sprint31B_HeavyDegradation`. A camada não contém colisão, navegação, interação, trigger, corpo físico ou mudança estrutural. O deck congelado da varanda não é lido nem modificado.

### Materiais e sobreposições

- `Decay_HeavyMold`: mofo escuro concentrado em cozinha, banheiro, lavanderia e bases de paredes úmidas.
- `Decay_PeeledPlaster`: variação opaca de reboco gasto em paredes secas selecionadas.
- `Decay_SootAndGrime`: sujeira escura em regiões altas das paredes úmidas.
- `Decay_CeilingCrack`: rachaduras finas visuais em recepção, cozinha, salão superior e banheiro.
- `Decay_CeilingDampHalo`: halos irregulares de infiltração nos tetos da recepção, cozinha, banheiro, corredor e salão superior.
- `Decay_VerticalWaterStreak`: escorridos verticais alternados para quebrar a repetição das paredes.
- `Window_DirtyNightGlass`: vidro noturno escurecido e encardido nos Quartos 201 e 202.
- `Furniture_OldDustPatina`: camada transparente de poeira/madeira envelhecida sobre móveis Blender, preservando os materiais e texturas originais.
- Decals Blender `prop_mold_stain_decal_01` e `prop_wall_scratch_decal_01` reforçam infiltração e arranhões sem colisão.
- Decals Blender `prop_drag_mark_decal_01` reforçam desgaste de circulação e arrasto no piso.
- Oito faixas visuais escuras reforçam sujeira acumulada nas bordas de recepção, cozinha, banheiro, Quarto 201 e corredores.

### Dressing visual sem colisão

- Panos sujos: recepção, salão superior e banheiro.
- Papéis espalhados: recepção, cozinha, salão superior e Quarto 204.
- Garrafas antigas: recepção, cozinha, salão superior e escritório.
- Caixas de madeira: Quarto 102 e lavanderia, ambas em cantos.
- Tábua: cozinha, junto à parede e fora de portas/interações.
- Cortina rasgada adicional: janela do Quarto 202. O Quarto 201 preserva a cortina Blender já existente da Sprint 30B, sem duplicação.

Ralo do banheiro, painel técnico, fusíveis, arame, bilhete do Quarto 203, portas e eixo central de circulação foram deliberadamente mantidos livres.

### Iluminação

- Recepção: ponto quente fraco com oscilação sutil, sem estrobo e sem sombra dinâmica.
- Cozinha: ponto quente fraco e localizado.
- Salão superior: preenchimento frio de baixa energia.
- Banheiro: preenchimento frio e úmido de alcance curto.

As quatro luzes usam energia máxima inferior a `0,4`, alcance máximo inferior a `7 m` e não projetam sombras dinâmicas.

O ambiente da cena oficial passou de energia `0,30` para `0,27`; a exposição passou de `0,82` para `0,79`. A redução é deliberadamente pequena para aumentar contraste sem eliminar a leitura da navegação ou substituir a lanterna.

## Nós e cenas alterados

- `World/VisualPolish/Sprint31B_HeavyDegradation`: única árvore nova de degradação e dressing.
- `World/VisualPolish/Sprint30A_BlenderAssetPilot` e `World/VisualPolish/Sprint30B_BlenderProps`: somente `MaterialOverlay` visual de pátina nos meshes; transforms, scripts e colisores permanecem intactos.
- `World/VisualPolish/Sprint27_FakeWindowsLighting`: somente pátina escura nos meshes das janelas existentes.
- `Pension_WorldEnvironment`: apenas energia ambiente e exposição.

Resultado automático atual: 58 sobreposições de parede, 16 detalhes de piso, 11 detalhes de teto, 12 props leves, 2 vidros encardidos, 4 luzes locais e 280 submeshes visuais existentes com pátina. Nenhum node físico ou de gameplay foi criado.

## Etapa futura recomendada no Blender

- Materiais PBR autorais com mapas de albedo, roughness e normal para reboco descascado e madeira úmida.
- Variações modulares de canto/rodapé com infiltração não repetitiva.
- Cortinas, tecidos e colchões com silhueta mais orgânica e rasgos modelados.
- Props de cozinha e banheiro com ferrugem, gordura e calcificação incorporadas ao UV.
- Decals atlas próprios da Pensão Santa Luzia para reduzir a aparência procedural mantendo a geometria validada.

## Validação

Automática obrigatória: build, carga da cena oficial, F9 sem erros/avisos, deck 49/49, paredes superiores pareadas e auditoria da árvore 31B sem física/gameplay.

Manual obrigatória antes de commit: percorrer os dois andares em todas as direções, verificar portas e prompts, concluir puzzle do ralo/painel/Quarto 203, validar perseguições, safe zones, IA, retorno ao térreo e confirmar ausência de props flutuando, cruzados ou bloqueando circulação.
