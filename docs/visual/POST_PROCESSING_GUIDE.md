# Pos-processamento - BREU DE DENTRO

Objetivo: dar aparencia cinematografica, escura e suja, sem destruir visibilidade.

## Recomendacoes iniciais no Godot Environment

- Tonemap: Filmic ou equivalente disponivel no projeto.
- Exposure: levemente baixo.
- Contrast: moderado.
- Saturation: levemente reduzida.
- SSAO: ligado se disponivel e performatico.
- SSIL: testar se disponivel.
- Glow/Bloom: muito sutil, apenas em velas/lampioes.
- Fog: leve em exterior e corredor.
- Volumetric Fog: usar com cuidado.
- Vignette: implementar futuramente via shader/UI se necessario.
- Color grading: futuro LUT ou ajuste global.

## Configuracoes-alvo

- Visual escuro.
- Sombras profundas.
- Luz quente destacada.
- Sem exagerar bloom.
- Sem deixar tudo cinza.
- Sem estourar pretos a ponto de esconder gameplay.

## Regra de seguranca

Qualquer perfil visual deve ser reversivel e moderado. Se uma cena perder leitura de porta, item, inimigo ou caminho principal, o ajuste visual deve ser reduzido antes de continuar.
