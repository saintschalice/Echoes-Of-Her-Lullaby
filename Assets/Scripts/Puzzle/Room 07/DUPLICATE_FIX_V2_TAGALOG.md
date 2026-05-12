# Room 07 - Ayos sa Duplicate Dialogues (Mirror Sequence)

## 🐛 ANO ANG PROBLEMA?

May **duplicate dialogues** sa mirror interaction. Umuulit ang dialogues kasi dalawang scripts ang nag-trigger:

### Dati (May Problema):

**Room07_Interactable.cs**:
1. ✅ MIRROR_READY dialogues (3 parts)
2. ✅ MIRROR_JUMPSCARE dialogues (2 parts)
3. ✅ MIRROR_CHASE dialogue
4. Tapos tatawag ng → `MirrorJumpscareSequence.TriggerJumpscare()`

**MirrorJumpscareSequence.cs**:
1. ❌ "Let me check the mirror..." (UULIT!)
2. Jumpscare visual + sound
3. ❌ "That lullaby..." (UULIT!)
4. ❌ "The door is locked!..." (UULIT!)
5. Start chase

**Resulta**: Sobrang daming dialogues! Nakakalito!

---

## ✅ ANO ANG GINAWA?

**Tinanggal** ang duplicate dialogues sa `MirrorJumpscareSequence.cs` kasi nandun na lahat sa `Room07_Interactable.cs`.

### Ngayon (Ayos Na):

**Room07_Interactable.cs**:
1. ✅ MIRROR_READY dialogues (3 parts)
2. ✅ MIRROR_JUMPSCARE dialogues (2 parts)
3. ✅ MIRROR_CHASE dialogue
4. Tapos tatawag ng → `MirrorJumpscareSequence.TriggerJumpscare()`

**MirrorJumpscareSequence.cs**:
1. ✅ Jumpscare visual + sound
2. ✅ Spawn Emily
3. ✅ Camera shake
4. ✅ Play lullaby fragment 3
5. ✅ Lock bedroom door
6. ✅ Start chase (walang dialogue na)

**Resulta**: Malinis na! Walang uulit!

---

## 📋 DALOY NG MIRROR SEQUENCE (FINAL)

### Buong Daloy:

1. **Player nag-interact sa mirror** (Room07_Interactable.cs)
   - Check kung tapos na lahat ng puzzles
   - Kung hindi pa: Ipakita hint kung ano pa kulang
   - Kung tapos na: Proceed sa step 2

2. **Mirror Ready Dialogues** (Room07_Interactable.cs)
   - MIRROR_READY_1: "This mirror... it's the same one from my nightmares."
   - MIRROR_READY_2: "Every night, I see her reflection behind me..."
   - MIRROR_READY_3: "But she's not there when I turn around."

3. **Mirror Jumpscare Dialogues** (Room07_Interactable.cs)
   - MIRROR_JUMPSCARE_1: "Wait... something's different this time."
   - MIRROR_JUMPSCARE_2: "The reflection... it's moving on its own!"

4. **Chase Dialogue** (Room07_Interactable.cs)
   - MIRROR_CHASE: "She's here! I need to get out!"

5. **Jumpscare Sequence** (MirrorJumpscareSequence.cs)
   - Ipakita jumpscare image
   - Play jumpscare sound
   - Spawn Emily sa likod ni Lisa
   - Camera shake
   - Itago jumpscare image

6. **Lullaby** (MirrorJumpscareSequence.cs)
   - Play lullaby fragment 3
   - Hintayin matapos ang lullaby

7. **Lock Door & Start Chase** (MirrorJumpscareSequence.cs)
   - I-lock ang bedroom door (hindi makalabas)
   - I-enable ulit player controls
   - I-activate ang Emily's aggressive chase AI
   - Kailangan tumakbo sa bathroom!

---

## 🎯 MGA PAGBABAGO

### Sa MirrorJumpscareSequence.cs:

**TINANGGAL**:
- ❌ "Let me check the mirror..." dialogue
- ❌ "That lullaby..." dialogue
- ❌ "The door is locked!..." dialogue
- ❌ Mga dialogue waiting loops

**PINANATILI**:
- ✅ Jumpscare visual effects
- ✅ Audio playback
- ✅ Emily spawn at AI activation
- ✅ Door locking
- ✅ Camera shake

**DINAGDAG**:
- ✅ Hintayin matapos ang lullaby
- ✅ Comment na nag-explain na sa ibang script na ang dialogues

---

## 🔊 DALOY NG AUDIO

1. **Jumpscare sound** - Pag lumitaw si Emily
2. **Lullaby fragment 3** - Pagkatapos ng jumpscare
3. **Chase music** - Habang chase (handled ng Emily AI)

---

## 🎮 DALOY NG PLAYER CONTROL

1. **Disabled** - Habang mirror dialogues (Room07_Interactable)
2. **Disabled** - Habang jumpscare sequence
3. **Disabled** - Habang tumutugtog ang lullaby
4. **ENABLED** - Pag nagsimula ang chase (pwede na tumakbo!)

---

## ✅ PAANO I-TEST

### Test Mirror Interaction:

1. **Tapusin lahat ng puzzles** sa Lisa's Bedroom
2. **I-interact ang mirror**
3. **Expected na Mangyayari**:
   - ✅ MIRROR_READY dialogues (3 parts)
   - ✅ MIRROR_JUMPSCARE dialogues (2 parts)
   - ✅ MIRROR_CHASE dialogue
   - ✅ Jumpscare visual + sound
   - ✅ Tumugtog ang lullaby (buong duration)
   - ✅ Na-enable ang player controls
   - ✅ Nagsimula humabol si Emily
   - ✅ Na-lock ang bedroom door
   - ✅ Pwede tumakbo sa bathroom

4. **Check kung may duplicates**:
   - ❌ WALANG "Let me check the mirror..." dialogue
   - ❌ WALANG "That lullaby..." dialogue
   - ❌ WALANG "The door is locked!..." dialogue
   - ✅ Proper MIRROR dialogues lang from Room07_ShortDialogues_FINAL

---

## 💡 MGA NOTES

### Bakit Gumagana ang Fix:

1. **Single Source of Truth**: Lahat ng dialogues galing sa `Room07_Interactable.cs`
2. **Clear Separation**: Interactable = story, Jumpscare = effects
3. **Walang Duplicates**: Bawat dialogue isang beses lang
4. **Better Flow**: Smooth transition from dialogue → jumpscare → chase

### Design Pattern:

```
Room07_Interactable.cs (Story Layer)
    ↓ Nag-handle ng lahat ng dialogues
    ↓ Nag-manage ng narrative flow
    ↓ Tumatawag ng jumpscare pag ready na
    ↓
MirrorJumpscareSequence.cs (Effects Layer)
    ↓ Nag-handle ng visual effects
    ↓ Nag-manage ng audio playback
    ↓ Nag-control ng Emily AI
    ↓ Nag-lock ng doors
```

---

## 🐛 TROUBLESHOOTING

### Issue: "May duplicate dialogues pa rin"

**Possible Causes**:
1. Luma pa yung cached script
2. May multiple MirrorJumpscareSequence components sa scene

**Solution**:
1. I-close ang Unity
2. I-delete ang Library folder
3. I-open ulit ang Unity
4. Check sa scene kung may duplicate components

### Issue: "Walang dialogues"

**Possible Causes**:
1. Wala ang Room07_ShortDialogues_FINAL
2. Wala ang DialogueSystemV2 sa scene

**Solution**:
1. Check kung nandun ang Room07_ShortDialogues_FINAL.cs
2. Verify na nandun ang DialogueSystemV2 sa scene
3. Check ang Console kung may errors

### Issue: "Hindi tumutugtog ang lullaby"

**Possible Causes**:
1. Hindi naka-assign ang lullaby clip
2. Hindi naka-assign ang audio source

**Solution**:
1. I-assign ang lullaby fragment 3 sa Inspector
2. I-assign ang music box audio source
3. Check kung hindi naka-mute ang audio source

---

## 📝 MGA FILES NA BINAGO

- `Assets/Scripts/Puzzle/Room 07/MirrorJumpscareSequence.cs`
  - Tinanggal ang duplicate dialogues
  - Dinagdag ang lullaby wait duration
  - Pinaganda ang comments

---

**Ayos na! Walang duplicate dialogues sa mirror sequence!** 🎉

