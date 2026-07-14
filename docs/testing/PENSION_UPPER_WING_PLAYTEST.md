# Playtest — Saneamento 18C / ala superior

## Checkpoint 2026-07-14 — transição escada e chegada superior

Automático neste checkpoint: [x] build C# 0 erro / 0 aviso [x] materiais dedicados registrados no sanity checker [x] degraus/corrimãos/deck excluídos do passe visual

Ao abrir em outro computador: [ ] importação Godot concluída [ ] F9 0 ERROR / 0 WARNING

Poço da escada: [ ] parede oeste texturizada [ ] fechamentos do poço texturizados [ ] piso visual envelhecido [ ] teto de transição infiltrado [ ] sem material aplicado aos degraus [ ] corrimãos intactos [ ] descida continua natural [ ] iluminação legível

Chegada superior: [ ] paredes leste/oeste e fundo com reboco frio/desbotado [ ] piso do patamar com madeira envelhecida [ ] teto acinzentado/infiltrado [ ] diferença visual clara em relação ao térreo [ ] sem repetição ou emenda evidente [ ] móveis continuam apoiados [ ] corredor e portas livres

Regressão: [ ] player sobe e desce sem prender [ ] sem queda no limbo [ ] varanda/deck intactos [ ] retorno ao térreo sem teleporte [ ] prompts, painel, ralo, fusíveis e Quarto 203 funcionais [ ] IA, perseguições e esconderijos funcionais [ ] sem crash

Este commit é um checkpoint de continuidade. Não marcar o art pass como aprovado enquanto os itens manuais acima estiverem pendentes.

## Sprint 31C-1 — materiais mestres PBR

Automático: [x] recursos importados [x] build 0/0 [x] cena oficial [x] F9 0/0 [x] deck 49/49 [x] 38 paredes pareadas [x] zero física/gameplay/luz própria na árvore 31C [x] 104 paredes/6 pisos/16 tetos [x] triplanar global [x] 0 decals-caixa ativos [x] 4 overlays rígidos antigos ocultos

Materiais: [ ] reboco seco velho e não uniforme [ ] mofo/umidade forte em cozinha, banheiro e rodapés [ ] madeira velha com tábuas/frestas visíveis [ ] teto infiltrado visível [ ] varanda visual fria sem mudança no deck físico [ ] resposta da lanterna revela normal/roughness [ ] ambiente escuro mas legível

Prioridade do vídeo: [ ] nenhuma linha horizontal dividindo a mesma parede [ ] nenhuma placa/retângulo opaco [ ] nenhuma travessa branca no piso/teto [ ] nenhuma sujeira cobrindo porta/prompt [ ] piso sem mudança abrupta entre peças vizinhas

Gameplay: [ ] portas e prompts [ ] banheiro/ralo/arame/chave [ ] painel e fusíveis [ ] Quarto 203 e bilhete [ ] primeira perseguição [ ] IA [ ] segunda perseguição [ ] safe zones/guarda-roupas [ ] retorno ao térreo sem teleporte [ ] sem player preso [ ] sem crash

Não criar o commit final da Sprint 31C antes da aprovação manual completa.

## Sprint 31B — degradação ambiental pesada

Automático: [x] build 0/0 [x] cena oficial [x] F9 0/0 [x] deck 49/49 [x] paredes superiores pareadas [x] zero física/gameplay na árvore 31B [x] quatro luzes locais contidas

Visual e circulação: [ ] mofo/reboco/fuligem/escorridos sem z-fighting [ ] marcas e sujeira de borda apoiadas [ ] rachaduras/halos no teto sem flutuar [ ] pátina dos móveis preserva texturas [ ] vidro e cortinas alinhados [ ] iluminação escura mas navegável [ ] nenhum prop atravessando parede/móvel [ ] recepção livre [ ] corredor térreo livre [ ] salão superior livre [ ] corredor superior livre [ ] escada e varanda intactas

Gameplay: [ ] portas e prompts livres [ ] ralo/arame/chave funcional [ ] painel e fusíveis funcionais [ ] Quarto 203 e bilhete funcionais [ ] primeira perseguição [ ] IA [ ] segunda perseguição [ ] safe zones/guarda-roupas [ ] retorno ao térreo sem teleporte [ ] sem player preso [ ] sem crash

Não criar o commit final da Sprint 31B antes da aprovação manual completa.

## Sprint 31 — passe de materiais

Validação automática: [x] build 0/0 [x] cena oficial [x] F9 0 ERROR / 0 WARNING [x] deck 49/49 [x] 38 paredes pareadas [x] zero colisão/gameplay novo [x] deck físico congelado intacto

Playtest manual obrigatório: [ ] recepção/corredor legíveis [ ] quartos navegáveis [ ] cozinha sem bloqueio [ ] banheiro/ralo funcional [ ] lavanderia e painel funcional [ ] tetos sem manchas flutuando [ ] rodapés sem cobrir portas [ ] Quarto 203 completo [ ] primeira perseguição [ ] IA [ ] segunda perseguição [ ] safe zones/guarda-roupas [ ] retorno ao térreo sem teleporte [ ] sem player preso [ ] sem crash

## Sprint 30B — props Blender principais

- [ ] Recepção: balcão e cadeira aparecem uma vez, apoiados no piso, sem bloquear entrada, corredor, balcão interativo ou esconderijo.
- [ ] Cozinha: mesa, fogão, pia e balde aparecem com escala/material corretos; entrada e rota central permanecem livres.
- [ ] Prompt `Examinar cozinha` e interação do fogão continuam aparecendo perto e não através de parede.
- [ ] Banheiro: ralo, espelho e balde aparecem sem duplicata; entrada e centro do cômodo permanecem livres.
- [ ] Ralo continua visível e interativo; arame retira a chave normalmente.
- [ ] Sala técnica: painel Blender está alinhado à parede; prompt, chave do ralo e dois fusíveis continuam funcionando.
- [ ] Quarto 201: mala, janela e cortina não atravessam cama, parede, teto, porta ou prompt.
- [ ] Quarto 202: janela nova substitui a falsa antiga sem z-fighting ou abertura real.
- [ ] Criado-mudo do 204 fica apoiado e fora da passagem.
- [ ] Quarto 203, porta, página, trigger e evento permanecem idênticos e funcionais.
- [ ] Visible Collision Shapes: zero collider novo na 30B; colliders antigos de balcão/cadeira/mesa/fogão continuam simples e alinhados.
- [ ] Player não fica preso em nenhum prop, quarto, cozinha, recepção ou banheiro.
- [ ] IA, primeira e segunda perseguições, safe zone, guarda-roupas e eventos ambientais continuam funcionando.
- [ ] Escada, porta verde, varanda/deck, circulação nos dois andares e retorno ao térreo continuam sem queda ou teleporte.
- [ ] Sem crash, mesh gigante/minúscula, material ausente, duplicata ou placeholder por cima.

Automático: [x] build 0 erro/0 aviso [x] cena oficial headless [x] F9 0 ERROR/0 WARNING [x] deck 49/49 [x] 38 paredes superiores pareadas [x] 15 instâncias Blender [x] zero física/gameplay novo [x] painel/ralo funcionais preservados.

Não criar o commit final da Sprint 30B antes da aprovação manual completa.

## Sprint 30A — cama Blender piloto no Quarto 201

- [ ] Cama nova aparece uma única vez no Quarto 201, com escala/material corretos e apoiada no piso.
- [ ] Não atravessa parede, teto, porta, janela ou prompt e não há placeholder duplicado.
- [ ] Player entra, circula e sai do Quarto 201 sem prender.
- [ ] Visible Collision Shapes: cama sem collider e nenhuma colisão estrutural nova.
- [ ] Piso, escada, porta verde, varanda/deck e Quarto 203 permanecem intactos.
- [ ] Puzzle de energia, IA, perseguições, safe zone e eventos ambientais continuam funcionando.
- [ ] Retorno ao térreo sem teleporte entre andares e sem crash.

Não criar o commit final da Sprint 30A antes da aprovação manual completa.

## Sprint 28 — Art pass leve dos cômodos

- [ ] Recepção: caixa registradora, papéis, pano e detalhe de parede aparecem sem bloquear balcão, chaveiro ou esconderijo.
- [ ] Entrada: banco, bagagens, guarda-chuvas, quadros e passadeira ocupam o vazio sem estreitar a rota principal.
- [ ] Cozinha: panelas, balde, prateleira e pano aparecem sem cobrir os prompts do fogão/armário.
- [ ] Banheiro: ralo está mais legível e continua interagível; banheira, vaso, prateleira, espelho, balde, pano e cano deixam o centro livre.
- [ ] Sala técnica: painel está na parede leste, prompt e dois estágios de fusível funcionam; porta 205 fica livre e legível.
- [ ] Quarto 201: colchão, mala, papel, quadro e cortina não bloqueiam entrada nem interação.
- [ ] Quarto 202: caixa, objeto coberto e quadro preservam cadeira, armário e rota livre.
- [ ] Quarto 203: papéis, símbolo e cortina não cobrem a página, a cama, a porta ou o evento.
- [ ] Quarto 204: cama, armário, cadeira, baú e tapete ocupam a metade leste sem bloquear a entrada nem a travessia.
- [ ] Escritório: arquivo estreito permanece encostado na parede oeste; tapete e livros não cobrem o registro interagível nem se sobrepõem à mobília autorada.
- [ ] Corredor térreo: não existem mais quadro/travessa de perfil nem prateleira atravessando a passagem.
- [ ] Salão superior: tapete, sofá, relógio, oratório, retrato, castiçais e lustre estão apoiados em piso/parede/teto, sem móvel sobre o vão.
- [ ] Salão superior: caminhar da saída da escada até o corredor pela diagonal sem atravessar ou encostar em móvel visual.
- [ ] Corredor superior: passadeira e luminárias visuais ocupam o vazio sem estreitar a rota da IA/perseguição.
- [ ] Varanda/ala superior: somente marcas e pano visual; deck, bordas e circulação permanecem exatamente iguais.
- [ ] Lanterna revela os materiais sem z-fighting, flicker visual ou objetos flutuando.
- [ ] Puzzle completo do ralo/painel/fusíveis/203 continua funcionando.
- [ ] Primeira e segunda perseguições, IA, safe zone e guarda-roupas continuam funcionando.
- [ ] F6: caminhar/correr/agachar por todos os setores; frente, trás, laterais, diagonais e ida/volta.
- [ ] Voltar ao térreo após visitar a ala superior não causa teleporte ou trigger entre andares.

Validação automática após os prints: [x] cena oficial headless [x] F9 0 ERROR/0 WARNING [x] deck 49/49 [x] 38 paredes superiores pareadas [x] árvore Sprint 28 com 213 meshes e sem física/navegação/gameplay [x] ofensores visuais antigos ausentes [x] composição nova dentro do footprint do `UpperLanding_Main` [x] painel na parede leste com 11,00 m de separação da porta 205.

Não criar o commit final da Sprint 28 até todos os itens manuais acima serem aprovados.

## Sprint 27A — Expansão externa e ajuste fino de janelas falsas

- 14 janelas externas permanecem no mesmo container visual da Sprint 27; nenhuma cena oficial paralela foi criada.
- Fachada frontal térrea: as duas janelas baixas laterais foram removidas porque os barrancos as cortavam.
- Térreo: Quarto 102 na face interna da parede oeste (com tábuas), cozinha na face interna da parede leste (com cortina) e ponta do corredor (janela alta).
- Fachada frontal superior: duas janelas nos shells existentes, preservando a varanda visual central.
- Segundo andar original: Quarto 201 na face interna oeste e Quarto 202 na face interna leste (com tábuas).
- Grande parede oeste superior: duas janelas alinhadas e distribuídas ao longo da parede.
- Ala superior nova: Quarto 204, sala técnica alta/tampada, rouparia, banheiro alto e escritório da proprietária.
- Faces internas das paredes externas: Quarto 102, cozinha, Quarto 201 e Quarto 202 receberam uma janela centralizada cada; a parede oeste superior recebeu duas janelas distribuídas horizontalmente.
- Corredor térreo: janela, fresta no piso e luz local associada removidas, sem resíduo visual.
- Total do art pass 27/27A: 4 janelas internas decorativas + 14 externas de fachada.
- Todas as janelas externas usam moldura, vidro escuro e variação discreta de cortina/tábua/altura; nenhuma abre vão real.
- Auditoria runtime confirma apoio em parede sólida; as duas janelas frontais superiores são validadas contra `Shell_FacadeUpper_FrontLeft/Right`.
- Nenhum `StaticBody3D`, `CollisionShape3D`, `Area3D`, navegação, trigger, prompt ou interação foi criado na 27A; a luz órfã do corredor removido também foi apagada.
- Automático após ajuste fino: build 0 erros/0 avisos; cena oficial headless; F9 0 ERROR/0 WARNING; deck 49/49.
- Manual pendente: conferir alinhamento, ausência de clipping/flutuação e leitura interna/externa nos seis pontos marcados.

## Sprint 27 — Janelas falsas, frestas e leitura visual

- Container exclusivo: `World/VisualPolish/Sprint27_FakeWindowsLighting`.
- Janelas internas remanescentes: lateral oeste da recepção, parede alta do fundo da escada, lateral leste do corredor superior e parede interna do Quarto 203.
- A janela do 203 recebeu duas tábuas visuais; a recepção recebeu cortina parcial; todas continuam sobre paredes sólidas existentes.
- Frestas no piso: recepção, corredor superior e 203, com offset vertical mínimo para evitar z-fighting.
- Luzes locais: quatro `SpotLight3D` azul-acinzentadas, energia `0,08–0,16`, alcance `2,4–3,8 m`, sem sombras dinâmicas.
- Detalhes: três manchas de umidade, dois panos pendurados e duas sombras discretas de grade.
- Não foram criados `StaticBody3D`, `CollisionShape3D`, `Area3D`, navegação, interação ou física no art pass.
- Nenhuma parede foi cortada/removida e nenhuma colisão estrutural, piso, teto, porta, escada, corrimão ou deck foi alterado.
- Automático após 27A: build 0 erros/0 avisos; cena oficial headless; F9 0 ERROR/0 WARNING; deck 49/49; verificação confirma 18 janelas totais e zero nós físicos.
- Manual pendente: recepção, corredor, escada, ala superior, 203, banheiro, rouparia, sala técnica e varanda; conferir luz, z-fighting, lanterna e regressão completa.

## Sprint 26 — Eventos dinâmicos de terror ambiental

- `AmbientHorrorDirector` criado sob `World/Gameplay/AmbientHorror`.
- Eventos: porta rangendo, batidas superiores, passos distantes, flicker local, arranhão, objeto caindo, respiração atrás, sussurro do ralo e sombra rápida rara.
- Condições: progressão pós-203/primeira presença, energia superior, chave do ralo e perseguições concluídas, conforme cada evento.
- Sons existentes reutilizados: `old_house_settle_*`, `distant_knock_*`, `distant_step_*`, `door_scratch_02`, `player_breath_heavy_*` e `water_drop_03`.
- Triggers pequenos: recepção, pé da escada, corredor superior, fundo do térreo, banheiro e saída do 203.
- Cooldown global: 25–45 segundos; somente um evento por vez; raros são one-shot.
- Chase, Search, esconderijo e safe zone bloqueiam novos eventos.
- Não houve alteração de geometria, deck, escada, portas, PlayerController, puzzle ou core da IA.
- Automático: build 0 erros/0 avisos; cena oficial headless; F9 0 ERROR/0 WARNING; deck 49/49.
- Manual: aprovado pelo usuário em 2026-07-13 conforme `docs/testing/PENSION_AMBIENT_HORROR_PLAYTEST.md`.

## Sprint 19E — Rebuild limpo da ala superior

- a ala anterior foi removida como ownership concorrente; `BalconyWing.tscn` permanece somente com a porta verde;
- container oficial reconstruído/consolidado: `World/Level/SecondFloor/UpperWingRooms`;
- layout: `Corridor_Main`, `Room204_Bedroom`, `SharedBathroom`, `LaundryStorage`, `TechnicalRoom`, `OwnersOffice`, `Room205_Locked`, `Doors`, `Props`, `Interactions`, `Triggers`;
- removidos: banheiro/quarto do proprietário antigos, paredes/colliders/tetos antigos, ralo/espelho/ledger e porta antiga duplicados;
- análise do vídeo/áudio de 19:25 confirmou portas desenhadas diante de paredes sólidas completas;
- Escritório: `Wall_Bath_North` foi substituída por segmento que deixa vão real para `Door_OwnersOffice`;
- 205: `Wall_Tech_North` foi dividida em segmentos esquerdo/direito, deixando vão real para `Door_Room205_Locked`;
- 31 segmentos de parede modulares autorados; 31 colliders filhos correspondentes;
- seis portas reais: 204, banheiro, rouparia, sala técnica, escritório e 205 trancado;
- tetos visuais: corredor, 204, banheiro, rouparia, sala técnica, escritório e 205;
- ownership: arame/fusível em `LaundryStorage`; ralo/espelho em `SharedBathroom`; painel em `TechnicalRoom`;
- painel montado na parede interna norte, com InteractionArea pequena à frente;
- F8 `WallCollisionProbe`: implementado; execução em pontos manuais pendente;
- F9 `UpperWingWallAudit`: `0 ERROR`, todos os 31 segmentos aprovados;
- `LevelSanityChecker`: `0 ERROR / 0 WARNING`;
- duplicados/Old/Temp/Debug/Legacy: nenhum na ala viva;
- `UpperWing_CollisionDeck`: não alterado; grid preservado em `49/49`;
- compilação: 0 erros e 0 avisos.

Playtest manual pendente: [ ] todas as paredes [ ] seis portas [ ] arame [ ] ralo [ ] fusível/painel [ ] 203 [ ] retorno ao térreo [ ] zero limbo/teleporte

Não criar commit final antes da aprovação manual.

## Sprint 19D — Hotfix estrutural final da ala superior

- causa estrutural: os cômodos antigos de `BalconyWing.tscn` ainda coexistiam com a ala 19C em `UpperWingRooms.tscn`, produzindo paredes, tetos e interações concorrentes;
- `BalconyWing.tscn` foi reduzido ao seu ownership válido: somente porta verde;
- banheiro/quarto antigo, colliders, espelho, ralo, ledger e porta antiga duplicados foram removidos;
- lavanderia/arame e banheiro/ralo permanecem exclusivamente em `UpperWingRooms.tscn`, ambos com paredes e teto;
- sala técnica mantém entrada real em `Door_TechnicalRoom`;
- painel foi restaurado como `TechnicalRoom/TechnicalPanel`, montado na parede norte em `(3,20; 3,95; 5,38)`;
- `InteractionArea` é local, pequena e fica somente à frente do painel;
- prompts preservados: `Examinar painel` e `Inserir fusível`;
- F9 runtime: 30 paredes com mesh/shape correspondente e bloqueio físico confirmado;
- F9 final: `0 ERROR / 0 WARNING`;
- deck congelado: não alterado; grid continua `49/49`;
- boundaries globais: ausentes;
- fechamento mínimo confirmado: `Ceiling_Laundry`, `Ceiling_Bath` e `Ceiling_Tech` presentes.

Playtest manual pendente: [ ] arame [ ] ralo [ ] painel/fusível [ ] tentativa de atravessar paredes [ ] zero limbo [ ] retorno ao térreo sem teleporte [ ] porta verde/203

Não aprovar nem criar commit final antes do percurso manual.

## Sprint 19C — Correção estrutural da ala superior

**Deck:** `UpperWing_CollisionDeck` **não alterado**.

### Problema do arame
Estava no banheiro/alcova estreita da `BalconyWing` (Z ~−5,7). A parede leste era só visual → player atravessava e saía para limbo/área escura.

### Solução do arame (Opção A)
- `Interact_BalconyWireHook` removido da BalconyWing;
- `Interact_LaundryWire` na Rouparia (`UpperWingRooms`), sobre a prateleira;
- prompt: Pegar arame torto;
- mensagem orienta uso no ralo do banheiro da varanda.

### Problema da sala técnica
Painel no fundo leste (X ~13,7); sensação de atravessar parede / sala incoerente; fresta 0,5 m entre Wall_204_North e Wall_Tech_South.

### Solução do painel
- painel + InteractionArea em ~(3,2 / 4,85) — logo após a porta da TechnicalRoom;
- Wall_204_North removida; Wall_Tech_South como divisor único;
- props da técnica aproximados da entrada.

### Paredes / limbo
- BalconyWing: East/North/Divider/South com collider **filho** da mesh;
- Collisions soltos da BalconyWing removidos;
- Room204: `Wall_204_South_Mid` fecha o vão sul.

### Regressão
[ ] porta verde / varanda / térreo sem teleporte / 203 OK  
[ ] não atravessar paredes da BalconyWing nem da ala  
[ ] arame na rouparia sem limbo  
[ ] painel acessível pela porta da técnica  
[ ] Visible Collision Shapes: deck intacto; colliders filhos  

## Sprint 19B — Ala superior completa

**Cena:** `UpperWingRooms.tscn` → `World/Level/SecondFloor/UpperWingRooms`  
**Deck:** `UpperWing_CollisionDeck` **não alterado** (`5, 2.4, 4.6` / `50×0,8×30,8`).  
**Sem** boundary global / mureta / collider solto / piso físico novo.

### Substituição da Sprint 19
A tentativa anterior criou só um bloco/quadradinho isolado no canto. Foi removida e substituída por uma ala completa ligada ao corredor, ocupando a maior parte da área vazia da laje (MasterSlab ~X −7,7…16 / Z −10,8…8,6; construção principal a partir de ~Z −2,2 para não bloquear rota verde→203).

### Layout final
- `Corridor_MainUpper` (~1,6 m) — intro: "O ar aqui em cima parece mais pesado."
- Esquerda: `LaundryStorage`, `SharedBathroom`
- Direita: `Room204_Bedroom`, `TechnicalRoom`
- Fundo: `OwnersOffice`, `Room205_Locked`
- Props + portas + InteractionAreas pequenas + Triggers (intro / exit scare)

### Puzzle / flags
`ReadRoom204Note` → `HasUpperFuse` → `IsUpperPowerRestored` → Room203: "Há algo arrastando…" (com bilhete; não abre).  
Também: `ReadOwnersOfficeLog`, `BathroomScarePlayed`, `LaundryScarePlayed`, `CorridorIntroPlayed`, `Room204ExitScarePlayed`.

### Sustos
Corredor (estalido + flicker), banheiro (gota + arranhão + flicker), rouparia ao pegar fusível, saída do 204.

### Debug
**F4** — concede chaves/itens de puzzle e destranca depósito / varanda / quarto da dona (Fusível Superior já no inventário; energia superior ainda precisa inserir no painel).

### Regressão
[ ] entrar pensão / escada / porta verde / varanda sem cair  
[ ] sem teleporte térreo → segundo  
[ ] correr no térreo OK  
[ ] Quarto 203 + porta verde OK  

### Cômodos / puzzle
[ ] corredor + Room204 (bilhete) + banheiro + rouparia (fusível) + técnica (inserir)  
[ ] OwnersOffice (registros) + Room205 trancado  
[ ] UpperPowerOn / mensagem 203 alterada  
[ ] ≥4 cômodos acessíveis; sem player preso; prompts perto  
[ ] Visible Collision Shapes: deck intacto; colliders filhos; sem boundary/trigger gigante  

## Sprint 19 — Cômodos claustrofóbicos (substituída)

Tentativa rejeitada no playtest: só um bloco isolado. Ver Sprint 19B.

## ✅ CHECKPOINT — Varanda aprovada para gameplay (CONGELADA)

**Data:** 2026-07-12  
**Resultado:** APROVADA

Confirmado no playtest:
- [x] andar na varanda/laje superior;
- [x] sem queda no limbo;
- [x] sem teleporte do térreo para o segundo andar;
- [x] escada funcional;
- [x] porta verde funcional;
- [x] Quarto 203 acessível;
- [x] varanda aberta e navegável;
- [x] `UpperWing_CollisionDeck` intacto e congelado.

Congelado:
- não mexer no chão/deck da varanda;
- não recriar mureta / boundary / guarda-corpo / colliders soltos;
- não criar paredes invisíveis na área caminhável.

Próximas colisões: apenas paredes de cômodos novos, com collider filho da parede visual.

## Rollback — colliders invisíveis bugados da varanda

- último hotfix criou paredes invisíveis no meio da varanda (`BalconyWallColliders`);
- nodes removidos (não ajustados):
  - `World/Level/SecondFloor/Collisions/BalconyWallColliders` (container inteiro);
  - `BalconyWallCollider_Left`;
  - `BalconyWallCollider_Right`;
  - `BalconyWallCollider_FrontGuard`;
  - shapes `BalconyWallSideShape` / `BalconyWallFrontGuardShape`;
- F8: forward hit esperado nesses StaticBody3D quando o player batia no meio do caminho;
- `UpperWing_CollisionDeck` **não alterado** (`pos (5, 2.4, 4.6)`, `50×0,8×30,8`, layer/mask `1/0`);
- varanda volta ao estado aberto/navegável; sem boundary, mureta ou guarda-corpo;
- F9/LevelSanity passam a tratar `BalconyWallCollider*` como forbidden leftover.

Validação: [ ] circulação livre na varanda [ ] sem parede invisível [ ] deck intacto [ ] porta verde/203 [ ] térreo sem teleporte

## Hotfix final — colisão das paredes da varanda (REVERTIDO)

Tentativa de colliders finos por coordenada (`BalconyWallCollider_*`) bloqueou o caminho principal. Rollback completo; não recriar.

## Hotfix — varanda limpa e isolamento do térreo

### Boundary da varanda removida
- removidos: `BalconyBoundaryColliders`, `BalconyBoundary_Left`, `BalconyBoundary_Right`, `BalconyBoundary_Front` e shapes associados;
- preservados: `UpperWing_CollisionDeck` (chão funcional), `SecondFloor_MasterSlab` (visual), porta verde, Quarto 203, escada;
- sem mureta, rail global, placa escura ou boundary novo na varanda.

### Causa do teleporte térreo → segundo andar
- script: `DebugFallRecovery.cs`;
- bug: após visitar a ala superior (`Y >= 2,65`), qualquer posição com `Y < 1,9` dentro do AABB enorme do deck (`X=-20..30`, `Z=-10,8..20`) era tratada como queda e o player era mandado para `SafeMarker_SecondFloor`;
- isso incluía corredor e recepção do térreo após descer a escada.

### Correção
- `DebugFallRecovery` só ativa com `playerY < KillY` (`-3,0`);
- destino: `SafeMarker_Reception` se o último andar válido for o térreo; `SafeMarker_SecondFloor` só se a última posição válida foi segundo andar/varanda;
- logs: `[DebugFallRecovery] CHECK ...`, `TRIGGERED reason=...`, `ignored: player is on first floor`.

### Isolamento por andar
- volumes lógicos: `FirstFloorVolume`, `SecondFloorVolume`, `UpperWingVolume`;
- F8: `PlayerAreaProbe` — posição, andar estimado, raycasts, Area3D sobrepostas (ERROR se trigger superior pegar térreo);
- F9: `FloorTriggerIsolationChecker` + `LevelSanityChecker` — boundary ausente, deck ativo, triggers sem invadir térreo.

### Resultado esperado do playtest
- F8 no térreo (corredor/recepção): sem Area3D superior;
- F9: sem ERROR de isolamento / boundary;
- correr no primeiro andar após abrir a varanda: sem teleporte;
- varanda livre e navegável sem paredes/limites bugados.

Validação manual: [ ] varanda livre [ ] F8 térreo limpo [ ] F9 OK [ ] 3× rota térreo sem teleporte [ ] Visible Collision Shapes

Sem commit final até esses itens passarem.

## Hotfix final — colisão dos limites da varanda (REVERTIDO)

- tentativa anterior de `BalconyBoundaryColliders` poluiu a varanda com volumes/paredes;
- rollback completo no hotfix de isolamento; não recriar boundary global.

## Hotfix — UpperWing_CollisionDeck

- problema: o player ainda caía em partes da varanda, principalmente à direita, e atravessava o teto da recepção;
- solução: a colisão foi separada da laje visual e concentrada em `World/Level/SecondFloor/Floors/UpperWing_CollisionDeck`;
- `SecondFloor_MasterSlab` permanece somente visual, sem `StaticBody3D` ou `CollisionShape3D`;
- após análise do vídeo (queda na quina direita entre 30,8–32,9 s), a margem foi ampliada drasticamente;
- BoxShape3D final: `50 × 0,80 × 30,8 m`;
- posição global: `(5,00; 2,40; 4,60)`; topo em `Y=2,80`;
- AABB global: `X=-20,00..30,00`, `Y=2,00..2,80`, `Z=-10,80..20,00`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- F8: 9 markers superiores e 2 markers inferiores atingiram `UpperWing_CollisionDeck`;
- F9: grade 7×7 passou `49/49`, sem buracos;
- o grid exclui outros corpos somente para medir a existência contínua do deck; blockers continuam cobertos pelo sanity check/inspeção manual;
- DebugFallRecovery durante rota normal: pendente de teste manual.

Teste manual: [ ] caminhada completa [ ] extrema direita [ ] diagonais [ ] retorno/203/porta verde [ ] pulos na recepção/entrada [ ] recovery não acionou

Sem commit final até o percurso manual passar.

## Hotfix definitivo — SecondFloor_MasterSlab

- problema confirmado no playtest: queda pela direita e travessia do teto da recepção ao pular;
- `SecondFloor_PhysicalSlab`, teto sul antigo e `Ceiling_Reception_Soffit` foram substituídos, não sobrepostos;
- peça única: `World/Level/SecondFloor/Floors/SecondFloor_MasterSlab`;
- MeshInstance3D e BoxShape3D: `23,7 × 0,60 × 19,4 m`;
- centro global `(4,15; 2,50; -1,10)`;
- AABB global `X=-7,70..16,00`, `Y=2,20..2,80`, `Z=-10,80..8,60`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- `DebugFallRecovery` usa `SafeMarker_SecondFloor`; `SafeMarker_Reception` também está disponível;
- acionamento do failsafe na rota normal: pendente de teste manual.

Markers automáticos: [x] Start [x] Center [x] Right [x] FarRight [x] Left [x] Front [x] Back [x] Room203Path [x] GreenDoorPath [x] ReceptionJump [x] EntryJump — os onze atingiram exclusivamente `SecondFloor_MasterSlab` em runtime; os dois testes inferiores atingiram a face em `Y=2,20`.

Testes manuais: [ ] caminhada completa/diagonais/retorno [ ] pulo sob recepção/entrada [ ] failsafe não acionou na rota normal [ ] Visible Collision Shapes

Não aprovado e sem commit final enquanto os itens manuais estiverem pendentes.

## Hotfix crítico — laje física única

- mureta frontal, corpo físico, shape, área e prompt removidos;
- `BalconyRail_Right` residual removido;
- piso oficial: `World/Level/SecondFloor/Floors/SecondFloor_PhysicalSlab`;
- MeshInstance3D/BoxShape3D: `23,7 × 0,60 × 19,4 m`;
- centro `(4,15; 2,50; -1,10)`; AABB `X=-7,70..16,00`, `Y=2,20..2,80`, `Z=-10,80..8,60`;
- layer/mask `1/0`, copiados do piso funcional `PensionGroundFloor_MainFloor` (`AddSolid`);
- corredor e piso principal terminam na borda `Z=-10,80`, sem colisões sobrepostas;
- `DebugFallRecovery` é exclusivo de debug e retorna o player ao `SafeMarker` abaixo de `Y=-3`.

Markers da laje: [x] Start [x] Center [x] Right [x] FarRight [x] Left [x] Back [x] Front [x] Room203Path — todos atingiram exclusivamente `SecondFloor_PhysicalSlab` em runtime.

Markers de teto: [x] Reception_CeilingTest [x] Entrance_CeilingTest — ambos atingiram a face inferior da laje em `Y=2,20`.

Manuais: [ ] caminhada completa e diagonais [ ] pulos sob recepção/entrada [ ] Visible Collision Shapes

Sem aprovação ou commit final enquanto os testes manuais estiverem pendentes.

**Cena:** `PensaoVerticalBlockout01.tscn`

## Sprint 18C — obrigatório

- [ ] **F9** LevelSanityChecker → 0 ERROR
- [ ] entrada olhando para cima → forro limpo
- [ ] recepção olhando para cima → forro limpo
- [ ] sem salas tortas / divisórias da expansão antiga
- [ ] laje superior caminhável Start→End sem cair
- [ ] porta verde OK
- [ ] Quarto 203 OK
- [ ] escada OK
- [ ] F3 reset player OK
- [ ] F10/F11 / HUD / lanterna / áudio OK

## Nota

Cômodos 204 / banheiro coletivo / rouparia / gerador / 205 foram **removidos provisoriamente** da `UpperWingExpansion` (18C) para limpar a rota. Reintroduzir só depois da cena passar F9 + F6 limpos.
# Hotfix — queda na direita da laje superior

## Hotfix — escada e laje superior

- node que atravessava a escada: `Ceiling_FirstFloor_Seal` monolítico;
- causa: placa visual de teto cobria também o retângulo do stair shaft;
- correção: substituída por `Seal_South`, `Seal_North`, `Seal_West` e `Seal_East`, deixando o vazio central limpo;
- piso principal do segundo andar agora termina em `Z=-7,8`, onde começa a laje oficial, sem competição;
- `UpperWing_SolidFloor` final: centro global `(3,65; 2,65; -1,10)`;
- limites: `X=-4,70..12,00`, `Z=-7,80..5,60`, topo `Y=2,80`;
- mesh/shape: `16,7 × 0,30 × 13,4 m`, idênticos;
- layer/mask: `1/1`;
- F8 agora imprime raycast frontal da câmera e raycast inferior do player.

Markers automáticos:

- [x] Start → `(0; 2,80; -7,30)` → `UpperWing_SolidFloor`
- [x] Center → `(3,65; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] Right → `(8,00; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] FarRight → `(11,20; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] Left → `(-4,00; 2,80; -1,10)` → `UpperWing_SolidFloor`
- [x] End → `(3,65; 2,80; 5,00)` → `UpperWing_SolidFloor`

Resultado automático: seis de seis raycasts atingiram exclusivamente o piso oficial.

O teste manual continua obrigatório: escada limpa, extrema direita, diagonais e retorno.

## Hotfix — laje ainda falhava na direita

O playtest confirmou que o piso preliminar de `X=-1,7..6,8` ainda era insuficiente. O F8 `UpperFloorCollisionProbe` foi criado para imprimir posição do player, collider inferior, transform, AABB, mesh, shape, layer e mask, além de testar todos os markers.

- coordenadas globais finais: `X=-2,70..9,00`, `Y topo=2,80`, `Z=-5,80..3,60`;
- centro global: `(3,15; 2,65; -1,10)`;
- MeshInstance3D: `11,7 × 0,30 × 9,4 m`;
- CollisionShape3D: `11,7 × 0,30 × 9,4 m`;
- layer/mask: `1/1`;
- parent `UpperWing_Extension`: identidade, sem rotação/escala;
- `UpperWing_SolidFloor`: sem rotação e escala global `(1,1,1)`.

Raycasts automáticos ao carregar a cena:

- [x] Start → `UpperWing_SolidFloor`
- [x] Center → `UpperWing_SolidFloor`
- [x] Right → `UpperWing_SolidFloor`
- [x] FarRight (`X=8,5`) → `UpperWing_SolidFloor`
- [x] Left → `UpperWing_SolidFloor`
- [x] End → `UpperWing_SolidFloor`

Topo da escada: auditoria estática encontrou somente sólidos gerados por `AddWall/AddSolid`, com mesh e collider pareados. Nenhum collider órfão foi removido sem evidência do probe. O teste manual da passagem continua obrigatório.

O limite norte `Z=-5,8` encosta exatamente no piso principal funcional do segundo andar; a sobreposição anterior com esse slab foi removida.

- [ ] F8 no ponto real antes da antiga queda
- [ ] percurso manual direita/esquerda/diagonais
- [ ] topo da escada sem contato invisível
- [ ] confirmação final: player não cai mais

## Diagnóstico — queda para a direita da laje superior

- ponto aproximado: lateral direita da área aberta, além de `X=1,7`;
- causa: quatro pisos parciais em `BalconyWing.tscn` competiam com um piso central de apenas 3,4 m em `UpperWingExpansion.tscn`;
- a área visual/cômodos alcançava `X=6,8`, mas a colisão central terminava perto de `X=1,7`;
- removidos: `BalconyLanding`, `BalconyWalkable`, `UpperWing_FreeWalkableFloor`, pisos individuais dos cômodos e `UpperBalcony_FrontWalkway`;
- piso oficial: `UpperWing_SolidFloor`;
- cobertura: `X=-1,70..6,80`, `Z=-7,60..2,60`;
- mesh/colisão: `8,5 × 0,30 × 10,2 m`, dimensões idênticas;
- layer/mask: 1/0, igual aos pisos funcionais do nível.

## Teste Start/Center/Right/Left/End

- [x] markers Start, Center, Right, Left e End criados sobre a mesma BoxShape;
- [x] análise estrutural confirma cobertura da lateral direita;
- [x] cena carrega com um único piso oficial;
- [ ] F6 com Visible Collision Shapes;
- [ ] Start → Center → Right → Center → Left → Center → End;
- [ ] End → Right → Left → Start;
- [ ] diagonais e ida/volta sem queda.

O resultado de gameplay permanece pendente até o percurso manual. Não commitar como concluído se houver qualquer queda.
## Sprint 20 — Quarto 203 e evento forte

- Condição de abertura: `IsUpperPowerRestored && HasOwnerRoomKey`.
- Estados: tentar abrir/bloqueado; forçar após o puzzle; aberto permanente com blocker desativado.
- Item: `Room203_LedgerPage`, acessível sobre a cama.
- Evento único: estalo da casa, flicker local, arranhão de porta e passos pesados; mensagem orienta retorno ao corredor.
- Saída: passos distantes, flicker de um segundo no corredor e objetivo para descer e verificar o barulho.
- Sons existentes: `door_scratch_01/02`, `old_house_settle_02`, `distant_step_03/04`.
- Não há inimigo físico, dano, perseguição ou teleporte nesta sprint.
- `UpperWing_CollisionDeck`, corredores, térreo, escada e porta verde não foram alterados.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING

Playtest manual aprovado em 2026-07-13: [x] bloqueio antes do puzzle [x] prompt muda após energia+chave [x] porta abre estável [x] entrada/saída livre [x] página legível [x] evento toca uma vez [x] hint de saída toca uma vez [x] objetivo “Desça para verificar o barulho” [x] sem teleporte/dano [x] regressão completa da ala e térreo
## Sprint 21 — Descida após Quarto 203 e primeira presença

- Início: `Room203EventPlayed && FirstPresenceHintPlayed`, após o jogador sair do 203.
- Trigger: `Trigger_After203_StairDescent`, pequeno, no pé da escada em `(-3,6; 0,8; -30,1)`; não alcança o segundo andar.
- Sequência: madeira estala, batida distante, recepção pisca, passos tocam e `FirstPresence_Shadow` aparece por 1,35 s.
- Sons: `old_house_settle_01`, `distant_knock_02`, `distant_step_02`.
- Mensagem/objetivo: “Alguém passou pelo corredor. Objetivo: Verifique a recepção.”
- Pista: `Downstairs_Clue_After203`, registro rasgado na recepção, revelado após a presença.
- Objetivo final: “O barulho veio do fundo da pensão.”
- Não há inimigo completo, colisão na sombra, dano, perseguição, pathfinding ou teleporte.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING

Playtest manual aprovado em 2026-07-13: [x] não dispara antes do 203 [x] dispara somente no térreo [x] sombra aparece/desaparece [x] evento não repete após subir/descer [x] pista acessível [x] objetivo final “O barulho veio do fundo da pensão” [x] sem dano/teleporte/bloqueio [x] regressão térreo/escada/ala/varanda/203
## Sprint 22 — Primeiro inimigo protótipo e perseguição curta

- Início: somente após `Sprint21Completed`/`DownstairsClueCollected`.
- Revelação: `Trigger_FirstEnemyReveal` no corredor profundo, `(0; 0,8; -22,8)`.
- Inimigo: `Enemy_FirstPresence`, silhueta sem collider, dano ou corpo físico.
- Rota: reveal `Z=-27,7` → corredor `-21` → borda `-14` → parada `-8,5`, velocidade 2,65 m/s.
- Escape: `Trigger_FirstChaseEscape` na borda da recepção, `(0; 0,8; -7)`.
- Mensagens: “Tem alguém ali.” → “Corra.” → “Ele parou… Procure uma forma de se esconder.”
- Sons: `old_house_settle_01`, `distant_step_04`, `distant_knock_02`.
- Não há IA final, NavMesh, combate, dano, morte, teleporte ou colisão bloqueadora.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING

Playtest manual obrigatório: [ ] não inicia antes da Sprint 21 [ ] reveal one-shot [ ] mensagens aparecem [ ] perseguição curta e escapável [ ] inimigo não atravessa paredes [ ] safezone encerra [ ] não reinicia [ ] sem dano/teleporte/bloqueio [ ] regressão completa
## Hotfix 22B — Chave do ralo e dois fusíveis

- Problema: chave do ralo não tinha função clara e o Fusível Velho não participava da energia superior.
- Solução: `HasDrainKey` reutiliza a chave retirada do ralo e destrava o painel técnico.
- Slots: `OldFuseInstalled` para o Fusível Velho do depósito e `UpperFuseInstalled` para o Fusível Superior da rouparia.
- Condição final: `TechnicalPanelUnlocked && OldFuseInstalled && UpperFuseInstalled` → `IsUpperPowerRestored`.
- O Quarto 203 só se torna forçável quando `IsUpperPowerRestored` é verdadeiro.
- Nenhuma geometria, colisão, porta física, rota ou conteúdo da perseguição foi alterado.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING [x] deck preservado: 49/49

Playtest manual aprovado em 2026-07-13: [x] painel bloqueia sem chave [x] chave do ralo destrava [x] Fusível Velho instala [x] Fusível Superior instala [x] um único fusível não liga energia [x] dois fusíveis ligam energia [x] 203 muda de estado [x] regressão Sprints 20/21/22

## Sprint 23 — Sistema simples de esconderijo / sala segura

- Local: atrás do balcão existente da recepção, centro global aproximado `(3,4; 0,75; -2,45)`.
- `SafeZone_FirstShelter`: `Area3D` de `2,0 × 1,5 × 1,2 m`, máscara exclusiva do player, sem corpo físico ou teleporte.
- `Interact_FirstHidingSpot`: interação pequena na face sul do balcão; prompt “Se esconder”.
- Integração: entrar no abrigo durante a perseguição encerra `FirstChase`, interrompe/oculta `Enemy_FirstPresence`; após a fuga normal, o abrigo continua ensinando o sistema uma única vez.
- Tutorial: “Quando ouvir passos próximos, procure um lugar escuro.” e “Fique quieto até eles se afastarem.”
- Sequência: “Segure a respiração...” → passos distantes → “Os passos se afastaram.” → novo objetivo.
- Sons existentes: `old_house_settle_01`, `distant_step_03` e `distant_step_04`, em volume baixo.
- Flags: `Sprint22Completed`, `PlayerInSafeZone`, `PlayerHidden`, `SafeRoomDiscovered`, `FirstHideTutorialShown` e `Sprint23Completed`.
- Nenhum piso, parede, varanda, cômodo, collider físico ou rota aprovada foi modificado.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING [x] safe zone térrea, pequena e sem `StaticBody3D` [x] deck preservado: 49/49

Playtest manual aprovado em 2026-07-13: [x] perseguição ativa [x] objetivo de abrigo [x] zona atrás do balcão [x] prompt aparece [x] sequência completa [x] inimigo para/some [x] tutorial uma vez [x] controle preservado [x] saída livre [x] objetivo final [x] regressão Sprints 20/21/22 e Hotfix 22B

## Hotfix pós-Sprint 23 — guarda-roupas reutilizáveis e Ctrl

- Guarda-roupas vintage no térreo: recepção `(-4,0; -6,38)`, Quarto 102 `(-2,45; -16,98)` e cozinha `(5,5; -21,98)`.
- Cada shell possui peças visuais e colliders filhos equivalentes; o interior é aberto, agachável e contém uma `Area3D` pequena apenas no móvel.
- Após a primeira perseguição, entrar agachado e olhar para o fundo mostra “Se esconder no guarda-roupa”.
- O esconderijo reutiliza `PlayerInSafeZone`/`PlayerHidden`, encerra uma perseguição ainda ativa e não teleporta nem desabilita movimento.
- Correção do Ctrl: o clearance para levantar cobre apenas `Y=1,0…1,8 m` acima dos pés; a verificação antiga alcançava aproximadamente `Y=2,2 m` e confundia a laje com teto baixo.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING [x] deck 49/49 [x] sem alteração na varanda/ala superior

Playtest manual aprovado em 2026-07-13: [x] Ctrl abaixa [x] soltar Ctrl levanta na recepção/corredor/quartos [x] teto realmente baixo mantém agachado [x] sair do móvel permite levantar [x] três móveis acessíveis [x] colliders correspondem ao visual [x] sem bloqueio de corredor [x] prompt dentro do móvel [x] esconder/soltar funciona [x] regressão completa

## Sprint 24 — IA básica da presença

- Ativação: somente após `Sprint23Completed`.
- Presença reutilizada: `FirstEnemyChase/Enemy_FirstPresence`, ainda sem corpo físico ou collider bloqueador.
- Container: `World/Gameplay/EnemyAI`; cinco pontos em linha livre no térreo: recepção `(0; -5)`, entrada `(0; -9,5)`, meio `(0; -15,5)`, fundo `(0; -22)` e retorno `(0; -11,5)`.
- Patrulha: 1,35 m/s, pausas de 1,2–1,65 s e passos baixos; rota não entra em quartos, escada ou segundo andar.
- Visão: 6 m, cone total aproximado de 56°, reduzido a 3,2 m com player agachado; raycast em layer de parede impede visão através da geometria.
- Audição: corrida em movimento até 8 m; ignora agachamento, andar superior, safe zone e `PlayerHidden`.
- Alerta: projeta a última posição conhecida no eixo seguro do corredor e aproxima-se a 2,35 m/s sem atravessar paredes.
- Busca: espera 3,2 s; ao perder o alvo, toca passos distantes, marca `EnemyLostPlayer` e retorna à patrulha.
- Encontro próximo: apenas mensagem “Você escapou por pouco.” e reset de alerta; sem dano, morte, teleporte ou input travado.
- Flags: `EnemyPatrolActive`, `EnemyAlerted`, `EnemySearching`, `EnemyLostPlayer`, `EnemySawPlayerOnce`, `EnemyHeardPlayerOnce` e `Sprint24Completed`.
- Limitação: patrulha roteirizada central, sem NavMesh, entrada em quartos, escada, combate ou IA final.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena headless [x] F9: 0 ERROR / 0 WARNING [x] deck 49/49 [x] nenhum collider/Area3D novo de percepção

Playtest manual aprovado em 2026-07-13: [x] não ativa antes da Sprint 23 [x] patrulha ativa depois [x] cinco pontos livres [x] não atravessa parede [x] não sobe escada [x] vê somente com linha livre [x] correr alerta [x] agachar reduz visão/audição [x] balcão e guarda-roupas cancelam [x] busca termina [x] retorna à patrulha [x] não bloqueia player [x] regressão Sprints 20–23 e Hotfix 22B

Hotfix validado: os esconderijos deixam de chamar o desligamento visual legado da Sprint 22 após `Sprint23Completed`; ao perder o jogador, a presença permanece visível e retoma a patrulha.

## Sprint 25 — Segunda perseguição real

- Condição: `Sprint24Completed`; a disponibilidade e o início são separados para impedir ativação precoce.
- Trigger: `Trigger_SecondChaseStart`, pequeno (`1,8 × 1,5 × 1,2 m`), no fundo térreo em `(0; 0,8; -21)`, máscara exclusiva do player e one-shot.
- Rota: cinco markers no eixo central entre fundo, corredor e aproximação da recepção; perseguição usa posição conhecida projetada em `X=0`, `Z=-23,5..-4,5`.
- IA: pausa patrulha, aviso sonoro de 0,75 s, Chase a 3,25 m/s, visão/raycast e audição já validadas; sem linha livre passa a Search.
- Safe zones: balcão ou guarda-roupa projetam a aproximação para o corredor externo, iniciam busca de 4 s e concluem `SecondChaseEscaped`, `SecondChaseFinished` e `Sprint25Completed`.
- Mensagens: “Ele ouviu você.”, “Corra. Se esconda.”, “Não respire.”, “Fique quieto.”, “Os passos se afastaram.” e objetivo “Procure outra saída da pensão.”
- Sons: `old_house_settle_01`, passos `distant_step_01/03/04` e `distant_knock_02`, todos em volume moderado; flicker curto e reversível no corredor.
- Limitações: sem NavMesh, combate, morte, dano, teleporte ou colisão física; perseguição restrita à rota central do térreo.

Validação automática: [x] build C# (0 erros/0 avisos) [x] cena oficial headless [x] F9 0 ERROR / 0 WARNING [x] trigger térreo `Y=0,05..1,55`, sem alcance do segundo andar [x] nenhum collider físico no inimigo [x] deck 49/49

Playtest manual obrigatório: [ ] fluxo Sprints 20–24 [ ] Sprint 25 não ativa antes [ ] objetivo leva ao fundo [ ] trigger one-shot [ ] duas mensagens iniciais [ ] chase escapável [ ] não bloqueia player [ ] não atravessa parede [ ] não sobe escada [ ] safe zone/guarda-roupa encerra [ ] busca do lado de fora [ ] passos se afastam [ ] objetivo final [ ] não repete [ ] regressão térreo/escada/ala/varanda/203/puzzle
