using UnityEngine;
using UnityEngine.UI;

public class UIPlayerListComponent : MonoBehaviour
{
    [SerializeField] Text vehicleNameText;
    [SerializeField] Text killCountText;
    [SerializeField] Text nicknameText;

    private int killCount = 0;

    public Text VehicleNameText { get => vehicleNameText;}
    public Text KillCountText { get => killCountText;}
    public Text NicknameText { get => nicknameText;}

    private void Awake()
    {
        killCountText.text = killCount.ToString();
    }

    public void AddKillPoint()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }

    public void PlayerDestroyed()
    {
        vehicleNameText.color = Color.gray;
        killCountText.color = Color.gray;
        nicknameText.color = Color.gray;
    } 
}
