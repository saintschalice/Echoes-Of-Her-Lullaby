# Fix: Cup Not Disappearing After Taking

## ❌ Problem

Cup GameObject sa scene hindi nawawala after kunin.

---

## ✅ Solution

I-assign ang cup GameObject sa CabinetItemPanel script para ma-hide automatically.

---

## 🔧 Setup Steps

### Step 1: Find Cup GameObject in Scene

```
Sa Hierarchy, hanapin ang cup GameObject
Example names:
- EmilyCup
- Cup
- TeaCup
- SmallCup
etc.

Ito yung actual 3D/2D object na nakikita sa scene
(Hindi yung sa UI panel!)
```

---

### Step 2: Assign to CabinetItemPanel

```
1. Select Cabinet_Panel (yung UI panel)
2. Inspector → CabinetItemPanel component
3. Find "Scene References" section
4. Drag cup GameObject to "Cup In Scene" field
```

**Inspector Setup:**
```
CabinetItemPanel:
  UI References:
    Cabinet Panel: [Cabinet_Panel]
    Close Button: [Close_Button]
  
  Scene References:
    Cup In Scene: [Drag EmilyCup GameObject here] ← NEW!
  
  Item Display:
    Item Image: [Item_Image]
    Item Name Text: [ItemName_Text]
    Item Description Text: [ItemDescription_Text]
  
  Item to Give:
    Item Id: emily_cup
```

---

## 🎮 How It Works

### Before Taking:
```
1. Cup GameObject in scene: Active (visible)
2. Player opens cabinet panel
3. Sees cup in UI
```

### After Taking:
```
1. Player clicks cup in UI
2. Script automatically hides cup GameObject in scene
3. cupInScene.SetActive(false)
4. Cup disappears from scene ✓
```

### If Already Taken:
```
1. Player opens cabinet again
2. Script checks if item in inventory
3. If yes: Cup already hidden
4. Shows "Already taken" message
```

---

## 🧪 Testing

### Test 1: First Time Taking
```
1. Play Mode
2. Open cabinet panel
3. Cup should be visible in scene ✓
4. Click cup in UI panel
5. Panel closes
6. Cup in scene should disappear ✓
```

### Test 2: Already Taken
```
1. Take cup once
2. Open cabinet again
3. Cup should still be hidden ✓
4. UI shows "Already taken" ✓
```

### Test 3: Scene Reload
```
1. Take cup
2. Save game
3. Reload scene
4. Cup should still be hidden ✓
```

---

## 🐛 Troubleshooting

### Issue 1: Cup Still Visible
```
Problem: Cup doesn't disappear after taking

Check:
1. Cup In Scene field assigned in Inspector?
2. Correct GameObject assigned?
3. GameObject name matches?
```

### Issue 2: Wrong Object Hidden
```
Problem: Different object disappears

Check:
1. Assigned correct cup GameObject?
2. Not assigned cabinet or other object?
3. Check GameObject name in Hierarchy
```

### Issue 3: Cup Reappears
```
Problem: Cup comes back after closing panel

Check:
1. hasEmilyCup flag set to true?
2. Item added to inventory?
3. Save system working?
```

---

## 📊 Scene Structure

```
Hierarchy (Scene):
├── SmallCabinet (GameObject with collider)
│   └── Room07_Interactable
│       └── My Type: Cabinet_Cup
│
└── EmilyCup (GameObject - the actual cup)
    └── Sprite Renderer / Mesh Renderer
    └── This is what gets hidden!

Canvas (UI):
└── Cabinet_Panel
    └── CabinetItemPanel
        └── Cup In Scene: [EmilyCup] ← Assign here!
```

---

## ✅ Complete Setup Checklist

- [ ] Cup GameObject exists in scene
- [ ] Cup GameObject has Sprite/Mesh Renderer
- [ ] Cabinet_Panel has CabinetItemPanel script
- [ ] Cup In Scene field assigned in Inspector
- [ ] Correct cup GameObject assigned
- [ ] Tested taking cup
- [ ] Cup disappears after taking
- [ ] Cup stays hidden after reload

---

## 🎯 Key Points

1. **Two Different Cups:**
   - Cup in Scene (3D/2D GameObject) ← Gets hidden
   - Cup in UI Panel (Image) ← Just for display

2. **Automatic Hiding:**
   - Script hides cup when taken
   - Script keeps cup hidden if already taken
   - No manual code needed!

3. **Assignment:**
   - Drag scene cup to "Cup In Scene" field
   - That's it!

---

## 💡 Alternative: Use ItemPickup Instead

If you want simpler approach, you can also use ItemPickup script on the cup GameObject itself:

```
EmilyCup GameObject:
└── ItemPickup component
    └── Item Id: emily_cup
    └── Pickup Message: "Found Emily's Cup..."
    └── Auto-hides after pickup
```

But current approach (CabinetItemPanel) is better because:
- Shows cup in cabinet panel
- Click to take (more interactive)
- Consistent with other puzzles

---

**Just assign Cup In Scene field!** 🍵

**Cup will auto-hide after taking!** ✨
