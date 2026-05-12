# Room 07 - Bathroom Door Setup (Tagalog)

## 🚪 OVERVIEW

Ang bathroom door ay **LOCKED** hanggang sa matapos lahat ng puzzles sa Lisa's Bedroom.

**Kailangan**:
- ✅ Lahat ng environmental checks
- ✅ Lahat ng puzzles solved
- ✅ Mirror interaction HINDI required (door unlocks BEFORE mirror)

---

## 🔧 PAANO I-SETUP

### Step 1: Create Bathroom Door

1. **Create GameObject**: `BathroomDoor`
2. **Position**: Sa location ng bathroom door
3. **Add Collider2D**:
   - BoxCollider2D or CircleCollider2D
   - **Is Trigger**: ✅ CHECKED
   - **Size**: 1.5-2.0

### Step 2: Add Script

1. **Add Component**: `Room07_BathroomDoor`
2. **Assign sa Inspector**:
   - **Bathroom Scene Name**: "Room08_Lisa'sBathroom"
   - **Locked Dialogue**: "The door is locked. I need to finish what I came here for first."
   - **Unlocked Dialogue**: "The door... it's open now. The bathroom. Where it all ended."
   - **Locked Sound**: Audio clip
   - **Unlock Sound**: Audio clip
   - **Door Open Sound**: Audio clip
   - **Debug Mode**: ✅

---

## 🎯 ANO ANG MANGYAYARI

### Pag LOCKED ang Door:

**Conditions**: Hindi pa tapos lahat ng puzzles

**Flow**:
1. Player lumapit sa door
2. **Check**: Tapos na ba lahat?
3. **Result**: HINDI PA!
4. **Play**: Locked sound
5. **Show**: "The door is locked..."
6. **Hindi makapasok** sa bathroom

### Pag UNLOCKED ang Door:

**Conditions**: Tapos na lahat ng puzzles!

**Kailangan Tapos**:
- ✅ Bed
- ✅ Wall
- ✅ Diary
- ✅ Curtains
- ✅ Cup
- ✅ Tea Party
- ✅ Chair
- ✅ Closet
- ✅ Toybox
- ✅ Doll
- ✅ Dollhouse
- ✅ Reading Table

**Flow (First Time)**:
1. Player lumapit sa door
2. **Check**: Tapos na ba lahat?
3. **Result**: OO, TAPOS NA!
4. **Play**: Unlock sound
5. **Show**: "The door... it's open now..."
6. **Wait**: Dialogue finish
7. **Play**: Door open sound
8. **Fade to black**
9. **Load bathroom scene**
10. **Fade from black**

**Flow (Next Times)**:
1. Player lumapit sa door
2. Door unlocked na
3. **Skip**: Unlock dialogue
4. **Play**: Door open sound
5. **Fade to black**
6. **Load bathroom**

---

## 📋 CHECKLIST

Ang door ay mag-unlock kapag **LAHAT** ng ito ay tapos:

### Environmental:
- [ ] Bed
- [ ] Wall Drawings
- [ ] Diary
- [ ] Chair
- [ ] Closet
- [ ] Reading Table

### Puzzles:
- [ ] Curtains
- [ ] Cup (obtained)
- [ ] Tea Party
- [ ] Toybox
- [ ] Doll (obtained)
- [ ] Dollhouse

**NOTE**: Mirror HINDI required! Door unlocks BEFORE mirror.

---

## 🎨 VISUAL FEEDBACK

**Sa Scene View**:
- **Red gizmo** = Locked (puzzles incomplete)
- **Green gizmo** = Unlocked (puzzles complete)

---

## 🔊 AUDIO

**Kailangan**:
1. **Locked Sound** - Door rattle
2. **Unlock Sound** - Lock clicking
3. **Door Open Sound** - Door creaking

---

## ✅ TESTING

### Test Locked:
1. Start game
2. Lumapit sa door (walang completed puzzles)
3. **Expected**:
   - ✅ Locked sound
   - ✅ Locked dialogue
   - ✅ Hindi makapasok
   - ✅ Red gizmo

### Test Unlocked:
1. Complete lahat ng puzzles
2. Lumapit sa door
3. **Expected**:
   - ✅ Unlock sound
   - ✅ Unlock dialogue
   - ✅ Door open sound
   - ✅ Fade to black
   - ✅ Load bathroom
   - ✅ Green gizmo

---

## 🐛 KUNG MAY PROBLEMA

### "Door laging locked"
- Check kung tapos na lahat ng puzzles
- Enable Debug Mode
- Check Console for status
- Verify FlowController flags

### "Door laging unlocked"
- Check kung attached ang script
- Verify IsDoorUnlocked() is called
- Check Console logs

### "Walang dialogue"
- Check DialogueSystemV2
- Verify dialogue text assigned
- Check Console for errors

### "Walang sound"
- Assign audio clips
- Check AudioManager
- Test clips directly

### "Hindi nag-load ang scene"
- Check scene name: "Room08_Lisa'sBathroom"
- Add scene sa Build Settings
- Check Console for errors

---

## 💡 TIPS

### Para sa Better Experience:
- Use distinct sounds
- Clear dialogue
- Smooth transitions (0.8s fade)
- Unlock dialogue first time only

### Para sa Testing:
- Enable Debug Mode
- Check Console for puzzle status
- Test with all flags true
- Reset after testing

---

## 📝 ALTERNATIVE

Kung gusto mo unlock AFTER mirror:

```csharp
private bool IsDoorUnlocked()
{
    Room07_FlowController flow = Room07_FlowController.Instance;
    if (flow == null) return false;
    
    // Unlock after mirror
    return flow.hasInteractedWithMirror;
}
```

**Current**: Door unlocks BEFORE mirror (after all puzzles).

---

**Setup complete! Test mo na!** 🚪✨
