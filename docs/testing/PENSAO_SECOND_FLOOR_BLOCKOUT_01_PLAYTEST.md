# Playtest — Segundo andar blockout 01

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 10 — Segundo andar blockout 01  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_SECOND_FLOOR_BLOCKOUT_BASELINE.md`

**Cena térreo preservada:** `PensaoTerreoBlockout01.tscn` (baseline intacta)

---

## Status

**✅ Sprint 10 APROVADA (2026-07-11)** — blockout funcional do segundo andar validado pelo usuário.

**Cena validada:** `PensaoVerticalBlockout01.tscn`

---

## Checklist aprovado — Sprint 10

### Escada

- [x] Player sobe a escada
- [x] Player não trava no meio
- [x] Player não bate em teto
- [x] Player não cai por lateral
- [x] Player desce a escada sem quicar

### Landing superior

- [x] Saída da escada livre (sem parede na cara)
- [x] `UpperLanding_Main` acessível
- [x] Transição clara escada → corredor

### Guarda-corpo / caixa de escada

- [x] Vão da escada protegido (`StairBox_Wall_*` + `Stairwell_Rail_*`)
- [x] Player não cai pelo vão
- [x] Visão lateral do poço controlada (blockout aceito)

### Corredor superior

- [x] Corredor superior acessível após sair da escada
- [x] Sem bloqueio central / placas atravessadas
- [x] Largura jogável (2,4 m)
- [x] Player alcança porta bloqueada superior

### Quartos superiores

- [x] Room201 acessível + interação funciona
- [x] Room202 acessível + interação funciona
- [x] Porta bloqueada superior funciona (prompt + colisão)

### Retorno ao térreo

- [x] Player volta à escada
- [x] Player desce ao térreo
- [x] Fluxo ida e volta completo

### Regressão do térreo

- [x] Trilha → pensão → corredor → quarto 102
- [x] Cozinha, recepção, depósito navegáveis
- [x] Chave e fusível funcionam
- [x] Interações do térreo intactas

### Regressão do puzzle

- [x] Puzzle do depósito destranca
- [x] Fluxo chave → depósito → fusível preservado

### HUD

- [x] HUD intacto (stamina, vida, lanterna, prompts)

### Player movement

- [x] PlayerController aprovado
- [x] Camera feel aprovado (sprint, crouch, lean, look back)

---

## Sprint 10 Hotfix 3 — Corridor Unlock + Stair Box + Wall Sealing

### Bloqueios / problemas encontrados

1. **`Wall_Second_CorridorNorthCap`** atravessava x = 0 e bloqueava entrada no corredor superior (vão da porta centrado em x = -4,1, corredor em x = 0).
2. **`UpperLanding_CorridorBridge`** separado gerava leitura confusa no topo da escada.
3. Caixa de escada incompleta — visão crua do térreo pelas laterais do poço.
4. Guarda-corpos baixos sem paredes laterais altas no poço.

### Correções aplicadas

- **Removido** `Wall_Second_CorridorNorthCap` (bloqueio principal do corredor).
- **Landing unificado:** `UpperLanding_Main` (5,4 × 3,2 m) conecta escada → corredor x = 0; corredor norte estendido até z = -19,5.
- **Caixa de escada:** `StairBox_Wall_West/East/North` (3,0 m) + `StairBox_Wall_EastWing` fechando ala leste.
- **Guarda-corpos** apenas na borda sul do vão (`Stairwell_Rail_*`); saída livre ~3,2 m.
- Vão da escada reduzido para 5,8 × 8,2 m (mais controlado).

### Checklist final (F6)

**Térreo**

- [ ] Entrar na pensão
- [ ] Andar no corredor
- [ ] Quarto 102 + interação
- [ ] Chave → depósito → fusível
- [ ] HUD e movimento intactos

**Escada**

- [ ] Subir sem travar
- [ ] Não bater em teto
- [ ] Não cair por lateral

**Segundo andar**

- [ ] Sair da escada sem bater em parede
- [ ] Landing superior acessível
- [ ] Corredor superior acessível (sem bloqueio central)
- [ ] Quartos 201/202 acessíveis + interações
- [ ] Porta bloqueada superior funciona
- [ ] Paredes sem gretas graves
- [ ] Vão da escada protegido (caixa + guarda-corpo)
- [ ] Sem limbo exposto forte
- [ ] Player não atravessa paredes / não fica preso
- [ ] Descer escada OK

---

## Sprint 10 Hotfix 2 — Second Floor Sealing and Stairwell Guardrail

### Vãos / problemas encontrados

1. Laje segmentada com fresta entre `Floor_Second_Main_NorthEast` e `NorthCap` (≈ 1,45 m).
2. Vão da escada sem proteção — player via buraco aberto para o térreo.
3. Paredes externas superiores estendiam-se além da laje (sobre varanda), parecendo soltas.
4. Laterais sul e norte do corredor superior sem fechamento completo.

### Correções aplicadas

- **Laje contínua:** `NorthBridge`, faixas `WestEdge`/`EastEdge`, `NorthWestCap`; sul estendido até parede frontal (z = -5,8).
- **Guarda-corpos:** `Stairwell_Rail_Left/Right/Back/Front_Side_West/Front_Side_East` (1,1 m, colisão); saída sul da escada livre (~3,1 m).
- **Vedação interna:** `Wall_Second_SouthFlank_West/East`, `Wall_Second_CorridorNorthCap` (vão para acesso da escada).
- **Shell ajustado:** `Wall_Second_Left/Right` limitadas à profundidade real da laje superior.

### Checklist final (F6)

**Segundo andar**

- [ ] Player sobe a escada
- [ ] Player chega ao segundo andar
- [ ] Player não bate em parede no topo
- [ ] Vão da escada está protegido
- [ ] Player não cai pelo vão da escada
- [ ] Player anda no corredor superior
- [ ] Player entra no Room201
- [ ] Interação Room201 funciona
- [ ] Player entra no Room202
- [ ] Interação Room202 funciona
- [ ] Player chega na porta bloqueada superior
- [ ] Porta bloqueada superior funciona
- [ ] Player desce a escada
- [ ] Piso superior parece contínuo
- [ ] Não há buracos grandes além do vão da escada
- [ ] Paredes superiores fecham melhor a leitura
- [ ] Não há limbo grande visível

**Regressão**

- [ ] Térreo continua funcionando
- [ ] Puzzle do depósito continua funcionando
- [ ] HUD continua funcionando
- [ ] Movimento continua aprovado
- [ ] Interações não quebraram
- [ ] Player não atravessa paredes principais

---

## Sprint 10 Rebuild — Clean Second Floor

### Problemas do layout anterior

1. Segundo andar desproporcional — laje pequena deslocada para oeste (x ≈ -4,1), não cobria o térreo.
2. Paredes soltas e blocos no meio bloqueando saída da escada.
3. Limbo visível ao olhar para cima a partir do térreo.
4. Corredor superior desalinhado do eixo do corredor térreo.

### Reconstrução aplicada

- **Removido** layout anterior (corredor oeste, pisos fragmentados, rails com colisão, shell parcial).
- **Floor_Second_Main** — laje segmentada **14,08 × 44,58 m** (mesmas dimensões do piso térreo), com vão só na escada.
- **UpperLanding_Main** — patamar 3,5 × 3,5 m + pontes de encaixe rampa/corredor.
- **UpperCorridor_Main** — corredor x = 0, largura 2,4 m, z = -20 a -7,5.
- **Room201 / Room202** — espelham quarto 102 e cozinha do térreo.
- **UpperBlockedDoor** — porta trancada no fim do corredor.
- **Wall_Second_Front/Back/Left/Right** — caixa aberta proporcional ao casco do edifício.
- Luzes superiores reposicionadas no eixo central.

### Checklist final (F6)

**Segundo andar**

- [ ] Player sobe a escada
- [ ] Player NÃO bate em parede no topo
- [ ] Player acessa UpperLanding_Main
- [ ] Player anda no Floor_Second_Main
- [ ] Player anda no UpperCorridor_Main
- [ ] Player entra no Room201
- [ ] Player consegue virar câmera no Room201
- [ ] Interação Room201 funciona
- [ ] Player entra no Room202
- [ ] Player consegue virar câmera no Room202
- [ ] Interação Room202 funciona
- [ ] Player chega na UpperBlockedDoor
- [ ] Prompt da UpperBlockedDoor funciona
- [ ] Player não atravessa UpperBlockedDoor
- [ ] Player consegue voltar para a escada
- [ ] Player desce para o térreo
- [ ] Player não cai por fresta
- [ ] Segundo andar parece proporcional ao térreo
- [ ] Não há bloco no meio impedindo navegação
- [ ] Não há limbo grande visível no segundo andar

**Térreo / regressão**

- [ ] Térreo continua navegável
- [ ] Recepção continua funcionando
- [ ] Corredor térreo continua funcionando
- [ ] Quarto 102 continua funcionando
- [ ] Cozinha continua funcionando
- [ ] Depósito continua funcionando
- [ ] Chave continua funcionando
- [ ] Fusível continua funcionando
- [ ] Bilhete continua funcionando
- [ ] Prompt da cozinha não aparece através de parede
- [ ] HUD continua funcionando
- [ ] Movimento continua aprovado

---

## Sprint 10 Hotfix — Second Floor Access Fix (histórico)

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
