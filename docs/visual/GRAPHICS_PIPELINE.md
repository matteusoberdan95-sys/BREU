# Pipeline Grafico - BREU DE DENTRO

## Fluxo geral

1. Blockout no Blender.
2. Export `.glb`.
3. Import no Godot.
4. Colisao simples no Godot.
5. Materiais base.
6. Iluminacao.
7. Pos-processamento.
8. Pass de sujeira/desgaste.
9. Props narrativos.
10. Otimizacao.
11. Teste em gameplay.

## Regra principal

Blender cria:

- forma;
- escala;
- silhueta;
- props;
- modularidade;
- UV/textura no futuro;
- origem e organizacao dos objetos.

Godot controla:

- iluminacao final;
- sombras;
- neblina;
- pos-processamento;
- colisoes;
- interacoes;
- triggers;
- audio;
- gameplay.

## Nao fazer

- Nao depender da luz do Blender para o visual final.
- Nao exportar camera/luz do Blender como solucao final.
- Nao juntar tudo cedo demais.
- Nao criar modelos finais antes de validar gameplay.
- Nao deixar material sem nome.
- Nao usar textura pesada sem necessidade.
- Nao fazer cenario inteiro como uma peca so.

## Fazer

- Criar assets modulares.
- Criar 2 a 4 variacoes de props repetidos.
- Usar instancias/copias.
- Nomear objetos corretamente.
- Separar collections.
- Usar materiais com prefixo `MAT_`.
- Testar tudo no Godot rapidamente.

# Performance visual

- Usar blockout simples durante gameplay.
- So detalhar apos validar.
- Evitar excesso de luz com sombra.
- Nao usar muitos `OmniLight3D` com shadow ao mesmo tempo.
- Usar neblina com cuidado.
- Texturas em resolucao moderada.
- Props repetidos devem usar instancing quando possivel.
- Manter cenas divididas por area.
- Preparar streaming/areas conectadas no futuro.

## Checkpoint antes de cenas

Antes de alterar cenas, verificar estado atual do projeto e recomendar commit caso existam mudancas nao salvas. Pass visual mexe em leitura, iluminacao e atmosfera; checkpoint evita perder uma versao jogavel boa.
