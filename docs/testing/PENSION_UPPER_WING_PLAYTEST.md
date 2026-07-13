# Playtest — Saneamento 18C / ala superior

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
