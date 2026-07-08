# BREU - Plano de implementacao

## Continuidade

Este plano mostra a direcao macro. Para saber o ponto exato em que paramos, leia `docs/PROJECT_STATE.md` e `docs/HANDOFF.md`.

## Fase 1 - Fundacao jogavel

- Criar projeto Godot 4 .NET com C# e TargetFramework net10.0.
- Definir estrutura de pastas para cenas, scripts, resources, assets e docs.
- Implementar Player em primeira pessoa com movimento, camera, mouse look e captura de mouse.
- Implementar stamina basica para corrida e combate.
- Implementar lanterna com bateria, toggle e sinal para HUD.
- Implementar interacao por RayCast e interface `IInteractable`.
- Implementar HUD minimo com stamina, bateria, arma e prompt.

## Fase 2 - Quarto 07

- Criar `DemoRoom.tscn` como cena principal.
- Usar placeholders de cubos/cilindros para quarto, cama, mesa, janela, manchas, bilhete e martelo.
- Instanciar porta e corredor curto.
- Preparar escala 1 unidade = 1 metro para receber assets `.glb`.
- Registrar identidade do lugar: Quarto 07 - Pensao Santa Luzia.

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
