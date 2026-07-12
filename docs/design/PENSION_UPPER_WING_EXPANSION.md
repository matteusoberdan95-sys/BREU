# Sprint 18 — expansão da ala superior

## Organização

- `BalconyWing.tscn`: varanda estreita, banheiro e quarto da proprietária, agora com cobertura simples.
- corredor superior: destino 203 na parede esquerda; rouparia e Quarto 204 na parede direita.
- guarda-corpos permanecem apenas nas bordas externas; nenhum novo blocker foi colocado na circulação.

## Novos pontos

- `Room_LinenCloset`: nicho de rouparia com lençóis empilhados e leitura narrativa variável após o aviso 203.
- `Door_Room204_Blocked`: porta ambiental que permanece fechada e ganha mensagem de respiração após o aviso.
- `Door_Room203_Blocked`: destino final atual, número reorientado e porta sempre bloqueada.

## Event_Room203_FirstWarning

Na primeira tentativa após ler o caderno:

1. mensagem de bloqueio;
2. `door_scratch_01`;
3. `distant_step_01`;
4. flicker curto apenas na luz do corredor;
5. “Eu não estou sozinho aqui.”

O evento é único. Não abre a porta, não mostra inimigo, não inicia chase e não causa dano.
