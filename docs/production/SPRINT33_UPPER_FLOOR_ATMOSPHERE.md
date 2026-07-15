# Sprint 33 — ambientação pesada do segundo andar

## Escopo

Passe exclusivamente visual aplicado em `World/VisualPolish/Sprint33_UpperFloorAtmosphere` na cena oficial da Pensão Santa Luzia. A implementação troca somente `MaterialOverride` de meshes visuais já existentes no segundo andar e acrescenta overlays finos, sem colisão, além de duas luzes de preenchimento fracas.

O primeiro andar, transforms, geometria, colisões, navegação, paredes, pisos, tetos, portas, escada, varanda, puzzle, painel, fusíveis, ralo, IA, perseguições, safe room e triggers não foram alterados.

## Materiais criados

- `M_Upper_Wall_RebocoMofado.tres`: reboco/cal brasileiro envelhecido, fosco e mofado.
- `M_Upper_Floor_CeramicaAntiga.tres`: cerâmica brasileira de alto tráfego inspirada no fim dos anos 1980/início dos 1990, fosca, rachada e encardida.
- `M_Upper_Ceiling_Infiltrado.tres`: teto escurecido e infiltrado, sem brilho.
- `M_Upper_Varanda_Mureta_RebocoPodre.tres`: reboco podre de chuva para a mureta visual superior.

Todos ficam em `assets/materials/pensao/upper_floor/` e usam mapas próprios do segundo andar. As áreas molhadas e o Quarto 203 recebem variações runtime mais escuras, derivadas desses materiais exclusivos.

## Texturas criadas

Foram criados dez mapas em `assets/textures/pensao/upper_floor/`: albedo, roughness e normal para parede mofada, teto infiltrado e mureta de reboco podre, além do novo albedo de cerâmica antiga. As famílias de parede/teto/mureta foram derivadas das bases PBR brasileiras já validadas; o piso cerâmico foi gerado especificamente para o segundo andar, evitando repetição com o térreo.

## Decals aplicados

Foram criadas oito texturas exclusivas em `assets/decals/pensao/upper_floor/`. A primeira aplicação usava doze overlays-caixa, posteriormente removidos após o playtest revelar retângulos cinza/brancos sobre portas e no corredor:

- mofo de rodapé e de canto;
- reboco descascado grande;
- infiltração vertical;
- manchas de teto;
- poeira acumulada no piso;
- marca de arrasto do Quarto 203;
- umidade próxima a janelas.

As imagens permanecem disponíveis para uma futura implementação com projetores próprios. Nenhum overlay é instanciado atualmente; os containers de decals ficam vazios, sem `StaticBody3D`, `CollisionShape3D`, `Area3D` ou navegação.

## Hotfix visual do piso e artefatos

- removidos os doze overlays-caixa que apareciam como blocos claros sobre as portas 201/202 e no corredor;
- criada a textura `upper_floor_ceramica_anos80_albedo.png`, diferente do assoalho do térreo;
- cerâmica aplicada de forma contínua às placas internas do corredor, quartos, áreas comuns e à ponte da escada;
- acabamento fosco com roughness `0,96`, rejunte escuro, peças bege/caramelo, rachaduras, lascas e sujeira acumulada;
- material de duas faces: cerâmica somente em cima e o reboco infiltrado aprovado preservado na face inferior vista pelo térreo;
- placas do corredor/patamar que antes permaneciam cinzas agora entram no mesmo material world-aligned, eliminando a diferença visual nas sobreposições;
- `UpperWing_CollisionDeck`, transforms e colisões permaneceram intocados.
- vergas superiores das portas 201 e 202 incluídas explicitamente no material de reboco mofado, eliminando os dois blocos cinza claros sem remover a geometria da passagem.
- duas faixas estreitas de borda do piso receberam reboco de parede para não aparecerem atravessando a fachada;
- quatro guardas estruturais da caixa da escada mantiveram geometria e colisão, mas trocaram o cinza limpo por reboco podre coerente com a ala.
- hotfix autorizado em vídeo removeu os pares visual/colisão `Floor_Second_Main_NorthCap` e `Floor_Second_Main_NorthWestCap`, que criavam uma plataforma escalável atrás da parede da escada;
- o par duplicado `UpperLanding_Main` também foi removido: `Floor_Second_Main_South` já cobre integralmente o mesmo footprint e a coplanaridade era a origem da textura piscando.
- a ponte `UpperLanding_StairBridge` foi encurtada até a borda exata do piso principal, eliminando os 0,79 m de sobreposição que ainda alternavam madeira e cerâmica;
- o shader dual-face descarta somente as faces laterais finas das caixas de piso, removendo as molduras claras vistas no poço da escada sem tocar na face superior, no teto inferior ou na colisão.

## Áreas do segundo andar atendidas

- corredor superior;
- quartos 201, 202 e 203;
- banheiro, lavanderia e sala técnica;
- paredes, pisos e tetos da ala superior;
- muretinha/guarda-corpo visual da varanda superior.

Materiais antigos do segundo andar são substituídos em runtime pelos materiais exclusivos da Sprint 33. Nenhum recurso compartilhado foi apagado ou movido. `UpperFloor_DeprecatedOldTextures` permanece vazio e documentado, pois não foi encontrado material legado exclusivo que pudesse ser removido com segurança.

## Iluminação

Duas luzes frias, fracas e sem sombras foram adicionadas somente ao segundo andar: uma no corredor e outra nos quartos traseiros. Elas preservam a navegação e servem para revelar normal/roughness sem lavar as novas superfícies.

## Validação

Estado em 2026-07-14:

- checkpoint anterior criado no commit `5afbd40`;
- build C#: aprovado com 0 erros e 0 avisos;
- importação das novas texturas: concluída;
- carga headless da cena oficial e auditoria Sprint 33: `0 ERROR / 0 WARNING`;
- primeiro andar atingido pelo filtro Sprint 33: 0 meshes;
- nós de colisão/navegação/gameplay criados: 0;
- `UpperWing_CollisionDeck`: explicitamente excluído e não alterado;
- grade automatizada do deck superior: 49/49 pontos aprovados;
- inspeção visual no editor e playtest manual completo: pendentes;
- commit final da sprint: bloqueado até F9 limpo e aprovação manual de circulação, puzzle, IA, perseguições e safe room.

## Problemas encontrados

- Os materiais anteriores eram compartilhados entre áreas; por isso não foram editados. Foram criadas cópias exclusivas para o segundo andar.
- O projeto registrava uma repetição diferida de dois diagnósticos visuais da Sprint 31C relativos à escada/patamar. Os metadados declarativos foram alinhados aos cinco elementos de cada perfil já aplicados em runtime; a carga final ficou limpa, sem modificar materiais ou geometria da escada.
