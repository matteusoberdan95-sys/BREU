# Playtest — Segundo andar blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 10 — Segundo andar blockout 01  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

**Cena térreo preservada:** `PensaoTerreoBlockout01.tscn` (baseline intacta)

---

## Status

**Hotfix Sprint 10 aplicado (2026-07-11).** Validar com F6 antes de marcar sprint como aprovada.

---

## Sprint 10 Hotfix — Second Floor Access Fix

### Problemas encontrados

1. **Parede bloqueando saída da escada** — `Wall_Second_Back` @ z = -25,0 e `UpperLanding_Rail_Back` ficavam exatamente no topo da rampa (z ≈ -24,7), impedindo o player de sair.
2. **Prompt da cozinha através de parede** — `KitchenInspect` com Area3D 1,5 m e raycast sem checagem de oclusão por geometria world.

### Correções aplicadas

- Removido `UpperLanding_Rail_Back`; guarda-corpos laterais reposicionados sem bloquear saída +Z.
- `Wall_Second_Back` movida para z = -27,5 (fechamento atrás do patamar, não na rampa).
- Criado `UpperLanding_Main` (3,5 × 3,5 m) + transição `Floor_Second_StairTransition` ampliada.
- Criado `UpperCorridor_Main` com piso contínuo até a porta bloqueada.
- Adicionada `Wall_Second_East` para fechar frestas laterais.
- Áreas de interação dos quartos reduzidas; cozinha reduzida para 0,9 × 1,0 × 0,9 m.
- `PlayerInteractionRaycast`: raycast world-only para oclusão — parede mais próxima cancela prompt.

### Checklist final (F6)

**Escada / segundo andar**

- [ ] Player sobe a escada
- [ ] Player NÃO bate em parede no topo
- [ ] Player acessa UpperLanding_Main
- [ ] Player anda no corredor superior
- [ ] Player entra no quarto 201
- [ ] Prompt do quarto 201 funciona
- [ ] Player entra no quarto 202
- [ ] Prompt do quarto 202 funciona
- [ ] Player chega na porta bloqueada superior
- [ ] Prompt da porta bloqueada funciona
- [ ] Player não atravessa a porta bloqueada
- [ ] Player volta para a escada
- [ ] Player desce para o térreo
- [ ] Player não cai por frestas
- [ ] Player não vê limbo grande no segundo andar
- [ ] Player não atravessa paredes principais

**Raycast / interação**

- [ ] Prompt da cozinha só aparece olhando para a cozinha
- [ ] Prompt da cozinha NÃO aparece através de parede
- [ ] Prompt da cozinha NÃO aparece no segundo andar
- [ ] Prompt do quarto 201 aparece só no quarto 201
- [ ] Prompt do quarto 202 aparece só no quarto 202
- [ ] Prompt da porta bloqueada aparece só mirando a porta
- [ ] Não aparece caixa vazia

**Regressão**

- [ ] Movimento continua aprovado
- [ ] HUD continua aprovado
- [ ] Lanterna continua funcionando
- [ ] Debug F10/F11 continua funcionando
- [ ] Chave ainda funciona
- [ ] Depósito ainda destranca
- [ ] Fusível ainda funciona
- [ ] Player não cai do mapa
- [ ] Player não fica preso em cantos

---

## Checklist — Rota completa

- [ ] Player nasce na trilha
- [ ] Player entra na Pensão
- [ ] Player acessa recepção
- [ ] Player acessa corredor do térreo
- [ ] Player acessa escada
- [ ] Player sobe a escada
- [ ] Player chega no segundo andar
- [ ] Player anda no corredor superior
- [ ] Player entra no quarto 201
- [ ] Player vira a câmera no quarto 201
- [ ] Interação do quarto 201 funciona
- [ ] Player entra no quarto 202
- [ ] Player vira a câmera no quarto 202
- [ ] Interação do quarto 202 funciona
- [ ] Player tenta abrir porta bloqueada superior
- [ ] Prompt/mensagem da porta bloqueada funciona
- [ ] Player volta para a escada
- [ ] Player desce para o térreo
- [ ] Player volta para recepção

---

## Checklist — Segurança

- [ ] Player não cai do segundo andar
- [ ] Player não cai por frestas
- [ ] Player não atravessa paredes principais
- [ ] Player não fica preso em quinas
- [ ] Player não quica na escada
- [ ] Player não atravessa a porta bloqueada
- [ ] Sem limbo grande visível no segundo andar
- [ ] Sem z-fighting forte

---

## Checklist — Regressão

- [ ] Movimento continua aprovado
- [ ] Sprint funciona
- [ ] Crouch funciona
- [ ] Lean Q/R funciona
- [ ] Look back funciona
- [ ] HUD funciona
- [ ] Debug F10/F11 funciona
- [ ] Interações antigas funcionam
- [ ] Puzzle do depósito ainda funciona

---

## Não testar nesta sprint

- Teto / telhado
- Inimigo / combate
- Arte final / GLB
