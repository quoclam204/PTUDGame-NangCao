using UnityEngine;
using UnityEngine.UI;
public class HydrationBar : MonoBehaviour
{
    private Slider slider;
    public Text hydrationCounter;
    public GameObject playerState;
    private float currentHydration, maxHydration;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHydration = playerState.GetComponent<PlayerState>().currentHydration;
        maxHydration = playerState.GetComponent<PlayerState>().maxHydration;
        float fillValue = currentHydration / maxHydration;
        slider.value = fillValue;
        hydrationCounter.text = currentHydration + "%";
    }
}
