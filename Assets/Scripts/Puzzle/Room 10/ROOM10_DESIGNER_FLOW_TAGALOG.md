# ROOM 10: MASTER BEDROOM - DETALYADONG FLOW (TAGALOG)

## Buod
Ito ang **HULING ROOM** ng game. Dito makikita ni Lisa ang buong katotohanan tungkol sa kanyang nakaraan. Ito ang pinakaemotional na parte ng buong laro.

---

## DALOY NG ROOM 10

### PHASE 1: PAGPASOK (Entry)
**Ano ang Nangyayari**:
- Si Lisa ay papasok sa master bedroom
- Ramdam niya na ito ang sentro ng lahat ng sikreto
- Nararamdaman niya ang tawag ng salamin

**Dialogues**:
1. "This room... it feels like the center of everything."
2. "All the pain, all the secrets lead here."
3. "The mirror... I feel drawn to it."
4. "I need to look into it. Something's calling me."

**Technical**:
- Player controls DISABLED habang may dialogue
- Automatic sequence, walang player input needed
- After dialogues, player controls ENABLED

---

### PHASE 2: SI EMILY AY HUMAHARANG (Emily Blocks)
**Ano ang Nangyayari**:
- Si Emily ay lilitaw, mas solid kaysa dati
- Humaharang siya sa salamin
- Ayaw niyang makita ni Lisa ang katotohanan

**Dialogues**:
1. "Emily appears. More solid than ever. Blocking the mirror."
2. (Emily) "I've been practicing what to tell you for decades."
3. (Emily) "But every word tastes like ash now that you're here."

**Technical**:
- Emily GameObject ay naka-active at visible
- Positioned in front of mirror
- Player controls ENABLED after dialogues
- Player can now explore room

---

### PHASE 3: PAG-EXPLORE NG ROOM (Examination)
**Ano ang Kailangan Gawin ng Player**:
- I-click ang **BED** o **DIARY** para mag-examine
- I-click ang **MUSIC BOX** para makuha ang Lullaby Fragment #4

**Interactable Objects**:

#### A. BED (Kama)
**Kapag Nag-click**:
1. "Evidence of violent struggle everywhere. Furniture overturned. Blood stains."
2. "A small child's bed... next to mother's bed."
3. "A child slept here... with her mother. That child was me."

**Result**: `hasExaminedRoom = true`

#### B. DIARY (Talaarawan)
**Kapag Nag-click**:
1. "Mother's final diary entry."
2. "'Tonight I end the child's defiance. She will learn obedience, or she will not learn at all.'"

**Result**: `hasExaminedRoom = true`

#### C. MUSIC BOX (Kahon ng Musika)
**Kapag Nag-click**:
1. "A music box. Emily's melody."
2. [Tumutugtog ang lullaby]
3. "This song... it's been in my head my whole life."
4. "Emily sang this to me. To calm me. To protect me."
5. **NOTIFICATION**: "Lullaby Fragment #4 added to inventory"

**Result**: `hasFoundLullaby = true`

---

### PHASE 4: UNLOCK NG MIRROR (Mirror Access)
**Conditions**:
- ✅ `hasExaminedRoom = true` (nag-examine ng bed O diary)
- ✅ `hasFoundLullaby = true` (nakuha ang music box)

**Kapag Na-unlock**:
1. Reality distortion dialogues:
   - "The room... it's changing. Temperature drops. Time slows."
   - "Shadows move incorrectly. Emily's desperation warps reality itself."

2. Emily's breakdown:
   - (Emily) "She was going to kill you that night. I couldn't let her."
   - (Emily) "But what I did... what I made you do..."
   - (Emily) "I possessed you. A child. I used your hands."

3. **Mirror glow effect ACTIVATES**

**Technical**:
- `canAccessMirror = true`
- Mirror glow GameObject ay nag-activate
- Player can now click mirror

---

### PHASE 5: PAGLAPIT SA SALAMIN (Approaching Mirror)
**Kapag I-click ang Mirror**:

**Kung Hindi Pa Unlocked**:
- "I should examine the room. Understand what happened here."
- (Hint para mag-explore pa)

**Kung Unlocked Na**:
1. Lisa approaches:
   - "I move toward the mirror. Emily tries to stop me."
   - "I need to know. I need to remember."

2. Emily desperate:
   - (Emily) "Please, Lisa. You don't need to see this."
   - (Emily) "I wanted to protect you from this memory forever."

3. Emily accepts:
   - (Emily) "I always knew this day would come."
   - (Emily) "I just hoped I'd found better words by now."
   - (Emily) "Look into the mirror, Lisa. See what I did. See what we did."

4. Mirror activates:
   - "The mirror glows. Images form. The past comes alive."
   - "I see... that night. The night everything changed."

---

### PHASE 6: FLASHBACK SEQUENCE (9 Parts)
**Ano ang Nangyayari**:
- Full-screen flashback panel ay lalabas
- Makikita ang 9 images na nagpapakita ng possession at murder
- Bawat image may dialogue

**Flashback Images at Dialogues**:

1. **Image 1**: Mother entering with pillow
   - "Mother enters the room. She's holding something. A pillow."

2. **Image 2**: Young Lisa in bed, terrified
   - "I'm in bed. Small. Terrified. She approaches."

3. **Image 3**: Emily's spirit entering Lisa
   - "Emily's spirit... she enters me. Possesses me."

4. **Image 4**: Possessed Lisa moving
   - "My small body moves on its own. Emily's will, my hands."

5. **Image 5**: Mother trying to smother, Lisa fighting
   - "Mother tries to smother me. Emily makes me fight back."

6. **Image 6**: Lisa's hands around mother's throat
   - "My hands... around mother's throat. But they're not my hands."

7. **Image 7**: Emily overlapping Lisa's body
   - "Emily's ghostly form overlaps my small body. We move as one."

8. **Image 8**: Mother falls
   - "Mother struggles. Falls. Goes still."

9. **Image 9**: Emily leaves, Lisa collapses
   - "Emily leaves my body. I collapse. Blood everywhere."

**Technical**:
- Flashback panel covers entire screen
- Each image shows for 3 seconds (or until player clicks)
- Dialogue appears with each image
- After all 9 images, panel closes

---

### PHASE 7: PAG-UNAWA (Understanding)
**After Flashback**:

1. Lisa processes:
   - "I... I killed her. We killed her."
   - "No. Emily killed her. Using me. To save me."

2. Lisa confronts Emily:
   - "You possessed me. Made me kill my own mother."
   - "To save me from her. But at what cost?"

3. Emily explains (5 parts):
   - (Emily) "She was going to kill you. I had no choice."
   - (Emily) "I was a child once too. Killed by my own mother."
   - (Emily) "When I died, I became... this. A protector. A guardian."
   - (Emily) "I found you. Felt your pain. Your fear. It was my pain. My fear."
   - (Emily) "I couldn't let another child die the way I did."

4. Lisa responds (4 parts):
   - "You saved me. But you made me a killer."
   - "I've lived my whole life not knowing. Not remembering."
   - "The nightmares. The fear. The feeling that something was wrong."
   - "It was all real. It all happened."

5. Emily apologizes (4 parts):
   - (Emily) "I'm sorry. I'm so sorry, Lisa."
   - (Emily) "I saved your life, but I stole your innocence."
   - (Emily) "I made you forget, hoping you could live a normal life."
   - (Emily) "But the truth always finds a way back."

---

### PHASE 8: PAGPAPATAWAD (Forgiveness)
**Ano ang Nangyayari**:
- Si Lisa ay magpapatawad kay Emily
- Ito ang emotional climax ng buong game

**Dialogues**:

1. Lisa forgives (3 parts):
   - "You did what you had to do. To save a child."
   - "You've been protecting me ever since. Carrying this burden alone."
   - "I forgive you, Emily. And I thank you."

2. Emily's relief:
   - (Emily) "Thank you. I've waited so long to hear those words."
   - (Emily) "I can finally... let go."

**Technical**:
- Music switches to peaceful track
- Emotional moment, let it breathe

---

### PHASE 9: PAGLISAN NI EMILY (Emily's Departure)
**Ano ang Nangyayari**:
- Si Emily ay unti-unting nawawala
- Peaceful music plays
- Ito ang goodbye

**Dialogues**:

1. Emily fades:
   - "Emily begins to fade. Her form becoming light."
   - (Emily) "You don't need me anymore, Lisa. You're strong enough now."
   - (Emily) "Live your life. Be free. Remember me, but don't let me haunt you."

2. Final goodbye:
   - "Goodbye, Emily. My protector. My friend."
   - "Thank you for saving me. Thank you for everything."

**Technical**:
- Emily sprite fades from alpha 1.0 to 0.0 over 3 seconds
- Smooth fade effect
- Emily GameObject deactivates after fade

---

### PHASE 10: EPILOGUE (Ending)
**Ano ang Nangyayari**:
- Si Lisa ay pwede nang umalis
- Game completion

**Dialogues**:
1. "The house is quiet now. The truth revealed. The burden lifted."
2. "I can finally leave this place. Leave the past behind."
3. "I survived. We both survived. And now, we can both rest."

**Technical**:
- Fade to black (2 seconds)
- Save game completion: `SaveSystem.Instance?.MarkPuzzleSolved("game_complete")`
- Load ending scene: `SceneManager.LoadScene("EndingScene")`

---

## UNITY SETUP GUIDE

### Step 1: Create GameObjects

#### A. Room10_FlowController
1. Create empty GameObject: "Room10_FlowController"
2. Add script: Room10_FlowController.cs
3. Assign lahat ng references sa inspector (tingnan sa ROOM10_COMPLETE_DESIGN.md)

#### B. Emily Manifestation
1. Create GameObject: "Emily_Manifestation"
2. Add SpriteRenderer
3. Assign Emily sprite (solid, visible)
4. Position in front of mirror
5. Drag to FlowController inspector

#### C. Truth Mirror
1. Create GameObject: "TruthMirror"
2. Add SpriteRenderer (mirror sprite)
3. Add Room10_Interactable script
4. Set type: Mirror
5. Add Collider2D (para ma-click)

**Child Object**: Glow Effect
- Add particle system or glowing sprite
- Initially disabled
- Drag to FlowController inspector

#### D. Bed
1. Create GameObject: "Bed"
2. Add SpriteRenderer (bed sprite)
3. Add Room10_Interactable script
4. Set type: Bed
5. Add Collider2D

#### E. Diary
1. Create GameObject: "Diary"
2. Add SpriteRenderer (diary sprite)
3. Add Room10_Interactable script
4. Set type: Diary
5. Add Collider2D

#### F. Music Box
1. Create GameObject: "MusicBox"
2. Add SpriteRenderer (music box sprite)
3. Add Room10_Interactable script
4. Set type: MusicBox
5. Add AudioSource
6. Assign lullaby clip sa inspector
7. Add Collider2D

---

### Step 2: Create UI (Flashback Panel)

**Hierarchy**:
```
Canvas
└── FlashbackPanel
    ├── Background (Black Image, full screen, alpha 0.9)
    ├── FlashbackImage (Image component)
    └── DialogueText (TextMeshProUGUI)
```

**Setup**:
1. Create Canvas (kung wala pa)
2. Add Panel: "FlashbackPanel"
3. Add Image: "Background" (black, full screen)
4. Add Image: "FlashbackImage" (para sa sprites)
5. Add TextMeshProUGUI: "DialogueText"
6. Disable panel initially
7. Drag to FlowController inspector

---

### Step 3: Assign Flashback Images

**Sa FlowController Inspector**:
1. Expand "Flashback Images" array
2. Set size: 9
3. Para sa bawat entry:
   - Assign sprite (flashback image)
   - Assign dialogue (copy from Room10_Dialogues.cs)
   - Set displayDuration: 3

**Kailangan ng 9 Images**:
- Image 1: Mother with pillow
- Image 2: Young Lisa terrified
- Image 3: Emily entering Lisa
- Image 4: Possessed Lisa
- Image 5: Fighting back
- Image 6: Hands on throat
- Image 7: Emily overlapping
- Image 8: Mother falling
- Image 9: Emily leaving

---

### Step 4: Setup Audio

**Audio Clips Needed**:
1. **Tense Music** - Intro phase
2. **Lullaby Clip** - Music box phase
3. **Peaceful Music** - Departure phase

**Setup**:
1. Create AudioSource GameObject: "BackgroundMusic"
2. Assign to FlowController inspector
3. Assign all 3 audio clips sa inspector

---

### Step 5: Add to Inventory Database

**Item**: Lullaby Fragment #4
- Name: "Lullaby Fragment #4"
- Description: "The final piece of Emily's lullaby."
- Sprite: Music box icon
- Category: Key Item

---

### Step 6: Testing

**Test Sequence**:
1. ✅ Play scene, intro plays
2. ✅ Click bed/diary, dialogues show
3. ✅ Click music box, lullaby plays, item added
4. ✅ Mirror unlocks, glow appears
5. ✅ Click mirror, approach sequence plays
6. ✅ Flashback shows all 9 images
7. ✅ Understanding dialogues play
8. ✅ Forgiveness dialogues play
9. ✅ Emily fades smoothly
10. ✅ Epilogue plays
11. ✅ Scene transitions to ending

---

## IMPORTANT NOTES

### Timing
- Maraming dialogues (60+)
- Bawat dialogue ay hinihintay ang player click
- Total sequence: 10-15 minutes
- **Huwag mag-rush**, ito ang climax ng game

### Emotional Pacing
1. **Tension** - Build up
2. **Investigation** - Discovery
3. **Revelation** - Truth
4. **Understanding** - Processing
5. **Resolution** - Forgiveness
6. **Peace** - Departure
7. **Closure** - Ending

### Player Controls
- **DISABLED** during all dialogue sequences
- **ENABLED** during exploration phase
- **DISABLED** during flashback
- **DISABLED** during ending

---

## TROUBLESHOOTING

### Problem: Mirror hindi nag-unlock
**Solution**: Check kung `hasExaminedRoom` at `hasFoundLullaby` ay both true

### Problem: Flashback images hindi lumalabas
**Solution**: Check kung may 9 entries sa flashbackImages array

### Problem: Emily hindi nag-fade
**Solution**: Check kung may SpriteRenderer component si Emily

### Problem: Music hindi nag-switch
**Solution**: Check kung assigned lahat ng audio clips

### Problem: Scene hindi nag-transition
**Solution**: Check kung tama ang scene name sa Build Settings

---

## FINAL CHECKLIST

- [ ] Lahat ng GameObjects created
- [ ] Lahat ng scripts assigned
- [ ] Lahat ng references assigned sa inspector
- [ ] 9 flashback images assigned
- [ ] 3 audio clips assigned
- [ ] Flashback panel setup
- [ ] Inventory item added
- [ ] Tested full sequence
- [ ] Ending scene created
- [ ] Build settings updated

---

**GOOD LUCK!** Ito ang huling room, gawin mong memorable! 🎮✨
