# BREU — Protocolo de Playtest

**Versão:** 1.0  
**Data:** 2026-07-11  

---

## 1. Quando playtestar

- **Obrigatório** no fim de toda sprint com gameplay ou level (S02+).
- **Obrigatório** antes de declarar térreo/escada/2º andar “aprovado”.
- QA Agent pode reprovar sprint sem registro de teste.

---

## 2. Ambiente de teste

| Item | Recomendação |
|------|--------------|
| Godot | 4.7 stable mono (mesma versão do time) |
| Modo | F6 (cena atual) ou F5 (main scene) |
| Debug | `DebugInfiniteLantern = true`, fog reduzida |
| Gravação | Vídeo 30–90 s para sprints de level |

---

## 3. Como testar o player

1. Spawn: player no chão, não flutuando, não enterrado.
2. Olhar 360° horizontal + cima/baixo — sem clipping em geometria.
3. Andar 30 s em todas direções — sem travar em invisible wall.
4. Sprint (se existir) — stamina responde no HUD.
5. Agachar (se existir) — passa em porta 1,4 m; não trava no teto (quando houver).
6. Pular (se existir) — não atravessa teto nem sai do mapa.

**Falha crítica:** capsule presa, queda infinita, câmera dentro de mesh.

---

## 4. Como testar colisão

1. Caminhar **bordas** do piso — não cair.
2. Encostar em cada parede de blockout — superfície sólida, sem “entrar” no mesh.
3. Testar **cantos** de corredor — player gira câmera sem clip.
4. Portas: passagem livre ≥ 1,4 m; sem colisão extra no vão.
5. Depósito trancado: colisão bloqueia; áreas abertas não bloqueiam.

**Técnica:** visualizar `StaticBody3D` / collision shapes no editor em dúvida.

**Proibido em blockout:** colisão automática importada de GLB.

---

## 5. Como testar interação

1. Aproximar até prompt `[E]` aparecer.
2. Pressionar E — mensagem no HUD (não “Interação indisponível” por bug).
3. Repetir todos os interactables da sprint.
4. Confirmar textos conforme design da sprint.

---

## 6. Como testar cena / level

### Checklist térreo Pensão (Sprint 05–06)

- [ ] Nascer na trilha
- [ ] Chegar à varanda
- [ ] Entrar pela porta principal (sem loading)
- [ ] Recepção navegável
- [ ] Corredor 2,2 m — girar câmera confortável
- [ ] Quarto 102 — entrar e sair
- [ ] Cozinha — entrar e sair
- [ ] Depósito — chegar; trancado conforme design
- [ ] Saída futura escada — **sem rampa invisível**, **sem queda**
- [ ] Sem teto na câmera
- [ ] Lanterna não descarrega (debug)

### Checklist escada (Sprint 08+)

- [ ] Rampa visual **não** aparece
- [ ] Subida suave por colisão invisível
- [ ] Topo alinhado ao piso superior
- [ ] Câmera não atravessa laje/teto

### Checklist 2º andar (Sprint 09+)

- [ ] Piso contínuo — não cair ao térreo
- [ ] Vão escada com guarda-corpo
- [ ] Gerente / banheiro / quarto trancado acessíveis ou bloqueados conforme design

---

## 7. Como gravar vídeo de validação

1. Iniciar gravação **antes** do spawn.
2. Percorrer rota completa da sprint **uma vez**, sem cortes.
3. Mostrar interações principais (E + mensagem).
4. Duração: 30–90 s (térreo); até 3 min (vertical slice).
5. Nomear: `breu-sXX-playtest-YYYY-MM-DD.mp4`

---

## 8. Como reportar bug

```markdown
## Bug

- Sprint:
- Cena:
- Severidade: crítico / alto / médio / baixo
- Passos:
  1.
  2.
- Esperado:
- Obtido:
- Vídeo/screenshot:
- Regressão: sim/não
```

**Crítico** = impede concluir rota principal → sprint **não aprovada**.

---

## 9. Headless vs F6

| Tipo | Headless OK? |
|------|--------------|
| Projeto abre | Sim |
| Build C# | Sim |
| Navegação player | **Não** — exige F6 |
| Colisão / queda | **Não** |
| Interação HUD | **Não** |
| Clipping câmera | **Não** |

Headless **nunca** substitui playtest manual para level design.

---

## 10. Aprovação QA

QA assina no fechamento da sprint:

- [ ] Protocolo executado
- [ ] Vídeo anexado (se level)
- [ ] Zero bugs críticos abertos
- [ ] Recomendação: **Aprovar** / **Reprovar**
