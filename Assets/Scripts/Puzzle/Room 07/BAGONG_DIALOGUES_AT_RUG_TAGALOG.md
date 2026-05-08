# Bagong Dialogues at Rug Transition (Tagalog Guide)

## 🎯 Ano ang Bago?

### 1. **Mas Magandang Dialogues**
Lahat ng dialogues ay pinalitan para gumawa ng emotional story na nag-bubuild habang naglalaro.

### 2. **Rug Transition**
Bagong script para sa rug - pwede lang gamitin AFTER ng mirror interaction para pumunta sa next room.

---

## 📖 Halimbawa ng Bagong Dialogues

### Dati (Simple):
```
"Child's bed has two pillow indentations..."
```

### Ngayon (Story-Driven):
```
"A child's bed... with two pillows. There's a note pinned to the second one. 
'For my friend Emily - she keeps me safe at night.' 
Emily... why does that name make my heart ache?"
```

**Mas emotional, mas may story!** ✨

---

## 🪜 Paano I-setup ang Rug

### Step 1: Create Rug GameObject

```
1. Sa Room 07 scene, create GameObject: "Rug"
2. Add Sprite Renderer (rug sprite mo)
3. Add Box Collider 2D
4. Add Component: Room07_RugTransition
```

### Step 2: I-configure ang Script

```
Inspector → Room07_RugTransition:

Next Scene Name: "Room08_Lisa'sBathroom"
  ↑ Palitan mo ng name ng next room mo

Transition Delay: 1
  ↑ Seconds bago mag-load ng next scene

Rug Move Sound: (optional sound effect)
Trapdoor Open Sound: (optional sound effect)
```

### Step 3: I-position ang Rug

```
Ilagay ang rug kung saan mo gusto ang exit.
Example: gitna ng room, malapit sa pinto, etc.
```

---

## 🎮 Paano Gumagana ang Rug

### BEFORE Mirror Interaction:
```
Player: *interact with rug*
Lisa: "The rug... there's something underneath it. 
       But I can't move it yet. I need to face Emily first."
Result: Hindi pa pwede lumipat ng room
```

### AFTER Mirror Interaction:
```
Player: *interact with rug*
Lisa: "The rug... I can move it now. There's a trapdoor underneath..."
  ↓
Rug moves (may animation kung meron)
  ↓
Trapdoor opens (may sound)
  ↓
Lisa: "The trapdoor opens to darkness below. Another room. Another memory. 
       I'm sorry, Emily. I have to know the truth."
  ↓
Fade out
  ↓
Load next scene! ✓
```

---

## ✅ Requirements para Gumana ang Rug

Kailangan COMPLETE lahat:
- ✅ Bed, Wall, Diary, Chair, Closet, Reading Table
- ✅ Curtains, Tea Party, Toybox, Dollhouse
- ✅ Mirror interaction
- ✅ `hasInteractedWithMirror = true`

**Kung hindi complete, hindi gumagana ang rug!**

---

## 📋 Complete Sequence

```
1. Enter Room → Intro dialogue
2. Bed → Wall → Diary
3. Curtains → Cabinet → Tea Party
4. Chair → Closet
5. Toybox → Get Doll
6. Dollhouse
7. Reading Table
8. Mirror (jumpscare)
9. Rug (transition to next room) ← BAGO!
```

---

## 🎬 Emotional Story Progression

### Simula (Intro):
> "This room... it feels so familiar..."

### Gitna (Discoveries):
> "Emily was always there when I needed her..."
> "She made the pain go away. She made me feel... safe."

### Climax (Mirror):
> "I've remembered everything. I need to see the truth."

### Ending (Rug):
> "Leaving this room means leaving Emily behind. 
> ...I have to go. I have to understand what happened to us."

**Complete emotional journey!** 💔✨

---

## 🧪 Paano I-test

### Test 1: Rug Before Mirror
```
1. Complete lahat EXCEPT mirror
2. Try rug
3. Dapat: "I need to face Emily first"
4. Hindi pa pwede lumipat ✓
```

### Test 2: Rug After Mirror
```
1. Complete lahat including mirror
2. Interact with rug
3. Dapat: Transition dialogue
4. Rug moves
5. Next scene loads ✓
```

---

## 📝 Files na Ginawa/Na-update

### Bagong Files:
1. **Room07_ImprovedDialogues.cs** - Lahat ng bagong dialogues
2. **Room07_RugTransition.cs** - Rug script
3. **IMPROVED_DIALOGUES_SETUP.md** - Complete guide (English)
4. **BAGONG_DIALOGUES_AT_RUG_TAGALOG.md** - This guide

### Na-update na Files:
1. **Room07_Interactable.cs** - Gumagamit ng bagong dialogues
2. **Room07_FlowController.cs** - May bagong flag: `hasInteractedWithMirror`
3. **Room07UIManager.cs** - Updated dialogues
4. **CabinetItemPanel.cs** - Updated cup dialogue

---

## 💡 Important Notes

### Dialogues:
- ✅ Mas emotional at story-driven
- ✅ Nag-bubuild ang story habang naglalaro
- ✅ Reveals Lisa's past gradually
- ✅ Creates connection sa characters

### Rug:
- ✅ Only works after EVERYTHING complete
- ✅ Smooth transition with dialogue
- ✅ Loads next room automatically
- ✅ Natural ending point for Room 07

---

## 🎯 Next Steps

### 1. Test ang Bagong Dialogues
```
Play through at basahin lahat ng dialogues.
Check kung natural at emotional ang flow.
```

### 2. Setup ang Rug
```
Create rug GameObject
Add Room07_RugTransition script
Set next scene name
```

### 3. Create Next Room
```
Make sure may "Room08_Lisa'sBathroom" scene
Add sa Build Settings
```

### 4. Add Sounds (Optional)
```
Rug move sound
Trapdoor open sound
Para mas immersive
```

---

## 🎮 Ano ang Makikita ng Player

**Dati:**
- Simple dialogues
- Walang emotional connection
- Unclear story
- Biglang ending

**Ngayon:**
- Complete emotional story ✓
- Clear character development ✓
- Natural story progression ✓
- Smooth transition to next room ✓

---

**Lahat ng dialogues ay nag-tetell ng complete story about Lisa and Emily!** 📖✨

**Ang rug ay natural na transition point para sa next room!** 🪜🚪

**Test mo na at enjoy the improved story!** 🎮💖

