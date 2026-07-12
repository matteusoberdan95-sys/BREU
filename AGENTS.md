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
