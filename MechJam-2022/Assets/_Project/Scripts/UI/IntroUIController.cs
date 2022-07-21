using Gisha.Effects.Audio;
using UnityEngine;

namespace Gisha.MechJam.UI
{
    public class IntroUIController : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject[] pages;

        private int _pageIndex;

        private void Awake()
        {
            popup.SetActive(true);
            _pageIndex = 0;
            pages[_pageIndex].SetActive(true);
        }

        public void OnClick_Ok()
        {
            pages[_pageIndex].SetActive(false);
            _pageIndex++;
            if (_pageIndex < pages.Length)
                pages[_pageIndex].SetActive(true);
            else
                popup.SetActive(false);
            
            AudioManager.Instance.PlaySFX("click");
        }
    }
}