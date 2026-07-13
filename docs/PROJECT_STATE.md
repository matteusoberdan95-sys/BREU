# BREU — Estado do projeto

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
