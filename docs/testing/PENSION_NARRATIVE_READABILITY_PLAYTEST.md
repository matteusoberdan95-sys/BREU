# Playtest — Leitura narrativa (Sprint 14)

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`  
**Sprint:** 14 — Portas, quartos e leitura narrativa  
**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_ROOM_READABILITY_BASELINE.md`

---

## Status

Implementado — validar F6 antes de aprovar.

---

## Rota testada

Trilha → entrada → recepção → quarto 102 → chave → cozinha → depósito → fusível → escada → 2º andar → quartos 201/202 → porta bloqueada → retorno

---

## Portas/molduras adicionadas

- `Door_MainEntrance_Frame` — varanda/entrada
- `Door_ReceptionSouth_Frame` / `Door_ReceptionCorridor_Frame`
- `Door_Room102_Frame` / `Door_Kitchen_Frame`
- `Door_Deposit_Frame` (+ `Door_Deposit_Blocked` puzzle)
- `Door_StairEntry_Frame`
- `Door_Room201_Frame` / `Door_Room202_Frame`
- `Door_UpperBlocked_Frame` / `Door_UpperBlocked_Locked`

---

## Interações adicionadas/atualizadas

| ID | Prompt |
|----|--------|
| reception_book | Ler livro de hóspedes |
| reception_keys | Examinar chaves |
| reception_counter | Examinar recepção |
| room_102_bed | Examinar cama |
| room_102_suitcase | Examinar mala |
| kitchen | Examinar cozinha |
| kitchen_stove | Examinar fogão |
| kitchen_cabinet | Examinar armário |
| deposit_shelf | Examinar prateleira |
| stair_inspect | Examinar escada |
| room_201 / room_201_note | Examinar quarto 201 / Ler anotação |
| room_202 / room_202_cabinet | Examinar quarto 202 / Examinar armário |
| room_203_locked | Tentar abrir porta |
| Puzzle preservado | Pegar chave / Usar chave / Pegar fusível / Ler bilhete |

---

## Checklist técnico

- [ ] Movimento / HUD / lanterna / F10 / F11
- [ ] Fog intacto (sem quadrados)
- [ ] Puzzle depósito completo
- [ ] Escada / 2º andar navegáveis
- [ ] Sem prompts através de parede
- [ ] Sem mensagens vazias
- [ ] Sem prender em móveis

---

## Checklist visual

- [ ] Recepção, 102, cozinha, depósito legíveis
- [ ] Portas abertas vs trancadas compreensíveis
- [ ] Escada e quartos superiores legíveis
- [ ] Ainda blockout, mais narrativa

---

## Sprint 14C — reconstrução limpa das portas

- [x] Compilação C# sem erros ou avisos.
- [x] Três prefabs de porta presentes em `scenes/props/doors/`.
- [x] Entrada, 102, cozinha, 201 e 202 sem painel bloqueador.
- [x] Depósito: destravar somente oculta painel e desativa colisão.
- [x] Porta verde: painel opaco único, colisão e interação local.
- [ ] F6: confirmar visualmente os vãos, a rota e o puzzle da chave.
- [ ] F6: confirmar HUD, lanterna, stamina, fog e escada.

Correções após revisão visual dos prints:

- [x] Fechamento de parede acima das molduras abertas.
- [x] Folhas abertas estáticas adicionadas sem colisão.
- [x] Folhas dos quartos esquerdos orientadas para fora do corredor.
- [x] Placa de oferta deixou de ser um bloco claro sem identificação.
- [x] Depósito expõe `Tentar abrir depósito` no nó raiz e mantém `InteractionArea` na camada 2.
- [x] Travessa da moldura não invade mais o cabeçalho da parede do depósito.
- [x] Placa de oferta redimensionada para conter todo o texto.
- [x] Parede leste da caixa da escada estendida sem fechar a saída do patamar.
- [x] Raycast não procura mais portas entre irmãos de uma parede atingida.
- [ ] F6: confirmar ausência do prompt `Tentar abrir porta` ao mirar piso e paredes comuns do segundo andar.

A aprovação visual permanece pendente até o playtest manual em F6.

## Regressão

- [ ] Atmosfera Sprint 13 preservada
- [ ] Estrutura S05–12A preservada

---

## Sprint 14A — Door readability and deposit door fix

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md`

### Correções

- **Depósito:** `Door_Deposit` reestruturado (Frame / Panel / Collision / Interact). Destravar = painel some + colisão desativa — sem scale/animação.
- **Molduras térreo:** entrada, quarto 102, cozinha, depósito — leitura Tipo A/B/C.
- **2º andar:** quartos 201/202 com moldura + folha aberta.
- **Porta verde → varanda:** `Door_UpperBalcony_Locked` — prompt *Tentar abrir varanda*; trancada.
- **Varanda placeholder:** `UpperBalcony_Placeholder` + guarda-corpos; piso fechado; sem acesso jogável.

### Portas/molduras (14A)

| Nó | Tipo |
|----|------|
| `Door_MainEntrance_Frame` | A — aberta |
| `Door_Room102_Frame` | A — aberta |
| `Door_Kitchen_Frame` | A — aberta |
| `Door_Deposit_*` | C — destravável |
| `Door_Room201_Frame` / `Door_Room202_Frame` | A — aberta |
| `Door_UpperBalcony_Locked` | B — varanda trancada |

### Checklist 14A

**Portas**
- [ ] Entrada / 102 / cozinha — moldura clara
- [ ] Depósito — porta clara; **não** estica/treme/canto da parede
- [ ] Depósito destranca com chave; passagem livre depois
- [ ] 201 / 202 — moldura clara
- [ ] Porta verde — prompt varanda; não abre; não atravessável

**Varanda**
- [ ] Frontal superior = varanda placeholder (piso + guarda-corpo)
- [ ] Não parece buraco; player não acessa

**Regressão**
- [ ] Movimento / HUD / lanterna / F10 / F11
- [ ] Fog intacto
- [ ] Puzzle chave → depósito → fusível
- [ ] Escada / 2º andar
- [ ] Prompts não atravessam parede

---

## Sprint 14B — Door system fix and balcony readability

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.1

### Removido (sistema quebrado 14/14A)

- Todas as folhas `Door_*_Leaf` (painéis flutuantes no vão)
- `AddOpenDoorLeafXWall` / `AddOpenDoorLeafZWall`
- Portas trancadas via `AddInteractableBody` + `_matInteractable` (verde translúcido)
- Molduras duplicadas no 2º andar (`AddSecondFloorDoorFrameIn*`)
- Moldura duplicada `Door_UpperBalcony_Frame` no placeholder
- Interação do depósito no `StaticBody3D` (agora em `Door_Deposit_InteractArea`)

### Padrão estável 14B

| Tipo | Implementação |
|------|---------------|
| A — aberta | Somente moldura, sem painel, sem colisão |
| B — trancada | `AddLockedDoorPanelZWall` — painel opaco + colisão WorldLayer + Area3D |
| C — depósito | Painel opaco + colisão; destravar = hide panel + disable collision |

### Varanda

- Interior: `UpperBalcony_Placeholder` (piso, guarda-corpos, parede fundo)
- Trilha: `UpperBalcony_TrailReadability` (piso saliente, guarda-corpo, painel verde)

### Checklist 14B

**Entrada**
- [ ] Sem porta grande bugada; só moldura; atravessável

**Térreo**
- [ ] 102 / cozinha — moldura, passagem livre
- [ ] Depósito — painel opaco bloqueia; destranca sem tremer/esticar
- [ ] Prompt depósito localizado na porta

**2º andar**
- [ ] 201 / 202 — moldura apenas
- [ ] `Door_UpperBalcony_Locked` — verde opaco, bloqueia, prompt varanda

**Varanda**
- [ ] Vista da trilha comunica varanda bloqueada
- [ ] Não parece buraco; sem acesso jogável

**Regressão**
- [ ] HUD / lanterna / F10 / F11 / fog / movimento / escada / puzzle intactos

---

## Sprint 14E — Final door readability hotfix

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.2

### Correções

- **Placa:** `Sign_PensaoSantaLuzia` alinhada na fachada (rotação 0°, offset 0,055 m); `JobOfferSign` redimensionada e centralizada.
- **Porta verde:** única instância `Door_UpperBalcony_Locked` no vão interior (não coplanar na fachada); `ConfigureLockedDoor` com moldura + painel a -0,08 m.
- **Corredor inútil:** `UpperStair_BackClosureWall`, `UpperLanding_BackSeal`, `UpperStair_NorthEastSeal` fecham pocket atrás da escada.
- **Z-fighting:** molduras afastadas 0,05 m; folhas decorativas +0,12 m; painéis fechados a -0,08 m.

### Checklist 14E

**Fachada**
- [ ] Placa PENSÃO SANTA LUZIA legível e estável
- [ ] Varanda na trilha sem painel verde duplicado colado

**Portas**
- [ ] Porta verde parece porta real no vão com moldura
- [ ] Nenhuma moldura/folha pisca nos vãos principais
- [ ] Depósito continua funcionando

**2º andar**
- [ ] Corredor inútil atrás da escada fechado
- [ ] Escada e patamar continuam navegáveis

**Regressão**
- [ ] Atmosfera / fog / HUD / puzzle / movimento intactos

---

## Sprint 14F — Definitive sign and door cleanup

**Data:** 2026-07-11  
**Baseline:** `docs/technical/PENSION_DOOR_BLOCKOUT_BASELINE.md` v1.3

### Removido (não mais gerado em runtime)

- `Sign_PensaoSantaLuzia` dentro do interior (clipava a entrada)
- `JobOfferSign` como mesh 3D com texto (virou só `Area3D` na trilha)
- Prefab `DoorFrameOpen` com `UpperWallInfill`, folhas e painéis decorativos
- Portas abertas extras: recepção sul/corredor, escada (`Door_Reception*`, `Door_StairEntry`)
- `ConfigureOpenDoor` / duplicatas de moldura por prefab

### Padrão mínimo 14F

| Elemento | Implementação |
|----------|----------------|
| Placa | Única: `Sign_Pensao_Main_Exterior` no nó `Exterior`, fora da fachada |
| Porta aberta | `AddMinimalDoorFrame*` — 3 peças opacas, offset 0,06 m |
| Porta trancada | Prefab + `FinalizeLockedDoor` — painel nomeado `*_Panel` |
| Depósito | `Door_Deposit_Panel` |
| Varanda | `Door_UpperBalcony_Panel` |

### Checklist 14F

**Entrada**
- [ ] Uma placa externa apenas; nada dentro do vão
- [ ] Sem painel horizontal / infill / folha na entrada
- [ ] Passagem livre; sem flicker

**Portas**
- [ ] 102 / cozinha / 201 / 202 = só moldura
- [ ] Depósito e varanda = painel opaco único cada
- [ ] Nenhuma moldura pisca

**2º andar**
- [ ] Corredor inútil permanece fechado (14E)

**Regressão**
- [ ] Puzzle / escada / atmosfera intactos
