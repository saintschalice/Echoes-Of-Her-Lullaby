# 🎯 QTE PANEL SETUP - QUICK REFERENCE

## 📋 HIERARCHY

```
Canvas
└── QTE_Panel (Panel)
    ├── Mirror_Image (Image)
    ├── Tap_Target_Parent (Empty RectTransform)
    ├── Timer_Text (Text/TextMeshProUGUI)
    ├── Progress_Text (Text/TextMeshProUGUI)
    └── Shatter_Effect (Particle System) [Optional]
```

---

## 🎨 DETAILED SETUP

### **1. QTE_Panel (Panel)**
```
Component: Panel (Image)
Anchors: Stretch (full screen)
  - Min: (0, 0)
  - Max: (1, 1)
  - Left/Right/Top/Bottom: 0
Color: Black (0, 0, 0, 200) - Semi-transparent
Active: FALSE (initially hidden)
```

### **2. Mirror_Image (Image)**
```
Component: Image
Parent: QTE_Panel
Anchors: Center
  - Anchor Min: (0.5, 0.5)
  - Anchor Max: (0.5, 0.5)
  - Pivot: (0.5, 0.5)
Position: (0, 0, 0)
Size: (400, 600) - Adjust as needed
Sprite: Mirror_Normal (clean mirror)
Preserve Aspect: TRUE
```

### **3. Tap_Target_Parent (Empty RectTransform)**
```
Component: RectTransform (no Image)
Parent: QTE_Panel
Anchors: Center
  - Anchor Min: (0.5, 0.5)
  - Anchor Max: (0.5, 0.5)
  - Pivot: (0.5, 0.5)
Position: (0, 0, 0)
Size: (400, 300) - Tap area
```
**Note:** This is where tap targets will spawn dynamically

### **4. Timer_Text (Text/TextMeshProUGUI)**
```
Component: Text or TextMeshProUGUI
Parent: QTE_Panel
Anchors: Top Center
  - Anchor Min: (0.5, 1)
  - Anchor Max: (0.5, 1)
  - Pivot: (0.5, 1)
Position: (0, -50, 0)
Size: (200, 60)
Text: "2.00"
Font Size: 48
Alignment: Center
Color: White (255, 255, 255, 255)
```

### **5. Progress_Text (Text/TextMeshProUGUI)**
```
Component: Text or TextMeshProUGUI
Parent: QTE_Panel
Anchors: Bottom Center
  - Anchor Min: (0.5, 0)
  - Anchor Max: (0.5, 0)
  - Pivot: (0.5, 0)
Position: (0, 50, 0)
Size: (200, 60)
Text: "1/5"
Font Size: 36
Alignment: Center
Color: White (255, 255, 255, 255)
```

### **6. Shatter_Effect (Particle System)** [Optional]
```
Component: Particle System
Parent: QTE_Panel
Position: (0, 0, 0)
Active: FALSE (initially hidden)

Particle System Settings:
- Duration: 1.0
- Start Lifetime: 0.5-1.0
- Start Speed: 5-10
- Start Size: 0.1-0.3
- Emission Rate: 50
- Shape: Sphere
- Renderer: Sprite (glass shard sprite)
```

---

## 🎯 TAP TARGET PREFAB

### **TapTarget (Prefab)**
```
Component: Image + Button
Size: (100, 100)
Sprite: Circle (white circle)
Color: White (255, 255, 255, 200)

Button Settings:
- Interactable: TRUE
- Transition: Color Tint
- Normal Color: White (255, 255, 255, 200)
- Highlighted Color: Yellow (255, 255, 0, 255)
- Pressed Color: Green (0, 255, 0, 255)
- Disabled Color: Gray (128, 128, 128, 128)

Optional: Add Animator
- Pulsing/scaling animation
- Makes target more visible
```

**Create Prefab:**
1. Create UI → Image
2. Name: `TapTarget`
3. Setup as above
4. Drag to `Assets/Prefabs/UI/TapTarget.prefab`
5. Delete from scene

---

## 🔗 ROOM08_MIRRORQTE REFERENCES

Assign these in Inspector:

### **QTE Settings**
```
Total Taps: 5
Starting Time: 2.0
Minimum Time: 0.8
Max Failures: 3
```

### **UI References**
```
QTE Panel: QTE_Panel
Tap Target Prefab: TapTarget (prefab)
Tap Target Parent: Tap_Target_Parent
Timer Text: Timer_Text
Progress Text: Progress_Text
```

### **Visual Effects**
```
Mirror Image: Mirror_Image
Crack Sprites: [5 sprites]
  - Mirror_Crack_1
  - Mirror_Crack_2
  - Mirror_Crack_3
  - Mirror_Crack_4
  - Mirror_Crack_5
Shatter Effect: Shatter_Effect (optional)
```

### **Audio**
```
Tap Sound: Click/tap sound
Crack Sound: Glass crack sound
Shatter Sound: Glass shatter sound
Fail Sound: Error/fail sound
Glass Stress Sounds: [5 clips]
  - Stress_1 (light creaking)
  - Stress_2 (medium creaking)
  - Stress_3 (louder creaking)
  - Stress_4 (very loud creaking)
  - Stress_5 (almost breaking)
```

### **Camera Shake**
```
Shake Intensity: 0.1
Shake Duration: 0.2
```

---

## ✅ QUICK CHECKLIST

- [ ] QTE_Panel created (initially inactive)
- [ ] Mirror_Image added with normal sprite
- [ ] Tap_Target_Parent created (empty)
- [ ] Timer_Text added (shows "2.00")
- [ ] Progress_Text added (shows "1/5")
- [ ] Shatter_Effect added (optional, initially inactive)
- [ ] TapTarget prefab created
- [ ] Room08_MirrorQTE GameObject created
- [ ] All references assigned in Inspector
- [ ] 5 crack sprites created and assigned
- [ ] All audio clips assigned
- [ ] Canvas has GraphicRaycaster
- [ ] EventSystem exists in scene

---

## 🐛 TROUBLESHOOTING

### **QTE Panel Not Showing**
- Check QTE_Panel is assigned
- Check panel is child of Canvas
- Check Canvas has GraphicRaycaster

### **Tap Targets Not Clickable**
- Check TapTarget has Button component
- Check EventSystem exists
- Check Tap_Target_Parent is assigned

### **Timer Not Updating**
- Check Timer_Text is assigned
- Check text component exists

### **Mirror Not Cracking**
- Check all 5 crack sprites are assigned
- Check Mirror_Image is assigned

---

**READY!** 🎮✨
