# Room 08 - Lisa's Bathroom (Final Setup)

## 🎯 COMPLETE FLOW

1. **Collect 2 Evidence Items**:
   - Torn Dress (pickup with notification)
   - Note (pickup with notification)

2. **Get Hammer from Cabinet**:
   - Interact with cabinet
   - Hammer obtained (with notification)

3. **Interact with Bathtub**:
   - Examine bathtub
   - Show dialogue

4. **Interact with Mirror**:
   - Panel appears (like photo frame)
   - Tap puzzle: 15 taps, 25 seconds
   - Panel auto-closes

5. **Mirror Breaks**:
   - World mirror sprite → Broken sprite
   - Passage appears

6. **Interact with Passage**:
   - Transition to Master's Bathroom (Room 09)

---

## 📋 EVIDENCE ITEMS (2 Total)

### Evidence 1: Torn Dress
```
GameObject: Evidence_TornDress
├─ SpriteRenderer (torn dress sprite)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_EvidencePickup (Script)
    ├─ Evidence Id: "torn_dress"
    ├─ Evidence Name: "Torn Dress"
    ├─ Evidence Description: "A torn and bloodied dress. Someone was hurt here."
    ├─ Auto Pickup: ☑
    └─ Pickup Sound: [assign]
```

### Evidence 2: Note
```
GameObject: Evidence_Note
├─ SpriteRenderer (note sprite)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_EvidencePickup (Script)
    ├─ Evidence Id: "apology_note"
    ├─ Evidence Name: "Apology Note"
    ├─ Evidence Description: "A crumpled note. 'I'm sorry... I didn't mean to...'"
    ├─ Auto Pickup: ☑
    └─ Pickup Sound: [assign]
```

---

## 🔧 CABINET INTERACTION

### Medicine Cabinet
```
GameObject: MedicineCabinet
├─ SpriteRenderer (cabinet sprite)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: MedicineCabinet
```

**Flow**:
- **Before evidence collected**: "I should collect the evidence first."
- **After evidence collected**: Opens cabinet, gives hammer with notification

---

## 🛁 BATHTUB INTERACTION

### Bathtub
```
GameObject: Bathtub
├─ SpriteRenderer (bathtub sprite)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: Bathtub
```

**Flow**:
- **Before hammer obtained**: "I should look around more..."
- **After hammer obtained**: Shows bathtub dialogue

---

## 🪞 MIRROR PANEL SYSTEM

### Mirror (World Object)
```
GameObject: Mirror
├─ SpriteRenderer (normal mirror sprite)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: Mirror
```

### Mirror Panel (UI)
```
Canvas
└─ MirrorPanel (GameObject)
    ├─ Initially: SetActive(false)
    ├─ Panel (Image) - Black background, alpha 0.8
    │
    ├─ TapArea (Image)
    │   ├─ Size: 800x600 (or appropriate)
    │   ├─ Color: (0.8, 0.2, 0.2, 0.5) - Red-ish
    │   └─ FillImage (Image, child)
    │       ├─ Image Type: Filled
    │       ├─ Fill Method: Horizontal or Radial
    │       └─ Fill Amount: 0 → 1
    │
    ├─ MirrorImage (Image)
    │   ├─ Size: 400x600
    │   └─ Sprite: Mirror phase 1
    │
    ├─ TimerText (Text/TMP)
    │   └─ Text: "25.0s"
    │
    ├─ ProgressText (Text/TMP)
    │   └─ Text: "0/15"
    │
    └─ Room08_MirrorQTE (Script)
        ├─ Total Taps: 15
        ├─ Total Time Limit: 25
        ├─ Full Screen Tap Area: [assign TapArea]
        ├─ Fill Image: [assign FillImage]
        ├─ Fill Color: (0.8, 0.2, 0.2, 0.5)
        ├─ Timer Text / TMP: [assign]
        ├─ Progress Text / TMP: [assign]
        ├─ Mirror Image: [assign]
        ├─ Mirror Phase 1-4: [assign sprites]
        ├─ Tap Sound: [assign]
        ├─ Crack Sound: [assign]
        └─ Shatter Sound: [assign]
```

---

## 🚪 PASSAGE TO ROOM 09

### Passage (World Object)
```
GameObject: Passage
├─ Initially: SetActive(false)
├─ SpriteRenderer (optional)
├─ BoxCollider2D (Is Trigger ✓)
└─ Room08_Interactable (Script)
    └─ Object Type: Passage
```

**Flow**:
- Hidden until mirror breaks
- Appears after mirror puzzle complete
- Interact → Transition to Room 09

---

## 🎮 MANAGERS

### Room08_FlowController
```
GameObject: Room08_FlowController
└─ Room08_FlowController (Script)
    ├─ Total Evidence Items: 2
    ├─ Mirror Sprite Renderer: [assign Mirror's SpriteRenderer]
    ├─ Mirror Normal Sprite: [assign normal sprite]
    ├─ Mirror Broken Sprite: [assign broken sprite]
    ├─ Passage Object: [assign Passage GameObject]
    └─ Next Scene Name: "Room09_Master's_Bathroom"
```

### Room08UIManager
```
GameObject: Room08UIManager
└─ Room08UIManager (Script)
    └─ Mirror Panel: [assign MirrorPanel]
```

---

## ✅ QUICK CHECKLIST

### Evidence Items:
- [ ] Evidence_TornDress created with Room08_EvidencePickup
- [ ] Evidence_Note created with Room08_EvidencePickup
- [ ] Both have BoxCollider2D (Is Trigger ✓)
- [ ] Both have pickup sounds assigned

### Cabinet:
- [ ] MedicineCabinet created with Room08_Interactable
- [ ] Object Type set to MedicineCabinet
- [ ] BoxCollider2D (Is Trigger ✓)

### Bathtub:
- [ ] Bathtub created with Room08_Interactable
- [ ] Object Type set to Bathtub
- [ ] BoxCollider2D (Is Trigger ✓)

### Mirror:
- [ ] Mirror (world) created with Room08_Interactable
- [ ] Object Type set to Mirror
- [ ] BoxCollider2D (Is Trigger ✓)
- [ ] MirrorPanel created under Canvas
- [ ] Room08_MirrorQTE configured with all UI elements
- [ ] Mirror sprites assigned (normal + broken + phases)

### Passage:
- [ ] Passage created with Room08_Interactable
- [ ] Object Type set to Passage
- [ ] Initially SetActive(false)
- [ ] BoxCollider2D (Is Trigger ✓)

### Managers:
- [ ] Room08_FlowController created and configured
- [ ] Room08UIManager created and configured
- [ ] Total Evidence Items set to 2

---

## 🔄 INTERACTION ORDER

```
1. Collect Torn Dress → Notification
2. Collect Note → Notification
   └─ "All evidence collected" dialogue

3. Interact with Cabinet → Get Hammer
   └─ Hammer notification

4. Interact with Bathtub → Dialogue
   └─ Bathtub examined

5. Interact with Mirror → Panel appears
   └─ Tap puzzle (15 taps, 25 seconds)
   └─ Panel closes
   └─ Mirror breaks (sprite changes)
   └─ Passage appears

6. Interact with Passage → Go to Room 09
```

---

## 🎨 RECOMMENDED COLORS

**Mirror Panel Fill**:
- **Red**: `(0.8, 0.2, 0.2, 0.5)` ← Recommended (urgent, aggressive)
- **Purple**: `(0.6, 0.2, 0.8, 0.5)` (mysterious, eerie)
- **Blue**: `(0.2, 0.4, 0.8, 0.5)` (calm, focused)

---

## 📝 SCENE HIERARCHY

```
Room08_Lisa'sBathroom (Scene)
├─ Room08_FlowController
├─ Room08UIManager
├─ Evidence_TornDress
├─ Evidence_Note
├─ MedicineCabinet
├─ Bathtub
├─ Mirror
├─ Passage (hidden)
└─ Canvas
    └─ MirrorPanel (hidden)
        └─ Room08_MirrorQTE
```

---

**Setup complete! 2 evidence items + hammer + bathtub + mirror!** 🎮✨

