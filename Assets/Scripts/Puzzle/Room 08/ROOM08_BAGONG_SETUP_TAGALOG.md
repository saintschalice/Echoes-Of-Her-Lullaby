# Room 08 - Bagong Setup Guide (Tagalog)

## MGA PAGBABAGO

### 1. QTE (Quick Time Event)
- **DATI**: 15 taps sa moving targets, 2 minutes
- **NGAYON**: **50 taps anywhere sa screen, 25 seconds**

### 2. Evidence
- **DATI**: Bandages, Torn Clothes, Note, Hammer
- **NGAYON**: **Torn Clothes, Note, Hammer lang** (walang bandages)

### 3. Emily sa Mirror
- **DATI**: Laging nakikita si Emily sa mirror
- **NGAYON**: **Hindi makikita si Emily initially, lalabas lang after makuha lahat ng items**

### 4. Mirror Design
- **DATI**: Cracks lang during QTE
- **NGAYON**: **Magbabago ang sprite ng mirror, lalabas ang passage**

---

## SETUP SA UNITY

### STEP 1: I-update ang QTE Panel

1. **Tanggalin ang Tap Target**:
   ```
   - Delete tap target prefab references
   - Delete tap target parent
   ```

2. **I-add ang Full Screen Tap Area**:
   ```
   QTE Panel → Right-click → UI → Image
   Name: FullScreenTapArea
   
   Settings:
   - Anchor: Stretch (full screen)
   - Left: 0, Top: 0, Right: 0, Bottom: 0
   - Color: Transparent (R:0, G:0, B:0, A:0) o semi-transparent
   - Raycast Target: ✓ (checked)
   ```

3. **I-update ang Texts**:
   ```
   TimerText:
   - Text: "25.0s"
   
   ProgressText:
   - Text: "0/50"
   ```

### STEP 2: I-setup ang Room08_MirrorQTE Component

```
Room08_MirrorQTE Component:
├── QTE Settings
│   ├── Total Taps: 50
│   ├── Total Time Limit: 25
│   └── Max Failures: 0
├── UI References
│   ├── QTE Panel: [I-drag ang QTE Panel]
│   ├── Full Screen Tap Area: [I-drag ang FullScreenTapArea Image] ⭐
│   ├── Timer Text TMP: [I-drag ang TimerText]
│   └── Progress Text TMP: [I-drag ang ProgressText]
└── Visual Effects
    ├── Mirror Phase 1-4: [I-assign ang sprites]
    └── Shatter Effect: [Particle effect]
```

### STEP 3: I-remove ang Bandages

```
1. Hanapin ang "Bandages" GameObject sa scene
2. Delete
```

### STEP 4: I-setup ang Mirror Sprites

1. **Gumawa ng 2 sprites**:
   ```
   - mirrorNormalSprite (normal mirror)
   - mirrorBrokenSprite (broken mirror with cracks)
   ```

2. **I-assign sa Room08_FlowController**:
   ```
   Room08_FlowController Component:
   └── Mirror Sprites
       ├── Mirror Sprite Renderer: [I-drag ang Mirror GameObject's SpriteRenderer]
       ├── Mirror Normal Sprite: [I-drag ang normal sprite]
       └── Mirror Broken Sprite: [I-drag ang broken sprite]
   ```

### STEP 5: I-setup ang Passage

1. **Gumawa ng Passage GameObject**:
   ```
   Hierarchy → Right-click → Create Empty
   Name: Passage
   ```

2. **I-add ang SpriteRenderer**:
   ```
   Add Component → Sprite Renderer
   Sprite: [Passage sprite - dapat may design ng passage/hole]
   ```

3. **I-add ang Room08_Interactable**:
   ```
   Add Component → Room08_Interactable
   My Type: Passage
   ```

4. **I-disable initially**:
   ```
   Passage GameObject → Active: ✗ (unchecked)
   ```

5. **I-assign sa Room08_FlowController**:
   ```
   Room08_FlowController Component:
   └── Mirror Sprites
       └── Passage Object: [I-drag ang Passage GameObject]
   ```

### STEP 6: I-setup si Emily sa Mirror

```
EmilyInMirror GameObject:
└── Active: ✗ (unchecked) ⭐ IMPORTANTE!

Dapat disabled initially, lalabas lang after makuha lahat ng items.
```

---

## FLOW NG PUZZLE

### 1. Pagpasok sa Bathroom
```
✅ Mirror: Normal sprite
✅ Emily: Hindi visible
✅ Passage: Hidden
```

### 2. Mag-explore
```
1. Interact with Bathtub → Get Torn Clothes
2. Interact with Evidence → Get Apology Note
3. Interact with Medicine Cabinet → Get Hammer
```

### 3. After Makuha Lahat
```
✅ Emily appears sa mirror
✅ Dialogue: "All evidence found"
✅ Dialogue: Emily appears sequence
```

### 4. Interact with Mirror
```
✅ Dialogue: Mirror examination
✅ Dialogue: Two-way mirror discovery
✅ Dialogue: Emily confrontation
✅ Prompt: "Break the mirror"
```

### 5. QTE Starts
```
✅ Panel shows: "0/50" and "25.0s"
✅ Tap anywhere on screen
✅ Each tap: Counter increases, mirror cracks more
✅ Timer counts down
```

### 6. QTE Success (50 taps within 25 seconds)
```
✅ Mirror shatters
✅ Mirror sprite changes to broken sprite
✅ Emily disappears from mirror
✅ Passage becomes visible
✅ Dialogue: "Passage found"
```

### 7. Climb Through Passage
```
✅ Interact with passage
✅ Dialogue: "Climb through"
✅ Load next scene
```

---

## TESTING

### Test 1: QTE Mechanics
```
1. Start QTE
2. Expected: "0/50" and "25.0s"
3. Tap anywhere on screen
4. Expected: Counter increases
5. Expected: Timer counts down
6. Tap 50 times within 25 seconds
7. Expected: Mirror breaks
```

### Test 2: Evidence Collection
```
1. Find torn clothes
2. Find apology note
3. Find hammer
4. Expected: Emily appears in mirror
5. Expected: Dialogue sequence plays
```

### Test 3: Mirror Breaking
```
1. Complete QTE
2. Expected: Mirror sprite changes to broken
3. Expected: Emily disappears
4. Expected: Passage becomes visible
5. Expected: Can interact with passage
```

### Test 4: No Bandages
```
1. Search entire bathroom
2. Expected: NO bandages GameObject
3. Expected: Only 3 items (torn clothes, note, hammer)
```

---

## COMMON ISSUES

### Issue 1: Hindi gumagana ang full screen tap
**Fix**:
```
- Check if FullScreenTapArea has Raycast Target checked
- Check if FullScreenTapArea is full screen (Anchor: Stretch)
- Check if assigned sa Room08_MirrorQTE component
```

### Issue 2: Hindi lumalabas si Emily
**Fix**:
```
- Check if EmilyInMirror is assigned sa Room08_FlowController
- Check if nakuha na lahat ng 3 items
- Check console for errors
```

### Issue 3: Hindi nagbabago ang mirror sprite
**Fix**:
```
- Check if mirrorNormalSprite and mirrorBrokenSprite are assigned
- Check if mirrorSpriteRenderer is assigned
- Check if sprites are different
```

### Issue 4: Hindi lumalabas ang passage
**Fix**:
```
- Check if Passage GameObject exists
- Check if assigned sa Room08_FlowController.passageObject
- Check if Passage has Room08_Interactable component
```

---

## QUICK CHECKLIST

Before testing:

- [ ] FullScreenTapArea created and assigned
- [ ] Room08_MirrorQTE: totalTaps = 50, totalTimeLimit = 25
- [ ] Bandages GameObject deleted
- [ ] Mirror sprites created and assigned
- [ ] Passage GameObject created and assigned
- [ ] EmilyInMirror initially disabled
- [ ] Passage initially disabled
- [ ] All components assigned correctly

---

## SUMMARY

### Ano ang Binago:
- ✅ QTE: 50 taps, 25 seconds, full screen
- ✅ Tinanggal ang bandages
- ✅ Emily lumalabas after items collected
- ✅ Mirror sprite nagbabago
- ✅ Passage lumalabas after breaking

### Ano ang Hindi Binago:
- ✅ Torn clothes sa bathtub
- ✅ Apology note evidence
- ✅ Hammer sa medicine cabinet
- ✅ Emily humming outside
- ✅ Door locked

**Status**: ✅ CODE UPDATED
**Next**: I-update ang Unity scene
**Date**: May 4, 2026
