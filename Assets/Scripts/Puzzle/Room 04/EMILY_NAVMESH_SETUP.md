# Emily NavMesh Setup - Quick Guide (Tagalog)

## PROBLEMA: Emily naiistuck sa island, hindi gumagana ang states

## SOLUSYON: 3 Steps Lang!

---

## STEP 1: Add NavMesh Obstacle sa Island

### Sa Unity Editor:
1. **Hanapin ang Island** sa Hierarchy (Room04_KitchenDining scene)
   - Pwedeng "Island", "KitchenIsland", "Counter", etc.

2. **Select ang Island GameObject**

3. **Add Component**:
   - Click **Add Component** button
   - Type: `NavMesh Obstacle`
   - Press Enter

4. **Configure ang NavMesh Obstacle**:
   ```
   Inspector Settings:
   ├─ Shape: Box
   ├─ Center: (0, 0, 0)
   ├─ Size: 
   │   ├─ X: [width ng island + 0.5]
   │   └─ Y: [height ng island + 0.5]
   ├─ ✓ Carve (DAPAT CHECKED!)
   └─ Move Threshold: 0.1
   ```

   **CRITICAL**: Ang **Carve** checkbox ay DAPAT naka-check!

---

## STEP 2: Rebake ang NavMesh

### Sa Unity Editor:
1. **Open Navigation Window**:
   - Menu: `Window > AI > Navigation`

2. **Go to Bake Tab**

3. **Verify Settings**:
   ```
   Agent Radius: 0.3
   Agent Height: 1.0
   Max Slope: 45
   Step Height: 0.4
   ```

4. **Click BAKE Button** (bottom right)

5. **Verify sa Scene View**:
   - Dapat makita mo ang **blue NavMesh** sa floor
   - Ang **island area ay WALANG blue** (carved out)
   - Kung may blue pa rin sa island, check kung naka-check ang Carve

---

## STEP 3: Test in Play Mode

### Test Sequence:
1. **Play the scene**
2. **Trigger Emily intro** (walk to trigger area)
3. **Watch Emily**:
   - ✓ Dapat mag-spawn si Emily
   - ✓ Dapat mag-play ang intro (push Lisa, dialogue)
   - ✓ Dapat mag-Hunt or Search si Emily after intro
   - ✓ Dapat HINDI pumunta sa island si Emily
   - ✓ Dapat gumagana ang lahat ng states (Patrol, Hunt, Search, Cooldown)

### Check Console Logs:
```
Expected logs:
[KitchenController] Emily AI fully enabled. State: Hunt
[EMILY] State -> Hunt
[EMILY] State -> Search (if player hides)
[EMILY] State -> Patrol (after cooldown)
```

### If you see ERROR:
```
[Emily] Not on NavMesh! Attempting recovery...
[Emily] Recovered to NavMesh position: (x, y, z)
```
**MEANING**: Emily tried to go somewhere without NavMesh
**ACTION**: Check if may area na walang NavMesh Obstacle

---

## COMMON ISSUES

### Issue 1: Emily pa rin pumupunta sa island
**CAUSE**: Carve is not checked
**FIX**: 
1. Select Island GameObject
2. Check ang NavMesh Obstacle component
3. Make sure **Carve** is ✓ checked
4. Rebake NavMesh

### Issue 2: Emily hindi gumagalaw after intro
**CAUSE**: NavMesh agent not properly enabled
**FIX**: Already fixed in code - just rebake NavMesh

### Issue 3: Emily nawawala or teleporting
**CAUSE**: NavMesh may holes or gaps
**FIX**:
1. Open Navigation window
2. Check Scene view for blue NavMesh
3. Make sure walang gaps sa floor
4. Rebake if needed

---

## VERIFICATION CHECKLIST

Before testing:
- [ ] Island has NavMesh Obstacle component
- [ ] Carve is checked in NavMesh Obstacle
- [ ] NavMesh is baked (blue in Scene view)
- [ ] Island area has NO blue NavMesh

During testing:
- [ ] Emily spawns correctly
- [ ] Emily intro plays
- [ ] Emily AI activates after intro
- [ ] Emily does NOT go to island
- [ ] All 4 states work properly

After retry:
- [ ] Scene reloads properly
- [ ] Emily intro plays again
- [ ] Emily AI works again
- [ ] No stuck issues

---

## CODE CHANGES SUMMARY

### Files Updated:
1. **KitchenRoomController.cs**
   - Added WaitForEndOfFrame before enabling Emily AI
   - Ensures NavMeshAgent is ready before setting state

2. **EmilyMovement.cs**
   - Added safety check for off-NavMesh situations
   - Auto-recovery if Emily gets stuck

3. **GameOverManager.cs**
   - Full puzzle reset on retry
   - Clears all kitchen progress and items

### No Code Changes Needed For:
- **EmilyGhost.cs** - Already has proper state machine
- **KitchenChaseTrigger.cs** - Already handles existing Emily

---

## QUICK DEBUG

### In Play Mode, press these keys:
- **R** (while in Kitchen scene) - Reset all kitchen progress
- **F5** - Quick save
- **F9** - Quick load

### Console Filter:
Type in Console search: `[Emily]` or `[KitchenController]`

---

## FINAL NOTES

Ang main issue ay ang **NavMesh** - kailangan ng **NavMesh Obstacle** sa island para hindi makapasok si Emily. Ang code fixes ay para lang sa proper initialization at recovery.

**MOST IMPORTANT**: 
1. Add NavMesh Obstacle sa island
2. Check ang Carve checkbox
3. Rebake NavMesh

Yan lang! Good luck! 🎮
