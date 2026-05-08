# 📝 TEXTMESHPRO SETUP GUIDE

## ✅ SCRIPT UPDATED!

The `Room08_MirrorQTE.cs` script now supports **both** `Text` and `TextMeshProUGUI`!

---

## 🎯 INSPECTOR FIELDS

### **OLD (Text only):**
```
Timer Text: [Text component]
Progress Text: [Text component]
Tap Timer Text: [Text component]
```

### **NEW (Both supported!):** ✅
```
Timer Text: [Text component] (legacy, optional)
Timer Text TMP: [TextMeshProUGUI component] (TMP, optional)

Progress Text: [Text component] (legacy, optional)
Progress Text TMP: [TextMeshProUGUI component] (TMP, optional)

Tap Timer Text: [Text component] (legacy, optional)
Tap Timer Text TMP: [TextMeshProUGUI component] (TMP, optional)
```

---

## 🔧 HOW TO USE

### **If using TextMeshPro (TMP):**

1. **Create TextMeshProUGUI elements** in your QTE Panel:
   - Timer_Text (TextMeshProUGUI)
   - Progress_Text (TextMeshProUGUI)
   - Tap_Timer_Text (TextMeshProUGUI) [optional]

2. **Assign to TMP fields** in Inspector:
   - Drag Timer_Text to `Timer Text TMP`
   - Drag Progress_Text to `Progress Text TMP`
   - Drag Tap_Timer_Text to `Tap Timer Text TMP` (optional)

3. **Leave legacy fields empty:**
   - `Timer Text` = None
   - `Progress Text` = None
   - `Tap Timer Text` = None

### **If using legacy Text:**

1. **Create Text elements** in your QTE Panel:
   - Timer_Text (Text)
   - Progress_Text (Text)
   - Tap_Timer_Text (Text) [optional]

2. **Assign to legacy fields** in Inspector:
   - Drag Timer_Text to `Timer Text`
   - Drag Progress_Text to `Progress Text`
   - Drag Tap_Timer_Text to `Tap Timer Text` (optional)

3. **Leave TMP fields empty:**
   - `Timer Text TMP` = None
   - `Progress Text TMP` = None
   - `Tap Timer Text TMP` = None

---

## 📊 QTE PANEL HIERARCHY (TMP VERSION)

```
Canvas
└── QTE_Panel (Panel)
    ├── Mirror_Image (Image)
    ├── Tap_Target_Parent (Empty RectTransform)
    ├── Timer_Text (TextMeshProUGUI) ← TMP!
    ├── Progress_Text (TextMeshProUGUI) ← TMP!
    ├── Tap_Timer_Text (TextMeshProUGUI) ← TMP! [Optional]
    └── Shatter_Effect (Particle System)
```

---

## 🎨 TEXTMESHPRO SETUP

### **1. Timer_Text (TextMeshProUGUI)**
```
Component: TextMeshProUGUI
Parent: QTE_Panel
Anchors: Top Center
  - Anchor Min: (0.5, 1)
  - Anchor Max: (0.5, 1)
  - Pivot: (0.5, 1)
Position: (0, -50, 0)
Size: (200, 60)
Text: "2:00"
Font Size: 48
Alignment: Center
Color: White (255, 255, 255, 255)
```

### **2. Progress_Text (TextMeshProUGUI)**
```
Component: TextMeshProUGUI
Parent: QTE_Panel
Anchors: Bottom Center
  - Anchor Min: (0.5, 0)
  - Anchor Max: (0.5, 0)
  - Pivot: (0.5, 0)
Position: (0, 50, 0)
Size: (200, 60)
Text: "0/15"
Font Size: 36
Alignment: Center
Color: White (255, 255, 255, 255)
```

### **3. Tap_Timer_Text (TextMeshProUGUI)** [Optional]
```
Component: TextMeshProUGUI
Parent: QTE_Panel
Anchors: Center
  - Anchor Min: (0.5, 0.5)
  - Anchor Max: (0.5, 0.5)
  - Pivot: (0.5, 0.5)
Position: (0, -200, 0) (below mirror)
Size: (100, 40)
Text: "3.0"
Font Size: 32
Alignment: Center
Color: Yellow (255, 255, 0, 255)
```

---

## 🔗 INSPECTOR ASSIGNMENT (TMP)

### **Room08_MirrorQTE Component:**

```
UI References:
├─ QTE Panel: [QTE_Panel]
├─ Tap Target Prefab: [TapTarget]
├─ Tap Target Parent: [Tap_Target_Parent]
│
├─ Timer Text: None (leave empty)
├─ Timer Text TMP: [Timer_Text] ← Drag TMP component here!
│
├─ Progress Text: None (leave empty)
├─ Progress Text TMP: [Progress_Text] ← Drag TMP component here!
│
├─ Tap Timer Text: None (leave empty)
└─ Tap Timer Text TMP: [Tap_Timer_Text] ← Drag TMP component here!
```

---

## ✅ QUICK CHECKLIST

### **For TextMeshPro Users:**
- [ ] Create TextMeshProUGUI elements (not Text)
- [ ] Assign to TMP fields in Inspector
- [ ] Leave legacy Text fields empty
- [ ] Test - text should update during QTE

### **For Legacy Text Users:**
- [ ] Create Text elements (not TextMeshProUGUI)
- [ ] Assign to legacy Text fields in Inspector
- [ ] Leave TMP fields empty
- [ ] Test - text should update during QTE

---

## 🐛 TROUBLESHOOTING

### **Text not updating during QTE**
**Check:**
- [ ] Correct component type assigned (TMP to TMP field, Text to Text field)
- [ ] Component is not null in Inspector
- [ ] Text element is active in hierarchy

### **Can't drag TextMeshProUGUI to field**
**Solution:**
- Make sure you're dragging to the **TMP field** (not the legacy Text field)
- Field should say "Timer Text TMP" not "Timer Text"

### **Text shows but doesn't change color**
**Check:**
- [ ] TextMeshProUGUI component exists
- [ ] Color changes are working (check script)
- [ ] Text is visible (not behind other UI)

---

## 💡 TIPS

### **Why both Text and TMP?**
- Some projects use legacy `Text` (old UI system)
- Some projects use `TextMeshProUGUI` (new, better quality)
- Script supports both so you can use either!

### **Which should I use?**
- **Use TextMeshPro (TMP)** if possible - better quality, more features
- **Use legacy Text** only if you're already using it in your project

### **Can I use both at the same time?**
- Yes! You can assign both if you want
- Script will update both simultaneously
- Not recommended (unnecessary), but it works

---

## 📝 EXAMPLE SETUP (TMP)

### **Step 1: Create TMP Elements**
```
Right-click QTE_Panel → UI → Text - TextMeshPro
Name: Timer_Text
```

### **Step 2: Configure**
```
Font Size: 48
Alignment: Center
Text: "2:00"
```

### **Step 3: Assign**
```
Room08_MirrorQTE Inspector:
Timer Text TMP: [Drag Timer_Text here]
```

### **Step 4: Test**
```
Start QTE → Timer should count down: 2:00, 1:59, 1:58...
```

---

## 🎉 DONE!

Your script now supports TextMeshPro! 🎮✨

Just assign your TMP components to the TMP fields and you're good to go! 💖

---

## 📄 SUMMARY

| Field | Type | Required | Purpose |
|-------|------|----------|---------|
| Timer Text | Text | Optional | Total time (legacy) |
| Timer Text TMP | TextMeshProUGUI | Optional | Total time (TMP) |
| Progress Text | Text | Optional | Tap progress (legacy) |
| Progress Text TMP | TextMeshProUGUI | Optional | Tap progress (TMP) |
| Tap Timer Text | Text | Optional | Per-tap timer (legacy) |
| Tap Timer Text TMP | TextMeshProUGUI | Optional | Per-tap timer (TMP) |

**Note:** Assign either Text OR TMP fields (or both), at least one is required for each timer!

---

**READY!** 🚀
