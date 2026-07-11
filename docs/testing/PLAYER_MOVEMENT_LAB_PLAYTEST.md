# Playtest — Player Movement Lab

**Cena:** `res://scenes/test/PlayerMovementLab.tscn`  
**Sprint:** 02  
**Data:** 2026-07-11

---

## Como testar

1. Abrir projeto no Godot 4.7 mono.
2. Abrir `scenes/test/PlayerMovementLab.tscn`.
3. Pressionar **F6** (Play Scene) ou **F5** (main scene bootstrap — depois F6 no lab).
4. Clicar na viewport para capturar mouse.

---

## Controles

| Ação | Tecla |
|------|-------|
| Andar | WASD |
| Olhar | Mouse |
| Sprint | Shift |
| Agachar | C ou Ctrl |
| Lanterna | F |
| Liberar mouse | Esc |
| Recapturar mouse | Clique esquerdo |

---

## Checklist obrigatório

- [ ] Projeto abre sem erro.
- [ ] Player nasce no chão.
- [ ] WASD funciona.
- [ ] Mouse look funciona.
- [ ] Câmera não gira infinito verticalmente.
- [ ] Shift corre.
- [ ] Stamina consome.
- [ ] Stamina regenera.
- [ ] Sem stamina, sprint não funciona.
- [ ] Ctrl/C agacha.
- [ ] Player passa por porta larga (x ≈ -6).
- [ ] Player não atravessa parede.
- [ ] Player sobe rampa simples (lado direito).
- [ ] Player não treme em colisão.
- [ ] Player não escorrega de forma estranha.
- [ ] Lanterna base (F) não quebra nada.
- [ ] Cena `PlayerMovementLab` roda no F6.
- [ ] Agachar passa pelo túnel baixo (centro-esquerda).

---

## Geometria do lab

| Elemento | Propósito |
|----------|-----------|
| Chão 28×22 m | Navegação base |
| Porta larga (~2,2 m) | Passagem em pé |
| Porta estreita (~0,9 m) | Passagem apertada |
| Rampa inclinada | Teste de subida |
| Túnel baixo (teto ~1,32 m) | Teste de agachar |

---

## Métricas do player (Sprint 02)

| Parâmetro | Valor |
|-----------|-------|
| WalkSpeed | 4.0 |
| SprintSpeed | 6.5 |
| CrouchSpeed | 2.0 |
| Acceleration | 12.0 |
| Deceleration | 14.0 |
| MaxStamina | 100 |
| SprintDrainPerSecond | 18 |
| StaminaRegenPerSecond | 14 |
| RegenDelay | 0.75 s |
| CameraHeightStanding | 1.65 |
| CameraHeightCrouching | 1.05 |

---

## Bugs conhecidos

Nenhum bloqueante identificado na validação automatizada (build + headless).

Playtest manual F6 recomendado para confirmar feel.

---

## Próximo passo

Sprint 03 — HUD mínimo (vida, stamina, lanterna display) + flags de debug.
