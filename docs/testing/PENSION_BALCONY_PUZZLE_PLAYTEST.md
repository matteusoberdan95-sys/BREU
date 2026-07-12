# Playtest — Puzzle da Varanda (Sprint 17)

**Arquitetura 17E:** a ala é uma cena manual única. Testar com Visible Collision Shapes; `BalconyWingPuzzleSetup` não pode aparecer ativo no Scene Tree.

## Sprint 17F — Quarto 203 encontrável

O objetivo narrativo existia, mas não havia uma porta 203 clara no corredor. A porta bloqueada foi criada como cena estática em `X=-1,48 / Z=-10`, na parede esquerda do corredor superior e visível ao retornar da varanda.

- nó único `Door_Room203_Blocked`;
- placa simples `203` e painel marrom escuro;
- `Interact_Room203Door` curto, somente no lado do corredor;
- antes do caderno: “A porta está bloqueada.”;
- depois do caderno: bloqueio pelo outro lado + `door_scratch_01` + mensagem de unhas na madeira;
- a porta não abre nesta sprint.

Checklist F6 pendente: confirmar descoberta visual, alcance curto do prompt e ausência de prompt através da parede.

**Hotfix visual:** o primeiro playtest mostrou apenas o prompt porque o painel estava embutido na face da parede. Porta, moldura e placa 203 foram avançadas para o lado do corredor, eliminando ocultação e z-fighting.

**Cena:** `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Data:** 2026-07-12  
**Status:** 🔄 Implementado — aguardando F6

## Fluxo

1. Trilha → Pensão → chave depósito → fusível  
2. Mensagem pós-fusível sobre varanda  
3. 2º andar → porta verde bloqueada  
4. Quarto 201 → ler anotação  
5. Recepção → pegar chave da varanda  
6. Destravar porta verde → varanda → ala leste  
7. Quarto 203 + caderno da proprietária → voltar

## Itens / mensagens

| Item | Prompt | Mensagem |
|------|--------|----------|
| Nota 201 | Ler anotação | Chave perto da recepção |
| Chave (sem nota) | Examinar | Nada chama atenção |
| Chave (com nota) | Pegar chave da varanda | Chave enferrujada |
| Porta (sem chave) | Tentar abrir varanda | Emperrada / chave certa |
| Porta (com chave) | Destravar varanda | A porta cede |

## Locais

- Nota: `Interact_Note_OwnerBalcony` (201)  
- Chave: `Interact_BalconyKey` (recepção)  
- Porta: `Door_UpperBalcony`  
- Ala: `UpperBalconyWing` / `Room_203` / `Room_OwnerOffice`

## Checklist navegação

| Check | Resultado |
|-------|-----------|
| Porta verde bloqueia antes da chave | ☐ |
| Porta libera sem flicker/tremor | ☐ |
| Varanda sem queda/limbo | ☐ |
| Ala / 203 / office navegáveis | ☐ |
| Volta à Pensão OK | ☐ |

## Regressão

| Check | Resultado |
|-------|-----------|
| Movimento / HUD / áudio / fog | ☐ |
| Puzzle depósito/fusível | ☐ |
| Escada / 2º andar | ☐ |
| Sem inimigo/combat/chase | ☐ |

## Hotfix 17B — Acesso único + varanda real

**Problemas:**
- Porta marrom `Door_UpperBlocked` duplicava o vão e pedia outra chave.
- `Wall_Second_Front` sólida fechava a caixa atrás da porta verde (sem varanda).

**Correções:**
- Removida porta marrom / prompt *Tentar abrir porta* do vão da varanda.
- Porta verde única, largura total do vão.
- Gap em `Wall_Second_Front` + piso externo até `BalconyOuterZ` + guarda-corpos.
- Corredor curto `UpperBalconyWing_Corridor` com entrada limpa; stub trancado no fim.
- Trigger presença movido para Z≈−16,5 (longe da porta).

| Check | Resultado |
|-------|-----------|
| Uma só porta no vão | ☐ F6 |
| Destravar → varanda externa | ☐ F6 |
| Guarda-corpo / sem limbo | ☐ F6 |
| Prompt presença longe da porta | ☐ F6 |

---

## Sprint 17C — Ala da varanda + puzzle macabro

**Status:** 🔄 Implementada — aguardando F6  
**Docs:** `docs/design/PENSION_BALCONY_WING_PUZZLE.md`

### Rota completa

| Check | Resultado |
|-------|-----------|
| Nascer na trilha | ☐ |
| Entrar na pensão | ☐ |
| Fluxo chave → depósito → fusível | ☐ |
| Subir segundo andar | ☐ |
| Abrir porta verde com chave da varanda | ☐ |
| Acessar varanda + guarda-corpo / sem queda | ☐ |
| Voltar da varanda | ☐ |
| Pegar arame torto | ☐ |
| Entrar no banheiro | ☐ |
| Examinar espelho | ☐ |
| Ralo sem arame = mensagem correta | ☐ |
| Arame no ralo → chave | ☐ |
| Abrir quarto da proprietária | ☐ |
| Examinar caderno → evento macabro | ☐ |
| Mensagem Quarto 203 | ☐ |
| Quarto 203 bloqueado | ☐ |
| Sair e voltar OK | ☐ |

### Checklist técnico

| Check | Resultado |
|-------|-----------|
| Player / stamina / HUD / lanterna / F10–F11 | ☐ |
| Áudio / passos / respiração / fog | ☐ |
| Puzzle antigo + porta verde | ☐ |
| Dois cômodos acessíveis / sem limbo / sem z-fight novo | ☐ |
| Sem porta tremendo / sem inimigo / combate / chase | ☐ |

## Sprint 17D — hotfix de acesso e leitura

**Problemas confirmados no playtest do usuário:** os dois cômodos estavam inacessíveis, objetos ocupavam a circulação, prompts apareciam deslocados, o trigger de olhar para baixo estava na leitura errada e o teto da recepção precisava de revisão.

**Correções aplicadas:**
- vão lateral do guarda-corpo alinhado ao acesso real da ala;
- `UpperBalconyWing_EntryFloor` fecha a lacuna entre varanda e corredor;
- pia, cama, mesa e armário afastados dos eixos de entrada;
- removido `Door_Room203_Blocked`, que criava um terceiro prompt inacessível;
- área de `Interact_BalconyEdgeHint` reduzida e rebaixada junto à borda externa;
- `Door_OwnerBedroom` permanece único no vão e desativa painel, colisão e interação ao abrir;
- forro opaco `Ceiling_Reception_Liner` cobre as emendas visíveis acima da recepção.

### Checklist final 17D

| Check | Resultado |
|-------|-----------|
| Compilação C# sem erros/avisos | ✅ 2026-07-12 |
| Projeto carrega no Godot headless | ✅ 2026-07-12 |
| Porta verde → varanda → retorno | ☐ F6 usuário |
| Piso e guarda-corpos sem limbo | ☐ F6 usuário |
| Olhar para baixo somente na borda externa | ☐ F6 usuário |
| Entrar/sair do banheiro; espelho e ralo | ☐ F6 usuário |
| Destravar, entrar/sair do quarto; caderno | ☐ F6 usuário |
| Recepção mostra forro limpo ao olhar para cima | ☐ F6 usuário |
| Movimento/HUD/lanterna/áudio/fog/puzzle antigo preservados | ☐ F6 usuário |

Sprint 17C/17D foi substituída pelo rebuild controlado da Sprint 17E.

## Sprint 17E — rebuild cirúrgico da microárea

**Motivo:** a ala anterior acumulava piso de conexão, rails divididos e entradas desalinhadas. Ela foi substituída por um layout único e direto.

**Estrutura nova:** `BalconyDoor_Green` → `BalconyLanding` → `BalconyWalkable` → `Room_Bathroom` / `Room_OwnerBedroom`.

- uma única porta verde e uma única área de interação;
- patamar e varanda no mesmo nível, sem mureta interna;
- banheiro aberto com porta, pia, espelho e ralo fora da entrada;
- quarto com `Room_OwnerDoor`, cama, cômoda e caderno fora da circulação;
- interações antigas/fantasmas não são mais instanciadas;
- `Interact_BalconyLookDown` existe somente junto à borda externa;
- `Ceiling_Reception_Liner` mantém o teto do térreo visualmente fechado.

### Validação 17E

| Check | Resultado |
|-------|-----------|
| Compilação C# sem erros/avisos | ✅ 2026-07-12 |
| Cena oficial inicia e monta os puzzles | ✅ 2026-07-12 |
| Porta → patamar → varanda → retorno | ☐ F6 manual |
| Banheiro e quarto acessíveis | ☐ F6 manual |
| Prompts somente diante das portas | ☐ F6 manual |
| Olhar para baixo somente na borda | ☐ F6 manual |
| Teto da recepção limpo | ☐ F6 manual |
| Regressão completa da pensão | ☐ F6 manual |

### Correção orientada pelos prints do playtest

- removido da montagem o gerador legado `BuildUpperSouthRoomPlaceholder`, responsável pela pilastra/mureta sobre o acesso;
- `BalconyLanding` agora reutiliza o slab principal, sem placa coplanar;
- visual do piso do banheiro foi recortado para não sobrepor o piso principal;
- teto da entrada/recepção foi substituído por uma única peça `Ceiling_Reception_Continuous`, sem liner sobreposto;
- mensagem do arame agora orienta explicitamente o uso no ralo do banheiro;
- o ralo usa o arame para retirar a chave do quarto da proprietária.
- o arame foi reposicionado sobre a pia quebrada do banheiro, em altura visível e antes do ralo no fluxo do cômodo.

### Checklist emocional

| Check | Resultado |
|-------|-----------|
| Varanda = descoberta | ☐ |
| Banheiro = abandono / ralo desconfortável | ☐ |
| Quarto + caderno = impacto | ☐ |
| 203 prepara medo | ☐ |

