# BREU — Regras de Ouro para Cenário e Colisão

Documento obrigatório para Cursor, Codex e agentes antes de alterações de cenário.

## Problemas que não podem voltar

- piso visual sem colisão ou piso parcial;
- queda do segundo andar, inclusive para direita/esquerda;
- objetos/portas antigas duplicadas;
- builders recriando geometria aprovada;
- prompts longe ou através de parede;
- teto térreo invadido pelo segundo andar;
- blockers invisíveis, props no caminho, salas inacessíveis ou corredores com limbo.

## Ordem obrigatória

Piso sólido → colisão → parede → teto → circulação → porta → interação → puzzle → áudio → arte final.

## Teste binário de piso

Todo piso deve ser atravessado no centro, esquerda, direita, fundo, frente, diagonais e ida/volta. Se o player cair, a task falhou.

## Caso real: laje superior parcial

A ala superior usou múltiplos pisos estreitos e duplicados. A colisão central não alcançava a lateral direita, permitindo queda ao primeiro andar. Nenhum piso parcial pode ser commitado; direita, esquerda e diagonais são obrigatórias.

## Regra anti-acúmulo

Antes de nova versão, remover geometria, colliders, prompts, builders e nodes duplicados antigos. Não esconder lixo; remover lixo.
