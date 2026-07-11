# BREU — Handoff

**Última atualização:** 2026-07-11  
**Status:** REBOOT GREENFIELD — Sprint 02 concluída  
**Branch:** `reboot/breu-clean-start`

---

## Retomar em 60 segundos

1. `docs/PROJECT_STATE.md`
2. `docs/testing/PLAYER_MOVEMENT_LAB_PLAYTEST.md`
3. `docs/production/SPRINT_ROADMAP.md` — Sprint 03
4. `docs/production/SPRINT_COMMIT_POLICY.md` — commit/push por sprint

---

## Como testar movimento

```bash
cd BREU
dotnet build
```

Godot 4.7 mono → abrir `scenes/test/PlayerMovementLab.tscn` → **F6**.

| Tecla | Ação |
|-------|------|
| WASD | Mover |
| Mouse | Olhar |
| Shift | Sprint |
| C / Ctrl | Agachar |
| F | Lanterna |
| Esc | Liberar mouse |

---

## Estado do disco

- Player FPS recriado (`scenes/player/Player.tscn`)
- Lab de movimento (`scenes/test/PlayerMovementLab.tscn`)
- Sem pensão, HUD, combate, inimigos
- Política de commit automático por sprint documentada

---

## Bugs conhecidos

Nenhum bloqueante. Playtest manual F6 recomendado para validar feel.

---

## Próxima ação

**Sprint 03 — HUD e Debug**

- HUD: vida, stamina, lanterna (display)
- `PlaytestDebugSettings`: lanterna infinita, fog reduzida
- **Não** implementar pensão ainda

---

## Regra de produção

> Primeiro jogável, depois bonito.  
> Térreo → escada → 2º andar → teto → atmosfera → inimigo → Blender.
