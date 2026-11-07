using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages slot gameplay, bet menu popup, and auto-fading status messages.
/// </summary>
public class SlotMachineGame : MonoBehaviour
{
    [Header("Reels")]
    [SerializeField] private ReelHandler[] reels;

    [Header("Main UI")]
    [SerializeField] private Button spinTriggerButton; // Opens bet menu
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI balanceText;

    [Header("Bet Menu Popup Panel")]
    [SerializeField] private GameObject betMenuPanel;
    [SerializeField] private Button bet10Button;
    [SerializeField] private Button bet50Button;
    [SerializeField] private Button bet100Button;
    [SerializeField] private Button exitBetMenuButton;

    [Header("Lever Animation")]
    [SerializeField] private Image leverImage;
    [SerializeField] private Sprite leverUpSprite;
    [SerializeField] private Sprite leverDownSprite;

    [Header("Assets & Economy")]
    [SerializeField] private int playerBalance = 1000;
    [SerializeField] private Sprite[] symbolSprites;

    private int selectedBetAmount = 50;
    private bool isGameActive = false;
    private Coroutine fadeRoutine;

    private void Start()
    {
        UpdateUI();
        
        // Hide bet menu and status text initially
        if (betMenuPanel != null) betMenuPanel.SetActive(false);
        if (statusText != null) statusText.gameObject.SetActive(false);

        // Bind button events
        spinTriggerButton.onClick.AddListener(OpenBetMenu);
        bet10Button.onClick.AddListener(() => ConfirmBet(10));
        bet50Button.onClick.AddListener(() => ConfirmBet(50));
        bet100Button.onClick.AddListener(() => ConfirmBet(100));
        exitBetMenuButton.onClick.AddListener(CloseBetMenu);
    }

    private void OpenBetMenu()
    {
        if (isGameActive) return;
        if (betMenuPanel != null) betMenuPanel.SetActive(true);
    }

    private void CloseBetMenu()
    {
        if (betMenuPanel != null) betMenuPanel.SetActive(false);
    }

    private void ConfirmBet(int amount)
    {
        CloseBetMenu();

        if (playerBalance < amount)
        {
            ShowStatusMessage("NOT ENOUGH BALANCE!");
            return;
        }

        selectedBetAmount = amount;
        StartCoroutine(RunSpinSequence());
    }

    private IEnumerator RunSpinSequence()
    {
        isGameActive = true;
        spinTriggerButton.interactable = false;
        ShowStatusMessage($"SPINNING ({selectedBetAmount}G)...");

        // Deduct bet and update balance
        playerBalance -= selectedBetAmount;
        UpdateUI();

        // Pull lever down
        SetLeverState(true);

        // Start reel spinning loops
        foreach (var reel in reels)
        {
            reel.StartSpinning(symbolSprites);
        }

        // Staggered stop sequence for tension
        yield return new WaitForSeconds(1.2f);
        int r1 = reels[0].StopReel(Random.Range(0, symbolSprites.Length));

        yield return new WaitForSeconds(0.6f);
        int r2 = reels[1].StopReel(Random.Range(0, symbolSprites.Length));

        yield return new WaitForSeconds(0.6f);
        int r3 = reels[2].StopReel(Random.Range(0, symbolSprites.Length));

        // Push lever back up
        SetLeverState(false);

        // Evaluate win condition
        EvaluateWin(r1, r2, r3);

        isGameActive = false;
        spinTriggerButton.interactable = true;
    }

    private void SetLeverState(bool pulledDown)
    {
        if (leverImage != null)
        {
            leverImage.sprite = pulledDown ? leverDownSprite : leverUpSprite;
        }
    }

    private void EvaluateWin(int s1, int s2, int s3)
    {
        if (s1 == s2 && s2 == s3)
        {
            int winnings = selectedBetAmount * 10; // 10x multiplier payout
            playerBalance += winnings;
            ShowStatusMessage($"JACKPOT! Won {winnings}G!");
        }
        else
        {
            ShowStatusMessage("TRY AGAIN!");
        }
        UpdateUI();
    }

    private void ShowStatusMessage(string message)
    {
        if (statusText == null) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        statusText.text = message;
        statusText.gameObject.SetActive(true);
        
        // Reset alpha to fully visible
        Color c = statusText.color;
        c.a = 1f;
        statusText.color = c;

        // Start fade out after 2.5 seconds
        fadeRoutine = StartCoroutine(FadeOutStatusRoutine());
    }

    private IEnumerator FadeOutStatusRoutine()
    {
        yield return new WaitForSeconds(2.5f);

        float fadeDuration = 1.0f;
        float elapsed = 0f;
        Color startColor = statusText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            
            // Pixelated/Stepped alpha effect emulation if desired, or smooth fade:
            statusText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        statusText.gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        balanceText.text = $"Balance: {playerBalance}G";
    }
}