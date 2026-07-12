# Playtest — Sprint 18

**Cena:** `PensaoVerticalBlockout01.tscn`  
**Status:** compilação e carga aprovadas; percurso F6 visual pendente.

## Rota

- [ ] fluxo depósito/fusível preservado
- [ ] porta verde e varanda acessíveis
- [ ] arame → ralo → chave preservado
- [ ] quarto da proprietária e caderno preservados
- [ ] número 203 legível e não espelhado
- [ ] 203 permanece bloqueado
- [ ] arranhão → passos → flicker → mensagem final
- [ ] evento não repete integralmente
- [ ] rouparia acessível e prompt próximo
- [ ] Quarto 204 visível, próximo e bloqueado
- [ ] guarda-corpo não bloqueia circulação
- [ ] olhar para baixo somente na borda
- [ ] sem z-fighting, limbo ou prompt através de parede

## Prioridade: circulação sem mureta

- [x] mesh da mureta lateral interna removido
- [x] collider correspondente removido
- [x] parede sul direita do corredor aberta para a extensão
- [ ] confirmar em F6 que não há contato invisível na antiga barreira
- [ ] atravessar corredor B sem pular ou contornar
- [ ] entrar e sair do Quarto 204
- [ ] entrar e sair do banheiro B
- [ ] entrar e sair da rouparia
- [ ] confirmar props fora dos eixos de entrada
- [ ] confirmar fluxo anterior até o 203 intacto

## Regressão

- [ ] movimento/stamina/crouch
- [ ] HUD/lanterna/F10/F11
- [ ] áudio/passos/respiração
- [ ] fog/WorldEnvironment
- [ ] escada e térreo
- [ ] sem inimigo, combate, dano ou chase
