# Room 07 - Item Database Setup Guide

## 🎯 Overview

All items in Room 07 need to be added to the ItemDatabase so they can be picked up, stored in inventory, and displayed with notifications.

---

## 📦 Room 07 Items List

### Key Items (Required for Puzzles):

1. **emily_cup** - Emily's Cup
   - Required for: Tea Party Puzzle
   - Obtained from: Small Cabinet (after opening curtains)
   
2. **emily_doll** - Emily Doll
   - Required for: Dollhouse Puzzle
   - Obtained from: Toybox (after solving sliding puzzle)
   - Triggers: Memory Cutscene 2

### Story Items (Optional Collectibles):

3. **diary_page_5** - Diary Page 5
   - Lisa's diary about Emily
   - Obtained from: Nightstand/Bookshelf
   - Triggers: Memory fragment

4. **lullaby_fragment_3** - Lullaby Fragment 3
   - Music box melody
   - Obtained from: Mirror interaction
   - Triggers: Memory fragment

5. **bedroom_key** - Bedroom Key
   - Key to Lisa's bedroom
   - Obtained from: Previous room (if needed)

6. **fairy_tale_book** - Fairy Tale Book
   - Children's book with Emily's note
   - Obtained from: Reading Table
   - Triggers: Memory fragment

7. **emily_chair_note** - Emily's Chair Note
   - Note from Emily's chair
   - Obtained from: Chair interaction

8. **closet_scratches** - Closet Scratches Photo
   - Photo of scratches in closet
   - Obtained from: Closet interaction
   - Triggers: Memory fragment

9. **wall_drawing** - Wall Drawing
   - Crayon drawing of Lisa and Emily
   - Obtained from: Wall interaction
   - Triggers: Memory fragment

10. **bed_note** - Bed Note
    - Note about Emily keeping Lisa safe
    - Obtained from: Bed interaction

---

## 🔧 Setup Method 1: Using Helper Script (EASIEST)

### Step 1: Create Setup GameObject

```
1. In Hierarchy, create empty GameObject
2. Rename to "Room07_DatabaseSetup"
3. Add Component → Room07_ItemDatabaseSetup
```

### Step 2: Assign Database

```
1. Select Room07_DatabaseSetup
2. In Inspector, find Room07_ItemDatabaseSetup component
3. Drag your ItemDatabase asset to "Database" field
   (Usually in Assets/Resources/Data/ItemDatabase.asset)
```

### Step 3: Run Setup

```
1. Right-click on Room07_ItemDatabaseSetup component
2. Click "Add Room 07 Items to Database"
3. Check Console for success message
4. All 10 items added automatically! ✅
```

### Step 4: Assign Sprites

```
1. Open ItemDatabase asset in Inspector
2. Find each Room 07 item
3. Assign Item Icon sprite for each:
   - emily_cup → Cup sprite
   - emily_doll → Doll sprite
   - diary_page_5 → Diary page sprite
   - etc.
```

---

## 🔧 Setup Method 2: Manual (If Script Doesn't Work)

### Step 1: Open ItemDatabase

```
1. In Project window, navigate to:
   Assets/Resources/Data/ItemDatabase.asset
2. Double-click to open in Inspector
```

### Step 2: Add Each Item Manually

For each item, click "+" button and fill in:

#### Emily's Cup:
```
Item Id: emily_cup
Item Name: Emily's Cup
Description: A small porcelain cup with delicate floral patterns. It feels cold to the touch. This was Emily's favorite cup for their tea parties.
Item Icon: [Assign cup sprite]
Is Key Item: ✓ (checked)
Is Usable: ✓ (checked)
Is Consumable: ✗ (unchecked)
Triggers Memory: ✗
Memory Fragment Id: (empty)
Required For Puzzle: tea_party
```

#### Emily Doll:
```
Item Id: emily_doll
Item Name: Emily Doll
Description: A handmade doll with button eyes and yarn hair. The note attached reads: 'Dear Emily, thank you for making mommy stop hurting me yesterday.'
Item Icon: [Assign doll sprite]
Is Key Item: ✓
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_doll
Required For Puzzle: dollhouse
```

#### Diary Page 5:
```
Item Id: diary_page_5
Item Name: Diary Page 5
Description: Lisa's diary entry: 'Emily came to me again last night. She sang the pretty song and made the scary dreams go away. I wish she could stay forever.'
Item Icon: [Assign diary sprite]
Is Key Item: ✓
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_diary_5
Required For Puzzle: (empty)
```

#### Lullaby Fragment 3:
```
Item Id: lullaby_fragment_3
Item Name: Lullaby Fragment 3
Description: A haunting melody fragment. The music box in the toy chest plays this tune. It triggers a memory of someone tucking young Lisa into bed, singing softly.
Item Icon: [Assign music note sprite]
Is Key Item: ✓
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_lullaby_3
Required For Puzzle: (empty)
```

#### Bedroom Key:
```
Item Id: bedroom_key
Item Name: Bedroom Key
Description: An old brass key with a tag labeled 'Lisa's Room'. The metal is tarnished but the key still works.
Item Icon: [Assign key sprite]
Is Key Item: ✓
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✗
Memory Fragment Id: (empty)
Required For Puzzle: bedroom_door
```

#### Fairy Tale Book:
```
Item Id: fairy_tale_book
Item Name: Fairy Tale Book
Description: A worn children's book. A note inside reads: 'Emily likes the stories where the princess gets saved.' Several pages are bookmarked.
Item Icon: [Assign book sprite]
Is Key Item: ✗
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_fairy_tales
Required For Puzzle: (empty)
```

#### Emily's Chair Note:
```
Item Id: emily_chair_note
Item Name: Emily's Chair Note
Description: A small note attached to a child's chair: 'Emily's Chair - Do Not Sit.' The chair is always cold to the touch.
Item Icon: [Assign note sprite]
Is Key Item: ✗
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✗
Memory Fragment Id: (empty)
Required For Puzzle: (empty)
```

#### Closet Scratches Photo:
```
Item Id: closet_scratches
Item Name: Closet Scratches Photo
Description: A photo of deep scratches inside the closet. They look like they were made by small fingers. Lisa hid here often when she was scared.
Item Icon: [Assign photo sprite]
Is Key Item: ✗
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_closet
Required For Puzzle: (empty)
```

#### Wall Drawing:
```
Item Id: wall_drawing
Item Name: Wall Drawing
Description: A crayon drawing showing two figures holding hands - one labeled 'Me' and another labeled 'Emily'. They're playing together under a smiling sun.
Item Icon: [Assign drawing sprite]
Is Key Item: ✗
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✓
Memory Fragment Id: memory_drawing
Required For Puzzle: (empty)
```

#### Bed Note:
```
Item Id: bed_note
Item Name: Bed Note
Description: A note pinned to the bed: 'For my friend Emily - she keeps me safe at night.' The bed has two pillow indentations.
Item Icon: [Assign note sprite]
Is Key Item: ✗
Is Usable: ✓
Is Consumable: ✗
Triggers Memory: ✗
Memory Fragment Id: (empty)
Required For Puzzle: (empty)
```

---

## 🎨 Sprite Requirements

### Sprites Needed:

1. **emily_cup** - Porcelain tea cup sprite
2. **emily_doll** - Handmade doll sprite
3. **diary_page_5** - Diary page sprite
4. **lullaby_fragment_3** - Music note or music box sprite
5. **bedroom_key** - Old brass key sprite
6. **fairy_tale_book** - Children's book sprite
7. **emily_chair_note** - Note/paper sprite
8. **closet_scratches** - Photo or scratches sprite
9. **wall_drawing** - Crayon drawing sprite
10. **bed_note** - Note/paper sprite

### Sprite Specs:
```
Size: 256x256 or 512x512
Format: PNG with transparency
Style: Match your game's art style
Location: Assets/Art/UI/Items/ (recommended)
```

---

## 🧪 Testing

### Test 1: Verify Items in Database
```
1. Open ItemDatabase asset
2. Check "All Items" list
3. Should see all 10 Room 07 items ✓
4. Each item should have:
   - Unique Item Id
   - Item Name
   - Description
   - Icon (sprite assigned)
```

### Test 2: Test Item Pickup
```
1. Play Mode
2. Interact with cabinet
3. Take Emily's Cup
4. Check inventory ✓
5. Should see cup with icon and name ✓
```

### Test 3: Test Notification
```
1. Pick up any item
2. Notification should show:
   - Item icon
   - Item name
   - "Added to inventory" message
3. Tap to continue ✓
```

### Test 4: Test Item Usage
```
1. Open inventory
2. Click on Emily's Cup
3. Should show description ✓
4. Should show "Required for: Tea Party" ✓
```

---

## 🐛 Troubleshooting

### Issue 1: Items Not in Database
```
Problem: Can't find items in ItemDatabase

Solution:
1. Run Room07_ItemDatabaseSetup script
2. Or add items manually
3. Save ItemDatabase asset (Ctrl+S)
```

### Issue 2: No Item Icon
```
Problem: Item shows but no icon

Solution:
1. Open ItemDatabase
2. Find item
3. Assign Item Icon sprite
4. Save asset
```

### Issue 3: Item Not Found Error
```
Problem: Console shows "Item not found: emily_cup"

Solution:
1. Check Item Id spelling (case-sensitive!)
2. Make sure item exists in database
3. Check InventoryManager has database assigned
```

### Issue 4: Duplicate Item IDs
```
Problem: Warning about duplicate IDs

Solution:
1. Open ItemDatabase
2. Right-click → Validate Database
3. Fix any duplicate IDs shown
4. Each ID must be unique
```

---

## ✅ Verification Checklist

### Database Setup:
- [ ] ItemDatabase asset exists
- [ ] Room07_ItemDatabaseSetup script created
- [ ] Script run successfully
- [ ] All 10 items added to database
- [ ] No duplicate IDs
- [ ] All items have names
- [ ] All items have descriptions

### Sprites:
- [ ] emily_cup sprite assigned
- [ ] emily_doll sprite assigned
- [ ] diary_page_5 sprite assigned
- [ ] lullaby_fragment_3 sprite assigned
- [ ] bedroom_key sprite assigned
- [ ] fairy_tale_book sprite assigned
- [ ] emily_chair_note sprite assigned
- [ ] closet_scratches sprite assigned
- [ ] wall_drawing sprite assigned
- [ ] bed_note sprite assigned

### Testing:
- [ ] Can pick up Emily's Cup
- [ ] Notification shows correctly
- [ ] Item appears in inventory
- [ ] Item icon displays
- [ ] Item description shows
- [ ] Can use item in puzzle

---

## 🎯 Quick Summary

### Method 1 (Easy):
1. Create GameObject with Room07_ItemDatabaseSetup
2. Assign ItemDatabase
3. Run "Add Room 07 Items to Database"
4. Assign sprites in Inspector
5. Done! ✅

### Method 2 (Manual):
1. Open ItemDatabase asset
2. Add each item manually (10 items)
3. Fill in all fields
4. Assign sprites
5. Save asset
6. Done! ✅

---

**Use the helper script for fastest setup!** 🚀

**Don't forget to assign sprites!** 🎨✨
