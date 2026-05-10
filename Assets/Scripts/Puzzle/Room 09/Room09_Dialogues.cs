using UnityEngine;

/// <summary>
/// All dialogues for Room 09 - Master Bedroom's Bathroom
/// The climactic puzzle room with 4 mirror puzzles
/// </summary>
public static class Room09_Dialogues
{
    // ==================== ENTRY SEQUENCE ====================
    public static readonly string ENTRY_1 = "I climb through the broken mirror. Glass shards cut deep.";
    public static readonly string ENTRY_2 = "Blood... everywhere. My blood.";
    
    public static readonly string DOOR_SLAMS = "The door slams shut behind me. I'm locked in.";
    public static readonly string TRAPPED = "I'm locked in here with her... and she's not holding back anymore.";
    
    // ==================== EMILY MANIFESTATION ====================
    public static readonly string EMILY_APPEARS_1 = "Emily... she's here. Fully manifested. Solid. Terrifying.";
    public static readonly string EMILY_APPEARS_2 = "The entire bathroom warps around her desperation. Reality itself bends.";
    public static readonly string EMILY_WARNING = "She's not holding back. I need to solve these mirrors before she breaks completely.";
    
    // ==================== MIRROR 1: MEDICINE CABINET ====================
    public static readonly string MIRROR1_EXAMINE = "The medicine cabinet. Prescription bottles scattered everywhere.";
    public static readonly string MIRROR1_HINT = "Different dates... different medications. I need to arrange them chronologically.";
    public static readonly string MIRROR1_SUCCESS_1 = "The bottles align... the mirror shows mother's face.";
    public static readonly string MIRROR1_SUCCESS_2 = "Increasing dosages. Year after year. She was planning this for so long.";
    
    // ==================== MIRROR 2: BATHTUB DRAIN ====================
    public static readonly string MIRROR2_EXAMINE = "The bathtub. Water rises and falls. Something's in the drain.";
    public static readonly string MIRROR2_DRAIN_OPEN = "I remove the drain cover. Torn paper pieces... hidden in the pipes.";
    public static readonly string MIRROR2_HINT = "A note. Torn into pieces. I need to reassemble it.";
    public static readonly string MIRROR2_SUCCESS_1 = "The note is complete. Mother's handwriting.";
    public static readonly string MIRROR2_SUCCESS_2 = "'Tonight I end this child's suffering and mine - forever.' A murder-suicide plan.";
    
    // ==================== MIRROR 3: VANITY TERROR ====================
    public static readonly string MIRROR3_EXAMINE = "The vanity mirror. Diary pages scattered around it.";
    public static readonly string MIRROR3_HINT = "Mother's diary. Fragments of her descent into madness. I need to put them in order.";
    public static readonly string MIRROR3_SUCCESS_1 = "The timeline is complete. I can see it all now.";
    public static readonly string MIRROR3_SUCCESS_2 = "Her defiance... my defiance. The discipline sessions. Emily protecting me. Mother's final plan.";
    
    // ==================== MIRROR 4: EVIDENCE SEQUENCE ====================
    public static readonly string MIRROR4_EXAMINE = "The large mirror. Empty picture frames below it.";
    public static readonly string MIRROR4_HINT = "Evidence scattered around the bathroom. Rope. Pills. Knife. Bloody towel. The order matters.";
    public static readonly string MIRROR4_SUCCESS_1 = "The sequence is complete. Each item shows a flashback.";
    public static readonly string MIRROR4_SUCCESS_2 = "Restraint. Sedation. Murder. Cleanup. She had it all planned out.";
    
    // ==================== EMILY'S BREAKDOWN ====================
    public static readonly string EMILY_BREAKDOWN_1 = "Emily's power... it's breaking. She's becoming translucent.";
    public static readonly string EMILY_BREAKDOWN_2 = "'Every time I saved you, I became more like her!'";
    public static readonly string EMILY_BREAKDOWN_3 = "She's exhausted. Collapsing. The water rises around her.";
    
    // ==================== TRUTH REVEALED ====================
    public static readonly string ALL_MIRRORS_COMPLETE = "All four mirrors show the complete story. The truth I tried to forget.";
    public static readonly string MOTHER_VOICE = "*Mother's voice echoes* 'Tonight I end this child's defiance forever.'";
    public static readonly string DOOR_UNLOCKS = "The master bedroom door... it's unlocking.";
    
    // ==================== EMILY'S FINAL WORDS ====================
    public static readonly string EMILY_WHISPER_1 = "'The mirror in there... it will show you everything I tried to hide.'";
    public static readonly string EMILY_WHISPER_2 = "'I'm sorry, Lisa. I couldn't protect you from the truth.'";
    
    // ==================== FINAL APPROACH ====================
    public static readonly string APPROACH_DOOR_1 = "The master bedroom. Where it all ended.";
    public static readonly string APPROACH_DOOR_2 = "Emily lies collapsed in the flooded bathroom behind me. Powerless.";
    public static readonly string APPROACH_DOOR_3 = "I open the door. The final truth awaits.";
    
    // ==================== PUZZLE HINTS ====================
    public static readonly string HINT_CHRONOLOGICAL = "The dates... I need to arrange them from oldest to newest.";
    public static readonly string HINT_REASSEMBLE = "These pieces fit together. I need to form the complete message.";
    public static readonly string HINT_TIMELINE = "A timeline. From the beginning of her madness to the end.";
    public static readonly string HINT_SEQUENCE = "The order of her plan. How she prepared for that night.";
    
    // ==================== EMILY ATTACKS (GAME OVER) ====================
    public static readonly string EMILY_ATTACK_1 = "Emily's face fills my vision. Screaming. Furious.";
    public static readonly string EMILY_ATTACK_2 = "I can't... I can't think... Everything goes dark.";
    public static readonly string GAME_OVER = "I failed. Emily's desperation consumed us both.";
    
    // ==================== PREREQUISITES ====================
    public static readonly string NEED_SOLVE_MIRRORS = "I need to solve all four mirrors to unlock the master bedroom.";
    public static readonly string MIRROR_INCOMPLETE = "This mirror isn't complete yet. I need to finish the puzzle.";
    
    // ==================== ENDING CUTSCENE (20 DIALOGUES) ====================
    // Final realization (1-3)
    public static readonly string ENDING_1 = "All four mirrors... they show the complete truth.";
    public static readonly string ENDING_2 = "Mother planned everything. The medications. The note. The timeline. The execution.";
    public static readonly string ENDING_3 = "She was going to kill me that night. And herself. A murder-suicide.";
    
    // Understanding Emily (4-6)
    public static readonly string ENDING_4 = "Emily... she saved me. That night, she manifested fully to stop mother.";
    public static readonly string ENDING_5 = "But every time she protected me, she absorbed more of mother's rage. Her methods. Her violence.";
    public static readonly string ENDING_6 = "'I became what I fought against... to keep you alive.'";
    
    // Mother's plan revealed (7-9)
    public static readonly string ENDING_7 = "The rope was to restrain me. The pills to sedate me. The knife to... end it.";
    public static readonly string ENDING_8 = "Mother saw my defiance as a disease. Emily as a demon. Both needed to be eliminated.";
    public static readonly string ENDING_9 = "She couldn't control me anymore. So she decided to end us both.";
    
    // Emily's sacrifice (10-12)
    public static readonly string ENDING_10 = "'I stopped her that night. But I couldn't save her from herself. She took her own life after I intervened.'";
    public static readonly string ENDING_11 = "Emily saved me... but at the cost of becoming a monster herself.";
    public static readonly string ENDING_12 = "'Every scar you carry... I put there trying to protect you the only way I learned how.'";
    
    // Forgiveness (13-15)
    public static readonly string ENDING_13 = "You were never the monster, Emily. You were a child too. Trying to save another child.";
    public static readonly string ENDING_14 = "Mother's violence... it infected us both. But you fought it. You tried to break the cycle.";
    public static readonly string ENDING_15 = "'Thank you... for finally understanding. For finally letting me rest.'";
    
    // Emily fades away (16-18)
    public static readonly string ENDING_16 = "Emily's form... it's fading. Becoming light. Peaceful.";
    public static readonly string ENDING_17 = "She's smiling. For the first time, she looks... free.";
    public static readonly string ENDING_18 = "The bathroom is quiet now. The water still. The mirrors dark.";
    
    // Final words (19-20)
    public static readonly string ENDING_19 = "I understand now. The echoes of her lullaby weren't a threat. They were a cry for help.";
    public static readonly string ENDING_20 = "Rest now, Emily. You've protected me long enough. We're both free now.";
}
