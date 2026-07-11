# Playtest — Integração da escada na Pensão

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 09A — Integrar escada no térreo — **APROVADA**  
**Data aprovação:** 2026-07-11  
**Baseline escada:** `docs/technical/STAIR_RAMP_BASELINE.md`

---

## Status

**Escada integrada na Pensão aprovada pelo usuário.**  
Patamar superior é **temporário** — segundo andar completo será sprint futura.

---

## Checklist final aprovado

- [x] Player encontra a escada dentro da Pensão
- [x] Player sobe a escada sem travar
- [x] Player desce a escada sem quicar
- [x] Player chega na plataforma superior temporária
- [x] Player anda na plataforma superior
- [x] Player não cai da plataforma superior
- [x] Degraus visuais não prendem o player
- [x] Rampa invisível é a colisão principal

---

## Checklist — Regressão térreo / puzzle

- [x] Player nasce na trilha
- [x] Player entra na pensão
- [x] Player acessa recepção
- [x] Player acessa corredor
- [x] Player acessa quarto 102
- [x] Player pega a chave
- [x] Player acessa cozinha
- [x] Player destranca depósito
- [x] Player pega fusível
- [x] Interações continuam funcionando
- [x] HUD continua funcionando
- [x] Movimento continua aprovado
- [x] Player não cai do mapa
- [x] Player não atravessa paredes principais

---

## Checklist — Escada na Pensão

- [x] Entrada da escada é clara (álcove oeste, corredor z ≈ -25,5)
- [x] Player entra na escada sem travar
- [x] Player sobe até o topo
- [x] Player não quica nos degraus
- [x] Player não fica preso na rampa
- [x] Player não atravessa lateral da escada
- [x] Player volta ao térreo descendo
- [x] Player não quica descendo

---

## Regressão — HUD / movimentação

- [x] PlayerController não alterado
- [x] PlayerCameraFeel não alterado
- [x] HUD e debug F10/F11 funcionando

---

## Critério principal

**Aprovado:** subir e descer na Pensão tão suave quanto no `StairMovementLab`.

---

## Não testar nesta sprint

- Segundo andar completo
- Teto
- Inimigo / combate
- Arte final / GLB
