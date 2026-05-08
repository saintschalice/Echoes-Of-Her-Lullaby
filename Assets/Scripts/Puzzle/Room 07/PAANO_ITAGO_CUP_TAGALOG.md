# Paano Itago ang Yellow Cup (Tagalog Guide)

## 🎯 Problema

Yung yellow cup sa screen ay **nakikita pa rin** kahit kinuha mo na sa panel.

**Dahilan:** Hindi pa naka-assign yung cup GameObject sa script.

---

## ✅ Solusyon (3 Simple Steps)

### Step 1: Hanapin ang Yellow Cup GameObject

**Madaling Paraan:**
```
1. Sa Scene view (hindi Game view), i-click yung yellow cup
2. Mapapansin mo sa Hierarchy, may nag-highlight
3. Tandaan yung pangalan (halimbawa: "EmilyCup" o "Cup")
```

**O kaya:**
```
1. Sa Hierarchy window, i-type sa search: "cup"
2. Hanapin yung GameObject na may Sprite Renderer
3. Yan yung yellow cup
```

---

### Step 2: I-assign sa Cabinet Panel

```
1. Sa Hierarchy, i-click: Cabinet_Panel
   (Ito yung UI panel, hindi yung cup!)

2. Tingnan ang Inspector window

3. Hanapin: CabinetItemPanel component

4. Hanapin ang section: "Scene References"

5. Makikita mo: Cup In Scene: None (GameObject)

6. I-drag yung cup GameObject mula Step 1 dito

7. Dapat lumabas: Cup In Scene: EmilyCup (GameObject)
```

---

### Step 3: I-save at Test

```
1. I-save ang scene (Ctrl+S)

2. I-play ang game

3. Buksan ang cabinet panel

4. I-click ang cup sa panel

5. Dapat mawala na yung yellow cup! ✓
```

---

## 📸 Visual Guide

### Bago i-assign:
```
Inspector (Cabinet_Panel selected):
┌─────────────────────────────────┐
│ CabinetItemPanel                │
├─────────────────────────────────┤
│ Scene References                │
│   Cup In Scene: None            │ ← WALANG LAMAN!
└─────────────────────────────────┘

Result: Nakikita pa rin ang cup ❌
```

### Pagkatapos i-assign:
```
Inspector (Cabinet_Panel selected):
┌─────────────────────────────────┐
│ CabinetItemPanel                │
├─────────────────────────────────┤
│ Scene References                │
│   Cup In Scene: EmilyCup ✓      │ ← MAY LAMAN NA!
└─────────────────────────────────┘

Result: Nawawala na ang cup ✓
```

---

## 🎮 Ano ang Dapat I-select?

### ❌ MALI - Huwag ito:
- Cabinet_Panel (UI panel)
- Item_Image (larawan sa loob ng panel)
- Canvas
- Kahit anong UI element

### ✅ TAMA - Ito ang kailangan:
- Yung yellow cup GameObject sa scene
- May Sprite Renderer component
- Nakikita sa Scene view
- Bahagi ng game world, hindi UI

---

## 🔍 Paano Malaman Kung Tama?

### Check 1: Tipo ng GameObject
```
I-select ang cup
Sa Inspector dapat makita:
- Transform (hindi RectTransform)
- Sprite Renderer (hindi Image)
- Baka may Collider

Kung RectTransform o Image ang nakita:
→ Mali! Yan ay UI, hindi scene object
```

### Check 2: Pagkatapos I-assign
```
Inspector → CabinetItemPanel:
  Cup In Scene: Dapat may pangalan ng GameObject
  
Kung "None" pa rin:
→ Hindi nag-save, ulitin
```

---

## 🧪 Testing

### Test 1: Check Assignment
```
1. I-select ang Cabinet_Panel
2. Inspector → CabinetItemPanel
3. Cup In Scene field dapat HINDI "None"
4. Dapat may pangalan ng cup
```

### Test 2: Check Console
```
1. Play Mode
2. Buksan ang cabinet
3. Sa Console dapat makita:
   "[CabinetItemPanel] Panel opened. Cup In Scene assigned: True"
   
Kung "False":
→ Hindi pa naka-assign!
```

### Test 3: Kunin ang Cup
```
1. I-click ang cup sa panel
2. Sa Console dapat makita:
   "[CabinetItemPanel] Cup in scene hidden!"
3. Dapat mawala ang yellow cup
```

---

## 🎯 Common Problems

### Problem 1: Hindi Makita ang Cup
```
Subukan:
1. Sa Scene view, i-zoom out
2. Hanapin ang yellow cup sprite
3. I-click ito
4. Tingnan sa Hierarchy kung ano ang selected
```

### Problem 2: Maraming Cup
```
Kung may maraming cups:
- Hanapin yung nasa cabinet area
- Yung nakikita bago kunin
- Hindi yung nasa tea party area
```

### Problem 3: Hindi Nag-save ang Assignment
```
Pagkatapos i-assign:
1. I-save ang scene (Ctrl+S)
2. Check kung nandoon pa rin ang pangalan
3. Kung bumalik sa "None": Hindi nag-save
```

---

## 💡 Quick Checklist

- [ ] Nahanap ang yellow cup GameObject sa scene
- [ ] May Sprite Renderer ang cup (hindi Image)
- [ ] May Transform ang cup (hindi RectTransform)
- [ ] Naka-select ang Cabinet_Panel (UI)
- [ ] Nahanap ang CabinetItemPanel component
- [ ] Nahanap ang "Cup In Scene" field
- [ ] Naka-drag ang cup GameObject sa field
- [ ] May pangalan na ang field (hindi "None")
- [ ] Naka-save ang scene (Ctrl+S)
- [ ] Na-test sa Play Mode
- [ ] Sa Console: "assigned: True"
- [ ] Nawawala ang cup pagkatapos kunin

---

## 🎬 Step-by-Step (Imagine This)

```
1. [I-click ang yellow cup sa scene]
   → Sa Hierarchy, nag-highlight ang "EmilyCup"

2. [Huwag i-deselect, hanapin ang Cabinet_Panel]
   → I-click ang Cabinet_Panel sa Hierarchy

3. [Tingnan ang Inspector]
   → Hanapin ang CabinetItemPanel component
   → Hanapin ang "Cup In Scene" field

4. [I-drag ang EmilyCup mula Hierarchy]
   → I-drop sa "Cup In Scene" field
   → Dapat lumabas "EmilyCup" sa field

5. [I-save ang scene]
   → Ctrl+S o File → Save

6. [I-test]
   → Play Mode
   → Kunin ang cup
   → Dapat mawala! ✓
```

---

## 🎯 Importante!

**Ang yellow cup sa screenshot mo ay KAILANGAN i-assign sa "Cup In Scene" field!**

**I-select ang Cabinet_Panel → Inspector → I-drag ang cup GameObject sa field!** ✅

**I-save ang scene pagkatapos i-assign!** 💾✨

---

## 📝 Kung May Problema Pa Rin

Kung hindi pa rin gumagana:

1. **Screenshot mo ang Inspector** habang naka-select ang Cabinet_Panel
2. **Screenshot mo rin ang Hierarchy** para makita ko ang cup GameObject
3. **I-check ang Console** kung may error messages

Tapos ipakita sa akin para matulungan kita! 🙂

---

**Yan lang! Simple lang, kailangan lang i-assign! Good luck!** 🎮✨
