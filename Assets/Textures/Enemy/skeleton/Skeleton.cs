using UnityEngine; // Import UnityEngine để dùng API Unity

public class Skeleton : MonoBehaviour // Class Skeleton kế thừa từ MonoBehaviour
{
    [SerializeField] private Transform rayCast; // Điểm gốc để bắn Raycast
    [SerializeField] public LayerMask raycastMask; // Layer để raycast kiểm tra (ví dụ: Player)
    [SerializeField] public float rayCastLenght; // Độ dài của raycast
    [SerializeField] public float attackDistance; // khoảng cách tối thiểu để tấn công
    [SerializeField] private float moveSpeed; // Tốc độ di chuyển của skeleton
    [SerializeField] private float timerCooldowAttack; // Thời gian hồi chiêu sau khi tấn công

    private RaycastHit2D hit; // Lưu kết quả raycast
    private GameObject target; // Đối tượng player
    private Animator animator; // Animator để điều khiển animation
    private float distance; // Lưu khoảng cách với player
    private bool attackMode; // Trạng thái có đang tấn công không
    private bool inRange; // Kiểm tra player có trong phạm vi không
    private bool cooling; // Kiểm tra có đang hồi chiêu không
    private float intTimer; // Biến gốc để reset cooldown

    private int AttackParam = Animator.StringToHash("Attack"); // Hash tên animation "Attack" để tối ưu

    private void Awake() // Hàm Awake chạy khi object được tạo
    {
        intTimer = timerCooldowAttack; // Lưu lại thời gian cooldown gốc
        animator = GetComponent<Animator>(); // Lấy Animator gắn với skeleton
    }

    void Update() // Hàm Update chạy mỗi frame
    {
        if (inRange) // Nếu player đang trong vùng trigger
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLenght, raycastMask); // Bắn raycast về bên trái
            RaycastDebugger(); // Vẽ raycast để debug trong Scene
        }

        if (hit.collider != null) // Nếu raycast trúng collider
        {
            SkeletonLogic(); // Chạy logic skeleton
        }
        else if (hit.collider == null) // Nếu raycast không trúng gì
        {
            inRange = false; // Player ra khỏi vùng
        }

        if (inRange == false) // Nếu không thấy player
        {
            animator.SetBool("canWalk", false); // Dừng animation đi bộ
            StopAttack(); // Ngừng tấn công
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Khi player vào trigger
    {
        if (collision.CompareTag("Player")) // Nếu object có tag là Player
        {
            target = collision.gameObject; // Gán target là player
            inRange = true; // Đánh dấu đã vào phạm vi
        }
    }

    void SkeletonLogic() // Xử lý logic skeleton
    {
        distance = Vector2.Distance(transform.position, target.transform.position); // Tính khoảng cách với player

        if (distance > attackDistance) // Nếu xa hơn khoảng cách tấn công
        {
            Move(); // Đi tới player
            StopAttack(); // Ngừng tấn công
        }
        else if (attackDistance >= distance && cooling == false) // Nếu player trong tầm và không cooldown
        {
            Attack(); // Thực hiện tấn công
        }

        if (cooling) // Nếu đang hồi chiêu
        {
            Cooldown(); // Giảm timer hồi chiêu
            animator.SetBool(AttackParam, false); // Dừng animation attack
        }
    }

    void Move() // Hàm di chuyển
    {
        animator.SetBool("CanWalk", true); // Bật animation đi bộ
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) // Nếu không ở trạng thái Attack
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y); // Giữ nguyên trục Y
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); // Di chuyển tới player
        }
    }

    void Attack() // Hàm tấn công
    {
        timerCooldowAttack = intTimer; // Reset cooldown
        attackMode = true; // Đang trong trạng thái attack

        animator.SetBool("canWalk", false); // Dừng đi bộ
        animator.SetBool(AttackParam, true); // Bật animation attack
    }

    void Cooldown() // Hàm hồi chiêu
    {
        timerCooldowAttack -= Time.deltaTime; // Giảm thời gian cooldown
        if (timerCooldowAttack <= 0 && cooling && attackMode) // Nếu hết cooldown và vẫn đang attack
        {
            cooling = false; // Reset trạng thái cooldown
            timerCooldowAttack = intTimer; // Reset lại timer
        }
    }

    void StopAttack() // Ngừng tấn công
    {
        cooling = false; // Reset cooldown
        attackMode = false; // Reset trạng thái attack
        animator.SetBool(AttackParam, false); // Tắt animation attack
    }

    void RaycastDebugger() // Vẽ raycast trong Scene view để debug
    {
        if (distance > attackDistance) // Nếu player ngoài tầm đánh
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.red); // Vẽ ray đỏ
        }
        else if (attackDistance > distance) // Nếu player trong tầm đánh
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.green); // Vẽ ray xanh
        }
    }

    public void TriggerCooling() // Hàm được gọi trong animation event sau khi attack
    {
        cooling = true; // Bật trạng thái hồi chiêu
    }
}
