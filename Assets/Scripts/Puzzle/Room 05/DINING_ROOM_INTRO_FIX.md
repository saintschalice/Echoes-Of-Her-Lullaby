# Dining Room - Intro Before Chase Fix

## 🐛 Problem

Player was being force-moved (knockback) BEFORE intro dialogue, causing confusion.

## ✅ Fix Applied

Updated `EmilyGetsAngrySequence()` to:
1. **Show intro dialogue FIRST** (R05_ANGRY_1)
2. **Wait for dialogue to finish**
3. **Then jumpscare + knockback**
4. **Then chase starts**

---

## 🎮 New Flow

### Before Fix:
```
Calendar closed
   ↓
[IMMEDIATE KNOCKBACK] ← Player confused!
   ↓
Dialogue
   ↓
Emily chases
```

### After Fix:
```
Calendar closed
   ↓
Intro dialogue (R05_ANGRY_1) ← Player understands what's happening
   ↓
Wait for dialogue to finish
   ↓
[JUMPSCARE + KNOCKBACK] ← Now makes sense!
   ↓
Dialogue (R05_ANGRY_2)
   ↓
Emily chases
```

---

## 📝 What Changed

### In `EmilyGetsAngrySequence()`:

**Added**:
- Intro dialogue (R05_ANGRY_1) at the start
- Wait for dialogue to finish before knockback
- Proper sequence: Dialogue → Jumpscare → Chase

**Result**: Player gets context before being knocked back!

---

## 🎯 Testing

### Test 1: Calendar Interaction
1. Interact with calendar
2. Close calendar
3. **Expected**: Intro dialogue plays FIRST
4. **Expected**: Dialogue finishes
5. **Expected**: THEN jumpscare + knockback
6. **Expected**: Emily starts chasing

### Test 2: No Force Move During Dialogue
1. Intro dialogue should play
2. **Expected**: Player NOT moving during dialogue
3. **Expected**: Player can read dialogue
4. **Expected**: AFTER dialogue, knockback happens

---

## 💡 Why This Matters

**Before**: Player was confused - "Why am I being pushed?"
**After**: Player understands - Dialogue explains, then action happens

**Better player experience!** ✅

---

## 🎯 Summary

**Fix**: Intro dialogue BEFORE knockback
**Result**: Player understands what's happening
**No more confusion!** 🎯✨
