# BREU — Regras de Ouro para Cenário e Colisão

Documento obrigatório para Cursor, Codex e agentes antes de alterações de cenário.

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
- boundary global / mureta / placa bugada na varanda.

## Ordem obrigatória

Piso sólido → colisão → parede → teto → circulação → porta → interação → puzzle → áudio → arte final.

## Teste binário de piso

Todo piso deve ser atravessado no centro, esquerda, direita, fundo, frente, diagonais e ida/volta. Se o player cair, a task falhou.

## Caso real: laje superior parcial

A ala superior usou múltiplos pisos estreitos e duplicados. A colisão central não alcançava a lateral direita, permitindo queda ao primeiro andar. Nenhum piso parcial pode ser commitado; direita, esquerda e diagonais são obrigatórias.

## Caso real: DebugFallRecovery jogando o player para cima

Após visitar a varanda, o failsafe marcava `_enteredUpperWing` e tratava qualquer Y < 1,9 dentro do AABB enorme do deck como “queda”, teleportando o player do corredor/recepção para `SafeMarker_SecondFloor`. Recovery só pode ativar abaixo de KillY real (void). Voltar ao térreo e correr após abrir a varanda é teste obrigatório.

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

A navegação da varanda foi aprovada.

É proibido:
- alterar UpperWing_CollisionDeck sem pedido explícito;
- recriar o chão da varanda;
- criar boundary global visual;
- criar mureta genérica;
- criar parede visual gigante;
- criar collider que atravesse dois andares;
- criar collider que afete o térreo.

Paredes da varanda só podem receber colliders finos, invisíveis, alinhados às paredes visíveis existentes (`BalconyWallCollider_Left` / `_Right` / `_FrontGuard`).

Antes de qualquer expansão de cômodos, a varanda precisa continuar andável, sem queda, sem teleporte, sem travamento e com paredes bloqueando o player.

## Regra anti-acúmulo

Antes de nova versão, remover geometria, colliders, prompts, builders e nodes duplicados antigos. Não esconder lixo; remover lixo.
