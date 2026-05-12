# Photo Frame Interaction Fix

## ❌ Problem: Hindi ma-interact ang photo frame

---

## ✅ Solution Checklist

### 1. Check GameObject Setup

**PhotoFrame GameObject must have:**
- ✅ `Room06_PhotoFrameInteractable` script component
- ✅ `Collider2D` component (BoxCollider2D or CircleCollider2D)
- ✅ Collider **Is Trigger** = ✅ CHECKED
- ✅ Layer = **Interactable** (or default)
- ✅ Tag = (no specific tag needed)

### 2. Check Collider Settings

**In Inspector:**
```
Box Collider 2D (or Circle Collider 2D):
├─ Is Trigger: ☑ (MUST BE CHECKED!)
├─ Size: Large enough for player to reach
└─ Offset: Centered on sprite
```

**Recommended Size:**
- Width: 1.5 - 2.0
- Height: 1.5 - 2.0

### 3. Check Script Component

**Room06_PhotoFrameInteractable:**
```
Debug Mode: ☑ (Enable to see logs)
```

### 4. Check Room Controller

**Room06_HallwayController must exist in scene:**
- GameObject name: `Room06_HallwayController`
- Script: `Room06_HallwayController.cs`
- Photo Frame reference assigned

---

## 🔍 Debug Steps

### Step 1: Check Console Logs

When you approach the photo frame, you should see:
```
[PhotoFrame] Player focused on photo frame
```

When you press interact button, you should see:
```
[PhotoFrame] OnInteract called!
```

### Step 2: If No Logs Appear

**Problem:** Player interaction system not detecting the object

**Solutions:**
1. Check if collider **Is Trigger** is checked
2. Check if collider size is large enough
3. Check if player is close enough
4. Check if GameObject is active in Hierarchy

### Step 3: If "OnInteract called!" appears but nothing happens

**Problem:** Room controller not found or not set up

**Solutions:**
1. Check if `Room06_HallwayController` GameObject exists
2. Check if script component is attached
3. Check if photo frame reference is assigned in controller

---

## 🎮 Testing in Unity

### Test 1: Collider Visualization

1. Select PhotoFrame in Hierarchy
2. Look at Scene view (not Game view)
3. You should see green outline (collider)
4. Make sure it's large enough

### Test 2: Player Approach

1. Play the game
2. Walk towards photo frame
3. Check Console for "Player focused on photo frame"
4. If no message, collider is too small or not trigger

### Test 3: Interaction

1. Stand near photo frame
2. Press interact button (E or UI button)
3. Check Console for "OnInteract called!"
4. If no message, interaction system issue

---

## 🔧 Common Issues & Fixes

### Issue 1: "Is Trigger" not checked
**Symptom:** No focus/blur messages in Console
**Fix:** 
1. Select PhotoFrame
2. Find Collider component
3. Check "Is Trigger" checkbox

### Issue 2: Collider too small
**Symptom:** Focus message only appears when very close
**Fix:**
1. Select PhotoFrame
2. Increase collider size to 1.5 or 2.0
3. Test again

### Issue 3: Room Controller not found
**Symptom:** "Room06_HallwayController not found!" error
**Fix:**
1. Create GameObject: `Room06_HallwayController`
2. Add `Room06_HallwayController` script
3. Assign photo frame reference

### Issue 4: Script not attached
**Symptom:** Nothing happens, no errors
**Fix:**
1. Select PhotoFrame
2. Add Component → `Room06_PhotoFrameInteractable`
3. Enable Debug Mode

### Issue 5: GameObject inactive
**Symptom:** Can't interact at all
**Fix:**
1. Check Hierarchy
2. Make sure PhotoFrame checkbox is checked (active)

---

## 📋 Complete Setup Checklist

### PhotoFrame GameObject:
- [ ] GameObject is active (checkbox checked)
- [ ] Has `Room06_PhotoFrameInteractable` script
- [ ] Has `Collider2D` component
- [ ] Collider "Is Trigger" is checked
- [ ] Collider size is 1.5-2.0
- [ ] Debug Mode is enabled

### Room06_HallwayController:
- [ ] GameObject exists in scene
- [ ] Has `Room06_HallwayController` script
- [ ] Photo Frame reference is assigned
- [ ] Normal Photo Sprite is assigned
- [ ] Scratched Photo Sprite is assigned

### Testing:
- [ ] Approach photo frame → See focus message
- [ ] Press interact → See interact message
- [ ] Photo scratches → Sprite changes
- [ ] Emily spawns → Chase starts

---

## 🎯 Quick Fix

If nothing works, try this:

1. **Delete PhotoFrame GameObject**
2. **Create new GameObject: "PhotoFrame"**
3. **Add components:**
   - Sprite Renderer (with photo sprite)
   - Box Collider 2D
   - Room06_PhotoFrameInteractable script
4. **Configure Box Collider 2D:**
   - Is Trigger: ✅
   - Size: (1.5, 1.5)
5. **Configure script:**
   - Debug Mode: ✅
6. **Test!**

---

## 💡 Debug Commands

Add this to Room06_PhotoFrameInteractable for testing:

```csharp
private void Update()
{
    // Press T to manually trigger interaction
    if (Input.GetKeyDown(KeyCode.T))
    {
        Debug.Log("[PhotoFrame] Manual trigger!");
        OnInteract(null);
    }
}
```

**Press T key to test interaction without player!**

---

## ✅ Expected Behavior

### When Working Correctly:

1. **Approach photo frame:**
   - Console: "[PhotoFrame] Player focused on photo frame"

2. **Press interact button:**
   - Console: "[PhotoFrame] OnInteract called!"
   - Dialogue: "A family photo... they look happy."

3. **After dialogue:**
   - Photo sprite changes to scratched version
   - Scratch sound plays
   - More dialogue appears

4. **After 1.5 seconds:**
   - Emily spawns
   - Chase begins!

---

## 🐛 Still Not Working?

### Check these:

1. **Player has interaction system?**
   - Check if player can interact with other objects
   - If not, player interaction system issue

2. **Correct scene?**
   - Make sure you're in Room 06 scene
   - Not testing in wrong scene

3. **Script compiled?**
   - Check Console for compilation errors
   - Fix any errors first

4. **Unity version?**
   - Some Unity versions have collider bugs
   - Try restarting Unity

---

## 📞 Need More Help?

**Enable Debug Mode and send these:**
1. Console logs when approaching photo frame
2. Console logs when pressing interact
3. Screenshot of PhotoFrame Inspector
4. Screenshot of Room06_HallwayController Inspector

**This will help identify the exact issue!** 🔍✨
