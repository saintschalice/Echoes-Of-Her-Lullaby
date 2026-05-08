# 🔨 HAMMER - ITEM DATABASE ENTRY

## 📋 ADD TO ITEM DATABASE

Add this entry to your `ItemDatabase` ScriptableObject:

---

## 🔨 HAMMER ITEM

### **Item Details:**
```
Item ID: "hammer"
Item Name: "Hammer"
Item Description: "A heavy hammer. Hidden in the medicine cabinet. Mother knew I'd need it."
Item Sprite: [Hammer sprite]
Item Type: Key Item / Tool
Is Stackable: false
Max Stack: 1
```

### **Usage:**
- Found in medicine cabinet (Room 08)
- Used to break the mirror in QTE
- Required to progress

---

## 📝 HOW TO ADD

### **Method 1: Unity Inspector**

1. **Open ItemDatabase:**
   - Find your `ItemDatabase` ScriptableObject
   - Usually in `Assets/Resources/Data/` or similar

2. **Add New Item:**
   - Increase `Items` array size by 1
   - Fill in the new entry:
     ```
     Item ID: hammer
     Item Name: Hammer
     Description: A heavy hammer. Hidden in the medicine cabinet. Mother knew I'd need it.
     Sprite: [Drag hammer sprite]
     Type: KeyItem
     Stackable: false
     Max Stack: 1
     ```

3. **Save:**
   - Ctrl+S or File → Save

---

### **Method 2: Code (if using ItemDatabase script)**

If your ItemDatabase has an `AddItem()` method:

```csharp
ItemDatabase.Instance.AddItem(new ItemData
{
    itemId = "hammer",
    itemName = "Hammer",
    description = "A heavy hammer. Hidden in the medicine cabinet. Mother knew I'd need it.",
    sprite = hammerSprite,
    itemType = ItemType.KeyItem,
    isStackable = false,
    maxStack = 1
});
```

---

## 🎨 HAMMER SPRITE

Create or import a hammer sprite:

### **Sprite Requirements:**
- Size: 64x64 or 128x128 pixels
- Format: PNG with transparency
- Style: Match your game's art style
- Color: Dark metal/wood

### **Sprite Location:**
```
Assets/Art/Sprites/Items/hammer.png
```

---

## 🔧 SETUP IN ROOM 08

### **1. Create Hammer GameObject**

```
GameObject: Hammer
Parent: MedicineCabinet (or near it)
Position: Inside/near medicine cabinet

Components:
├─ SpriteRenderer (hammer sprite)
├─ Collider2D (trigger)
└─ Room08_Interactable.cs
    ├─ Object Type: Hammer
    └─ Evidence ID: [empty]
```

### **2. Room08_Interactable Settings**

```
My Type: Hammer
Evidence Id: [leave empty]
```

---

## 📊 FLOW INTEGRATION

### **Collection Flow:**
```
1. Player examines medicine cabinet
   → Shows medicine dialogue
   
2. Player clicks hammer
   → Adds to inventory with notification
   → Shows hammer dialogue
   → Hammer disappears
   → Checks if all evidence collected
   
3. If all evidence + hammer collected
   → Emily appears in mirror
   → Player can now break mirror
```

---

## 🎮 TESTING CHECKLIST

- [ ] Hammer sprite created/imported
- [ ] Hammer added to ItemDatabase
- [ ] Hammer GameObject created in scene
- [ ] Room08_Interactable attached
- [ ] Object Type set to "Hammer"
- [ ] Collider2D added (trigger)
- [ ] Click hammer → Adds to inventory
- [ ] Notification shows "Hammer obtained"
- [ ] Dialogue shows after notification
- [ ] Hammer disappears after pickup
- [ ] Can't break mirror without hammer

---

## 💡 ALTERNATIVE: HAMMER IN CABINET UI

If you want hammer to appear in a cabinet panel (like Room 07):

### **Option A: Direct Pickup (Current)**
- Hammer is a GameObject in scene
- Click to pick up directly
- Simpler, faster

### **Option B: Cabinet Panel**
- Create cabinet panel UI
- Hammer appears as button in panel
- Click button to take hammer
- More complex, but consistent with Room 07

**Recommended:** Use Option A (direct pickup) for simplicity.

---

## 🔊 AUDIO (Optional)

Add pickup sound:

```csharp
void PickupHammer()
{
    // Play pickup sound
    AudioManager.Instance?.PlaySFX(itemPickupSound);
    
    // Add to inventory
    InventorySystem.Instance?.AddItemWithNotification("hammer");
    
    // ... rest of code
}
```

---

## 📝 SUMMARY

### **What to Create:**
1. ✅ Hammer sprite (64x64 or 128x128)
2. ✅ ItemDatabase entry (ID: "hammer")
3. ✅ Hammer GameObject in scene
4. ✅ Room08_Interactable component
5. ✅ Collider2D (trigger)

### **What Script Does:**
1. ✅ Adds hammer to inventory with notification
2. ✅ Shows hammer dialogue after notification
3. ✅ Hides hammer GameObject
4. ✅ Checks if all evidence collected
5. ✅ Triggers Emily appearance if ready

### **Flow:**
```
Medicine Cabinet → Hammer → Inventory → Dialogue → All Evidence? → Emily Appears → Break Mirror
```

---

**READY!** 🔨✨
