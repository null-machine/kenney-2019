using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// god class but whatever 
// game jams are about as maintainable as my comp tech average
public class Scorekeeper : MonoBehaviour {
  
  [SerializeField]
  private SpriteRenderer arena;
  [SerializeField]
  private Camera cam;
  [SerializeField]
  private Animator animator;
  
  [SerializeField]
  private Animator filter;
  [SerializeField]
  private MainCamera mainCam;
  
  [SerializeField]
  private Color[] arenaColors;
  [SerializeField]
  private Color[] backgroundColors;
  
  [Header("UI stuff")]
  [SerializeField]
  private Text text;
  [SerializeField]
  private Color[] ranks;
  [SerializeField]
  private Animator textAnim;
  
  [SerializeField]
  private Player player;
  
  private int index;
  private int deaths;
  
  internal void SetColor(string colorName) {
    switch (colorName) {
      case "Red": index = 0; break;
      case "Yellow": index = 1; break;
      case "Blue": index = 2; break;
      case "Green": index = 3; break; 
    } // should use layer bitmask
    deaths++;
    if (deaths == 1) {
      animator.SetTrigger("DeathOne");
    } else if (deaths == 2) {
      animator.SetTrigger("DeathTwo");
    }
    if (deaths > 2) {
      EndGame();
    }
  }
  
  internal void EndGame() {
    StartCoroutine(CoruEndGame());
  }
  
  private IEnumerator CoruEndGame() {
    filter.SetTrigger("Fade");
    yield return new WaitForSecondsRealtime(0.5f);
    mainCam.Freeze();
    Time.timeScale = 0f;
    
    text.color = ranks[deaths - 1];
    switch (deaths) {
      case 1: text.text = "FOURTH"; break;
      case 2: text.text = "THIRD"; break;
      case 3:
        if (player.alive) {
          text.text = "FIRST!";
          text.color = ranks[3];
        } else {
          text.text = "SECOND";
        }
      break;
    }
    
    // change text and colour
    textAnim.SetTrigger("End");
    
    yield return new WaitForSecondsRealtime(1f);
    StartCoroutine(Restart());
  }
  
  private IEnumerator Restart() {
    while (!Input.GetButton("Shoot")) {
      yield return null;
    }
    Time.timeScale = 1.0f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  
  private void Start() {
    index = 4;
    deaths = 0;
  }
  
  private void Update() {
    arena.color = Color.Lerp(arena.color, arenaColors[index], 0.1f);
    cam.backgroundColor = Color.Lerp(cam.backgroundColor, backgroundColors[index], 0.1f);
  }
}
