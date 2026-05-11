# 🌳 ROOM 09 - HIERARCHY STRUCTURE VISUAL GUIDE

## 📊 COMPLETE HIERARCHY LAYOUT

Ito ang visual guide ng hierarchy structure para sa Room 09!

---

## 🎮 SCENE HIERARCHY (Room 09 Scene)

```
Room09_MasterBathroomFinal (Scene)
│
├── 📷 Main Camera (from Persistent Scene)
│
├── 🎨 Canvas
│   ├── Component: Canvas
│   ├── Component: Canvas Scaler
│   └── Component: Graphic Raycaster ⭐ IMPORTANT!
│
├── 🎮 EventSystem
│   ├── Component: EventSystem
│   └── Component: Standalone Input Module
│
├── 🕹️ Player
│   ├── Component: JoystickPlayerController
│   ├── Component: PlayerInteractionController ⭐
│   ├── Component: Collider2D
│   └── Tag: "Player"
│
├── 🕹️ Joystick (Virtual Joystick)
│
├── 🔘 InteractButton (Bottom right)
│
├── 🪞 Mirror1_MedicineCabinet ⭐ PUZZLE 1
│   ├── Component: Sprite Renderer
│   ├── Component: Box Collider 2D (Is Trigger: ✓)
│   ├── Component: Room09_Interactable (Mirror Number: 1)
│   └── Component: Mirror1_MedicineCabinet
│
├── 🪞 Mirror2_BathtubDrain ⭐ PUZZLE 2
│   ├── Component: Sprite Renderer
│   ├── Component: Box Collider 2D (Is Trigger: ✓)
│   ├── Component: Room09_Interactable (Mirror Number: 2)
│   └── Component: Mirror2_BathtubDrain
│
├── 🪞 Mirror3_VanityTerror ⭐ PUZZLE 3
│   ├── Component: Sprite Renderer
│   ├── Component: Box Collider 2D (Is Trigger: ✓)
│   ├── Component: Room09_Interactable (Mirror Number: 3)
│   └── Component: Mirror3_VanityTerror
│
├── 🪞 Mirror4_EvidenceSequence ⭐ PUZZLE 4
│   ├── Component: Sprite Renderer
│   ├── Component: Box Collider 2D (Is Trigger: ✓)
│   ├── Component: Room09_Interactable (Mirror Number: 4)
│   └── Component: Mirror4_EvidenceSequence
│
├── 👻 Emily (Ghost GameObject)
│
├── 🚪 Door (Locked)
│
└── 🎬 Room09_FlowController (Empty GameObject)
    └── Component: Room09_FlowController
```

---

## 🎨 CANVAS HIERARCHY (UI Elements)

```
Canvas
│
├── 📱 MedicineCabinet_Panel ⭐ MIRROR 1 UI
│   ├── Active: ✗ (starts hidden)
│   ├── Anchor: Stretch (full screen)
│   └── Color: Black, Alpha: 200
│
├── 📱 BathtubDrain_Panel ⭐ MIRROR 2 UI
│   ├── Active: ✗ (starts hidden)
│   ├── Anchor: Stretch (full screen)
│   └── Color: Black, Alpha: 200
│
├── 📱 VanityTerror_Panel ⭐ MIRROR 3 UI
│   ├── Active: ✗ (starts hidden)
│   ├── Anchor: Stretch (full screen)
│   └── Color: Black, Alpha: 200
│
├── 📱 EvidenceSequence_Panel ⭐ MIRROR 4 UI
│   ├── Active: ✗ (starts hidden)
│   ├── Anchor: Stretch (full screen)
│   └── Color: Black, Alpha: 200
│
├── 😱 Emily_Jumpscare_Panel
│   ├── Active: ✗ (starts hidden)
│   └── Shows on puzzle failure
│
├── 🕹️ Joystick (if in Canvas)
│
└── 🔘 InteractButton (if in Canvas)
```

---

## 📱 MIRROR 1: MEDICINE CABINET PANEL

```
MedicineCabinet_Panel
│
├── 📝 Title_Text (TextMeshProUGUI)
│   ├── Text: "Medicine Cabinet"
│   ├── Font Size: 48
│   ├── Position: Top center (0, -50)
│   └── Color: White
│
├── ⏱️ Timer_Text (TextMeshProUGUI)
│   ├── Text: "1:00"
│   ├── Font Size: 36
│   ├── Position: Top right (300, -50)
│   └── Color: White
│
├── 📦 Slots_Container (Panel)
│   ├── Component: Horizontal Layout Group
│   ├── Spacing: 20
│   ├── Size: (700, 200)
│   ├── Position: Center (0, 0)
│   │
│   ├── 🔲 Slot_1 (Image)
│   │   ├── Color: Dark Gray (50, 50, 50)
│   │   └── 🏷️ Label (Text): "1"
│   │
│   ├── 🔲 Slot_2 (Image)
│   │   └── 🏷️ Label (Text): "2"
│   │
│   ├── 🔲 Slot_3 (Image)
│   │   └── 🏷️ Label (Text): "3"
│   │
│   ├── 🔲 Slot_4 (Image)
│   │   └── 🏷️ Label (Text): "4"
│   │
│   ├── 🔲 Slot_5 (Image)
│   │   └── 🏷️ Label (Text): "5"
│   │
│   └── 🔲 Slot_6 (Image)
│       └── 🏷️ Label (Text): "6"
│
├── 🍾 Bottle_1973 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "bottle_1973"
│   │   └── Puzzle Number: 1
│   ├── Size: (80, 120)
│   ├── Position: (-200, 100)
│   ├── Raycast Target: ✓
│   └── 🏷️ Year_Label (Text): "1973"
│
├── 🍾 Bottle_1974 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "bottle_1974"
│   │   └── Puzzle Number: 1
│   └── 🏷️ Year_Label (Text): "1974"
│
├── 🍾 Bottle_1975a (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "bottle_1975a"
│   │   └── Puzzle Number: 1
│   └── 🏷️ Year_Label (Text): "1975"
│
├── 🍾 Bottle_1975b (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "bottle_1975b"
│   │   └── Puzzle Number: 1
│   └── 🏷️ Year_Label (Text): "1975"
│
├── 🍾 Bottle_1976a (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "bottle_1976a"
│   │   └── Puzzle Number: 1
│   └── 🏷️ Year_Label (Text): "1976"
│
└── 🍾 Bottle_1976b (Image) ⭐ DRAGGABLE
    ├── Component: DraggableItem
    │   ├── Item Id: "bottle_1976b"
    │   └── Puzzle Number: 1
    └── 🏷️ Year_Label (Text): "1976"
```

---

## 📱 MIRROR 2: BATHTUB DRAIN PANEL

```
BathtubDrain_Panel
│
├── 📝 Title_Text (TextMeshProUGUI)
│   └── Text: "Bathtub"
│
├── ⏱️ Timer_Text (TextMeshProUGUI)
│   └── Text: "1:00"
│
├── 🛁 Bathtub_Image (Image)
│   ├── Size: (400, 300)
│   ├── Position: Upper center (0, 100)
│   │
│   └── 🔘 DrainCover_Button (Button)
│       ├── Size: (80, 80)
│       ├── Position: Center of bathtub
│       └── Text: "Remove Cover"
│
├── 📦 Assembly_Container (Panel)
│   ├── Component: Vertical Layout Group
│   ├── Spacing: 10
│   ├── Size: (600, 250)
│   ├── Position: Lower center (0, -150)
│   │
│   ├── 🔲 Slot_1 (Image)
│   │   └── Size: (550, 50)
│   │
│   ├── 🔲 Slot_2 (Image)
│   │   └── Size: (550, 50)
│   │
│   ├── 🔲 Slot_3 (Image)
│   │   └── Size: (550, 50)
│   │
│   └── 🔲 Slot_4 (Image)
│       └── Size: (550, 50)
│
├── 📄 Note_Piece_1 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "piece1"
│   │   └── Puzzle Number: 2
│   ├── Active: ✗ (shown after drain opened)
│   ├── Size: (500, 45)
│   └── 📝 Text: "Tonight I"
│
├── 📄 Note_Piece_2 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "piece2"
│   │   └── Puzzle Number: 2
│   └── 📝 Text: "end this child's"
│
├── 📄 Note_Piece_3 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "piece3"
│   │   └── Puzzle Number: 2
│   └── 📝 Text: "suffering and"
│
└── 📄 Note_Piece_4 (Image) ⭐ DRAGGABLE
    ├── Component: DraggableItem
    │   ├── Item Id: "piece4"
    │   └── Puzzle Number: 2
    └── 📝 Text: "mine forever"
```

---

## 📱 MIRROR 3: VANITY TERROR PANEL

```
VanityTerror_Panel
│
├── 📝 Title_Text (TextMeshProUGUI)
│   └── Text: "Mother's Diary"
│
├── ⏱️ Timer_Text (TextMeshProUGUI)
│   └── Text: "1:30"
│
├── 📦 Slots_Container (Panel)
│   ├── Component: Grid Layout Group
│   │   ├── Cell Size: (200, 140)
│   │   ├── Spacing: (10, 10)
│   │   └── Constraint: Fixed Column Count = 4
│   ├── Size: (900, 600)
│   ├── Position: Center (0, 0)
│   │
│   ├── 🔲 Slot_1 (Image)
│   │   └── 🏷️ Label: "1"
│   ├── 🔲 Slot_2 (Image)
│   │   └── 🏷️ Label: "2"
│   ├── 🔲 Slot_3 (Image)
│   │   └── 🏷️ Label: "3"
│   ├── 🔲 Slot_4 (Image)
│   │   └── 🏷️ Label: "4"
│   ├── 🔲 Slot_5 (Image)
│   │   └── 🏷️ Label: "5"
│   ├── 🔲 Slot_6 (Image)
│   │   └── 🏷️ Label: "6"
│   ├── 🔲 Slot_7 (Image)
│   │   └── 🏷️ Label: "7"
│   └── 🔲 Slot_8 (Image)
│       └── 🏷️ Label: "8"
│
├── 📖 DiaryPage_1 (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "page1"
│   │   └── Puzzle Number: 3
│   ├── Size: (180, 120)
│   └── 📝 Text: "Child defied me at dinner..."
│
├── 📖 DiaryPage_2 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "The defiance continues..."
│
├── 📖 DiaryPage_3 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "I've increased discipline..."
│
├── 📖 DiaryPage_4 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "Strange things happening..."
│
├── 📖 DiaryPage_5 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "Supernatural events escalated..."
│
├── 📖 DiaryPage_6 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "The presence grows bolder..."
│
├── 📖 DiaryPage_7 (Image) ⭐ DRAGGABLE
│   └── 📝 Text: "I've made my preparations..."
│
└── 📖 DiaryPage_8 (Image) ⭐ DRAGGABLE
    └── 📝 Text: "Everything is ready..."
```

---

## 📱 MIRROR 4: EVIDENCE SEQUENCE PANEL

```
EvidenceSequence_Panel
│
├── 📝 Title_Text (TextMeshProUGUI)
│   └── Text: "The Plan"
│
├── ⏱️ Timer_Text (TextMeshProUGUI)
│   └── Text: "1:00"
│
├── 🪞 Mirror_Image (Image)
│   ├── Size: (400, 500)
│   ├── Position: Upper center (0, 100)
│   │
│   └── 🖼️ Flashback_Image (Image)
│       ├── Size: (300, 300)
│       ├── Position: Center (0, 0)
│       └── Active: ✗ (shown when item placed)
│
├── 📦 Frames_Container (Panel)
│   ├── Component: Horizontal Layout Group
│   ├── Spacing: 15
│   ├── Size: (600, 150)
│   ├── Position: Below mirror (0, -200)
│   │
│   ├── 🖼️ Frame_1 (Image)
│   │   ├── Size: (120, 120)
│   │   └── 🏷️ Label: "1"
│   │
│   ├── 🖼️ Frame_2 (Image)
│   │   ├── Size: (120, 120)
│   │   └── 🏷️ Label: "2"
│   │
│   ├── 🖼️ Frame_3 (Image)
│   │   ├── Size: (120, 120)
│   │   └── 🏷️ Label: "3"
│   │
│   └── 🖼️ Frame_4 (Image)
│       ├── Size: (120, 120)
│       └── 🏷️ Label: "4"
│
├── 🪢 Evidence_Rope (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "rope"
│   │   └── Puzzle Number: 4
│   └── Size: (100, 100)
│
├── 💊 Evidence_Pills (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "pills"
│   │   └── Puzzle Number: 4
│   └── Size: (100, 100)
│
├── 🔪 Evidence_Knife (Image) ⭐ DRAGGABLE
│   ├── Component: DraggableItem
│   │   ├── Item Id: "knife"
│   │   └── Puzzle Number: 4
│   └── Size: (100, 100)
│
└── 🩸 Evidence_Towel (Image) ⭐ DRAGGABLE
    ├── Component: DraggableItem
    │   ├── Item Id: "towel"
    │   └── Puzzle Number: 4
    └── Size: (100, 100)
```

---

## 🎯 KEY POINTS

### **Scene Objects (World Space)**:

```
✅ Mirrors are GameObjects in scene
✅ Have Sprite Renderer
✅ Have Collider2D (Is Trigger: ✓)
✅ Have Room09_Interactable
✅ Have puzzle scripts
```

### **UI Objects (Canvas Space)**:

```
✅ Panels are UI elements in Canvas
✅ All draggable items are UI Images
✅ All slots are UI Images
✅ All text is TextMeshProUGUI
✅ All start inactive (panels)
```

### **Draggable Items**:

```
✅ Must be UI Images
✅ Must have DraggableItem script
✅ Must have Raycast Target ✓
✅ Must be children of Panel (or Canvas)
✅ Must have Item Id and Puzzle Number set
```

---

## 📋 HIERARCHY CHECKLIST

### **Scene Level**:

- [ ] Canvas exists
  - [ ] Has Graphic Raycaster
- [ ] EventSystem exists
- [ ] Player exists
  - [ ] Has PlayerInteractionController
- [ ] 4 mirrors exist
  - [ ] All have colliders (Is Trigger: ✓)
  - [ ] All have Room09_Interactable
  - [ ] All have puzzle scripts

### **Canvas Level**:

- [ ] 4 panels exist
  - [ ] All start inactive
  - [ ] All have titles and timers
- [ ] All slots created
  - [ ] Names contain "Slot" or "Frame"
- [ ] All draggable items created
  - [ ] All have DraggableItem script
  - [ ] All have Raycast Target ✓

---

## 🎨 VISUAL LAYOUT

### **In Game View**:

```
┌─────────────────────────────────────────────┐
│                                             │
│  🪞        🪞        🪞        🪞           │ ← 4 Mirrors
│ Mirror1   Mirror2   Mirror3   Mirror4       │
│                                             │
│                                             │
│              👤 Player                      │
│                                             │
│                                             │
│  🕹️ Joystick              🔘 Interact      │ ← Controls
└─────────────────────────────────────────────┘
```

### **When Panel Opens**:

```
┌─────────────────────────────────────────────┐
│  Medicine Cabinet              1:00         │ ← Title & Timer
│                                             │
│  [1]  [2]  [3]  [4]  [5]  [6]              │ ← Slots
│                                             │
│     🍾      🍾                              │
│                                             │
│  🍾    🍾    🍾    🍾                       │ ← Draggable Items
│                                             │
└─────────────────────────────────────────────┘
```

---

## ✅ SUMMARY

### **Total GameObjects**:

**Scene**:
- 4 mirrors (world space)
- 1 player
- 1 canvas
- 1 event system
- 1 flow controller

**Canvas**:
- 4 panels
- 4 titles
- 4 timers
- 26 slots total (6 + 4 + 8 + 4)
- 22 draggable items (6 + 4 + 8 + 4)

**Total**: ~70 GameObjects

### **Hierarchy Depth**:

```
Scene (Root)
└── Canvas (Level 1)
    └── Panel (Level 2)
        ├── Title (Level 3)
        ├── Timer (Level 3)
        ├── Slots_Container (Level 3)
        │   └── Slot (Level 4)
        │       └── Label (Level 5)
        └── Draggable_Item (Level 3)
            └── Label (Level 4)
```

**Max Depth**: 5 levels

---

**HIERARCHY STRUCTURE COMPLETE!** 🌳✨

Use this as reference when creating your hierarchy!

**KAYA MO YAN!** 💪📊
