using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour
{
    public Transform monster;
    public Transform princess;
    public Transform hero;
    public DialogueManager dialogue;

    private bool clicked = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked = true;
        }
    }

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator WaitForClick()
    {
        clicked = false;
        while (!clicked)
            yield return null;
    }

    IEnumerator PlayIntro()
    {
        // Quái bay vào
        yield return Move(monster, new Vector3(3f, monster.position.y), 2f);

        dialogue.Show("", "<align=center><b>Khi màn đêm buông xuống vương quốc yên bình...</b></align>");
        yield return WaitForClick();

        dialogue.Show("Monster", "Cuối cùng… ta đã tìm thấy nàng, công chúa");
        yield return WaitForClick();

        dialogue.Show("Princess", "HeroKnight… cứu ta với…");
        yield return WaitForClick();

        // Bắt công chúa
        princess.SetParent(monster);

        dialogue.Show("Monster", "Vẻ đẹp này thuộc về bóng tối. Ngươi sẽ theo ta.");
        yield return WaitForClick();

        dialogue.Show("HeroKnight", "Hãy buông nàng ra!");
        yield return WaitForClick();

        // Quái bay đi
        yield return Move(monster, new Vector3(10f, monster.position.y), 2f);

        dialogue.Show("HeroKnight", "Dù ngươi trốn trong lâu đài hắc ám… ta vẫn sẽ tìm ra.");
        yield return WaitForClick();

        dialogue.Show("Monster", "Vậy hãy đến đi, hiệp sĩ. Ta sẽ chờ ngươi trong bóng tối.");
        yield return WaitForClick();

        dialogue.Show("", "<align=center><b>Hành trình đến lâu đài hắc ám bắt đầu…</b></align>");
        yield return WaitForClick();

        dialogue.Hide();

        PlayerPrefs.SetInt("HasSeenIntro", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("LV1");
    }

    IEnumerator Move(Transform obj, Vector3 target, float time)
    {
        Vector3 start = obj.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            obj.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}
