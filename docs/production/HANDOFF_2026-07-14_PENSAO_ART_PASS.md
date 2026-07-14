# Handoff — Pensão Santa Luzia — 2026-07-14

## Objetivo deste checkpoint

Permitir que outra pessoa abra o BREU em outro computador e continue exatamente do ponto atual: props Blender principais importados, térreo com materiais brasileiros degradados, entrada corrigida e transição da escada para o segundo andar com dois perfis visuais distintos.

Este é um checkpoint de continuidade autorizado pelo usuário. O build está aprovado, mas o passe visual mais recente ainda precisa de F9 e playtest manual no Godot.

## Cena e arquivos principais

- Cena oficial: `scenes/levels/pensao_santa_luzia/PensaoVerticalBlockout01.tscn`
- Passe PBR: `scripts/levels/pensao_santa_luzia/Sprint31CPbrMaterialPass.cs`
- Cena do passe PBR: `scenes/levels/pensao_santa_luzia/visual/Sprint31C_PBRMaterials.tscn`
- Props Blender: `scripts/levels/pensao_santa_luzia/Sprint30BBlenderProps.cs`
- Art pass leve/dressing: `scripts/levels/pensao_santa_luzia/Sprint28LightArtPass.cs`
- Builder do térreo/entrada: `scripts/levels/pensao_santa_luzia/PensaoTerreoBlockout01Builder.cs`
- Auditoria: `scripts/debug/LevelSanityChecker.cs`
- Materiais: `assets/materials/pensao/`
- Texturas: `assets/textures/pensao/`
- Checklist manual: `docs/testing/PENSION_UPPER_WING_PLAYTEST.md`

## Estado visual atual

### Térreo

- Reboco brasileiro mofado e contínuo, sem a antiga linha horizontal.
- Assoalho escuro de tábuas largas, gasto e sem aparência de laminado novo.
- Teto de reboco infiltrado, separado visualmente do piso superior.
- Fachada frontal, soleiras e depósito integrados ao passe.
- Entrada e recepção retificadas; paredes pontudas/residuais removidas.
- Barrancos externos recuados para não atravessarem a parede.
- Salão de entrada recebeu móveis, quadros, caixas, papéis, manchas e lixo visual sem bloqueio novo.

### Escada

- Perfil dedicado mais escuro, úmido e esverdeado.
- Somente paredes, piso visual do poço e teto de transição recebem o perfil.
- Degraus, rampa física, corrimãos e colisões permanecem intocados.

### Chegada ao segundo andar

- Perfil dedicado mais seco, frio e desbotado.
- Paredes do patamar, piso visual e teto recebem variação própria.
- `UpperWing_CollisionDeck` permanece congelado e não recebe alteração física.

## Decisões importantes

- Não gerar uma família completamente nova para a escada/segundo andar antes do playtest.
- Reutilizar a família PBR atual com variações locais mantém coerência e evita repetição direta.
- Novas texturas devem ser hero/localizadas, somente se a repetição continuar visível.
- Decals-caixa antigos permanecem desativados porque apareciam como placas e travessas.
- Não recriar sujeira com BoxMesh fino; usar `Decal3D`, transparência feathered ou malha conformada numa etapa futura.

## O que não pode ser alterado

- `UpperWing_CollisionDeck` e navegação aprovada da varanda.
- Escada física, rampa, degraus e corrimãos aprovados.
- Paredes estruturais, portas, puzzles, IA, perseguições, safe zones e triggers.
- Painel técnico, ralo, fusíveis, porta e evento do Quarto 203.

## Como retomar em outro computador

1. Usar Godot Mono 4.7 e abrir a raiz do projeto.
2. Aguardar a importação completa de texturas e assets Blender.
3. Executar `dotnet build`.
4. Abrir a cena oficial e rodar F9.
5. Percorrer: entrada → recepção → térreo → escada → patamar superior → corredor → varanda → retorno ao térreo.
6. Preencher `docs/testing/PENSION_UPPER_WING_PLAYTEST.md`.

## Validação registrada neste checkpoint

- `dotnet build`: 0 erros / 0 avisos em 2026-07-14.
- Geometria física da escada e deck excluídos explicitamente do passe de materiais.
- Sanity checker atualizado para exigir aplicação dos dois perfis visuais.
- F9 e playtest manual mais recentes: pendentes após reiniciar a cena com a nova DLL.

## Próximos passos

1. Validar a transição visual escada/patamar e a legibilidade da lanterna.
2. Corrigir somente alvos identificados por print e coordenada, sem cleanup amplo.
3. Se aprovado, avançar para texturas hero/Blender: reboco inchado, tábuas quebradas e UVs dedicadas.
4. Depois, revisar telhado/fachada como tarefa separada e estruturalmente planejada; não misturar com este passe.

