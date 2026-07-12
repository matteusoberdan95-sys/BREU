# BREU — Instruções obrigatórias para Codex, Cursor e agentes

Antes de alterar cenário, geometria, piso, parede, teto, porta, varanda, escada, blocker, collider ou interação, leia e siga:

- `.cursor/rules/level-geometry-guardrails.mdc`
- `docs/production/LEVEL_GEOMETRY_GOLDEN_RULES.md`
- `docs/production/LEVEL_CHANGE_CHECKLIST.md`, se existir

## Regra principal

Nenhum agente pode considerar sprint de cenário concluída se houver queda no limbo, piso sem colisão, duplicata, objeto velho acumulado, builder recriando geometria, invasão entre andares, prompt fantasma, collider sem função, sala inacessível ou interação através de parede.

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
