using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    public Button btn;
    public TMP_Text cardNameText;
    public TMP_Text cardDescText;

    private BufferCard _card;
    private System.Action<BufferCard> _onSelect;

    public void Init(BufferCard card, System.Action<BufferCard> onSelect)
    {
        _card = card;
        _onSelect = onSelect;

        cardNameText.text = card.cardName;
        cardDescText.text = card.description;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => _onSelect?.Invoke(_card));
    }
}
