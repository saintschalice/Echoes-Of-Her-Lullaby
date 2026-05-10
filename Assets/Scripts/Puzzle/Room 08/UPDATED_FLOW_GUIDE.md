# 🔄 ROOM 08 - UPDATED FLOW WITH HAMMER & EVIDENCE

## 🎯 NEW FLOW

```
1. ENTRY
   ↓
2. EXAMINE BATHTUB
   ↓
3. EXAMINE MEDICINE CABINET
   ↓
4. COLLECT EVIDENCE (3 items)
   - Bandages (individual dialogue)
   - Torn Clothes (individual dialogue)
   - Apology Note (individual dialogue)
   ↓
5. COLLECT HAMMER from Medicine Cabinet
   - Adds to inventory with notification
   - Shows hammer dialogue
   ↓
6. EMILY APPEARS IN MIRROR (automatic)
   - Triggered when all evidence + hammer collected
   - Emily sprite appears in mirror
   - Dialogue sequence
   ↓
7. EXAMINE MIRROR
   - Long confrontation sequence
   ↓
8. BREAK MIRROR (QTE)
   - 15 taps, 2 minutes
   - Requires hammer in inventory
   ↓
9. ESCAPE
   - Passage revealed
   - Climb through
```

---

## 📋 DETAILED FLOW

### **PHASE 1: ENTRY (30 seconds)**

```
Player enters Room 08
  ↓
Intro dialogue (4 parts):
  1. "The bathroom. My only sanctuary."
  2. "The only room with a lock..."
  3. "The door is locked. Emily locked me in."
  4. "I can hear her outside. Humming. Waiting."
  ↓
Emily humming sound starts (looping)
  ↓
Player can move
```

---

### **PHASE 2: EXAMINATION (2 minutes)**

#### **A. Bathtub**
```
Click Bathtub
  ↓
Dialogue:
  1. "The bathtub. I would fill it with cold water."
  2. "Sit here until I couldn't feel anything anymore..."
  ↓
hasCheckedBathtub = true
```

#### **B. Medicine Cabinet**
```
Click Medicine Cabinet
  ↓
Dialogue:
  1. "The medicine cabinet. Pills everywhere."
  2. "Mother's pills. Father's pills. So many pills."
  ↓
hasCheckedMedicine = true
```

---

### **PHASE 3: EVIDENCE COLLECTION (3 minutes)**

#### **Evidence 1: Bandages**
```
Click Bandages
  ↓
Dialogue:
  1. "Bloodstained bandages. Hidden under the sink."
  2. "Evidence of what she did to me. What I survived."
  ↓
Bandages disappear
hasFoundBandages = true
```

#### **Evidence 2: Torn Clothes**
```
Click Torn Clothes
  ↓
Dialogue:
  1. "My torn clothes. From that night."
  2. "I remember the pain. The fear. Emily took it all away."
  ↓
Torn Clothes disappear
hasFoundTornClothes = true
```

#### **Evidence 3: Apology Note**
```
Click Apology Note
  ↓
Dialogue:
  1. "A handwritten note: 'I'm sorry. I'm so sorry.'"
  2. "Mother's handwriting. Shaky. Desperate. But it doesn't change what happened."
  ↓
Apology Note disappears
hasFoundApologyNote = true
```

---

### **PHASE 4: HAMMER COLLECTION (1 minute)**

```
Click Hammer (in/near Medicine Cabinet)
  ↓
Notification: "Hammer obtained"
  ↓
Wait 2 seconds (notification visible)
  ↓
Dialogue:
  1. "A hammer. Hidden behind the pills."
  2. "Why would mother hide a hammer here? Unless... she knew I'd need it."
  ↓
Hammer disappears
hasFoundHammer = true
  ↓
Check: All evidence + hammer collected?
  ↓
YES → Trigger Emily Appearance
```

---

### **PHASE 5: EMILY APPEARS (1 minute)** ✨

```
All evidence + hammer collected
  ↓
Dialogue: "I've found everything. All the evidence of what happened here."
  ↓
Wait 1 second
  ↓
Emily sprite appears in mirror (fade in or instant)
  ↓
Wait 0.5 seconds
  ↓
Dialogue sequence (player stops):
  1. "Wait... there's someone in the mirror."
  2. "Emily? But she's... inside the mirror. Inside me."
  3. "She's not behind me. She's IN the reflection. She's part of me."
  4. "I need to break this mirror. I need to face the truth."
  ↓
hasSeenEmilyInMirror = true
  ↓
Player can move again
```

---

### **PHASE 6: MIRROR EXAMINATION (2 minutes)**

```
Click Mirror (after Emily appears)
  ↓
Check prerequisites:
  - All evidence? ✅
  - Has hammer? ✅
  - Seen Emily? ✅
  ↓
First time examining mirror:
  ↓
Long confrontation sequence (11 dialogues):
  - Mirror examination
  - Truth realization
  - Two-way mirror discovery
  - Emily's voice
  - Lisa's response
  - Emily's farewell
  - Prompt to break mirror
  ↓
hasExaminedMirror = true
```

---

### **PHASE 7: BREAK MIRROR QTE (2 minutes)**

```
Click Mirror again
  ↓
Dialogue: "The mirror is cracking. Keep going!"
  ↓
QTE starts:
  - 15 taps
  - 2 minutes total
  - 3 seconds per tap
  - 3 failures = game over
  ↓
Mirror phases:
  - Taps 0-3: Phase 1 (clean)
  - Taps 4-7: Phase 2 (first cracks)
  - Taps 8-11: Phase 3 (more cracks)
  - Taps 12-15: Phase 4 (almost shattered)
  ↓
Success:
  - Mirror shatters
  - Passage revealed
  - Emily humming stops
  ↓
Failure (3 misses or timeout):
  - Game over
  - Return to checkpoint
```

---

### **PHASE 8: ESCAPE (30 seconds)**

```
Mirror shattered
  ↓
Passage revealed (becomes active)
  ↓
Dialogue:
  1. "A passage. Behind the mirror."
  2. "A narrow crawlspace leading to... Mother's bedroom."
  3. "There's one more door. One more room."
  4. "Mother's bedroom. Where it all ended..."
  ↓
Click Passage
  ↓
Dialogue: "I can climb through. To the Master Bathroom."
  ↓
Scene transition → Room 09 (Master Bathroom)
```

---

## 🎮 PREREQUISITES SYSTEM

### **Try Mirror Before Ready:**

#### **No Evidence:**
```
"I should examine all the evidence first. There's more to find."
```

#### **No Hammer:**
```
"I need something to break the mirror. The hammer from the medicine cabinet."
```

#### **Haven't Seen Emily:**
```
"I should look around more. Something feels... different."
```

#### **Ready:**
```
All checks pass → Show confrontation sequence
```

---

## 📊 FLOWCONTROLLER FLAGS

```csharp
// Story
bool isIntroDone

// Environmental
bool hasCheckedBathtub
bool hasCheckedMedicine

// Evidence (3 items)
bool hasFoundBandages
bool hasFoundTornClothes
bool hasFoundApologyNote

// Hammer
bool hasFoundHammer

// Emily
bool hasSeenEmilyInMirror
GameObject emilyInMirror (sprite in mirror)

// Mirror
bool hasExaminedMirror
bool hasBrokenMirror
bool canClimbThrough
```

---

## 🎨 EMILY IN MIRROR SETUP

### **GameObject:**
```
Name: Emily_In_Mirror
Parent: Mirror or Canvas
Position: Inside mirror bounds
Layer: UI or same as mirror

Components:
├─ SpriteRenderer (Emily sprite, semi-transparent)
│   ├─ Sprite: Emily ghost sprite
│   ├─ Color: White with alpha 0.5-0.7
│   └─ Sorting Layer: Above mirror
│
└─ Initially: SetActive(false)
```

### **Appearance Effect (Optional):**
```csharp
// Fade in
IEnumerator FadeInEmily()
{
    SpriteRenderer sr = emilyInMirror.GetComponent<SpriteRenderer>();
    Color c = sr.color;
    c.a = 0;
    sr.color = c;
    
    emilyInMirror.SetActive(true);
    
    float elapsed = 0f;
    while (elapsed < 1f)
    {
        elapsed += Time.deltaTime;
        c.a = Mathf.Lerp(0, 0.7f, elapsed);
        sr.color = c;
        yield return null;
    }
}
```

---

## 🔧 SETUP CHECKLIST

### **GameObjects:**
- [ ] Bathtub (Interactable)
- [ ] MedicineCabinet (Interactable)
- [ ] Bandages (Evidence, ID: "bandages")
- [ ] TornClothes (Evidence, ID: "torn_clothes")
- [ ] ApologyNote (Evidence, ID: "apology_note")
- [ ] Hammer (Hammer type)
- [ ] Emily_In_Mirror (Sprite, initially inactive)
- [ ] Mirror (Interactable)
- [ ] Passage (Interactable, initially inactive)

### **FlowController:**
- [ ] Emily In Mirror assigned
- [ ] All flags initialized to false

### **ItemDatabase:**
- [ ] Hammer entry added (ID: "hammer")

### **Testing:**
- [ ] Collect all evidence → Individual dialogues
- [ ] Collect hammer → Notification + dialogue
- [ ] All collected → Emily appears automatically
- [ ] Emily appears → Dialogue sequence
- [ ] Try mirror before ready → Correct message
- [ ] Try mirror after ready → Confrontation
- [ ] Break mirror → QTE works
- [ ] Success → Passage revealed

---

## 💡 TIPS

### **Evidence Order:**
- Player can collect in any order
- Each has unique dialogue
- All 3 required before Emily appears

### **Hammer Timing:**
- Can be collected anytime
- Notification shows first
- Dialogue shows after notification (2s delay)

### **Emily Appearance:**
- Automatic when all evidence + hammer collected
- Can trigger from any evidence/hammer pickup
- Only happens once

### **Mirror Interaction:**
- Different messages based on what's missing
- Guides player to collect everything
- Clear progression

---

## 🎉 SUMMARY

### **Key Changes:**
1. ✅ 3 evidence items with individual dialogues
2. ✅ Hammer pickup with notification
3. ✅ Emily appears in mirror automatically
4. ✅ Prerequisites system for mirror
5. ✅ Clear progression flow

### **Player Experience:**
```
Explore → Collect Evidence → Get Hammer → Emily Appears → Confront Mirror → Break Mirror → Escape
```

**EVERYTHING IS CONNECTED!** 🎮✨
