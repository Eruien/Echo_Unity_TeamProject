using UnityEngine;

public class CharacterEchoEvent : MonoBehaviour
{
    private MovePlayer playerCharacter;
    private GameObject playerCane;
    
    // 에코 횟수 조절용 Count 변수들
    [SerializeField]
    private float endStopEchoTime = 2.0f;

    private int caneEchoCountMax = 1;
    private int caneEchoCount = 0;
    private int walkEchoCountMax = 1;
    private int walkEchoCount = 0;
    private int runEchoCountMax = 1;
    private int runEchoCount = 0;
    private float startStopEchoTime = 0;

    // 플레이어와 지팡이에 에코적용을 위해서 컴포넌트 참조를 가져옴
    private void Awake()
    {
        Transform[] allObjects = transform.GetComponentsInChildren<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.name == "cane")
            {
                playerCane = obj.gameObject;
                break;
            }
        }
        playerCharacter = transform.parent.GetComponent<MovePlayer>();
    }

    // 플레이어가 멈춰있을땐 에코를 뿌리지 않음
    private void Update()
    {
        if (playerCharacter.MoveDirection == Vector3.zero)
        {
            startStopEcoTime += Time.deltaTime;

            if (startStopEcoTime > endStopEcoTime)
            {
                startStopEcoTime = 0.0f;
                StopEvent();
            }
        }
        else
        {
            startStopEcoTime = 0.0f;
        }
    }

    // 플레이어 쪽에서 이벤트를 전달받아 에코를 멈춤
    public void StopEvent()
    {
        if (!playerCharacter.IsEco) return;

        playerCharacter.StopEco(playerCharacter.transform);
    }

    // 지팡이 소리와 에코 횟수 조절
    public async void CaneEvent()
    {
        if (!playerCharacter.IsEco) return;
        GameManager.Instance.SoundManager.PlaySound2D("Sound/caneSound", SoundType.SFX, 1);

        caneEcoCount = 0;

        while (caneEcoCount < caneEcoCountMax)
        {
            await UniTask.Delay(600);
            playerCharacter.CaneEco(playerCane.transform);
            caneEcoCount++;
        }
    }

    // 걸을 때 에코 횟수 조절
    public async void WalkEvent()
    {
        if (!playerCharacter.IsEco) return;
     
        walkEcoCount = 0;

        while (walkEcoCount < walkEcoCountMax)
        {
            await UniTask.Delay(600);
            playerCharacter.WalkEco(playerCharacter.transform);
            walkEcoCount++;
        }
    }

    // 달릴 때 에코 횟수 조절
    public async void RunEvent()
    {
        if (!playerCharacter.IsEco) return;
       
        runEcoCount = 0;

        while (runEcoCount < runEcoCountMax)
        {
            await UniTask.Delay(800);
            playerCharacter.RunEco(playerCharacter.transform);
            runEcoCount++;
        }
    }

    // 걸을 때 소리 이벤트
    public void WalkSoundEvent()
    {
        GameManager.Instance.SoundManager.PlaySound2D("Sound/walk", SoundType.SFX, 0.2f, 1.0f);
    }

    // 달릴 때 소리 이벤트
    public void RunSoundEvent()
    {
        GameManager.Instance.SoundManager.PlaySound2D("Sound/walk", SoundType.SFX, 0.2f, 1.0f);
    }
}
