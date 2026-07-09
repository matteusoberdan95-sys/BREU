# BREU - Plano de implementacao

## Continuidade

Este plano mostra a direcao macro. Para saber o ponto exato em que paramos, leia `docs/PROJECT_STATE.md` e `docs/HANDOFF.md`.

## Status atual

A primeira base jogavel do Quarto 07 ja existe. O jogador anda em primeira pessoa, usa lanterna, ve HUD minimo, le o bilhete, coleta o martelo, ve o martelo placeholder na mao, abre a porta e entra em um corredor placeholder. Combate e inimigo continuam fora da demo atual.

## Fase 1 - Fundacao jogavel

- [x] Criar projeto Godot 4 .NET com C# e TargetFramework net10.0.
- [x] Definir estrutura de pastas para cenas, scripts, resources, assets e docs.
- [x] Implementar Player em primeira pessoa com movimento, camera, mouse look e captura de mouse.
- [x] Implementar lanterna com bateria e toggle.
- [x] Implementar interacao por RayCast e interface `IInteractable`.
- [x] Implementar HUD minimo com prompt e arma equipada.
- [ ] Integrar stamina ao loop completo de corrida/combate quando combate entrar.

## Fase 2 - Quarto 07

- [x] Criar `DemoRoom.tscn` como cena principal.
- [x] Instanciar o blockout `.glb` exportado do Blender.
- [x] Criar colisoes auxiliares para quarto e moveis.
- [x] Criar bilhete interativo.
- [x] Criar martelo coletavel.
- [x] Criar porta interativa em modo debug.
- [x] Instanciar corredor placeholder.
- [ ] Substituir corredor placeholder por cena modular definitiva.
- [ ] Criar porta final/transicao.
- [ ] Adicionar feedback visual/sonoro para interacoes.

## Fase 3 - Combate

- Criar `WeaponData` como Resource.
- Criar resources iniciais: soco, martelo enferrujado, faca velha e tabua rachada.
- Criar `WeaponController` com ataque leve, custo de stamina, durabilidade e quebra.
- Usar fallback automatico para soco quando a arma quebra.
- Manter TODOs para animacao, audio, impacto visual e camera shake.

## Fase 4 - Inimigo

- Criar `Enemy_Hospede.tscn`.
- Implementar estados basicos: Idle, Chase, Attack, Stunned, Dead.
- Perseguir player por raio de deteccao.
- Receber dano, stun e morte.
- Registrar ataques por debug enquanto o sistema de vida do player nao existe.

## Fase 5 - Atmosfera e polimento

- Substituir placeholders por assets do Blender.
- Adicionar audio ambiente, radio chiando, rangidos, passos e feedback de combate.
- Criar decals reais para mofo, sangue seco, marcas de unha e sujeira.
- Ajustar luzes, sombras, bateria da lanterna e ritmo de susto.
- Criar checklist de QA manual para fluxo completo da demo.
