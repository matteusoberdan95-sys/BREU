# BREU — Estado do projeto

## Sprint 33 — ambientação pesada do segundo andar

**Status em 2026-07-14:** implementação visual concluída; build, importação e carga headless com `0 ERROR / 0 WARNING` aprovados; inspeção visual e playtest manual ainda obrigatórios antes do commit final.

**Hotfix de revisão em 2026-07-15:** removida a plataforma física residual `NorthCap/NorthWestCap` atrás da caixa da escada e removido o piso duplicado `UpperLanding_Main`, que estava coplanar ao `Floor_Second_Main_South` e causava a textura piscando. O piso principal que sustenta a chegada, a escada e o `UpperWing_CollisionDeck` permanecem intactos.

**Revisão complementar em vídeo:** a ponte da escada foi encurtada para terminar exatamente na borda do piso principal e recebeu a mesma cerâmica do segundo andar. As faces laterais das caixas de piso deixaram de renderizar, eliminando as molduras claras do poço sem alterar colisões ou as faces caminháveis.

- Passe isolado em `World/VisualPolish/Sprint33_UpperFloorAtmosphere`, limitado a materiais e duas luzes fracas do segundo andar; overlays-caixa removidos após revisão visual.
- Primeiro andar preservado: o filtro da sprint não alcança nenhuma superfície térrea e nenhum material compartilhado foi editado.
- Segundo andar recebeu famílias PBR exclusivas de reboco/cal mofado brasileiro, cerâmica antiga bege/caramelo, teto infiltrado e reboco podre da muretinha superior.
- Corredor, quartos 201/202/203, banheiro, lavanderia, sala técnica e varanda receberam variações de umidade; dez placas de piso usam cerâmica brasileira antiga e podre somente na face superior. A face inferior mantém o teto de reboco infiltrado aprovado no térreo.
- Quarto 203 usa parede mais escura, marca de arrasto e teto manchado, sem cobrir puzzle ou interações.
- Gameplay preservada: zero colisões, navegação, triggers, portas, IA, perseguições, safe room ou puzzle adicionados/alterados.
- Documentação completa: `docs/production/SPRINT33_UPPER_FLOOR_ATMOSPHERE.md`.

**Próxima trava:** executar F9 até `0 ERROR / 0 WARNING` e fazer o playtest manual completo do térreo, escada, varanda e ala superior. Não criar o commit `feat: add heavy upper floor atmosphere pass` sem essa aprovação.

## Sprint 32B — telhado colonial externo visual

**Status em 2026-07-14:** casca visual implementada e validação automática específica aprovada; playtest manual externo/interno e F9 final ainda obrigatórios antes do commit da sprint.

- Quatro GLBs do kit colonial foram encapsulados em cenas reutilizáveis e instanciados exclusivamente em `World/ExteriorShell/Sprint32B_RoofVisualShell`.
- Após o primeiro vídeo externo, a pequena cobertura frontal que parecia uma placa flutuante foi ocultada; o telhado principal foi ampliado para `15,12 m`, ganhou beiral nas duas laterais e avançou até cobrir corretamente a linha frontal.
- Telhado principal, cobertura frontal, telhas quebradas e musgo usam apenas meshes visuais; não existe colisão, física, navegação, câmera, luz ou gameplay novo no container.
- Materiais incorporados de barro antigo, madeira velha, sujeira e musgo foram preservados por já terem metalicidade zero e roughness alta.
- Interior, paredes, pisos, tetos internos, escada, varanda, portas, `UpperWing_CollisionDeck`, puzzle, IA, perseguições, safe room e triggers não foram alterados.
- Importação Godot e carga headless concluídas; build com 0 erros; auditoria 32B limpa, deck `49/49` e 38 paredes superiores pareadas.
- A primeira auditoria F9 da carga ficou em `0 ERROR / 0 WARNING`; uma repetição diferida no encerramento apontou dois erros preexistentes de ordem de aplicação dos perfis visuais 31C da escada/patamar. Repetir F9 no editor antes do commit.
- Documentação completa: `docs/production/SPRINT32B_ROOF_VISUAL_SHELL.md`.

**Próximo passo imediato:** inspecionar o telhado da trilha e de dentro da pensão, ativar Visible Collision Shapes, validar sombras/entrada/varanda e repetir F9. Após aprovação, criar o commit `feat: add external colonial roof visual shell to pension`.

## CHECKPOINT ATUAL — 2026-07-14

**Commit de continuidade autorizado pelo usuário.** Este checkpoint reúne as Sprints 30B, 31, 31B e 31C e os hotfixes visuais feitos durante a revisão em vídeo. Ele serve para retomada em outro computador; não significa que o playtest manual final foi concluído.

### Estado funcional preservado

- cena oficial: `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`;
- deck oficial da varanda: `UpperWing_CollisionDeck`, congelado e fora do passe visual;
- puzzle do ralo, painel, dois fusíveis e Quarto 203 preservados;
- IA, perseguições, safe zones, esconderijos, portas, escada física e triggers não foram redesenhados;
- último build C# deste checkpoint: **0 erros / 0 avisos** em 2026-07-14.

### Conteúdo incluído no checkpoint

- props Blender 30B organizados por setor, sem física nova e com placeholders substituídos ocultos;
- passes visuais 31/31B com desgaste, sujeira, dressing leve e iluminação contida;
- materiais PBR 31C para reboco seco, reboco úmido/mofado, madeira velha e teto infiltrado;
- extensão brasileira do térreo: parede mofada contínua, assoalho antigo, teto de reboco infiltrado e entrepiso com faces visuais diferentes;
- entrada/recepção retificada, paredes residuais de `VarandaWalls` removidas, barrancos externos recuados e salão de entrada ambientado;
- poço da escada com perfil mais úmido, escuro e opressivo;
- chegada ao segundo andar com perfil mais seco, frio e desbotado, evitando repetição direta do térreo.

### Validação ainda pendente após abrir em outro computador

1. Abrir o projeto no Godot Mono 4.7 e aguardar a importação dos PNGs/GLBs.
2. Rodar `dotnet build` e confirmar 0 erros / 0 avisos.
3. Rodar a cena oficial e executar o diagnóstico F9; confirmar 0 ERROR / 0 WARNING.
4. Fazer F6 manual completo usando `docs/testing/PENSION_UPPER_WING_PLAYTEST.md`.
5. Conferir especialmente: materiais no poço da escada, paredes/piso/teto do patamar superior, iluminação legível, ausência de emendas e ausência de objetos cobrindo prompts.
6. Só depois da aprovação visual decidir se serão criadas texturas hero novas ou refinamentos no Blender.

**Próximo passo recomendado:** validar visualmente a transição térreo → escada → segundo andar. Se a repetição ainda for perceptível, criar variações hero localizadas; não substituir toda a família PBR nem alterar geometria estrutural.

## Sprint 31C-1 — materiais mestres PBR

**Status:** incluída no checkpoint de continuidade; build C# aprovado, diagnóstico F9 e playtest manual da extensão escada/patamar ainda obrigatórios.

- Quatro materiais PBR mestres para reboco seco, reboco úmido/mofado, madeira velha e teto infiltrado.
- Bases visuais próprias do projeto, com mapas de normal e roughness derivados localmente; nenhum download externo.
- Aplicação triplanar em paredes, pisos e tetos existentes, sem alterar transforms ou geometria.
- Seis texturas-fonte de decals preservadas; as 14 instâncias-caixa foram desativadas após o vídeo revelar placas opacas e travessas brancas.
- Aplicação isolada em `World/VisualPolish/Sprint31C_PBRMaterials`; deck físico da varanda explicitamente excluído.
- Ambiente recalibrado para destacar relevo/roughness sem perder legibilidade.
- Documentação: `docs/production/SPRINT31C_PBR_MATERIAL_PASS.md`.
- Hotfix prioritário: projeção triplanar global para continuidade entre blocos e quatro contêineres antigos de manchas rígidas ocultos.
- Extensão visual exclusiva do térreo: reboco mofado brasileiro sem emenda horizontal, assoalho largo e muito gasto, teto de reboco infiltrado sem madeira e material de duas faces no entrepiso; segundo andar preservado.
- Build `0 erro / 0 aviso`, cena oficial, F9 `0 ERROR / 0 WARNING`, deck `49/49`, 38 paredes superiores pareadas e aplicação registrada em 104 paredes, 6 pisos e 16 tetos.
- Validação do térreo brasileiro: 50 paredes, 10 pisos, 5 placas de teto e 1 laje de duas faces. Fachada frontal superior texturizada, duas sobras laterais da entrada fechadas por paredes com colliders filhos pareados, soleiras integradas ao assoalho e depósito texturizado; jogabilidade e andar superior preservados.
- Entrada interna da recepção retificada: as paredes laterais não avançam mais além da linha frontal, os painéis frontais fecham os dois buracos até a casca externa e a passagem central mantém 1,40 m. As quatro paredes possuem BoxMesh/BoxShape3D locais e pareados; F9 permanece em `0 ERROR / 0 WARNING`.
- Revisão em vídeo do térreo: removido o builder legado `VarandaWalls`, responsável por oito paredes pontudas e corredores inúteis logo após a porta. Ambiente recuperado para background `1,00`, energia ambiente `0,34` e exposição `0,90`; aplicação PBR atual em 42 paredes térreas, 10 pisos, 5 tetos e 1 laje de duas faces.

O checkpoint pode ser compartilhado para continuidade, mas a Sprint 31C só deve ser marcada como visualmente aprovada após F9 limpo e playtest manual.

## Sprint 31B — degradação ambiental pesada

**Status:** implementação e validação automática concluídas; playtest manual pendente.

- Camada visual removível em `World/VisualPolish/Sprint31B_HeavyDegradation`.
- Mofo pesado, reboco gasto, fuligem, escorridos verticais, rachaduras, halos de teto, marcas de arrasto, sujeira de borda e vidros escurecidos.
- Dressing Blender leve nas bordas: panos, papéis, garrafas, caixas, tábuas e cortinas rasgadas.
- Pátina transparente aplicada aos móveis Blender e janelas falsas, preservando textura e função.
- Quatro luzes locais contidas: duas quentes fracas e duas frias; flicker sutil somente na recepção.
- Ambiente reduzido de `0,30` para `0,27` e exposição de `0,82` para `0,79`, mantendo legibilidade.
- Zero colisão, navegação, interação, trigger ou alteração estrutural; deck da varanda preservado.
- Build `0 erro / 0 aviso`, cena oficial, F9 `0 ERROR / 0 WARNING`, deck `49/49` e 38 paredes superiores pareadas.

O commit final permanece bloqueado até F9 limpo e playtest manual completo.

## Sprint 31 — material pass visual

**Status:** implementação e validação automática concluídas; F6 manual pendente.

- Materiais compartilhados de reboco envelhecido, piso gasto, teto úmido, rodapé e manchas.
- Aplicação visual em 95 paredes, 6 pisos e 16 tetos existentes.
- Acabamentos isolados em `World/VisualPolish/Sprint31_Materials`.
- Zero colisão, trigger, navegação ou gameplay novo; `UpperWing_CollisionDeck` preservado.
- Build 0/0, F9 0/0 e deck 49/49.

## Sprint 30B — props Blender principais por setor

Os 13 GLBs principais solicitados receberam cenas reutilizáveis e 15 instâncias visuais controladas em `World/VisualPolish/Sprint30B_BlenderProps`. A recepção recebeu balcão/cadeira; a cozinha, mesa/fogão/pia/balde; o banheiro, ralo/espelho/balde; a sala técnica, o visual Blender do painel; os quartos, mala/criado-mudo; e duas janelas falsas aprovadas foram substituídas, com cortina no Quarto 201. O Quarto 203 foi deliberadamente ignorado.

Não foi criada colisão nova. Balcão, cadeira, mesa e fogão preservam os `BoxShape3D` simples dos placeholders; painel e ralo preservam integralmente seus nodes funcionais, scripts e `Area3D`. Vinte e sete visuais antigos são ocultados após os builders, sem apagar lógica ou geometria estrutural.

Validação automática concluída: build `0 erro / 0 aviso`, cena oficial carregada, F9 `0 ERROR / 0 WARNING`, deck `49/49`, 38 paredes superiores pareadas e árvore 30B com zero física/gameplay. O playtest manual completo e Visible Collision Shapes continuam obrigatórios antes do commit final `feat: replace main pension placeholders with blender props`. Próxima sprint recomendada após aprovação: substituição gradual dos props restantes ou refinamento de materiais/texturas.

## Sprint 30A — primeiro asset Blender (piloto)

O primeiro asset Blender foi integrado de forma controlada: `prop_single_bed_old_01.glb` ganhou a cena reutilizável `PropSingleBedOld01.tscn` e uma única instância visual no Quarto 201, sob `World/VisualPolish/Sprint30A_BlenderAssetPilot`. O GLB mede aproximadamente `1,04 × 1,99 × 0,95 m`, usa escala `1:1`, contém 18 meshes/5 materiais e não trouxe câmera ou luz.

O piloto começa sem colisão, como o placeholder que substitui. As duas peças visuais antigas da cama permanecem ocultas no container de rollback `Backup_Placeholders_Sprint30A`; nenhum outro móvel do kit foi instanciado. Piso, parede, teto, porta, deck/varanda, puzzle, Quarto 203, IA, perseguições, safe zones, triggers e eventos não foram alterados.

Validação automática concluída: build `0 erro / 0 aviso`, importação e carga das duas cenas sem falha, F9 `0 ERROR / 0 WARNING`, deck `49/49` e auditoria do piloto com 18 meshes e nenhuma colisão ativa/câmera/luz. O playtest manual permanece obrigatório antes do commit final `feat: import blender bed prop pilot`. Próxima sprint somente após aprovação: substituição gradual de outras camas ou importação de um segundo tipo de prop.

## Sprint 28 — art pass leve dos cômodos

Foi criado o contêiner isolado `World/VisualPolish/Sprint28_LightArtPass`, dividido por recepção, cozinha, banheiro, sala técnica, quartos superiores, Quarto 203, corredores, manchas e panos/cortinas. Depois da revisão completa do vídeo e dos prints de playtest, o passe foi recomposto em escala maior: entrada com banco/bagagem/tapete, banheiro mais ocupado, metade leste do Quarto 204 mobiliada, escritório com arquivo lateral e sala técnica com bancada, armários, gerador e tubulação. Os dois quadros vistos de perfil, a prateleira atravessada, o banco/baú sobre o vão da escada e o conjunto apertado do escritório foram removidos pela origem.

O salão de chegada superior agora ocupa exclusivamente o `UpperLanding_Main` comprovado (`X -4,75..0,65`, `Z -22,60..-19,40`): tapete, sofá na borda norte, relógio de chão no canto sólido, oratório, retrato, castiçais e lustre visual. Uma passadeira e duas luminárias apenas visuais dão continuidade ao corredor. O eixo diagonal da saída da escada até a boca do corredor permanece vazio de móveis.

Nenhum `StaticBody3D`, `CollisionShape3D`, `Area3D`, corpo rígido, navegação, trigger ou luz nova foi criado. A única exceção autorizada foi mover o `TechnicalPanel` existente, junto da sua `InteractionArea` original, para a parede leste da mesma sala: isso libera visualmente a porta 205 sem alterar scripts, modos, estados ou regras do puzzle. Pisos, paredes, teto, portas, escada, deck/varanda, IA, perseguições, esconderijos e eventos permanecem inalterados.

Validação automática após a correção dos prints: cena oficial headless carregada, F9 `0 ERROR / 0 WARNING`, deck `49/49`, 38 paredes superiores com colisores locais pareados, árvore Sprint 28 com 213 meshes visuais/zero nós físicos e painel técnico isolado da porta 205 por `11,00 m`. A inspeção manual completa ainda precisa ser aprovada antes do commit final `feat: add light art pass to pension rooms`.

## Hotfix de fechamento — escada e forro frontal

As placas diagonais superdimensionadas `Stair_Stringer_Left/Right`, os guias residuais anteriores e o adereço legado de altura fixa `Stair_Handrail_Visual` foram removidos somente da escada da pensão. Permanecem os dois corrimãos antigos inclinados aprovados, cada um com cinco postes, barra superior e travessa intermediária, sempre com mesh e `BoxShape3D` pareados fora da faixa central da rampa. Degraus e rampa física permanecem intactos. A continuação visual `Ceiling_FirstFloor_TransitionFront` fecha a faixa entre a borda frontal da laje e a fachada com o mesmo perfil vertical de 0,6 m e a mesma cor da `SecondFloor_MasterSlab`, sem ficar pendurada abaixo do teto e sem adicionar collider ao deck congelado. O fechamento `Wall_Corr_North_Mid`, em `Z positivo`, passou de 0,55 m para 1,60 m, fechando exatamente a largura do corredor com mesh e collider pareados; as portas laterais do escritório e do Quarto 205 permanecem livres. A porta verde oficial da varanda continua no lado oposto, em `Z -7,55`. Nenhum sistema de gameplay, perseguição, puzzle, porta ou varanda foi alterado.

A descida da escada usa a rampa física aprovada sob os degraus visuais. O player agora mantém velocidade constante em inclinações e usa `FloorSnapLength = 0,5 m` (antes `0,1 m`), evitando perder contato com a rampa e disparar ciclos falsos de queda/aterrissagem ao descer andando ou correndo. Pulo intencional, gravidade, velocidades de caminhada/corrida e geometria da escada permanecem inalterados.

## Sprint 27A — expansão externa e ajuste fino de janelas falsas

A revisão visual removeu a janela sem função do corredor térreo e as duas janelas baixas da entrada que ficavam cortadas pelos barrancos. Quarto 102 e Quarto 201 agora mostram a janela na face interna da parede externa oeste; cozinha e Quarto 202 usam a face interna da parede externa leste. A grande parede oeste do setor superior recebeu duas janelas equilibradas, e as peças colocadas por engano nas paredes posteriores foram removidas. A varanda visual central e a entrada principal continuam livres e legíveis. O total ajustado das Sprints 27/27A é de 18 janelas falsas: 4 internas decorativas e 14 externas.

As peças foram acrescentadas ao container visual já integrado à cena oficial; nenhuma cena alternativa foi criada. Cada janela externa está apoiada em parede sólida existente ou, no frontal superior, nos shells visuais autorados da fachada. Não houve abertura real, alteração de parede, sala, colisão, navegação, piso, teto, porta, varanda, escada, puzzle ou gameplay.

Validação automática: build sem erros/avisos, cena oficial carregada, F9 `0 ERROR / 0 WARNING`, deck `49/49`, seis posições solicitadas presentes, três posições removidas ausentes e zero nós físicos no art pass. Inspeção visual manual interna/externa e regressão de gameplay permanecem pendentes.

## Sprint 27 — janelas falsas, frestas e leitura visual

Um art pass visual isolado adicionou cinco janelas falsas de madeira e vidro escuro, quatro frestas frias, cinco luzes locais fracas, cortinas/panos, umidade e sombras de grade na recepção, corredor térreo, escada, ala superior e Quarto 203. Todo o conteúdo vive em `World/VisualPolish/Sprint27_FakeWindowsLighting` e não possui colisão, navegação, interação ou física.

Nenhuma parede foi aberta, cortada, removida ou reconstruída. Pisos, tetos, deck/varanda, escada, corrimão, portas físicas, puzzle, IA, perseguições, esconderijos e eventos ambientais permanecem inalterados. A lanterna continua sendo a fonte principal; as luzes novas usam energia máxima de `0,16` e alcance máximo de `3,8 m`.

Validação automática: build C# sem erros/avisos, cena oficial headless, F9 `0 ERROR / 0 WARNING`, deck `49/49` e verificação específica com cinco janelas falsas/zero nós físicos. Playtest visual e regressão manual permanecem pendentes.

Próxima sprint recomendada após validação: art pass modular/Blender ou refinamento gradual da IA.

## Sprint 26 — eventos dinâmicos de terror ambiental

A pensão agora possui a árvore isolada `World/Gameplay/AmbientHorror`, com diretor, seis triggers pequenos, emissores 3D, visuais sem colisão e debug. São nove eventos sutis: rangido distante, pancadas superiores, passos, flicker, arranhado, objeto caindo, respiração atrás, sussurro do ralo e uma sombra rápida rara.

O sistema só habilita após o evento do Quarto 203/primeira presença, respeita progressão adicional (energia, chave do ralo e perseguições concluídas), executa somente um evento por vez e aplica cooldown global aleatório de 25–45 segundos. Chase, Search, esconderijo e safe zone bloqueiam completamente novos eventos. Não há `StaticBody3D`, dano, teleporte, empurrão, alteração de geometria, porta, IA ou puzzle.

Validação automática: build C# sem erros/avisos, cena headless carregada e verificação estrutural específica adicionada ao F9. Playtest manual aprovado pelo usuário em 2026-07-13 conforme `docs/testing/PENSION_AMBIENT_HORROR_PLAYTEST.md`.

## Sprint 25 — segunda perseguição real

Após a conclusão da Sprint 24, o fundo do térreo libera uma segunda perseguição one-shot. A presença interrompe a patrulha, reage ao ruído, segue a última posição conhecida somente pelo eixo central e busca o jogador sem entrar em quartos, escada ou segundo andar. Balcão e guarda-roupas encerram o fluxo após uma espera curta; a presença permanece sem collider, dano, morte ou teleporte.

O esconderijo passa a funcionar como mecânica real de sobrevivência, e o objetivo final orienta o jogador a procurar outra saída da pensão. Próxima sprint recomendada: eventos dinâmicos leves ou um novo objetivo de saída, preservando a geometria aprovada.

## Sprint 24 — IA básica da presença

Após `Sprint23Completed`, a silhueta da primeira presença passa a patrulhar apenas o eixo navegável do térreo entre recepção, entrada do corredor, corredor central e fundo da pensão. A IA possui visão curta por ângulo e raycast contra paredes, audição de corrida em até 8 m, alerta, busca na última coordenada segura do corredor e retorno à patrulha. `PlayerHidden` e `PlayerInSafeZone` cancelam detecção e fazem a presença se afastar; o inimigo continua sem collider físico, dano, morte, NavMesh ou acesso ao segundo andar.

Próxima sprint sugerida: segunda perseguição controlada e expansão gradual do comportamento, sem transformar esta patrulha em IA final.

## Hotfix pós-Sprint 23 — guarda-roupas e agachamento

Três guarda-roupas brasileiros antigos e reutilizáveis foram colocados no térreo: recepção, Quarto 102 e cozinha. Cada móvel possui shell visual com colliders filhos correspondentes, abertura real, `WardrobeSafeZone` interna e interação de esconderijo; não há teleporte nem bloqueador invisível. O teste de teto de `PlayerCrouch` agora verifica somente o volume adicional entre a postura agachada e em pé, evitando que a laje superior mantenha o jogador agachado em todo o interior.

## Sprint 23 — primeiro esconderijo / sala segura

A recepção agora possui o primeiro abrigo jogável, atrás do balcão existente. `SafeZone_FirstShelter` é uma `Area3D` térrea pequena e não bloqueadora; `Interact_FirstHidingSpot` permite esperar até os passos se afastarem sem teleporte ou perda permanente de controle. A zona integra-se ao estado da perseguição da Sprint 22, encerra uma perseguição ainda ativa, oculta a presença e conclui `Sprint23Completed` com o objetivo de investigar o que está caçando o jogador.

Próxima sprint recomendada: IA básica de patrulha, visão e audição simples, reutilizando `PlayerHidden` e as safe zones.

## Hotfix 22B — chave do ralo e dois fusíveis

O puzzle de energia superior agora exige três elementos reais: Chave Enferrujada retirada do ralo, Fusível Velho encontrado no depósito térreo e Fusível Superior encontrado na rouparia. A chave destrava a tampa do painel e cada fusível ocupa um slot próprio; `IsUpperPowerRestored` só ativa com painel destravado e ambos instalados.

O Quarto 203 continua dependendo da energia superior, e as progressões das Sprints 20, 21 e 22 permanecem encadeadas após o hotfix. Nenhuma geometria, colisão, porta física ou rota foi alterada.

## Sprint 22 — primeiro inimigo protótipo

O fundo do corredor térreo agora revela `Enemy_FirstPresence`, uma silhueta visual sem colisão que inicia a primeira perseguição curta do jogo. O movimento segue quatro pontos autorados pelo corredor até a borda da recepção, sem NavMesh, IA final, combate, dano ou interação física com o player.

A recepção funciona como zona segura e encerra o evento permanentemente, ensinando que fugir é necessário. A próxima sprint recomendada é um sistema simples de esconderijo e sala segura.

## Sprint 21 — descida e primeira presença

Após concluir o evento do Quarto 203 e descer a escada, o jogador encontra a primeira presença visual/sonora no térreo. Um trigger pequeno no pé da escada executa a sequência uma única vez: sons existentes, flicker localizado da recepção e uma sombra sem colisão no corredor.

Uma pista rasgada aparece na recepção depois da manifestação e aponta o jogador para o fundo da pensão. Não existe inimigo físico, IA, dano, perseguição ou teleporte. A próxima sprint recomendada é o primeiro inimigo protótipo com perseguição curta.

## Sprint 20 — clímax do Quarto 203

O puzzle da ala superior agora recompensa o jogador com a abertura forçada do Quarto 203, condicionada à energia superior restaurada e à chave suja retirada do ralo. A porta possui estados bloqueado, forçável e aberto; o blocker acompanha o painel e é desativado permanentemente ao abrir.

O interior local do 203 recebeu leitura visual claustrofóbica e uma página rasgada que dispara, uma única vez, um evento de madeira, luz e presença sonora. O evento não cria inimigo físico, não causa dano e não teleporta o player; ele prepara o protótipo da primeira presença perseguidora para a próxima sprint. Playtest manual da Sprint 20 permanece obrigatório antes do commit final.

## Sprint 19E — ala superior limpa

A ala superior possui ownership único em `UpperWingRooms`, corredor principal, quatro cômodos acessíveis, sala técnica, escritório e 205 trancado. O puzzle foi reorganizado com arame/fusível na Rouparia, ralo no Banheiro e painel acessível na Sala Técnica. As paredes têm colliders filhos correspondentes; os vãos reais do Escritório e 205 foram abertos após análise do vídeo, removendo paredes sólidas que bloqueavam as portas. A próxima sprint só pode começar após aprovação manual do playtest 19E.

## Sprint 19D — ownership estrutural da ala superior

O painel técnico voltou a ser um objeto montado e acessível dentro do `TechnicalRoom`. O quarto do arame e o banheiro/ralo estão fechados pelas paredes/tetos de `UpperWingRooms.tscn`. A geometria antiga concorrente foi removida de `BalconyWing.tscn`, que agora possui somente a porta verde. O próximo passo, após playtest manual, poderá ser refinar puzzle e ambientação em vez de corrigir a estrutura base.

## Sprint 19C — Correção estrutural da ala superior

**Status:** implementada (playtest F6 obrigatório)  
**Deck:** `UpperWing_CollisionDeck` **não alterado**.

Sprint 19B criou a ala, mas o playtest mostrou paredes atravessáveis e puzzle confuso. Sprint 19C corrigiu:
- arame torto movido da alcova/banheiro da BalconyWing (corredor fino + parede leste sem collider) para a **Rouparia**;
- paredes leste da BalconyWing agora têm collider filho (sem saída para limbo);
- painel técnico reposicionado perto da porta da TechnicalRoom (acessível sem atravessar parede);
- divisor 204/Tech unificado (removida fresta de 0,5 m);
- Room204 sul fechado com parede média.

Próxima sprint só após playtest completo do puzzle (F6 + Visible Collision Shapes).

## ✅ CHECKPOINT — Varanda aprovada para gameplay (CONGELADA)

**Data:** 2026-07-12  
**Status:** APROVADA E CONGELADA

Estado aprovado:
- Player anda na varanda/laje superior sem cair no limbo;
- teleporte térreo → segundo andar corrigido (`DebugFallRecovery` só abaixo de KillY);
- escada, porta verde e Quarto 203 funcionais;
- varanda aberta e navegável;
- chão oficial: `UpperWing_CollisionDeck` — **congelado**.

Congelado / proibido mexer:
- `UpperWing_CollisionDeck` (posição, shape, layer/mask, parent, nome);
- mureta, boundary global, guarda-corpo ou colliders soltos na varanda;
- paredes invisíveis na área caminhável.

Próximo passo permitido:
- cômodos novos da ala superior;
- cada parede visual com collider **filho** da mesh correspondente;
- nunca boundary/collider chutado no meio do caminho.

## Rollback — colliders de parede da varanda

Colisão de paredes da varanda (`BalconyWallColliders` / `BalconyWallCollider_*`) foi **revertida**: ficavam no meio do caminho. Varanda permanece aberta e navegável com `UpperWing_CollisionDeck` intacto. Próxima sprint: cômodos com paredes próprias e colliders **filhos das meshes visuais**, nunca boundary/collider lateral chutado.

## Hotfix — varanda limpa + isolamento do térreo

Boundary global da varanda (`BalconyBoundaryColliders` e filhos) removida; `UpperWing_CollisionDeck` permanece como chão navegável. O teleporte térreo → segundo andar vinha de `DebugFallRecovery` tratando o corredor como queda da laje; agora só ativa abaixo de KillY (`-3`). Regra de isolamento por andar documentada. Próxima sprint de cômodos/puzzle só libera se: (1) varanda livre sem placa/mureta bugada; (2) correr no térreo após abrir a varanda sem ser jogado para cima.

## Limites externos da varanda

A tentativa de limites contínuos foi revertida por poluir a circulação. Não recriar boundary global; paredes futuras virão dos cômodos.

## Fonte física da ala superior

`UpperWing_CollisionDeck` é a única fonte de colisão do piso/teto na área aberta superior. A laje visual não possui collider concorrente. O deck passou a grade automática 7×7 com 49/49 impactos, mas nenhuma expansão de cômodos pode seguir antes da caminhada e dos pulos manuais confirmarem que o recovery não é acionado.

## SecondFloor_MasterSlab

A laje física anterior foi removida. O teto da recepção e o piso da varanda agora são a mesma peça física mestre, `SecondFloor_MasterSlab`; o teto visual antigo da região foi retirado. Nenhuma expansão de cômodo pode continuar até a travessia integral e os pulos contra a face inferior passarem manualmente.

## Hotfix bloqueante — laje física do segundo andar

O playtest invalidou a laje anterior: o player ainda caía para o primeiro andar e atravessava o teto da recepção ao pular. A solução adotada é uma única caixa física grossa, `SecondFloor_PhysicalSlab`, substituindo `UpperWing_SolidFloor` e removendo mureta, rail e colisões associadas. Nenhuma expansão de cômodo pode continuar até caminhada, diagonais e pulos contra o teto passarem manualmente.

**Última atualização:** 2026-07-12  
**Fase:** REBOOT GREENFIELD — Sprint 18C; **varanda aprovada/congelada**; próxima: cômodos da ala superior  
**Baseline:** `docs/production/REBOOT_BASELINE_DECISION.md`

---

## Estado atual

| Item | Status |
|------|--------|
| Branch | `reboot/breu-clean-start` |
| Sprint 02–16E | **✅ Aprovadas** (ver histórico) |
| Sprint 17–17F | **✅ Varanda aprovada/congelada** — 203 acessível |
| Sprint 18A | **⏸️ Cômodos da expansão removidos provisoriamente na 18C** |
| Sprint 18B | **🔄 Absorvida / reforçada pela 18C** |
| Sprint 18C | **✅ Deck/varanda aprovados** |
| Sprint 19 | **🔄 Cômodos + fusível superior** — F6 pendente |

### Regra obrigatória (18C)

Antes de qualquer commit de cenário:

1. `docs/production/LEVEL_CHANGE_CHECKLIST.md`
2. **F9** `LevelSanityChecker` → **0 ERROR**

Docs:
- `docs/technical/PENSION_SCENE_OWNERSHIP.md`
- `docs/technical/PENSION_LEVEL_METRICS.md`
- `docs/production/LEVEL_CHANGE_CHECKLIST.md`

---

## Cena atual (F6)

**Vertical (térreo + 2º andar):** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

**Térreo baseline preservada:** `PensaoTerreoBlockout01.tscn`

**Lab escada:** `StairMovementLab.tscn`

**Baseline 2º andar:** `docs/technical/PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

---

## Sprint 10 — resumo (aprovada)

- Segundo andar aprovado como **blockout cinza navegável** — visualmente cru, funcionalmente validado.
- Player sobe escada, acessa piso superior, navega corredor + quartos 201/202 + porta bloqueada.
- Vão da escada com caixa (`StairBox_Wall_*`) + guarda-corpos (`Stairwell_Rail_*`).
- Térreo, puzzle depósito, HUD, PlayerController e camera feel **preservados**.
- Sem teto, sem telhado, sem arte final, sem inimigo.

**Playtest blockout:** `docs/testing/PENSAO_SECOND_FLOOR_BLOCKOUT_01_PLAYTEST.md`

---

## Sprint 11 — resumo (aprovada)

- Playtest fino do segundo andar — rota completa ida/volta validada.
- Escada, landing, corredor, quartos 201/202, porta bloqueada — OK.
- Regressão térreo, puzzle depósito, HUD, movimento — OK.
- Layout do 2º andar **congelado**; sem refatoração nesta fase.

**Playtest:** `docs/testing/PENSAO_SECOND_FLOOR_FINE_PLAYTEST.md`

---

## Baselines congeladas

| Sprint | Documento |
|--------|-----------|
| 02 Player | `PLAYER_CONTROLLER_BASELINE.md` |
| 03 HUD | `HUD_DEBUG_BASELINE.md` |
| 04 Interação | `INTERACTION_SYSTEM_BASELINE.md` |
| 05–06 Térreo | `PENSION_GROUND_FLOOR_BLOCKOUT_BASELINE.md` |
| 07 Puzzle | `DEPOSIT_PUZZLE_BASELINE.md` |
| 08–09A Escada | `STAIR_RAMP_BASELINE.md` |
| 10 Segundo andar | `PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md` |
| 11 Playtest 2º andar | `PENSAO_SECOND_FLOOR_FINE_PLAYTEST.md` |
| 12 Teto blockout | `PENSION_CEILING_BLOCKOUT_BASELINE.md` |
| 12A Hotfix fechamento | `PENSION_CEILING_HOTFIX_12A.md` |
| 13 Atmosfera base | `PENSION_ATMOSPHERE_BASELINE.md` |
| 14 Leitura narrativa | `PENSION_ROOM_READABILITY_BASELINE.md` |
| 14A Portas blockout | `PENSION_DOOR_BLOCKOUT_BASELINE.md` |

---

## Sprint 14 — resumo (em validação)

Blockout narrativo — portas/molduras, props simples e interações de texto.

- **Portas/molduras** em entrada, recepção, quartos, cozinha, depósito, escada, 2º andar.
- **Props:** balcão, chaves, cama, mala, fogão, prateleiras, caixas, anotação, marcas de arrasto.
- **Interações** narrativas em recepção, quartos, cozinha, depósito, escada — puzzle **preservado**.
- **Atmosfera S13, geometria S05–12A, player/HUD/interação** — não alterados.

**Baseline:** `docs/technical/PENSION_ROOM_READABILITY_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

---

## Sprint 14B — resumo (executada)

Substituição do sistema de portas quebrado (14/14A) por padrão blockout estável.

- **Removidas** folhas `Door_*_Leaf`, portas interactable verdes, duplicatas de moldura.
- **Porta aberta** = somente moldura (entrada, 102, cozinha, 201, 202).
- **Porta trancada** = painel opaco + colisão WorldLayer + Area3D local.
- **Depósito** = painel some + colisão desativa (sem animação); interação em `Door_Deposit_InteractArea`.
- **Varanda** = `Door_UpperBalcony_Locked` + placeholder interior + leitura na trilha.

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.1  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14B

---

## Sprint 14C — sistema de portas reconstruído

Estado técnico: implementado e compilado sem erros. Há três prefabs estáveis; as passagens abertas usam somente moldura; o depósito preserva chave e mensagens; e a porta verde possui um único painel opaco. Player, HUD, atmosfera, fog, fachada e escada não foram alterados.

Pendente: executar F6 e aprovar visualmente a rota e as interações.

Revisão visual posterior: corrigidos sobreposição da travessa do depósito, placa com texto fora da madeira, vão lateral da caixa da escada e prompts de porta disparados por paredes/piso distantes.

Sprint 14D: auditoria formal criada em `docs/testing/PENSION_DOOR_AUDIT_14D.md`; entrada principal sem folha ou bloqueio; placa deslocada para fora do eixo da trilha; painéis fechados afastados do plano da parede; porta verde renomeada para o nó oficial.

Hotfix 14D: `UpperBalcony_BackWall` estava usando altura local de primeiro andar e aparecia como um bloco diante da passagem do térreo. Reposicionado para `Y = 4,25`, no segundo andar.

## Sprint 14F — resumo (executada)

Limpeza definitiva — remover duplicatas em vez de empilhar correções.

- **Placa única** `Sign_Pensao_Main_Exterior` fora da fachada; removida placa interna que clipava a entrada.
- **Portas abertas** = moldura mínima inline (3 peças); sem infill, folhas ou painéis.
- **Painéis permitidos** apenas: `Door_Deposit_Panel`, `Door_UpperBalcony_Panel`.
- **JobOfferSign** na trilha = interação sem mesh (sem placa 3D duplicada).
- Corredor inútil do 2º andar **permanece fechado** (14E).

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.3  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14F

## Sprint 14Z — reset final (executada)

Limpeza destrutiva — remover meshes instáveis em vez de ajustar.

- **Placas removidas** temporariamente (entrada limpa).
- **Portas abertas** = vão limpo + `Header_*` (sem moldura).
- **Blockers inline** apenas: `Door_Deposit_Blocker`, `Door_UpperBalcony_Blocker`, `Door_UpperBlocked_Blocker`.
- Prefabs/molduras de porta **não** instanciados em runtime.
- Corredor inútil do 2º andar **permanece fechado** (14E).

**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v2.0  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md` — seção Sprint 14Z

## Sprint 14 — APROVADA (blockout narrativo limpo)

**Data de aprovação:** 2026-07-11  
**Cena oficial:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

**Aprovado:**
- Pensão navegável (térreo + 2º andar + escada)
- Atmosfera preservada
- Puzzle chave → depósito → fusível preservado
- Portas decorativas bugadas removidas/simplificadas (vãos limpos + headers)
- Placas problemáticas removidas (arte de placa fica para sprint futura)
- Cômodos com leitura básica por vãos/headers

**Não avançar nesta sprint:** portas bonitas, molduras decorativas, placas finais, layout, atmosfera, player, HUD, puzzle.

**Baseline de portas:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v2.0  
**Playtest:** `docs/testing/PENSION_NARRATIVE_READABILITY_PLAYTEST.md`

## Sprint 15 — APROVADA (eventos narrativos simples)

**Data de aprovação:** 2026-07-11  
**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

- Eventos one-shot funcionando (entrada, pós-chave, pós-fusível, escada, corredor, porta)
- Mensagens narrativas + flicker de luz; sem loop
- Puzzle, atmosfera, HUD, player, escada e 2º andar preservados
- **Sem inimigo / combate / chase**

**Baseline:** `docs/technical/PENSION_NARRATIVE_EVENTS_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_SIMPLE_NARRATIVE_EVENTS_PLAYTEST.md`

## Sprint 16 — Áudio funcional base da Pensão (aprovada)

**Status:** ✅ **Aprovada** (2026-07-12)  
**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`

**Aprovado:**
- Áudio ambiente + lâmpada
- Eventos narrativos com áudio
- Gotas / ambiência úmida
- Passos do player (madeira aprovada)
- Respiração normal e ofegante
- Sistema mais realista e imersivo
- Player movement, HUD, lanterna, atmosfera/fog, puzzle chave → depósito → fusível preservados

**Ressalva (backlog — não corrigir agora):**
- Passos na terra/cascalho andando e correndo ainda precisam de refinamento futuro

**Baseline:** `docs/technical/PENSION_AUDIO_BASELINE.md`  
**Playtest:** `docs/testing/PENSION_AUDIO_FUNCTIONAL_PLAYTEST.md`  
**Playtest ambience:** `docs/testing/PENSION_AMBIENCE_AUDIO_PLAYTEST.md`  
**Assets:** `docs/audio/PENSION_AUDIO_ASSET_LIST.md`

## Sprint 16B — Passos + gotas + debug (aprovada)

**Status:** ✅ Aprovada  
- `PlayerFootstepAudio` (audio-only; **não** altera `PlayerController`)
- Gotas + F7 debug

### Sprint 17D — hotfix de navegação da ala da varanda

**Status:** 🔄 Implementada e validada por compilação/carga — F6 final do usuário pendente
- alinhado o vão do guarda-corpo ao acesso lateral real;
- adicionado piso sólido de conexão varanda → ala;
- liberadas as entradas do banheiro e do quarto da proprietária;
- removido o prompt inacessível do stub do quarto 203;
- trigger “Olhar para baixo” limitado à borda externa;
- forro da recepção revisado com placa opaca de acabamento;
- PlayerController, câmera, HUD, lanterna, áudio, passos, respiração, fog, escada e puzzle depósito/fusível não foram alterados.

Sprint 17C/17D foi preservada no checkpoint `4a16478` e substituída pela Sprint 17E.

### Sprint 17E — rebuild cirúrgico da ala da varanda

**Status:** 🔄 Rebuild implementado e cena inicializada — percurso F6 manual pendente
- microárea antiga substituída integralmente por porta verde → patamar → varanda → dois cômodos;
- `BalconyDoor_Green` é a única porta/painel de acesso;
- `BalconyLanding` e `BalconyWalkable` formam piso contínuo e nivelado;
- banheiro e quarto da proprietária têm entradas diretas, sem props na circulação;
- interactions antigas não são instanciadas; áreas novas ficam diante dos elementos;
- `Interact_BalconyLookDown` foi recriado na borda externa;
- teto da recepção permanece fechado pelo acabamento opaco dedicado;
- progressão, PlayerController, HUD, áudio, escada, fog e puzzle depósito/fusível preservados.

**Correção pós-playtest:** removidos o placeholder legado que criava a pilastra no acesso e os pisos/forros coplanares que causavam flicker. O teto da recepção agora é uma peça contínua. O arame aponta para o ralo do banheiro, onde retira a chave do quarto.

**Arquitetura final 17E:** `BalconyWing.tscn` é a única fonte da microárea. `BalconyWingPuzzleSetup` foi removido da cena principal e permanece desativado por padrão; o builder geral não cria mais porta/varanda/cômodos. `BalconyPuzzleSetup` ficou restrito à progressão da nota/chave e à inicialização da porta manual.

### Sprint 17F — Quarto 203 encontrável

**Status:** implementado; descoberta visual final depende do F6 do usuário.

- porta 203 estática posicionada na parede esquerda do corredor superior;
- prompt específico e área curta voltada para o corredor;
- antes do caderno informa bloqueio simples;
- depois do caderno toca arranhão, informa bloqueio interno e encerra o fluxo com tensão;
- porta permanece fechada para sprint futura.

**Próxima sprint recomendada:** Sprint 19 — Quarto 203 e primeira manifestação/inimigo, sem assumir combate/chase automaticamente.

### Sprint 18 — expansão da ala superior e primeiro aviso

**Status:** implementada; percurso visual F6 pendente.

- número 203 reorientado;
- varanda/ala recebeu cobertura simples para reduzir leitura de laje vazia;
- rouparia e Quarto 204 bloqueado adicionados ao corredor superior;
- primeiro aviso 203: arranhão, passos, flicker curto e mensagem final;
- flag impede repetição completa do susto;
- fluxo aprovado da varanda e puzzles anteriores preservado.

### Hotfix — remoção definitiva da barreira superior

`BalconyRail_Front`, seu collider e o trigger “Olhar para baixo” foram removidos da cena manual e do builder histórico. Um piso simples e nivelado conecta o antigo limite ao espaço aberto, marcado por `Marker_UpperWing_PathStart` e `Marker_UpperWing_PathEnd`. Compilação e carga passaram; travessia manual F6 ainda precisa confirmar o caminho binário antes do commit final.

### Sprint 18A — Laje sólida + cômodos jogáveis

**Status:** 🔄 Implementada mas **expansão pausada** até 18B validada no F6

- `UpperWing_SolidFloor` + connector + reforço na `BalconyWing` (colisão layer 1, espessura 0.30)
- Faixa `UpperBalcony_FrontWalkway` com guarda-corpo só na borda externa
- Cômodos: 204, banheiro coletivo, rouparia, sala do gerador, 205 bloqueado
- Puzzle: `HasUpperFuse` → `IsUpperPowerRestored` → reação do Quarto 203 (sem abrir)
- Docs: `docs/design/PENSION_UPPER_WING_EXPANSION.md`, playtest 18A

**Não aprovada** até F6 confirmar que o player não cai.

### Sprint 18B — Saneamento estrutural + anti-acúmulo

**Status:** reforçado / sucedido pela **18C**

### Sprint 18C — Saneamento obrigatório + regra anti-lixo

**Status:** 🔄 Implementada — F6 pendente

- `UpperWingExpansion` reduzida a **laje + walkway + rail** (cômodos tortos removidos);
- forro GF reforçado + markers de altura completos;
- `LevelSanityChecker` no **F9** com contagem ERROR/WARNING;
- builders da ala continuam congelados;
- checklist F9 obrigatório no PROJECT_STATE.

**Não criar** novos cômodos até F9 = 0 ERROR e F6 limpo.

## Sprint 16C — Ajuste fino de passos (aprovada)

**Status:** ✅ Aprovada — corrida = mesmo banco da superfície; `player_run_step_*` reservado.

## Sprint 16D — Cadência definitiva (aprovada)

**Status:** ✅ Aprovada (terra/cascalho em backlog)  
- Walk 0,64 s / Run 0,36 s / Crouch 0,85 s; `MinimumStepCooldown` 0,28 s

## Sprint 16E — Respiração do player (aprovada)

**Status:** ✅ Aprovada  
- `PlayerBreathingAudio` — normal + panting; áudio-only

## Backlog técnico/artístico (áudio)

- Refinar passos na terra/cascalho **andando**
- Refinar passos na terra/cascalho **correndo**
- Avaliar novos samples de terra mais naturais
- `player_run_step_*` permanece reservado (chase/pânico futuro)

## Próxima sprint recomendada (após F6 da 17)

**Sprint 18 — Primeiro susto controlado sem inimigo físico**

**Não avançar automaticamente** para inimigo/combate/chase físico.

## Sprint 17 — Puzzle da varanda + ala superior (implementada)

**Status:** 🔄 Implementada — F6 pendente  
**Cena:** `PensaoVerticalBlockout01.tscn`

- Porta verde (`Door_UpperBalcony`) faz parte do puzzle `BalconyAccess`
- Nota 201 → chave na recepção → destravar varanda
- `UpperBalconyWing` navegável: varanda + corredor leste + Room_203 + Room_OwnerOffice
- Eventos sutis (abrir varanda / bilhete 203 / caderno); **sem inimigo**
- Áudio: `AudioZone_UpperBalcony` / `AudioZone_UpperBalconyWing`
- Sistemas 02–16 preservados (movimento, HUD, áudio, fog, depósito/fusível)

**Design:** `docs/design/PENSION_BALCONY_PUZZLE_DESIGN.md`  
**Playtest:** `docs/testing/PENSION_BALCONY_PUZZLE_PLAYTEST.md`  
**Interação:** `docs/technical/PENSION_INTERACTION_BASELINE.md`

### Sprint 17A — Hotfix porta verde (posição)

**Status:** 🔄 Aplicada — validar no F6  
**Causa:** Y do painel/Area somava altura do 2º andar duas vezes → porta/prompt no teto.  
**Fix:** root no piso; painel/Area locais; Area na altura do peito; marker no vão.

### Sprint 17B — Hotfix acesso real da varanda

**Status:** 🔄 Aplicada — validar no F6  
- Removida porta marrom duplicada (`Door_UpperBlocked`)
- Porta verde única no vão
- `Wall_Second_Front` com gap + varanda externa com guarda-corpo
- Corredor curto da ala; trigger de presença afastado da porta  

### Sprint 17C — Ala da varanda + puzzle macabro

**Status:** 🔄 Implementada — F6 pendente  
- `Room_UpperBathroom` + `Room_OwnerBedroom` acessíveis
- Puzzle: arame → ralo → chave → quarto da proprietária
- Caderno → `EventOwnerLedgerReveal` (tensão sem inimigo)
- `Door_Room203_Blocked` como gancho da próxima sprint
- Áudio: zona banheiro com gotas; varanda/wing ajustadas
- Docs: `docs/design/PENSION_BALCONY_WING_PUZZLE.md`

Sprint 17 ainda depende de playtest completo da rota 17–17C.

# Regra obrigatória de geometria

Toda alteração de cenário deve seguir `.cursor/rules/level-geometry-guardrails.mdc`, `AGENTS.md` e `docs/production/LEVEL_GEOMETRY_GOLDEN_RULES.md`.

Nenhuma sprint passa com queda no limbo/direita/esquerda, piso sem colisão, duplicata, builder antigo, invasão entre andares, prompt fantasma ou collider sem função.

Estado do hotfix: a mureta foi removida; pisos parciais concorrentes foram substituídos por um único `UpperWing_SolidFloor` de cobertura integral. A próxima sprint só pode criar cômodos após validação F6 completa do piso.

## Hotfix definitivo da laje superior

O estado anterior falhou no playtest ao andar para a direita. O piso oficial foi ampliado para `X=-2,7..9,0` e `Z=-5,8..3,6`, com mesh/BoxShape idênticos e layer/mask `1/1`; ele encosta sem sobrepor o piso principal do segundo andar. `UpperFloorCollisionProbe` usa F8 e valida Start/Center/Right/FarRight/Left/End. A laje só será aprovada após o teste manual direito/esquerdo/diagonais. `AGENTS.md` e as regras Cursor são obrigatórios.

### Hotfix cirúrgico posterior

`Ceiling_FirstFloor_Seal` monolítico, que atravessava visualmente a escada, foi dividido em quatro placas ao redor do shaft. A laje foi ampliada para `X=-4,7..12,0` e `Z=-7,8..5,6`; o F8 agora diagnostica frente e chão. Nenhuma nova sala será criada até escada e laje passarem no teste manual de extrema direita.
