using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles vertical scrolling animation for a slot reel column.
/// </summary>
public class ReelHandler : MonoBehaviour
{
    [Header("UI Slots (Top, Middle, Bottom)")]
    [SerializeField] private Image topSymbolImage;
    [SerializeField] private Image middleSymbolImage; // Winning payline
    [SerializeField] private Image bottomSymbolImage;

    [Header("Spin Settings")]
    [SerializeField] private float scrollSpeed = 0.03f;

    private Sprite[] reelSymbols;
    private bool isSpinning = false;

    /// <summary>
    /// Starts the vertical scrolling reel animation.
    /// </summary>
    public void StartSpinning(Sprite[] symbols)
    {
        reelSymbols = symbols;
        isSpinning = true;
        StartCoroutine(VerticalScrollRoutine());
    }

    private IEnumerator VerticalScrollRoutine()
    {
        while (isSpinning)
        {
            // Shift symbols downward visually to simulate vertical reel rolling
            int r1 = Random.Range(0, reelSymbols.Length);
            int r2 = Random.Range(0, reelSymbols.Length);
            int r3 = Random.Range(0, reelSymbols.Length);

            topSymbolImage.sprite = reelSymbols[r1];
            middleSymbolImage.sprite = reelSymbols[r2];
            bottomSymbolImage.sprite = reelSymbols[r3];

            yield return new WaitForSeconds(scrollSpeed);
        }
    }

    /// <summary>
    /// Stops the reel on the final target index for the middle row.
    /// </summary>
    public int StopReel(int targetMiddleIndex)
    {
        isSpinning = false;

        // Ensure correct cyclic alignment for top and bottom adjacent rows
        int topIndex = (targetMiddleIndex - 1 + reelSymbols.Length) % reelSymbols.Length;
        int bottomIndex = (targetMiddleIndex + 1) % reelSymbols.Length;

        topSymbolImage.sprite = reelSymbols[topIndex];
        middleSymbolImage.sprite = reelSymbols[targetMiddleIndex];
        bottomSymbolImage.sprite = reelSymbols[bottomIndex];

        return targetMiddleIndex;
    }
}