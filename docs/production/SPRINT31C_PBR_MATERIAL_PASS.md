# Sprint 31C-1 — Materiais mestres PBR da Pensão Santa Luzia

## Escopo e segurança

Passe exclusivamente visual aplicado às superfícies existentes da cena oficial. Nenhum transform estrutural, parede, piso, teto, porta, escada, collider, navegação, interação, puzzle, IA, perseguição, safe zone ou trigger foi criado ou alterado. O `UpperWing_CollisionDeck` permanece explicitamente excluído; apenas o visual de `SecondFloor_MasterSlab` pode receber o acabamento frio da varanda.

A implementação fica isolada em `World/VisualPolish/Sprint31C_PBRMaterials`. Os detalhes adicionados são `MeshInstance3D` finos, sem física, sem colisão e sem interação.

## Materiais mestres

- `M_RebocoSeco_Master.tres`: reboco bege/cinza sujo, pintura gasta, roughness 0,94 e normal moderada.
- `M_RebocoUmidoMofado_Master.tres`: reboco úmido verde-escuro, manchas fortes, roughness 0,98 e normal mais intensa.
- `M_MadeiraVelha_Master.tres`: tábuas escuras, frestas, veios, desgaste e manchas de água; roughness 0,93.
- `M_TetoInfiltrado_Master.tres`: tinta velha, halos de infiltração, escurecimento e normal sutil; roughness 0,97.

Todos usam albedo, roughness e normal map, projeção triplanar e paleta coerente com a pensão: bege sujo, cinza envelhecido, marrom escuro, verde mofo e preto úmido.

## Texturas e origem

Não foi baixada textura aleatória nem conteúdo externo. As quatro bases foram geradas para o próprio projeto com o gerador de imagens integrado, como scans planos e repetíveis, sem iluminação direcional, perspectiva, texto ou objetos. Os mapas de normal e roughness foram derivados localmente e de forma determinística a partir dessas bases.

- Paredes: `T_RebocoSeco_*` e `T_RebocoUmidoMofado_*`.
- Piso: `T_MadeiraVelha_*`.
- Teto: `T_TetoInfiltrado_*`.
- `assets/textures/pensao/README.md` registra a proveniência.

## Aplicação por área

- Paredes de quartos, recepção e corredores: alternância controlada entre reboco seco gasto e reboco úmido mofado.
- Cozinha, banheiro, lavanderia e superfícies externas: versão úmida pesada.
- Pisos internos e quartos: madeira velha escura; cozinha/banheiro recebem modulação úmida.
- Varanda: acabamento visual frio e bruto somente no slab visual, sem tocar o deck físico congelado.
- Tetos: material infiltrado nos quartos, corredores, cozinha e banheiro.

## Decals visuais

Foram criadas seis famílias RGBA grandes, aplicadas alguns milímetros à frente da superfície para evitar z-fighting:

- `Decal_Mofo_Rodape_01`
- `Decal_Umidade_Canto_01`
- `Decal_Reboco_Descascado_01`
- `Decal_Infiltracao_Vertical_01`
- `Decal_Teto_Mancha_Agua_01`
- `Decal_Piso_Sujeira_Canto_01`

### Hotfix prioritário após revisão em vídeo

A revisão de 2026-07-14 mostrou que as primeiras 14 instâncias, implementadas como caixas muito finas, apareciam em movimento como placas opacas, travessas brancas e retângulos fora da superfície. Elas deixaram de ser instanciadas. As seis texturas permanecem no projeto apenas como fonte para uma futura reconstrução com transparência feathered/`Decal3D`, depois de o material-base ser validado.

Também foram ocultados quatro contêineres antigos de overlays com borda geométrica dura (`DampPatches`, `CeilingStains`, `WallDecay` e `CeilingDecay`). Os props seguros, sujeira de piso e demais ambientações permanecem.

Os quatro materiais mestres agora usam projeção triplanar em coordenadas globais, evitando que a textura reinicie em cada bloco vizinho e forme linhas de divisão.

## Extensão brasileira exclusiva do primeiro andar

Após a validação dos artefatos do corredor superior, o segundo andar foi preservado e o trabalho passou a ser isolado no térreo. Foram criados:

- `M_Terreo_RebocoMofadoBR_Master.tres`: reboco de cal bege, descascado, com mofo verde-escuro e umidade irregular. A base foi refeita sem gradiente vertical e processada como tile contínuo para eliminar a linha que denunciava duas imagens;
- `M_Terreo_AssoalhoBR_Master.tres`: tábuas largas, escuras e foscas, com frestas, pregos, rachaduras, desgaste e manchas de água; não usa aparência de laminado moderno;
- `M_Terreo_TetoRebocoInfiltrado_Master.tres`: teto de reboco claro e velho, com halos de água, bolhas, mofo leve e fissuras, sem madeira;
- `M_Entrepiso_TerreoTeto_SuperiorAssoalho.tres`: material de duas faces para a laje já existente. A face superior preserva a madeira aprovada; somente a face inferior, vista pelo primeiro andar, recebe o teto de reboco infiltrado.

A seleção usa pertencimento explícito de andar e altura global por tipo de superfície. Paredes do térreo recebem um único material alinhado ao mundo, em vez da alternância seco/úmido que produzia uma fronteira horizontal. Nenhum mesh, transform ou collider foi criado, removido ou movido.

### Fechamento e acabamento da entrada

- `Shell_FacadeUpper_FrontLeft`, `Shell_FacadeUpper_FrontRight` e o parapeito frontal agora entram explicitamente na família de parede e recebem o mesmo reboco mofado do térreo, sem alterar as janelas falsas existentes.
- O vão antigo de 5,2 m da fachada mantinha duas aberturas genéricas ao lado da passagem real. Foram adicionados apenas `Wall_Exterior_Entrance_Infill_Left` e `Wall_Exterior_Entrance_Infill_Right`, calculados pela diferença entre `MainEntryWidth` e `DoorWidth`. Cada peça usa `AddWall`, com um único BoxMesh e um único BoxShape3D filho do mesmo tamanho; a passagem central permanece livre.
- As soleiras e o `Floor_Deposit_Interior` já existiam, mas seus MeshInstance3D filhos não eram reconhecidos como piso. A classificação agora considera pais `Floor_*` somente abaixo de Y 1,35, fazendo as soleiras se integrarem ao assoalho e aplicando a textura ao depósito sem afetar os pisos superiores.
- A revisão interna da recepção identificou que `Wall_Reception_Left` e `Wall_Reception_Right` avançavam cerca de quatro metros além da parede frontal e formavam duas pontas sem função. As duas paredes laterais agora terminam na linha Z 1,2 da entrada; `Wall_Reception_SouthLeft` e `Wall_Reception_SouthRight` foram prolongadas de forma reta até a casca externa. A passagem central preserva exatamente os 1,40 m originais. Cada uma das quatro peças continua sendo criada por `AddWall`, com BoxMesh e BoxShape3D filhos do mesmo tamanho.
- O playtest em vídeo revelou ainda oito paredes antigas sob `PensionGroundFloor/VarandaWalls`, logo após a porta térrea. Laterais, fundos, frentes e cantos formavam bolsões, pontas e corredores sem função. O builder desse conjunto residual foi removido; a casca externa e as paredes retas da recepção permanecem como fechamento oficial.

## Iluminação

Após o vídeo mostrar perda severa de leitura, o ambiente foi recalibrado para `background energy 1,00`, `ambient energy 0,34` e exposição `0,90`. O objetivo é preservar o terror e o contraste dos materiais escuros sem deixar o térreo quase preto. Luzes, ranges, sombras e scripts de gameplay existentes não são substituídos.

## Extensão do poço da escada e chegada ao segundo andar

Para evitar repetição sem introduzir uma família artística desconectada, foram reutilizados os materiais PBR licenciados já presentes no projeto, com duas variações locais:

- poço da escada: reboco mais escuro, úmido e esverdeado, piso envelhecido e teto infiltrado, reforçando a sensação opressiva da transição;
- chegada ao segundo andar: reboco mais seco, frio e desbotado, madeira menos saturada e teto acinzentado, criando uma mudança perceptível de ambiente;
- superfícies-alvo: `StairWell_Wall_West`, fechamentos do poço, `StairWell_FloorVisual`, `Ceiling_Transition`, `UpperLanding_BackSeal`, paredes de transição leste/oeste, `UpperLanding_Main`, `UpperLanding_StairBridge` e tetos do patamar;
- degraus, corrimãos, portas, meshes de colisão e o `UpperWing_CollisionDeck` congelado não foram alterados.

Nenhuma textura externa aleatória foi adicionada nesta extensão. Novos mapas só serão necessários numa futura etapa hero/Blender, caso a variação dos materiais existentes ainda fique visivelmente repetitiva durante o playtest.

## Validação automática

O `LevelSanityChecker` valida a presença dos quatro materiais originais, dos quatro materiais da extensão do térreo, das seis texturas de decal, dos três contêineres visuais e a ausência de física, gameplay, navegação e luzes próprias dentro da Sprint 31C.

Estado após execução automática em 2026-07-14:

- [x] importação dos recursos sem erro
- [x] build C# 0 erro / 0 aviso
- [x] cena oficial carrega sem crash
- [x] F9 0 ERROR / 0 WARNING
- [x] deck superior 49/49 e 38 paredes superiores pareadas
- [x] hotfix prioritário: 104 paredes, 6 pisos, 16 tetos e 1 slab visual; triplanar global, 4 overlays rígidos ocultos e 0 decals-caixa ativos
- [x] extensão brasileira do térreo: 50 paredes, 10 pisos, 5 placas de teto e 1 laje com material de duas faces; fachada superior frontal, soleiras e depósito incluídos; segundo andar preservado
- [x] entrada interna da recepção retificada: duas pontas laterais eliminadas, vãos laterais fechados, passagem central de 1,40 m preservada e quatro pares mesh/collider verificados
- [x] oito paredes residuais de `VarandaWalls` removidas do térreo; iluminação recuperada para 1,00/0,34/0,90
- [x] build C# após a extensão escada/patamar: 0 erro / 0 aviso
- [ ] reiniciar a cena e confirmar F9 após carregar a extensão escada/patamar
- [ ] playtest manual F6 completo

## Pendências futuras para Blender

- Geometria real de reboco inchado/descascado em pontos hero, sem alterar o blockout jogável.
- Tábuas individualizadas e bordas quebradas para pisos de primeiro plano.
- UVs dedicadas por cômodo para eliminar repetição em tomadas cinematográficas.
- Decals com malha conformada para cantos complexos.
- Revisão final de texel density e bake PBR quando os módulos arquitetônicos definitivos substituírem o blockout.

O checkpoint de continuidade pode ser commitado para transferência entre computadores. A aprovação final da Sprint 31C continua condicionada ao F9 limpo e ao playtest manual exigido pelas regras do projeto.
