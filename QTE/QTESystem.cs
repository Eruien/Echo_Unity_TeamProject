using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class QTESystem : MonoBehaviour, INotifyPropertyChanged
{
    // QTE Scene을 종료하기 위한 트리거
    [SerializeField]
    private CutSceneTrigger cutSceneTrigger;
   // 프로그래스바 조절용 변수
    [SerializeField]
    private float progressbar = 0.0f;
    [SerializeField]
    private float progressbarInitialValue = 50.0f;
    [SerializeField]
    private float progressbarComplete = 100.0f;
    [SerializeField]
    private float progressbarFail = 0.0f;
    [SerializeField]
    private float QTEReduceRate = 2.5f;
    [SerializeField]
    private float QTERecoveryRate = 5.0f;

    // QTE Scene 시작용 트리거
    [SerializeField]
    private bool QTEStartTrigger = false;

    private bool IsQTEStart = false;

    public event PropertyChangedEventHandler PropertyChanged;

    public float Progressbar
    {
        get { return progressbar; }
        set 
        {
            progressbar = value;
            OnPropertyChanged("Progressbar");
        }
    }

    private void Awake()
    {
        Progressbar = progressbarInitialValue;
    }

    // QTE 이벤트 체크
    private void Update()
    {
        if (QTEStartTrigger)
        {
            QTEStart();
            QTEStartTrigger = false;
        }

        if (IsQTEStart)
        {
            CheckQTE();
        }
        else
        {
            Progressbar = progressbarInitialValue;
        }
    }

    // QTE 시작때 UI를 보여주기
    public void QTEStart()
    {
        GameManager.Instance.UIManager.ShowUI<QTEUI>("UI/QTE UI");
        IsQTEStart = true;
    }

    // QTE 성공과 실패 체크
    void CheckQTE()
    {
        Progressbar = progressbar - (QTEReduceRate * Time.deltaTime);
        
        if (progressbar >= progressbarComplete)
        {
            Progressbar = progressbarInitialValue;
            IsQTEStart = false;
         
            cutSceneTrigger.FinishCutScene();
            GameManager.Instance.UIManager.CloaseCurrentUI();
        }
        else if (progressbar <= progressbarFail)
        {
            Progressbar = progressbarInitialValue;
            IsQTEStart = false;
            
            cutSceneTrigger.TrueEndingScene();
            GameManager.Instance.UIManager.CloaseCurrentUI();
        }
    }

    // 플레이어가 QTE를 눌렀을때 회복
    public void OnQTEPush(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Progressbar += QTERecoveryRate;
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
