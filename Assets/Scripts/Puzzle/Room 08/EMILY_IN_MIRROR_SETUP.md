# 👻 EMILY IN MIRROR - SETUP GUIDE

## 🎨 VISUAL REFERENCE

Based on your image, Emily appears as a semi-transparent ghost inside the mirror!

---

## 🔧 SETUP

### **1. Create Emily Sprite GameObject**

```
Name: Emily_In_Mirror
Parent: Mirror GameObject (or Canvas if UI)
Position: Inside mirror bounds (center of mirror)
```

---

### **2. Add SpriteRenderer Component**

```
Component: SpriteRenderer

Settings:
├─ Sprite: [Emily ghost sprite]
├─ Color: White (255, 255, 255, 180) ← Semi-transparent!
├─ Sorting Layer: Above mirror
├─ Order in Layer: 1 (above mirror sprite)
└─ Flip X/Y: As needed to face forward
```

---

### **3. Position & Scale**

```
Transform:
├─ Position: Center of mirror
│   Example: (0, 2, 0) if mirror is at (0, 2, 0)
│
├─ Scale: Adjust to fit mirror
│   Example: (0.8, 0.8, 1) for smaller appearance
│
└─ Rotation: (0, 0, 0)
```

---

### **4. Initial State**

```
GameObject:
└─ Active: FALSE ← Very important!
```

**Emily should be HIDDEN at start!**

---

### **5. Assign to FlowController**

```
Room08_FlowController Inspector:
└─ Emily In Mirror: [Drag Emily_In_Mirror GameObject here]
```

---

## 🎨 SPRITE SETUP

### **Option A: Use Existing Emily Sprite**
```
Use the same Emily sprite from other rooms
Add semi-transparency (alpha 0.5-0.7)
Position inside mirror bounds
```

### **Option B: Create Ghost Version**
```
Duplicate Emily sprite
Add ghostly effects:
├─ Lower opacity (50-70%)
├─ Slight blur (optional)
├─ Bluish tint (optional)
└─ Ethereal glow (optional)
```

---

## 📊 VISUAL LAYOUT

```
┌─────────────────────────┐
│                         │
│    BATHROOM MIRROR      │
│   ┌─────────────────┐   │
│   │                 │   │
│   │    👻 EMILY     │   │ ← Emily appears here!
│   │  (transparent)  │   │
│   │                 │   │
│   └─────────────────┘   │
│                         │
│         PLAYER          │
│           🧍            │
│                         │
└─────────────────────────┘
```

---

## 🎭 APPEARANCE EFFECT

### **Option 1: Instant (Simple)**
```csharp
// In Room08_FlowController.cs
if (emilyInMirror != null)
{
    emilyInMirror.SetActive(true);
}
```

### **Option 2: Fade In (Better)**
```csharp
System.Collections.IEnumerator FadeInEmily()
{
    if (emilyInMirror == null) yield break;
    
    SpriteRenderer sr = emilyInMirror.GetComponent<SpriteRenderer>();
    if (sr == null) yield break;
    
    // Start invisible
    Color c = sr.color;
    c.a = 0;
    sr.color = c;
    
    // Show GameObject
    emilyInMirror.SetActive(true);
    
    // Fade in over 1 second
    float elapsed = 0f;
    float duration = 1f;
    
    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        c.a = Mathf.Lerp(0, 0.7f, elapsed / duration);
        sr.color = c;
        yield return null;
    }
    
    // Ensure final alpha
    c.a = 0.7f;
    sr.color = c;
}
```

**To use fade in, replace in `ShowEmilyInMirror()`:**
```csharp
// OLD:
if (emilyInMirror != null)
{
    emilyInMirror.SetActive(true);
}

// NEW:
yield return StartCoroutine(FadeInEmily());
```

---

## 🎮 TRIGGER LOGIC

### **When Emily Appears:**
```
All evidence collected (3 items)
  AND
Hammer collected
  ↓
Automatic trigger
  ↓
Dialogue: "I've found everything..."
  ↓
Emily appears in mirror
  ↓
Dialogue sequence (4 parts)
```

### **Code Flow:**
```csharp
// In Room08_Interactable.cs
void ExamineEvidence()
{
    // ... collect evidence ...
    
    // Check if all collected
    if (flow.IsAllEvidenceFound() && 
        flow.hasFoundHammer && 
        !flow.hasSeenEmilyInMirror)
    {
        flow.OnAllEvidenceCollected(); // ← Triggers Emily appearance
    }
}

void PickupHammer()
{
    // ... pickup hammer ...
    
    // Check if all collected
    if (flow.IsAllEvidenceFound() && 
        !flow.hasSeenEmilyInMirror)
    {
        flow.OnAllEvidenceCollected(); // ← Triggers Emily appearance
    }
}
```

---

## 🎨 VISUAL VARIATIONS

### **Style 1: Ghostly (Recommended)**
```
Color: White with alpha 0.5-0.7
Effect: Semi-transparent, ethereal
Mood: Mysterious, unsettling
```

### **Style 2: Reflection**
```
Color: Slightly blue-tinted
Effect: Mirror-like, distorted
Mood: Uncanny, surreal
```

### **Style 3: Shadow**
```
Color: Dark with low alpha
Effect: Silhouette, ominous
Mood: Threatening, dark
```

---

## 🔊 AUDIO (Optional)

Add sound when Emily appears:

```csharp
System.Collections.IEnumerator ShowEmilyInMirror()
{
    // ... dialogue ...
    
    // Play eerie sound
    AudioManager.Instance?.PlaySFX(emilyAppearsSound);
    
    // Show Emily
    if (emilyInMirror != null)
    {
        emilyInMirror.SetActive(true);
    }
    
    // ... rest of sequence ...
}
```

**Sound suggestions:**
- Whisper sound
- Glass chime
- Eerie ambience
- Heartbeat

---

## ✅ TESTING CHECKLIST

### **Setup:**
- [ ] Emily_In_Mirror GameObject created
- [ ] SpriteRenderer added with Emily sprite
- [ ] Color set to semi-transparent (alpha 0.5-0.7)
- [ ] Position inside mirror bounds
- [ ] Initially SetActive(false)
- [ ] Assigned to Room08_FlowController

### **Functionality:**
- [ ] Collect all 3 evidence items
- [ ] Collect hammer
- [ ] Emily appears automatically
- [ ] Dialogue plays
- [ ] Emily visible in mirror
- [ ] Can proceed to mirror examination

### **Visual:**
- [ ] Emily sprite visible
- [ ] Semi-transparent (ghostly)
- [ ] Positioned correctly in mirror
- [ ] Doesn't block player view
- [ ] Looks natural/creepy

---

## 🐛 TROUBLESHOOTING

### **Emily doesn't appear**
**Check:**
- [ ] All 3 evidence collected
- [ ] Hammer collected
- [ ] `emilyInMirror` assigned in FlowController
- [ ] GameObject initially inactive
- [ ] `OnAllEvidenceCollected()` being called

### **Emily appears too early**
**Check:**
- [ ] `hasSeenEmilyInMirror` flag working
- [ ] Only triggers once
- [ ] All prerequisites checked

### **Emily not visible**
**Check:**
- [ ] SpriteRenderer has sprite assigned
- [ ] Alpha not 0 (should be 0.5-0.7)
- [ ] Sorting layer above mirror
- [ ] GameObject active after trigger

### **Emily positioned wrong**
**Check:**
- [ ] Position matches mirror center
- [ ] Scale appropriate for mirror size
- [ ] Not behind mirror sprite

---

## 💡 TIPS

### **Positioning:**
- Place Emily in center of mirror
- Adjust scale to fit mirror size
- Make sure she's visible but not overwhelming

### **Transparency:**
- 0.5 alpha = Very ghostly
- 0.7 alpha = Clearly visible but ethereal
- 0.3 alpha = Almost invisible (too subtle)

### **Timing:**
- Appears immediately after all evidence + hammer
- Can trigger from any evidence/hammer pickup
- Only happens once per playthrough

### **Effect:**
- Instant appearance = Sudden, shocking
- Fade in = Gradual, eerie
- Choose based on desired mood

---

## 🎬 SEQUENCE EXAMPLE

```
Player collects last evidence item
  ↓
Dialogue: "I've found everything..."
  ↓
Wait 1 second
  ↓
Emily fades in (or appears instantly)
  ↓
Wait 0.5 seconds
  ↓
Dialogue 1: "Wait... there's someone in the mirror."
  ↓
Player clicks
  ↓
Dialogue 2: "Emily? But she's... inside the mirror."
  ↓
Player clicks
  ↓
Dialogue 3: "She's not behind me. She's IN the reflection."
  ↓
Player clicks
  ↓
Dialogue 4: "I need to break this mirror. I need to face the truth."
  ↓
Player can move
  ↓
Emily remains visible in mirror
```

---

## 📝 SUMMARY

### **GameObject:**
```
Emily_In_Mirror
├─ SpriteRenderer (Emily sprite, alpha 0.5-0.7)
├─ Position: Inside mirror
├─ Initially: Inactive
└─ Assigned to: Room08_FlowController
```

### **Trigger:**
```
All evidence + hammer collected
  ↓
Automatic appearance
  ↓
Dialogue sequence
  ↓
Remains visible
```

### **Effect:**
```
Semi-transparent ghost
Inside mirror
Mysterious and unsettling
Drives story forward
```

---

**READY TO HAUNT!** 👻✨
