# Room 08 - Lisa's Bathroom (Bagong Flow)

## 🎯 BUONG DALOY

### Step 1: Kolektahin ang Evidence Items
- Pumasok si player sa bathroom
- **Kolektahin ang evidence items** na nakakalat sa room
- Ito ay pickup items (hindi interactables)
- I-mark ang `hasCollectedAllEvidence = true` pag nakuha na lahat

### Step 2: Kunin ang Hammer sa Cabinet
- **I-interact ang Medicine Cabinet**
- Prerequisite: Kailangan nakuha na ang evidence
- Makukuha ang **hammer** sa cabinet
- I-mark ang `hasFoundHammer = true`

### Step 3: I-interact ang Bathtub
- **I-interact ang Bathtub**
- Prerequisite: Kailangan nakuha na ang hammer
- Ipakita ang bathtub dialogue
- I-mark ang `hasInteractedWithBathtub = true`

### Step 4: I-interact ang Mirror (QTE)
- **I-interact ang Mirror**
- Prerequisites:
  - ✅ Evidence nakuha na
  - ✅ Hammer nakuha na
  - ✅ Bathtub na-interact na
- Magsisimula ang **Mirror QTE** (15 taps)
- Kailangan mag-tap ng 15 beses sa loob ng 25 seconds
- Pupunan ng kulay ang screen habang tumataas ang progress

### Step 5: Basag na ang Mirror
- Pagkatapos ng 15 successful taps:
  - Magbabago ang mirror sprite sa **broken mirror**
  - Lalabas ang **passage** sa likod ng mirror
  - Pwede na i-interact ang passage

### Step 6: Umakyat sa Passage
- **I-interact ang Passage**
- Lilipat sa **Master's Bathroom** (Room 09)

---

## 📋 FLOW CONTROLLER FLAGS

```csharp
// Evidence Collection
public bool hasCollectedAllEvidence = false; // Lahat ng evidence nakuha na
public bool hasFoundHammer = false; // Hammer galing sa cabinet
public bool hasInteractedWithBathtub = false; // Bathtub interaction

// Mirror Progress
public bool hasExaminedMirror = false; // Na-examine ang mirror
public bool hasBrokenMirror = false; // Mirror QTE tapos na
public bool canClimbThrough = false; // Pwede na umakyat sa passage
```

---

## 🎮 MIRROR QTE SYSTEM

### Settings:
- **Total Taps**: 15 (binago from 50)
- **Time Limit**: 25 seconds
- **Tap Area**: Full screen button
- **Visual Feedback**: Tataas ang color fill kada tap

### UI Setup:

```
QTE Panel (GameObject)
├─ Full Screen Tap Area (Image)
│   ├─ Color: Semi-transparent red (0.8, 0.2, 0.2, 0.5)
│   ├─ Button Component (idadagdag sa runtime)
│   └─ Fill Image (Image, child)
│       ├─ Image Type: Filled
│       ├─ Fill Method: Horizontal o Radial
│       ├─ Fill Amount: 0 → 1 (animated)
│       └─ Color: Same sa parent o iba
│
├─ Timer Text (Text o TMP)
│   └─ Ipapakita: "25.0s" → "0.0s"
│
├─ Progress Text (Text o TMP)
│   └─ Ipapakita: "0/15" → "15/15"
│
└─ Mirror Image (Image)
    └─ Magbabago ang sprite based sa progress:
        - Phase 1: Clean mirror (0-25%)
        - Phase 2: First cracks (25-50%)
        - Phase 3: More cracks (50-75%)
        - Phase 4: Almost shattered (75-100%)
```

### Recommended na Kulay:

**Fill Color** (para sa tap area at fill image):
- **Red-ish**: `new Color(0.8f, 0.2f, 0.2f, 0.5f)` - Aggressive, urgent
- **Blue-ish**: `new Color(0.2f, 0.4f, 0.8f, 0.5f)` - Calm, focused
- **Purple-ish**: `new Color(0.6f, 0.2f, 0.8f, 0.5f)` - Mysterious, eerie
- **Green-ish**: `new Color(0.2f, 0.8f, 0.4f, 0.5f)` - Progress, success

**Recommended**: Red-ish para sa urgency at tension!

---

## 🎨 UNITY SETUP

### 1. Scene Objects

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController (GameObject)
│   └─ Room08_FlowController (Script)
│
├─ Evidence Items (GameObjects with pickup script)
│   ├─ Evidence_1
│   ├─ Evidence_2
│   └─ Evidence_3
│
├─ Medicine Cabinet (GameObject)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: MedicineCabinet
│
├─ Bathtub (GameObject)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Bathtub
│
├─ Mirror (GameObject)
│   ├─ SpriteRenderer (normal mirror sprite)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Mirror
│
├─ Passage (GameObject)
│   ├─ Initially: SetActive(false)
│   ├─ BoxCollider2D (Is Trigger ✓)
│   └─ Room08_Interactable (Script)
│       └─ Object Type: Passage
│
└─ Canvas
    └─ QTE Panel (GameObject)
        └─ Room08_MirrorQTE (Script)
```

### 2. Room08_FlowController Setup

**I-assign sa Inspector**:
- Mirror Sprite Renderer: I-drag ang mirror GameObject's SpriteRenderer
- Mirror Normal Sprite: Normal mirror sprite
- Mirror Broken Sprite: Broken/shattered mirror sprite
- Passage Object: I-drag ang passage GameObject
- Next Scene Name: "Room09_Master's_Bathroom"

### 3. Room08_MirrorQTE Setup

**Gumawa ng QTE Panel**:

1. **Create Panel**:
   - Right-click Canvas → UI → Panel
   - I-rename sa "QTE_Panel"
   - I-stretch to full screen (Anchor: Stretch, Offset: 0,0,0,0)
   - Set Color: Black with alpha 0.8 (para sa background)

2. **Create Full Screen Tap Area**:
   - Right-click QTE_Panel → UI → Image
   - I-rename sa "TapArea"
   - I-stretch to full screen
   - Set Color: `RGBA(204, 51, 51, 128)` o `(0.8, 0.2, 0.2, 0.5)`
   - **WALANG Button component** (idadagdag sa runtime)

3. **Create Fill Image** (child ng TapArea):
   - Right-click TapArea → UI → Image
   - I-rename sa "FillImage"
   - I-stretch to full screen
   - Image Type: **Filled**
   - Fill Method: **Horizontal** (Left to Right) o **Radial 360**
   - Fill Origin: Left o Bottom
   - Fill Amount: 0 (mag-animate to 1)
   - Set Color: Same sa TapArea o iba

4. **Create Timer Text**:
   - Right-click QTE_Panel → UI → Text (o TextMeshPro)
   - I-rename sa "TimerText"
   - Position: Top center
   - Font Size: 48
   - Alignment: Center
   - Text: "25.0s"

5. **Create Progress Text**:
   - Right-click QTE_Panel → UI → Text (o TextMeshPro)
   - I-rename sa "ProgressText"
   - Position: Bottom center
   - Font Size: 36
   - Alignment: Center
   - Text: "0/15"

6. **Create Mirror Image** (optional):
   - Right-click QTE_Panel → UI → Image
   - I-rename sa "MirrorImage"
   - Position: Center
   - Size: 400x600 (o appropriate size)
   - I-assign ang mirror sprite

**I-assign sa Room08_MirrorQTE Inspector**:
- QTE Panel: I-drag ang QTE_Panel
- Full Screen Tap Area: I-drag ang TapArea Image
- Fill Image: I-drag ang FillImage
- Fill Color: I-set sa gusto mong kulay (default: red-ish)
- Timer Text: I-drag ang TimerText (kung Text)
- Timer Text TMP: I-drag ang TimerText (kung TMP)
- Progress Text: I-drag ang ProgressText (kung Text)
- Progress Text TMP: I-drag ang ProgressText (kung TMP)
- Mirror Image: I-drag ang MirrorImage (optional)
- Mirror Phase 1-4: I-assign ang mirror sprites (clean → cracked → shattered)
- Total Taps: 15
- Total Time Limit: 25
- Audio clips: I-assign ang tap, crack, shatter sounds

---

## 🔄 INTERACTION ORDER

### Tamang Order:
1. ✅ Kolektahin ang evidence items
2. ✅ I-interact ang cabinet → Kunin ang hammer
3. ✅ I-interact ang bathtub
4. ✅ I-interact ang mirror → Magsimula ang QTE
5. ✅ Tapusin ang QTE (15 taps)
6. ✅ Basag na ang mirror → Lalabas ang passage
7. ✅ I-interact ang passage → Pumunta sa Room 09

### Blocked Interactions:

**Cabinet (bago ang evidence)**:
- Message: "I should collect the evidence first."

**Bathtub (bago ang hammer)**:
- Message: "I should look around more before examining the bathtub."

**Mirror (bago ang prerequisites)**:
- Message: "I need to finish examining everything first."

---

## ✅ PAANO I-TEST

### Test Flow:

1. **Pumasok sa Room 08**
   - ✅ Tumugtog ang intro dialogue
   - ✅ Pwede gumalaw si player pagkatapos ng intro

2. **Subukan ang Cabinet Muna**
   - ✅ Ipapakita ang "collect evidence first" message

3. **Kolektahin ang Evidence Items**
   - ✅ Mawawala ang evidence items pag nakuha
   - ✅ `hasCollectedAllEvidence = true`

4. **I-interact ang Cabinet**
   - ✅ Bubuksan ang cabinet
   - ✅ Makukuha ang hammer
   - ✅ Ipapakita ang hammer dialogue

5. **Subukan ang Bathtub**
   - ✅ Ipapakita ang bathtub dialogue
   - ✅ `hasInteractedWithBathtub = true`

6. **I-interact ang Mirror**
   - ✅ Lalabas ang QTE panel
   - ✅ Makikita ang full screen tap area
   - ✅ Magsisimula ang timer
   - ✅ Ipapakita ang progress "0/15"

7. **Mag-tap ng 15 Beses**
   - ✅ Bawat tap may tunog
   - ✅ Tataas ang fill image
   - ✅ Mag-update ang progress "1/15", "2/15", etc.
   - ✅ Magbabago ang mirror sprite sa 25%, 50%, 75%
   - ✅ Mag-shake ang camera kada tap

8. **Tapusin ang QTE**
   - ✅ Tutugtog ang shatter sound
   - ✅ Malaking camera shake
   - ✅ Ipapakita ang success dialogue
   - ✅ Magsasara ang QTE panel
   - ✅ Magbabago ang mirror sprite sa broken
   - ✅ Lalabas ang passage

9. **I-interact ang Passage**
   - ✅ Climb through dialogue
   - ✅ Fade transition
   - ✅ I-load ang Room 09

---

## 🐛 TROUBLESHOOTING

### Issue: "Hindi bumubukas ang cabinet"

**Solution**: Siguraduhing nakuha na ang evidence. Check ang `hasCollectedAllEvidence` flag.

### Issue: "Hindi nag-i-interact ang bathtub"

**Solution**: Siguraduhing nakuha na ang hammer. Check ang `hasFoundHammer` flag.

### Issue: "Hindi nagsisimula ang mirror QTE"

**Solution**: Check lahat ng prerequisites:
- `hasCollectedAllEvidence = true`
- `hasFoundHammer = true`
- `hasInteractedWithBathtub = true`

### Issue: "Hindi nag-re-register ang tap"

**Solution**:
- Check kung may Button component sa TapArea (idadagdag sa runtime)
- Check kung active ang QTE panel
- Check kung `isQTEActive = true`

### Issue: "Hindi pumupuno ang fill image"

**Solution**:
- Check kung naka-assign ang Fill Image sa Inspector
- Check kung Image Type ay "Filled"
- Check kung nag-a-animate ang Fill Amount from 0 to 1

### Issue: "Hindi bumabasag ang mirror"

**Solution**:
- Check kung natapos na ang 15 taps
- Check kung tinawag ang `OnMirrorBroken()`
- Check kung naka-assign ang broken sprite

---

## 💡 TIPS

### Para sa Better Experience:

1. **Visual Feedback**:
   - Gumamit ng distinct colors para sa fill (red = urgent)
   - I-animate ng smooth ang fill
   - Ipakita ang mirror cracking progressively

2. **Audio Feedback**:
   - Tap sound: Quick at satisfying
   - Crack sound: Escalating intensity
   - Shatter sound: Dramatic at final

3. **Camera Shake**:
   - Maliit na shake kada tap
   - Malaking shake sa final shatter
   - Nagdadagdag ng impact at urgency

4. **Timer Pressure**:
   - 25 seconds para sa 15 taps = ~1.67s per tap
   - Comfortable pero may tension
   - Magbabago ang kulay (white → yellow → red) habang nauubos ang oras

---

**Setup complete! Test mo na sa Unity!** 🎮✨

