# 🎨 ROOM 08 - VISUAL SETUP GUIDE

## 🗺️ SCENE LAYOUT

```
┌─────────────────────────────────────────┐
│         LISA'S BATHROOM                 │
│                                         │
│  ┌──────┐              ┌─────────┐    │
│  │ Door │              │ Medicine│    │
│  │  🚪  │              │ Cabinet │    │
│  └──────┘              └─────────┘    │
│                                         │
│                        📝 Apology Note  │
│                                         │
│  ┌──────────────┐      ┌──────────┐   │
│  │   Bathtub    │      │  Mirror  │   │
│  │     🛁       │      │    🪞    │   │
│  └──────────────┘      └──────────┘   │
│                                         │
│  🩹 Bandages                            │
│  👕 Torn Clothes                        │
│                                         │
│  [Passage behind mirror - hidden]      │
│                                         │
└─────────────────────────────────────────┘

Emily is OUTSIDE, humming 🎵
```

---

## 📊 FLOW DIAGRAM

```
START
  ↓
┌─────────────────────┐
│  ENTRY SEQUENCE     │
│  - Lisa enters      │
│  - Door locks       │
│  - Emily humming    │
│  - 4 dialogues      │
└─────────────────────┘
  ↓
┌─────────────────────┐
│  EXAMINE EVIDENCE   │
│  - Bathtub          │
│  - Medicine Cabinet │
│  - Bandages         │
│  - Torn Clothes     │
│  - Apology Note     │
└─────────────────────┘
  ↓
┌─────────────────────┐
│  EXAMINE MIRROR     │
│  - First time only  │
│  - Long sequence    │
│  - Emily reveal     │
│  - 11 dialogues     │
└─────────────────────┘
  ↓
┌─────────────────────┐
│  BREAK MIRROR (QTE) │
│  - 5 taps           │
│  - Decreasing time  │
│  - 3 failures = ❌  │
└─────────────────────┘
  ↓
┌─────────────────────┐
│  ESCAPE             │
│  - Passage revealed │
│  - Climb through    │
│  - Next scene       │
└─────────────────────┘
  ↓
END (Master Bathroom)
```

---

## 🎯 QTE VISUAL BREAKDOWN

### **QTE Panel Layout**

```
┌─────────────────────────────────────────┐
│                                         │
│           ⏱️ TIMER: 2.00                │
│                                         │
│                                         │
│         ┌─────────────┐                │
│         │             │                │
│         │   MIRROR    │                │
│         │   🪞        │                │
│         │             │                │
│         │   🎯 TAP    │  ← Target      │
│         │   TARGET    │     spawns     │
│         │             │     here       │
│         └─────────────┘                │
│                                         │
│                                         │
│          📊 PROGRESS: 3/5               │
│                                         │
└─────────────────────────────────────────┘
```

### **Mirror Crack Progression**

```
Tap 1:  🪞 → 🪞 (small crack)
Tap 2:  🪞 → 🪞 (more cracks)
Tap 3:  🪞 → 🪞 (even more)
Tap 4:  🪞 → 🪞 (almost shattered)
Tap 5:  🪞 → 💥 (SHATTER!)
```

---

## 🎮 GAMEOBJECT HIERARCHY

```
Room08_Scene
├── Room08_FlowController (Script)
│   └── AudioSource (Emily humming)
│
├── Room08_MirrorQTE (Script)
│
├── Interactables
│   ├── Bathtub (Room08_Interactable)
│   │   └── Collider2D
│   │
│   ├── MedicineCabinet (Room08_Interactable)
│   │   └── Collider2D
│   │
│   ├── Mirror (Room08_Interactable)
│   │   ├── SpriteRenderer
│   │   └── Collider2D
│   │
│   ├── Door (Room08_Interactable)
│   │   └── Collider2D
│   │
│   └── Passage (Room08_Interactable) [INACTIVE]
│       └── Collider2D
│
├── Evidence
│   ├── Bandages (Room08_Interactable)
│   │   ├── SpriteRenderer
│   │   └── Collider2D
│   │
│   ├── TornClothes (Room08_Interactable)
│   │   ├── SpriteRenderer
│   │   └── Collider2D
│   │
│   └── ApologyNote (Room08_Interactable)
│       ├── SpriteRenderer
│       └── Collider2D
│
└── Canvas
    ├── GraphicRaycaster
    │
    └── QTE_Panel (Panel) [INACTIVE]
        ├── Mirror_Image (Image)
        ├── Tap_Target_Parent (RectTransform)
        ├── Timer_Text (Text)
        ├── Progress_Text (Text)
        └── Shatter_Effect (ParticleSystem) [INACTIVE]
```

---

## 🎨 SPRITE REQUIREMENTS

### **Mirror Sprites (6 total)**

```
1. Mirror_Normal
   ┌─────────┐
   │         │
   │  Clean  │
   │  Mirror │
   │         │
   └─────────┘

2. Mirror_Crack_1
   ┌─────────┐
   │    /    │
   │  Small  │
   │  Crack  │
   │         │
   └─────────┘

3. Mirror_Crack_2
   ┌─────────┐
   │  / \    │
   │  More   │
   │  Cracks │
   │         │
   └─────────┘

4. Mirror_Crack_3
   ┌─────────┐
   │ /|\ /   │
   │  Even   │
   │  More   │
   │         │
   └─────────┘

5. Mirror_Crack_4
   ┌─────────┐
   │/|\|\/\  │
   │ Almost  │
   │Shattered│
   │         │
   └─────────┘

6. Mirror_Crack_5
   ┌─────────┐
   │/|\|\/\|/│
   │ Heavily │
   │ Cracked │
   │         │
   └─────────┘
```

### **Evidence Sprites**

```
🩹 Bandages      - Medical bandages sprite
👕 Torn Clothes  - Ripped clothing sprite
📝 Apology Note  - Handwritten note sprite
```

### **UI Sprites**

```
🎯 Tap Target    - White circle (100x100)
```

---

## 🔊 AUDIO REQUIREMENTS

### **Ambient**
```
🎵 Emily Humming - Looping, eerie humming sound
   - Duration: 10-30 seconds (loops)
   - Volume: 0.3-0.5
   - Mood: Unsettling, waiting
```

### **QTE Sounds**
```
🔊 Tap Sound     - Click/tap (0.1s)
🔊 Crack Sound   - Glass crack (0.3s)
🔊 Shatter Sound - Glass shatter (1.0s)
🔊 Fail Sound    - Error beep (0.2s)

🔊 Glass Stress Sounds (5 clips):
   1. Light creaking   (0.5s)
   2. Medium creaking  (0.5s)
   3. Louder creaking  (0.5s)
   4. Very loud        (0.5s)
   5. Almost breaking  (0.5s)
```

---

## 📋 INSPECTOR REFERENCE

### **Room08_FlowController**
```
┌─────────────────────────────────────┐
│ Room08_FlowController               │
├─────────────────────────────────────┤
│ Story Milestones                    │
│   Is Intro Done: ☐                  │
│                                     │
│ Environmental Checks                │
│   Has Checked Bathtub: ☐            │
│   Has Checked Medicine: ☐           │
│   Has Found Evidence: ☐             │
│                                     │
│ Mirror Progress                     │
│   Has Examined Mirror: ☐            │
│   Has Broken Mirror: ☐              │
│   Can Climb Through: ☐              │
│                                     │
│ Emily AI                            │
│   Emily AI: [None]                  │
│   Emily Humming Sound: [AudioClip]  │
│   Emily Audio Source: [AudioSource] │
│                                     │
│ Door                                │
│   Bathroom Door: [GameObject]       │
│   Is Door Locked: ☑                 │
│                                     │
│ Scene Transition                    │
│   Next Scene Name: Room09_Master's  │
└─────────────────────────────────────┘
```

### **Room08_MirrorQTE**
```
┌─────────────────────────────────────┐
│ Room08_MirrorQTE                    │
├─────────────────────────────────────┤
│ QTE Settings                        │
│   Total Taps: 5                     │
│   Starting Time: 2.0                │
│   Minimum Time: 0.8                 │
│   Max Failures: 3                   │
│                                     │
│ UI References                       │
│   QTE Panel: [QTE_Panel]            │
│   Tap Target Prefab: [TapTarget]    │
│   Tap Target Parent: [Transform]    │
│   Timer Text: [Text]                │
│   Progress Text: [Text]             │
│                                     │
│ Visual Effects                      │
│   Mirror Image: [Image]             │
│   Crack Sprites: [5 sprites]        │
│   Shatter Effect: [ParticleSystem]  │
│                                     │
│ Audio                               │
│   Tap Sound: [AudioClip]            │
│   Crack Sound: [AudioClip]          │
│   Shatter Sound: [AudioClip]        │
│   Fail Sound: [AudioClip]           │
│   Glass Stress Sounds: [5 clips]    │
│                                     │
│ Camera Shake                        │
│   Shake Intensity: 0.1              │
│   Shake Duration: 0.2               │
└─────────────────────────────────────┘
```

### **Room08_Interactable (Example: Mirror)**
```
┌─────────────────────────────────────┐
│ Room08_Interactable                 │
├─────────────────────────────────────┤
│ My Type: Mirror                     │
│                                     │
│ Evidence Type                       │
│   Evidence Id: [Empty]              │
└─────────────────────────────────────┘
```

### **Room08_Interactable (Example: Evidence)**
```
┌─────────────────────────────────────┐
│ Room08_Interactable                 │
├─────────────────────────────────────┤
│ My Type: Evidence                   │
│                                     │
│ Evidence Type                       │
│   Evidence Id: "bandages"           │
└─────────────────────────────────────┘
```

---

## ⚡ QUICK SETUP STEPS

### **1. Create Scripts Folder**
```
Assets/Scripts/Puzzle/Room 08/
├── Room08_Dialogues.cs ✅
├── Room08_FlowController.cs ✅
├── Room08_Interactable.cs ✅
└── Room08_MirrorQTE.cs ✅
```

### **2. Create GameObjects**
```
1. Room08_FlowController (Empty)
2. Room08_MirrorQTE (Empty)
3. Bathtub (Sprite + Collider)
4. MedicineCabinet (Sprite + Collider)
5. Mirror (Sprite + Collider)
6. Door (Sprite + Collider)
7. Passage (Sprite + Collider) [INACTIVE]
8. Bandages (Sprite + Collider)
9. TornClothes (Sprite + Collider)
10. ApologyNote (Sprite + Collider)
```

### **3. Create UI**
```
Canvas/QTE_Panel/
├── Mirror_Image
├── Tap_Target_Parent
├── Timer_Text
├── Progress_Text
└── Shatter_Effect [INACTIVE]

Prefabs/UI/
└── TapTarget (prefab)
```

### **4. Assign References**
```
1. Room08_FlowController → Assign all fields
2. Room08_MirrorQTE → Assign all fields
3. Each Interactable → Set Object Type
4. Evidence → Set Evidence ID
```

### **5. Add Assets**
```
1. 6 mirror sprites (normal + 5 cracks)
2. 3 evidence sprites
3. 1 tap target sprite
4. 1 humming audio clip
5. 5 QTE audio clips
```

### **6. Test**
```
1. Entry sequence
2. Evidence examination
3. Mirror examination
4. QTE success
5. QTE failure
6. Escape sequence
```

---

## 🎯 TESTING SCENARIOS

### **Scenario 1: Happy Path**
```
1. Enter room → Intro plays
2. Examine all evidence → Dialogues show
3. Examine mirror → Long sequence
4. Break mirror (QTE) → Success
5. Climb through passage → Next scene
```

### **Scenario 2: QTE Failure**
```
1. Enter room → Intro plays
2. Examine all evidence → Dialogues show
3. Examine mirror → Long sequence
4. Break mirror (QTE) → Miss 3 times
5. Game over → Checkpoint
```

### **Scenario 3: Wrong Order**
```
1. Enter room → Intro plays
2. Try mirror first → "Need evidence" message
3. Examine evidence → Dialogues show
4. Try mirror again → Long sequence
5. Break mirror (QTE) → Success
```

---

## 💡 VISUAL TIPS

### **Mirror Placement**
- Center of room, visible from entrance
- Large enough to be focal point
- Behind it: hidden passage (initially inactive)

### **Evidence Placement**
- Bandages: Near bathtub/sink
- Torn Clothes: Near bathtub
- Apology Note: In/near medicine cabinet

### **UI Design**
- QTE Panel: Full screen, semi-transparent black
- Mirror Image: Center, large (400x600)
- Tap Targets: Random positions, visible
- Timer: Top center, large font
- Progress: Bottom center, medium font

### **Audio Mixing**
- Emily Humming: 30-50% volume, looping
- QTE Sounds: 70-100% volume, clear
- Stress Sounds: Escalating volume (50% → 100%)

---

**READY TO BUILD!** 🎮✨

Use this visual guide alongside the complete guide for easy implementation! 💖
