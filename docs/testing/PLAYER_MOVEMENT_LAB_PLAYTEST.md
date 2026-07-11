# Playtest — Player Movement Lab

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Sprint:** 02 (corrigida)  
**Data:** 2026-07-11

---

## Falha inicial (Sprint 02 v1)

**Sintomas:** tela escura, player tremendo, travado, sem movimento.

**Causa raiz:**
1. `PlayerMovementLabBuilder` gerava túnel baixo (teto ~1,32 m) e paredes perimetrais.
2. Spawn em `(0, 0.05, 8)` ficava **dentro** do túnel — cápsula colidia com teto lateral o tempo todo.
3. `WorldEnvironment` escuro + lanterna obrigatória = tela quase preta.
4. Direção de movimento usava `+Basis.Z` para frente (invertido em Godot; frente = `-Z`).

**Correção aplicada:**
- Cena estática mínima (chão + luz direcional clara).
- Spawn em `PlayerSpawn` `(0, 0, 0)` — pés no chão (origem = base do personagem).
- Capsule corrigida: radius 0.35, center Y 0.9, Head Y 1.65.
- Movimento: `Basis * (input.X, 0, -input.Y)`.
- Reset com **R** → `PlayerSpawn`.
- Lanterna desligada por padrão no lab (luz ambiente suficiente).
- Removido `PlayerMovementLabBuilder.cs`.

---

## Como testar

1. Godot 4.7 mono → `scenes/test/PlayerMovementLab.tscn` → **F6**.
2. Clicar na viewport (capturar mouse).

| Tecla | Ação |
|-------|------|
| WASD | Mover |
| Mouse | Olhar |
| Shift | Sprint |
| C / Ctrl | Agachar |
| F | Lanterna (opcional) |
| Esc | Liberar mouse |
| R | Reset para spawn |

Debug: **Debug → Visible Collision Shapes** — cápsula acima do chão, câmera na cabeça.

---

## Teste 1 — Chão plano

Geometria: chão 20×20 m em Y=0, spawn centro.

- [x] `IsOnFloor=true` após spawn (validado headless)
- [x] Câmera em Y=1.65, não dentro do chão
- [ ] Tela não preta (playtest manual)
- [ ] WASD + mouse (playtest manual)
- [ ] Sem tremor (playtest manual)
- [ ] R reseta (playtest manual)

---

## Teste 2 — Parede simples

Parede em **Z = -5**, 6 m × 3 m × 0,3 m — longe do spawn.

- [ ] Player colide, não atravessa (playtest manual)
- [ ] Sem tremor na parede (playtest manual)

---

## Teste 3 — Rampa simples

Rampa em **X=6, Z=4**, inclinada ~12°.

- [ ] Player sobe sem quicar/travar (playtest manual)

---

## Métricas do player

| Parâmetro | Valor |
|-----------|-------|
| Capsule radius | 0.35 |
| Capsule height | 1.8 |
| CollisionShape Y | 0.9 |
| Head Y | 1.65 |
| Camera near | 0.05 |
| WalkSpeed | 4.0 |
| SprintSpeed | 6.5 |

---

## Próximo passo

Sprint 02 só concluída após playtest manual dos 3 testes. Sprint 03 bloqueada até lá.
