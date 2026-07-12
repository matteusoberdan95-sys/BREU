# BREU — Regras de Ouro para Cenário e Colisão

Documento obrigatório para Cursor, Codex e agentes antes de alterações de cenário.

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

## Problemas que não podem voltar

- piso visual sem colisão ou piso parcial;
- queda do segundo andar, inclusive para direita/esquerda;
- objetos/portas antigas duplicadas;
- builders recriando geometria aprovada;
- prompts longe ou através de parede;
- teto térreo invadido pelo segundo andar;
- blockers invisíveis, props no caminho, salas inacessíveis ou corredores com limbo;
- teleporte do térreo para o segundo andar após abrir a varanda;
- DebugFallRecovery ativando em gameplay normal no corredor/recepção;
- boundary global / mureta / placa / collider chutado no meio da varanda.

## Ordem obrigatória

Piso sólido → colisão → parede → teto → circulação → porta → interação → puzzle → áudio → arte final.

## Teste binário de piso

Todo piso deve ser atravessado no centro, esquerda, direita, fundo, frente, diagonais e ida/volta. Se o player cair, a task falhou.

## Caso real: laje superior parcial

A ala superior usou múltiplos pisos estreitos e duplicados. A colisão central não alcançava a lateral direita, permitindo queda ao primeiro andar. Nenhum piso parcial pode ser commitado; direita, esquerda e diagonais são obrigatórias.

## Caso real: DebugFallRecovery jogando o player para cima

Após visitar a varanda, o failsafe marcava `_enteredUpperWing` e tratava qualquer Y < 1,9 dentro do AABB enorme do deck como “queda”, teleportando o player do corredor/recepção para `SafeMarker_SecondFloor`. Recovery só pode ativar abaixo de KillY real (void). Voltar ao térreo e correr após abrir a varanda é teste obrigatório.

## Caso real: colliders de parede chutados na varanda

`BalconyWallCollider_Left/Right/FrontGuard` foram criados por coordenada e ficaram no meio do caminho, bloqueando a circulação antes das paredes reais. Rollback obrigatório: remover, não ajustar. Varanda fica aberta; colliders futuros só como filhos de paredes visuais de cômodos.

## Regra de isolamento por andar

Toda Area3D, trigger, interação, teleporte, recover, safe marker ou evento precisa pertencer claramente a um andar.

Proibido:
- trigger do segundo andar alcançar o primeiro andar;
- trigger da varanda alcançar recepção/corredor térreo;
- DebugFallRecovery teleportar player do térreo para o segundo andar durante gameplay normal;
- Area3D com altura gigante sem justificativa;
- boundary global de varanda;
- collider invisível sem nome claro.

Volumes lógicos: `FirstFloorVolume`, `SecondFloorVolume`, `UpperWingVolume`.
Debug: F8 `PlayerAreaProbe`, F9 `FloorTriggerIsolationChecker` / `LevelSanityChecker`.

## Regra da varanda aprovada

A navegação da varanda (chão/deck) foi aprovada. A varanda permanece aberta.

É proibido:
- alterar UpperWing_CollisionDeck sem pedido explícito;
- recriar o chão da varanda;
- criar boundary global / mureta / guarda-corpo / collider lateral chutado;
- criar parede visual gigante;
- criar collider que atravesse dois andares;
- criar collider que afete o térreo ou fique no meio do caminho.

## Regra anti-acúmulo

Antes de nova versão, remover geometria, colliders, prompts, builders e nodes duplicados antigos. Não esconder lixo; remover lixo.
