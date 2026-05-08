# Paano Itago ang Yellow Cup - SIMPLE STEPS

## 🎯 Yung Yellow Cup sa Screenshot Mo

Yan yung cup na nakikita mo pa rin kahit kinuha mo na sa panel.

---

## ✅ 3 SIMPLE STEPS LANG

### STEP 1: Hanapin ang Cup GameObject

**Gawin mo:**
```
1. Sa Unity, i-click ang Scene view tab (hindi Game view)
2. I-click yung yellow cup sa screen
3. Tingnan ang Hierarchy window - may nag-highlight
4. Yan ang cup GameObject (tandaan ang pangalan)
```

**O kaya:**
```
1. Sa Hierarchy window, i-type: "cup" sa search box
2. Hanapin yung GameObject na may yellow cup sprite
3. I-click para ma-select
```

**Possible names:**
- EmilyCup
- Cup
- emily_cup
- Cabinet_Cup
- Item_Cup

---

### STEP 2: I-assign sa Cabinet Panel

**Gawin mo:**
```
1. Sa Hierarchy, i-click: Cabinet_Panel
   (Huwag yung cup! Yung panel!)

2. Tingnan ang Inspector window (right side)

3. Scroll down, hanapin: CabinetItemPanel (Script)

4. Hanapin ang section: "Scene References"

5. May makikita kang field: "Cup In Scene"
   Nakalagay: None (GameObject)

6. I-drag yung cup GameObject mula Step 1
   Mula Hierarchy → I-drop sa "Cup In Scene" field

7. Dapat lumabas ang pangalan ng cup sa field
```

---

### STEP 3: Save at Test

**Gawin mo:**
```
1. I-save ang scene: Ctrl+S (o File → Save)

2. I-play ang game (Play button)

3. Buksan ang cabinet panel

4. I-click ang cup sa panel

5. Dapat mawala na ang yellow cup sa scene! ✓
```

---

## 📸 Visual Guide

### Ano ang Hahanapin Mo:

```
Inspector (pag naka-select ang Cabinet_Panel):
┌─────────────────────────────────────────┐
│ Cabinet Item Panel (Script)             │
├─────────────────────────────────────────┤
│ UI References                           │
│   Cabinet Panel: ...                    │
│   Close Button: ...                     │
│                                         │
│ Scene References                        │ ← HANAPIN MO ITO!
│   Cup In Scene: None (GameObject)       │ ← I-DRAG DITO ANG CUP!
│                                         │
│ Item Display                            │
│   Item Image: ...                       │
└─────────────────────────────────────────┘
```

### Pagkatapos I-assign:

```
Inspector (pag naka-select ang Cabinet_Panel):
┌─────────────────────────────────────────┐
│ Cabinet Item Panel (Script)             │
├─────────────────────────────────────────┤
│ Scene References                        │
│   Cup In Scene: EmilyCup (GameObject)   │ ← MAY LAMAN NA! ✓
└─────────────────────────────────────────┘
```

---

## 🎮 Paano Malaman Kung Tama?

### Check 1: Bago I-play
```
1. Select Cabinet_Panel sa Hierarchy
2. Inspector → CabinetItemPanel
3. Cup In Scene field: Dapat MAY PANGALAN (hindi "None")
```

### Check 2: Habang Nag-play
```
1. Play Mode
2. Buksan ang cabinet
3. Tingnan ang Console window (bottom)
4. Dapat may message:
   "[CabinetItemPanel] Panel opened. Cup In Scene assigned: True"
   
Kung "False":
→ Hindi naka-assign! Ulitin ang Step 2
```

### Check 3: Pagkatapos Kunin
```
1. I-click ang cup sa panel
2. Console dapat may:
   "[CabinetItemPanel] Cup in scene hidden!"
3. Yellow cup sa scene dapat MAWALA na
```

---

## 🐛 Common Problems

### Problem 1: "Hindi ko makita ang Cup In Scene field"

**Solution:**
```
1. Make sure naka-select ang Cabinet_Panel (UI panel)
2. Hindi yung cup mismo!
3. Scroll down sa Inspector
4. Hanapin ang "CabinetItemPanel (Script)" component
5. Expand kung naka-collapse
```

### Problem 2: "Pag i-drag ko, walang nangyayari"

**Solution:**
```
1. Make sure naka-select ang TAMANG cup GameObject
   - Yung yellow cup sa scene (may Sprite Renderer)
   - Hindi yung UI image sa panel!

2. I-drag mula Hierarchy window
3. I-drop sa "Cup In Scene" field sa Inspector
4. Dapat mag-highlight ang field pag nag-hover
```

### Problem 3: "Bumabalik sa None pagkatapos i-assign"

**Solution:**
```
1. After i-assign, i-save agad: Ctrl+S
2. Check kung naka-save ba ang scene
3. Kung may asterisk (*) sa scene name: Hindi pa saved
4. I-save ulit
```

### Problem 4: "Maraming cup sa Hierarchy"

**Solution:**
```
1. Hanapin yung cup na nasa cabinet area
2. Yung nakikita sa screenshot mo (yellow cup sa taas)
3. Hindi yung cup sa tea party area
4. I-click sa Scene view para sigurado
```

---

## 💡 Quick Checklist

Gawin mo isa-isa:

- [ ] Step 1: Nahanap ko ang yellow cup GameObject
- [ ] Step 1: Naka-select ang cup sa Hierarchy
- [ ] Step 2: Naka-select ang Cabinet_Panel (UI)
- [ ] Step 2: Nakita ko ang CabinetItemPanel component
- [ ] Step 2: Nakita ko ang "Cup In Scene" field
- [ ] Step 2: Naka-drag ang cup GameObject sa field
- [ ] Step 2: May pangalan na sa field (hindi "None")
- [ ] Step 3: Naka-save ang scene (Ctrl+S)
- [ ] Step 3: Nag-play mode
- [ ] Step 3: Console: "assigned: True"
- [ ] Step 3: Kinuha ang cup
- [ ] Step 3: Nawala ang yellow cup! ✓

---

## 🎬 Exact Steps (Copy-Paste)

```
1. Click Scene view
2. Click yellow cup in scene
3. Note the name in Hierarchy (e.g., "EmilyCup")
4. Click Cabinet_Panel in Hierarchy
5. Look at Inspector window
6. Find "CabinetItemPanel (Script)"
7. Find "Scene References" section
8. Find "Cup In Scene: None (GameObject)"
9. Drag "EmilyCup" from Hierarchy to this field
10. Field should now show "EmilyCup"
11. Press Ctrl+S to save
12. Press Play button
13. Open cabinet panel
14. Click cup in panel
15. Yellow cup should disappear! ✓
```

---

## 📞 Kung Hindi Pa Rin Gumagana

Kung na-follow mo na lahat pero hindi pa rin nawawala ang cup:

1. **Screenshot mo ang Inspector** habang naka-select ang Cabinet_Panel
2. **Screenshot mo ang Hierarchy** para makita ko ang cup GameObject
3. **Screenshot mo ang Console** pagkatapos kunin ang cup

Ipakita sa akin para matulungan kita! 🙂

---

**Yan lang! 3 steps lang talaga!** 🎮✨

**IMPORTANTE: I-assign mo lang ang cup GameObject sa "Cup In Scene" field!** ✅
