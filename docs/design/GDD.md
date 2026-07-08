# BREU - Game Design Document inicial

## Visao

BREU e um survival horror em primeira pessoa ambientado em uma pensao antiga, decadente e opressiva no interior do Nordeste brasileiro, entre os anos 70 e 80. O jogador nao e totalmente indefeso, mas cada reacao deve parecer desesperada, limitada e custosa.

## Pilar de experiencia

- Medo de perseguicao em espacos pequenos.
- Combate brutal, pesado e arriscado.
- Recursos instaveis: stamina, lanterna e armas quebraveis.
- Narrativa ambiental brasileira, suja, religiosa e estranha.
- Lugar especifico em vez de terror generico.

## Primeira demo

Nome do cenario: Quarto 07 - Pensao Santa Luzia.

O jogador acorda em um quarto mofado, com cama de ferro, colchao imundo, ventilador quebrado, janela gradeada, crucifixo torto, sangue seco, marcas na parede, radio chiando, bilhete antigo e um martelo enferrujado perto da cama. A meta e sair do quarto, atravessar um corredor curto e sobreviver ao primeiro contato com `Enemy_Hospede`.

## Loop da vertical slice

1. Acordar no quarto.
2. Olhar, andar e usar lanterna.
3. Ler bilhete.
4. Pegar martelo.
5. Abrir porta.
6. Entrar no corredor.
7. Encontrar `Enemy_Hospede`.
8. Atacar com martelo ate a durabilidade acabar.
9. Continuar no soco.
10. Chegar a uma porta bloqueada/fim de demo.

## Combate

O jogador pode reagir, mas nao deve dominar a cena. Ataques custam stamina, armas quebram e o alcance e curto. O soco existe como fallback, nao como solucao forte.

## Inimigo inicial

`Enemy_Hospede` e um antigo morador deformado e agressivo. Na primeira versao, ele detecta o jogador por distancia, persegue, ataca, recebe dano, fica atordoado e morre.

## Lore inicial

No fim dos anos 70, a Pensao Santa Luzia ficou conhecida por desaparecimentos. Decadas depois, o protagonista acorda no Quarto 07 sem lembrar como chegou ali. O lugar parece abandonado, mas ainda ha passos, vozes atras das paredes e sinais de que alguem continua preparando os quartos.

Elementos sugeridos:

- Culto familiar.
- Violencia antiga.
- Entidade ligada ao escuro.
- Desaparecimentos.
- Figuras: O Homem do Candeeiro, A Mae do Breu.
