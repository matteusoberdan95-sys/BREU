# Playtest — Integração da escada na Pensão

**Cena:** `res://scenes/levels/pensao_santa_luzia/PensaoTerreoBlockout01.tscn`  
**Sprint:** 09A — Integrar escada no térreo  
**Data:** 2026-07-11  
**Baseline escada:** `docs/technical/STAIR_RAMP_BASELINE.md`

---

## Status

**Escada integrada no térreo.** Validar com F6 antes de marcar sprint como aprovada.

---

## Checklist — Regressão térreo / puzzle

- [ ] Player nasce na trilha
- [ ] Player entra na pensão
- [ ] Player acessa recepção
- [ ] Player acessa corredor
- [ ] Player acessa quarto 102
- [ ] Player pega a chave
- [ ] Player acessa cozinha
- [ ] Player destranca depósito
- [ ] Player pega fusível
- [ ] Interações continuam funcionando
- [ ] HUD continua funcionando
- [ ] Movimento continua aprovado
- [ ] Player não cai do mapa
- [ ] Player não atravessa paredes principais

---

## Checklist — Escada na Pensão

- [ ] Player encontra a escada (álcove oeste, corredor z ≈ -25,5)
- [ ] Entrada da escada é clara
- [ ] Player entra na escada sem travar
- [ ] Player sobe até o topo
- [ ] Player não quica nos degraus
- [ ] Player não fica preso na rampa
- [ ] Player não atravessa lateral da escada
- [ ] Player chega na plataforma superior temporária
- [ ] Player anda na plataforma superior
- [ ] Player não cai da plataforma superior
- [ ] Player desce a escada
- [ ] Player não quica descendo
- [ ] Player volta ao térreo
- [ ] Degraus visuais não têm colisão ruim
- [ ] Rampa invisível é a colisão principal

---

## Critério principal

Subir e descer na Pensão deve ser **tão suave quanto** no `StairMovementLab`.

---

## Não testar nesta sprint

- Segundo andar completo
- Teto
- Inimigo / combate
- Arte final / GLB
