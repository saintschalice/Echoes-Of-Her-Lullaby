using UnityEngine;

public class TriggerFinalChase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && Room05_DiningRoomController.Instance != null)
        {
            bool isPuzzleDone = Room05_DiningRoomController.Instance.puzzleCompleted;
            bool isEmilyGone = !Room05_DiningRoomController.Instance.isEmilyHunting;

            // Kapag tapos na ang puzzle, saka lang talaga aatake si Emily
            if (isPuzzleDone && isEmilyGone)
            {
                Debug.Log("[FinalChase] Starting Final Chase sequence.");
                Room05_DiningRoomController.Instance.OnTriggerExitRoom();
                gameObject.SetActive(false);
            }
            // Tinanggal na natin yung "else" para tahimik na lang siya kapag di pa tapos ang puzzle
        }
    }
}