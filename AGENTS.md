## REGRA CRÍTICA — SPRINT 26

Esta sprint é somente de eventos ambientais. É proibido alterar geometria aprovada, criar paredes, pisos, cômodos, colliders estruturais ou portas físicas novas. Eventos ambientais devem usar triggers pequenos e localizados, timers, áudio, luz e objetos visuais não bloqueantes. Nenhum evento ambiental pode prender, teleportar, empurrar ou matar o player, iniciar durante perseguição ou esconderijo, nem quebrar puzzle, perseguição, safe zone ou IA. Todo visual ambiental deve ser sem colisão por padrão.

## REGRA CRÍTICA — SPRINT 25

Esta sprint é de comportamento da presença e perseguição controlada. É proibido alterar geometria aprovada, reconstruir paredes, pisos, cômodos, varanda, escada ou térreo, criar collider gigante, boundary global ou trigger atravessando andares. O inimigo não pode bloquear fisicamente o player. A safe zone deve encerrar a perseguição. Todo evento de chase deve ser one-shot, possuir começo/meio/fim e não repetir infinitamente.

## REGRA CRÍTICA — SPRINT 24

Esta sprint é apenas de IA básica da presença. É proibido alterar a geometria aprovada, reconstruir cômodos, pisos, paredes, varanda ou térreo, criar colliders gigantes ou triggers entre andares. O inimigo protótipo não pode bloquear ou prender o player, atravessar paredes de forma absurda ou subir ao segundo andar. Safe zone e esconderijo devem sempre cancelar visão e audição da IA.

## REGRA CRÍTICA — SPRINT 23

Esta sprint cria um sistema simples de esconderijo/sala segura. É proibido reconstruir mapa, pisos, paredes, varanda ou cômodos. Safe zones devem ser `Area3D` pequenas, localizadas, sem colisão física bloqueadora e sem alcançar outro andar. O esconderijo não pode prender, teleportar, empurrar ou travar o jogador e deve ser testado com a perseguição da Sprint 22 ativa.

## REGRA CRÍTICA — HOTFIX 22B

Este hotfix é somente de lógica de puzzle. É proibido alterar layout, paredes, pisos, portas físicas, varanda, navegação, cômodos ou o inimigo da Sprint 22. Item de puzzle não pode ficar sem função: a chave do ralo deve destravar claramente o painel técnico, e a energia superior exige o Fusível Velho e o Fusível Superior instalados.

## REGRA CRÍTICA — SPRINT 22

Esta sprint é de evento/inimigo protótipo, não de construção estrutural. É proibido reconstruir mapa, paredes, pisos, cômodos ou varanda. O inimigo protótipo não pode possuir colisão bloqueadora nem prender o player. A perseguição deve ser curta, roteirizada, reversível e one-shot antes de qualquer IA complexa. Nenhum evento pode teleportar o player, alcançar outro andar ou criar collider gigante.

## REGRA CRÍTICA — SPRINT 21

Esta sprint é de evento e presença, não de construção estrutural. É proibido reconstruir cômodos, pisos, paredes, varanda, térreo ou ala superior. Qualquer trigger deve ser pequeno, localizado, condicionado à flag correta e one-shot. Presenças visuais não podem ter colisão nem bloquear caminho. Nenhum evento pode teleportar, empurrar, matar, causar dano ou travar o jogador.

## REGRA CRÍTICA — SPRINT 20

Esta sprint só pode mexer no Quarto 203, seus eventos, suas interações e pequenos objetos internos. É proibido reconstruir a ala superior, alterar deck/varanda ou térreo, e criar paredes globais, pisos novos ou colliders soltos.

Toda geometria nova deve ser local ao Quarto 203, ter nome claro e collider alinhado apenas quando necessário. Porta de evento não pode duplicar painel ou moldura. Evento forte não pode teleportar, causar dano ou prender o player sem saída. Trigger de evento deve tocar uma única vez.

## REGRA CRÍTICA — SPRINT 19C

## REGRA CRÍTICA — SPRINT 19D

## REGRA CRÍTICA — SPRINT 19E

Esta sprint é um rebuild limpo da ala superior. Não remendar ou manter sala bugada; não deixar parede visual sem colisão, cômodo atrás de parede sem porta, item/painel atrás de parede, cômodo acessível sem teto ou objeto antigo duplicado. F9 sem erros é obrigatório antes de commit.

Regras permanentes: nunca criar cômodo atrás de parede sem porta; nunca criar parede visual sem collider; nunca deixar item/painel atrás de parede; nunca deixar cômodo acessível sem teto visual; nunca manter objetos antigos duplicados após rebuild.

É proibido compensar bug de parede com trigger invisível, boundary global ou collider solto no corredor. Toda parede visual acessível deve ter colisão compatível. Todo cômodo do puzzle deve ter acesso por abertura real. Nenhum item, painel ou objetivo pode ficar atrás de parede atravessável. Nenhum cômodo pode ter saída para limbo ou ficar sem fechamento mínimo de paredes/teto quando isso permite fuga da área.

Regras permanentes: todo objetivo de puzzle deve estar em cômodo acessível por rota real; toda parede visual acessível deve bloquear o player; todo cômodo jogável deve ser minimamente fechado; nunca esconder bug estrutural com collider solto no corredor.

Nesta sprint é proibido criar novos cômodos grandes.
O objetivo é corrigir os cômodos existentes.

Falha automática se:
- o player atravessar parede visual;
- o player sair para fora da área jogável;
- o player precisar atravessar parede para chegar em item ou painel;
- existir corredor falso sem saída atravessável;
- o arame torto ficar em local bugado;
- o painel técnico ficar atrás de parede;
- o chão/deck da varanda for alterado.

Regras permanentes derivadas:
- item nunca pode ficar em corredor bugado;
- painel nunca pode ficar atrás de parede;
- parede visual acessível sempre precisa bloquear;
- saída para limbo é bug crítico;
- todo cômodo precisa ser acessado por porta/abertura real.

## REGRA CRÍTICA DA SPRINT 19B

Esta sprint falha automaticamente se for criado apenas um cômodo, apenas um cubo, apenas um bloco isolado ou uma sala sem conexão.

Objetivo obrigatório:
Construir uma ala superior completa com no mínimo:
- 1 corredor principal;
- 4 cômodos acessíveis;
- 1 cômodo trancado/futuro;
- 1 sala técnica;
- 1 puzzle funcional;
- portas/interações testadas.

Proibido considerar sprint de cômodos concluída com apenas um bloco/placeholder isolado.
Mínimo 4 cômodos novos acessíveis para a ala superior.
Usar AABB do `UpperWing_CollisionDeck` / MasterSlab aprovado.
Não construir só no canto.
Não criar placeholder isolado sem conexão ao corredor.

## ✅ VARANDA APROVADA E CONGELADA PARA GAMEPLAY

Estado aprovado (2026-07-12):
- Player anda na varanda/laje sem cair;
- sem teleporte térreo → segundo andar;
- escada, porta verde e Quarto 203 funcionais;
- varanda aberta e navegável;
- `UpperWing_CollisionDeck` é o chão oficial e está **congelado**.

Não mexer mais no chão/deck da varanda.
Não recriar mureta, boundary global, colliders soltos ou paredes invisíveis na área caminhável.
Próximas paredes/colisões: só cômodos novos, collider filho da parede visual correspondente.

## Regra de cômodos da ala superior

Toda sala nova deve:
- usar o deck aprovado como piso físico;
- ter paredes próprias com colliders filhos;
- não criar piso físico global;
- não criar boundary global;
- não criar collider solto;
- não criar trigger que alcance outro andar;
- passar teste manual antes de commit;
- ocupar a maior parte da área vazia da laje (não um bloco no canto);
- conectar-se a um corredor principal.

## REGRA CRÍTICA — Não chutar collider de parede

É proibido criar collider de parede por coordenada chutada.

Para paredes de cômodos:
- só criar collider como filho da parede visual correspondente;
- o collider deve usar o AABB/tamanho da parede visual;
- o collider não pode ficar no meio da área caminhável;
- o collider não pode bloquear o caminho principal;
- o collider não pode atravessar outro andar;
- o collider não pode ser boundary global.

Para varanda:
- NÃO criar collider lateral/global por enquanto.
- NÃO criar mureta.
- NÃO criar guarda-corpo.
- NÃO criar boundary.
- A varanda deve permanecer aberta e navegável.
- As próximas colisões serão apenas das paredes dos cômodos novos.

Se o player bater em parede invisível:
- remover o collider;
- não tentar ajustar por cima;
- não commitar.

REGRA BLOQUEANTE:
Nenhum trigger, Area3D, DebugFallRecovery, teleporte, porta, puzzle ou interação do segundo andar pode afetar o player enquanto ele está no primeiro andar.

Nenhum hotfix de cenário pode ser considerado concluído se:
- o player cair no limbo;
- o player for teleportado sem intenção;
- o player for jogado do térreo para o segundo andar;
- uma Area3D do segundo andar atravessar o térreo;
- um DebugFallRecovery ativar durante gameplay normal;
- houver parede/placa/mureta bugada na varanda;
- houver collider invisível sem nome e sem função.

Compilar não aprova.
Cena carregar não aprova.
Só aprova com playtest manual.

## Regra da varanda aprovada

A navegação da varanda (chão/deck) foi aprovada. A varanda permanece **aberta**.

É proibido:
- alterar UpperWing_CollisionDeck sem pedido explícito;
- recriar o chão da varanda;
- criar boundary global / mureta / guarda-corpo / collider lateral chutado;
- criar parede visual gigante;
- criar collider que atravesse dois andares;
- criar collider que afete o térreo ou fique no meio do caminho.

Próximas colisões: apenas paredes dos cômodos novos, como filhos da mesh visual correspondente.

REGRA BLOQUEANTE: compilar não é aprovação e carregar a cena não é aprovação. Só aprova se o player não cair no limbo, não atravessar teto pulando, andar para direita/esquerda/frente/trás/diagonais na laje, não ficar preso em parede e não encontrar piso visual sem colisão.

ATENÇÃO: nenhuma task de cenário pode ser concluída se o player cair no limbo/direita, existir parede atravessando escada, collider invisível sem função, piso visual sem colisão, duplicata velha ou builder recriando geometria. Teste manual é obrigatório; compilar não é aprovação.

REGRA BLOQUEANTE: nenhum agente pode considerar cenário concluído se o player cair no limbo ou do segundo para o primeiro andar, atravessar teto pulando, existir piso visual sem colisão, mureta/lixo antigo, collider parcial, piso duplicado competindo ou builder antigo recriando geometria. Compilar e carregar a cena não aprovam; o player precisa andar e pular sem atravessar ou cair.

# BREU — REGRA OBRIGATÓRIA PARA CENÁRIO

Antes de qualquer alteração em cenário, piso, parede, teto, porta, escada, varanda, collider, blocker ou interação, Cursor, Codex e qualquer agente devem ler:

- `.cursor/rules/level-geometry-guardrails.mdc`
- `docs/production/LEVEL_GEOMETRY_GOLDEN_RULES.md`

Nenhuma task de cenário passa se o player cair no limbo ou ao andar para direita/esquerda; se piso for só visual; se CollisionShape3D não cobrir a área visual; se houver collider invisível sem função, duplicata antiga, prompt distante ou invasão entre andares.

Teste real obrigatório: o player precisa andar de ponta a ponta, incluindo direita, esquerda e diagonais.

# BREU — Instruções obrigatórias para Codex, Cursor e agentes

Antes de alterar cenário, geometria, piso, parede, teto, porta, varanda, escada, blocker, collider ou interação, leia e siga:

- `.cursor/rules/level-geometry-guardrails.mdc`
- `docs/production/LEVEL_GEOMETRY_GOLDEN_RULES.md`
- `docs/production/LEVEL_CHANGE_CHECKLIST.md`, se existir

## Regra principal

Nenhum agente pode considerar sprint de cenário concluída se houver queda no limbo, piso sem colisão, duplicata, objeto velho acumulado, builder recriando geometria, invasão entre andares, prompt fantasma, collider sem função, sala inacessível ou interação através de parede.

## Regra de isolamento por andar

Toda Area3D, trigger, interação, teleporte, recover, safe marker ou evento precisa pertencer claramente a um andar.

Proibido:
- trigger do segundo andar alcançar o primeiro andar;
- trigger da varanda alcançar recepção/corredor térreo;
- DebugFallRecovery teleportar player do térreo para o segundo andar durante gameplay normal;
- Area3D com altura gigante sem justificativa;
- boundary global de varanda;
- collider invisível sem nome claro.

Teste obrigatório:
Depois de abrir qualquer porta/evento do segundo andar, voltar ao térreo e correr pelo corredor.
Se o player for jogado para cima, a task falhou.

## Ordem obrigatória

1. Piso sólido.
2. Colisão validada.
3. Parede.
4. Teto.
5. Circulação.
6. Porta.
7. Interação.
8. Puzzle.
9. Áudio.
10. Arte final.

Nunca inverter essa ordem.

## Regra anti-acúmulo

Antes de criar uma nova versão, remover versão, colliders, prompts, builders e nodes duplicados antigos. Não esconder lixo; remover lixo.

## Teste obrigatório

Toda alteração de cenário exige F6. Todo piso novo deve ser atravessado de ponta a ponta para frente, trás, esquerda, direita, diagonais e ida/volta. Se cair, a task falhou.

## Regra de hotfix visual cirúrgico

- Priorizar ajuste fino ou remoção localizada.
- Proibido fazer cleanup destrutivo para esconder resíduos visuais.
- Proibido apagar parede estrutural sem provar que o node é duplicado ou residual.
- Parede inferior atravessando o piso superior: rebaixar somente o topo e preservar o footprint.
