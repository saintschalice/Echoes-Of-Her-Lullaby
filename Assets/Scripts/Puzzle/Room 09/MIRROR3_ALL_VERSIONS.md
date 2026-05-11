# Mirror 3 - All Versions Reference

## Available Versions:

### 1. Mirror3_DiaryArrangement ⭐ RECOMMENDED
**File**: `Mirror3_DiaryArrangement.cs`

**Features**:
- ✅ Simplest setup
- ✅ Built-in drag & drop
- ✅ Automatic swap
- ✅ Automatic shuffle
- ✅ Auto-complete detection
- ✅ No external components needed

**Setup**:
- Pages are SIBLINGS of slots (not children)
- Assign slots and pages arrays in Inspector
- That's it!

**Best For**: New setups, clean start

---

### 2. Mirror3_VanityTerror_Simple
**File**: `Mirror3_VanityTerror_Simple.cs`

**Features**:
- ✅ Self-contained
- ✅ Automatic component setup
- ✅ Swap system
- ✅ Shuffle system

**Setup**:
- Pages are CHILDREN of slots
- Assign Slots_Container in Inspector
- Script auto-detects pages

**Best For**: If you already have pages as children of slots

---

### 3. Mirror3_VanityTerror (OLD)
**File**: `Mirror3_VanityTerror.cs`

**Features**:
- Requires DraggableItem on each page
- Manual setup per page
- Complex configuration

**Setup**:
- Add DraggableItem to each page
- Set Puzzle Number = 3
- Assign slots array

**Best For**: Legacy projects only

---

## Which One to Use?

### Use Mirror3_DiaryArrangement if:
- ✅ Starting fresh
- ✅ Want simplest setup
- ✅ Don't want to deal with child/parent hierarchy
- ✅ Want everything in one script

### Use Mirror3_VanityTerror_Simple if:
- ✅ Already have pages as children of slots
- ✅ Want automatic detection
- ✅ Don't want to manually assign arrays

### Use Mirror3_VanityTerror if:
- ✅ Already using it and it works
- ✅ Don't want to change anything

---

## Room09_Interactable Support

The `Room09_Interactable` script now supports ALL 3 versions!

**Priority Order** (checks in this order):
1. Mirror3_DiaryArrangement (newest)
2. Mirror3_VanityTerror_Simple
3. Mirror3_VanityTerror (oldest)

**Console Messages**:
```
[Room09] Using Mirror3_DiaryArrangement (newest version)
```
OR
```
[Room09] Using Mirror3_VanityTerror_Simple
```
OR
```
[Room09] Using Mirror3_VanityTerror (old version)
```

---

## Migration Guide

### From OLD to DiaryArrangement:

1. **Remove**:
   - Mirror3_VanityTerror component
   - DraggableItem from all pages

2. **Restructure**:
   - Move pages OUT of slots (make them siblings)
   - Keep slots as empty GameObjects

3. **Add**:
   - Mirror3_DiaryArrangement component

4. **Assign**:
   - Slots array (8 slots)
   - Pages array (8 pages)

5. **Test**!

### From Simple to DiaryArrangement:

1. **Remove**:
   - Mirror3_VanityTerror_Simple component

2. **Restructure**:
   - Move pages OUT of slots (make them siblings)

3. **Add**:
   - Mirror3_DiaryArrangement component

4. **Assign**:
   - Slots array
   - Pages array

5. **Test**!

---

## Comparison Table

| Feature | DiaryArrangement | Simple | Old |
|---------|-----------------|--------|-----|
| Setup Difficulty | ⭐ Easy | ⭐⭐ Medium | ⭐⭐⭐ Hard |
| Components Needed | 1 | 1 | 9+ |
| Hierarchy | Siblings | Children | Children |
| Auto-Detection | No (manual arrays) | Yes | No |
| Drag Logic | Built-in | Built-in | External |
| Swap Logic | Built-in | Built-in | Complex |
| Shuffle | Built-in | Built-in | Built-in |
| Debugging | Easy | Medium | Hard |

---

## Current Setup

**On your Mirror 3 GameObject, you should have**:

1. **Room09_Interactable** component
   - Mirror Number = 3

2. **ONE of these**:
   - Mirror3_DiaryArrangement ⭐ (recommended)
   - OR Mirror3_VanityTerror_Simple
   - OR Mirror3_VanityTerror

**NOT all three!** Just pick one!

---

## Troubleshooting

### Error: "Mirror3 component not found!"

**Cause**: No Mirror3 component on GameObject

**Fix**: Add ONE of these components:
- Mirror3_DiaryArrangement (recommended)
- Mirror3_VanityTerror_Simple
- Mirror3_VanityTerror

### Multiple Components

**Problem**: Have more than one Mirror3 component

**Fix**: Remove all except one (keep DiaryArrangement if possible)

### Wrong Component Name

**Problem**: Component name doesn't match exactly

**Fix**: Make sure component is named EXACTLY:
- `Mirror3_DiaryArrangement`
- `Mirror3_VanityTerror_Simple`
- `Mirror3_VanityTerror`

---

## Recommendation

**For new setups**: Use `Mirror3_DiaryArrangement`
- Simplest
- Easiest to debug
- Most reliable
- Best performance

**For existing setups**: 
- If it works, keep it
- If having issues, migrate to DiaryArrangement

---

## Summary

✅ **3 versions available**
✅ **Room09_Interactable supports all**
✅ **DiaryArrangement recommended for new setups**
✅ **Easy migration path**

Pick the one that works best for you! 🎯
